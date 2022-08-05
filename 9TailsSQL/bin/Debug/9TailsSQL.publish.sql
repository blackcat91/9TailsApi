﻿/*
Deployment script for 9TailsSQL

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "9TailsSQL"
:setvar DefaultFilePrefix "9TailsSQL"
:setvar DefaultDataPath "C:\Users\brand\AppData\Local\Microsoft\VisualStudio\SSDT\"
:setvar DefaultLogPath "C:\Users\brand\AppData\Local\Microsoft\VisualStudio\SSDT\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                CURSOR_DEFAULT LOCAL 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET PAGE_VERIFY NONE,
                DISABLE_BROKER 
            WITH ROLLBACK IMMEDIATE;
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET TARGET_RECOVERY_TIME = 0 SECONDS 
    WITH ROLLBACK IMMEDIATE;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET QUERY_STORE (QUERY_CAPTURE_MODE = ALL, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 367), MAX_STORAGE_SIZE_MB = 100) 
            WITH ROLLBACK IMMEDIATE;
    END


GO
PRINT N'Creating Table [dbo].[ArchivedUsers]...';


GO
CREATE TABLE [dbo].[ArchivedUsers] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (255) NOT NULL,
    [Email]    NVARCHAR (255) NOT NULL,
    [Avatar]   NVARCHAR (MAX) NULL,
    [Joined]   DATETIME2 (7)  NULL,
    [Archived] DATETIME2 (7)  NULL,
    [Password] NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC),
    UNIQUE NONCLUSTERED ([UserName] ASC)
);


GO
PRINT N'Creating Table [dbo].[Details]...';


GO
CREATE TABLE [dbo].[Details] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Title]      NVARCHAR (MAX) NULL,
    [Poster]     NVARCHAR (MAX) NULL,
    [Overview]   NVARCHAR (MAX) NULL,
    [OtherNames] NVARCHAR (MAX) NULL,
    [Language]   NVARCHAR (50)  NULL,
    [Episodes]   NVARCHAR (MAX) NULL,
    [Views]      NVARCHAR (100) NULL,
    [LastAdded]  DATETIME2 (7)  NULL,
    [Release]    INT            NULL,
    [Type]       NVARCHAR (255) NULL,
    [Status]     VARCHAR (255)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [dbo].[Episodes]...';


GO
CREATE TABLE [dbo].[Episodes] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [SeriesId] INT            NOT NULL,
    [Episode]  INT            NULL,
    [Download] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [dbo].[Genres]...';


GO
CREATE TABLE [dbo].[Genres] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [dbo].[Links]...';


GO
CREATE TABLE [dbo].[Links] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [SeriesId] INT           NOT NULL,
    [Source]   NVARCHAR (50) NULL,
    [Link]     NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [dbo].[SeriesGenres]...';


GO
CREATE TABLE [dbo].[SeriesGenres] (
    [SeriesId] INT NOT NULL,
    [GenreId]  INT NOT NULL,
    PRIMARY KEY CLUSTERED ([SeriesId] ASC, [GenreId] ASC)
);


GO
PRINT N'Creating Index [dbo].[SeriesGenres].[IX_Series_Genres_SeriesId]...';


GO
CREATE NONCLUSTERED INDEX [IX_Series_Genres_SeriesId]
    ON [dbo].[SeriesGenres]([SeriesId] ASC);


GO
PRINT N'Creating Table [dbo].[Users]...';


GO
CREATE TABLE [dbo].[Users] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (255) NOT NULL,
    [Email]    NVARCHAR (255) NOT NULL,
    [Avatar]   NVARCHAR (MAX) NULL,
    [Joined]   DATETIME2 (7)  NULL,
    [Password] NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC),
    UNIQUE NONCLUSTERED ([UserName] ASC)
);


GO
PRINT N'Creating Table [dbo].[WatchList]...';


GO
CREATE TABLE [dbo].[WatchList] (
    [SeriesId] INT NOT NULL,
    [UserId]   INT NOT NULL,
    PRIMARY KEY CLUSTERED ([SeriesId] ASC, [UserId] ASC)
);


GO
PRINT N'Creating Index [dbo].[WatchList].[IX_WatchList_UserId]...';


GO
CREATE NONCLUSTERED INDEX [IX_WatchList_UserId]
    ON [dbo].[WatchList]([UserId] ASC);


GO
PRINT N'Creating Foreign Key unnamed constraint on [dbo].[Episodes]...';


GO
ALTER TABLE [dbo].[Episodes] WITH NOCHECK
    ADD FOREIGN KEY ([SeriesId]) REFERENCES [dbo].[Details] ([Id]);


GO
PRINT N'Creating Foreign Key unnamed constraint on [dbo].[Links]...';


GO
ALTER TABLE [dbo].[Links] WITH NOCHECK
    ADD FOREIGN KEY ([SeriesId]) REFERENCES [dbo].[Details] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating Foreign Key unnamed constraint on [dbo].[SeriesGenres]...';


GO
ALTER TABLE [dbo].[SeriesGenres] WITH NOCHECK
    ADD FOREIGN KEY ([SeriesId]) REFERENCES [dbo].[Details] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating Foreign Key unnamed constraint on [dbo].[SeriesGenres]...';


GO
ALTER TABLE [dbo].[SeriesGenres] WITH NOCHECK
    ADD FOREIGN KEY ([GenreId]) REFERENCES [dbo].[Genres] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating Foreign Key unnamed constraint on [dbo].[WatchList]...';


GO
ALTER TABLE [dbo].[WatchList] WITH NOCHECK
    ADD FOREIGN KEY ([SeriesId]) REFERENCES [dbo].[Details] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating Foreign Key unnamed constraint on [dbo].[WatchList]...';


GO
ALTER TABLE [dbo].[WatchList] WITH NOCHECK
    ADD FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
CREATE TABLE [#__checkStatus] (
    id           INT            IDENTITY (1, 1) PRIMARY KEY CLUSTERED,
    [Schema]     NVARCHAR (256),
    [Table]      NVARCHAR (256),
    [Constraint] NVARCHAR (256)
);

SET NOCOUNT ON;

DECLARE tableconstraintnames CURSOR LOCAL FORWARD_ONLY
    FOR SELECT SCHEMA_NAME([schema_id]),
               OBJECT_NAME([parent_object_id]),
               [name],
               0
        FROM   [sys].[objects]
        WHERE  [parent_object_id] IN (OBJECT_ID(N'dbo.Episodes'), OBJECT_ID(N'dbo.Links'), OBJECT_ID(N'dbo.SeriesGenres'), OBJECT_ID(N'dbo.WatchList'))
               AND [type] IN (N'F', N'C')
                   AND [object_id] IN (SELECT [object_id]
                                       FROM   [sys].[check_constraints]
                                       WHERE  [is_not_trusted] <> 0
                                              AND [is_disabled] = 0
                                       UNION
                                       SELECT [object_id]
                                       FROM   [sys].[foreign_keys]
                                       WHERE  [is_not_trusted] <> 0
                                              AND [is_disabled] = 0);

DECLARE @schemaname AS NVARCHAR (256);

DECLARE @tablename AS NVARCHAR (256);

DECLARE @checkname AS NVARCHAR (256);

DECLARE @is_not_trusted AS INT;

DECLARE @statement AS NVARCHAR (1024);

BEGIN TRY
    OPEN tableconstraintnames;
    FETCH tableconstraintnames INTO @schemaname, @tablename, @checkname, @is_not_trusted;
    WHILE @@fetch_status = 0
        BEGIN
            PRINT N'Checking constraint: ' + @checkname + N' [' + @schemaname + N'].[' + @tablename + N']';
            SET @statement = N'ALTER TABLE [' + @schemaname + N'].[' + @tablename + N'] WITH ' + CASE @is_not_trusted WHEN 0 THEN N'CHECK' ELSE N'NOCHECK' END + N' CHECK CONSTRAINT [' + @checkname + N']';
            BEGIN TRY
                EXECUTE [sp_executesql] @statement;
            END TRY
            BEGIN CATCH
                INSERT  [#__checkStatus] ([Schema], [Table], [Constraint])
                VALUES                  (@schemaname, @tablename, @checkname);
            END CATCH
            FETCH tableconstraintnames INTO @schemaname, @tablename, @checkname, @is_not_trusted;
        END
END TRY
BEGIN CATCH
    PRINT ERROR_MESSAGE();
END CATCH

IF CURSOR_STATUS(N'LOCAL', N'tableconstraintnames') >= 0
    CLOSE tableconstraintnames;

IF CURSOR_STATUS(N'LOCAL', N'tableconstraintnames') = -1
    DEALLOCATE tableconstraintnames;

SELECT N'Constraint verification failed:' + [Schema] + N'.' + [Table] + N',' + [Constraint]
FROM   [#__checkStatus];

IF @@ROWCOUNT > 0
    BEGIN
        DROP TABLE [#__checkStatus];
        RAISERROR (N'An error occurred while verifying constraints', 16, 127);
    END

SET NOCOUNT OFF;

DROP TABLE [#__checkStatus];


GO
PRINT N'Update complete.';


GO