using BryanJonatan_Acceloka.Exceptions;
using BryanJonatan_Acceloka.Model;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BryanJonatan_Acceloka.Handlers
{
    public class RevokeTicketHandler : IRequestHandler<RevokeTicketCommand, RevokeTicketResponse>
    {
        private readonly AppDbContext _context;

        public RevokeTicketHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RevokeTicketResponse> Handle(RevokeTicketCommand request, CancellationToken cancellationToken)
        {
            var bookedTicket = await _context.BookedTickets
                .Include(bt => bt.Ticket)
                .FirstOrDefaultAsync(bt => bt.BookingId == request.BookedTicketId && bt.TicketCode == request.TicketCode, cancellationToken);

            if (bookedTicket == null)
            {
                throw new NotFoundException($"Booked Ticket with ID {request.BookedTicketId} and Ticket Code {request.TicketCode} does not exist.");
            }

            if (request.Qty > bookedTicket.Quantity)
            {
                throw new ValidationException(new[]
                {
                new ValidationFailure(nameof(request.Qty), $"The requested quantity exceeds the booked quantity for ticket {request.TicketCode}.")
            });
            }

            bookedTicket.Quantity -= request.Qty;
            if (bookedTicket.Quantity <= 0)
            {
                _context.BookedTickets.Remove(bookedTicket);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new RevokeTicketResponse
            {
                KodeTicket = bookedTicket.TicketCode,
                NamaTicket = bookedTicket.Ticket?.TicketName,
                NamaKategori = bookedTicket.Ticket?.CategoryName,
                SisaQuantity = bookedTicket.Quantity
            };
        }
    }

}
