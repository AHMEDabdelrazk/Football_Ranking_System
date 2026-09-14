using FootballRanking.Core.DTOs;
using FootballRanking.Core.Entities;

namespace FootballRanking.Core.Interfaces;

public interface IRankingEngine
{
    PlayerRankingDto CalculatePlayerRanking(
        Player player, 
        Performance performance, 
        Competition competition, 
        RankingWeightsDto? weights = null);

    ClubRankingDto CalculateClubRanking(
        Club club, 
        Competition competition, 
        IEnumerable<PlayerRankingDto>? playerRankings = null);

    IEnumerable<PlayerRankingDto> RankPlayers(
        IEnumerable<(Player Player, Performance Performance, Competition Competition)> playerData, 
        RankingWeightsDto? weights = null);

    IEnumerable<ClubRankingDto> RankClubs(
        IEnumerable<(Club Club, Competition Competition, IEnumerable<PlayerRankingDto> PlayerRankings)> clubData);
}
