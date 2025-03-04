using BryanJonatan_Acceloka.Model;
using FluentValidation;

namespace BryanJonatan_Acceloka.Validators
{
    public class BookTicketValidator : AbstractValidator<BookTicketCommand>
    {
        private readonly AppDbContext _context;

        public BookTicketValidator(AppDbContext context)
        {
            _context = context;

            RuleForEach(x => x.BookingRequests).SetValidator(new BookTicketRequestValidator(_context));
        }
    }

}
