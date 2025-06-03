using System.Reflection;

namespace Ordering.Domain.SeedWork;

public abstract class ValueObject : IEquatable<ValueObject>
{
    private List<PropertyInfo> _properties = [];
    private List<FieldInfo> _fields = [];

    /// <summary>
    /// Checks if the current ValueObject is equal to another ValueObject.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool Equals(ValueObject? obj)
    {
        return Equals(obj as object);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || GetType() != obj.GetType())
        {
            return false;
        }

        return GetProperties().All(p => PropertiesAreEqual(obj, p)) 
            && GetFields().All(f => FieldsAreEqual(obj, f));
    }

    /// <summary>
    /// Returns a hash code for this ValueObject based on its properties and fields.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        //return base.GetHashCode();
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
    /// Retrieves the fields of the ValueObject that are not marked with IgnoreMemberAttribute.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<PropertyInfo> GetProperties()
    {
        if (_properties == null)
        {
            _properties = GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !Attribute.IsDefined(p, typeof(IgnoreMemberAttribute)))
                .ToList();
        }
        return _properties;
    }


    private IEnumerable<FieldInfo> GetFields()
    {
        if (_fields == null)
        {
            _fields = GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => !Attribute.IsDefined(f, typeof(IgnoreMemberAttribute)))
                .ToList();
        }
        return _fields;
    }

    private static int HashValue(int seed, object? value)
    {
        var currentHash = value?.GetHashCode() ?? 0;

        return seed * 23 + currentHash;
    }

    protected static void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new BusinessRuleValidationException(rule);
        }
    }
}
