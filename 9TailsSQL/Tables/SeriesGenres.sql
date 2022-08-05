CREATE TABLE [dbo].[SeriesGenres]
(
	[SeriesId] INT NOT NULL,
	[GenreId] INT NOT NULL,
	
	FOREIGN KEY ([SeriesId]) REFERENCES [dbo].[Details] ([Id]) ON DELETE CASCADE,
	FOREIGN KEY ([GenreId]) REFERENCES [dbo].[Genres] ([Id]) ON DELETE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_Series_Genres_SeriesId]
    ON [dbo].[SeriesGenres]([SeriesId] ASC);