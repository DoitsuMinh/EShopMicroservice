using Ordering.Application.Configuration.Data;
using Ordering.Domain.Customers;
using Dapper;

namespace Ordering.Application.Customers.DomainService;

public class CustomerUniquenessChecker : ICustomerUniquenessChecker
{
    private readonly ISqlConnectionFactory _sqlConnection;
    public CustomerUniquenessChecker(ISqlConnectionFactory sqlConnection)
    {
        _sqlConnection = sqlConnection;
    }

    public bool IsUnique(string email)
    {
        var connection = _sqlConnection.GetOpenConnection();

        const string sql = @"
            SELECT 1
            FROM orders.customers c
            WHERE c.""Email"" = @Email
            LIMIT 1";
        var count = connection.QuerySingleOrDefault<int?>(sql, new { Email = email });
        return !count.HasValue;

    }
}