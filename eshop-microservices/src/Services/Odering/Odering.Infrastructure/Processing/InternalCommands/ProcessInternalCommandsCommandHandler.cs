using Dapper;
using MediatR;
using Newtonsoft.Json;
using Ordering.Application.Configuration.CQRS.Commands;
using Ordering.Application.Configuration.Data;

namespace Odering.Infrastructure.Processing.InternalCommands;

internal class ProcessInternalCommandsCommandHandler : ICommandHandler<ProcessInternalCommandsCommand, Unit>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    public ProcessInternalCommandsCommandHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Unit> Handle(ProcessInternalCommandsCommand command, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sql = @"
                        SELECT ""Type"", ""Data""
                        FROM app.internalcommands
                        WHERE ""ProcessedDate"" is NULL;";
        var commands = await connection.QueryAsync<InternalCommandDto>(sql);

        var internalCommandsList = commands.ToList();

        foreach (var internalCommand in internalCommandsList)
        {
            Type type = Assemblies.Application.GetType(internalCommand.Type);
            dynamic commandToProcess = JsonConvert.DeserializeObject(internalCommand.Data, type);

            await CommandsExecutor.Execute(commandToProcess);
        }
        
        return Unit.Value;
    }

    private class InternalCommandDto
    {
        public string Type { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
    }
}