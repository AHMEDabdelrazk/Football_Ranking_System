namespace FootballRanking.Core.Entities;

public enum PreferredFoot
{
    Left,
    Right,
    Both
}

public enum Position
{
    GK,
    CB,
    LB,
    RB,
    CDM,
    CM,
    CAM,
    LW,
    RW,
    ST
}

public class Player
{
    public int PlayerId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public DateOnly BirthDate { get; set; }

    public short Height { get; set; }

    public byte Weight { get; set; }

    public string Nationality { get; set; } = string.Empty;
    public Position Position { get; set; }

    public PreferredFoot PreferredFoot { get; set; }

    public byte? ShirtNumber { get; set; }

    public string? Photo { get; set; }

    public long MarketValueEur { get; set; }

    public string? CountryCode { get; set; }

    public int ClubId { get; set; }

    public Club Club { get; set; } = null!;

    public ICollection<Performance> Performances { get; set; } = new List<Performance>();
}