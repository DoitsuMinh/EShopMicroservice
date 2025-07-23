using Dapper;
using MediatR;
using Newtonsoft.Json;
using Ordering.Application.Configuration.CQRS.Commands;
using Ordering.Application.Configuration.Data;
using Ordering.Application.Configuration.DomainEvents;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Odering.Infrastructure.Processing.Outbox;

internal class ProcessOutboxCommandHandler : ICommandHandler<ProcessOutboxCommand, Unit>
{
    private readonly IMediator _mediator;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public ProcessOutboxCommandHandler(IMediator mediator, ISqlConnectionFactory sqlConnectionFactory)
    {
        _mediator = mediator;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Unit> Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();
        const string sql = @"
            SELECT ""Id"", ""Type"", ""Data""
            FROM app.outboxmessages
            WHERE ""ProcessedDate"" IS NULL;";

        var messages = await connection.QueryAsync<OutBoxMessageDto>(sql);
        var messagesList = messages.ToList();

        const string sqlUpdateProcessDate = @"
            UPDATE app.outboxmessages
            SET ""ProcessedDate"" = @ProcessedDate
            WHERE ""Id"" = @Id;";

        if (messagesList.Count > 0)
        {
            foreach (var message in messagesList)
            {
                Type type = Assemblies.Application.GetType(message.Type);
                var request = JsonConvert.DeserializeObject(message.Data, type) as IDomainEventNotification;

                using (LogContext.Push(new OutboxMessageContextEnricher(request)))
                {
                    await _mediator.Publish(request, cancellationToken);

                    await connection.ExecuteAsync(sqlUpdateProcessDate, new
                    {
                        ProcessedDate = DateTime.UtcNow,
                        message.Id
                    });
                }                
            }
        }

        return Unit.Value;
    }

    private class OutboxMessageContextEnricher : ILogEventEnricher
    {
        private readonly IDomainEventNotification _notification;

        public OutboxMessageContextEnricher(IDomainEventNotification domainEventNotification)
        {
            _notification = domainEventNotification;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(
                new LogEventProperty("Context", new ScalarValue($"OutboxMessage: {_notification.Id.ToString()}")));
        }
    }
}
