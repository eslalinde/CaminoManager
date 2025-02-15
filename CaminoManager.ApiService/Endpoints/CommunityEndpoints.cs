using CaminoManager.Data.Contexts;
using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.DTOs;
using Microsoft.EntityFrameworkCore;
using CaminoManager.ApiService.Mappers;

namespace CaminoManager.ApiService.Endpoints;

public static class CommunityEndpoints
{
    public static IEndpointRouteBuilder MapCommunityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/communities").WithTags("Communities");
        var mapper = new CommunityMapper();

        // GET all communities
        group.MapGet("/", async (CaminoManagerDbContext db) =>
            await db.Communities
                .Select(c => mapper.ToCommunityDto(c))
                .ToListAsync());

        // GET community by id
        group.MapGet("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var community = await db.Communities.FindAsync(id);
            if (community == null) return Results.NotFound();

            return Results.Ok(mapper.ToCommunityDto(community));
        });

        // POST new community
        group.MapPost("/", async (CreateCommunityDto dto, CaminoManagerDbContext db) =>
        {
            var community = mapper.ToEntity(dto);
            
            db.Communities.Add(community);
            await db.SaveChangesAsync();
            
            return Results.Created($"/communities/{community.Id}", 
                mapper.ToCommunityDto(community));
        });

        // PUT update community
        group.MapPut("/{id}", async (string id, UpdateCommunityDto dto, CaminoManagerDbContext db) =>
        {
            var community = await db.Communities.FindAsync(id);
            if (community == null) return Results.NotFound();
            if (id != dto.Id) return Results.BadRequest();

            mapper.UpdateEntity(dto, community);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await db.Communities.AnyAsync(c => c.Id == Guid.Parse(id)))
                    return Results.NotFound();
                throw;
            }
            return Results.NoContent();
        });

        // DELETE community
        group.MapDelete("/{id}", async (string id, CaminoManagerDbContext db) =>
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