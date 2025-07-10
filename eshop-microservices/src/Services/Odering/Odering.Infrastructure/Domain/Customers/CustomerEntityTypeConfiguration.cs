using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Odering.Infrastructure.Database;
using Ordering.Domain.Customers;
using System.Reflection.Emit;

namespace Odering.Infrastructure.Domain.Customers;

internal sealed class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    internal const string OrderList = "_orders";
    internal const string OrderProducts = "_orderProducts";

    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customer", SchemaNames.Orders);

        builder.HasKey(c => c.Id);

        builder.Property("");
    }
}
