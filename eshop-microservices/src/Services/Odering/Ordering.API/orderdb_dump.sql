--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4
-- Dumped by pg_dump version 17.4

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: app; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA app;


--
-- Name: orders; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA orders;


--
-- Name: payments; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA payments;


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: internalcommands; Type: TABLE; Schema: app; Owner: -
--

CREATE TABLE app.internalcommands (
    "Id" uuid NOT NULL,
    "EnqueueDate" timestamp without time zone NOT NULL,
    "Type" character varying(255) NOT NULL,
    "Data" text NOT NULL,
    "ProcessedDate" timestamp without time zone
);


--
-- Name: outboxmessages; Type: TABLE; Schema: app; Owner: -
--

CREATE TABLE app.outboxmessages (
    "Id" uuid NOT NULL,
    "OccurredOn" timestamp without time zone NOT NULL,
    "Type" character varying(255) NOT NULL,
    "Data" text NOT NULL,
    "ProcessedDate" timestamp without time zone
);


--
-- Name: customers; Type: TABLE; Schema: orders; Owner: -
--

CREATE TABLE orders.customers (
    "Id" uuid NOT NULL,
    "Email" character varying(255) NOT NULL,
    "Name" character varying(200),
    "WelcomeEmailWasSent" boolean NOT NULL
);


--
-- Name: orderproducts; Type: TABLE; Schema: orders; Owner: -
--

CREATE TABLE orders.orderproducts (
    "OrderId" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "Quantity" integer NOT NULL,
    "Value" numeric(18,3) NOT NULL,
    "Currency" character varying(3) NOT NULL,
    "ValueInAUD" numeric(18,3),
    "CurrencyAUD" character varying(3)
);


--
-- Name: orders; Type: TABLE; Schema: orders; Owner: -
--

CREATE TABLE orders.orders (
    "Id" uuid NOT NULL,
    "CustomerId" uuid NOT NULL,
    "IsRemoved" boolean NOT NULL,
    "Value" numeric(18,3) NOT NULL,
    "Currency" character varying(3) NOT NULL,
    "StatusId" smallint NOT NULL,
    "OrderDate" timestamp without time zone NOT NULL,
    "OrderChangeDate" timestamp without time zone,
    "ValueInAUD" numeric(18,3),
    "CurrencyAUD" character varying(3)
);


--
-- Name: productprices; Type: TABLE; Schema: orders; Owner: -
--

CREATE TABLE orders.productprices (
    "ProductId" uuid NOT NULL,
    "Value" numeric(18,3) NOT NULL,
    "Currency" character varying(3) NOT NULL
);


--
-- Name: products; Type: TABLE; Schema: orders; Owner: -
--

CREATE TABLE orders.products (
    "Id" uuid NOT NULL,
    "Name" character varying(200)
);


--
-- Name: v_customers; Type: VIEW; Schema: orders; Owner: -
--

CREATE VIEW orders.v_customers AS
 SELECT "Id",
    "Email",
    "Name",
    "WelcomeEmailWasSent"
   FROM orders.customers "Customer";


--
-- Name: v_orderproducts; Type: VIEW; Schema: orders; Owner: -
--

CREATE VIEW orders.v_orderproducts AS
 SELECT "OrderProduct"."OrderId",
    "OrderProduct"."ProductId",
    "OrderProduct"."Quantity",
    "OrderProduct"."Value",
    "OrderProduct"."Currency",
    "Product"."Name"
   FROM (orders.orderproducts "OrderProduct"
     JOIN orders.products "Product" ON (("OrderProduct"."ProductId" = "Product"."Id")));


--
-- Name: v_orders; Type: VIEW; Schema: orders; Owner: -
--

CREATE VIEW orders.v_orders AS
 SELECT "Id",
    "CustomerId",
    "Value",
    "IsRemoved",
    "Currency"
   FROM orders.orders "Order";


--
-- Name: v_productprices; Type: VIEW; Schema: orders; Owner: -
--

CREATE VIEW orders.v_productprices AS
 SELECT "ProductId",
    "Value",
    "Currency"
   FROM orders.productprices "ProductPrice";


--
-- Name: payments; Type: TABLE; Schema: payments; Owner: -
--

CREATE TABLE payments.payments (
    "Id" uuid NOT NULL,
    "CreateDate" timestamp without time zone NOT NULL,
    "StatusId" smallint NOT NULL,
    "OrderId" uuid NOT NULL,
    "EmailNotificationIsSent" boolean NOT NULL
);


--
-- Data for Name: internalcommands; Type: TABLE DATA; Schema: app; Owner: -
--

