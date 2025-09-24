using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();
        if (await session.Query<Product>().AnyAsync(cancellation))
            return;

        // Marten will automatically create the schema for the Product class
        session.Store<Product>(GetPreconfiguredProducts());

        await session.SaveChangesAsync(cancellation);
    }

    private static IEnumerable<Product> GetPreconfiguredProducts() =>
    [
        new Product {
            Name = "Casual T-Shirt",
            Category = ["Apparel", "Clothing", "Casual"],
            Description = "Comfortable cotton t-shirt available in various colors",
            ImageFile = "casual_tshirt.jpg",
            Price = 19.99m,
            Status = true
        },
        new Product {
            Name = "Denim Jeans",
            Category = ["Apparel", "Clothing", "Casual"],
            Description = "Classic blue denim jeans with a modern fit",
            ImageFile = "denim_jeans.jpg",
            Price = 49.99m,
            Status = true
        },
        new Product {
            Name = "Formal Suit",
            Category = ["Apparel", "Clothing", "Formal"],
            Description = "Elegant two-piece suit for formal occasions",
            ImageFile = "formal_suit.jpg",
            Price = 199.99m,
            Status = true
        },
        new Product {
            Name = "Winter Jacket",
            Category = ["Apparel", "Clothing", "Outerwear"],
            Description = "Insulated jacket designed for cold weather",
            ImageFile = "winter_jacket.jpg",
            Price = 129.99m,
            Status = true
        },
        new Product {
            Name = "Running Shoes",
            Category = ["Apparel", "Footwear", "Sports"],
            Description = "Lightweight running shoes with excellent grip",
            ImageFile = "running_shoes.jpg",
            Price = 89.99m,
            Status = true
        },
        new Product {
            Name = "Summer Dress",
            Category = ["Apparel", "Clothing", "Casual"],
            Description = "Light and breezy summer dress for warm days",
            ImageFile = "summer_dress.jpg",
            Price = 39.99m,
            Status = true
        },
        new Product {
            Name = "Leather Belt",
            Category = ["Apparel", "Accessories", "Formal"],
            Description = "Genuine leather belt with a sleek buckle",
            ImageFile = "leather_belt.jpg",
            Price = 29.99m,
            Status = true
        },
        new Product {
            Name = "Wool Scarf",
            Category = ["Apparel", "Accessories", "Winter"],
            Description = "Soft wool scarf to keep you warm in winter",
            ImageFile = "wool_scarf.jpg",
            Price = 24.99m,
            Status = true
        },
        new Product {
            Name = "Hoodie",
            Category = ["Apparel", "Clothing", "Casual"],
            Description = "Cozy hoodie with a front pocket and drawstring hood",
            ImageFile = "hoodie.jpg",
            Price = 49.99m,
            Status = true
        },
        new Product {
            Name = "Sneakers",
            Category = ["Apparel", "Footwear", "Casual"],
            Description = "Trendy sneakers for everyday wear",
            ImageFile = "sneakers.jpg",
            Price = 69.99m,
            Status = true
        }
    ];
}
