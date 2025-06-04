//namespace Ordering.Domain.Abstraction;

//public interface IAggregate<T>: IAggregate, IEntity<T>
//{

//}


///// <summary>  
///// Represents an aggregate root in the domain-driven design pattern.  
///// An aggregate root is an entity that serves as the entry point to a cluster of related objects.  
///// This interface extends <see cref="IEntity"/> and includes domain events.  
///// </summary>  
//public interface IAggregate : IEntity
//{ 
//    IReadOnlyList<IDomainEvent> DomainEvents { get; }
//    IDomainEvent[] ClearDomainEvents();
//}
