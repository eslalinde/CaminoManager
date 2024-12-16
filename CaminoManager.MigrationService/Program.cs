using CaminoManager.MigrationService;
using CaminoManager.Data.Contexts;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddNpgsqlDbContext<CaminoManagerDbContext>("postgres",
    configureSettings: settings =>
    {
        // Configure your connection settings here if needed
    });

var host = builder.Build();
host.Run();