COPY app.internalcommands ("Id", "EnqueueDate", "Type", "Data", "ProcessedDate") FROM stdin;
cb471135-365f-40b4-9ba2-efae51f24a27	2025-07-28 15:10:16.299924	Ordering.Application.Payments.SendEmailAfterPayment.SendEmailAfterPaymentCommand	{"PaymentId":{"Value":"8410ac35-79f2-4be6-95d7-dcbb3e514eed"},"Id":"cb471135-365f-40b4-9ba2-efae51f24a27"}	2025-07-28 16:04:38.213746
81c80e01-b34c-46a9-90dc-e7d3fc9b83c0	2025-07-29 08:56:10.863312	Ordering.Application.Payments.SendEmailAfterPayment.SendEmailAfterPaymentCommand	{"PaymentId":{"Value":"417b517c-c6ff-4f42-acaa-297c58c24801"},"Id":"81c80e01-b34c-46a9-90dc-e7d3fc9b83c0"}	2025-07-29 09:16:47.813657
e1ccba6f-d7fa-439c-886d-aaf217dbc5c9	2025-07-30 15:14:39.49529	Ordering.Application.Payments.SendEmailAfterPayment.SendEmailAfterPaymentCommand	{"PaymentId":{"Value":"3e96dc6b-5939-4424-ae2a-7177fbdb47a1"},"Id":"e1ccba6f-d7fa-439c-886d-aaf217dbc5c9"}	2025-07-30 15:14:45.06691
4d34e21c-556a-45c7-b2f4-f2056505cd45	2025-07-29 09:18:13.370392	Ordering.Application.Payments.SendEmailAfterPayment.SendEmailAfterPaymentCommand	{"PaymentId":{"Value":"7c8bc052-719a-4212-b959-326e3be2dd9b"},"Id":"4d34e21c-556a-45c7-b2f4-f2056505cd45"}	2025-07-29 11:38:24.347763
c124d06b-f3e8-4c2c-9c3d-c409f28a50ee	2025-07-29 11:39:30.304691	Ordering.Application.Payments.SendEmailAfterPayment.SendEmailAfterPaymentCommand	{"PaymentId":{"Value":"c17adb96-571c-48b6-889f-982171bcfe52"},"Id":"c124d06b-f3e8-4c2c-9c3d-c409f28a50ee"}	2025-07-29 11:46:39.190108
fb4bae76-7a37-4baa-940a-6531070aa968	2025-07-30 14:51:47.88749	Ordering.Application.Payments.SendEmailAfterPayment.SendEmailAfterPaymentCommand	{"PaymentId":{"Value":"84313bbc-25b4-43db-a034-e5bf91b1f084"},"Id":"fb4bae76-7a37-4baa-940a-6531070aa968"}	2025-07-30 16:08:15.809989
a7de261e-bcc8-48f9-8d6f-616aa6d9cae0	2025-07-29 11:49:37.391982	Ordering.Application.Payments.SendEmailAfterPayment.SendEmailAfterPaymentCommand	{"PaymentId":{"Value":"b5b81265-b010-4939-9e8d-2e9c8836778d"},"Id":"a7de261e-bcc8-48f9-8d6f-616aa6d9cae0"}	2025-07-30 10:31:22.972209
756a5178-b120-46e2-8978-c3b71dc0e604	2025-09-26 10:23:03.210907	Ordering.Application.Payments.SendEmailAfterPayment.SendEmailAfterPaymentCommand	{"PaymentId":{"Value":"c8cb6a52-5387-4d7d-828c-58338e5fbbd1"},"Id":"756a5178-b120-46e2-8978-c3b71dc0e604"}	2025-09-26 10:58:10.603992
485304af-0ac5-4e64-924b-c82350e8722a	2025-09-26 10:24:30.138515	Ordering.Application.Payments.SendEmailAfterPayment.SendEmailAfterPaymentCommand	{"PaymentId":{"Value":"b11fbcd2-b2e6-4ecd-ab59-27deedcbab87"},"Id":"485304af-0ac5-4e64-924b-c82350e8722a"}	2025-09-26 10:58:10.720278
14cd5c80-ccb3-4569-b4c5-2b71e9ad2252	2025-07-30 10:51:03.760364	Ordering.Application.Payments.SendEmailAfterPayment.SendEmailAfterPaymentCommand	{"PaymentId":{"Value":"ff3d9b00-459f-49e5-af14-3658f768ba3d"},"Id":"14cd5c80-ccb3-4569-b4c5-2b71e9ad2252"}	2025-07-30 14:41:15.763269
5fb72a53-f0d6-4635-90de-d6f3552c1ca3	2025-07-30 14:47:54.3821	Ordering.Application.Payments.SendEmailAfterPayment.SendEmailAfterPaymentCommand	{"PaymentId":{"Value":"af2575a1-40dd-4b7e-adc8-af6798a479c6"},"Id":"5fb72a53-f0d6-4635-90de-d6f3552c1ca3"}	2025-07-30 14:48:00.069719
9d326a0c-2fcb-44b7-b1db-373f5aab8e3f	2025-09-26 10:42:45.329048	Ordering.Application.Payments.SendEmailAfterPayment.SendEmailAfterPaymentCommand	{"PaymentId":{"Value":"4eda7436-ee51-4808-88c3-4c3c3eec44dc"},"Id":"9d326a0c-2fcb-44b7-b1db-373f5aab8e3f"}	2025-09-26 10:58:10.725826
9ce10bdf-cafb-442e-af91-3b4528f19038	2025-09-26 11:03:45.025482	Ordering.Application.Payments.SendEmailAfterPayment.SendEmailAfterPaymentCommand	{"PaymentId":{"Value":"3c001177-960f-496e-a860-e55bafeeb000"},"Id":"9ce10bdf-cafb-442e-af91-3b4528f19038"}	2025-09-26 11:04:47.993778
6b0eb6d4-6ab9-476a-b3f3-265bc9790ce8	2025-09-29 13:54:17.039516	Ordering.Application.Payments.SendEmailAfterPayment.SendEmailAfterPaymentCommand	{"PaymentId":{"Value":"8bb581ab-4fb9-4a09-980f-9407b5a6f74a"},"Id":"6b0eb6d4-6ab9-476a-b3f3-265bc9790ce8"}	2025-09-29 14:13:45.794337
98adbfe5-07d4-4852-a076-0d956e34765c	2025-09-29 14:14:31.431352	Ordering.Application.Payments.SendEmailAfterPayment.SendEmailAfterPaymentCommand	{"PaymentId":{"Value":"0ee8a4c3-9aa7-4ac4-be18-c3309219187f"},"Id":"98adbfe5-07d4-4852-a076-0d956e34765c"}	2025-10-07 14:57:30.967202
7638308f-6004-4cf5-8e45-e66d21e987f3	2025-10-07 14:58:19.509915	Ordering.Application.Payments.SendEmailAfterPayment.SendEmailAfterPaymentCommand	{"PaymentId":{"Value":"d966c04a-76ec-48f0-b651-32557c4743fb"},"Id":"7638308f-6004-4cf5-8e45-e66d21e987f3"}	2025-10-07 14:58:30.016512
\.


--
-- Data for Name: outboxmessages; Type: TABLE DATA; Schema: app; Owner: -
--

