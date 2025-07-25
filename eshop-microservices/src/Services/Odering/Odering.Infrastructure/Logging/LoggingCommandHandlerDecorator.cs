using MediatR;
using Odering.Infrastructure.Processing.Outbox;
using Ordering.Application;
using Ordering.Application.Configuration.CQRS.Commands;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Odering.Infrastructure.Logging;

internal class LoggingCommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
{
    private readonly ICommandHandler<T> _decorated;

    private readonly IExecutionContextAccessor _executionContextAccessor;

    private readonly ILogger _logger;

    public LoggingCommandHandlerDecorator(
        ICommandHandler<T> decorated, 
        IExecutionContextAccessor executionContextAccessor,
        ILogger logger)
    {
        _decorated = decorated;
        _executionContextAccessor = executionContextAccessor;
        _logger = logger;
    }

    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        if (command is IRecurringCommand)
        {
            await _decorated.Handle(command, cancellationToken);
            ;
        }

        using (LogContext.Push(
            new RequestLogEnricher(_executionContextAccessor),
            new CommandLogEnricher(command))
            )
        {
            var commandName = command.GetType().Name;
            try
            {
                _logger.Information($"Executing command {commandName}");

                await _decorated.Handle(command, cancellationToken);

                _logger.Information($"Command {commandName} executed successfully");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Command {commandName} processing failed");
                throw;
            }
        }
    }

    private class RequestLogEnricher : ILogEventEnricher
    {
        private readonly IExecutionContextAccessor _execContextAccessor;

        public RequestLogEnricher(IExecutionContextAccessor executionContextAccessor)
        {
            _execContextAccessor = executionContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (_execContextAccessor.IsAvailable)
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId", new ScalarValue(_execContextAccessor.CorrelationId)));
            }
        }
    }

    private class CommandLogEnricher : ILogEventEnricher
    {
        private readonly ICommand _command;

        public CommandLogEnricher(ICommand command)
        {
            _command = command;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"Command: {_command.Id.ToString()}")));
        }
    }
}