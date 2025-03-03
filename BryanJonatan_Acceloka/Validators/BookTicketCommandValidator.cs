using BryanJonatan_Acceloka.Model;
using FluentValidation;

namespace BryanJonatan_Acceloka.Validators
{
    public class BookTicketCommandValidator : AbstractValidator<BookTicketCommand>
    {
        public BookTicketCommandValidator()
        {
            RuleForEach(x => x.Tickets).ChildRules(ticket =>
            {
                ticket.RuleFor(t => t.TicketCode).NotEmpty().WithMessage("Ticket code is required.");
                ticket.RuleFor(t => t.Quantity).GreaterThan(0).WithMessage("Quantity must be at least 1.");
            });
        }
    }
}
