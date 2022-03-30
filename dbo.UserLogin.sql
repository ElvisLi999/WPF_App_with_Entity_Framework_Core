CREATE TABLE [dbo].[UserLogin]
(
	[EmployeeID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] VARCHAR(100) NOT NULL, 
    [LastName] VARCHAR(100) NOT NULL, 
    [Department] VARCHAR(50) NOT NULL, 
    [PhoneNr] VARCHAR(20) NOT NULL, 
    [UserName] VARCHAR(20) NOT NULL, 
    [Password] VARCHAR(20) NOT NULL
)
