using CaminoManager.Data.Contexts;
using CaminoManager.MigrationService.Data.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace CaminoManager.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CaminoManagerDbContext>();

            await EnsureDatabaseAsync(dbContext, stoppingToken);
            await RunMigrationAsync(dbContext, stoppingToken);
            await SeedDataAsync(dbContext, stoppingToken);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task EnsureDatabaseAsync(CaminoManagerDbContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }

    private static async Task RunMigrationAsync(CaminoManagerDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            // await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
            // await transaction.CommitAsync(cancellationToken);
        });
    }

    private static async Task SeedDataAsync(CaminoManagerDbContext dbContext, CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Seeding data");
        await LocationSeeder.SeedLocationsAsync(dbContext, cancellationToken);
        await StepWaySeeder.SeedStepWaysAsync(dbContext, cancellationToken);
        await TeamTypeSeeder.SeedTeamTypesAsync(dbContext, cancellationToken);
        await PersonSeeder.SeedPeopleAsync(dbContext, cancellationToken);
        await ParishSeeder.SeedParishesAsync(dbContext, cancellationToken);
        await PriestSeeder.SeedPriestsAsync(dbContext, cancellationToken);
        await CommunitySeeder.SeedCommunitiesAsync(dbContext, cancellationToken);
    }
}