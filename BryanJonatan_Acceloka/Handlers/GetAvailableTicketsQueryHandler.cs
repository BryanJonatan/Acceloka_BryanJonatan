using BryanJonatan_Acceloka.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BryanJonatan_Acceloka.Handlers
{
    public class GetAvailableTicketsQueryHandler : IRequestHandler<GetAvailableTicketsQuery, List<TicketResponse>>
    {
        private readonly AppDbContext _context;
        public GetAvailableTicketsQueryHandler(AppDbContext context) => _context = context;

        public async Task<List<TicketResponse>> Handle(GetAvailableTicketsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Tickets.AsQueryable();
            if (!string.IsNullOrEmpty(request.CategoryName))
            {
                query = query.Where(t => t.CategoryName.Contains(request.CategoryName));
            }
            if (!string.IsNullOrEmpty(request.TicketCode))
            {
                query = query.Where(t => t.TicketCode.Contains(request.TicketCode));
            }
            if (!string.IsNullOrEmpty(request.TicketName))
            {
                query = query.Where(t => t.TicketName.Contains(request.TicketName));
            }
            if (request.Price.HasValue)
            {
                query = query.Where(t => t.Price <= request.Price);
            }
            if (request.MinEventDate.HasValue)
            {
                query = query.Where(t => t.EventDateMinimum >= request.MinEventDate);
            }
            if (request.MaxEventDate.HasValue)
            {
                query = query.Where(t => t.EventDateMaximum <= request.MaxEventDate);
            }
            query = request.OrderState == "desc"
                ? query.OrderByDescending(t => EF.Property<object>(t, request.OrderBy ?? "TicketCode"))
                : query.OrderBy(t => EF.Property<object>(t, request.OrderBy ?? "TicketCode"));
            return await query.Select(t => new TicketResponse(t.CategoryName, t.TicketCode, t.TicketName, t.EventDateMinimum,t.EventDateMaximum, t.Price, t.Quota)).ToListAsync(cancellationToken);
        }
    }
}
