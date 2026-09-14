using FootballRanking.Core.DTOs;
using FootballRanking.Core.Entities;

namespace FootballRanking.Core.Interfaces;

public class ClubDto
{
    public int ClubId { get; set; }
    public string Name { get; set; } = string.Empty;
    public short? FoundedYear { get; set; }
    public string? Logo { get; set; }
    public int CompetitionId { get; set; }
    public string CompetitionName { get; set; } = string.Empty;
    public string? CompetitionCode { get; set; }
    public int PlayerCount { get; set; }
}

public interface IClubService
{
    Task<IEnumerable<ClubDto>> GetAllClubsAsync();

    Task<ClubDto?> GetClubByIdAsync(int id);

    Task<IEnumerable<ClubRankingDto>> GetClubRankingsAsync(string? competitionCode = null);

    Task<IEnumerable<PlayerRankingDto>> GetClubSquadRankingsAsync(int clubId);
}
