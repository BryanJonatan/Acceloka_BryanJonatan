using BryanJonatan_Acceloka.Model;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BryanJonatan_Acceloka.Validators
{
    public class EditBookedTicketValidator : AbstractValidator<EditBookedTicketCommand>
    {
        private readonly AppDbContext _context;

        public EditBookedTicketValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.BookedTicketId)
                .NotEmpty().WithMessage("Booked Ticket ID is required.")
                .MustAsync(async (id, _) => await _context.BookedTickets.AnyAsync(bt => bt.BookingId == id))
                .WithMessage("Booked Ticket does not exist.");

            RuleForEach(x => x.Updates).SetValidator(new BookedTicketUpdateValidator(_context));
        }
    }
}
