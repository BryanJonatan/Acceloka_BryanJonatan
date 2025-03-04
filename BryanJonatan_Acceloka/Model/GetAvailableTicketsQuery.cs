using MediatR;

namespace BryanJonatan_Acceloka.Model
{
    public class GetAvailableTicketsQuery : IRequest<GetAvailableTicketsResponse>
    {
        public string? CategoryName { get; set; }
        public string? TicketCode { get; set; }
        public string? TicketName { get; set; }
        public int? Price { get; set; }
        public DateTime? EventDateMin { get; set; }
        public DateTime? EventDateMax { get; set; }
        public string OrderBy { get; set; } = "TicketCode";
        public string OrderState { get; set; } = "asc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
