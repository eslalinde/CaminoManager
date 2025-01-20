using Microsoft.EntityFrameworkCore;
using CaminoManager.ServiceDefaults.DTOs;
using CaminoManager.ApiService.Mappers;
using CaminoManager.Data.Contexts;

namespace CaminoManager.ApiService.Endpoints;

public static class PriestEndpoints
{
    public static void MapPriestEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/priests").WithTags("Priests");
        var mapper = new PriestMapper();

        group.MapGet("/", async (CaminoManagerDbContext db) =>
        {
            var priests = await db.Priests
                .Include(p => p.Person)
                .Include(p => p.Parish)
                .ToListAsync();
            return priests.Select(mapper.ToDto);
        });

        group.MapGet("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var priest = await db.Priests
                .Include(p => p.Person)
                .Include(p => p.Parish)
                .FirstOrDefaultAsync(p => p.Id == id);
            
            return priest is null
                ? Results.NotFound()
                : Results.Ok(mapper.ToDto(priest));
        });

        group.MapPost("/", async (CreatePriestDto priestDto, CaminoManagerDbContext db) =>
        {
            var priest = mapper.ToEntity(priestDto);
            db.Priests.Add(priest);
            await db.SaveChangesAsync();
            
            var priestWithRelations = await db.Priests
                .Include(p => p.Parish)
                .Include(p => p.Person)
                .FirstAsync(p => p.Id == priest.Id);
            
            return Results.Created($"/priests/{priest.Id}", mapper.ToDto(priestWithRelations));
        });

        group.MapPut("/{id}", async (Guid id, UpdatePriestDto priestDto, CaminoManagerDbContext db) =>
        {
            var priest = await db.Priests.FindAsync(id);
            if (priest is null) return Results.NotFound();

            mapper.UpdateEntity(priestDto, priest);
            await db.SaveChangesAsync();
            
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var priest = await db.Priests.FindAsync(id);
            if (priest is null) return Results.NotFound();

            db.Priests.Remove(priest);
            await db.SaveChangesAsync();
            
            return Results.NoContent();
        });
    }
} 