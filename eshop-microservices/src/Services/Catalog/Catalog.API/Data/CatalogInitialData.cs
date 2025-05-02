using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();
        if (await session.Query<Product>().AnyAsync(cancellation))
            return;

        // Marten UPSERT will cater for existing records
        session.Store<Product>(GetPreconfiguredProducts());
        await session.SaveChangesAsync(cancellation);
    }

    private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>()
    {
        new Product {
            Id = Guid.NewGuid(),
            Name = "Tactical Combat Boots",
            Category = new List<string> { "Apparel", "Footwear", "Military" },
            Description = "High-durability boots designed for rough terrain and tactical missions",
            ImageFile = "combat_boots.jpg",
            Price = 149.99m,
            Status = true
        },
        new Product {
            Id = Guid.NewGuid(),
            Name = "Night Vision Goggles",
            Category = new List<string> { "Equipment", "Optics", "Military" },
            Description = "Gen-3 infrared night vision goggles for stealth operations",
            ImageFile = "nv_goggles.jpg",
            Price = 2499.00m,
            Status = true
        },
        new Product {
            Id = Guid.NewGuid(),
            Name = "Tactical Vest",
            Category = new List<string> { "Gear", "Protection", "Military" },
            Description = "Modular tactical vest with ballistic plate support",
            ImageFile = "tactical_vest.jpg",
            Price = 289.50m,
            Status = true
        },
        new Product {
            Id = Guid.NewGuid(),
            Name = "Camouflage Uniform Set",
            Category = new List<string> { "Apparel", "Uniform", "Military" },
            Description = "Multi-terrain camo uniform with moisture-wicking fabric",
            ImageFile = "camo_uniform.jpg",
            Price = 89.99m,
            Status = true
        },
        new Product {
            Id = Guid.NewGuid(),
            Name = "Military Backpack 60L",
            Category = new List<string> { "Gear", "Bags", "Military" },
            Description = "Heavy-duty 60-liter rucksack with Molle system",
            ImageFile = "military_backpack.jpg",
            Price = 79.99m,
            Status = true
        },
        new Product {
            Id = Guid.NewGuid(),
            Name = "Field Radio Transceiver",
            Category = new List<string> { "Communication", "Equipment", "Military" },
            Description = "Encrypted VHF/UHF handheld field radio",
            ImageFile = "field_radio.jpg",
            Price = 499.00m,
            Status = true
        },
        new Product {
            Id = Guid.NewGuid(),
            Name = "Combat Knife",
            Category = new List<string> { "Weapons", "Tools", "Military" },
            Description = "Full-tang steel combat knife with tactical sheath",
            ImageFile = "combat_knife.jpg",
            Price = 59.99m,
            Status = true
        },
        new Product {
            Id = Guid.NewGuid(),
            Name = "Ballistic Helmet",
            Category = new List<string> { "Protection", "Headgear", "Military" },
            Description = "NIJ Level IIIA ballistic combat helmet",
            ImageFile = "helmet.jpg",
            Price = 399.99m,
            Status = true
        },
        new Product {
            Id = Guid.NewGuid(),
            Name = "Drone Recon System",
            Category = new List<string> { "Surveillance", "Technology", "Military" },
            Description = "Reconnaissance drone with real-time HD video and GPS",
            ImageFile = "drone.jpg",
            Price = 6999.00m,
            Status = true
        },
        new Product {
            Id = Guid.NewGuid(),
            Name = "Portable Ration Pack (MRE)",
            Category = new List<string> { "Supplies", "Food", "Military" },
            Description = "24-hour meal kit for field deployment - 3000 kcal",
            ImageFile = "mre.jpg",
            Price = 19.99m,
            Status = true
        }
    };

}
