﻿
//namespace Ordering.Domain.Abstraction;

//public class Aggregate<TId> : Entity<TId>, IAggregate<TId>
//{
//    private readonly List<IDomainEvent> _domainEvents = [];
//    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

//    public void AddDomainEvent(IDomainEvent domainEvent)
//    {
//        _domainEvents.Add(domainEvent);
//    }

//    public IDomainEvent[] ClearDomainEvents()
//    {
//        IDomainEvent[] events = [.. _domainEvents]; // same as _domainEvents.ToArray()

//        _domainEvents.Clear();

//        return events;
//    }
//}
