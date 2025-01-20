using CaminoManager.ApiService.Mappers;
using CaminoManager.Data.Contexts;
using CaminoManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.ApiService.Endpoints;

public static class ParishEndpoints
{
    private static readonly ParishMapper _mapper = new();

    public static IEndpointRouteBuilder MapParishEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/parishes")
            .WithTags("Parishes");

        group.MapGet("/", async (CaminoManagerDbContext db) =>
        {
            var parishes = await db.Parishes
                .Include(p => p.City)
                .Include(p => p.Priests)
                    .ThenInclude(p => p.Person)
                .ToListAsync();

            return Results.Ok(parishes.Select(p => new ParishDto
            {
                Id = p.Id.ToString(),
                Name = p.Name,
                Diocese = p.Diocese,
                Address = p.Address,
                Phone = p.Phone,
                Email = p.Email,
                CityId = p.CityId.ToString(),
                CityName = p.City.Name,
                Priests = p.Priests.Select(priest => new ParishPriestDto
                {
                    PersonId = priest.PersonId.ToString(),
                    IsParishPriest = priest.IsParishPriest,
                    PersonName = priest.Person.PersonName
                }).ToList()
            }));
        });

        group.MapGet("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var parish = await db.Parishes
                .Include(p => p.City)
                .Include(p => p.Priests)
                    .ThenInclude(p => p.Person)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (parish is null)
                return Results.NotFound();

            return Results.Ok(new ParishDto
            {
                Id = parish.Id.ToString(),
                Name = parish.Name,
                Diocese = parish.Diocese,
                Address = parish.Address,
                Phone = parish.Phone,
                Email = parish.Email,
                CityId = parish.CityId.ToString(),
                CityName = parish.City.Name,
                Priests = parish.Priests.Select(priest => new ParishPriestDto
                {
                    PersonId = priest.PersonId.ToString(),
                    IsParishPriest = priest.IsParishPriest,
                    PersonName = priest.Person.PersonName
                }).ToList()
            });
        });

        group.MapPost("/", async (CreateParishDto createDto, CaminoManagerDbContext db) =>
        {
            var strategy = db.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await db.Database.BeginTransactionAsync();
                try
                {
                    var parish = _mapper.ToEntity(createDto);
                    db.Parishes.Add(parish);
                    await db.SaveChangesAsync();

                    // Add priest relationships
                    parish.Priests = createDto.Priests.Select(priestDto => new Priest
                    {
                        ParishId = parish.Id,
                        PersonId = Guid.Parse(priestDto.PersonId),
                        IsParishPriest = priestDto.IsParishPriest
                    }).ToList();

                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var parishWithCity = await db.Parishes
                        .Include(p => p.City)
                        .Include(p => p.Priests)
                            .ThenInclude(p => p.Person)
                        .FirstAsync(p => p.Id == parish.Id);

                    return Results.Created($"/parishes/{parish.Id}", _mapper.ToDto(parishWithCity));
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        });

        group.MapPut("/{id}", async (Guid id, UpdateParishDto updateDto, CaminoManagerDbContext db) =>
        {
            var strategy = db.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await db.Database.BeginTransactionAsync();
                try
                {
                    var parish = await db.Parishes
                        .Include(p => p.City)
                        .Include(p => p.Priests)
                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (parish is null)
                        return Results.NotFound();

                    _mapper.UpdateEntity(updateDto, parish);

                    // Update priest relationships
                    parish.Priests.Clear(); // Clear the navigation property
                    var existingPriests = await db.Priests
                        .Where(pp => pp.ParishId == id)
                        .ToListAsync();
                    db.Priests.RemoveRange(existingPriests);

                    parish.Priests = updateDto.Priests.Select(priestDto => new Priest
                    {
                        ParishId = parish.Id,
                        PersonId = Guid.Parse(priestDto.PersonId),
                        IsParishPriest = priestDto.IsParishPriest
                    }).ToList();

                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Results.Ok(_mapper.ToDto(parish));
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        });

        group.MapDelete("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var parish = await db.Parishes.FindAsync(id);
            if (parish is null)
                return Results.NotFound();

            db.Parishes.Remove(parish);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        return app;
    }
}