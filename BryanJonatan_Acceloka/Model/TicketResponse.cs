namespace BryanJonatan_Acceloka.Model
{
    public record TicketResponse(string CategoryName, string TicketCode, string TicketName, DateTime EventDateMinimum, DateTime EventDateMaximum, decimal Price, int RemainingQuota);
}
