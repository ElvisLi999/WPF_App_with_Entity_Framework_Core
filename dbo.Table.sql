CREATE TABLE [dbo].[Customer]
(
	[CustomerID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NameStyle] VARCHAR(100) NULL, 
    [Title] VARCHAR(50) NOT NULL, 
    [FirstName] VARCHAR(100) NOT NULL, 
    [MiddleName] VARCHAR(100) NULL, 
    [LastName] VARCHAR(100) NOT NULL, 
    [CompanyName] VARCHAR(200) NULL, 
    [SalesPerson] VARCHAR(100) NOT NULL, 
    [EmailAddress] VARCHAR(100) NOT NULL, 
    [Phone] VARCHAR(50) NULL, 
    [Password] VARCHAR(100) NOT NULL
)
