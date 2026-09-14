namespace FootballRanking.Core.Entities;

public class Stadium
{
    public int StadiumId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public int ClubId { get; set; }

    public Club Club { get; set; } = null!;
}