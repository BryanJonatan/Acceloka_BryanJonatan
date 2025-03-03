using BryanJonatan_Acceloka.Model;
using FluentValidation;

namespace BryanJonatan_Acceloka.Validators
{
    public class RevokeTicketCommandValidator : AbstractValidator<RevokeTicketCommand>
    {
        public RevokeTicketCommandValidator()
        {
            RuleFor(x => x.bookedTicketId).NotEmpty().WithMessage("Booking ID is required.");
            RuleFor(x => x.ticketCode).NotEmpty().WithMessage("Ticket code is required.");
            RuleFor(x => x.quantity).GreaterThan(0).WithMessage("Quantity must be at least 1.");
        }
    }
}
