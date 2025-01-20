using CaminoManager.Data.Contexts;
using CaminoManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.MigrationService.Data.Seeders;

public static class StepWaySeeder
{
    public static async Task SeedStepWaysAsync(CaminoManagerDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.StepWays.AnyAsync(cancellationToken))
            return;

        var steps = new[]
        {
            (name: "Catequesis iniciales", order: 1),
            (name: "1º Escrutinio Bautismal", order: 2),
            (name: "Shemá Israel", order: 3),
            (name: "2º Escrutinio Bautismal", order: 4),
            (name: "1ª Iniciación a la Oración", order: 5),
            (name: "Traditio Symboli", order: 6),
            (name: "Redditio Symboli", order: 7),
            (name: "2ª Iniciación a la Oración: Padrenuestro", order: 8),
            (name: "3º Escrutinio Bautismal", order: 9),
            (name: "Renovación Promesas Bautismales", order: 10)
        };

        foreach (var (name, order) in steps)
        {
            var step = new StepWay
            {
                Id = Guid.NewGuid(),
                Name = name,
                Order = order
            };

            await dbContext.StepWays.AddAsync(step, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}