COPY app.outboxmessages ("Id", "OccurredOn", "Type", "Data", "ProcessedDate") FROM stdin;
108356c9-d90b-4949-aa73-fcf45bbf22fb	2025-07-28 14:55:05.896057	Ordering.Application.Payments.PaymentCreatedNotification	{"PaymentId":{"Value":"8410ac35-79f2-4be6-95d7-dcbb3e514eed"},"Id":"83d5bb0b-d893-47d5-8193-57df28a68d55"}	2025-07-28 15:10:18.588813
11b10ed6-925e-4fe2-9dd7-7611bce0322c	2025-07-28 14:55:03.018322	Ordering.Application.Orders.PlaceCustomerOrders.OrderPlacedNotification	{"OrderId":{"Value":"5cdfe838-4212-4fd8-a8ae-bb3874456629"},"CustomerId":{"Value":"1ff70690-3722-4548-b216-6402fdd012e4"},"Id":"b4acdddf-f524-4a10-97a6-b2ccf45f5dd5"}	2025-07-28 15:10:41.954273
27dad0e0-3373-459f-b7cb-a5150bca5ae3	2025-07-29 08:17:10.988951	Ordering.Application.Orders.PlaceCustomerOrders.OrderPlacedNotification	{"OrderId":{"Value":"b9ce519c-9243-4cfc-89df-bda4a50e05ad"},"CustomerId":{"Value":"1ff70690-3722-4548-b216-6402fdd012e4"},"Id":"ff5cce71-b6f6-434b-892b-634ad4f92219"}	2025-07-29 08:18:40.466352
1d5b4d59-33fd-48c1-a4f4-1b6d4e385522	2025-07-28 14:39:41.572745	Ordering.Application.Orders.PlaceCustomerOrders.OrderPlacedNotification	{"OrderId":{"Value":"16e26ea5-8b24-4735-9063-db67bcecc0d9"},"CustomerId":{"Value":"1ff70690-3722-4548-b216-6402fdd012e4"},"Id":"e269c90a-00ed-40cf-a8c0-c23963d9f877"}	2025-07-28 14:41:31.550963
2f9b929d-0b39-4cf2-9227-574d3dcfedbb	2025-07-28 14:39:43.044123	Ordering.Application.Payments.PaymentCreatedNotification	{"PaymentId":{"Value":"229fb28e-6fa8-4947-a50c-89d6b7e5d6f0"},"Id":"8c8f2777-bd56-48bb-9c11-d36e7315de4a"}	2025-07-28 14:46:42.586203
a4264361-7da3-4cc1-87da-07ce9e9df712	2025-07-29 08:17:14.8291	Ordering.Application.Payments.PaymentCreatedNotification	{"PaymentId":{"Value":"53a31343-500f-4275-ac84-43f3202a5fed"},"Id":"6028ac62-eb7e-4f43-a8ac-dbbf4435ad62"}	2025-07-29 08:19:05.179181
17173692-ca91-4b9c-8c40-590b4913a314	2025-07-29 08:49:04.145233	Ordering.Application.Payments.PaymentCreatedNotification	{"PaymentId":{"Value":"417b517c-c6ff-4f42-acaa-297c58c24801"},"Id":"739d0f96-437e-45a9-b1e1-779f8463931a"}	2025-07-29 08:56:15.883314
beca3269-2ad8-4692-80da-07b991322a99	2025-07-29 08:47:35.397752	Ordering.Application.Orders.PlaceCustomerOrders.OrderPlacedNotification	{"OrderId":{"Value":"7b68b0ee-4edc-4599-be2e-7d7eafb0d4d1"},"CustomerId":{"Value":"1ff70690-3722-4548-b216-6402fdd012e4"},"Id":"85551d32-e623-494e-8884-8da09d6058d1"}	2025-07-29 08:56:34.889313
ba0f685c-232c-4694-9839-4ab64383a23a	2025-07-29 09:17:22.135627	Ordering.Application.Payments.PaymentCreatedNotification	{"PaymentId":{"Value":"7c8bc052-719a-4212-b959-326e3be2dd9b"},"Id":"2c36d061-455b-4189-a1e0-ab72f4ce216d"}	2025-07-29 09:18:28.184217
dc263f71-f512-445e-a29d-b8402fbf3075	2025-07-29 09:17:18.2048	Ordering.Application.Orders.PlaceCustomerOrders.OrderPlacedNotification	{"OrderId":{"Value":"e09d1014-0686-45d3-b652-99e8ac29675a"},"CustomerId":{"Value":"1ff70690-3722-4548-b216-6402fdd012e4"},"Id":"fb122aea-99f8-440b-8048-92b5ef456924"}	2025-07-29 09:18:48.739222
79f564e2-915d-4cc4-8d36-10fe17061a11	2025-07-29 11:38:50.572655	Ordering.Application.Orders.PlaceCustomerOrders.OrderPlacedNotification	{"OrderId":{"Value":"063802e7-066d-4914-afa0-22b2c4270a88"},"CustomerId":{"Value":"1ff70690-3722-4548-b216-6402fdd012e4"},"Id":"38293b7d-b888-447a-bee2-be7ed5c1f523"}	2025-07-29 11:39:20.37983
7c0451c3-7733-433a-9c76-0eeb100e9e55	2025-07-29 11:38:53.165498	Ordering.Application.Payments.PaymentCreatedNotification	{"PaymentId":{"Value":"c17adb96-571c-48b6-889f-982171bcfe52"},"Id":"10a95a85-397c-4275-a15f-2221fb4a64dc"}	2025-07-29 11:39:32.051845
431fb6b8-22d8-452f-a7f7-c42c9f33a5aa	2025-07-29 11:49:25.709939	Ordering.Application.Payments.PaymentCreatedNotification	{"PaymentId":{"Value":"b5b81265-b010-4939-9e8d-2e9c8836778d"},"Id":"1b9ed5e8-732e-4c6f-87e1-baf4a22ebf80"}	2025-07-29 11:49:37.411557
cd598691-c8e3-45a9-8823-92facc4cf0b3	2025-07-29 11:49:21.628039	Ordering.Application.Orders.PlaceCustomerOrders.OrderPlacedNotification	{"OrderId":{"Value":"1766ddcf-1ae2-450d-8b86-5b23e9938972"},"CustomerId":{"Value":"1ff70690-3722-4548-b216-6402fdd012e4"},"Id":"549a48c3-f206-4816-9a56-ff5acaffcce7"}	2025-07-29 11:49:40.238213
390f39a4-4692-4b58-8aab-d25df14c73fd	2025-07-30 10:49:47.46187	Ordering.Application.Orders.PlaceCustomerOrders.OrderPlacedNotification	{"OrderId":{"Value":"5f79c667-0be0-413f-bcaa-09fa3438bf64"},"CustomerId":{"Value":"1ff70690-3722-4548-b216-6402fdd012e4"},"Id":"9c87a051-47df-4bb0-8eec-6cf392766e61"}	2025-07-30 10:50:56.201996
a2910b50-df53-43f3-8da9-772e10bd83a2	2025-07-30 10:49:51.46514	Ordering.Application.Payments.PaymentCreatedNotification	{"PaymentId":{"Value":"ff3d9b00-459f-49e5-af14-3658f768ba3d"},"Id":"5576abdd-f8bd-4cb6-ae29-2c8fadbe018b"}	2025-07-30 10:51:12.600413
86fca789-b5bc-4fcf-88ad-49f4d34afd1a	2025-07-30 14:47:42.810439	Ordering.Application.Orders.PlaceCustomerOrders.OrderPlacedNotification	{"OrderId":{"Value":"badf9f27-a39d-4786-98d3-79a0dc204dfe"},"CustomerId":{"Value":"1ff70690-3722-4548-b216-6402fdd012e4"},"Id":"4b76b990-4057-44b9-a258-cd8b3a1a5691"}	2025-07-30 14:47:52.649984
c861660d-679b-464b-aeed-8562efef0bdb	2025-07-30 14:47:50.254543	Ordering.Application.Payments.PaymentCreatedNotification	{"PaymentId":{"Value":"af2575a1-40dd-4b7e-adc8-af6798a479c6"},"Id":"62d93adb-7ad1-48fa-b511-e8c5069d3196"}	2025-07-30 14:47:54.390951
49b6372d-7b8a-4da2-a1ba-402b8948f11f	2025-07-30 14:51:37.390283	Ordering.Application.Payments.PaymentCreatedNotification	{"PaymentId":{"Value":"84313bbc-25b4-43db-a034-e5bf91b1f084"},"Id":"dad9e55c-22da-4171-9678-8ed5988ca63d"}	2025-07-30 14:51:47.907444
c36dc536-175d-4d44-a214-aacfe09c8516	2025-07-30 14:51:37.365604	Ordering.Application.Orders.PlaceCustomerOrders.OrderPlacedNotification	{"OrderId":{"Value":"e74e0a01-58e0-41cc-a0d1-45ebd2823acd"},"CustomerId":{"Value":"1ff70690-3722-4548-b216-6402fdd012e4"},"Id":"ef165a42-f766-4acc-a9b8-abef3888e704"}	2025-07-30 14:51:49.548076
013be141-086c-412c-a07f-f874966b96b3	2025-07-30 15:14:16.102929	Ordering.Application.Payments.PaymentCreatedNotification	{"PaymentId":{"Value":"3e96dc6b-5939-4424-ae2a-7177fbdb47a1"},"Id":"84407242-f940-4a20-99b6-726ffc087c53"}	2025-07-30 15:14:39.506924
a8fe2899-048f-4750-86b0-63eafe1e8362	2025-07-30 15:14:16.08118	Ordering.Application.Orders.PlaceCustomerOrders.OrderPlacedNotification	{"OrderId":{"Value":"c310449e-06dd-44dc-a72d-00733b77b1fb"},"CustomerId":{"Value":"1ff70690-3722-4548-b216-6402fdd012e4"},"Id":"3c29a2a4-99ed-4580-8e5d-117ee880ece3"}	2025-07-30 15:14:40.648904
5621377c-9e7d-4871-adab-ca7c18027c38	2025-09-26 10:39:24.940493	Ordering.Application.Payments.PaymentCreatedNotification	{"PaymentId":{"Value":"4eda7436-ee51-4808-88c3-4c3c3eec44dc"},"Id":"51a8cdeb-0ef2-4d57-8705-7cacce5f4a85"}	2025-09-26 10:42:45.753526
355c1d35-9793-4461-b433-4439374f6fc0	2025-09-26 10:39:24.912882	Ordering.Application.Orders.PlaceCustomerOrders.OrderPlacedNotification	{"OrderId":{"Value":"ab0e6614-9e79-425e-95a6-39e9a867fe5e"},"CustomerId":{"Value":"1ff70690-3722-4548-b216-6402fdd012e4"},"Id":"1b000fcc-9600-43b9-8476-bfa2e20a93a5"}	2025-09-26 10:58:10.303182
c47f0723-c4b3-48e3-8f22-cb495d4c50c4	2025-09-26 11:03:41.531962	Ordering.Application.Payments.PaymentCreatedNotification	{"PaymentId":{"Value":"3c001177-960f-496e-a860-e55bafeeb000"},"Id":"85b724a5-178b-450d-8ff5-5872df97e40d"}	2025-09-26 11:03:45.035385
c56ed88a-5755-449f-bf01-52d96621a620	2025-09-26 11:03:41.494792	Ordering.Application.Orders.PlaceCustomerOrders.OrderPlacedNotification	{"OrderId":{"Value":"52211ee7-1bf9-456b-9349-433353e6c0c8"},"CustomerId":{"Value":"1ff70690-3722-4548-b216-6402fdd012e4"},"Id":"9f7eff80-8155-403d-bdf1-980961adb6b6"}	2025-09-26 11:04:47.80114
caa030eb-e767-4535-87e3-0691102a4e2c	2025-09-29 13:53:56.935716	Ordering.Application.Orders.PlaceCustomerOrders.OrderPlacedNotification	{"OrderId":{"Value":"7c7baf75-7197-4dba-8bb8-90bdca81213a"},"CustomerId":{"Value":"1a0b92a8-e27e-409c-972f-3b4034d00a0b"},"Id":"7acd740e-4d72-4213-8bcf-7f11fc8475cb"}	2025-09-29 13:54:17.032765
e42da682-983c-4b26-b5d9-0d414617cbb4	2025-09-29 13:54:01.53857	Ordering.Application.Payments.PaymentCreatedNotification	{"PaymentId":{"Value":"8bb581ab-4fb9-4a09-980f-9407b5a6f74a"},"Id":"5f732636-5186-4af2-af52-0be0ca1588da"}	2025-09-29 13:54:17.042595
0bd1b740-c424-4ad6-a1ec-2e0ed7238758	2025-09-29 14:14:18.683603	Ordering.Application.Orders.PlaceCustomerOrders.OrderPlacedNotification	{"OrderId":{"Value":"2c95ee0d-3362-4012-9ac0-64a40ed89afc"},"CustomerId":{"Value":"1a0b92a8-e27e-409c-972f-3b4034d00a0b"},"Id":"10b3202c-fbb9-4520-996a-e61cc851826f"}	2025-09-29 14:14:31.421251
f1743461-3eb7-4466-98a4-a3ea0be4533a	2025-09-29 14:14:19.477924	Ordering.Application.Payments.PaymentCreatedNotification	{"PaymentId":{"Value":"0ee8a4c3-9aa7-4ac4-be18-c3309219187f"},"Id":"f22fdacf-0483-4d54-b765-80f4ad6c77c3"}	2025-09-29 14:14:31.434569
635db238-38b9-4978-bca4-14e8d4181e51	2025-10-07 14:58:00.689945	Ordering.Application.Orders.PlaceCustomerOrders.OrderPlacedNotification	{"OrderId":{"Value":"b41c6822-3a62-4f53-b8b2-bffd8448844f"},"CustomerId":{"Value":"1ff70690-3722-4548-b216-6402fdd012e4"},"Id":"ca1ac572-2a22-4f0f-88c9-2fd800bcaa58"}	2025-10-07 14:58:19.501786
cbb30dcb-c762-4f16-9ce1-b0ee2b6116fc	2025-10-07 14:58:02.718347	Ordering.Application.Payments.PaymentCreatedNotification	{"PaymentId":{"Value":"d966c04a-76ec-48f0-b651-32557c4743fb"},"Id":"7441212a-850e-4b1e-8061-3b865ec0679b"}	2025-10-07 14:58:19.512629
\.


