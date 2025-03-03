using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BryanJonatan_Acceloka.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using MediatR;

namespace BryanJonatan_Acceloka.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class TicketsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TicketsController(AppDbContext context)
        {
            _context = context;
        }


        private readonly IMediator _mediator;
        public TicketsController(IMediator mediator) => _mediator = mediator;

        [HttpGet("get-available-ticket")]
        public async Task<ActionResult<List<TicketResponse>>> GetAvailableTickets([FromQuery] GetAvailableTicketsQuery query) => await _mediator.Send(query);

        [HttpPost("book-ticket")]
        public async Task<ActionResult<BookTicketResponse2>> BookTicket([FromBody] BookTicketCommand command) => await _mediator.Send(command);

        [HttpGet("get-booked-ticket/{bookedTicketId}")]
        public async Task<ActionResult<BookedTicketResponse>> GetBookedTicket(string bookedTicketId) => await _mediator.Send(new GetBookedTicketQuery(bookedTicketId));

        [HttpDelete("revoke-ticket/{bookedTicketId}/{ticketCode}/{qty}")]
        public async Task<IActionResult> RevokeTicket(string bookedTicketId, string ticketCode, int qty)
        {
            await _mediator.Send(new RevokeTicketCommand(bookedTicketId, ticketCode, qty));
            return NoContent();
        }

        [HttpPut("edit-booked-ticket/{bookedTicketId}")]
        public async Task<ActionResult<BookedTicketResponse>> EditBookedTicket(string bookedTicketId, [FromBody] List<TicketItem> tickets)
        {
            return await _mediator.Send(new EditBookedTicketCommand(bookedTicketId, tickets));
        }
    }
}