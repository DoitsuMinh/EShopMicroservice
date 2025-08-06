using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Data;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    internal const string ProductDetails = "ProductDetail";
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(p => p.Id);
        builder.OwnsMany<ProductDetails>(p => p.ProductDetails, d =>
        {
            d.ToTable("ProductDetail");

            d.WithOwner().HasForeignKey("ProductId");

            d.Property<long>("ProdetailId");
            d.HasKey("ProdetailId");
        });

    }
}
