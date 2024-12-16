using CaminoManager.Data.Contexts;
using CaminoManager.Data.Models;
using Microsoft.EntityFrameworkCore;
using CaminoManager.ApiService.DTOs;
using CaminoManager.ApiService.Mappers;

namespace CaminoManager.ApiService.Endpoints;

public static class PersonEndpoints
{
    public static IEndpointRouteBuilder MapPersonEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/people");
        var mapper = new PersonMapper();

        // GET all persons
        group.MapGet("/", async (CaminoManagerDbContext db) =>
            await db.People
                .Select(p => mapper.ToDto(p))
                .ToListAsync());

        // GET person by id
        group.MapGet("/{id}", async (Guid id, CaminoManagerDbContext db) =>
            await db.People.FindAsync(id) is Person person
                ? Results.Ok(mapper.ToDetailDto(person))
                : Results.NotFound());

        // POST new person
        group.MapPost("/", async (PersonDto personDto, CaminoManagerDbContext db) =>
        {
            var person = mapper.ToEntity(personDto);
            db.People.Add(person);
            await db.SaveChangesAsync();
            return Results.Created($"/people/{person.Id}", mapper.ToDto(person));
        });

        // PUT update person
        group.MapPut("/{id}", async (Guid id, PersonDto personDto, CaminoManagerDbContext db) =>
        {
            if (id != personDto.Id) return Results.BadRequest();

            var person = mapper.ToEntity(personDto);
            db.Entry(person).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await db.People.AnyAsync(p => p.Id == id))
                    return Results.NotFound();
                throw;
            }
            return Results.NoContent();
        });

        // DELETE person
        group.MapDelete("/{id}", async (Guid id, CaminoManagerDbContext db) =>
        {
            var person = await db.People.FindAsync(id);
            if (person == null) return Results.NotFound();

            db.People.Remove(person);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return app;
    }
} 