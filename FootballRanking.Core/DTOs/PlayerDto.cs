namespace FootballRanking.Core.DTOs;

public class PlayerDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public string Nationality { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;

    public string ClubName { get; set; } = string.Empty;
}