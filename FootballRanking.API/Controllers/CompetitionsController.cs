using FootballRanking.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FootballRanking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompetitionsController : ControllerBase
{
    private readonly ICompetitionService _competitionService;

    public CompetitionsController(ICompetitionService competitionService)
    {
        _competitionService = competitionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var competitions = await _competitionService.GetAllCompetitionsAsync();
        return Ok(competitions);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        var competition = await _competitionService.GetCompetitionByCodeAsync(code);
        if (competition == null)
            return NotFound(new { message = $"Competition with code '{code}' not found." });

        return Ok(competition);
    }

    [HttpGet("{code}/standings")]
    public async Task<IActionResult> GetStandings(string code)
    {
        var standings = await _competitionService.GetLeagueStandingsAsync(code);
        return Ok(standings);
    }
}
