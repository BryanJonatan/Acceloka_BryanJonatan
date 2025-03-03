using BryanJonatan_Acceloka.Model;
using FluentValidation;

namespace BryanJonatan_Acceloka.Validators
{
    public class GetAvailableTicketsQueryValidator : AbstractValidator<GetAvailableTicketsQuery>
    {
        public GetAvailableTicketsQueryValidator()
        {
            RuleFor(x => x.OrderBy).Must(BeAValidColumn).WithMessage("Invalid OrderBy column.");
            RuleFor(x => x.OrderState).Must(x => x == "asc" || x == "desc" || string.IsNullOrEmpty(x)).WithMessage("OrderState must be 'asc' or 'desc'.");
        }

        private bool BeAValidColumn(string? column)
        {
            var validColumns = new[] { "CategoryName", "TicketCode", "TicketName", "Price", "EventDateMinimum","EventDateMaximum", "RemainingQuota" };
            return string.IsNullOrEmpty(column) || validColumns.Contains(column);
        }
    }
}
