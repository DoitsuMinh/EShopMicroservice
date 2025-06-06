-- Create Schemas
CREATE SCHEMA IF NOT EXISTS orders;
CREATE SCHEMA IF NOT EXISTS app;
CREATE SCHEMA IF NOT EXISTS payments;


---
-- orders Schema
---

-- Table: orders.Customers
CREATE TABLE orders.Customers
(
    "Id" UUID NOT NULL,
    "Email" VARCHAR(255) NOT NULL,
    "Name" VARCHAR(200) NULL,
    "WelcomeEmailWasSent" BOOLEAN NOT NULL,
    CONSTRAINT "PK_orders_Customers_Id" PRIMARY KEY ("Id")
);
-- Insert data into orders.Customers
INSERT INTO orders.Customers ("Id", "Email", "Name", "WelcomeEmailWasSent") VALUES ('8A812F08-0647-443B-8FA3-A98C3B9493A7', 'johndoe@mail.com', 'John Doe', TRUE);
INSERT INTO orders.Customers ("Id", "Email", "Name", "WelcomeEmailWasSent") VALUES ('42441057-b6c1-4852-9ea7-1f382f99e4eb', 'janedoe@mail.com', 'Jane Doe', TRUE);

SELECT * FROM orders.Customers;

-- Table: orders.Orders
CREATE TABLE orders.Orders
(
    "Id" UUID NOT NULL,
    "CustomerId" UUID NOT NULL,
    "IsRemoved" BOOLEAN NOT NULL,
    "Value" NUMERIC(18, 2) NOT NULL,
    "Currency" VARCHAR(3) NOT NULL,
    "ValueInEUR" NUMERIC(18, 2) NOT NULL,
    "CurrencyEUR" VARCHAR(3) NOT NULL,
    "StatusId" SMALLINT NOT NULL,
    "OrderDate" TIMESTAMP NOT NULL,
    "OrderChangeDate" TIMESTAMP NULL,
    CONSTRAINT "PK_orders_Orders_Id" PRIMARY KEY ("Id")
);

-- Table: orders.Products
CREATE TABLE orders.Products
(
    "Id" UUID NOT NULL,
    "Name" VARCHAR(200),
    CONSTRAINT "PK_orders_Products_Id" PRIMARY KEY ("Id")
);

-- Insert data into orders.Products
INSERT INTO orders.Products ("Id", "Name") VALUES ('8fad1e5a-d4a2-4688-aa49-e70776940c19', 'Jacket');
INSERT INTO orders.Products ("Id", "Name") VALUES ('9db6e474-ae74-4cf5-a0dc-ba23a42e2566', 'T-shirt');

-- Table: orders.OrderProducts
CREATE TABLE orders.OrderProducts
(
    "OrderId" UUID NOT NULL,
    "ProductId" UUID NOT NULL,
    "Quantity" INTEGER,
    "Value" NUMERIC(18, 2),
    "Currency" VARCHAR(3),
    "ValueInEUR" NUMERIC(18, 2),
    "CurrencyEUR" VARCHAR(3),
    CONSTRAINT "PK_orders_OrderProducts_OrderId_ProductId" PRIMARY KEY ("OrderId", "ProductId")
);

-- Table: orders.ProductPrices
CREATE TABLE orders.ProductPrices
(
    "ProductId" UUID NOT NULL,
    "Value" NUMERIC(18, 2) NOT NULL,
    "Currency" VARCHAR(3) NOT NULL,
    CONSTRAINT "PK_orders_ProductPrices_ProductId_Currency" PRIMARY KEY ("ProductId", "Currency")
);

-- Insert data into orders.ProductPrices
INSERT INTO orders.ProductPrices ("ProductId", "Value", "Currency") VALUES ('8fad1e5a-d4a2-4688-aa49-e70776940c19', 200, 'USD');
INSERT INTO orders.ProductPrices ("ProductId", "Value", "Currency") VALUES ('8fad1e5a-d4a2-4688-aa49-e70776940c19', 180, 'EUR');
INSERT INTO orders.ProductPrices ("ProductId", "Value", "Currency") VALUES ('9db6e474-ae74-4cf5-a0dc-ba23a42e2566', 40, 'USD');
INSERT INTO orders.ProductPrices ("ProductId", "Value", "Currency") VALUES ('9db6e474-ae74-4cf5-a0dc-ba23a42e2566', 35, 'EUR');
INSERT INTO orders.ProductPrices ("ProductId", "Value", "Currency") VALUES ('9db6e474-ae74-4cf5-a0dc-ba23a42e2566', 61.57, 'AUD');
INSERT INTO orders.ProductPrices ("ProductId", "Value", "Currency") VALUES ('8fad1e5a-d4a2-4688-aa49-e70776940c19', 307.91, 'AUD');

SELECT * FROM orders.ProductPrices p
ORDER BY p."ProductId" asc

-- View: orders.v_Orders
CREATE VIEW orders.v_Orders
AS
SELECT
    "Order"."Id",
    "Order"."CustomerId",
    "Order"."Value",
    "Order"."IsRemoved",
    "Order"."Currency"
FROM orders.Orders AS "Order";

-- View: orders.v_OrderProducts
CREATE VIEW orders.v_OrderProducts
AS
SELECT
    "OrderProduct"."OrderId",
    "OrderProduct"."ProductId",
    "OrderProduct"."Quantity",
    "OrderProduct"."Value",
    "OrderProduct"."Currency",
    "Product"."Name"
FROM orders.OrderProducts AS "OrderProduct"
    INNER JOIN orders.Products AS "Product"
        ON "OrderProduct"."ProductId" = "Product"."Id";

-- View: orders.v_Customers
CREATE VIEW orders.v_Customers
AS
SELECT
    "Customer"."Id",
    "Customer"."Email",
    "Customer"."Name",
    "Customer"."WelcomeEmailWasSent"
FROM orders.Customers AS "Customer";

-- View: orders.v_ProductPrices
CREATE VIEW orders.v_ProductPrices
AS
SELECT
    "ProductPrice"."ProductId",
    "ProductPrice"."Value",
    "ProductPrice"."Currency"
FROM orders.ProductPrices AS "ProductPrice";

---
-- app Schema
---

-- Table: app.OutboxMessages
CREATE TABLE app.OutboxMessages
(
    "Id" UUID NOT NULL,
    "OccurredOn" TIMESTAMP NOT NULL,
    "Type" VARCHAR(255) NOT NULL,
    "Data" TEXT NOT NULL,
    "ProcessedDate" TIMESTAMP NULL,
    CONSTRAINT "PK_app_OutboxMessages_Id" PRIMARY KEY ("Id")
);

-- Table: app.InternalCommands
CREATE TABLE app.InternalCommands
(
    "Id" UUID NOT NULL,
    "EnqueueDate" TIMESTAMP NOT NULL,
    "Type" VARCHAR(255) NOT NULL,
    "Data" TEXT NOT NULL,
    "ProcessedDate" TIMESTAMP NULL,
    CONSTRAINT "PK_app_InternalCommands_Id" PRIMARY KEY ("Id")
);

---
-- payments Schema
---

-- Table: payments.Payments
CREATE TABLE payments.Payments
(
    "Id" UUID NOT NULL,
    "CreateDate" TIMESTAMP NOT NULL,
    "StatusId" SMALLINT NOT NULL,
    "OrderId" UUID NOT NULL,
    "EmailNotificationIsSent" BOOLEAN NOT NULL,
    CONSTRAINT "PK_payments_Payments_Id" PRIMARY KEY ("Id")
);