--
-- Data for Name: customers; Type: TABLE DATA; Schema: orders; Owner: -
--

COPY orders.customers ("Id", "Email", "Name", "WelcomeEmailWasSent") FROM stdin;
8a812f08-0647-443b-8fa3-a98c3b9493a7	johndoe@mail.com	John Doe	t
42441057-b6c1-4852-9ea7-1f382f99e4eb	janedoe@mail.com	Jane Doe	t
1ff70690-3722-4548-b216-6402fdd012e4	jv@mail.com	Jv	f
d0c78526-d46f-4c39-a8b4-032db2ded1c8	jv1@mail.com	Jv	f
0c5a23bd-8764-4305-b085-004f659b81d6	jv2@mail.com	Jv	f
e0912f2b-f00f-46d7-b79a-e7a0413c7f8a	jv3@mail.com	Jv	f
1a0b92a8-e27e-409c-972f-3b4034d00a0b	mixg@test.cc	MIXG VU	f
\.


--
-- Data for Name: orderproducts; Type: TABLE DATA; Schema: orders; Owner: -
--

COPY orders.orderproducts ("OrderId", "ProductId", "Quantity", "Value", "Currency", "ValueInAUD", "CurrencyAUD") FROM stdin;
50ad4d35-7b1a-4ab9-b485-0e62fe404977	8fad1e5a-d4a2-4688-aa49-e70776940c19	1	200.000	USD	270.000	USD
49ce0715-3492-4c65-8e3b-14f40624b759	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6080.000	USD
3ac53adf-7a3e-4b3b-9df7-4bcdbcb894fb	8fad1e5a-d4a2-4688-aa49-e70776940c19	1	307.910	AUD	307.910	AUD
5b4e319e-c674-4824-8996-1de20b0b704b	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6080.000	USD
07ecf8f0-00ce-4201-a974-56607752bb7c	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6080.000	USD
e25f3eb4-9e74-4e56-bb53-3e27ea775f5a	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	2	80.000	USD	121.600	USD
1b72cbbd-7ec1-4fa7-93fa-06ca11673598	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6154.881	USD
7f23d21d-c318-4806-b459-9edfae900c69	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	1	61.570	AUD	61.570	AUD
42dbc020-b5ff-4cbf-aee5-04b9b6df8608	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	2	123.140	AUD	123.140	AUD
26f7add6-ec8d-4e4a-a4ab-9f920aa7d4cf	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	1000	61570.000	AUD	61570.000	AUD
eaa87e6c-daa3-4478-afe3-0024b6733efb	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	1000	61570.000	AUD	61570.000	AUD
64900eff-fb25-4bb7-8868-71f5f1dbaf46	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	1000	61570.000	AUD	61570.000	AUD
3b743e21-f2f1-4fde-8d88-c48870971488	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6065.761	USD
12a77642-b21d-43ac-8dd0-06fb6f1aba12	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6065.761	USD
ac5c25cd-2992-41b7-ab5a-edf2ee7b8fe6	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6065.761	USD
44d04ad8-45f5-4559-bca3-6fdac7e4667e	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6065.761	USD
79a7c560-2fcc-4360-98c6-14d67dbe7069	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6065.761	USD
c8409be2-3cf8-41a5-88db-a6f4bd02efd8	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6065.761	USD
81466d53-9726-4a7b-8918-98cacafbe584	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6065.761	USD
fc350559-adde-4e53-80d8-69b0b40c6369	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6065.761	USD
adce9087-6c70-4602-ba4c-7480fe696710	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6065.761	USD
52d9e4b0-e5be-469f-a44c-d95649df8f21	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6065.761	USD
5a4c1d58-28a9-4152-9ce2-ae39aa9cb01f	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6086.281	USD
7ff1e1df-1bde-4ba4-8f5c-56fe69ccaf80	8fad1e5a-d4a2-4688-aa49-e70776940c19	10	2000.000	USD	3043.140	USD
7ff1e1df-1bde-4ba4-8f5c-56fe69ccaf80	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	1	40.000	USD	60.863	USD
c624de75-d2a4-4ae3-a06a-d96c0c5d2aec	8fad1e5a-d4a2-4688-aa49-e70776940c19	10	2000.000	USD	3043.140	USD
c624de75-d2a4-4ae3-a06a-d96c0c5d2aec	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	1	40.000	USD	60.863	USD
dc4755c0-126f-4aca-bd1a-b0cf6eeca7a4	8fad1e5a-d4a2-4688-aa49-e70776940c19	10	2000.000	USD	3043.140	USD
dc4755c0-126f-4aca-bd1a-b0cf6eeca7a4	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	1	40.000	USD	60.863	USD
892e2aa0-f34a-447d-9069-f3cdce8f307d	8fad1e5a-d4a2-4688-aa49-e70776940c19	10	2000.000	USD	3043.140	USD
892e2aa0-f34a-447d-9069-f3cdce8f307d	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	1	40.000	USD	60.863	USD
16e26ea5-8b24-4735-9063-db67bcecc0d9	8fad1e5a-d4a2-4688-aa49-e70776940c19	10	2000.000	USD	3043.140	USD
16e26ea5-8b24-4735-9063-db67bcecc0d9	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	1	40.000	USD	60.863	USD
5cdfe838-4212-4fd8-a8ae-bb3874456629	8fad1e5a-d4a2-4688-aa49-e70776940c19	10	2000.000	USD	3043.140	USD
5cdfe838-4212-4fd8-a8ae-bb3874456629	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	1	40.000	USD	60.863	USD
b9ce519c-9243-4cfc-89df-bda4a50e05ad	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6130.921	USD
7b68b0ee-4edc-4599-be2e-7d7eafb0d4d1	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6130.921	USD
e09d1014-0686-45d3-b652-99e8ac29675a	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6130.921	USD
063802e7-066d-4914-afa0-22b2c4270a88	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6130.921	USD
1766ddcf-1ae2-450d-8b86-5b23e9938972	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6130.921	USD
5f79c667-0be0-413f-bcaa-09fa3438bf64	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6142.921	USD
badf9f27-a39d-4786-98d3-79a0dc204dfe	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6142.921	USD
e74e0a01-58e0-41cc-a0d1-45ebd2823acd	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6142.921	USD
c310449e-06dd-44dc-a72d-00733b77b1fb	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	20	1231.400	AUD	1231.400	AUD
d545b41d-19a2-4771-a206-19e27b7bf15a	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6118.921	USD
9cbe624d-ec6c-4b96-83ea-197407478012	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6118.921	USD
24233b54-0f41-46a0-bd1b-8e25fb6ddb75	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6118.921	USD
ab0e6614-9e79-425e-95a6-39e9a867fe5e	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6118.921	USD
52211ee7-1bf9-456b-9349-433353e6c0c8	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6118.921	USD
7c7baf75-7197-4dba-8bb8-90bdca81213a	01997a13-8b27-4e70-b469-68f23ef940ae	1	19.990	AUD	19.990	AUD
7c7baf75-7197-4dba-8bb8-90bdca81213a	01997a13-8b28-439c-afc4-62bb372be845	2	99.980	AUD	99.980	AUD
2c95ee0d-3362-4012-9ac0-64a40ed89afc	01997a13-8b27-4e70-b469-68f23ef940ae	1	19.990	AUD	19.990	AUD
2c95ee0d-3362-4012-9ac0-64a40ed89afc	01997a13-8b28-439c-afc4-62bb372be845	2	99.980	AUD	99.980	AUD
b41c6822-3a62-4f53-b8b2-bffd8448844f	9db6e474-ae74-4cf5-a0dc-ba23a42e2566	100	4000.000	USD	6045.601	USD
\.


