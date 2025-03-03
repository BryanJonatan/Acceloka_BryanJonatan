using BryanJonatan_Acceloka.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BryanJonatan_Acceloka.Handlers
{
    public class BookTicketCommandHandler : IRequestHandler<BookTicketCommand, BookTicketResponse2>
    {
        private readonly AppDbContext _context;
        public BookTicketCommandHandler(AppDbContext context) => _context = context;

        public async Task<BookTicketResponse2> Handle(BookTicketCommand request, CancellationToken cancellationToken)
        {
            var bookedTickets = new List<TicketDetail>();
            decimal totalPrice = 0;

            foreach (var ticket in request.Tickets)
            {
                var dbTicket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketCode == ticket.TicketCode, cancellationToken);
                if (dbTicket == null || dbTicket.Quota < ticket.Quantity)
                {
                    throw new Exception("Invalid ticket code or insufficient quota.");
                }

                dbTicket.Quota -= ticket.Quantity;
                _context.BookedTickets.Add(new BookedTicket { TicketCode = ticket.TicketCode, Quantity = ticket.Quantity });
                bookedTickets.Add(new TicketDetail(ticket.TicketCode, dbTicket.TicketName, dbTicket.Price, ticket.Quantity));
                totalPrice += dbTicket.Price * ticket.Quantity;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return new BookTicketResponse2(bookedTickets, totalPrice);
        }
    }
}
