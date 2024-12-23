using CaminoManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CaminoManager.Data.Contexts;

public class CaminoManagerDbContext : DbContext
{
    public CaminoManagerDbContext(DbContextOptions<CaminoManagerDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<Belong> Belongs { get; set; } = null!;
    public DbSet<Brother> Brothers { get; set; } = null!;
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<Community> Communities { get; set; } = null!;
    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<Parish> Parishes { get; set; } = null!;
    public DbSet<ParishTeam> ParishTeams { get; set; } = null!;
    public DbSet<Person> People { get; set; } = null!;
    public DbSet<Priest> Priests { get; set; } = null!;
    public DbSet<State> States { get; set; } = null!;
    public DbSet<StepWay> StepWays { get; set; } = null!;
    public DbSet<Team> Teams { get; set; } = null!;
    public DbSet<TeamType> TeamTypes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("People");
            entity.HasKey(e => e.Id);

#pragma warning disable CS8603 // Possible null reference return.
            entity.HasGeneratedTsVectorColumn(
                p => p.SearchVector,
                "spanish",  // or your preferred language
                p => p.PersonName)
                .HasIndex(p => p.SearchVector)
                .HasMethod("GIN"); // GIN index is recommended for full-text search
#pragma warning restore CS8603 // Possible null reference return.
        });

        modelBuilder.Entity<Brother>(entity =>
        {
            entity.ToTable("Brothers");
            entity.HasKey(e => new { e.PersonId, e.CommunityId });

            entity.HasOne(e => e.Person)
                .WithMany(p => p.Brothers)
                .HasForeignKey(e => e.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Community)
                .WithMany(c => c.Brothers)
                .HasForeignKey(e => e.CommunityId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Community>(entity =>
        {
            entity.ToTable("Communities");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Number)
                .IsRequired();

            entity.Property(e => e.BornDate)
                .IsRequired();

            entity.Property(e => e.ParishId)
                .IsRequired();

            entity.Property(e => e.StepWayId)
                .IsRequired();

            entity.Property(e => e.BornBrothers)
                .IsRequired();

            entity.Property(e => e.ActualBrothers)
                .IsRequired();

            // Optional properties
            entity.Property(e => e.StepWayDate)
                .IsRequired(false);

            entity.Property(e => e.CatechistTeamId)
                .IsRequired(false);

            entity.Property(e => e.OldCatechist)
                .IsRequired(false);
        });

        modelBuilder.Entity<Belong>(entity =>
        {
            entity.ToTable("Belongs");
            entity.HasKey(e => new { e.PersonId, e.CommunityId, e.TeamId });

            // Person relationship
            entity.HasOne(e => e.Person)
                .WithMany()
                .HasForeignKey(e => e.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            // Community relationship
            entity.HasOne(e => e.Community)
                .WithMany()
                .HasForeignKey(e => e.CommunityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Team relationship
            entity.HasOne(e => e.Team)
                .WithMany()
                .HasForeignKey(e => e.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure relationships
        modelBuilder.Entity<City>()
            .HasOne(c => c.State)
            .WithMany(s => s.Cities)
            .HasForeignKey(c => c.StateId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<City>()
            .HasOne(c => c.Country)
            .WithMany(co => co.Cities)
            .HasForeignKey(c => c.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<State>()
            .HasOne(s => s.Country)
            .WithMany(c => c.States)
            .HasForeignKey(s => s.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StepWay>(entity =>
        {
            entity.ToTable("StepWays");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired();

            entity.Property(e => e.Order)
                .IsRequired();
        });

        modelBuilder.Entity<Priest>(entity =>
        {
            entity.ToTable("Priests");
            entity.HasKey(e => e.Id);

            entity.HasOne(p => p.Person)
                .WithMany()
                .HasForeignKey(p => p.PersonId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.ToTable("Teams");
            entity.HasKey(e => e.Id);

            entity.HasOne(t => t.TeamType)
                .WithMany(tt => tt.Teams)
                .HasForeignKey(t => t.TeamTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Community)
                .WithMany()
                .HasForeignKey(t => t.CommunityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Name)
                .IsRequired();
        });

        modelBuilder.Entity<TeamType>(entity =>
        {
            entity.ToTable("TeamTypes");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired();

            entity.Property(e => e.Order)
                .IsRequired();
        });

        modelBuilder.Entity<Parish>(entity =>
        {
            entity.ToTable("Parishes");
            entity.HasKey(e => e.Id);

            entity.HasOne(p => p.City)
                .WithMany()
                .HasForeignKey(p => p.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Name)
                .IsRequired();

            entity.Property(e => e.Diocese)
                .IsRequired();

            entity.Property(e => e.Address)
                .IsRequired();
        });

        modelBuilder.Entity<ParishTeam>(entity =>
        {
            entity.ToTable("ParishTeams");
            entity.HasKey(e => new { e.ParishId, e.TeamId });

            entity.HasOne(pt => pt.Parish)
                .WithMany(p => p.ParishTeams)
                .HasForeignKey(pt => pt.ParishId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(pt => pt.Team)
                .WithMany(t => t.ParishTeams)
                .HasForeignKey(pt => pt.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}