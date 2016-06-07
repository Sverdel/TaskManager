CREATE TABLE [dbo].[States] (
    [Id]   TINYINT       IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_dbo.States] PRIMARY KEY CLUSTERED ([Id] ASC)
);

