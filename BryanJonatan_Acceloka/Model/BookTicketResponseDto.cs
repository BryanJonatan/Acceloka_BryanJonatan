namespace BryanJonatan_Acceloka.Model
{
    public class BookTicketResponseDto
    {
        public List<BookTicketResponse> Bookings { get; set; } = new();
        public SummaryResponse Summary { get; set; } = new();
    }
}
