using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Odering.Infrastructure.Database;

namespace Odering.Infrastructure.Processing.InternalCommands;

internal sealed class InternalCommandEntityTypeConfiguration : IEntityTypeConfiguration<InternalCommand>
{
    public void Configure(EntityTypeBuilder<InternalCommand> builder)
    {
        builder.ToTable("internalcommands", SchemaNames.Application);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}
