using BryanJonatan_Acceloka.Model;
using FluentValidation;

namespace BryanJonatan_Acceloka.Validators
{
    public class GetBookedTicketQueryValidator : AbstractValidator<GetBookedTicketQuery>
    {
        public GetBookedTicketQueryValidator()
        {
            RuleFor(x => x.BookedTicketId).NotEmpty().WithMessage("Booking ID is required.");
        }
    }
}
