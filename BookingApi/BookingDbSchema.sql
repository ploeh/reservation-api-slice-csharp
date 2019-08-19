CREATE TABLE [dbo].[Reservations] (
    [Id]         INT           NOT NULL IDENTITY,
    [Date]       DATETIME2     NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [Email]      NVARCHAR (50) NOT NULL,
    [Quantity]   INT           NOT NULL
    PRIMARY KEY CLUSTERED ([Id] ASC)
)