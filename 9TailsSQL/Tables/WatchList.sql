CREATE TABLE [dbo].[WatchList]
(
	[SeriesId] INT NOT NULL,
	[UserId] INT NOT NULL,
	PRIMARY KEY CLUSTERED ([SeriesId] ASC, [UserId] ASC),
	FOREIGN KEY ([SeriesId]) REFERENCES [dbo].[Details] ([Id]) ON DELETE CASCADE,
	FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_WatchList_UserId]
    ON [dbo].[WatchList]([UserId] ASC);
