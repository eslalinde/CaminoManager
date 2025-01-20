using Bogus;
using CaminoManager.Data.Contexts;
using CaminoManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.MigrationService.Data.Seeders;

public static class CommunitySeeder
{
    public static async Task SeedCommunitiesAsync(CaminoManagerDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Communities.AnyAsync(cancellationToken))
            return;

        // Get all existing people
        var allPeople = await dbContext.People.ToListAsync(cancellationToken);
        var stepWays = await dbContext.StepWays.ToListAsync(cancellationToken);

        // Group married couples
        var marriedPeople = allPeople
            .Where(p => p.PersonTypeId == PersonType.Married && p.Spouse != null)
            .GroupBy(p => new { 
                CoupleId = p.Id < p.Spouse!.Id ? p.Id : p.Spouse.Id })
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
            .RuleFor(c => c.ParishId, f => Guid.NewGuid())
            .RuleFor(c => c.StepWayId, f => f.PickRandom(stepWays).Id)
            .RuleFor(c => c.StepWayDate, f => f.Date.Past(1))
            .RuleFor(c => c.CatechistTeamId, f => Guid.NewGuid())
            .RuleFor(c => c.OldCatechist, f => f.Name.FullName());

        var communities = new List<Community>();
        
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
            var communityPeopleLocal = selectedMarriedCouples
                .Concat([selectedPriest])
                .Concat(selectedSeminarians)
                .Concat(selectedOtherSingles);

            // Create Brother entries for each person
            foreach (var person in communityPeopleLocal)
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

        // Add Responsible team for the first community
        var firstCommunity = communities[0];
        var teamTypeResponsible = await dbContext.TeamTypes.FirstAsync(tt => tt.Name == "Responsables", cancellationToken);
        
        // Get people from the first community
        var communityPeople = firstCommunity.Brothers.Select(b => b.Person).ToList();
        var marriedCouples = communityPeople
            .Where(p => p.PersonTypeId == PersonType.Married && p.Spouse != null)
            .GroupBy(p => new { 
                CoupleId = p.Id < p.Spouse!.Id ? p.Id : p.Spouse.Id })
            .Select(g => g.ToList())
            .Take(2)
            .ToList();
        
        var singles = communityPeople
            .Where(p => p.PersonTypeId != PersonType.Married)
            .Take(3)
            .ToList();

        // Create the Responsible team
        var responsibleTeam = new Team
        {
            Id = Guid.NewGuid(),
            Name = "Responsables",
            TeamTypeId = teamTypeResponsible.Id,
            CommunityId = firstCommunity.Id
        };

