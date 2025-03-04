namespace BryanJonatan_Acceloka.Model
{
    public class AvailableTicketResponse
    {
        public string CategoryName { get; set; }
        public string TicketCode { get; set; }
        public string TicketName { get; set; }
        public EventDateRange EventDateRange { get; set; }
        public int Price { get; set; }
        public int AvailableQuota { get; set; }
    }
}
