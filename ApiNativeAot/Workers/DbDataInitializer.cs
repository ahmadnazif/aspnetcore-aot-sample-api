using ApiNativeAot.Services;

namespace ApiNativeAot.Workers;

public class DbDataInitializer(ILogger<DbDataInitializer> logger, InMemorySimpleDb db) : BackgroundService
{
    private readonly ILogger<DbDataInitializer> logger = logger;
    private readonly InMemorySimpleDb db = db;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(1000, stoppingToken);

        await db.AddSmsAsync(Guid.NewGuid().ToString(), "John", "Max", "Hello bro!");
        await db.AddSmsAsync(Guid.NewGuid().ToString(), "Max", "John", "Hello back bro!");

        logger.LogInformation($"2 initial entry added to database");
    }
}
