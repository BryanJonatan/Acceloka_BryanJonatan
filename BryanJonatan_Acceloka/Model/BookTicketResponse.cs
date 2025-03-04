namespace BryanJonatan_Acceloka.Model
{
    public class BookTicketResponse
    {
        public required string BookingId {  get; set; } = string.Empty;
        public required string TicketName { get; set; } = string.Empty;
        public required string TicketCode { get; set; } = string.Empty;
        public int Price { get; set; }  
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }

     
    }
}
