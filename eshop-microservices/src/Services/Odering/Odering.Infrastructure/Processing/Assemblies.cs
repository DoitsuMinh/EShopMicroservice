﻿using Ordering.Application.Customers.GetCustomerDetails;
using System.Reflection;
//using Odering.Application.Orders.PlaceCustomerOrder;

namespace Odering.Infrastructure.Processing;

internal static class Assemblies
{
    public static readonly Assembly Application =
        typeof(GetCustomerDetailsQuery).Assembly;


}