--
-- Data for Name: orders; Type: TABLE DATA; Schema: orders; Owner: -
--

COPY orders.orders ("Id", "CustomerId", "IsRemoved", "Value", "Currency", "StatusId", "OrderDate", "OrderChangeDate", "ValueInAUD", "CurrencyAUD") FROM stdin;
50ad4d35-7b1a-4ab9-b485-0e62fe404977	1ff70690-3722-4548-b216-6402fdd012e4	f	200.000	USD	0	2025-07-11 15:06:08.662706	\N	270.000	USD
49ce0715-3492-4c65-8e3b-14f40624b759	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-11 15:12:20.151501	\N	6080.000	USD
3ac53adf-7a3e-4b3b-9df7-4bcdbcb894fb	1ff70690-3722-4548-b216-6402fdd012e4	f	307.910	AUD	0	2025-07-11 15:20:53.107179	\N	307.910	AUD
5b4e319e-c674-4824-8996-1de20b0b704b	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-11 15:22:04.471398	\N	6080.000	USD
07ecf8f0-00ce-4201-a974-56607752bb7c	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-11 15:24:42.909089	\N	6080.000	USD
e25f3eb4-9e74-4e56-bb53-3e27ea775f5a	1ff70690-3722-4548-b216-6402fdd012e4	f	80.000	USD	0	2025-07-15 13:50:44.486094	\N	121.600	USD
1b72cbbd-7ec1-4fa7-93fa-06ca11673598	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-18 11:30:51.491747	\N	6154.881	USD
7f23d21d-c318-4806-b459-9edfae900c69	1ff70690-3722-4548-b216-6402fdd012e4	f	61.570	AUD	0	2025-07-18 11:33:16.810307	\N	61.570	AUD
42dbc020-b5ff-4cbf-aee5-04b9b6df8608	1ff70690-3722-4548-b216-6402fdd012e4	f	123.140	AUD	0	2025-07-18 15:37:49.127844	\N	123.140	AUD
26f7add6-ec8d-4e4a-a4ab-9f920aa7d4cf	1ff70690-3722-4548-b216-6402fdd012e4	f	61570.000	AUD	0	2025-07-23 16:13:29.57063	\N	61570.000	AUD
eaa87e6c-daa3-4478-afe3-0024b6733efb	1ff70690-3722-4548-b216-6402fdd012e4	f	61570.000	AUD	0	2025-07-23 16:16:22.394594	\N	61570.000	AUD
64900eff-fb25-4bb7-8868-71f5f1dbaf46	1ff70690-3722-4548-b216-6402fdd012e4	f	61570.000	AUD	0	2025-07-23 16:17:09.010296	\N	61570.000	AUD
3b743e21-f2f1-4fde-8d88-c48870971488	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-25 10:40:15.832521	\N	6065.761	USD
12a77642-b21d-43ac-8dd0-06fb6f1aba12	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-25 10:52:43.404371	\N	6065.761	USD
ac5c25cd-2992-41b7-ab5a-edf2ee7b8fe6	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-25 11:05:43.718732	\N	6065.761	USD
44d04ad8-45f5-4559-bca3-6fdac7e4667e	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-25 11:19:20.597797	\N	6065.761	USD
79a7c560-2fcc-4360-98c6-14d67dbe7069	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-25 11:30:39.306963	\N	6065.761	USD
c8409be2-3cf8-41a5-88db-a6f4bd02efd8	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-25 11:36:12.002317	\N	6065.761	USD
81466d53-9726-4a7b-8918-98cacafbe584	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-25 11:41:48.773993	\N	6065.761	USD
fc350559-adde-4e53-80d8-69b0b40c6369	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-25 13:28:54.822836	\N	6065.761	USD
adce9087-6c70-4602-ba4c-7480fe696710	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-25 13:29:20.345794	\N	6065.761	USD
52d9e4b0-e5be-469f-a44c-d95649df8f21	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-25 13:29:46.285297	\N	6065.761	USD
5a4c1d58-28a9-4152-9ce2-ae39aa9cb01f	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-28 13:18:59.860646	\N	6086.281	USD
7ff1e1df-1bde-4ba4-8f5c-56fe69ccaf80	1ff70690-3722-4548-b216-6402fdd012e4	f	2040.000	USD	0	2025-07-28 13:24:53.859405	\N	3104.003	USD
c624de75-d2a4-4ae3-a06a-d96c0c5d2aec	1ff70690-3722-4548-b216-6402fdd012e4	f	2040.000	USD	0	2025-07-28 13:42:20.568456	\N	3104.003	USD
dc4755c0-126f-4aca-bd1a-b0cf6eeca7a4	1ff70690-3722-4548-b216-6402fdd012e4	f	2040.000	USD	0	2025-07-28 13:58:36.299599	\N	3104.003	USD
892e2aa0-f34a-447d-9069-f3cdce8f307d	1ff70690-3722-4548-b216-6402fdd012e4	f	2040.000	USD	0	2025-07-28 14:25:03.946403	\N	3104.003	USD
16e26ea5-8b24-4735-9063-db67bcecc0d9	1ff70690-3722-4548-b216-6402fdd012e4	f	2040.000	USD	0	2025-07-28 14:39:41.569932	\N	3104.003	USD
5cdfe838-4212-4fd8-a8ae-bb3874456629	1ff70690-3722-4548-b216-6402fdd012e4	f	2040.000	USD	0	2025-07-28 14:55:03.016515	\N	3104.003	USD
b9ce519c-9243-4cfc-89df-bda4a50e05ad	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-29 08:17:10.986006	\N	6130.921	USD
7b68b0ee-4edc-4599-be2e-7d7eafb0d4d1	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-29 08:47:35.394221	\N	6130.921	USD
e09d1014-0686-45d3-b652-99e8ac29675a	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-29 09:17:18.201362	\N	6130.921	USD
063802e7-066d-4914-afa0-22b2c4270a88	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-29 11:38:50.570934	\N	6130.921	USD
1766ddcf-1ae2-450d-8b86-5b23e9938972	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-29 11:49:21.626775	\N	6130.921	USD
5f79c667-0be0-413f-bcaa-09fa3438bf64	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-30 10:49:47.460224	\N	6142.921	USD
badf9f27-a39d-4786-98d3-79a0dc204dfe	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-30 14:47:42.809141	\N	6142.921	USD
e74e0a01-58e0-41cc-a0d1-45ebd2823acd	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-07-30 14:51:37.364289	\N	6142.921	USD
c310449e-06dd-44dc-a72d-00733b77b1fb	1ff70690-3722-4548-b216-6402fdd012e4	f	1231.400	AUD	0	2025-07-30 15:14:16.079901	2025-08-01 14:49:06.177071	1231.400	AUD
d545b41d-19a2-4771-a206-19e27b7bf15a	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-09-26 10:12:41.872475	\N	6118.921	USD
9cbe624d-ec6c-4b96-83ea-197407478012	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-09-26 10:14:02.320259	\N	6118.921	USD
24233b54-0f41-46a0-bd1b-8e25fb6ddb75	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-09-26 10:24:25.958742	\N	6118.921	USD
ab0e6614-9e79-425e-95a6-39e9a867fe5e	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-09-26 10:39:24.911287	\N	6118.921	USD
52211ee7-1bf9-456b-9349-433353e6c0c8	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-09-26 11:03:41.492478	\N	6118.921	USD
7c7baf75-7197-4dba-8bb8-90bdca81213a	1a0b92a8-e27e-409c-972f-3b4034d00a0b	f	119.970	AUD	0	2025-09-29 13:53:48.876527	\N	119.970	AUD
2c95ee0d-3362-4012-9ac0-64a40ed89afc	1a0b92a8-e27e-409c-972f-3b4034d00a0b	f	119.970	AUD	0	2025-09-29 14:14:18.68008	\N	119.970	AUD
b41c6822-3a62-4f53-b8b2-bffd8448844f	1ff70690-3722-4548-b216-6402fdd012e4	f	4000.000	USD	0	2025-10-07 14:58:00.685772	\N	6045.601	USD
\.


