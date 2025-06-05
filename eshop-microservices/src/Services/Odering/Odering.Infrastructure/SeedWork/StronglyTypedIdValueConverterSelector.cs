using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ordering.Domain.SeedWork;
using System.Collections.Concurrent;

namespace Odering.Infrastructure.SeedWork;

/// <summary>
/// Custom value converter selector for strongly-typed ID value objects.
/// This allows EF Core to convert between strongly-typed IDs and their underlying Guid values.
/// </summary>
internal class StronglyTypedIdValueConverterSelector : ValueConverterSelector
{
    // Cache for created ValueConverterInfo instances to avoid redundant creation.
    private readonly ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo> _converters
        = new ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo>();

    /// <summary>
    /// Constructor that passes dependencies to the base ValueConverterSelector.
    /// </summary>
    public StronglyTypedIdValueConverterSelector(ValueConverterSelectorDependencies dependencies) : base(dependencies)
    { }

    /// <summary>
    /// Returns a set of value converters for the given model and provider types.
    /// Adds a custom converter for strongly-typed IDs if needed.
    /// </summary>
    public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type providerClrType = null)
    {
        // Yield all base converters first (default EF Core behavior).
        var baseConverters = base.Select(modelClrType, providerClrType);
        foreach (var converter in baseConverters)
        {
            yield return converter;
        }

        // Unwrap nullable types to get the underlying type.
        var underlyingModelType = UnwrapNullableType(modelClrType);
        var underlyingProviderType = UnwrapNullableType(providerClrType);

        // Only add a custom converter if the provider type is Guid or not specified.
        if (underlyingProviderType is null || underlyingProviderType == typeof(Guid))
        {
            // Check if the model type is a strongly-typed ID (inherits from TypedIdValueBase).
            var isTypedIdValue = typeof(TypedIdValueBase).IsAssignableFrom(underlyingModelType);
            if (isTypedIdValue)
            {
                // Create a generic converter type for the strongly-typed ID.
                var converterType = typeof(TypedIdValueConverter<>).MakeGenericType(underlyingModelType);

                // Get or add the converter info to the cache and yield it.
                yield return _converters.GetOrAdd((underlyingModelType, typeof(Guid)), _ =>
                {
                    return new ValueConverterInfo(
                        modelClrType: modelClrType,
                        providerClrType: typeof(Guid),
                        factory: valueConverterInfo =>
                        {
                            // Ensure mapping hints are provided.
                            if (valueConverterInfo.MappingHints == null)
                            {
                                throw new ArgumentNullException(nameof(valueConverterInfo.MappingHints), "MappingHints cannot be null.");
                            }
                            // Create the value converter instance using reflection.
                            return (ValueConverter)Activator.CreateInstance(converterType, valueConverterInfo.MappingHints)!;
                        });
                });
            }
        }
    }

 
    /** VERSION NOT USING YEILD
     public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type providerClrType = null)
    {
        // List to hold all converters.  
        var converters = new List<ValueConverterInfo>();

        // Add all base converters (default EF Core behavior).  
        var baseConverters = base.Select(modelClrType, providerClrType);
        converters.AddRange(baseConverters);

        // Unwrap nullable types to get the underlying type.  
        var underlyingModelType = UnwrapNullableType(modelClrType);
        var underlyingProviderType = UnwrapNullableType(providerClrType);

        // Only add a custom converter if the provider type is Guid or not specified.  
        if (underlyingProviderType is null || underlyingProviderType == typeof(Guid))
        {
            // Check if the model type is a strongly-typed ID (inherits from TypedIdValueBase).  
            var isTypedIdValue = typeof(TypedIdValueBase).IsAssignableFrom(underlyingModelType);
            if (isTypedIdValue)
            {
                // Create a generic converter type for the strongly-typed ID.  
                var converterType = typeof(TypedIdValueConverter<>).MakeGenericType(underlyingModelType);

                // Get or add the converter info to the cache and add it to the list.  
                var converterInfo = _converters.GetOrAdd((underlyingModelType, typeof(Guid)), _ =>
                {
                    return new ValueConverterInfo(
                        modelClrType: modelClrType,
                        providerClrType: typeof(Guid),
                        factory: valueConverterInfo =>
                        {
                            // Ensure mapping hints are provided.  
                            if (valueConverterInfo.MappingHints == null)
                            {
                                throw new ArgumentNullException(nameof(valueConverterInfo.MappingHints), "MappingHints cannot be null.");
                            }
                            // Create the value converter instance using reflection.  
                            return (ValueConverter)Activator.CreateInstance(converterType, valueConverterInfo.MappingHints)!;
                        });
                });

                converters.Add(converterInfo);
            }
        }

        return converters;
    }
     **/


    /// <summary>
    /// Helper to unwrap nullable types and return the underlying type.
    /// </summary>
    private static Type UnwrapNullableType(Type type)
    {
        if (type is null)
        {
            return null!;
        }

        return Nullable.GetUnderlyingType(type) ?? type;
    }
}
