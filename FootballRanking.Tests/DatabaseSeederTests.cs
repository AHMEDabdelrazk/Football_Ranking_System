using FluentAssertions;
using FootballRanking.Core.Entities;
using FootballRanking.Core.Interfaces;
using FootballRanking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace FootballRanking.Tests;

public class DatabaseSeederTests
{
    [Fact]
    public async Task SeedAsync_WithMockDataProvider_ShouldInvokeProviderAndPersistEntities()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<FootballDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new FootballDbContext(options);

        var mockProvider = new Mock<IFootballDataProvider>();

        var testComp = new Competition
        {
            Name = "Premier League",
            Code = "PL",
            Country = "England",
            CoefficientWeight = 1.0m,
            Season = "2024-2025"
        };

        var testClub = new Club
        {
            Name = "Manchester City",
            MatchesPlayed = 25,
            Wins = 18,
            Draws = 4,
            Losses = 3,
            GoalsFor = 60,
            GoalsAgainst = 22
        };

        var testPlayer = new Player
        {
            FullName = "Erling Haaland",
            Position = Position.ST,
            Nationality = "Norway",
            CountryCode = "NOR",
            Club = testClub
        };

        var testPerf = new Performance
        {
            Season = "2024-2025",
            Matches = 25,
            MinutesPlayed = 2100,
            Goals = 24,
            Rating = 8.3m
        };

        mockProvider
            .Setup(p => p.FetchCompetitionsAsync())
            .ReturnsAsync(new List<Competition> { testComp });

        mockProvider
            .Setup(p => p.FetchClubsByCompetitionAsync("PL"))
            .ReturnsAsync(new List<Club> { testClub });

        mockProvider
            .Setup(p => p.FetchPlayersAndPerformancesAsync("PL"))
            .ReturnsAsync(new List<(Player, Performance)> { (testPlayer, testPerf) });

        // Act
        await DatabaseSeeder.SeedAsync(context, mockProvider.Object);

        // Assert
        mockProvider.Verify(p => p.FetchCompetitionsAsync(), Times.Once);
        mockProvider.Verify(p => p.FetchClubsByCompetitionAsync("PL"), Times.Once);
        mockProvider.Verify(p => p.FetchPlayersAndPerformancesAsync("PL"), Times.Once);

        context.Competitions.Should().ContainSingle(c => c.Code == "PL");
        context.Clubs.Should().ContainSingle(c => c.Name == "Manchester City");
        context.Players.Should().ContainSingle(p => p.FullName == "Erling Haaland");
    }

    [Fact]
    public async Task SeedAsync_WhenDatabaseAlreadySeeded_ShouldNotReinvokeProvider()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<FootballDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new FootballDbContext(options);

        // Pre-populate with a player
        var existingComp = new Competition { Name = "Existing", Code = "EXT" };
        var existingClub = new Club { Name = "Existing FC", Competition = existingComp };
        var existingPlayer = new Player { FullName = "Existing Star", Club = existingClub, Position = Position.CM };
        context.Players.Add(existingPlayer);
        await context.SaveChangesAsync();

        var mockProvider = new Mock<IFootballDataProvider>();

        // Act
        await DatabaseSeeder.SeedAsync(context, mockProvider.Object);

        // Assert
        mockProvider.Verify(p => p.FetchCompetitionsAsync(), Times.Never);
    }
}
