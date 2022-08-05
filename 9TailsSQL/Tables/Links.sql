GO

CREATE TABLE [dbo].[Links]
(
	[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
	[SeriesId] INT NOT NULL,
	[Episode] INT NOT NULL,
	[Source] NVARCHAR(50),
	[Link] NVARCHAR(MAX),
	
	
);

