using CaminoManager.ApiService.Mappers;
using CaminoManager.Data.Contexts;
using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.ApiService.Endpoints;

public static class PersonEndpoints
{
    public static IEndpointRouteBuilder MapPersonEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/people").WithTags("People");
        var mapper = new PersonMapper();

        // GET all persons with optional personType filter
        group.MapGet("/", async (int? personType, CaminoManagerDbContext db) =>
        {
            var query = db.People.AsQueryable();
            
            if (personType.HasValue)
            {
                query = query.Where(p => (int)p.PersonTypeId == personType.Value);
            }

            return await query
                .Select(p => mapper.ToDto(p))
                .ToListAsync();
        });

        // GET person by id
        group.MapGet("/{id}", async (string id, CaminoManagerDbContext db) =>
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
        group.MapPut("/{id}", async (string id, PersonDto personDto, CaminoManagerDbContext db) =>
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
                if (!await db.People.AnyAsync(p => p.Id == Guid.Parse(id)))
                    return Results.NotFound();
                throw;
            }
            return Results.NoContent();
        });

        // DELETE person
        group.MapDelete("/{id}", async (string id, CaminoManagerDbContext db) =>
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