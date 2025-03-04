using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BryanJonatan_Acceloka.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using MediatR;
using FluentValidation;

namespace BryanJonatan_Acceloka.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<GetAvailableTicketsQuery> _availableTicketsValidator;
        private readonly IValidator<GetBookedTicketQuery> _bookedTicketValidator;

        public TicketsController(
            IMediator mediator,
            IValidator<GetAvailableTicketsQuery> availableTicketsValidator,
            IValidator<GetBookedTicketQuery> bookedTicketValidator)
        {
            _mediator = mediator;
            _availableTicketsValidator = availableTicketsValidator;
            _bookedTicketValidator = bookedTicketValidator;
        }

        [HttpGet("get-available-ticket")]
        public async Task<IActionResult> GetAvailableTickets([FromQuery] GetAvailableTicketsQuery query)
        {
            var validationResult = await _availableTicketsValidator.ValidateAsync(query);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Status = 400,
                    Detail = "Invalid input parameters.",
                    Extensions = { { "errors", validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) } }
                });
            }

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("book-ticket")]
        public async Task<IActionResult> BookTicket([FromBody] BookTicketCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Status = 400,
                    Detail = "One or more validation errors occurred.",
                    Extensions = { { "errors", ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) } }
                });
            }
        }

        [HttpGet("get-booked-ticket/{bookedTicketId}")]
        public async Task<IActionResult> GetBookedTicket(string bookedTicketId)
        {
            var query = new GetBookedTicketQuery { BookedTicketId = bookedTicketId };

            var validationResult = await _bookedTicketValidator.ValidateAsync(query);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Status = 400,
                    Detail = "Invalid input parameters.",
                    Extensions = { { "errors", validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) } }
                });
            }

            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [HttpDelete("revoke-ticket/{bookedTicketId}/{ticketCode}/{qty}")]
        public async Task<IActionResult> RevokeTicket(
            [FromRoute] string bookedTicketId,
            [FromRoute] string ticketCode,
            [FromRoute] int qty,
            [FromServices] IValidator<RevokeTicketCommand> validator)
        {
            var command = new RevokeTicketCommand
            {
                BookedTicketId = bookedTicketId,
                TicketCode = ticketCode,
                Qty = qty
            };

            var validationResult = await validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Status = 400,
                    Detail = "Invalid input parameters.",
                    Extensions = { { "errors", validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) } }
                });
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPut("edit-booked-ticket/{bookedTicketId}")]
        public async Task<IActionResult> EditBookedTicket(
     string bookedTicketId,
     [FromBody] List<BookedTicketUpdateRequest> updates,
     [FromServices] IValidator<EditBookedTicketCommand> validator)
        {
            var command = new EditBookedTicketCommand
            {
                BookedTicketId = bookedTicketId,
                Updates = updates
            };

            var validationResult = await validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Status = 400,
                    Detail = "Invalid input parameters.",
                    Extensions = { { "errors", validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) } }
                });
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}