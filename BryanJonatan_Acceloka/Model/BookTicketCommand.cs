using MediatR;

namespace BryanJonatan_Acceloka.Model
{
    public record BookTicketCommand(List<TicketItem> Tickets) : IRequest<BookTicketResponse2>;
}
