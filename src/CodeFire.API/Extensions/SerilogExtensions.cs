using Serilog;
using Serilog.Events;

namespace CodeFire.API.Extensions;

public static class SerilogExtensions
{
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        // Create Serilog logger
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .WriteTo.Console()
            .WriteTo.File(
                "logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

        // Register Serilog
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog(logger);

        return builder;
    }
} 