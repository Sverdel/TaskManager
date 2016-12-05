CREATE TABLE [dbo].[WorkTasks] (
    [Id]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActualTimeCost]    FLOAT (53)     NOT NULL,
    [ChangeDatetime]    DATETIME2 (7)  NOT NULL,
    [CreateDateTime]    DATETIME2 (7)  NOT NULL,
    [Description]       NVARCHAR (MAX) NULL,
    [Name]              NVARCHAR (200) NOT NULL,
    [PlanedTimeCost]    FLOAT (53)     NOT NULL,
    [PriorityId]        INT            NOT NULL,
    [RemainingTimeCost] FLOAT (53)     NOT NULL,
    [StateId]           INT            NOT NULL,
    [UserId]            NVARCHAR (450) NULL,
    [Version]           INT            NOT NULL,
    CONSTRAINT [PK_WorkTasks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WorkTasks_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_WorkTasks_Priorities_PriorityId] FOREIGN KEY ([PriorityId]) REFERENCES [dbo].[Priorities] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_WorkTasks_States_StateId] FOREIGN KEY ([StateId]) REFERENCES [dbo].[States] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_WorkTasks_PriorityId]
    ON [dbo].[WorkTasks]([PriorityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WorkTasks_StateId]
    ON [dbo].[WorkTasks]([StateId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WorkTasks_UserId]
    ON [dbo].[WorkTasks]([UserId] ASC);

