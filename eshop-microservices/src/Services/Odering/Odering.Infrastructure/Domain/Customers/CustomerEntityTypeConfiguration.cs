using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Odering.Infrastructure.Database;
using Ordering.Domain.Customers;
using Ordering.Domain.Customers.Orders;
using Ordering.Domain.Products;
using Ordering.Domain.Shared.MoneyValue;

namespace Odering.Infrastructure.Domain.Customers;

internal sealed class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    internal const string OrderList = "_orders";
    internal const string OrderProducts = "_orderProducts";

    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers", SchemaNames.Orders);

        builder.HasKey(c => c.Id);

        builder.Property("_welcomeEmailWasSent").HasColumnName("WelcomeEmailWasSent");
        builder.Property("_email").HasColumnName("Email");
        builder.Property("_name").HasColumnName("Name");

        builder.OwnsMany<Order>(OrderList, x =>
        {
            x.WithOwner().HasForeignKey("CustomerId");

            x.ToTable("orders", SchemaNames.Orders);

            x.Property<bool>("_isRemoved").HasColumnName("IsRemoved");
            x.Property<DateTime>("_orderDate").HasColumnName("OrderDate");
            x.Property<DateTime?>("_orderChangeDate")
                .HasColumnName("OrderChangeDate")
                .IsRequired(false);
            x.Property<OrderId>("Id");
            x.HasKey("Id");

            x.Property("_status").HasColumnName("StatusId")
                .HasConversion(new EnumToNumberConverter<OrderStatus, byte>());

            x.OwnsMany<OrderProduct>(OrderProducts, op =>
            {
                op.WithOwner().HasForeignKey("OrderId");
                
                op.ToTable("orderproducts", SchemaNames.Orders);
                op.Property<ProductId>("ProductId");
                op.Property<OrderId>("OrderId");

                op.HasKey("ProductId", "OrderId");

                op.OwnsOne<MoneyValue>("Value", mv =>
                {
                    mv.Property(p => p.Currency).HasColumnName("Currency");
                    mv.Property(p => p.Value).HasColumnName("Value");
                });

                op.OwnsOne<MoneyValue>("ValueInAUD", mv =>
                {
                    mv.Property(p => p.Currency).HasColumnName("CurrencyAUD");
                    mv.Property(p => p.Value).HasColumnName("ValueInAUD");
                });
            });

            x.OwnsOne<MoneyValue>("_value", mv =>
            {
                mv.Property(p => p.Currency).HasColumnName("Currency");
                mv.Property(p => p.Value).HasColumnName("Value");
            });

            x.OwnsOne<MoneyValue>("_valueInAUD", y =>
            {
                y.Property(p => p.Currency).HasColumnName("CurrencyAUD");
                y.Property(p => p.Value).HasColumnName("ValueInAUD");
            });
        });
    }
}
