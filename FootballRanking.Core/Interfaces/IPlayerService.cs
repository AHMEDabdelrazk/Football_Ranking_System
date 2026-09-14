using FootballRanking.Core.DTOs;

namespace FootballRanking.Core.Interfaces;

public interface IPlayerService
{
    Task<IEnumerable<PlayerDto>> GetAllPlayersAsync();

    Task<PlayerDto?> GetPlayerByIdAsync(int id);

    Task<PlayerRankingDto?> GetPlayerRankingDetailsAsync(int id, RankingWeightsDto? weights = null);

    Task<PagedResult<PlayerRankingDto>> GetRankedPlayersAsync(
        PlayerRankingFilterDto filter, 
        RankingWeightsDto? weights = null);

    Task<PlayerDto> CreatePlayerAsync(CreatePlayerDto dto);

    Task<bool> DeletePlayerAsync(int id);
}