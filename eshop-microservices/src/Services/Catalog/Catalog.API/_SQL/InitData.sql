USE [CatalogDb]
-- 1. Insert a Catalog
--INSERT INTO [dbo].[catalog] ([Name], [Description])
--VALUES ('Clothing Catalog', 'Seasonal and casual wear');

-- 2. Insert Categories under the catalog (assuming inserted catalog has Id = 1)
--INSERT INTO [dbo].[category] ([CatalogId], [Name], [ImageFile])
--VALUES 
--(1, 'Jackets', ''),
--(1, 'T-Shirts', '');

-- 3. Insert Products under the categories (Jackets = 1, T-Shirts = 2)
INSERT INTO [dbo].[product] ([Name], [Category], [Description], [ImageFile], [Price])
VALUES
('Black Jacket', 1, 'Premium Jackets', 'jacket.jpg', 307.910),
('White T-Shirt', 2, 'Casual T-shirts', 'tshirt.jpg', 61.570);

-- 4. Insert ProductDetails with RefId and ProdetailName
-- Assume product Ids: Jacket Series = 1, T-Shirt Series = 2
INSERT INTO [dbo].[productdetail] ([ProductId], [Sku], [RefId], [Size])
VALUES 
(1, '', '8fad1e5a-d4a2-4688-aa49-e70776940c19', 'L'),
(1, '', '8fad1e5a-d4a2-4688-aa49-e70776940c19', 'XL'),
(2, '', '9db6e474-ae74-4cf5-a0dc-ba23a42e2566', 'S'),
(2, '', '9db6e474-ae74-4cf5-a0dc-ba23a42e2566', 'M');

Update [dbo].[productdetail]
SET Sku = 'P00' + CAST(ProdetailId AS VARCHAR(20));


-- 5. Insert ProductQty for each ProductDetail
Insert into dbo.productqty (ProdetailId, Available, Onhand, OnOrder, OnCustOrder, OnBackOrder, OnReOrder, OnReturn)
select  ProdetailId, 100, 100, 0, 0, 0, 0, 0
from [dbo].[productdetail]
