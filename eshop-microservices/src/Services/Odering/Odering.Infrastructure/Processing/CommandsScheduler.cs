using Dapper;
using Newtonsoft.Json;
using Ordering.Application.Configuration.Commands;
using Ordering.Application.Configuration.CQRS.Commands;
using Ordering.Application.Configuration.Data;

namespace Odering.Infrastructure.Processing;

public class CommandsScheduler : ICommandScheduler
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public CommandsScheduler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task EnqueueAsync<T>(ICommand<T> command)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        var sql = @"
                INSERT INTO app.internalcommands(""Id"", ""EnqueueDate"", ""Type"", ""Data"")
                VALUES (@Id, @EnqueueDate, @Type, @Data); ";

        await connection.ExecuteAsync(sql, new
        {
            command.Id,
            EnqueueDate = DateTime.UtcNow,
            Type = command.GetType().FullName,
            Data = JsonConvert.SerializeObject(command)
        });
    }
}