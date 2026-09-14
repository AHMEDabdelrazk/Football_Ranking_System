using FluentAssertions;
using FootballRanking.API.Services;
using FootballRanking.Core.DTOs;
using FootballRanking.Core.Entities;
using FootballRanking.Core.Services;
using FootballRanking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FootballRanking.Tests;

public class PlayerServiceTests
{
    private readonly FootballDbContext _context;
    private readonly RankingEngine _rankingEngine;
    private readonly PlayerService _playerService;

    public PlayerServiceTests()
    {
        var options = new DbContextOptionsBuilder<FootballDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new FootballDbContext(options);
        _rankingEngine = new RankingEngine();
        _playerService = new PlayerService(_context, _rankingEngine);

        SeedSampleData();
    }

    private void SeedSampleData()
    {
        var comp = new Competition { CompetitionId = 1, Name = "Premier League", Code = "PL", CoefficientWeight = 1.00m };
        var club = new Club { ClubId = 1, Name = "Manchester City", CompetitionId = 1, Competition = comp };

        _context.Competitions.Add(comp);
        _context.Clubs.Add(club);

        var striker = new Player
        {
            PlayerId = 1,
            FullName = "Erling Haaland",
            Position = Position.ST,
            Nationality = "Norway",
            ClubId = 1,
            Club = club
        };
        var midfielder = new Player
        {
            PlayerId = 2,
            FullName = "Kevin De Bruyne",
            Position = Position.CAM,
            Nationality = "Belgium",
            ClubId = 1,
            Club = club
        };

        _context.Players.AddRange(striker, midfielder);

        _context.Performances.AddRange(
            new Performance { PerformanceId = 1, PlayerId = 1, Matches = 20, MinutesPlayed = 1800, Goals = 22, Rating = 8.4m },
            new Performance { PerformanceId = 2, PlayerId = 2, Matches = 18, MinutesPlayed = 1500, Goals = 6, Assists = 14, Rating = 8.5m }
        );

        _context.SaveChanges();
    }

    [Fact]
    public async Task GetRankedPlayersAsync_FilterByPosition_ShouldReturnOnlyMatchingPlayers()
    {
        // Arrange
        var filter = new PlayerRankingFilterDto { Position = "ST" };

        // Act
        var result = await _playerService.GetRankedPlayersAsync(filter);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items.First().Name.Should().Be("Erling Haaland");
    }

    [Fact]
    public async Task GetRankedPlayersAsync_SearchQuery_ShouldFilterByName()
    {
        // Arrange
        var filter = new PlayerRankingFilterDto { SearchQuery = "Bruyne" };

        // Act
        var result = await _playerService.GetRankedPlayersAsync(filter);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items.First().Name.Should().Be("Kevin De Bruyne");
    }

    [Fact]
    public async Task GetPlayerRankingDetailsAsync_ExistingId_ShouldReturnCompleteBreakdown()
    {
        // Act
        var details = await _playerService.GetPlayerRankingDetailsAsync(1);

        // Assert
        details.Should().NotBeNull();
        details!.Name.Should().Be("Erling Haaland");
        details.AttackingScore.Should().BeGreaterThan(50m);
        details.ClubName.Should().Be("Manchester City");
    }
}
