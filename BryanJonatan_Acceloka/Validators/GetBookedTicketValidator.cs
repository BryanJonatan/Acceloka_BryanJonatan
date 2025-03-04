using BryanJonatan_Acceloka.Model;
using FluentValidation;

namespace BryanJonatan_Acceloka.Validators
{
    public class GetBookedTicketValidator : AbstractValidator<GetBookedTicketQuery>
    {
        public GetBookedTicketValidator()
        {
            RuleFor(x => x.BookedTicketId)
                .NotEmpty().WithMessage("BookedTicketId is required.")
                .Matches("^[a-zA-Z0-9-]+$").WithMessage("Invalid format for BookedTicketId.");
        }
    }

}
