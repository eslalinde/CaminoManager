using CaminoManager.Data;
using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.Dtos;
using CaminoManager.ApiService.Mappers;
using Microsoft.EntityFrameworkCore;
using CaminoManager.Data.Contexts;

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
            var parishes = await db.Parishes.ToListAsync();
            return Results.Ok(parishes.Select(_mapper.ToDto));
        });

        group.MapGet("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var parish = await db.Parishes.FindAsync(id);
            if (parish is null)
                return Results.NotFound();

            return Results.Ok(_mapper.ToDto(parish));
        });

        group.MapPost("/", async (CreateParishDto createDto, CaminoManagerDbContext db) =>
        {
            var parish = _mapper.ToEntity(createDto);
            db.Parishes.Add(parish);
            await db.SaveChangesAsync();

            return Results.Created($"/api/parishes/{parish.Id}", _mapper.ToDto(parish));
        });

        group.MapPut("/{id}", async (Guid id, UpdateParishDto updateDto, CaminoManagerDbContext db) =>
        {
            var parish = await db.Parishes.FindAsync(id);
            if (parish is null)
                return Results.NotFound();

            _mapper.UpdateEntity(updateDto, parish);
            await db.SaveChangesAsync();

            return Results.Ok(_mapper.ToDto(parish));
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