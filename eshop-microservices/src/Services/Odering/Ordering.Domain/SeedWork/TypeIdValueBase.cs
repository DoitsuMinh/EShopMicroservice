namespace Ordering.Domain.SeedWork;

public abstract class TypedIdValueBase : IEquatable<TypedIdValueBase>
{
    public Guid Value { get; }

    protected TypedIdValueBase(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Value cannot be an empty GUID.", nameof(value));
        }
        Value = value;
    }

    /// <summary>
    /// Checks if the current instance is equal to another object.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return obj is TypedIdValueBase other && Equals(other);
    }

    /// <summary>
    /// Checks if the current instance is equal to another TypedIdValueBase instance.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(TypedIdValueBase? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    /// <summary>
    /// Returns a hash code for this instance based on the Value property.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Checks if two TypedIdValueBase instances are equal.
    /// </summary>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    /// <returns></returns>
    public static bool operator ==(TypedIdValueBase obj1, TypedIdValueBase obj2)
    {
        if (object.Equals(obj1, null))
        {
            return object.Equals(obj2, null);
        }
        return obj1.Equals(obj2);
    }

    /// <summary>
    /// Checks if two TypedIdValueBase instances are not equal.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static bool operator !=(TypedIdValueBase x, TypedIdValueBase y)
    {
        return !(x == y);
    }
}
