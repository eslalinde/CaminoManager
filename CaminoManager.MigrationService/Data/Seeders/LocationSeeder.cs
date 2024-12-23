using CaminoManager.Data.Contexts;
using CaminoManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.Data.Seeders;

public static class LocationSeeder
{
    public static async Task SeedLocationsAsync(CaminoManagerDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Countries.AnyAsync(cancellationToken))
            return;

        var colombia = new Country
        {
            Id = Guid.NewGuid(),
            Name = "Colombia",
            Code = "CO"
        };

        await dbContext.Countries.AddAsync(colombia, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var states = new[]
        {
            (name: "Amazonas", capital: "Leticia"),
            (name: "Antioquia", capital: "Medellín"),
            (name: "Arauca", capital: "Arauca"),
            (name: "Atlántico", capital: "Barranquilla"),
            (name: "Bolívar", capital: "Cartagena de Indias"),
            (name: "Boyacá", capital: "Tunja"),
            (name: "Caldas", capital: "Manizales"),
            (name: "Caquetá", capital: "Florencia"),
            (name: "Casanare", capital: "Yopal"),
            (name: "Cauca", capital: "Popayán"),
            (name: "Cesar", capital: "Valledupar"),
            (name: "Chocó", capital: "Quibdó"),
            (name: "Córdoba", capital: "Montería"),
            (name: "Cundinamarca", capital: "Bogotá"),
            (name: "Guainía", capital: "Inírida"),
            (name: "Guaviare", capital: "San José del Guaviare"),
            (name: "Huila", capital: "Neiva"),
            (name: "La Guajira", capital: "Riohacha"),
            (name: "Magdalena", capital: "Santa Marta"),
            (name: "Meta", capital: "Villavicencio"),
            (name: "Nariño", capital: "Pasto"),
            (name: "Norte de Santander", capital: "Cúcuta"),
            (name: "Putumayo", capital: "Mocoa"),
            (name: "Quindío", capital: "Armenia"),
            (name: "Risaralda", capital: "Pereira"),
            (name: "San Andrés y Providencia", capital: "San Andrés"),
            (name: "Santander", capital: "Bucaramanga"),
            (name: "Sucre", capital: "Sincelejo"),
            (name: "Tolima", capital: "Ibagué"),
            (name: "Valle del Cauca", capital: "Cali"),
            (name: "Vaupés", capital: "Mitú"),
            (name: "Vichada", capital: "Puerto Carreño")
        };

        foreach (var (stateName, capital) in states)
        {
            var state = new State
            {
                Id = Guid.NewGuid(),
                Name = stateName,
                CountryId = colombia.Id
            };

            await dbContext.States.AddAsync(state, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var city = new City
            {
                Id = Guid.NewGuid(),
                Name = capital,
                StateId = state.Id,
                CountryId = colombia.Id
            };

            await dbContext.Cities.AddAsync(city, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
} 