namespace BryanJonatan_Acceloka.Model
{
    public class BookedTicketUpdateRequest
    {
        public string TicketCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
