create database Erp_V3

USE Erp_V3
BEGIN TRANSACTION
create table [Category](
    [Id] INT IDENTITY(1,1),
    [Name] varchar(160) not null,
    [IsActive] bit not null,

    CONSTRAINT [PK_Primary_Key_CategoryId] PRIMARY KEY ([Id])
);

GO
CREATE INDEX IX_Category_Index ON [Category] ([Id]) 
GO

create table [Product]
(
    [Id] INT IDENTITY(1,1),
    [Name] varchar(160) not null,
    [Value] decimal(5,2) not null,
    [CategoryId] INT not null,
    [IsActive] bit not null,

    CONSTRAINT [PK_Primary_Key_ProductId] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Foreign_Key_PRoductFk] foreign key ([CategoryId]) REFERENCES [Category]([Id])


);
GO
CREATE INDEX IX_Produt_Index ON [Product]([Id]) 
GO
CREATE INDEX IX_Produt_CategoryId ON [Product]([CategoryId]) 
GO

CREATE TABLE [Buyer](
    [Id] INT IDENTITY(1,1),
    [FirstName] varchar(160) not null,
    [LastName] varchar(160) not null,
    [Email] varchar(160) not null,
    [CellPhone] varchar(13) not null,
    [IsActive] bit not null,

    CONSTRAINT [PK_Buyer_Primary_Key] PRIMARY KEY ([Id]),
    CONSTRAINT [UQ_Buyer_Email] unique ([Email]),
    CONSTRAINT [UQ_Buyer_cellphone] UNIQUE ([Cellphone])
    

);
GO
CREATE INDEX IX_Buyer_Email ON [Buyer](Email)

GO
CREATE INDEX IX_Buyer_ID ON [Buyer](Id)
GO


CREATE INDEX IX_Buyer_Cellphone ON [Buyer](Cellphone)
GO

CREATE TABLE [Order](
    [Id] INT IDENTITY(1,1) NOT NULL,
    [BuyerId] INT NOT NULL,
    [TotalValue] decimal (6,2) not null,
    [IsCompleted] bit not null,

    CONSTRAINT [PK_Primary_Key_Order] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_fOreignKey_Order] FOREIGN KEY ([BuyerId]) REFERENCES [Buyer]([Id])
    
);
GO
CREATE INDEX IX_Order_ID on [Order](Id)
GO
CREATE INDEX IX_Order_BuyerId ON [Order](BuyerId)
GO

CREATE TABLE [Order_Product](
    [OrderId] INT NOT NULL,
    [ProductId] INT NOT NULL,

    CONSTRAINT [PK_Primary_Key] PRIMARY KEY ([OrderId], [ProductId])
);
GO
CREATE INDEX IX_OrderId_Products ON [Order_Product](OrderId)
GO
CREATE INDEX IX_Order_ProductsId ON [Order_Product](ProductId)
GO


CREATE TABLE [User](
    [Id] int identity(1,1),
    [UserName] nvarchar(100) not null,
    [FirstName] varchar(100) not null,
    [LastName] varchar(100) not null,
    [PasswordHash] nvarchar(160) not null,
    [Email] nvarchar(160) not null,
    [CellPhone] nvarchar(13) not null

    CONSTRAINT [PK_Primary_Key_User] PRIMARY KEY ([Id]),
    CONSTRAINT [UQ_Unique_Email_User] UNIQUE ([Email]),
    CONSTRAINT [UQ_Unique_UserName_user] unique ([UserName]),
    CONSTRAINT [UQ_Unique_Cellphone_User] UNIQUE ([CellPhone]),
);
go

CREATE INDEX IX_User_Id ON [User] ([Id]) /*Change sort order as needed*/
GO
CREATE INDEX IX_User_Email ON [User] ([Email]) /*Change sort order as needed*/
GO
CREATE INDEX IX_User_UserName ON [User] ([UserName]) /*Change sort order as needed*/
GO

COMMIT

