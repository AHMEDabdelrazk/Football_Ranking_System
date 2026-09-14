using FootballRanking.Core.DTOs;
using FootballRanking.Core.Interfaces;
using FootballRanking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FootballRanking.API.Services;

public class CompetitionService : ICompetitionService
{
    private readonly FootballDbContext _context;
    private readonly IClubService _clubService;

    public CompetitionService(FootballDbContext context, IClubService clubService)
    {
        _context = context;
        _clubService = clubService;
    }

    public async Task<IEnumerable<CompetitionDto>> GetAllCompetitionsAsync()
    {
        var list = await _context.Competitions
            .AsNoTracking()
            .Include(c => c.Clubs)
            .OrderByDescending(c => c.CoefficientWeight)
            .ToListAsync();

        return list.Select(c => new CompetitionDto
        {
            CompetitionId = c.CompetitionId,
            Name = c.Name,
            Type = c.Type.ToString(),
            Country = c.Country,
            Code = c.Code,
            CoefficientWeight = c.CoefficientWeight,
            Logo = c.Logo,
            ClubCount = c.Clubs.Count
        });
    }

    public async Task<CompetitionDto?> GetCompetitionByCodeAsync(string code)
    {
        var comp = await _context.Competitions
            .AsNoTracking()
            .Include(c => c.Clubs)
            .FirstOrDefaultAsync(c => c.Code == code);

        if (comp == null)
            return null;

        return new CompetitionDto
        {
            CompetitionId = comp.CompetitionId,
            Name = comp.Name,
            Type = comp.Type.ToString(),
            Country = comp.Country,
            Code = comp.Code,
            CoefficientWeight = comp.CoefficientWeight,
            Logo = comp.Logo,
            ClubCount = comp.Clubs.Count
        };
    }

    public async Task<IEnumerable<ClubRankingDto>> GetLeagueStandingsAsync(string code)
    {
        return await _clubService.GetClubRankingsAsync(code);
    }
}
