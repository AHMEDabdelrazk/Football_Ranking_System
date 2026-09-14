using FootballRanking.Core.Entities;
using FootballRanking.Core.Interfaces;

namespace FootballRanking.Infrastructure.ExternalApi;

public class MockFootballDataProvider : IFootballDataProvider
{
    public bool IsLiveApiConfigured => false;
    public string ProviderName => "Top5Leagues-HighFidelity-Mock-Provider";

    public Task<IEnumerable<Competition>> FetchCompetitionsAsync()
    {
        var competitions = new List<Competition>
        {
            new() { CompetitionId = 1, Name = "Premier League", Code = "PL", Country = "England", Season = "2024-2025", Type = CompetitionType.League, CoefficientWeight = 1.00m, Logo = "https://media.api-sports.io/football/leagues/39.png" },
            new() { CompetitionId = 2, Name = "La Liga", Code = "PD", Country = "Spain", Season = "2024-2025", Type = CompetitionType.League, CoefficientWeight = 0.98m, Logo = "https://media.api-sports.io/football/leagues/140.png" },
            new() { CompetitionId = 3, Name = "Serie A", Code = "SA", Country = "Italy", Season = "2024-2025", Type = CompetitionType.League, CoefficientWeight = 0.96m, Logo = "https://media.api-sports.io/football/leagues/135.png" },
            new() { CompetitionId = 4, Name = "Bundesliga", Code = "BL1", Country = "Germany", Season = "2024-2025", Type = CompetitionType.League, CoefficientWeight = 0.95m, Logo = "https://media.api-sports.io/football/leagues/78.png" },
            new() { CompetitionId = 5, Name = "Ligue 1", Code = "FL1", Country = "France", Season = "2024-2025", Type = CompetitionType.League, CoefficientWeight = 0.92m, Logo = "https://media.api-sports.io/football/leagues/61.png" }
        };

        return Task.FromResult<IEnumerable<Competition>>(competitions);
    }

    public Task<IEnumerable<Club>> FetchClubsByCompetitionAsync(string competitionCode)
    {
        var clubs = GetAllClubs().Where(c => c.Competition.Code?.Equals(competitionCode, StringComparison.OrdinalIgnoreCase) == true);
        return Task.FromResult(clubs);
    }

    public Task<IEnumerable<(Player Player, Performance Performance)>> FetchPlayersAndPerformancesAsync(string competitionCode)
    {
        var list = GetAllPlayersAndPerformances()
            .Where(p => p.Player.Club?.Competition?.Code?.Equals(competitionCode, StringComparison.OrdinalIgnoreCase) == true);
        return Task.FromResult(list);
    }

    public static List<Competition> GetInitialCompetitions()
    {
        return new List<Competition>
        {
            new() { CompetitionId = 1, Name = "Premier League", Code = "PL", Country = "England", Season = "2024-2025", Type = CompetitionType.League, CoefficientWeight = 1.00m, Logo = "https://media.api-sports.io/football/leagues/39.png" },
            new() { CompetitionId = 2, Name = "La Liga", Code = "PD", Country = "Spain", Season = "2024-2025", Type = CompetitionType.League, CoefficientWeight = 0.98m, Logo = "https://media.api-sports.io/football/leagues/140.png" },
            new() { CompetitionId = 3, Name = "Serie A", Code = "SA", Country = "Italy", Season = "2024-2025", Type = CompetitionType.League, CoefficientWeight = 0.96m, Logo = "https://media.api-sports.io/football/leagues/135.png" },
            new() { CompetitionId = 4, Name = "Bundesliga", Code = "BL1", Country = "Germany", Season = "2024-2025", Type = CompetitionType.League, CoefficientWeight = 0.95m, Logo = "https://media.api-sports.io/football/leagues/78.png" },
            new() { CompetitionId = 5, Name = "Ligue 1", Code = "FL1", Country = "France", Season = "2024-2025", Type = CompetitionType.League, CoefficientWeight = 0.92m, Logo = "https://media.api-sports.io/football/leagues/61.png" }
        };
    }

