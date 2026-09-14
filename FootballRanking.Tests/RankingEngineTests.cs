using FluentAssertions;
using FootballRanking.Core.DTOs;
using FootballRanking.Core.Entities;
using FootballRanking.Core.Services;
using Xunit;

namespace FootballRanking.Tests;

public class RankingEngineTests
{
    private readonly RankingEngine _engine;
    private readonly Competition _premierLeague;
    private readonly Competition _ligue1;
    private readonly Club _manCity;

    public RankingEngineTests()
    {
        _engine = new RankingEngine();
        _premierLeague = new Competition
        {
            CompetitionId = 1,
            Name = "Premier League",
            Code = "PL",
            CoefficientWeight = 1.00m
        };
        _ligue1 = new Competition
        {
            CompetitionId = 5,
            Name = "Ligue 1",
            Code = "FL1",
            CoefficientWeight = 0.92m
        };
        _manCity = new Club
        {
            ClubId = 1,
            Name = "Manchester City",
            Competition = _premierLeague,
            MatchesPlayed = 25,
            Wins = 18,
            Draws = 4,
            Losses = 3,
            GoalsFor = 60,
            GoalsAgainst = 22,
            RecentForm = "W,W,D,W,W"
        };
    }

    [Fact]
    public void CalculatePlayerRanking_StrikerWithGoals_ShouldScoreHighInAttack()
    {
        // Arrange
        var player = new Player
        {
            PlayerId = 1,
            FullName = "Erling Haaland",
            Position = Position.ST,
            Club = _manCity
        };
        var performance = new Performance
        {
            Matches = 20,
            MinutesPlayed = 1800,
            Goals = 20,
            Assists = 3,
            Rating = 8.2m,
            ExpectedGoals = 18.5m,
            Shots = 70,
            ShotsOnTarget = 45,
            SuccessfulDribbles = 15,
            Dribbles = 25,
            PassAccuracy = 75m
        };

        // Act
        var result = _engine.CalculatePlayerRanking(player, performance, _premierLeague);

        // Assert
        result.AttackingScore.Should().BeGreaterThan(75m);
        result.OverallScore.Should().BeGreaterThan(75m);
        result.GoalsPer90.Should().BeApproximately(1.0m, 0.1m);
        result.MinutesDampener.Should().Be(1.0m);
    }

    [Fact]
    public void CalculatePlayerRanking_GoalkeeperWithCleanSheets_ShouldScoreHighInDefense()
    {
        // Arrange
        var player = new Player
        {
            PlayerId = 6,
            FullName = "David Raya",
            Position = Position.GK,
            Club = _manCity
        };
        var performance = new Performance
        {
            Matches = 20,
            MinutesPlayed = 1800,
            Goals = 0,
            Assists = 0,
            CleanSheets = 12,
            Saves = 60,
            GoalsConceded = 14,
            Rating = 7.7m
        };

        // Act
        var result = _engine.CalculatePlayerRanking(player, performance, _premierLeague);

        // Assert
        result.DefendingScore.Should().BeGreaterThan(70m);
        result.AttackingScore.Should().BeLessThan(25m); // GK attack score should stay low
        result.OverallScore.Should().BeGreaterThan(65m);
    }

    [Fact]
    public void CalculatePlayerRanking_PlayerWithLowMinutes_ShouldBeDampenedByMinutesDampener()
    {
        // Arrange
        var player = new Player
        {
            PlayerId = 99,
            FullName = "Wonderkid Sub",
            Position = Position.ST,
            Club = _manCity
        };
        var lowMinutesPerf = new Performance
        {
            Matches = 2,
            MinutesPlayed = 45, // Only 45 minutes played!
            Goals = 2,          // 4 goals per 90!
            Rating = 8.5m
        };

        // Act
        var result = _engine.CalculatePlayerRanking(player, lowMinutesPerf, _premierLeague);

        // Assert
        // Without dampener, 4 goals per 90 would skyrocket rank; dampener must scale it down
        result.MinutesDampener.Should().BeLessThan(0.40m);
        result.OverallScore.Should().BeLessThan(50m);
    }

    [Fact]
    public void CalculatePlayerRanking_LeagueCoefficient_ShouldScaleRankingBetweenLeagues()
    {
        // Arrange
        var player = new Player
        {
            PlayerId = 10,
            FullName = "Elite Attacker",
            Position = Position.LW,
            Club = _manCity
        };
        var performance = new Performance
        {
            Matches = 20,
            MinutesPlayed = 1800,
            Goals = 12,
            Assists = 8,
            Rating = 8.0m,
            ExpectedGoals = 10m,
            KeyPasses = 40,
            SuccessfulDribbles = 40,
            PassAccuracy = 82m
        };

        // Act
        var plResult = _engine.CalculatePlayerRanking(player, performance, _premierLeague);
        var fl1Result = _engine.CalculatePlayerRanking(player, performance, _ligue1);

        // Assert
        plResult.LeagueWeight.Should().Be(1.00m);
        fl1Result.LeagueWeight.Should().Be(0.92m);
        plResult.OverallScore.Should().BeGreaterThan(fl1Result.OverallScore);
    }

    [Fact]
    public void CalculatePlayerRanking_RedCards_ShouldDeductDisciplineScore()
    {
        // Arrange
        var cleanPlayer = new Player { PlayerId = 1, FullName = "Gentleman", Position = Position.CM, Club = _manCity };
        var wildPlayer = new Player { PlayerId = 2, FullName = "Aggressive", Position = Position.CM, Club = _manCity };

        var cleanPerf = new Performance { Matches = 15, MinutesPlayed = 1350, Rating = 7.5m, YellowCards = 0, RedCards = 0 };
        var wildPerf = new Performance { Matches = 15, MinutesPlayed = 1350, Rating = 7.5m, YellowCards = 5, RedCards = 2 };

        // Act
        var cleanResult = _engine.CalculatePlayerRanking(cleanPlayer, cleanPerf, _premierLeague);
        var wildResult = _engine.CalculatePlayerRanking(wildPlayer, wildPerf, _premierLeague);

        // Assert
        cleanResult.DisciplineScore.Should().Be(100m);
        wildResult.DisciplineScore.Should().BeLessThan(60m);
        cleanResult.OverallScore.Should().BeGreaterThan(wildResult.OverallScore);
    }

    [Fact]
    public void CalculateClubRanking_DominantClub_ShouldHaveHighPowerRating()
    {
        // Arrange
        var dominantClub = new Club
        {
            ClubId = 1,
            Name = "Champion FC",
            MatchesPlayed = 25,
            Wins = 22,
            Draws = 2,
            Losses = 1,
            GoalsFor = 70,
            GoalsAgainst = 15,
            RecentForm = "W,W,W,W,W"
        };

        // Act
        var result = _engine.CalculateClubRanking(dominantClub, _premierLeague);

        // Assert
        result.Points.Should().Be(68);
        result.GoalDifference.Should().Be(55);
        result.PowerRating.Should().BeGreaterThan(85m);
        result.AttackRating.Should().BeGreaterThan(80m);
        result.DefenseRating.Should().BeGreaterThan(80m);
    }
}
