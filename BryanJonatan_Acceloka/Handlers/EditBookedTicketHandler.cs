using BryanJonatan_Acceloka.Model;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BryanJonatan_Acceloka.Handlers
{
    public class EditBookedTicketHandler : IRequestHandler<EditBookedTicketCommand, EditBookedTicketResponse>
    {
        private readonly AppDbContext _context;

        public EditBookedTicketHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EditBookedTicketResponse> Handle(EditBookedTicketCommand request, CancellationToken cancellationToken)
        {
            var bookedTicket = await _context.BookedTickets
                .Include(bt => bt.Ticket)
                .FirstOrDefaultAsync(bt => bt.BookingId == request.BookedTicketId, cancellationToken);

            if (bookedTicket == null)
            {
                throw new ValidationException("Booked Ticket not found.");
            }

            foreach (var update in request.Updates)
            {
                var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketCode == update.TicketCode, cancellationToken);
                if (ticket == null)
                {
                    throw new ValidationException($"Ticket with code {update.TicketCode} does not exist.");
                }

                var totalBookedQuantity = await _context.BookedTickets
                    .Where(bt => bt.TicketCode == update.TicketCode)
                    .SumAsync(bt => bt.Quantity, cancellationToken);

                if (update.Quantity > (ticket.Quota - totalBookedQuantity + bookedTicket.Quantity))
                {
                    throw new ValidationException($"The requested quantity exceeds the available quota for ticket {update.TicketCode}.");
                }

                bookedTicket.Quantity = update.Quantity;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new EditBookedTicketResponse
            {
                KodeTicket = bookedTicket.TicketCode,
                NamaTicket = bookedTicket.Ticket?.TicketName,
                NamaKategori = bookedTicket.Ticket?.CategoryName,
                SisaQuantity = bookedTicket.Ticket?.Quota - await _context.BookedTickets
                    .Where(bt => bt.TicketCode == bookedTicket.TicketCode)
                    .SumAsync(bt => bt.Quantity, cancellationToken) ?? 0
            };
        }
    }

}
