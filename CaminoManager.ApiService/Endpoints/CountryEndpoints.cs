using CaminoManager.ApiService.Mappers;
using CaminoManager.Data.Contexts;
using CaminoManager.ServiceDefaults.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.ApiService.Endpoints;

public static class CountryEndpoints
{
    public static void MapCountryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/countries").WithTags("Countries");

        var mapper = new CountryMapper();

        group.MapGet("/", async (CaminoManagerDbContext db) =>
        {
            var countries = await db.Countries.ToListAsync();
            return Results.Ok(countries.Select(mapper.ToDto));
        });

        group.MapGet("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var country = await db.Countries.FindAsync(id);
            if (country == null)
                return Results.NotFound();

            return Results.Ok(mapper.ToDto(country));
        });

        group.MapPost("/", async (CreateCountryDto dto, CaminoManagerDbContext db) =>
        {
            var country = mapper.ToEntity(dto);
            db.Countries.Add(country);
            await db.SaveChangesAsync();

            return Results.Created($"/api/countries/{country.Id}", mapper.ToDto(country));
        });

        group.MapPut("/{id}", async (Guid id, UpdateCountryDto dto, CaminoManagerDbContext db) =>
        {
            var country = await db.Countries.FindAsync(id);
            if (country == null)
                return Results.NotFound();

            mapper.UpdateEntity(dto, country);
            await db.SaveChangesAsync();

            return Results.Ok(mapper.ToDto(country));
        });

        group.MapDelete("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var country = await db.Countries.FindAsync(id);
            if (country == null)
                return Results.NotFound();

            db.Countries.Remove(country);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}