using FootballRanking.Core.Entities;
using FootballRanking.Infrastructure.ExternalApi;
using Microsoft.EntityFrameworkCore;

namespace FootballRanking.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(FootballDbContext context)
    {
        // If players already exist, don't seed again
        if (await context.Players.AnyAsync())
            return;

        // Clear existing partial seed if any
        if (await context.Competitions.AnyAsync())
        {
            var existingComps = await context.Competitions.Include(c => c.Clubs).ToListAsync();
            context.Competitions.RemoveRange(existingComps);
            await context.SaveChangesAsync();
        }

        var competitions = new List<Competition>
        {
            new() { Name = "Premier League", Code = "PL", Country = "England", Season = "2024-2025", Type = CompetitionType.League, CoefficientWeight = 1.00m, Logo = "https://media.api-sports.io/football/leagues/39.png" },
            new() { Name = "La Liga", Code = "PD", Country = "Spain", Season = "2024-2025", Type = CompetitionType.League, CoefficientWeight = 0.98m, Logo = "https://media.api-sports.io/football/leagues/140.png" },
            new() { Name = "Serie A", Code = "SA", Country = "Italy", Season = "2024-2025", Type = CompetitionType.League, CoefficientWeight = 0.96m, Logo = "https://media.api-sports.io/football/leagues/135.png" },
            new() { Name = "Bundesliga", Code = "BL1", Country = "Germany", Season = "2024-2025", Type = CompetitionType.League, CoefficientWeight = 0.95m, Logo = "https://media.api-sports.io/football/leagues/78.png" },
            new() { Name = "Ligue 1", Code = "FL1", Country = "France", Season = "2024-2025", Type = CompetitionType.League, CoefficientWeight = 0.92m, Logo = "https://media.api-sports.io/football/leagues/61.png" }
        };

        context.Competitions.AddRange(competitions);
        await context.SaveChangesAsync();

        var pl = competitions[0];
        var laliga = competitions[1];
        var seriea = competitions[2];
        var bundes = competitions[3];
        var ligue1 = competitions[4];

        var clubs = new List<Club>
        {
            // PL
            new() { Name = "Manchester City", CompetitionId = pl.CompetitionId, FoundedYear = 1880, Logo = "https://media.api-sports.io/football/teams/50.png", MatchesPlayed = 28, Wins = 20, Draws = 5, Losses = 3, GoalsFor = 67, GoalsAgainst = 28, RecentForm = "W,W,D,W,W" },
            new() { Name = "Arsenal", CompetitionId = pl.CompetitionId, FoundedYear = 1886, Logo = "https://media.api-sports.io/football/teams/42.png", MatchesPlayed = 28, Wins = 19, Draws = 6, Losses = 3, GoalsFor = 64, GoalsAgainst = 24, RecentForm = "W,W,W,D,W" },
            new() { Name = "Liverpool", CompetitionId = pl.CompetitionId, FoundedYear = 1892, Logo = "https://media.api-sports.io/football/teams/40.png", MatchesPlayed = 28, Wins = 19, Draws = 5, Losses = 4, GoalsFor = 65, GoalsAgainst = 27, RecentForm = "W,D,W,W,L" },
            new() { Name = "Chelsea", CompetitionId = pl.CompetitionId, FoundedYear = 1905, Logo = "https://media.api-sports.io/football/teams/49.png", MatchesPlayed = 28, Wins = 15, Draws = 6, Losses = 7, GoalsFor = 55, GoalsAgainst = 38, RecentForm = "W,L,W,D,W" },

            // La Liga
            new() { Name = "Real Madrid", CompetitionId = laliga.CompetitionId, FoundedYear = 1902, Logo = "https://media.api-sports.io/football/teams/541.png", MatchesPlayed = 27, Wins = 21, Draws = 4, Losses = 2, GoalsFor = 66, GoalsAgainst = 20, RecentForm = "W,W,W,W,D" },
            new() { Name = "Barcelona", CompetitionId = laliga.CompetitionId, FoundedYear = 1899, Logo = "https://media.api-sports.io/football/teams/529.png", MatchesPlayed = 27, Wins = 20, Draws = 3, Losses = 4, GoalsFor = 71, GoalsAgainst = 25, RecentForm = "W,W,W,W,W" },
            new() { Name = "Atletico Madrid", CompetitionId = laliga.CompetitionId, FoundedYear = 1903, Logo = "https://media.api-sports.io/football/teams/530.png", MatchesPlayed = 27, Wins = 17, Draws = 6, Losses = 4, GoalsFor = 48, GoalsAgainst = 21, RecentForm = "W,D,W,L,W" },

            // Serie A
            new() { Name = "Inter Milan", CompetitionId = seriea.CompetitionId, FoundedYear = 1908, Logo = "https://media.api-sports.io/football/teams/505.png", MatchesPlayed = 27, Wins = 21, Draws = 4, Losses = 2, GoalsFor = 68, GoalsAgainst = 18, RecentForm = "W,W,W,D,W" },
            new() { Name = "Juventus", CompetitionId = seriea.CompetitionId, FoundedYear = 1897, Logo = "https://media.api-sports.io/football/teams/496.png", MatchesPlayed = 27, Wins = 16, Draws = 8, Losses = 3, GoalsFor = 47, GoalsAgainst = 22, RecentForm = "D,W,D,W,W" },
            new() { Name = "AC Milan", CompetitionId = seriea.CompetitionId, FoundedYear = 1899, Logo = "https://media.api-sports.io/football/teams/489.png", MatchesPlayed = 27, Wins = 16, Draws = 5, Losses = 6, GoalsFor = 52, GoalsAgainst = 32, RecentForm = "L,W,W,D,W" },

            // Bundesliga
            new() { Name = "Bayer Leverkusen", CompetitionId = bundes.CompetitionId, FoundedYear = 1904, Logo = "https://media.api-sports.io/football/teams/168.png", MatchesPlayed = 25, Wins = 19, Draws = 5, Losses = 1, GoalsFor = 62, GoalsAgainst = 22, RecentForm = "W,W,D,W,W" },
            new() { Name = "Bayern Munich", CompetitionId = bundes.CompetitionId, FoundedYear = 1900, Logo = "https://media.api-sports.io/football/teams/157.png", MatchesPlayed = 25, Wins = 19, Draws = 3, Losses = 3, GoalsFor = 74, GoalsAgainst = 24, RecentForm = "W,W,W,L,W" },
            new() { Name = "Borussia Dortmund", CompetitionId = bundes.CompetitionId, FoundedYear = 1909, Logo = "https://media.api-sports.io/football/teams/165.png", MatchesPlayed = 25, Wins = 14, Draws = 6, Losses = 5, GoalsFor = 49, GoalsAgainst = 31, RecentForm = "W,D,W,W,L" },

            // Ligue 1
            new() { Name = "Paris Saint-Germain", CompetitionId = ligue1.CompetitionId, FoundedYear = 1970, Logo = "https://media.api-sports.io/football/teams/85.png", MatchesPlayed = 25, Wins = 18, Draws = 6, Losses = 1, GoalsFor = 65, GoalsAgainst = 23, RecentForm = "W,W,W,D,W" },
            new() { Name = "Monaco", CompetitionId = ligue1.CompetitionId, FoundedYear = 1924, Logo = "https://media.api-sports.io/football/teams/91.png", MatchesPlayed = 25, Wins = 14, Draws = 5, Losses = 6, GoalsFor = 48, GoalsAgainst = 33, RecentForm = "W,L,W,D,W" }
        };

        context.Clubs.AddRange(clubs);
        await context.SaveChangesAsync();

        var clubMap = clubs.ToDictionary(c => c.Name);

        void CreatePlayer(string name, string nat, string code, Position pos, PreferredFoot foot, string clubName,
                         long value, short matches, short minutes, short goals, short assists, decimal rating,
                         decimal xg, decimal xa, short keyPasses, short cleanSheets, short conceded, short saves,
                         short tackles, short interceptions, short dribbles, short succDribbles, decimal passAcc, short duels)
        {
            var p = new Player
            {
                FullName = name,
                Nationality = nat,
                CountryCode = code,
                BirthDate = new DateOnly(1997, 3, 20),
                Position = pos,
                PreferredFoot = foot,
                ClubId = clubMap[clubName].ClubId,
                MarketValueEur = value,
                Photo = "https://images.unsplash.com/photo-1508098682722-e99c43a406b2?auto=format&fit=crop&w=256&q=80"
            };
            context.Players.Add(p);

            var perf = new Performance
            {
                Player = p,
                Season = "2024-2025",
                Matches = matches,
                MinutesPlayed = minutes,
                Goals = goals,
                Assists = assists,
                Rating = rating,
                ExpectedGoals = xg,
                ExpectedAssists = xa,
                KeyPasses = keyPasses,
                CleanSheets = cleanSheets,
                GoalsConceded = conceded,
                Saves = saves,
                Tackles = tackles,
                Interceptions = interceptions,
                Dribbles = dribbles,
                SuccessfulDribbles = succDribbles,
                PassAccuracy = passAcc,
                DuelsWon = duels,
                Shots = (short)(goals * 4),
                ShotsOnTarget = (short)(goals * 2),
                YellowCards = 2,
                RedCards = 0,
                Clearances = 15,
                ProgressiveCarries = (short)(succDribbles * 2)
            };
            context.Performances.Add(perf);
        }

        // Top 5 League Stars
        CreatePlayer("Erling Haaland", "Norway", "NOR", Position.ST, PreferredFoot.Left, "Manchester City", 180_000_000, 26, 2250, 24, 4, 8.25m, 21.4m, 3.2m, 28, 0, 0, 0, 8, 4, 25, 14, 76.5m, 82);
        CreatePlayer("Kevin De Bruyne", "Belgium", "BEL", Position.CAM, PreferredFoot.Right, "Manchester City", 60_000_000, 20, 1540, 6, 14, 8.42m, 5.1m, 12.8m, 68, 0, 0, 0, 22, 12, 35, 24, 86.4m, 64);
        CreatePlayer("Rodri", "Spain", "ESP", Position.CDM, PreferredFoot.Right, "Manchester City", 130_000_000, 27, 2380, 7, 8, 8.38m, 4.8m, 6.2m, 44, 11, 0, 0, 68, 38, 30, 22, 92.8m, 145);
        CreatePlayer("Bukayo Saka", "England", "ENG", Position.RW, PreferredFoot.Left, "Arsenal", 140_000_000, 27, 2310, 14, 11, 8.18m, 12.1m, 9.4m, 72, 0, 0, 0, 42, 21, 95, 58, 84.1m, 112);
        CreatePlayer("William Saliba", "France", "FRA", Position.CB, PreferredFoot.Right, "Arsenal", 85_000_000, 28, 2520, 2, 1, 8.10m, 1.2m, 0.8m, 12, 14, 22, 0, 54, 32, 12, 9, 93.1m, 138);
        CreatePlayer("David Raya", "Spain", "ESP", Position.GK, PreferredFoot.Right, "Arsenal", 40_000_000, 28, 2520, 0, 0, 7.80m, 0m, 0m, 4, 14, 22, 78, 2, 1, 0, 0, 81.2m, 18);
        CreatePlayer("Mohamed Salah", "Egypt", "EGY", Position.RW, PreferredFoot.Left, "Liverpool", 65_000_000, 27, 2300, 19, 12, 8.35m, 16.8m, 10.5m, 64, 0, 0, 0, 20, 12, 78, 44, 79.5m, 92);
        CreatePlayer("Virgil van Dijk", "Netherlands", "NED", Position.CB, PreferredFoot.Right, "Liverpool", 35_000_000, 27, 2430, 3, 2, 8.08m, 2.5m, 1.1m, 14, 12, 24, 0, 48, 36, 8, 6, 91.5m, 158);
        CreatePlayer("Cole Palmer", "England", "ENG", Position.CAM, PreferredFoot.Left, "Chelsea", 90_000_000, 26, 2150, 18, 9, 8.22m, 15.2m, 8.1m, 58, 0, 0, 0, 28, 14, 68, 42, 83.2m, 86);

        CreatePlayer("Vinicius Junior", "Brazil", "BRA", Position.LW, PreferredFoot.Right, "Real Madrid", 200_000_000, 25, 2120, 18, 11, 8.45m, 15.5m, 9.8m, 62, 0, 0, 0, 24, 15, 120, 72, 81.6m, 118);
        CreatePlayer("Jude Bellingham", "England", "ENG", Position.CAM, PreferredFoot.Right, "Real Madrid", 180_000_000, 24, 2080, 15, 8, 8.36m, 13.2m, 7.1m, 48, 0, 0, 0, 45, 28, 65, 41, 88.3m, 134);
        CreatePlayer("Kylian Mbappe", "France", "FRA", Position.ST, PreferredFoot.Right, "Real Madrid", 180_000_000, 25, 2150, 21, 5, 8.30m, 19.5m, 4.8m, 45, 0, 0, 0, 14, 8, 95, 54, 84.0m, 88);
        CreatePlayer("Lamine Yamal", "Spain", "ESP", Position.RW, PreferredFoot.Left, "Barcelona", 150_000_000, 25, 1980, 9, 13, 8.38m, 8.4m, 11.2m, 65, 0, 0, 0, 32, 18, 110, 68, 82.5m, 102);
        CreatePlayer("Robert Lewandowski", "Poland", "POL", Position.ST, PreferredFoot.Right, "Barcelona", 15_000_000, 26, 2180, 22, 4, 8.28m, 20.1m, 3.5m, 30, 0, 0, 0, 12, 6, 28, 16, 75.8m, 78);
        CreatePlayer("Pedri", "Spain", "ESP", Position.CM, PreferredFoot.Right, "Barcelona", 80_000_000, 23, 1750, 4, 7, 8.15m, 3.2m, 6.5m, 48, 0, 0, 0, 38, 22, 45, 32, 91.2m, 82);
        CreatePlayer("Antoine Griezmann", "France", "FRA", Position.CAM, PreferredFoot.Left, "Atletico Madrid", 25_000_000, 26, 2210, 13, 9, 8.12m, 11.8m, 8.4m, 54, 0, 0, 0, 35, 20, 48, 28, 82.8m, 94);

        CreatePlayer("Lautaro Martinez", "Argentina", "ARG", Position.ST, PreferredFoot.Right, "Inter Milan", 110_000_000, 26, 2240, 21, 5, 8.32m, 18.6m, 4.4m, 38, 0, 0, 0, 22, 14, 45, 26, 78.4m, 95);
        CreatePlayer("Nicolo Barella", "Italy", "ITA", Position.CM, PreferredFoot.Right, "Inter Milan", 80_000_000, 25, 2100, 5, 8, 8.14m, 4.2m, 7.2m, 46, 0, 0, 0, 48, 26, 42, 28, 88.7m, 110);
        CreatePlayer("Alessandro Bastoni", "Italy", "ITA", Position.CB, PreferredFoot.Left, "Inter Milan", 75_000_000, 25, 2180, 2, 4, 8.16m, 1.4m, 3.5m, 22, 12, 17, 0, 52, 34, 18, 14, 91.8m, 128);
        CreatePlayer("Rafael Leao", "Portugal", "POR", Position.LW, PreferredFoot.Right, "AC Milan", 90_000_000, 25, 2020, 11, 9, 8.08m, 9.8m, 8.2m, 52, 0, 0, 0, 20, 10, 115, 65, 80.2m, 98);

        CreatePlayer("Harry Kane", "England", "ENG", Position.ST, PreferredFoot.Right, "Bayern Munich", 100_000_000, 24, 2100, 25, 7, 8.52m, 22.8m, 6.5m, 44, 0, 0, 0, 16, 9, 32, 20, 81.0m, 85);
        CreatePlayer("Jamal Musiala", "Germany", "GER", Position.CAM, PreferredFoot.Right, "Bayern Munich", 130_000_000, 23, 1890, 12, 8, 8.35m, 10.5m, 7.8m, 55, 0, 0, 0, 30, 18, 110, 72, 86.8m, 108);
        CreatePlayer("Florian Wirtz", "Germany", "GER", Position.CAM, PreferredFoot.Right, "Bayer Leverkusen", 130_000_000, 24, 2010, 13, 12, 8.48m, 11.5m, 11.0m, 74, 0, 0, 0, 36, 20, 92, 60, 87.5m, 115);
        CreatePlayer("Granit Xhaka", "Switzerland", "SUI", Position.CDM, PreferredFoot.Left, "Bayer Leverkusen", 20_000_000, 25, 2240, 3, 5, 8.12m, 2.8m, 4.5m, 42, 10, 0, 0, 56, 32, 16, 12, 92.5m, 125);

        CreatePlayer("Ousmane Dembele", "France", "FRA", Position.RW, PreferredFoot.Both, "Paris Saint-Germain", 60_000_000, 23, 1850, 8, 12, 8.20m, 7.5m, 10.8m, 62, 0, 0, 0, 25, 14, 98, 62, 82.2m, 86);
        CreatePlayer("Bradley Barcola", "France", "FRA", Position.LW, PreferredFoot.Right, "Paris Saint-Germain", 65_000_000, 24, 1900, 14, 7, 8.15m, 12.2m, 6.5m, 45, 0, 0, 0, 28, 16, 85, 52, 83.5m, 82);
        CreatePlayer("Achraf Hakimi", "Morocco", "MAR", Position.RB, PreferredFoot.Right, "Paris Saint-Germain", 60_000_000, 23, 1980, 5, 6, 8.10m, 4.5m, 5.8m, 42, 9, 21, 0, 50, 28, 55, 34, 88.0m, 105);
        CreatePlayer("Gianluigi Donnarumma", "Italy", "ITA", Position.GK, PreferredFoot.Right, "Paris Saint-Germain", 40_000_000, 24, 2160, 0, 0, 7.85m, 0m, 0m, 2, 10, 22, 72, 1, 0, 0, 0, 80.5m, 16);

        await context.SaveChangesAsync();
    }
}