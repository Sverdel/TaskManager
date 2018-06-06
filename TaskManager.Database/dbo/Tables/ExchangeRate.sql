CREATE TABLE [dbo].[ExchangeRate]
(
    [Id]   INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [Date] datetime not null default getdate(),
    [Currency] SMALLINT not null,
    [Rate] FLOAT not null
)
