using BryanJonatan_Acceloka.Model;
using FluentValidation;

namespace BryanJonatan_Acceloka.Validators
{
    public class GetAvailableTicketsValidator : AbstractValidator<GetAvailableTicketsQuery>
    {
        private readonly string[] _validColumns = { "TicketCode", "TicketName", "CategoryName", "Price", "EventDateMinimum" };

        public GetAvailableTicketsValidator()
        {
            RuleFor(x => x.OrderBy)
                .Must(orderBy => string.IsNullOrEmpty(orderBy) || _validColumns.Contains(orderBy))
                .WithMessage($"OrderBy must be one of: {string.Join(", ", _validColumns)}.");

            RuleFor(x => x.OrderState)
                .Must(orderState => orderState?.ToLower() == "asc" || orderState?.ToLower() == "desc")
                .WithMessage("OrderState must be 'asc' or 'desc'.");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("PageNumber must be greater than zero.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");

            RuleFor(x => x.EventDateMax)
                .GreaterThanOrEqualTo(x => x.EventDateMin)
                .When(x => x.EventDateMin.HasValue && x.EventDateMax.HasValue)
                .WithMessage("EventDateMax must be greater than or equal to EventDateMin.");
        }
    }


}
