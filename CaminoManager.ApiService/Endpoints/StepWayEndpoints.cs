using CaminoManager.ApiService.Mappers;
using CaminoManager.Data.Contexts;
using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.ApiService.Endpoints;

public static class StepWayEndpoints
{
    private static readonly StepWayMapper _mapper = new();

    public static void MapStepWayEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/stepways")
            .WithTags("StepWays");

        group.MapGet("/", async (CaminoManagerDbContext db) =>
        {
            var stepWays = await db.Set<StepWay>()
                .OrderBy(x => x.Order)
                .ToListAsync();
            return stepWays.Select(_mapper.ToDto);
        });

        group.MapGet("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var stepWay = await db.Set<StepWay>().FindAsync(id);
            return stepWay is null ? Results.NotFound() : Results.Ok(_mapper.ToDto(stepWay));
        });

        group.MapPost("/", async (CreateStepWayDto dto, CaminoManagerDbContext db) =>
        {
            var entity = _mapper.ToEntity(dto);
            db.Set<StepWay>().Add(entity);
            await db.SaveChangesAsync();
            return Results.Created($"/api/stepways/{entity.Id}", _mapper.ToDto(entity));
        });

        group.MapPut("/{id}", async (Guid id, UpdateStepWayDto dto, CaminoManagerDbContext db) =>
        {
            var entity = await db.Set<StepWay>().FindAsync(id);
            if (entity is null) return Results.NotFound();

            _mapper.UpdateEntity(dto, entity);
            await db.SaveChangesAsync();
            return Results.Ok(_mapper.ToDto(entity));
        });

        group.MapDelete("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var entity = await db.Set<StepWay>().FindAsync(id);
            if (entity is null) return Results.NotFound();

            db.Set<StepWay>().Remove(entity);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}