using BryanJonatan_Acceloka.Model;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BryanJonatan_Acceloka.Validators
{
    public class BookTicketRequestValidator : AbstractValidator<BookTicketRequest>
    {
        private readonly AppDbContext _context;

        public BookTicketRequestValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.TicketCode)
                .NotEmpty().WithMessage("Ticket Code is required.")
                .MustAsync(TicketExists).WithMessage("Ticket Code does not exist.")
                .MustAsync(TicketHasQuota).WithMessage("Ticket is sold out.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .MustAsync(QuantityNotExceedQuota).WithMessage("Booking quantity exceeds available quota.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.TicketCode)
                        .MustAsync(EventDateIsValid).WithMessage("The event date has already passed.");
                });
        }

        private async Task<bool> TicketExists(string ticketCode, CancellationToken cancellationToken)
        {
            return await _context.Tickets.AnyAsync(t => t.TicketCode == ticketCode, cancellationToken);
        }

        private async Task<bool> TicketHasQuota(string ticketCode, CancellationToken cancellationToken)
        {
            var ticket = await _context.Tickets
                .Where(t => t.TicketCode == ticketCode)
                .Select(t => new { t.Quota, BookedQuantity = _context.BookedTickets.Where(bt => bt.TicketCode == t.TicketCode).Sum(bt => (int?)bt.Quantity) ?? 0 })
                .FirstOrDefaultAsync(cancellationToken);

            return ticket != null && (ticket.Quota - ticket.BookedQuantity) > 0;
        }

        private async Task<bool> QuantityNotExceedQuota(BookTicketRequest request, int quantity, CancellationToken cancellationToken)
        {
            var ticket = await _context.Tickets
                .Where(t => t.TicketCode == request.TicketCode)
                .Select(t => new { t.Quota, BookedQuantity = _context.BookedTickets.Where(bt => bt.TicketCode == t.TicketCode).Sum(bt => (int?)bt.Quantity) ?? 0 })
                .FirstOrDefaultAsync(cancellationToken);

            return ticket != null && quantity <= (ticket.Quota - ticket.BookedQuantity);
        }

        private async Task<bool> EventDateIsValid(string ticketCode, CancellationToken cancellationToken)
        {
            var ticket = await _context.Tickets
                .Where(t => t.TicketCode == ticketCode)
                .Select(t => t.EventDateMinimum)
                .FirstOrDefaultAsync(cancellationToken);

            return ticket > DateTime.Now;
        }
    }

}
