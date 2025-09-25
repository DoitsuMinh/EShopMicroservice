using Dapper;
using Ordering.Application.Configuration.CQRS.Queries;
using Ordering.Application.Configuration.Data;

namespace Ordering.Application.Orders.GetCustomerOrderDetails;

public class GetCustomerOrderDetailsQueryHandler : IQueryHandler<GetCustomerOrderDetailsQuery, OrderDetailsDto>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetCustomerOrderDetailsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<OrderDetailsDto> Handle(GetCustomerOrderDetailsQuery request, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sql = 
            @"SELECT 
                ""Id"", 
                ""CustomerId"", 
                ""Value"", 
                ""IsRemoved"", 
                ""Currency""
	        FROM orders.v_orders as o
            WHERE o.""Id"" = @OrderId;";
        var order = await connection.QuerySingleOrDefaultAsync<OrderDetailsDto>(sql, new { request.OrderId });

        const string sqlProducts =
            @"SELECT 
                ""ProductId"" AS Id, 
                ""Quantity"", 
                ""Name"",
                ""Value"", 
                ""Currency""
    	    FROM orders.v_orderproducts as op
            WHERE op.""OrderId"" = @OrderId;";
        var products = await connection.QueryAsync<ProductDto>(sqlProducts, new { request.OrderId });

        order.Products = products.ToList();

        return order;
    }
}
