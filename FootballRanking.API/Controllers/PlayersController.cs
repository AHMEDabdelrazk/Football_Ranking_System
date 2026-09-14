using FootballRanking.Core.DTOs;
using FootballRanking.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FootballRanking.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{

    private readonly IPlayerService _service;


    public PlayersController(IPlayerService service)
    {
        _service = service;
    }



    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllPlayersAsync());
    }



    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var player = await _service.GetPlayerByIdAsync(id);


        if (player == null)
            return NotFound();


        return Ok(player);
    }



    [HttpPost]
    public async Task<IActionResult> Create(CreatePlayerDto dto)
    {
        var player = await _service.CreatePlayerAsync(dto);

        return Ok(player);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeletePlayerAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}