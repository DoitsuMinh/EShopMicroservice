using Odering.Infrastructure.Processing.Outbox;
using Ordering.Application;
using Ordering.Application.Configuration.CQRS.Commands;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Odering.Infrastructure.Logging;

internal class LoggingCommandHandlerWithResultDecorator<T, TResult> : ICommandHandler<T, TResult> where T : ICommand<TResult>
{
    private readonly ILogger _logger;
    private readonly IExecutionContextAccessor _executionContextAccessor;
    private readonly ICommandHandler<T, TResult> _decorated;

    public LoggingCommandHandlerWithResultDecorator(
        ILogger logger, 
        IExecutionContextAccessor executionContextAccessor,
        ICommandHandler<T, TResult> decorated)
    {
        _logger = logger;
        _executionContextAccessor = executionContextAccessor;
        _decorated = decorated;
    }

    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"LoggingCommandHandlerWithResultDecorator triggered for: {command.GetType().Name}");
        if (command is IRecurringCommand)
        {
            return await _decorated.Handle(command, cancellationToken);
        }
        using (LogContext.Push(
            new RequestLogEnricher(_executionContextAccessor),
            new CommandLogEnricher(command)))
        {
            var commandName = command.GetType().Name;
            try
            {
                _logger.Information($"Executing command {commandName}");

                var result = await _decorated.Handle(command, cancellationToken);

                _logger.Information($"Command {commandName} executed successfully");

                return result;
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

        public RequestLogEnricher(IExecutionContextAccessor execContextAccessor)
        {
            _execContextAccessor = execContextAccessor;
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
        private readonly ICommand<TResult> _command;

        public CommandLogEnricher(ICommand<TResult> command)
        {
            _command = command;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"Command:{_command.Id.ToString()}")));
        }
    }
}