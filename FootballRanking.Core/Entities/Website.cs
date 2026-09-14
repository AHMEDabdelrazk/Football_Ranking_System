namespace FootballRanking.Core.Entities;

public class Website
{
    public int WebsiteId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    // Foreign Key
    public int ClubId { get; set; }

    // Navigation Property
    public Club Club { get; set; } = null!;
}