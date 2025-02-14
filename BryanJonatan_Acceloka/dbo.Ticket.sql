CREATE TABLE [dbo].[Ticket] (
    [TicketCode]   VARCHAR (255) NOT NULL,
    [TicketName]   VARCHAR (255) NOT NULL,
    [CategoryName] VARCHAR (255) NOT NULL,
    [Quota]        INT           NOT NULL,
    [EventDate]    DATETIME      NOT NULL,
    [Price]        INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([ticketCode] ASC)
);

