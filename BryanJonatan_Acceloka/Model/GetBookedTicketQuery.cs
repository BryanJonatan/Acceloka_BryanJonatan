using MediatR;

namespace BryanJonatan_Acceloka.Model
{
    public record GetBookedTicketQuery(string BookedTicketId) : IRequest<BookedTicketResponse>;
}
