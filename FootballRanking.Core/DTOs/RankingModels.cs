using FootballRanking.Core.Entities;

namespace FootballRanking.Core.DTOs;

public class RankingWeightsDto
{
    public decimal AttackingWeight { get; set; } = 1.0m;
    public decimal PlaymakingWeight { get; set; } = 1.0m;
    public decimal DefendingWeight { get; set; } = 1.0m;
    public decimal DisciplineWeight { get; set; } = 1.0m;
    public int MinutesThreshold { get; set; } = 360;
    public bool ApplyLeagueCoefficient { get; set; } = true;
}

public class PlayerRankingDto
{
    public int Rank { get; set; }
    public int PlayerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Nationality { get; set; } = string.Empty;
    public string? CountryCode { get; set; }
    public string Position { get; set; } = string.Empty;
    public string ClubName { get; set; } = string.Empty;
    public string? ClubLogo { get; set; }
    public string CompetitionName { get; set; } = string.Empty;
    public string? CompetitionCode { get; set; }
    public long MarketValueEur { get; set; }

    // Aggregate Stats
    public short Matches { get; set; }
    public short MinutesPlayed { get; set; }
    public short Goals { get; set; }
    public short Assists { get; set; }
    public decimal Rating { get; set; }

    // Advanced Metrics Per 90
    public decimal GoalsPer90 { get; set; }
    public decimal AssistsPer90 { get; set; }
    public decimal ExpectedGoalsPer90 { get; set; }
    public decimal KeyPassesPer90 { get; set; }
    public decimal TacklesPer90 { get; set; }
    public decimal InterceptionsPer90 { get; set; }
    public decimal PassAccuracy { get; set; }

    // Algorithmic Scores (Normalized 0 - 100)
    public decimal OverallScore { get; set; }
    public decimal AttackingScore { get; set; }
    public decimal PlaymakingScore { get; set; }
    public decimal DefendingScore { get; set; }
    public decimal DisciplineScore { get; set; }
    public decimal MinutesDampener { get; set; }
    public decimal LeagueWeight { get; set; }

    public int RankChange { get; set; }
}

public class ClubRankingDto
{
    public int Rank { get; set; }
    public int ClubId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public string CompetitionName { get; set; } = string.Empty;
    public string? CompetitionCode { get; set; }

    public short MatchesPlayed { get; set; }
    public short Wins { get; set; }
    public short Draws { get; set; }
    public short Losses { get; set; }
    public short GoalsFor { get; set; }
    public short GoalsAgainst { get; set; }
    public int GoalDifference { get; set; }
    public int Points { get; set; }

    public decimal PowerRating { get; set; }
    public decimal AttackRating { get; set; }
    public decimal DefenseRating { get; set; }
    public List<string> RecentForm { get; set; } = new();
    public decimal SquadAverageScore { get; set; }
}

public class PlayerRankingFilterDto
{
    public string? Position { get; set; }
    public string? CompetitionCode { get; set; }
    public int? MinMinutes { get; set; }
    public string? SearchQuery { get; set; }
    public string SortBy { get; set; } = "OverallScore";
    public bool SortDescending { get; set; } = true;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
