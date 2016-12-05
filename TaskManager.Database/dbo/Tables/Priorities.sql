CREATE TABLE [dbo].[Priorities] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Priorities] PRIMARY KEY CLUSTERED ([Id] ASC)
);

