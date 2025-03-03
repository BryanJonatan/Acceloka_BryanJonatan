using MediatR;

namespace BryanJonatan_Acceloka.Model
{
    public record EditBookedTicketCommand(string BookedTicketId, List<TicketItem> Tickets) : IRequest<BookedTicketResponse>;
}
