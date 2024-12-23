using Microsoft.EntityFrameworkCore;
using CaminoManager.Data;
using CaminoManager.Data.Models;
using CaminoManager.ApiService.Mappers;
using CaminoManager.Data.Contexts;
using CaminoManager.ServiceDefaults.Dtos;

namespace CaminoManager.ApiService.Endpoints;

public static class TeamTypeEndpoints
{
    private static readonly TeamTypeMapper _mapper = new();

    public static RouteGroupBuilder MapTeamTypeEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/teamtypes").WithTags("TeamTypes");

        group.MapGet("/", async (CaminoManagerDbContext db) =>
        {
            var teamTypes = await db.TeamTypes.ToListAsync();
            return Results.Ok(teamTypes.Select(_mapper.ToDto));
        });

        group.MapGet("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var teamType = await db.TeamTypes.FindAsync(id);
            if (teamType is null)
                return Results.NotFound();

            return Results.Ok(_mapper.ToDto(teamType));
        });

        group.MapPost("/", async (CreateTeamTypeDto dto, CaminoManagerDbContext db) =>
        {
            var teamType = _mapper.ToModel(dto);
            db.TeamTypes.Add(teamType);
            await db.SaveChangesAsync();

            return Results.Created($"/api/teamtypes/{teamType.Id}", _mapper.ToDto(teamType));
        });

        group.MapPut("/{id}", async (Guid id, UpdateTeamTypeDto dto, CaminoManagerDbContext db) =>
        {
            var teamType = await db.TeamTypes.FindAsync(id);
            if (teamType is null)
                return Results.NotFound();

            _mapper.UpdateModel(dto, teamType);
            await db.SaveChangesAsync();

            return Results.Ok(_mapper.ToDto(teamType));
        });

        group.MapDelete("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var teamType = await db.TeamTypes.FindAsync(id);
            if (teamType is null)
                return Results.NotFound();

            db.TeamTypes.Remove(teamType);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }
} 