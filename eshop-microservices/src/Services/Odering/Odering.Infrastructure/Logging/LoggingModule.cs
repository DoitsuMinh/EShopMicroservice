using Autofac;
using Serilog;

namespace Odering.Infrastructure.Logging;

public class LoggingModule : Module
{
    private readonly ILogger _logger;

    internal LoggingModule(ILogger logger)
    {
        _logger = logger;
    }

    protected override void Load(ContainerBuilder builder)
    {
        // Register Serilog's ILogger interface
        builder.RegisterInstance(_logger)
            .As<ILogger>()
            .SingleInstance();
    }
}