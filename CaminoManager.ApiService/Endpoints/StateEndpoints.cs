using CaminoManager.ApiService.Mappers;
using CaminoManager.Data.Contexts;
using CaminoManager.ServiceDefaults.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.ApiService.Endpoints;

public static class StateEndpoints
{
    private static readonly StateMapper _mapper = new();

    public static void MapStateEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/states")
            .WithTags("States");

        group.MapGet("/", async (CaminoManagerDbContext db) =>
        {
            var states = await db.States.ToListAsync();
            return states.Select(_mapper.ToDto);
        });

        group.MapGet("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var state = await db.States.FindAsync(id);
            return state is null ? Results.NotFound() : Results.Ok(_mapper.ToDto(state));
        });

        group.MapPost("/", async (CreateStateDto dto, CaminoManagerDbContext db) =>
        {
            var state = _mapper.ToEntity(dto);
            db.States.Add(state);
            await db.SaveChangesAsync();
            return Results.Created($"/api/states/{state.Id}", _mapper.ToDto(state));
        });

        group.MapPut("/{id}", async (Guid id, UpdateStateDto dto, CaminoManagerDbContext db) =>
        {
            var state = await db.States.FindAsync(id);
            if (state is null) return Results.NotFound();

            _mapper.UpdateEntity(dto, state);
            await db.SaveChangesAsync();
            return Results.Ok(_mapper.ToDto(state));
        });

        group.MapDelete("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var state = await db.States.FindAsync(id);
            if (state is null) return Results.NotFound();

            db.States.Remove(state);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}