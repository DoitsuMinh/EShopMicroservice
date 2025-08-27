using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Models;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("sku")]
    public string Sku { get; set; }

    [BsonElement("description")]
    public ProductDescription Description { get; set; }

    [BsonElement("price")]
    public PriceInfo Price { get; set; }

    [BsonElement("brand")]
    public string Brand { get; set; }

    [BsonElement("is_active")]
    public bool IsActive { get; set; }

    [BsonElement("stock_total")]
    public int StockTotal { get; set; }

    [BsonElement("images")]
    public List<ProductImage> Images { get; set; }

    [BsonElement("attributes")]
    public List<ProductAttribute> Attributes { get; set; }

    [BsonElement("variants")]
    public List<ProductVariant> Variants { get; set; }

    [BsonElement("category_ids")]
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> CategoryIds { get; set; }

    [BsonElement("created_at")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updated_at")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime UpdatedAt { get; set; }
}


public class ProductDescription
{
    [BsonElement("short")]
    public string Short { get; set; }

    [BsonElement("long")]
    public string Long { get; set; }
}

public class PriceInfo
{
    [BsonElement("base")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Base { get; set; }

    [BsonElement("sale")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Sale { get; set; }
}

public class ProductImage
{
    [BsonElement("url")]
    public string Url { get; set; }

    [BsonElement("label")]
    public string Label { get; set; }
}

public class ProductAttribute
{
    [BsonElement("key")]
    public string Key { get; set; }

    [BsonElement("value")]
    public string Value { get; set; }
}

public class ProductVariant
{
    [BsonElement("sku")]
    public string Sku { get; set; }

    [BsonElement("attributes")]
    public Dictionary<string, string> Attributes { get; set; } // e.g., {"color": "Blue", "size": "M"}

    [BsonElement("stock")]
    public int Stock { get; set; }

    [BsonElement("price_modifier")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PriceModifier { get; set; }
}

/*
 * 
 * db.category.insertMany([
  {
    "_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a1"),
    "name": "Clothing",
    "slug": "clothing",
    "parent_id": null,
    "ancestors": []
  },
  {
    "_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a2"),
    "name": "Menswear",
    "slug": "menswear",
    "parent_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a1"),
    "ancestors": [
      { "_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a1"), "name": "Clothing", "slug": "clothing" }
    ]
  },
  {
    "_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a3"),
    "name": "Womenswear",
    "slug": "womenswear",
    "parent_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a1"),
    "ancestors": [
      { "_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a1"), "name": "Clothing", "slug": "clothing" }
    ]
  },
  {
    "_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a4"),
    "name": "T-Shirts",
    "slug": "t-shirts",
    "parent_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a2"),
    "ancestors": [
      { "_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a1"), "name": "Clothing", "slug": "clothing" },
      { "_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a2"), "name": "Menswear", "slug": "menswear" }
    ]
  },
  {
    "_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a5"),
    "name": "Jeans",
    "slug": "jeans",
    "parent_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a2"),
    "ancestors": [
      { "_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a1"), "name": "Clothing", "slug": "clothing" },
      { "_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a2"), "name": "Menswear", "slug": "menswear" }
    ]
  },
  {
    "_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a6"),
    "name": "Dresses",
    "slug": "dresses",
    "parent_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a3"),
    "ancestors": [
        { "_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a1"), "name": "Clothing", "slug": "clothing" },
        { "_id": ObjectId("67c9c2a3e3f3a2b1d4e8f1a3"), "name": "Womenswear", "slug": "womenswear" }
    ]
  }
]);
 * 
 * 
 * db.product.insertMany([
  // Product 1: Men's T-Shirt
  {
    "name": "Organic Cotton Crewneck Tee",
    "sku": "TEE-CREW-ORG-01",
    "description": { "short": "A soft, breathable 100% organic cotton tee.", "long": "Sustainably sourced and incredibly comfortable, this classic crewneck is a wardrobe essential. Perfect for layering or wearing on its own." },
    "price": { "base": Decimal128("30.00"), "sale": Decimal128("24.99") },
    "brand": "EcoWear",
    "is_active": true,
    "stock_total": 150,
    "images": [{ "url": "/images/tee-crew-white.jpg", "label": "White" }, { "url": "/images/tee-crew-black.jpg", "label": "Black" }],
    "attributes": [{ "key": "Material", "value": "100% Organic Cotton" }, { "key": "Fit", "value": "Regular" }],
    "variants": [
      { "sku": "TEE-CREW-WHT-M", "attributes": { "color": "White", "size": "M" }, "stock": 50 },
      { "sku": "TEE-CREW-WHT-L", "attributes": { "color": "White", "size": "L" }, "stock": 40 },
      { "sku": "TEE-CREW-BLK-M", "attributes": { "color": "Black", "size": "M" }, "stock": 60 }
    ],
    "category_ids": [ObjectId("67c9c2a3e3f3a2b1d4e8f1a4"), ObjectId("67c9c2a3e3f3a2b1d4e8f1a2")],
    "created_at": ISODate("2025-08-27T01:25:00Z"),
    "updated_at": ISODate("2025-08-27T01:25:00Z")
  },
  // Product 2: Men's Jeans
  {
    "name": "Slim Fit Stretch Denim Jeans",
    "sku": "JNS-SLIM-STR-01",
    "description": { "short": "Modern slim fit jeans with a hint of stretch.", "long": "Crafted for comfort and style, these jeans move with you. Features a classic five-pocket design and a dark indigo wash." },
    "price": { "base": Decimal128("85.00"), "sale": null },
    "brand": "Denim Co.",
    "is_active": true,
    "stock_total": 80,
    "images": [{ "url": "/images/jeans-slim-indigo.jpg", "label": "Indigo Wash" }],
    "attributes": [{ "key": "Material", "value": "98% Cotton, 2% Elastane" }, { "key": "Fit", "value": "Slim" }],
    "variants": [
      { "sku": "JNS-SLIM-IND-32", "attributes": { "color": "Indigo", "size": "32x32" }, "stock": 40 },
      { "sku": "JNS-SLIM-IND-34", "attributes": { "color": "Indigo", "size": "34x32" }, "stock": 40 }
    ],
    "category_ids": [ObjectId("67c9c2a3e3f3a2b1d4e8f1a5"), ObjectId("67c9c2a3e3f3a2b1d4e8f1a2")],
    "created_at": ISODate("2025-08-26T14:00:00Z"),
    "updated_at": ISODate("2025-08-26T14:00:00Z")
  },
  // Product 3: Women's Dress
  {
    "name": "Floral Print Midi Dress",
    "sku": "DRS-MIDI-FLR-01",
    "description": { "short": "A light and airy floral midi dress.", "long": "Perfect for sunny days, this dress features a vibrant floral pattern, a comfortable waist tie, and a flowing skirt." },
    "price": { "base": Decimal128("75.00"), "sale": Decimal128("59.50") },
    "brand": "SummerBloom",
    "is_active": true,
    "stock_total": 65,
    "images": [{ "url": "/images/dress-midi-floral.jpg", "label": "Floral Print" }],
    "attributes": [{ "key": "Material", "value": "100% Viscose" }, { "key": "Style", "value": "A-Line" }],
    "variants": [
      { "sku": "DRS-MIDI-FLR-S", "attributes": { "color": "Floral", "size": "S" }, "stock": 20 },
      { "sku": "DRS-MIDI-FLR-M", "attributes": { "color": "Floral", "size": "M" }, "stock": 30 },
      { "sku": "DRS-MIDI-FLR-L", "attributes": { "color": "Floral", "size": "L" }, "stock": 15 }
    ],
    "category_ids": [ObjectId("67c9c2a3e3f3a2b1d4e8f1a6"), ObjectId("67c9c2a3e3f3a2b1d4e8f1a3")],
    "created_at": ISODate("2025-08-25T11:30:00Z"),
    "updated_at": ISODate("2025-08-25T11:30:00Z")
  },
  // Product 4: Unisex Hoodie
  {
    "name": "Classic Zip-Up Hoodie",
    "sku": "HD-ZIP-CLS-01",
    "description": { "short": "A cozy fleece-lined zip-up hoodie.", "long": "Your new go-to for comfort. Made with a soft cotton blend and fleece interior, featuring a drawstring hood and front pockets." },
    "price": { "base": Decimal128("60.00"), "sale": null },
    "brand": "Urban Threads",
    "is_active": true,
    "stock_total": 200,
    "images": [{ "url": "/images/hoodie-zip-grey.jpg", "label": "Heather Grey" }, { "url": "/images/hoodie-zip-navy.jpg", "label": "Navy" }],
    "attributes": [{ "key": "Material", "value": "80% Cotton, 20% Polyester" }],
    "variants": [
      { "sku": "HD-ZIP-GRY-M", "attributes": { "color": "Grey", "size": "M" }, "stock": 70 },
      { "sku": "HD-ZIP-GRY-L", "attributes": { "color": "Grey", "size": "L" }, "stock": 50 },
      { "sku": "HD-ZIP-NVY-M", "attributes": { "color": "Navy", "size": "M" }, "stock": 80 }
    ],
    "category_ids": [ObjectId("67c9c2a3e3f3a2b1d4e8f1a2"), ObjectId("67c9c2a3e3f3a2b1d4e8f1a3")],
    "created_at": ISODate("2025-08-24T09:00:00Z"),
    "updated_at": ISODate("2025-08-24T09:00:00Z")
  },
  // Product 5: Women's Blouse
  {
    "name": "Silk Button-Up Blouse",
    "sku": "BLS-SILK-BTN-01",
    "description": { "short": "An elegant and versatile silk blouse.", "long": "Made from pure mulberry silk, this blouse has a luxurious feel and a beautiful drape. A timeless piece for work or evening wear." },
    "price": { "base": Decimal128("120.00"), "sale": null },
    "brand": "Elegance Couture",
    "is_active": true,
    "stock_total": 45,
    "images": [{ "url": "/images/blouse-silk-ivory.jpg", "label": "Ivory" }],
    "attributes": [{ "key": "Material", "value": "100% Silk" }],
    "variants": [
      { "sku": "BLS-SILK-IVY-S", "attributes": { "color": "Ivory", "size": "S" }, "stock": 15 },
      { "sku": "BLS-SILK-IVY-M", "attributes": { "color": "Ivory", "size": "M" }, "stock": 20 },
      { "sku": "BLS-SILK-IVY-L", "attributes": { "color": "Ivory", "size": "L" }, "stock": 10 }
    ],
    "category_ids": [ObjectId("67c9c2a3e3f3a2b1d4e8f1a3")],
    "created_at": ISODate("2025-08-23T18:20:00Z"),
    "updated_at": ISODate("2025-08-23T18:20:00Z")
  },
  // Add 5 more dummy products...
  { // Product 6
    "name": "Men's Chino Shorts",
    "sku": "SHT-CHN-MEN-01",
    "description": { "short": "Classic chino shorts for warm weather.", "long": "Comfortable and stylish, these shorts are made from durable cotton twill with a touch of stretch for all-day comfort." },
    "price": { "base": Decimal128("45.00"), "sale": null },
    "brand": "Urban Threads",
    "is_active": true,
    "stock_total": 120,
    "images": [{ "url": "/images/shorts-chino-khaki.jpg", "label": "Khaki" }, { "url": "/images/shorts-chino-blue.jpg", "label": "Navy Blue" }],
    "attributes": [{ "key": "Material", "value": "97% Cotton, 3% Spandex" }],
    "variants": [
      { "sku": "SHT-CHN-KHK-32", "attributes": { "color": "Khaki", "size": "32" }, "stock": 50 },
      { "sku": "SHT-CHN-KHK-34", "attributes": { "color": "Khaki", "size": "34" }, "stock": 40 },
      { "sku": "SHT-CHN-BLU-32", "attributes": { "color": "Navy", "size": "32" }, "stock": 30 }
    ],
    "category_ids": [ObjectId("67c9c2a3e3f3a2b1d4e8f1a2")],
    "created_at": ISODate("2025-08-22T10:00:00Z"),
    "updated_at": ISODate("2025-08-22T10:00:00Z")
  },
  { // Product 7
    "name": "Women's High-Waist Leggings",
    "sku": "LEG-HW-WM-01",
    "description": { "short": "Performance leggings with a high-waist fit.", "long": "Designed for yoga or the gym, these leggings are made from a moisture-wicking fabric that offers support and flexibility." },
    "price": { "base": Decimal128("65.00"), "sale": Decimal128("49.99") },
    "brand": "ActiveLife",
    "is_active": true,
    "stock_total": 90,
    "images": [{ "url": "/images/leggings-hw-black.jpg", "label": "Black" }],
    "attributes": [{ "key": "Material", "value": "88% Polyester, 12% Spandex" }],
    "variants": [
      { "sku": "LEG-HW-BLK-S", "attributes": { "color": "Black", "size": "S" }, "stock": 30 },
      { "sku": "LEG-HW-BLK-M", "attributes": { "color": "Black", "size": "M" }, "stock": 40 },
      { "sku": "LEG-HW-BLK-L", "attributes": { "color": "Black", "size": "L" }, "stock": 20 }
    ],
    "category_ids": [ObjectId("67c9c2a3e3f3a2b1d4e8f1a3")],
    "created_at": ISODate("2025-08-21T12:45:00Z"),
    "updated_at": ISODate("2025-08-21T12:45:00Z")
  },
  { // Product 8
    "name": "Men's Plaid Flannel Shirt",
    "sku": "SHT-FLN-PLD-01",
    "description": { "short": "A warm and rugged plaid flannel shirt.", "long": "Made from heavyweight brushed cotton, this shirt is perfect for cooler temperatures. Features a classic button-down front and two chest pockets." },
    "price": { "base": Decimal128("55.00"), "sale": null },
    "brand": "NorthRidge",
    "is_active": true,
    "stock_total": 75,
    "images": [{ "url": "/images/shirt-flannel-red.jpg", "label": "Red Plaid" }],
    "attributes": [{ "key": "Material", "value": "100% Brushed Cotton" }],
    "variants": [
      { "sku": "SHT-FLN-RED-M", "attributes": { "color": "Red Plaid", "size": "M" }, "stock": 35 },
      { "sku": "SHT-FLN-RED-L", "attributes": { "color": "Red Plaid", "size": "L" }, "stock": 25 },
      { "sku": "SHT-FLN-RED-XL", "attributes": { "color": "Red Plaid", "size": "XL" }, "stock": 15 }
    ],
    "category_ids": [ObjectId("67c9c2a3e3f3a2b1d4e8f1a2")],
    "created_at": ISODate("2025-08-20T16:10:00Z"),
    "updated_at": ISODate("2025-08-20T16:10:00Z")
  },
  { // Product 9
    "name": "Vintage Wash Denim Jacket",
    "sku": "JKT-DNM-VIN-01",
    "description": { "short": "A timeless denim jacket with a vintage fade.", "long": "This wardrobe staple features a classic fit, button-front closure, and perfectly worn-in look." },
    "price": { "base": Decimal128("95.00"), "sale": null },
    "brand": "Denim Co.",
    "is_active": false,
    "stock_total": 30,
    "images": [{ "url": "/images/jacket-denim-vintage.jpg", "label": "Vintage Blue" }],
    "attributes": [{ "key": "Material", "value": "100% Cotton" }],
    "variants": [
      { "sku": "JKT-DNM-VIN-S", "attributes": { "color": "Vintage Blue", "size": "S" }, "stock": 10 },
      { "sku": "JKT-DNM-VIN-M", "attributes": { "color": "Vintage Blue", "size": "M" }, "stock": 20 }
    ],
    "category_ids": [ObjectId("67c9c2a3e3f3a2b1d4e8f1a2"), ObjectId("67c9c2a3e3f3a2b1d4e8f1a3")],
    "created_at": ISODate("2025-08-19T09:05:00Z"),
    "updated_at": ISODate("2025-08-19T09:05:00Z")
  },
  { // Product 10
    "name": "Women's Knit Sweater",
    "sku": "SWT-KNT-WM-01",
    "description": { "short": "A cozy cable-knit sweater for chilly days.", "long": "Stay warm in style with this beautifully textured sweater, featuring a relaxed fit and ribbed trim." },
    "price": { "base": Decimal128("70.00"), "sale": null },
    "brand": "Cozy Knits",
    "is_active": true,
    "stock_total": 55,
    "images": [{ "url": "/images/sweater-knit-cream.jpg", "label": "Cream" }],
    "attributes": [{ "key": "Material", "value": "55% Cotton, 45% Acrylic" }],
    "variants": [
      { "sku": "SWT-KNT-CRM-S", "attributes": { "color": "Cream", "size": "S" }, "stock": 20 },
      { "sku": "SWT-KNT-CRM-M", "attributes": { "color": "Cream", "size": "M" }, "stock": 35 }
    ],
    "category_ids": [ObjectId("67c9c2a3e3f3a2b1d4e8f1a3")],
    "created_at": ISODate("2025-08-18T15:00:00Z"),
    "updated_at": ISODate("2025-08-18T15:00:00Z")
  }
]);

 */