        await dbContext.Teams.AddAsync(responsibleTeam, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Create Belong records for team members
        var belongs = new List<Belong>();
        
        // First married couple with IsResponsibleForTheTeam = true
        belongs.AddRange(marriedCouples[0].Select(person => new Belong
        {
            PersonId = person.Id,
            CommunityId = firstCommunity.Id,
            TeamId = responsibleTeam.Id,
            IsResponsibleForTheTeam = true
        }));

        // Second married couple and singles with IsResponsibleForTheTeam = false
        belongs.AddRange(marriedCouples[1].Concat(singles).Select(person => new Belong
        {
            PersonId = person.Id,
            CommunityId = firstCommunity.Id,
            TeamId = responsibleTeam.Id,
            IsResponsibleForTheTeam = false
        }));

        await dbContext.Belongs.AddRangeAsync(belongs, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Add responsible team for the second community
        var secondCommunity = communities.Skip(1).First();
        
        // Get people from the second community
        var secondCommunityPeople = secondCommunity.Brothers.Select(b => b.Person).ToList();
        
        // Get single women
        var singleWoman = secondCommunityPeople
            .First(p => p.PersonTypeId != PersonType.Married 
                     && p.PersonTypeId != PersonType.Priest 
                     && p.PersonTypeId != PersonType.Seminarian
                     && p.GenderId == GenderType.Female);

        // Get married couples
        var secondCommunityMarriedCouples = secondCommunityPeople
            .Where(p => p.PersonTypeId == PersonType.Married && p.Spouse != null)
            .GroupBy(p => new { 
                CoupleId = p.Id < p.Spouse!.Id ? p.Id : p.Spouse.Id })
            .Select(g => g.ToList())
            .Take(2)
            .ToList();

        // Get one more single person (different from the responsible woman)
        var additionalSingle = secondCommunityPeople
            .First(p => p.PersonTypeId != PersonType.Married
                     && p.Id != singleWoman.Id);
            
        // Create the responsible team for second community
        var secondResponsibleTeam = new Team
        {
            Id = Guid.NewGuid(),
            Name = "Responsables",
            TeamTypeId = teamTypeResponsible.Id,
            CommunityId = secondCommunity.Id
        };

        await dbContext.Teams.AddAsync(secondResponsibleTeam, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Create Belong records for second community team
        var secondCommunityBelongs = new List<Belong>
        {
            // Single woman as responsible
            new() {
                PersonId = singleWoman.Id,
                CommunityId = secondCommunity.Id,
                TeamId = secondResponsibleTeam.Id,
                IsResponsibleForTheTeam = true
            }
        };

        // Two married couples and additional single with IsResponsibleForTheTeam = false
        secondCommunityBelongs.AddRange(
            secondCommunityMarriedCouples
                .SelectMany(couple => couple)
                .Concat([additionalSingle])
                .Select(person => new Belong
                {
                    PersonId = person.Id,
                    CommunityId = secondCommunity.Id,
                    TeamId = secondResponsibleTeam.Id,
                    IsResponsibleForTheTeam = false
                }));

        await dbContext.Belongs.AddRangeAsync(secondCommunityBelongs, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Get Catequistas TeamType
        var teamTypeCatequistas = await dbContext.TeamTypes.FirstAsync(tt => tt.Name == "Catequistas", cancellationToken);
        
        // Get available people from first community (excluding those already selected in marriedCouples)
        var availableMarriedCouples = firstCommunity.Brothers
            .Select(b => b.Person)
            .Where(p => p.PersonTypeId == PersonType.Married && p.Spouse != null)
            .Where(p => !marriedCouples.SelectMany(c => c).Select(p => p.Id).Contains(p.Id))
            .GroupBy(p => new {
                CoupleId = p.Id < p.Spouse!.Id ? p.Id : p.Spouse.Id
            })
            .Select(g => g.ToList())
            .First();

        // Get one priest
        var priest = firstCommunity.Brothers
            .Select(b => b.Person)
            .First(p => p.PersonTypeId == PersonType.Priest);

        // Get 2 singles (excluding those in the responsible team)
        var additionalSingles = firstCommunity.Brothers
            .Select(b => b.Person)
            .Where(p => p.PersonTypeId != PersonType.Married 
                     && p.PersonTypeId != PersonType.Priest
                     && !singles.Select(s => s.Id).Contains(p.Id))
            .Take(2)
            .ToList();

        // Create the Catequistas team
        var catequistasTeam = new Team
        {
            Id = Guid.NewGuid(),
            Name = "Catequistas",
            TeamTypeId = teamTypeCatequistas.Id,
            CommunityId = firstCommunity.Id
        };

        await dbContext.Teams.AddAsync(catequistasTeam, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Create Belong records for Catequistas team
        var catequistasBelongs = new List<Belong>();
        
        // First married couple (same as responsible) with IsResponsibleForTheTeam = true
        catequistasBelongs.AddRange(marriedCouples[0].Select(person => new Belong
        {
            PersonId = person.Id,
            CommunityId = firstCommunity.Id,
            TeamId = catequistasTeam.Id,
            IsResponsibleForTheTeam = true
        }));

        // Additional married couple, priest, and singles with IsResponsibleForTheTeam = false
        catequistasBelongs.AddRange(
            availableMarriedCouples
                .Concat([priest])
                .Concat(additionalSingles)
                .Select(person => new Belong
                {
                    PersonId = person.Id,
                    CommunityId = firstCommunity.Id,
                    TeamId = catequistasTeam.Id,
                    IsResponsibleForTheTeam = false
                }));

        await dbContext.Belongs.AddRangeAsync(catequistasBelongs, cancellationToken);

        // Get a random parish and create ParishTeam record
        var randomParish = await dbContext.Parishes
            .OrderBy(p => Guid.NewGuid())
            .FirstAsync(cancellationToken);

        var parishTeam = new ParishTeam
        {
            ParishId = randomParish.Id,
            TeamId = catequistasTeam.Id
        };

        await dbContext.ParishTeams.AddAsync(parishTeam, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}