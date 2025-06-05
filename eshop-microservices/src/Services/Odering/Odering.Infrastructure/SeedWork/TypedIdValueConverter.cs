using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ordering.Domain.SeedWork;

namespace Odering.Infrastructure.SeedWork;

public class TypedIdValueConverter<TTypedIdValue> : ValueConverter<TTypedIdValue, Guid>
  where TTypedIdValue : TypedIdValueBase
{
    public TypedIdValueConverter(ConverterMappingHints mappingHints = null)
            : base(id => id.Value, value => Create(value), mappingHints)
    {
    }

    private static TTypedIdValue Create(Guid id)
    {
        var instance = Activator.CreateInstance(typeof(TTypedIdValue), id);
        if (instance is null)
        {
            throw new InvalidOperationException($"Unable to create an instance of {typeof(TTypedIdValue)} with the provided Guid.");
        }
        return (TTypedIdValue)instance;
    }
}
