CREATE TABLE [dbo].[Episodes]
(
	[Id] INT NOT NULL IDENTITY (1, 1) PRIMARY KEY,
	[SeriesId] INT NOT NULL,
	[Episode] INT,
	[Download] NVARCHAR(MAX),
	FOREIGN KEY([SeriesId]) REFERENCES [dbo].[Details](Id),
)

