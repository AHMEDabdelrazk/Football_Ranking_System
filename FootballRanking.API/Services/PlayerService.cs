using FootballRanking.Core.DTOs;
using FootballRanking.Core.Entities;
using FootballRanking.Core.Interfaces;
using FootballRanking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FootballRanking.API.Services;

public class PlayerService : IPlayerService
{
    private readonly FootballDbContext _context;
    private readonly IRankingEngine _rankingEngine;

    public PlayerService(FootballDbContext context, IRankingEngine rankingEngine)
    {
        _context = context;
        _rankingEngine = rankingEngine;
    }

    public async Task<IEnumerable<PlayerDto>> GetAllPlayersAsync()
    {
        var players = await _context.Players
            .AsNoTracking()
            .Include(p => p.Club)
            .ToListAsync();

        return players.Select(p => new PlayerDto
        {
            Id = p.PlayerId,
            Name = p.FullName,
            Age = DateTime.Today.Year - p.BirthDate.Year,
            Nationality = p.Nationality,
            Position = p.Position.ToString(),
            ClubName = p.Club?.Name ?? "Free Agent"
        });
    }

    public async Task<PlayerDto?> GetPlayerByIdAsync(int id)
    {
        var player = await _context.Players
            .AsNoTracking()
            .Include(p => p.Club)
            .FirstOrDefaultAsync(p => p.PlayerId == id);

        if (player == null)
            return null;

        return new PlayerDto
        {
            Id = player.PlayerId,
            Name = player.FullName,
            Age = DateTime.Today.Year - player.BirthDate.Year,
            Nationality = player.Nationality,
            Position = player.Position.ToString(),
            ClubName = player.Club?.Name ?? "Free Agent"
        };
    }

    public async Task<PlayerRankingDto?> GetPlayerRankingDetailsAsync(int id, RankingWeightsDto? weights = null)
    {
        var player = await _context.Players
            .AsNoTracking()
            .Include(p => p.Club)
                .ThenInclude(c => c.Competition)
            .Include(p => p.Performances)
            .FirstOrDefaultAsync(p => p.PlayerId == id);

        if (player == null)
            return null;

        var perf = player.Performances.OrderByDescending(p => p.Season).FirstOrDefault() ?? new Performance { PlayerId = player.PlayerId };
        var comp = player.Club?.Competition ?? new Competition { Name = "Unknown League", Code = "UNK", CoefficientWeight = 1.0m };

        var dto = _rankingEngine.CalculatePlayerRanking(player, perf, comp, weights);
        return dto;
    }

    public async Task<PagedResult<PlayerRankingDto>> GetRankedPlayersAsync(
        PlayerRankingFilterDto filter, 
        RankingWeightsDto? weights = null)
    {
        var query = _context.Players
            .AsNoTracking()
            .Include(p => p.Club)
                .ThenInclude(c => c.Competition)
            .Include(p => p.Performances)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Position) && Enum.TryParse<Position>(filter.Position, true, out var pos))
        {
            query = query.Where(p => p.Position == pos);
        }

        if (!string.IsNullOrWhiteSpace(filter.CompetitionCode))
        {
            query = query.Where(p => p.Club.Competition.Code == filter.CompetitionCode);
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchQuery))
        {
            var search = filter.SearchQuery.Trim().ToLower();
            query = query.Where(p => p.FullName.ToLower().Contains(search) || p.Club.Name.ToLower().Contains(search) || p.Nationality.ToLower().Contains(search));
        }

        var players = await query.ToListAsync();

        var playerList = players.Select(p =>
        {
            var perf = p.Performances.OrderByDescending(x => x.Season).FirstOrDefault() ?? new Performance { PlayerId = p.PlayerId };
            var comp = p.Club?.Competition ?? new Competition { Name = "Unknown", Code = "UNK", CoefficientWeight = 1.0m };
            return (Player: p, Performance: perf, Competition: comp);
        });

        if (filter.MinMinutes.HasValue)
        {
            playerList = playerList.Where(x => x.Performance.MinutesPlayed >= filter.MinMinutes.Value);
        }

        var rankedList = _rankingEngine.RankPlayers(playerList, weights);

        // Sorting
        var sorted = filter.SortBy.ToLowerInvariant() switch
        {
            "goals" => filter.SortDescending ? rankedList.OrderByDescending(p => p.Goals) : rankedList.OrderBy(p => p.Goals),
            "assists" => filter.SortDescending ? rankedList.OrderByDescending(p => p.Assists) : rankedList.OrderBy(p => p.Assists),
            "attack" or "attackingscore" => filter.SortDescending ? rankedList.OrderByDescending(p => p.AttackingScore) : rankedList.OrderBy(p => p.AttackingScore),
            "playmaking" or "playmakingscore" => filter.SortDescending ? rankedList.OrderByDescending(p => p.PlaymakingScore) : rankedList.OrderBy(p => p.PlaymakingScore),
            "defense" or "defendingscore" => filter.SortDescending ? rankedList.OrderByDescending(p => p.DefendingScore) : rankedList.OrderBy(p => p.DefendingScore),
            "marketvalue" => filter.SortDescending ? rankedList.OrderByDescending(p => p.MarketValueEur) : rankedList.OrderBy(p => p.MarketValueEur),
            _ => filter.SortDescending ? rankedList.OrderBy(p => p.Rank) : rankedList.OrderByDescending(p => p.Rank)
        };

        var allItems = sorted.ToList();
        int totalCount = allItems.Count;
        int page = Math.Max(1, filter.Page);
        int pageSize = Math.Clamp(filter.PageSize, 5, 100);

        var pagedItems = allItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new PagedResult<PlayerRankingDto>
        {
            Items = pagedItems,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<PlayerDto> CreatePlayerAsync(CreatePlayerDto dto)
    {
        if (!Enum.TryParse<Position>(dto.Position, true, out var position))
        {
            throw new ArgumentException("Invalid player position.");
        }

        if (!Enum.TryParse<PreferredFoot>(dto.PreferredFoot, true, out var preferredFoot))
        {
            throw new ArgumentException("Invalid preferred foot.");
        }

        var player = new Player
        {
            FullName = dto.FullName,
            BirthDate = dto.BirthDate,
            Height = dto.Height,
            Weight = dto.Weight,
            Nationality = dto.Nationality,
            Position = position,
            PreferredFoot = preferredFoot,
            ClubId = dto.ClubId
        };

        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        await _context.Entry(player)
            .Reference(p => p.Club)
            .LoadAsync();

        return new PlayerDto
        {
            Id = player.PlayerId,
            Name = player.FullName,
            Age = DateTime.Today.Year - player.BirthDate.Year,
            Nationality = player.Nationality,
            Position = player.Position.ToString(),
            ClubName = player.Club?.Name ?? "Unknown"
        };
    }

    public async Task<bool> DeletePlayerAsync(int id)
    {
        var player = await _context.Players.FindAsync(id);

        if (player == null)
            return false;

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();

        return true;
    }
}