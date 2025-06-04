using System.Reflection;

namespace Ordering.Domain.SeedWork;

public abstract class ValueObject : IEquatable<ValueObject>
{
    private List<PropertyInfo>? _properties;
    private List<FieldInfo>? _fields;

    public static bool operator ==(ValueObject left, ValueObject right)
    {
        if (left.Equals(null))
        {
            if (right.Equals(null))
            {
                return true; // both are null
            }
            return false; // left is null, right is not
        }
        return left.Equals(right); // both are not null, check equality
    }

    public static bool operator !=(ValueObject left, ValueObject right)
    {
        return !(left == right); // use the equality operator
    }

    /// <summary>
    /// Checks if the current ValueObject is equal to another ValueObject.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ValueObject? obj)
    {
        return Equals(obj as object);
    }

    /// <summary>
    /// Checks if the current ValueObject is equal to another object.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        return GetProperties().All(p => PropertiesAreEqual(obj, p))
            && GetFields().All(f => FieldsAreEqual(obj, f));
    }

    /// <summary>
    /// This method generates a composite hash from all the properties and fields, so that it aligns with the equality logic.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        unchecked   //allow overflow
        {
            int hash = 17;
            foreach (var prop in GetProperties())
            {
                var value = prop.GetValue(this, null);
                hash = HashValue(hash, value);
            }

            foreach (var field in GetFields())
            {
                var value = field.GetValue(this);
                hash = HashValue(hash, value);
            }

            return hash;
        }
    }

    private bool PropertiesAreEqual(object obj, PropertyInfo p)
    {
        return object.Equals(p.GetValue(this, null), p.GetValue(obj, null));
    }

    private bool FieldsAreEqual(object obj, FieldInfo f)
    {
        return object.Equals(f.GetValue(this), f.GetValue(obj));
    }


    /// <summary>
    /// Retrieves the fields of the ValueObject that are not marked with [IgnoreMember].
    /// </summary>
    /// <returns></returns>
    public IEnumerable<PropertyInfo> GetProperties()
    {
        if (_properties == null)
        {
            _properties = GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !Attribute.IsDefined(p, typeof(IgnoreMemberAttribute)))
                .OrderBy(p => p.Name) // Ensure consistent order
                .ToList();
        }
        return _properties;
    }

    /// <summary>
    /// Retrieves the fields of the ValueObject that are not marked with [IgnoreMember].
    /// </summary>
    /// <returns></returns>
    private List<FieldInfo> GetFields()
    {
        if (_fields == null)
        {
            _fields = GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => !Attribute.IsDefined(f, typeof(IgnoreMemberAttribute)))
                .OrderBy(f => f.Name) // Ensure consistent order
                .ToList();
        }
        return _fields;
    }

    private static int HashValue(int seed, object? value)
    {
        var currentHash = value?.GetHashCode() ?? 0;

        return seed * 23 + currentHash;
    }

    /// <summary>
    /// This is a domain validation mechanism
    /// </summary>
    /// <param name="rule"></param>
    /// <exception cref="BusinessRuleValidationException"></exception>
    protected static void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new BusinessRuleValidationException(rule);
        }
    }

}
