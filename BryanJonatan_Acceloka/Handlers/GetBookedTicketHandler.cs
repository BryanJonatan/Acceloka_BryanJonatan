using BryanJonatan_Acceloka.Exceptions;
using BryanJonatan_Acceloka.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BryanJonatan_Acceloka.Handlers
{
    public class GetBookedTicketHandler : IRequestHandler<GetBookedTicketQuery, GetBookedTicketResponse>
    {
        private readonly AppDbContext _context;

        public GetBookedTicketHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GetBookedTicketResponse> Handle(GetBookedTicketQuery request, CancellationToken cancellationToken)
        {
            var bookedTicket = await _context.BookedTickets
                .Include(bt => bt.Ticket)
                .FirstOrDefaultAsync(bt => bt.BookingId == request.BookedTicketId, cancellationToken);

            if (bookedTicket == null)
            {
                throw new NotFoundException($"Booked Ticket with ID {request.BookedTicketId} does not exist.");
            }

            if (bookedTicket.Ticket == null)
            {
                throw new NotFoundException($"No ticket found for booking {request.BookedTicketId}.");
            }

            return new GetBookedTicketResponse
            {
                KodeTiket = bookedTicket.TicketCode,
                NamaTiket = bookedTicket.Ticket.TicketName,
                TanggalEvent = new EventDateRange
                {
                    Minimum = bookedTicket.Ticket.EventDateMinimum,
                    Maximum = bookedTicket.Ticket.EventDateMaximum
                },
                Quantity = bookedTicket.Quantity,
                Kategori = bookedTicket.Ticket.CategoryName
            };
        }
    }

}
