using Bogus;
using CaminoManager.Data.Contexts;
using CaminoManager.Data.Models;
using Microsoft.EntityFrameworkCore;
using Person = CaminoManager.Data.Models.Person;

namespace CaminoManager.MigrationService.Data.Seeders;

public static class PersonSeeder
{
    public static async Task SeedPeopleAsync(CaminoManagerDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.People.AnyAsync(cancellationToken))
            return;

        var genderType = new[] { GenderType.Male, GenderType.Female };

        // Create faker for people
        var personFaker = new Faker<Person>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.PersonName, f => f.Name.FullName())
            .RuleFor(p => p.Phone, f => f.Phone.PhoneNumber("(###) ###-####"))
            .RuleFor(p => p.Mobile, f => f.Phone.PhoneNumber("+1 ###-###-####"))
            .RuleFor(p => p.Email, (f, p) => f.Internet.Email(p.PersonName))
            .RuleFor(p => p.GenderId, f => f.Random.Enum(genderType));

        // Generate 15 married couples (30 people)
        var marriedPairs = new List<(Person Husband, Person Wife)>();
        for (int i = 0; i < 15; i++)
        {
            var husband = personFaker.Generate();
            husband.GenderId = GenderType.Male;
            husband.PersonTypeId = PersonType.Married;

            var wife = personFaker.Generate();
            wife.GenderId = GenderType.Female;
            wife.PersonTypeId = PersonType.Married;

            marriedPairs.Add((husband, wife));
        }

        // Get all married people in a flat list
        var marriedPeople = marriedPairs.SelectMany(pair => new[] { pair.Husband, pair.Wife }).ToList();

        // Generate specific types of single people
        var singlePeople = new List<Person>();

        // 6 seminarians (male)
        for (int i = 0; i < 6; i++)
        {
            var seminarian = personFaker.Generate();
            seminarian.PersonTypeId = PersonType.Seminarian;
            seminarian.GenderId = GenderType.Male;
            singlePeople.Add(seminarian);
        }

        // 3 nuns (female)
        for (int i = 0; i < 3; i++)
        {
            var nun = personFaker.Generate();
            nun.PersonTypeId = PersonType.Nun;
            nun.GenderId = GenderType.Female;
            singlePeople.Add(nun);
        }

        // 2 deacons (male)
        for (int i = 0; i < 2; i++)
        {
            var deacon = personFaker.Generate();
            deacon.PersonTypeId = PersonType.Deacon;
            deacon.GenderId = GenderType.Male;
            singlePeople.Add(deacon);
        }

        // 5 priests (male)
        for (int i = 0; i < 5; i++)
        {
            var priest = personFaker.Generate();
            priest.PersonTypeId = PersonType.Priest;
            priest.GenderId = GenderType.Male;
            singlePeople.Add(priest);
        }

        // 8 widowed (mixed gender)
        for (int i = 0; i < 8; i++)
        {
            var widowed = personFaker.Generate();
            widowed.PersonTypeId = PersonType.Widowed;
            singlePeople.Add(widowed);
        }

        // Remaining singles (36 people to reach 60 singles total)
        for (int i = 0; i < 36; i++)
        {
            var single = personFaker.Generate();
            single.PersonTypeId = PersonType.Single;
            singlePeople.Add(single);
        }

        // First, save all people to the database
        await dbContext.People.AddRangeAsync(marriedPeople.Concat(singlePeople), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Now update the spouse relationships
        foreach (var (husband, wife) in marriedPairs)
        {
            husband.Spouse = wife;
            wife.Spouse = husband;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}