--
-- Data for Name: productprices; Type: TABLE DATA; Schema: orders; Owner: -
--

COPY orders.productprices ("ProductId", "Value", "Currency") FROM stdin;
8fad1e5a-d4a2-4688-aa49-e70776940c19	200.000	USD
8fad1e5a-d4a2-4688-aa49-e70776940c19	180.000	EUR
9db6e474-ae74-4cf5-a0dc-ba23a42e2566	40.000	USD
9db6e474-ae74-4cf5-a0dc-ba23a42e2566	35.000	EUR
9db6e474-ae74-4cf5-a0dc-ba23a42e2566	61.570	AUD
8fad1e5a-d4a2-4688-aa49-e70776940c19	307.910	AUD
01997a13-8b28-439c-afc4-62bb372be845	49.990	AUD
01997a13-8b27-4e70-b469-68f23ef940ae	19.990	AUD
\.


--
-- Data for Name: products; Type: TABLE DATA; Schema: orders; Owner: -
--

COPY orders.products ("Id", "Name") FROM stdin;
8fad1e5a-d4a2-4688-aa49-e70776940c19	Jacket
9db6e474-ae74-4cf5-a0dc-ba23a42e2566	T-shirt
01997a13-8b27-4e70-b469-68f23ef940ae	Casual T-Shirt
01997a13-8b28-439c-afc4-62bb372be845	Denim Jeans
\.


