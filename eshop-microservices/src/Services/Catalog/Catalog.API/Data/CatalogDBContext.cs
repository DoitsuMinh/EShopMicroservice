//using Microsoft.EntityFrameworkCore;
//using System.Reflection;

//namespace Catalog.API.Data;

//public class CatalogDBContext : DbContext
//{
//    public CatalogDBContext(DbContextOptions<CatalogDBContext> options) : base(options)
//    {
//    }


//    public DbSet<Product> Product { get; set; }
//    public DbSet<Category> Categories { get; set; }
//    public DbSet<Models.Catalog> Catalogs { get; set; }
//    public DbSet<ProductDetail> ProductDetails { get; set; }
//    public DbSet<ProductQty> ProductQtys { get; set; }

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
//    }
//}