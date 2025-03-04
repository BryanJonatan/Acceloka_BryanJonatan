using BryanJonatan_Acceloka.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BryanJonatan_Acceloka.Handlers
{
    public class BookTicketHandler : IRequestHandler<BookTicketCommand, BookTicketResponseDto>
    {
        private readonly AppDbContext _context;

        public BookTicketHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BookTicketResponseDto> Handle(BookTicketCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var response = new List<BookTicketResponse>();
                var categoryTotals = new Dictionary<string, int>();
                int totalPriceAllCategories = 0;

                foreach (var bookingRequest in request.BookingRequests)
                {
                    var ticket = await _context.Tickets
                        .AsNoTracking()
                        .FirstOrDefaultAsync(t => t.TicketCode == bookingRequest.TicketCode, cancellationToken);

                    if (ticket == null)
                    {
                        throw new KeyNotFoundException($"Ticket with code {bookingRequest.TicketCode} does not exist.");
                    }

                    var availableTickets = await GetAvailableTicketsAsync(bookingRequest.TicketCode, cancellationToken);
                    var availableTicket = availableTickets.FirstOrDefault(t => t.TicketCode == bookingRequest.TicketCode);

                    if (availableTicket == null || availableTicket.AvailableQuota == null || availableTicket.AvailableQuota <= 0)
                    {
                        throw new InvalidOperationException($"Ticket {bookingRequest.TicketCode} is sold out or unavailable.");
                    }

                    if (bookingRequest.Quantity > availableTicket.AvailableQuota)
                    {
                        throw new InvalidOperationException($"Only {availableTicket.AvailableQuota} tickets available for {bookingRequest.TicketCode}.");
                    }

                    if (ticket.EventDateMinimum <= DateTime.Now)
                    {
                        throw new InvalidOperationException("The event date has already passed.");
                    }

                    var bookingId = Guid.NewGuid().ToString();
                    var bookedTicket = new BookedTicket
                    {
                        BookingId = bookingId,
                        TicketCode = bookingRequest.TicketCode,
                        Quantity = bookingRequest.Quantity
                    };

                    await _context.BookedTickets.AddAsync(bookedTicket, cancellationToken);

                    int ticketTotalPrice = bookingRequest.Quantity * ticket.Price;
                    response.Add(new BookTicketResponse
                    {
                        BookingId = bookingId,
                        TicketName = ticket.TicketName,
                        TicketCode = ticket.TicketCode,
                        Price = ticket.Price,
                        Quantity = bookingRequest.Quantity,
                        TotalPrice = ticketTotalPrice
                    });

                    if (!categoryTotals.ContainsKey(ticket.CategoryName))
                        categoryTotals[ticket.CategoryName] = 0;
                    categoryTotals[ticket.CategoryName] += ticketTotalPrice;
                    totalPriceAllCategories += ticketTotalPrice;
                }

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new BookTicketResponseDto
                {
                    Bookings = response,
                    Summary = new SummaryResponse
                    {
                        CategoryTotals = categoryTotals,
                        GrandTotal = totalPriceAllCategories
                    }
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private async Task<List<AvailableTicketResponse>> GetAvailableTicketsAsync(string ticketCode, CancellationToken cancellationToken)
        {
            return await _context.Tickets
                .Where(t => t.TicketCode == ticketCode)
                .Select(t => new AvailableTicketResponse
                {
                    CategoryName = t.CategoryName,
                    TicketCode = t.TicketCode,
                    TicketName = t.TicketName,
                    Price = t.Price,
                    AvailableQuota = t.Quota - (_context.BookedTickets
                        .Where(bt => bt.TicketCode == t.TicketCode)
                        .Sum(bt => (int?)bt.Quantity) ?? 0)
                })
                .Where(t => t.AvailableQuota > 0)
                .ToListAsync(cancellationToken);
        }
    }

}