--
-- Data for Name: payments; Type: TABLE DATA; Schema: payments; Owner: -
--

COPY payments.payments ("Id", "CreateDate", "StatusId", "OrderId", "EmailNotificationIsSent") FROM stdin;
1997b762-bf54-4937-9eab-473f1166b573	2025-07-28 13:19:23.859568	0	5a4c1d58-28a9-4152-9ce2-ae39aa9cb01f	f
3fc24950-b1ad-4863-9dd9-40d40245d019	2025-07-28 13:19:25.846746	0	5a4c1d58-28a9-4152-9ce2-ae39aa9cb01f	f
9f516508-940a-411d-9655-a628c6bdfac0	2025-07-28 13:19:02.381374	0	5a4c1d58-28a9-4152-9ce2-ae39aa9cb01f	f
a09028a2-7590-4825-8c59-08e83ad4a2ad	2025-07-28 13:19:09.360863	0	5a4c1d58-28a9-4152-9ce2-ae39aa9cb01f	f
252203c8-4b9a-412d-b6e9-728a5386051f	2025-07-28 13:25:38.398742	0	7ff1e1df-1bde-4ba4-8f5c-56fe69ccaf80	f
5d47d3c4-e6b1-4fe0-8239-078ab5125b68	2025-07-28 13:42:24.146007	0	c624de75-d2a4-4ae3-a06a-d96c0c5d2aec	f
53f00286-c488-423e-baf8-7b7bb111bc7d	2025-07-28 13:58:42.982891	0	dc4755c0-126f-4aca-bd1a-b0cf6eeca7a4	f
aae29d8e-05c3-4a93-bf20-a3d3e1bc1eb9	2025-07-28 14:25:06.679559	0	892e2aa0-f34a-447d-9069-f3cdce8f307d	f
229fb28e-6fa8-4947-a50c-89d6b7e5d6f0	2025-07-28 14:39:43.044058	0	16e26ea5-8b24-4735-9063-db67bcecc0d9	f
8410ac35-79f2-4be6-95d7-dcbb3e514eed	2025-07-28 14:55:05.896003	0	5cdfe838-4212-4fd8-a8ae-bb3874456629	t
53a31343-500f-4275-ac84-43f3202a5fed	2025-07-29 08:17:14.828988	0	b9ce519c-9243-4cfc-89df-bda4a50e05ad	t
417b517c-c6ff-4f42-acaa-297c58c24801	2025-07-29 08:49:04.14437	0	7b68b0ee-4edc-4599-be2e-7d7eafb0d4d1	f
7c8bc052-719a-4212-b959-326e3be2dd9b	2025-07-29 09:17:22.135486	0	e09d1014-0686-45d3-b652-99e8ac29675a	f
c17adb96-571c-48b6-889f-982171bcfe52	2025-07-29 11:38:53.165446	0	063802e7-066d-4914-afa0-22b2c4270a88	f
b5b81265-b010-4939-9e8d-2e9c8836778d	2025-07-29 11:49:25.709828	0	1766ddcf-1ae2-450d-8b86-5b23e9938972	t
ff3d9b00-459f-49e5-af14-3658f768ba3d	2025-07-30 10:49:51.465083	0	5f79c667-0be0-413f-bcaa-09fa3438bf64	t
af2575a1-40dd-4b7e-adc8-af6798a479c6	2025-07-30 14:47:50.254419	0	badf9f27-a39d-4786-98d3-79a0dc204dfe	t
84313bbc-25b4-43db-a034-e5bf91b1f084	2025-07-30 14:51:37.390245	0	e74e0a01-58e0-41cc-a0d1-45ebd2823acd	t
3e96dc6b-5939-4424-ae2a-7177fbdb47a1	2025-07-30 15:14:16.102896	0	c310449e-06dd-44dc-a72d-00733b77b1fb	t
f65c5efc-d72f-4a75-866a-01d2405e15e5	2025-09-26 10:12:41.900664	0	d545b41d-19a2-4771-a206-19e27b7bf15a	f
c8cb6a52-5387-4d7d-828c-58338e5fbbd1	2025-09-26 10:14:02.350168	0	9cbe624d-ec6c-4b96-83ea-197407478012	t
b11fbcd2-b2e6-4ecd-ab59-27deedcbab87	2025-09-26 10:24:25.986764	0	24233b54-0f41-46a0-bd1b-8e25fb6ddb75	t
4eda7436-ee51-4808-88c3-4c3c3eec44dc	2025-09-26 10:39:24.940435	0	ab0e6614-9e79-425e-95a6-39e9a867fe5e	t
3c001177-960f-496e-a860-e55bafeeb000	2025-09-26 11:03:41.531885	0	52211ee7-1bf9-456b-9349-433353e6c0c8	t
8bb581ab-4fb9-4a09-980f-9407b5a6f74a	2025-09-29 13:54:01.538435	0	7c7baf75-7197-4dba-8bb8-90bdca81213a	t
0ee8a4c3-9aa7-4ac4-be18-c3309219187f	2025-09-29 14:14:19.477838	0	2c95ee0d-3362-4012-9ac0-64a40ed89afc	t
d966c04a-76ec-48f0-b651-32557c4743fb	2025-10-07 14:58:02.718293	0	b41c6822-3a62-4f53-b8b2-bffd8448844f	t
\.


