using FootballRanking.Core.DTOs;

namespace FootballRanking.Core.Interfaces;

public class CompetitionDto
{
    public int CompetitionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? Code { get; set; }
    public decimal CoefficientWeight { get; set; }
    public string? Logo { get; set; }
    public int ClubCount { get; set; }
}

public interface ICompetitionService
{
    Task<IEnumerable<CompetitionDto>> GetAllCompetitionsAsync();

    Task<CompetitionDto?> GetCompetitionByCodeAsync(string code);

    Task<IEnumerable<ClubRankingDto>> GetLeagueStandingsAsync(string code);
}
