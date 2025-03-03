using MediatR;

namespace BryanJonatan_Acceloka.Model
{
    public record RevokeTicketCommand(string bookedTicketId, string ticketCode, int quantity) : IRequest<Unit>;
}
