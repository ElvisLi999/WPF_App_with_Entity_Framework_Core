CREATE TABLE [dbo].[Address]
(
	[AddressId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AddressLine1] VARCHAR(200) NOT NULL, 
    [AddressLine2] VARCHAR(200) NULL, 
    [City] VARCHAR(30) NOT NULL, 
    [StateProvince] VARCHAR(30) NOT NULL, 
    [CountryRegion] VARCHAR(30) NOT NULL, 
    [PostalCode] VARCHAR(15) NOT NULL
)