--
-- Name: internalcommands PK_app_InternalCommands_Id; Type: CONSTRAINT; Schema: app; Owner: -
--

ALTER TABLE ONLY app.internalcommands
    ADD CONSTRAINT "PK_app_InternalCommands_Id" PRIMARY KEY ("Id");


--
-- Name: outboxmessages PK_app_OutboxMessages_Id; Type: CONSTRAINT; Schema: app; Owner: -
--

ALTER TABLE ONLY app.outboxmessages
    ADD CONSTRAINT "PK_app_OutboxMessages_Id" PRIMARY KEY ("Id");


--
-- Name: customers PK_orders_Customers_Id; Type: CONSTRAINT; Schema: orders; Owner: -
--

ALTER TABLE ONLY orders.customers
    ADD CONSTRAINT "PK_orders_Customers_Id" PRIMARY KEY ("Id");


--
-- Name: orderproducts PK_orders_OrderProducts_OrderId_ProductId; Type: CONSTRAINT; Schema: orders; Owner: -
--

ALTER TABLE ONLY orders.orderproducts
    ADD CONSTRAINT "PK_orders_OrderProducts_OrderId_ProductId" PRIMARY KEY ("OrderId", "ProductId");


--
-- Name: orders PK_orders_Orders_Id; Type: CONSTRAINT; Schema: orders; Owner: -
--

ALTER TABLE ONLY orders.orders
    ADD CONSTRAINT "PK_orders_Orders_Id" PRIMARY KEY ("Id");


--
-- Name: productprices PK_orders_ProductPrices_ProductId_Currency; Type: CONSTRAINT; Schema: orders; Owner: -
--

ALTER TABLE ONLY orders.productprices
    ADD CONSTRAINT "PK_orders_ProductPrices_ProductId_Currency" PRIMARY KEY ("ProductId", "Currency");


--
-- Name: products PK_orders_Products_Id; Type: CONSTRAINT; Schema: orders; Owner: -
--

ALTER TABLE ONLY orders.products
    ADD CONSTRAINT "PK_orders_Products_Id" PRIMARY KEY ("Id");


--
-- Name: payments PK_payments_Payments_Id; Type: CONSTRAINT; Schema: payments; Owner: -
--

ALTER TABLE ONLY payments.payments
    ADD CONSTRAINT "PK_payments_Payments_Id" PRIMARY KEY ("Id");


--
-- PostgreSQL database dump complete
--