    public static List<Club> GetAllClubs()
    {
        var comps = GetInitialCompetitions().ToDictionary(c => c.CompetitionId);

        return new List<Club>
        {
            // Premier League (Id = 1)
            new() { ClubId = 1, Name = "Manchester City", CompetitionId = 1, Competition = comps[1], FoundedYear = 1880, Logo = "https://media.api-sports.io/football/teams/50.png", MatchesPlayed = 28, Wins = 20, Draws = 5, Losses = 3, GoalsFor = 67, GoalsAgainst = 28, RecentForm = "W,W,D,W,W" },
            new() { ClubId = 2, Name = "Arsenal", CompetitionId = 1, Competition = comps[1], FoundedYear = 1886, Logo = "https://media.api-sports.io/football/teams/42.png", MatchesPlayed = 28, Wins = 19, Draws = 6, Losses = 3, GoalsFor = 64, GoalsAgainst = 24, RecentForm = "W,W,W,D,W" },
            new() { ClubId = 3, Name = "Liverpool", CompetitionId = 1, Competition = comps[1], FoundedYear = 1892, Logo = "https://media.api-sports.io/football/teams/40.png", MatchesPlayed = 28, Wins = 19, Draws = 5, Losses = 4, GoalsFor = 65, GoalsAgainst = 27, RecentForm = "W,D,W,W,L" },
            new() { ClubId = 4, Name = "Chelsea", CompetitionId = 1, Competition = comps[1], FoundedYear = 1905, Logo = "https://media.api-sports.io/football/teams/49.png", MatchesPlayed = 28, Wins = 15, Draws = 6, Losses = 7, GoalsFor = 55, GoalsAgainst = 38, RecentForm = "W,L,W,D,W" },

            // La Liga (Id = 2)
            new() { ClubId = 5, Name = "Real Madrid", CompetitionId = 2, Competition = comps[2], FoundedYear = 1902, Logo = "https://media.api-sports.io/football/teams/541.png", MatchesPlayed = 27, Wins = 21, Draws = 4, Losses = 2, GoalsFor = 66, GoalsAgainst = 20, RecentForm = "W,W,W,W,D" },
            new() { ClubId = 6, Name = "Barcelona", CompetitionId = 2, Competition = comps[2], FoundedYear = 1899, Logo = "https://media.api-sports.io/football/teams/529.png", MatchesPlayed = 27, Wins = 20, Draws = 3, Losses = 4, GoalsFor = 71, GoalsAgainst = 25, RecentForm = "W,W,W,W,W" },
            new() { ClubId = 7, Name = "Atletico Madrid", CompetitionId = 2, Competition = comps[2], FoundedYear = 1903, Logo = "https://media.api-sports.io/football/teams/530.png", MatchesPlayed = 27, Wins = 17, Draws = 6, Losses = 4, GoalsFor = 48, GoalsAgainst = 21, RecentForm = "W,D,W,L,W" },

            // Serie A (Id = 3)
            new() { ClubId = 8, Name = "Inter Milan", CompetitionId = 3, Competition = comps[3], FoundedYear = 1908, Logo = "https://media.api-sports.io/football/teams/505.png", MatchesPlayed = 27, Wins = 21, Draws = 4, Losses = 2, GoalsFor = 68, GoalsAgainst = 18, RecentForm = "W,W,W,D,W" },
            new() { ClubId = 9, Name = "Juventus", CompetitionId = 3, Competition = comps[3], FoundedYear = 1897, Logo = "https://media.api-sports.io/football/teams/496.png", MatchesPlayed = 27, Wins = 16, Draws = 8, Losses = 3, GoalsFor = 47, GoalsAgainst = 22, RecentForm = "D,W,D,W,W" },
            new() { ClubId = 10, Name = "AC Milan", CompetitionId = 3, Competition = comps[3], FoundedYear = 1899, Logo = "https://media.api-sports.io/football/teams/489.png", MatchesPlayed = 27, Wins = 16, Draws = 5, Losses = 6, GoalsFor = 52, GoalsAgainst = 32, RecentForm = "L,W,W,D,W" },

            // Bundesliga (Id = 4)
            new() { ClubId = 11, Name = "Bayer Leverkusen", CompetitionId = 4, Competition = comps[4], FoundedYear = 1904, Logo = "https://media.api-sports.io/football/teams/168.png", MatchesPlayed = 25, Wins = 19, Draws = 5, Losses = 1, GoalsFor = 62, GoalsAgainst = 22, RecentForm = "W,W,D,W,W" },
            new() { ClubId = 12, Name = "Bayern Munich", CompetitionId = 4, Competition = comps[4], FoundedYear = 1900, Logo = "https://media.api-sports.io/football/teams/157.png", MatchesPlayed = 25, Wins = 19, Draws = 3, Losses = 3, GoalsFor = 74, GoalsAgainst = 24, RecentForm = "W,W,W,L,W" },
            new() { ClubId = 13, Name = "Borussia Dortmund", CompetitionId = 4, Competition = comps[4], FoundedYear = 1909, Logo = "https://media.api-sports.io/football/teams/165.png", MatchesPlayed = 25, Wins = 14, Draws = 6, Losses = 5, GoalsFor = 49, GoalsAgainst = 31, RecentForm = "W,D,W,W,L" },

            // Ligue 1 (Id = 5)
            new() { ClubId = 14, Name = "Paris Saint-Germain", CompetitionId = 5, Competition = comps[5], FoundedYear = 1970, Logo = "https://media.api-sports.io/football/teams/85.png", MatchesPlayed = 25, Wins = 18, Draws = 6, Losses = 1, GoalsFor = 65, GoalsAgainst = 23, RecentForm = "W,W,W,D,W" },
            new() { ClubId = 15, Name = "Monaco", CompetitionId = 5, Competition = comps[5], FoundedYear = 1924, Logo = "https://media.api-sports.io/football/teams/91.png", MatchesPlayed = 25, Wins = 14, Draws = 5, Losses = 6, GoalsFor = 48, GoalsAgainst = 33, RecentForm = "W,L,W,D,W" }
        };
    }

