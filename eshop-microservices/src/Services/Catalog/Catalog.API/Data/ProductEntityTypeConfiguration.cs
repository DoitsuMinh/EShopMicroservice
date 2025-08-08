using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Data;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("product");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.CreatedDate).IsRequired();
        builder.Property(p => p.Category).HasColumnType("int").IsRequired();
        builder.Property(p => p.Status).HasConversion<string>().HasColumnType("nvarchar(20)").IsRequired();

        builder.OwnsMany(p => p.ProductDetails, d =>
        {
            d.ToTable("ProductDetail");

            d.WithOwner().HasForeignKey("ProductId");

            d.Property<long>("ProdetailId");
            d.HasKey("ProdetailId");

            d.Property(x => x.RefId).HasColumnType("uniqueidentifier");
            d.Property(x => x.CreatedDate).IsRequired();
            d.Property(x => x.Sku).HasColumnType("nvarchar(20)").IsRequired();
            d.Property(x => x.Status).HasConversion<string>().HasColumnType("nvarchar(20)").IsRequired();

            d.OwnsOne(p => p.ProductQty, q =>
            {
                q.ToTable("ProductQty");
                q.WithOwner().HasForeignKey("ProdetailId");

                q.Property<long>("Id");
                q.HasKey("Id");
                q.Property(x => x.CreatedDate).IsRequired();
            });
        });

    }
}
