using FootballRanking.Core.DTOs;
using FootballRanking.Core.Entities;
using FootballRanking.Core.Interfaces;
using FootballRanking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FootballRanking.API.Services;

public class ClubService : IClubService
{
    private readonly FootballDbContext _context;
    private readonly IRankingEngine _rankingEngine;

    public ClubService(FootballDbContext context, IRankingEngine rankingEngine)
    {
        _context = context;
        _rankingEngine = rankingEngine;
    }

    public async Task<IEnumerable<ClubDto>> GetAllClubsAsync()
    {
        var clubs = await _context.Clubs
            .AsNoTracking()
            .Include(c => c.Competition)
            .Include(c => c.Players)
            .ToListAsync();

        return clubs.Select(c => new ClubDto
        {
            ClubId = c.ClubId,
            Name = c.Name,
            FoundedYear = c.FoundedYear,
            Logo = c.Logo,
            CompetitionId = c.CompetitionId,
            CompetitionName = c.Competition.Name,
            CompetitionCode = c.Competition.Code,
            PlayerCount = c.Players.Count
        });
    }

    public async Task<ClubDto?> GetClubByIdAsync(int id)
    {
        var club = await _context.Clubs
            .AsNoTracking()
            .Include(c => c.Competition)
            .Include(c => c.Players)
            .FirstOrDefaultAsync(c => c.ClubId == id);

        if (club == null)
            return null;

        return new ClubDto
        {
            ClubId = club.ClubId,
            Name = club.Name,
            FoundedYear = club.FoundedYear,
            Logo = club.Logo,
            CompetitionId = club.CompetitionId,
            CompetitionName = club.Competition.Name,
            CompetitionCode = club.Competition.Code,
            PlayerCount = club.Players.Count
        };
    }

    public async Task<IEnumerable<ClubRankingDto>> GetClubRankingsAsync(string? competitionCode = null)
    {
        var query = _context.Clubs
            .AsNoTracking()
            .Include(c => c.Competition)
            .Include(c => c.Players)
                .ThenInclude(p => p.Performances)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(competitionCode))
        {
            query = query.Where(c => c.Competition.Code == competitionCode);
        }

        var clubs = await query.ToListAsync();

        var clubData = clubs.Select(club =>
        {
            var squadRankings = club.Players.Select(p =>
            {
                var perf = p.Performances.OrderByDescending(x => x.Season).FirstOrDefault() ?? new Performance { PlayerId = p.PlayerId };
                return _rankingEngine.CalculatePlayerRanking(p, perf, club.Competition);
            });

            return (Club: club, Competition: club.Competition, PlayerRankings: squadRankings);
        });

        return _rankingEngine.RankClubs(clubData);
    }

    public async Task<IEnumerable<PlayerRankingDto>> GetClubSquadRankingsAsync(int clubId)
    {
        var club = await _context.Clubs
            .AsNoTracking()
            .Include(c => c.Competition)
            .Include(c => c.Players)
                .ThenInclude(p => p.Performances)
            .FirstOrDefaultAsync(c => c.ClubId == clubId);

        if (club == null)
            return Enumerable.Empty<PlayerRankingDto>();

        var playerData = club.Players.Select(p =>
        {
            var perf = p.Performances.OrderByDescending(x => x.Season).FirstOrDefault() ?? new Performance { PlayerId = p.PlayerId };
            return (Player: p, Performance: perf, Competition: club.Competition);
        });

        return _rankingEngine.RankPlayers(playerData);
    }
}
