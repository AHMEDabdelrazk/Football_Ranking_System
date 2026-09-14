using FootballRanking.Core.Entities;

namespace FootballRanking.Core.Interfaces;

public interface IFootballDataProvider
{
    bool IsLiveApiConfigured { get; }
    string ProviderName { get; }

    Task<IEnumerable<Competition>> FetchCompetitionsAsync();
    Task<IEnumerable<Club>> FetchClubsByCompetitionAsync(string competitionCode);
    Task<IEnumerable<(Player Player, Performance Performance)>> FetchPlayersAndPerformancesAsync(string competitionCode);
}
