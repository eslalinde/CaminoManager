using CaminoManager.ApiService.Mappers;
using CaminoManager.Data.Contexts;
using CaminoManager.ServiceDefaults.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.ApiService.Endpoints;

public static class CityEndpoints
{
    private static readonly CityMapper _mapper = new();

    public static RouteGroupBuilder MapCityEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/cities").WithTags("Cities");

        group.MapGet("/", async (CaminoManagerDbContext db) =>
        {
            var cities = await db.Cities.ToListAsync();
            return cities.Select(_mapper.ToDto);
        });

        group.MapGet("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var city = await db.Cities.FindAsync(id);
            return city is null ? Results.NotFound() : Results.Ok(_mapper.ToDto(city));
        });

        group.MapPost("/", async (CreateCityDto cityDto, CaminoManagerDbContext db) =>
        {
            var city = _mapper.ToEntity(cityDto);
            db.Cities.Add(city);
            await db.SaveChangesAsync();
            return Results.Created($"/api/cities/{city.Id}", _mapper.ToDto(city));
        });

        group.MapPut("/{id}", async (Guid id, UpdateCityDto cityDto, CaminoManagerDbContext db) =>
        {
            var city = await db.Cities.FindAsync(id);
            if (city is null) return Results.NotFound();

            _mapper.UpdateEntity(cityDto, city);
            await db.SaveChangesAsync();
            return Results.Ok(_mapper.ToDto(city));
        });

        group.MapDelete("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var city = await db.Cities.FindAsync(id);
            if (city is null) return Results.NotFound();

            db.Cities.Remove(city);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return group;
    }
}