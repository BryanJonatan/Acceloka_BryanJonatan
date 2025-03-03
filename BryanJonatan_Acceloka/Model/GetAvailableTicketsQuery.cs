using MediatR;

namespace BryanJonatan_Acceloka.Model
{
    public record GetAvailableTicketsQuery(string? CategoryName, string? TicketCode, string? TicketName, decimal? Price, DateTime? MinEventDate, DateTime? MaxEventDate, string? OrderBy, string? OrderState) : IRequest<List<TicketResponse>>;
  
}
