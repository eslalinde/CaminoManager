using CaminoManager.Data.Contexts;
using CaminoManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.ApiService.Endpoints;

public static class BrotherEndpoints
{
    public static IEndpointRouteBuilder MapBrotherEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/brothers");

        // GET all brothers
        group.MapGet("/", async (CaminoManagerDbContext db) =>
            await db.Brothers
                .Include(b => b.Person)
                .Include(b => b.Community)
                .ToListAsync());

        // GET brother by person and community ids
        group.MapGet("/{personId}/{communityId}", async (Guid personId, Guid communityId, CaminoManagerDbContext db) =>
            await db.Brothers
                .Include(b => b.Person)
                .Include(b => b.Community)
                .FirstOrDefaultAsync(b => b.PersonId == personId && b.CommunityId == communityId) is Brother brother
                    ? Results.Ok(brother)
                    : Results.NotFound());

        // POST new brother
        group.MapPost("/", async (Brother brother, CaminoManagerDbContext db) =>
        {
            db.Brothers.Add(brother);
            await db.SaveChangesAsync();
            return Results.Created($"/brothers/{brother.PersonId}/{brother.CommunityId}", brother);
        });

        // PUT update brother
        group.MapPut("/{personId}/{communityId}", async (Guid personId, Guid communityId, Brother brother, CaminoManagerDbContext db) =>
        {
            if (personId != brother.PersonId || communityId != brother.CommunityId) 
                return Results.BadRequest();

            db.Entry(brother).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await db.Brothers.AnyAsync(b => b.PersonId == personId && b.CommunityId == communityId))
                    return Results.NotFound();
                throw;
            }
            return Results.NoContent();
        });

        // DELETE brother
        group.MapDelete("/{personId}/{communityId}", async (Guid personId, Guid communityId, CaminoManagerDbContext db) =>
        {
            var brother = await db.Brothers.FindAsync(personId, communityId);
            if (brother == null) return Results.NotFound();

            db.Brothers.Remove(brother);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return app;
    }
} 