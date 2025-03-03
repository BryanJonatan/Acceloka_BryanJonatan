namespace BryanJonatan_Acceloka.Model
{
    public record BookedTicketResponse(string BookedTicketId, List<TicketDetail> Tickets);
}
