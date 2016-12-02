CREATE TABLE [dbo].[WorkTasks] (
    [Id]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (200)  NOT NULL,
    [Description]       NVARCHAR (4000) NULL,
    [CreateDateTime]    DATETIME        NOT NULL,
    [ChangeDatetime]    DATETIME        NOT NULL,
    [Version]           INT             NOT NULL,
    [PlanedTimeCost]    FLOAT (53)      NOT NULL,
    [ActualTimeCost]    FLOAT (53)      NOT NULL,
    [RemainingTimeCost] FLOAT (53)      NOT NULL,
    [UserId]            INT             NOT NULL,
    [StateId]           INT         NOT NULL,
    [PriorityId]        INT         NOT NULL,
    CONSTRAINT [PK_dbo.WorkTasks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.WorkTasks_dbo.Priorities_PriorityId] FOREIGN KEY ([PriorityId]) REFERENCES [dbo].[Priorities] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.WorkTasks_dbo.States_StateId] FOREIGN KEY ([StateId]) REFERENCES [dbo].[States] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.WorkTasks_dbo.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[WorkTasks]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StateId]
    ON [dbo].[WorkTasks]([StateId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PriorityId]
    ON [dbo].[WorkTasks]([PriorityId] ASC);

