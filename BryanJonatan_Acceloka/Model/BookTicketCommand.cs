using MediatR;

namespace BryanJonatan_Acceloka.Model
{
    public class BookTicketCommand : IRequest<BookTicketResponseDto>
    {
        public List<BookTicketRequest> BookingRequests { get; set; } = new();
    }

}
