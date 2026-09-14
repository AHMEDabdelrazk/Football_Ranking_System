using System.Numerics;

namespace FootballRanking.Core.Entities;

public class Club
{
    public int ClubId { get; set; }

    public string Name { get; set; } = string.Empty;

    public short? FoundedYear { get; set; }

    public string? Logo { get; set; }

    public short MatchesPlayed { get; set; }
    public short Wins { get; set; }
    public short Draws { get; set; }
    public short Losses { get; set; }
    public short GoalsFor { get; set; }
    public short GoalsAgainst { get; set; }
    public string RecentForm { get; set; } = "W,W,D,W,W";

    // Foreign Key
    public int CompetitionId { get; set; }

    // Navigation Property
    public Competition Competition { get; set; } = null!;

    public Stadium? Stadium { get; set; }

    public ICollection<Player> Players { get; set; } = new List<Player>();

    public ICollection<Website> Websites { get; set; } = new List<Website>();
}