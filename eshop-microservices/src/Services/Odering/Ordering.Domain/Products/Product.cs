﻿using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Products;

public class Product : Entity, IAggregateRoot
{
    public ProductId Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    private Product()
    {
        // EF Core
    }
}
