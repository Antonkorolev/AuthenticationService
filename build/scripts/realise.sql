IF NOT EXISTS(SELECT [Database_id]
              FROM sys.databases
              WHERE [Name] = 'UserDb')
BEGIN
        CREATE DATABASE [UserDb]
END
GO

USE [UserDb]
GO

IF OBJECT_ID(N'dbo.User', N'U') IS NULL
BEGIN
CREATE TABLE [dbo].[User]
(
    [UserId]    INT           NOT NULL PRIMARY KEY IDENTITY,
    [Login]     NVARCHAR(30)  NOT NULL,
    [Password]  NVARCHAR(256) NOT NULL,
    [Salt]      NVARCHAR(256) NOT NULL
    )
END

IF NOT EXISTS (SELECT name FROM [sys].[indexes] WHERE name = 'UQ_User_Login')
    CREATE UNIQUE INDEX [UQ_User_Login] ON [dbo].[User](Login);
GO

IF OBJECT_ID(N'dbo.Settings', N'U') IS NULL
BEGIN
CREATE TABLE [dbo].[Settings]
(
    [SettingId] INT               NOT NULL PRIMARY KEY IDENTITY,
    [Key]       NVARCHAR(256)     NOT NULL,
    [Value]     NVARCHAR(256)     NOT NULL,
    )
END

GO

INSERT INTO [dbo].Settings([Key], [Value]) VALUES ('WorkFactor','12');
INSERT INTO [dbo].Settings([Key], [Value]) VALUES ('BcryptMinorRevision','b');