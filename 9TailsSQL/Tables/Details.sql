CREATE TABLE [dbo].[Details]
(
	[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
	[Title] NVARCHAR(MAX),
	[Poster] NVARCHAR(MAX),
	[Overview] NVARCHAR(MAX),
	[OtherNames] NVARCHAR(MAX),
	[Language] NVARCHAR(50),
	[Episodes] NVARCHAR(MAX),
	[Views] NVARCHAR(100),
	[LastAdded] DATETIME2(7),
	[Release] INT,
	[Type] NVARCHAR(255),
	[Status] NVARCHAR(255),
	
	)


