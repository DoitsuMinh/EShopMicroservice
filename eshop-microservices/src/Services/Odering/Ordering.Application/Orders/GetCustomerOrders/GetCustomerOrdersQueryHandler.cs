using Dapper;
using Ordering.Application.Configuration.CQRS.Queries;
using Ordering.Application.Configuration.Data;

namespace Ordering.Application.Orders.GetCustomerOrders;

public class GetCustomerOrdersQueryHandler : IQueryHandler<GetCustomerOrdersQuery, List<OrderDto>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    internal GetCustomerOrdersQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<List<OrderDto>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();
        const string sql =
            @"SELECT 
                ""Id"" AS OrderId,
                ""Value"", 
                ""IsRemoved"", 
                ""Currency""
            FROM orders.v_orders AS o
            WHERE o.""CustomerId"" = @CustomerId";

        var orders = await connection.QueryAsync<OrderDto>(sql, new { request.CustomerId });
        return orders.ToList();
    }
}
