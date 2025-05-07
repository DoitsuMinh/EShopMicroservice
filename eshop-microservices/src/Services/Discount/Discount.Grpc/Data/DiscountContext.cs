using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContext: DbContext
{
    public DiscountContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>()
            .HasData(
                new Coupon
                {
                    Id = 1,
                    ProductName = "Tactical Combat Boots",
                    Description = "Tactical Combat Boots Discount",
                    Amount = 150
                },
                new Coupon
                {
                    Id = 2,
                    ProductName = "Night Vision Goggles",
                    Description = "Night Vision Goggles Discount",
                    Amount = 100
                },
                new Coupon
                {
                    Id = 3,
                    ProductName = "Tactical Vest",
                    Description = "Tactical Vest Discount",
                    Amount = 200
                }
            );
    }

    public DbSet<Coupon> Coupons { get; set; } = default!;


}