    public static List<(Player Player, Performance Performance)> GetAllPlayersAndPerformances()
    {
        var clubs = GetAllClubs().ToDictionary(c => c.ClubId);

        var data = new List<(Player Player, Performance Performance)>();

        void Add(int id, string name, string nat, string code, Position pos, PreferredFoot foot, int clubId, 
                 long marketValue, short matches, short minutes, short goals, short assists, decimal rating,
                 decimal xg, decimal xa, short keyPasses, short cleansheets, short conceded, short saves,
                 short tackles, short interceptions, short dribbles, short succDribbles, decimal passAcc, short duelsWon)
        {
            var p = new Player
            {
                PlayerId = id,
                FullName = name,
                Nationality = nat,
                CountryCode = code,
                BirthDate = new DateOnly(1996, 6, 15),
                Position = pos,
                PreferredFoot = foot,
                ClubId = clubId,
                Club = clubs[clubId],
                MarketValueEur = marketValue,
                Photo = $"https://images.unsplash.com/photo-1508098682722-e99c43a406b2?auto=format&fit=crop&w=256&q=80"
            };

            var perf = new Performance
            {
                PerformanceId = id,
                PlayerId = id,
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
                CleanSheets = cleansheets,
                GoalsConceded = conceded,
                Saves = saves,
                Tackles = tackles,
                Interceptions = interceptions,
                Dribbles = dribbles,
                SuccessfulDribbles = succDribbles,
                PassAccuracy = passAcc,
                DuelsWon = duelsWon,
                Shots = (short)(goals * 4),
                ShotsOnTarget = (short)(goals * 2),
                YellowCards = 2,
                RedCards = 0,
                Clearances = 12,
                ProgressiveCarries = (short)(succDribbles * 2)
            };

            data.Add((p, perf));
        }

        // PREMIER LEAGUE
        Add(1, "Erling Haaland", "Norway", "NOR", Position.ST, PreferredFoot.Left, 1, 180_000_000, 26, 2250, 24, 4, 8.25m, 21.4m, 3.2m, 28, 0, 0, 0, 8, 4, 25, 14, 76.5m, 82);
        Add(2, "Kevin De Bruyne", "Belgium", "BEL", Position.CAM, PreferredFoot.Right, 1, 60_000_000, 20, 1540, 6, 14, 8.42m, 5.1m, 12.8m, 68, 0, 0, 0, 22, 12, 35, 24, 86.4m, 64);
        Add(3, "Rodri", "Spain", "ESP", Position.CDM, PreferredFoot.Right, 1, 130_000_000, 27, 2380, 7, 8, 8.38m, 4.8m, 6.2m, 44, 11, 0, 0, 68, 38, 30, 22, 92.8m, 145);
        Add(4, "Bukayo Saka", "England", "ENG", Position.RW, PreferredFoot.Left, 2, 140_000_000, 27, 2310, 14, 11, 8.18m, 12.1m, 9.4m, 72, 0, 0, 0, 42, 21, 95, 58, 84.1m, 112);
        Add(5, "William Saliba", "France", "FRA", Position.CB, PreferredFoot.Right, 2, 85_000_000, 28, 2520, 2, 1, 8.10m, 1.2m, 0.8m, 12, 14, 22, 0, 54, 32, 12, 9, 93.1m, 138);
        Add(6, "David Raya", "Spain", "ESP", Position.GK, PreferredFoot.Right, 2, 40_000_000, 28, 2520, 0, 0, 7.80m, 0m, 0m, 4, 14, 22, 78, 2, 1, 0, 0, 81.2m, 18);
        Add(7, "Mohamed Salah", "Egypt", "EGY", Position.RW, PreferredFoot.Left, 3, 65_000_000, 27, 2300, 19, 12, 8.35m, 16.8m, 10.5m, 64, 0, 0, 0, 20, 12, 78, 44, 79.5m, 92);
        Add(8, "Virgil van Dijk", "Netherlands", "NED", Position.CB, PreferredFoot.Right, 3, 35_000_000, 27, 2430, 3, 2, 8.08m, 2.5m, 1.1m, 14, 12, 24, 0, 48, 36, 8, 6, 91.5m, 158);
        Add(9, "Cole Palmer", "England", "ENG", Position.CAM, PreferredFoot.Left, 4, 90_000_000, 26, 2150, 18, 9, 8.22m, 15.2m, 8.1m, 58, 0, 0, 0, 28, 14, 68, 42, 83.2m, 86);

        // LA LIGA
        Add(10, "Vinicius Junior", "Brazil", "BRA", Position.LW, PreferredFoot.Right, 5, 200_000_000, 25, 2120, 18, 11, 8.45m, 15.5m, 9.8m, 62, 0, 0, 0, 24, 15, 120, 72, 81.6m, 118);
        Add(11, "Jude Bellingham", "England", "ENG", Position.CAM, PreferredFoot.Right, 5, 180_000_000, 24, 2080, 15, 8, 8.36m, 13.2m, 7.1m, 48, 0, 0, 0, 45, 28, 65, 41, 88.3m, 134);
        Add(12, "Kylian Mbappe", "France", "FRA", Position.ST, PreferredFoot.Right, 5, 180_000_000, 25, 2150, 21, 5, 8.30m, 19.5m, 4.8m, 45, 0, 0, 0, 14, 8, 95, 54, 84.0m, 88);
        Add(13, "Lamine Yamal", "Spain", "ESP", Position.RW, PreferredFoot.Left, 6, 150_000_000, 25, 1980, 9, 13, 8.38m, 8.4m, 11.2m, 65, 0, 0, 0, 32, 18, 110, 68, 82.5m, 102);
        Add(14, "Robert Lewandowski", "Poland", "POL", Position.ST, PreferredFoot.Right, 6, 15_000_000, 26, 2180, 22, 4, 8.28m, 20.1m, 3.5m, 30, 0, 0, 0, 12, 6, 28, 16, 75.8m, 78);
        Add(15, "Pedri", "Spain", "ESP", Position.CM, PreferredFoot.Right, 6, 80_000_000, 23, 1750, 4, 7, 8.15m, 3.2m, 6.5m, 48, 0, 0, 0, 38, 22, 45, 32, 91.2m, 82);
        Add(16, "Antoine Griezmann", "France", "FRA", Position.CAM, PreferredFoot.Left, 7, 25_000_000, 26, 2210, 13, 9, 8.12m, 11.8m, 8.4m, 54, 0, 0, 0, 35, 20, 48, 28, 82.8m, 94);

        // SERIE A
        Add(17, "Lautaro Martinez", "Argentina", "ARG", Position.ST, PreferredFoot.Right, 8, 110_000_000, 26, 2240, 21, 5, 8.32m, 18.6m, 4.4m, 38, 0, 0, 0, 22, 14, 45, 26, 78.4m, 95);
        Add(18, "Nicolo Barella", "Italy", "ITA", Position.CM, PreferredFoot.Right, 8, 80_000_000, 25, 2100, 5, 8, 8.14m, 4.2m, 7.2m, 46, 0, 0, 0, 48, 26, 42, 28, 88.7m, 110);
        Add(19, "Alessandro Bastoni", "Italy", "ITA", Position.CB, PreferredFoot.Left, 8, 75_000_000, 25, 2180, 2, 4, 8.16m, 1.4m, 3.5m, 22, 12, 17, 0, 52, 34, 18, 14, 91.8m, 128);
        Add(20, "Dusan Vlahovic", "Serbia", "SRB", Position.ST, PreferredFoot.Left, 9, 65_000_000, 25, 2050, 16, 3, 7.95m, 15.2m, 2.8m, 26, 0, 0, 0, 10, 5, 32, 18, 74.5m, 72);
        Add(21, "Rafael Leao", "Portugal", "POR", Position.LW, PreferredFoot.Right, 10, 90_000_000, 25, 2020, 11, 9, 8.08m, 9.8m, 8.2m, 52, 0, 0, 0, 20, 10, 115, 65, 80.2m, 98);

        // BUNDESLIGA
        Add(22, "Harry Kane", "England", "ENG", Position.ST, PreferredFoot.Right, 12, 100_000_000, 24, 2100, 25, 7, 8.52m, 22.8m, 6.5m, 44, 0, 0, 0, 16, 9, 32, 20, 81.0m, 85);
        Add(23, "Jamal Musiala", "Germany", "GER", Position.CAM, PreferredFoot.Right, 12, 130_000_000, 23, 1890, 12, 8, 8.35m, 10.5m, 7.8m, 55, 0, 0, 0, 30, 18, 110, 72, 86.8m, 108);
        Add(24, "Florian Wirtz", "Germany", "GER", Position.CAM, PreferredFoot.Right, 11, 130_000_000, 24, 2010, 13, 12, 8.48m, 11.5m, 11.0m, 74, 0, 0, 0, 36, 20, 92, 60, 87.5m, 115);
        Add(25, "Granit Xhaka", "Switzerland", "SUI", Position.CDM, PreferredFoot.Left, 11, 20_000_000, 25, 2240, 3, 5, 8.12m, 2.8m, 4.5m, 42, 10, 0, 0, 56, 32, 16, 12, 92.5m, 125);
        Add(26, "Serhou Guirassy", "Guinea", "GUI", Position.ST, PreferredFoot.Right, 13, 40_000_000, 23, 1920, 17, 3, 7.98m, 15.8m, 2.5m, 24, 0, 0, 0, 12, 6, 25, 15, 76.0m, 76);

        // LIGUE 1
        Add(27, "Ousmane Dembele", "France", "FRA", Position.RW, PreferredFoot.Both, 14, 60_000_000, 23, 1850, 8, 12, 8.20m, 7.5m, 10.8m, 62, 0, 0, 0, 25, 14, 98, 62, 82.2m, 86);
        Add(28, "Bradley Barcola", "France", "FRA", Position.LW, PreferredFoot.Right, 14, 65_000_000, 24, 1900, 14, 7, 8.15m, 12.2m, 6.5m, 45, 0, 0, 0, 28, 16, 85, 52, 83.5m, 82);
        Add(29, "Achraf Hakimi", "Morocco", "MAR", Position.RB, PreferredFoot.Right, 14, 60_000_000, 23, 1980, 5, 6, 8.10m, 4.5m, 5.8m, 42, 9, 21, 0, 50, 28, 55, 34, 88.0m, 105);
        Add(30, "Gianluigi Donnarumma", "Italy", "ITA", Position.GK, PreferredFoot.Right, 14, 40_000_000, 24, 2160, 0, 0, 7.85m, 0m, 0m, 2, 10, 22, 72, 1, 0, 0, 0, 80.5m, 16);

        return data;
    }
}
