namespace BryanJonatan_Acceloka.Model
{
    public class BookTicketRequest
    {
        public required string TicketCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

}

