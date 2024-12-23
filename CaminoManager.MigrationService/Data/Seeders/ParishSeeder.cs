using Bogus;
using CaminoManager.Data.Contexts;
using CaminoManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.Data.Seeders;

public static class ParishSeeder
{
    public static async Task SeedParishesAsync(CaminoManagerDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Parishes.AnyAsync(cancellationToken))
            return;

        // Get all cities to distribute parishes among them
        var cities = await dbContext.Cities.ToListAsync(cancellationToken);

        var parishNames = new[]
        {
            "Parroquia La Visitación",
            "Parroquia Santa María de los Ángeles",
            "Parroquia La Balbanera",
            "Parroquia Santuario Niño Jesús de Praga",
            "Parroquia Nuestra Señora del Sagrado Corazón",
            "Parroquia María Madre Admirable",
            "Parroquia El Señor de las Misericordias",
            "Parroquia San José de El Poblado",
            "Parroquia San Joaquín",
            "Parroquia San Cayetano",
            "Parroquia San Juan Bosco",
            "Parroquia San Antonio de Padua",
            "Parroquia Nuestra Señora de Chiquinquirá",
            "Parroquia Nuestra Señora de Belén",
            "Parroquia Nuestra Señora del Rosario",
            "Parroquia Nuestra Señora de la Candelaria",
            "Parroquia Nuestra Señora de Fátima",
            "Parroquia Nuestra Señora de Guadalupe",
            "Parroquia Nuestra Señora de las Lajas",
            "Parroquia Nuestra Señora de Lourdes"
        };

        var faker = new Faker();
        var parishes = new List<Parish>();

        foreach (var name in parishNames)
        {
            // Randomly select a city for each parish
            var city = faker.PickRandom(cities);

            var parish = new Parish
            {
                Id = Guid.NewGuid(),
                Name = name,
                Diocese = $"Diócesis de {city.Name}",
                Address = faker.Address.StreetAddress(),
                Phone = faker.Phone.PhoneNumber("(###) ###-####"),
                Email = faker.Internet.Email(name.Replace("Parroquia ", "").Replace(" ", "").ToLower(),
                                          city.Name.ToLower(),
                                          "iglesia.org"),
                CityId = city.Id
            };

            parishes.Add(parish);
        }

        await dbContext.Parishes.AddRangeAsync(parishes, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}