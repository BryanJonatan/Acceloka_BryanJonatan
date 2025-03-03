using BryanJonatan_Acceloka.Model;
using FluentValidation;

namespace BryanJonatan_Acceloka.Validators
{
    public class EditBookedTicketCommandValidator : AbstractValidator<EditBookedTicketCommand>
    {
        public EditBookedTicketCommandValidator()
        {
            RuleFor(x => x.BookedTicketId).NotEmpty().WithMessage("Booking ID is required.");
            RuleForEach(x => x.Tickets).ChildRules(ticket =>
            {
                ticket.RuleFor(t => t.TicketCode).NotEmpty().WithMessage("Ticket code is required.");
                ticket.RuleFor(t => t.Quantity).GreaterThan(0).WithMessage("Quantity must be at least 1.");
            });
        }
    }
}
