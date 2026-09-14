using FootballRanking.Core.DTOs;
using FootballRanking.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FootballRanking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RankingsController : ControllerBase
{
    private readonly IPlayerService _playerService;
    private readonly IClubService _clubService;

    public RankingsController(IPlayerService playerService, IClubService clubService)
    {
        _playerService = playerService;
        _clubService = clubService;
    }

    [HttpGet("players")]
    public async Task<IActionResult> GetRankedPlayers(
        [FromQuery] string? position,
        [FromQuery] string? competitionCode,
        [FromQuery] int? minMinutes,
        [FromQuery] string? searchQuery,
        [FromQuery] string sortBy = "OverallScore",
        [FromQuery] bool sortDescending = true,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] decimal attackingWeight = 1.0m,
        [FromQuery] decimal playmakingWeight = 1.0m,
        [FromQuery] decimal defendingWeight = 1.0m,
        [FromQuery] decimal disciplineWeight = 1.0m,
        [FromQuery] int minutesThreshold = 360,
        [FromQuery] bool applyLeagueCoefficient = true)
    {
        var filter = new PlayerRankingFilterDto
        {
            Position = position,
            CompetitionCode = competitionCode,
            MinMinutes = minMinutes,
            SearchQuery = searchQuery,
            SortBy = sortBy,
            SortDescending = sortDescending,
            Page = page,
            PageSize = pageSize
        };

        var weights = new RankingWeightsDto
        {
            AttackingWeight = attackingWeight,
            PlaymakingWeight = playmakingWeight,
            DefendingWeight = defendingWeight,
            DisciplineWeight = disciplineWeight,
            MinutesThreshold = minutesThreshold,
            ApplyLeagueCoefficient = applyLeagueCoefficient
        };

        var result = await _playerService.GetRankedPlayersAsync(filter, weights);
        return Ok(result);
    }

    [HttpPost("players/simulate")]
    public async Task<IActionResult> SimulateRankings([FromBody] SimulateRequest request)
    {
        var filter = request.Filter ?? new PlayerRankingFilterDto();
        var weights = request.Weights ?? new RankingWeightsDto();

        var result = await _playerService.GetRankedPlayersAsync(filter, weights);
        return Ok(result);
    }

    [HttpGet("players/{id}")]
    public async Task<IActionResult> GetPlayerRankingDetails(
        int id, 
        [FromQuery] decimal attackingWeight = 1.0m,
        [FromQuery] decimal playmakingWeight = 1.0m,
        [FromQuery] decimal defendingWeight = 1.0m,
        [FromQuery] decimal disciplineWeight = 1.0m)
    {
        var weights = new RankingWeightsDto
        {
            AttackingWeight = attackingWeight,
            PlaymakingWeight = playmakingWeight,
            DefendingWeight = defendingWeight,
            DisciplineWeight = disciplineWeight
        };

        var details = await _playerService.GetPlayerRankingDetailsAsync(id, weights);
        if (details == null)
            return NotFound(new { message = $"Player with ID {id} not found." });

        return Ok(details);
    }

    [HttpGet("clubs")]
    public async Task<IActionResult> GetClubPowerRankings([FromQuery] string? competitionCode)
    {
        var rankings = await _clubService.GetClubRankingsAsync(competitionCode);
        return Ok(rankings);
    }

    [HttpGet("weights")]
    public IActionResult GetDefaultWeights()
    {
        return Ok(new
        {
            defaultWeights = new RankingWeightsDto(),
            leagueCoefficients = new Dictionary<string, decimal>
            {
                { "PL", 1.00m },
                { "PD", 0.98m },
                { "SA", 0.96m },
                { "BL1", 0.95m },
                { "FL1", 0.92m }
            },
            positions = Enum.GetNames<FootballRanking.Core.Entities.Position>()
        });
    }

    public class SimulateRequest
    {
        public RankingWeightsDto? Weights { get; set; }
        public PlayerRankingFilterDto? Filter { get; set; }
    }
}
