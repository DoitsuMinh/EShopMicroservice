﻿using Microsoft.EntityFrameworkCore;
using Odering.Infrastructure.Processing.InternalCommands;
using Ordering.Domain.Customers;
using Ordering.Domain.Customers.Orders;

namespace Odering.Infrastructure.Database;

public class OrdersContext: DbContext
{
    public OrdersContext(DbContextOptions<OrdersContext> options)
        : base(options)
    {
    }
    // Define DbSets for your entities here
     public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<InternalCommand> InternalCommands { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersContext).Assembly);
    }
}