using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Odering.Infrastructure.Database;
using Ordering.Domain.Products;

namespace Odering.Infrastructure.Domain.Products;


internal sealed class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    internal const string ProductPrices = "_productprices";
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products", schema: SchemaNames.Orders);

        builder.HasKey(p => p.Id);

        builder.OwnsMany<ProductPrice>(ProductPrices, i =>
        {
            i.ToTable("productprices", SchemaNames.Orders);
            i.Property<ProductId>("ProductId");
            i.Property<string>("Currency").HasColumnType("varchar(3)").IsRequired();
            i.HasKey("ProductId", "Currency");
            i.OwnsOne(x => x.Value, ii =>
            {
                ii.Property(p => p.Currency).HasColumnName("Currency").HasColumnType("varchar(3)").IsRequired();
                ii.Property(p => p.Value).HasColumnName("Value");
            });
        });

    }
}
