
CREATE TABLE [dbo].[catalog] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [Description] nvarchar(500) NULL,
    [Status] nvarchar(20) NOT NULL DEFAULT 'Active',
    [CreatedDate] datetime NOT NULL DEFAULT GETDATE(),
    [UpdatedDate] datetime NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_Catalog] PRIMARY KEY ([Id])
);

-- Create Category table (owned by Catalog)
CREATE TABLE [dbo].[category] (
    [CategoryId] int IDENTITY(1,1) NOT NULL,
    [CatalogId] int NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [ImageFile] nvarchar(200) NULL,
    [Status] nvarchar(20) NOT NULL DEFAULT 'Active',
    [CreatedDate] datetime NOT NULL DEFAULT GETDATE(),
    [UpdatedDate] datetime NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_Category] PRIMARY KEY ([CategoryId]),
    CONSTRAINT [FK_Category_Catalogs_CatalogId] FOREIGN KEY ([CatalogId]) REFERENCES [dbo].[catalog]([Id])
);

-- Create products table (lowercase as per configuration)
CREATE TABLE [dbo].[product] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [Category] int NOT NULL,
    [Description] nvarchar(500) NULL,
    [ImageFile] nvarchar(200) NULL,
    [Price] decimal(18,3) NOT NULL,
	[Status] nvarchar(20) NOT NULL DEFAULT 'Active',
    [CreatedDate] datetime NOT NULL DEFAULT GETDATE(),
    [UpdatedDate] datetime NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_product] PRIMARY KEY ([Id])
);

-- Create ProductDetail table (owned by Product)
CREATE TABLE [dbo].[productdetail] (
    [ProdetailId] bigint IDENTITY(1,1) NOT NULL,
    [ProductId] bigint NOT NULL,
    [Sku] nvarchar(50) NOT NULL,
    [RefId] uniqueidentifier NULL,
    [Size] nvarchar(20) NULL,
    [Status] nvarchar(20) NOT NULL DEFAULT 'Active',
    [CreatedDate] datetime NOT NULL DEFAULT GETDATE(),
    [UpdatedDate] datetime NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_ProductDetail] PRIMARY KEY ([ProdetailId]),
    CONSTRAINT [FK_ProductDetail_products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[product]([Id]),
);

-- Create ProductQty table (owned by ProductDetail)
CREATE TABLE [dbo].[productqty] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [ProdetailId] bigint NOT NULL,
    [Available] int NOT NULL DEFAULT 0,
    [Onhand] int NOT NULL DEFAULT 0,
    [OnOrder] int NOT NULL DEFAULT 0,
    [OnCustOrder] int NOT NULL DEFAULT 0,
    [OnBackOrder] int NOT NULL DEFAULT 0,
    [OnReOrder] int NOT NULL DEFAULT 0,
    [OnReturn] int NOT NULL DEFAULT 0,
    [CreatedDate] datetime NOT NULL DEFAULT GETDATE(),
    [UpdatedDate] datetime NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_ProductQty] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProductQty_ProductDetail_ProdetailId] FOREIGN KEY ([ProdetailId]) REFERENCES [dbo].[productdetail]([ProdetailId]),
    CONSTRAINT [UK_ProductQty_ProdetailId] UNIQUE ([ProdetailId])
);