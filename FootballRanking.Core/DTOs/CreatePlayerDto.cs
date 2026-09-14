namespace FootballRanking.Core.DTOs;

public class CreatePlayerDto
{
    public string FullName { get; set; } = string.Empty;

    public DateOnly BirthDate { get; set; }

    public short Height { get; set; }

    public byte Weight { get; set; }

    public string Nationality { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;

    public string PreferredFoot { get; set; } = string.Empty;

    public int ClubId { get; set; }
}