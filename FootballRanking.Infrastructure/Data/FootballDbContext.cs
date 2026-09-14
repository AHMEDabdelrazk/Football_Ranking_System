using FootballRanking.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FootballRanking.Infrastructure.Data;

public class FootballDbContext : DbContext
{
    public FootballDbContext(DbContextOptions<FootballDbContext> options)
        : base(options)
    {
    }

    public DbSet<Competition> Competitions => Set<Competition>();
    public DbSet<Club> Clubs => Set<Club>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Performance> Performances => Set<Performance>();
    public DbSet<Stadium> Stadiums => Set<Stadium>();
    public DbSet<Website> Websites => Set<Website>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Competition>(entity =>
        {
            entity.HasIndex(c => c.Code).IsUnique();
            entity.Property(c => c.CoefficientWeight).HasPrecision(4, 2);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasIndex(p => p.FullName);
            entity.HasIndex(p => p.ClubId);
        });

        modelBuilder.Entity<Performance>(entity =>
        {
            entity.Property(p => p.PassAccuracy).HasPrecision(5, 2);
            entity.Property(p => p.Rating).HasPrecision(4, 2);
            entity.Property(p => p.ExpectedGoals).HasPrecision(5, 2);
            entity.Property(p => p.ExpectedAssists).HasPrecision(5, 2);
        });
    }
}