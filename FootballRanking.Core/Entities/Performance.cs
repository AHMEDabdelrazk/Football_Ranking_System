namespace FootballRanking.Core.Entities;

public class Performance
{
    public int PerformanceId { get; set; }

    public int PlayerId { get; set; }

    public string Season { get; set; } = string.Empty;

    public short Matches { get; set; }

    public short MinutesPlayed { get; set; }

    public short Goals { get; set; }

    public short Assists { get; set; }

    public int Passes { get; set; }

    public decimal PassAccuracy { get; set; }

    public short Shots { get; set; }

    public short ShotsOnTarget { get; set; }

    public short Dribbles { get; set; }

    public short SuccessfulDribbles { get; set; }

    public short Tackles { get; set; }

    public short Interceptions { get; set; }

    public short Clearances { get; set; }

    public byte YellowCards { get; set; }

    public byte RedCards { get; set; }

    public decimal Rating { get; set; }

    // Advanced Analytic Metrics
    public decimal ExpectedGoals { get; set; }

    public decimal ExpectedAssists { get; set; }

    public short KeyPasses { get; set; }

    public short CleanSheets { get; set; }

    public short GoalsConceded { get; set; }

    public short Saves { get; set; }

    public short DuelsWon { get; set; }

    public short ProgressiveCarries { get; set; }

    public Player Player { get; set; } = null!;
}