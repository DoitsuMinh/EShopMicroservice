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
        try
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string sql = @"
                        SELECT ""Id"", ""Type"", ""Data""
                        FROM app.internalcommands
                        WHERE ""ProcessedDate"" is NULL;";
            var commands = await connection.QueryAsync<InternalCommandDto>(sql);

            //const string sqlUpdateProcessDate = @"
            //UPDATE app.internalcommands
            //SET ""ProcessedDate"" = @ProcessedDate
            //WHERE ""Id"" = @Id;";

            var internalCommandsList = commands.ToList();

            if (internalCommandsList.Count > 0)
            {
                foreach (var internalCommand in internalCommandsList)
                {
                    Type type = Assemblies.Application.GetType(internalCommand.Type);
                    dynamic commandToProcess = JsonConvert.DeserializeObject(internalCommand.Data, type);

                    await CommandsExecutor.Execute(commandToProcess);

                    //await connection.ExecuteAsync(sqlUpdateProcessDate, new
                    //{
                    //    ProcessedDate = DateTime.UtcNow,
                    //    internalCommand.Id
                    //});
                }
            }
            

            return Unit.Value;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new ApplicationException("An error occurred while processing internal commands.", ex);
        }
        
    }

    private class InternalCommandDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
    }
}