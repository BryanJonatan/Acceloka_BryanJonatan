namespace BryanJonatan_Acceloka.Model
{
    public class GetAvailableTicketsResponse
    {
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<AvailableTicketResponse> Data { get; set; } = new();
    }
}
