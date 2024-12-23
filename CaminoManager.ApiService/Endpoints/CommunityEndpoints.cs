using CaminoManager.Data.Contexts;
using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.ApiService.Endpoints;

public static class CommunityEndpoints
{
    public static IEndpointRouteBuilder MapCommunityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/communities").WithTags("Communities");

        // GET all communities
        group.MapGet("/", async (CaminoManagerDbContext db) =>
            await db.Communities
                .Select(c => new CommunityDto
                {
                    Id = c.Id,
                    Number = c.Number,
                    BornDate = c.BornDate,
                    ParishId = c.ParishId,
                    BornBrothers = c.BornBrothers,
                    ActualBrothers = c.ActualBrothers,
                    StepWayId = c.StepWayId,
                    StepWayDate = c.StepWayDate,
                    CatechistTeamId = c.CatechistTeamId,
                    OldCatechist = c.OldCatechist
                })
                .ToListAsync());

        // GET community by id
        group.MapGet("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var community = await db.Communities.FindAsync(id);
            if (community == null) return Results.NotFound();

            return Results.Ok(new CommunityDto
            {
                Id = community.Id,
                Number = community.Number,
                BornDate = community.BornDate,
                ParishId = community.ParishId,
                BornBrothers = community.BornBrothers,
                ActualBrothers = community.ActualBrothers,
                StepWayId = community.StepWayId,
                StepWayDate = community.StepWayDate,
                CatechistTeamId = community.CatechistTeamId,
                OldCatechist = community.OldCatechist
            });
        });

        // POST new community
        group.MapPost("/", async (CreateCommunityDto dto, CaminoManagerDbContext db) =>
        {
            var community = new Community
            {
                Number = dto.Number,
                BornDate = dto.BornDate,
                ParishId = dto.ParishId,
                BornBrothers = dto.BornBrothers,
                ActualBrothers = dto.ActualBrothers,
                StepWayId = dto.StepWayId,
                StepWayDate = dto.StepWayDate,
                CatechistTeamId = dto.CatechistTeamId,
                OldCatechist = dto.OldCatechist
            };

            db.Communities.Add(community);
            await db.SaveChangesAsync();
            return Results.Created($"/communities/{community.Id}",
                new CommunityDto { Id = community.Id, /* ... other properties ... */ });
        });

        // PUT update community
        group.MapPut("/{id}", async (Guid id, UpdateCommunityDto dto, CaminoManagerDbContext db) =>
        {
            var community = await db.Communities.FindAsync(id);
            if (community == null) return Results.NotFound();
            if (id != dto.Id) return Results.BadRequest();

            community.Number = dto.Number;
            community.BornDate = dto.BornDate;
            community.ParishId = dto.ParishId;
            community.BornBrothers = dto.BornBrothers;
            community.ActualBrothers = dto.ActualBrothers;
            community.StepWayId = dto.StepWayId;
            community.StepWayDate = dto.StepWayDate;
            community.CatechistTeamId = dto.CatechistTeamId;
            community.OldCatechist = dto.OldCatechist;

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