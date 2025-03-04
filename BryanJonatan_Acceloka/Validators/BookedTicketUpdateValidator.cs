using BryanJonatan_Acceloka.Model;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BryanJonatan_Acceloka.Validators
{
    public class BookedTicketUpdateValidator : AbstractValidator<BookedTicketUpdateRequest>
    {
        private readonly AppDbContext _context;

        public BookedTicketUpdateValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.TicketCode)
                .NotEmpty().WithMessage("Ticket code is required.")
                .MustAsync(async (code, _) => await _context.Tickets.AnyAsync(t => t.TicketCode == code))
                .WithMessage("Ticket does not exist.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1.");
        }
    }
}
