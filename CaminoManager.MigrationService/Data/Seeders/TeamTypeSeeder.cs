using CaminoManager.Data.Contexts;
using CaminoManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.Data.Seeders;

public static class TeamTypeSeeder
{
    public static async Task SeedTeamTypesAsync(CaminoManagerDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.TeamTypes.AnyAsync(cancellationToken))
            return;

        var teamTypes = new[]
        {
            (name: "Catequistas de la Nación", order: 1),
            (name: "Itinerantes", order: 2),
            (name: "Catequistas", order: 3),
            (name: "Responsables", order: 4),
            (name: "Corresponsables", order: 5)
        };

        foreach (var (name, order) in teamTypes)
        {
            var teamType = new TeamType
            {
                Id = Guid.NewGuid(),
                Name = name,
                Order = order
            };

            await dbContext.TeamTypes.AddAsync(teamType, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
} 