using BryanJonatan_Acceloka.Model;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BryanJonatan_Acceloka.Handlers
{
    public class GetAvailableTicketsHandler : IRequestHandler<GetAvailableTicketsQuery, GetAvailableTicketsResponse>
    {
        private readonly AppDbContext _context;

        public GetAvailableTicketsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GetAvailableTicketsResponse> Handle(GetAvailableTicketsQuery query, CancellationToken cancellationToken)
        {
            var queryable = _context.Tickets.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(query.CategoryName))
            {
                queryable = queryable.Where(t => t.CategoryName.Contains(query.CategoryName));
            }
               
            if (!string.IsNullOrEmpty(query.TicketCode))
            {
                queryable = queryable.Where(t => t.TicketCode.Contains(query.TicketCode));
            }
              
            if (!string.IsNullOrEmpty(query.TicketName))
            {
                queryable = queryable.Where(t => t.TicketName.Contains(query.TicketName));

            }
               
            if (query.Price.HasValue)
            {
                queryable = queryable.Where(t => t.Price <= query.Price.Value);
            }
                
            if (query.EventDateMin.HasValue)
            {
                queryable = queryable.Where(t => t.EventDateMinimum >= query.EventDateMin.Value);
            }
                
            if (query.EventDateMax.HasValue)
            {
                queryable = queryable.Where(t => t.EventDateMaximum <= query.EventDateMax.Value);
            }
                

     
            queryable = (query.OrderState?.ToLower() == "desc")
                ? queryable.OrderByDescending(t => EF.Property<object>(t, query.OrderBy ?? "TicketCode"))
                .ThenByDescending(t => t.EventDateMinimum)
                .ThenByDescending(t => t.Price)
                : queryable.OrderBy(t => EF.Property<object>(t, query.OrderBy ?? "TicketCode"))
                .ThenBy(t => t.EventDateMinimum)
                .ThenBy(t => t.Price);

            var totalRecords = await queryable.CountAsync(cancellationToken);
            var tickets = await queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            var availableTickets = tickets.Select(t => new AvailableTicketResponse
            {
                CategoryName = t.CategoryName,
                TicketCode = t.TicketCode,
                TicketName = t.TicketName,
                Price = t.Price,
                EventDateRange = new EventDateRange
                {
                    Minimum = t.EventDateMinimum,
                    Maximum = t.EventDateMaximum
                },
                AvailableQuota = Math.Max(0, t.Quota - (_context.BookedTickets
                .Where(bt => bt.TicketCode == t.TicketCode)
                .Sum(bt => (int?)bt.Quantity) ?? 0))


            })
            .Where(t => t.AvailableQuota > 0)
            .ToList();

            return new GetAvailableTicketsResponse
            {
                TotalRecords = totalRecords,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                Data = availableTickets
            };
        }
    }


}
