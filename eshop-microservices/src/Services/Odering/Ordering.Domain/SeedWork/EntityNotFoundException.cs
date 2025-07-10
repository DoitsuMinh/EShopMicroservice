namespace Ordering.Domain.SeedWork;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message) : base(message) { }

    public EntityNotFoundException(string name, object key)
        : base($"Entity {name} ({key}) was not found") { }
}
