CREATE TABLE [dbo].[Admin]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserName] NVARCHAR(500) NULL, 
    [PassWord] NVARCHAR(50) NULL, 
    [Name] NVARCHAR(50) NULL, 
    [Phone] NVARCHAR(20) NULL, 
    [Address] NVARCHAR(500) NULL, 
    [Email] NVARCHAR(50) NULL
)
