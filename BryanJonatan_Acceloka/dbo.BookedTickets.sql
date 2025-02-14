CREATE TABLE [dbo].[BookedTickets] (
    [BookingId]  VARCHAR (255) NOT NULL,
    [TicketCode] VARCHAR (255) NOT NULL,
    [Quantity]   INT           DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([BookingId] ASC),
    FOREIGN KEY ([TicketCode]) REFERENCES [dbo].[Tickets] ([TicketCode])
);

