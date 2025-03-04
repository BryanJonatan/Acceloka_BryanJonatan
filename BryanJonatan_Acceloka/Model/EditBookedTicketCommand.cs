using MediatR;

namespace BryanJonatan_Acceloka.Model
{
    public class EditBookedTicketCommand : IRequest<EditBookedTicketResponse>
    {
        public string BookedTicketId { get; set; } = string.Empty;
        public List<BookedTicketUpdateRequest> Updates { get; set; } = new();
    }
}
