CREATE TABLE [dbo].[CustomerAddress] (
    [CustomerID]   INT          NOT NULL,
    [AddressID]    INT          NOT NULL,
    [AddressType]  VARCHAR (10) NOT NULL,
    [ModifiedDate] DATE         NULL, 
    CONSTRAINT [FK_CustomerAddress_Customer] FOREIGN KEY ([CustomerID]) REFERENCES [Customer]([CustomerID]), 
    CONSTRAINT [FK_CustomerAddress_Address] FOREIGN KEY ([AddressID]) REFERENCES [Address]([AddressID])
);

