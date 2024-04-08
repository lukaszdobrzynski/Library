namespace Library.BuildingBlocks.Domain;

public abstract class TypedIdBase : IEquatable<TypedIdBase>
{
    public Guid Value { get; set; }

    protected TypedIdBase(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidOperationException("Id value cannot be empty.");
        }

        Value = value;
    }

    public bool Equals(TypedIdBase? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TypedIdBase)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    
    public static bool operator ==(TypedIdBase obj1, TypedIdBase obj2)
    {
        if (Equals(obj1, null))
        {
            return Equals(obj2, null);
        }

        return obj1.Equals(obj2);
    }

    public static bool operator !=(TypedIdBase x, TypedIdBase y)
    {
        return !(x == y);
    }
}