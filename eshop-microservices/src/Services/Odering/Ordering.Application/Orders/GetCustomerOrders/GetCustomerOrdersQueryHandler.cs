using Dapper;
using Ordering.Application.Configuration.Data;
using Ordering.Application.Configuration.Queries;

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
        const string sql = @"
            SELECT 
                o.*
            FROM orders.v_Orders AS o 
            WHERE o.""CustomerId"" = @CustomerId";

        var orders = await connection.QueryAsync<OrderDto>(sql, new { request.CustomerId });
        return orders.ToList();
    }
}
