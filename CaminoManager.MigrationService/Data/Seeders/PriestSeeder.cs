using CaminoManager.Data.Contexts;
using CaminoManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.MigrationService.Data.Seeders;

public static class PriestSeeder
{
    public static async Task SeedPriestsAsync(CaminoManagerDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Priests.AnyAsync(cancellationToken))
            return;

        // Get all parishes and priest-type people
        var parishes = await dbContext.Parishes.ToListAsync(cancellationToken);
        var priestPeople = await dbContext.People
            .Where(p => p.PersonTypeId == PersonType.Priest)
            .ToListAsync(cancellationToken);

        var priests = new List<Priest>();
        var currentPriestIndex = 0;

        foreach (var parish in parishes)
        {
            // Create parish priest (main priest)
            priests.Add(new Priest
            {
                Id = Guid.NewGuid(),
                PersonId = priestPeople[currentPriestIndex++].Id,
                IsParishPriest = true,
                Parish = parish
            });

            // Create assistant priest
            priests.Add(new Priest
            {
                Id = Guid.NewGuid(),
                PersonId = priestPeople[currentPriestIndex++].Id,
                IsParishPriest = false,
                Parish = parish
            });
        }

        await dbContext.Priests.AddRangeAsync(priests, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}