using BryanJonatan_Acceloka.Model;
using FluentValidation;

namespace BryanJonatan_Acceloka.Validators
{
    public class RevokeTicketValidator : AbstractValidator<RevokeTicketCommand>
    {
        public RevokeTicketValidator()
        {
            RuleFor(x => x.BookedTicketId)
                .NotEmpty().WithMessage("Booked Ticket ID is required.");

            RuleFor(x => x.TicketCode)
                .NotEmpty().WithMessage("Ticket Code is required.");

            RuleFor(x => x.Qty)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }

}
