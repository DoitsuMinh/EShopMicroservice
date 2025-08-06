using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Odering.Infrastructure.Database;
using Ordering.Domain.Payments;

namespace Odering.Infrastructure.Domain.Payments;

internal sealed class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments", SchemaNames.Payments);

        builder.HasKey(p => p.Id);

        builder.Property("_orderId")
            .HasColumnName("OrderId")
            .IsRequired();
        builder.Property<DateTime>("_createdDate")
            .HasColumnName("CreateDate");
        builder.Property("_status").HasColumnName("StatusId")
            .HasConversion(new EnumToNumberConverter<PaymentStatus, byte>())
            .IsRequired();
        builder.Property<bool>("_emailNotificationIsSent")
            .HasColumnName("EmailNotificationIsSent")
            .IsRequired();

    }
}
