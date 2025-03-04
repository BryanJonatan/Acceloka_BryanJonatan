using MediatR;

namespace BryanJonatan_Acceloka.Model
{
    public class GetBookedTicketQuery : IRequest<GetBookedTicketResponse>
    {
        public string BookedTicketId { get; set; } 
    }

}