﻿namespace Ordering.Domain.Abstraction;


// Generic interface for entity with a primary key of type T
public interface IEntity<T> : IEntity
{
    public T Id { get; set; }
}


public interface IEntity
{
    public DateTime? CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
}
