//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Catalog.API.Data;

//public class CatalogEntityTypeConfiguration : IEntityTypeConfiguration<Models.Catalog>
//{
//    public void Configure(EntityTypeBuilder<Models.Catalog> builder)
//    {
//        builder.ToTable("Catalog");
//        builder.HasKey(p => p.Id);

//        builder.Property(p => p.CreatedDate).IsRequired();

//        builder.Property(p => p.Status).HasColumnType("nvarchar(50)").IsRequired();

//        builder.OwnsMany(p => p.Categories, c =>
//        {
//            c.ToTable("Category");

//            c.WithOwner().HasForeignKey("CatalogId");
//            c.Property<int>("CategoryId");
//            c.HasKey("CategoryId");

//            c.Property(p => p.Status).HasConversion<string>().HasColumnType("nvarchar(50)").IsRequired();
//            c.Property(p => p.CreatedDate).IsRequired();
//        });
//    }
//}
