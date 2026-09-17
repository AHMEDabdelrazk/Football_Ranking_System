using FootballRanking.Core.Entities;
using FootballRanking.Core.Interfaces;
using FootballRanking.Infrastructure.ExternalApi;
using Microsoft.EntityFrameworkCore;

namespace FootballRanking.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(FootballDbContext context)
    {
        await SeedAsync(context, new MockFootballDataProvider());
    }

    public static async Task SeedAsync(FootballDbContext context, IFootballDataProvider dataProvider)
    {
        // If data is already populated, skip seeding
        if (await context.Players.AnyAsync())
            return;

        // Clear existing partial data if any
        if (await context.Competitions.AnyAsync())
        {
            var existingComps = await context.Competitions.Include(c => c.Clubs).ToListAsync();
            context.Competitions.RemoveRange(existingComps);
            await context.SaveChangesAsync();
        }

        // 1. Fetch competitions through the injected IFootballDataProvider
        var competitions = (await dataProvider.FetchCompetitionsAsync()).ToList();
        if (!competitions.Any())
        {
            competitions = MockFootballDataProvider.GetInitialCompetitions();
        }

        foreach (var comp in competitions)
        {
            comp.CompetitionId = 0; // Let EF Core identity handle primary key assignment
        }

        context.Competitions.AddRange(competitions);
        await context.SaveChangesAsync();

        // 2. Fetch clubs and players for each competition through IFootballDataProvider
        foreach (var comp in competitions)
        {
            if (string.IsNullOrWhiteSpace(comp.Code)) continue;

            var clubs = (await dataProvider.FetchClubsByCompetitionAsync(comp.Code)).ToList();
            foreach (var club in clubs)
            {
                club.ClubId = 0;
                club.CompetitionId = comp.CompetitionId;
                club.Competition = comp;
            }

            context.Clubs.AddRange(clubs);
            await context.SaveChangesAsync();

            var clubMap = clubs.ToDictionary(c => c.Name, StringComparer.OrdinalIgnoreCase);
            var firstClub = clubs.FirstOrDefault();

            var playerPairs = (await dataProvider.FetchPlayersAndPerformancesAsync(comp.Code)).ToList();
            foreach (var (player, perf) in playerPairs)
            {
                player.PlayerId = 0;
                perf.PerformanceId = 0;

                // Associate player with matching club or first available club in the league
                if (player.Club != null && clubMap.TryGetValue(player.Club.Name, out var matchedClub))
                {
                    player.ClubId = matchedClub.ClubId;
                    player.Club = matchedClub;
                }
                else if (firstClub != null)
                {
                    player.ClubId = firstClub.ClubId;
                    player.Club = firstClub;
                }

                perf.Player = player;
                context.Players.Add(player);
                context.Performances.Add(perf);
            }

            await context.SaveChangesAsync();
        }
    }
}