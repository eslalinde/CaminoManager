using Bogus;
using CaminoManager.Data.Contexts;
using CaminoManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.Data.Seeders;

public static class CommunitySeeder
{
    public static async Task SeedCommunitiesAsync(CaminoManagerDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Communities.AnyAsync(cancellationToken))
            return;

        // Get all existing people
        var allPeople = await dbContext.People.ToListAsync(cancellationToken);
        
        // Group married couples
        var marriedPeople = allPeople.Where(p => p.PersonTypeId == PersonType.Married)
                                   .GroupBy(p => p.Spouse?.Id)
                                   .Where(g => g.Count() == 2)
                                   .Select(g => g.ToList())
                                   .ToList();

        var priests = allPeople.Where(p => p.PersonTypeId == PersonType.Priest).ToList();
        var seminarians = allPeople.Where(p => p.PersonTypeId == PersonType.Seminarian).ToList();
        var otherSingles = allPeople.Where(p => p.PersonTypeId != PersonType.Married 
                                            && p.PersonTypeId != PersonType.Priest 
                                            && p.PersonTypeId != PersonType.Seminarian)
                                   .ToList();

        // Create faker for communities
        var communityFaker = new Faker<Community>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(c => c.Number, f => f.Random.Replace("C##"))
            .RuleFor(c => c.BornDate, f => f.Date.Past(20))
            .RuleFor(c => c.ParishId, f => Guid.NewGuid()) // You might want to link this to actual parishes
            .RuleFor(c => c.StepWayId, f => Guid.NewGuid())
            .RuleFor(c => c.StepWayDate, f => f.Date.Past(1))
            .RuleFor(c => c.CatechistTeamId, f => Guid.NewGuid())
            .RuleFor(c => c.OldCatechist, f => f.Name.FullName());

        var communities = new List<Community>();
        var random = new Random();

        // Create 3 communities
        for (int i = 0; i < 3; i++)
        {
            var community = communityFaker.Generate();
            
            // Assign 10 married couples (20 people)
            var selectedMarriedCouples = marriedPeople.Skip(i * 10).Take(10).SelectMany(x => x).ToList();
            
            // Assign 1 priest
            var selectedPriest = priests[i];
            
            // Assign 2 seminarians
            var selectedSeminarians = seminarians.Skip(i * 2).Take(2).ToList();
            
            // Calculate how many more people we need (should be 7)
            var remainingCount = 30 - (selectedMarriedCouples.Count + 1 + 2);
            
            // Select random other singles
            var selectedOtherSingles = otherSingles
                .Skip(i * remainingCount)
                .Take(remainingCount)
                .ToList();

            // Combine all selected people
            var communityPeople = selectedMarriedCouples
                .Concat(new[] { selectedPriest })
                .Concat(selectedSeminarians)
                .Concat(selectedOtherSingles);

            // Create Brother entries for each person
            foreach (var person in communityPeople)
            {
                community.Brothers.Add(new Brother
                {
                    PersonId = person.Id,
                    CommunityId = community.Id,
                    Person = person
                });
            }

            community.BornBrothers = 30;
            community.ActualBrothers = 30;
            
            communities.Add(community);
        }

        await dbContext.Communities.AddRangeAsync(communities, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
} 