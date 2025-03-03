using BryanJonatan_Acceloka.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BryanJonatan_Acceloka.Handlers
{
    public class RevokeTicketCommandHandler 
    {
        private readonly AppDbContext _context;
        public RevokeTicketCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> RevokeTicket(string bookedTicketId, string ticketCode, int qty)
        {
            var bookedTicket = await _context.BookedTickets
                .Include(bt => bt.Ticket)
                .FirstOrDefaultAsync(bt => bt.BookingId == bookedTicketId && bt.TicketCode == ticketCode);

            if (bookedTicket == null)
            {
                return NotFound(new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7807",
                    Title = "Booked Ticket Not Found",
                    Status = (int)HttpStatusCode.NotFound,
                    Detail = $"Booked Ticket with ID {bookedTicketId} and Ticket Code {ticketCode} does not exist."
                });
            }

            if (qty > bookedTicket.Quantity)
            {
                return BadRequest(new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7807",
                    Title = "Invalid Quantity",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = $"The requested quantity exceeds the booked quantity for ticket {ticketCode}."
                });
            }

            bookedTicket.Quantity -= qty;
            if (bookedTicket.Quantity <= 0)
            {
                _context.BookedTickets.Remove(bookedTicket);
            }

            await _context.SaveChangesAsync();

            var response = new
            {
                KodeTicket = bookedTicket.TicketCode,
                NamaTicket = bookedTicket.Ticket?.TicketName,
                NamaKategori = bookedTicket.Ticket?.CategoryName,
                SisaQuantity = bookedTicket.Quantity
            };

            return Ok(response);
        }

    }
}
