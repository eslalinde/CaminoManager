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

    public DbSet<Person> People { get; set; } = null!;
    public DbSet<Brother> Brothers { get; set; } = null!;
    public DbSet<Community> Communities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("People");
            entity.HasKey(e => e.Id);
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
    }
}