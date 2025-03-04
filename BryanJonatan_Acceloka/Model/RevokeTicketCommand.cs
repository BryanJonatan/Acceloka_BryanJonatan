using MediatR;

namespace BryanJonatan_Acceloka.Model
{
    public class RevokeTicketCommand : IRequest<RevokeTicketResponse>
    {
        public string BookedTicketId { get; set; }
        public string TicketCode { get; set; }
        public int Qty { get; set; }
    }

}
