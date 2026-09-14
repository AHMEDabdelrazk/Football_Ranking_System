using FootballRanking.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FootballRanking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClubsController : ControllerBase
{
    private readonly IClubService _clubService;

    public ClubsController(IClubService clubService)
    {
        _clubService = clubService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clubs = await _clubService.GetAllClubsAsync();
        return Ok(clubs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var club = await _clubService.GetClubByIdAsync(id);
        if (club == null)
            return NotFound(new { message = $"Club with ID {id} not found." });

        return Ok(club);
    }

    [HttpGet("{id}/squad")]
    public async Task<IActionResult> GetSquad(int id)
    {
        var squad = await _clubService.GetClubSquadRankingsAsync(id);
        return Ok(squad);
    }
}
