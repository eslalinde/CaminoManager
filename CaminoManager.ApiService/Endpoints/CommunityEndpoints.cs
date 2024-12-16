using CaminoManager.Data.Contexts;
using CaminoManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.ApiService.Endpoints;

public static class CommunityEndpoints
{
    public static IEndpointRouteBuilder MapCommunityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/communities");

        // GET all communities
        group.MapGet("/", async (CaminoManagerDbContext db) =>
            await db.Communities.ToListAsync());

        // GET community by id
        group.MapGet("/{id}", async (Guid id, CaminoManagerDbContext db) =>
            await db.Communities.FindAsync(id) is Community community
                ? Results.Ok(community)
                : Results.NotFound());

        // POST new community
        group.MapPost("/", async (Community community, CaminoManagerDbContext db) =>
        {
            db.Communities.Add(community);
            await db.SaveChangesAsync();
            return Results.Created($"/communities/{community.Id}", community);
        });

        // PUT update community
        group.MapPut("/{id}", async (Guid id, Community community, CaminoManagerDbContext db) =>
        {
            if (id != community.Id) return Results.BadRequest();

            db.Entry(community).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await db.Communities.AnyAsync(c => c.Id == id))
                    return Results.NotFound();
                throw;
            }
            return Results.NoContent();
        });

        // DELETE community
        group.MapDelete("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var community = await db.Communities.FindAsync(id);
            if (community == null) return Results.NotFound();

            db.Communities.Remove(community);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return app;
    }
} 