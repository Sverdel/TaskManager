CREATE TABLE [dbo].[Priorities] (
    [Id]   TINYINT        IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_dbo.Priorities] PRIMARY KEY CLUSTERED ([Id] ASC)
);

