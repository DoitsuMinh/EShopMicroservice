using Ordering.Domain.SeedWork;
using System.Collections;
using System.Reflection;

namespace Ordering.UnitTests.SeedWork;

public class DomainEventsTestHelper
{
    /// <summary>
    /// gets all domain events from an aggregate entity, including those from nested entities and collections.
    /// </summary>
    /// <param name="aggregate">The root aggregate entity to extract domain events from.</param>
    /// <returns>A list of all domain events found in the aggregate and its nested entities.</returns>
    public static List<IDomainEvent> GetAllDomainEvents(Entity aggregate)
    {
        // List to collect all domain events found
        var domainEvents = new List<IDomainEvent>();

        // Add domain events from the root aggregate if any exist
        if (aggregate.DomainEvents != null)
        {
            domainEvents.AddRange(aggregate.DomainEvents);
        }

        // Get all fields (private, public, instance) from the aggregate and its base type
        var fields =
            aggregate
            .GetType()
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
            .Concat(aggregate.GetType().BaseType?.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public) ?? [])
            .ToArray();

        // Iterate through each field to find nested entities or collections of entities
        foreach (var field in fields)
        {
            // Check if the field is an Entity or derived from Entity
            var isEntity = field.FieldType.IsAssignableFrom(typeof(Entity));

            if (isEntity)
            {
                // If the field value is an Entity, recursively get its domain events
                if (field.GetValue(aggregate) is Entity entity)
                {
                    domainEvents.AddRange(GetAllDomainEvents(entity).ToList());
                }
            }

            // Check if the field is a collection (excluding string)
            if (field.FieldType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(field.FieldType))
            {
                if (field.GetValue(aggregate) is IEnumerable enumerable)
                {
                    // Iterate through the collection and recursively get domain events from each Entity item
                    foreach (var e in enumerable)
                    {
                        if (e is Entity entityItem)
                        {
                            domainEvents.AddRange(GetAllDomainEvents(entityItem));
                        }
                    }
                }
            }
        }

        return domainEvents;
    }

    /// <summary>
    /// clears all domain events from an aggregate entity, including those from nested entities and collections.
    /// </summary>
    public static void ClearAllDomainEvents(Entity aggregate)
    {
        aggregate.ClearDomainEvents();

        var fields =
            aggregate
            .GetType()
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
            .Concat(aggregate.GetType().BaseType?.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public) ?? [])
            .ToArray();
        
        foreach (var field in fields)
        {
            // Check if the field is an Entity or derived from Entity
            var isEntity = field.FieldType.IsAssignableFrom(typeof(Entity));

            if (isEntity)
            {
                // If the field value is an Entity, recursively clear its domain events
                if (field.GetValue(aggregate) is Entity entity)
                {
                    ClearAllDomainEvents(entity);
                }
            }

            // Check if the field is a collection (excluding string)
            if (field.FieldType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(field.FieldType))
            {
                if (field.GetValue(aggregate) is IEnumerable enumerable)
                {
                    // Iterate through the collection and recursively clear domain events from each Entity item
                    foreach (var e in enumerable)
                    {
                        if (e is Entity entityItem)
                        {
                            ClearAllDomainEvents(entityItem);
                        }
                    }
                }
            }
        }
    }
}