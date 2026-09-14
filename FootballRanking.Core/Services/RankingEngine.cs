using FootballRanking.Core.DTOs;
using FootballRanking.Core.Entities;
using FootballRanking.Core.Interfaces;

namespace FootballRanking.Core.Services;

public class RankingEngine : IRankingEngine
{
    private static readonly Dictionary<string, decimal> DefaultLeagueCoefficients = new(StringComparer.OrdinalIgnoreCase)
    {
        { "PL", 1.00m },    // Premier League
        { "PD", 0.98m },    // La Liga
        { "SA", 0.96m },    // Serie A
        { "BL1", 0.95m },   // Bundesliga
        { "FL1", 0.92m }    // Ligue 1
    };

    public PlayerRankingDto CalculatePlayerRanking(
        Player player, 
        Performance performance, 
        Competition competition, 
        RankingWeightsDto? weights = null)
    {
        weights ??= new RankingWeightsDto();

        decimal minutesPlayed = Math.Max(0m, (decimal)performance.MinutesPlayed);
        decimal matches = Math.Max(1m, (decimal)performance.Matches);
        decimal per90Multiplier = minutesPlayed > 0 ? (90m / minutesPlayed) : 0m;

        // Per 90 stats
        decimal goalsPer90 = Math.Round(performance.Goals * per90Multiplier, 2);
        decimal assistsPer90 = Math.Round(performance.Assists * per90Multiplier, 2);
        decimal xGPer90 = Math.Round(performance.ExpectedGoals * per90Multiplier, 2);
        decimal keyPassesPer90 = Math.Round(performance.KeyPasses * per90Multiplier, 2);
        decimal tacklesPer90 = Math.Round(performance.Tackles * per90Multiplier, 2);
        decimal interceptionsPer90 = Math.Round(performance.Interceptions * per90Multiplier, 2);
        decimal dribblesPer90 = Math.Round(performance.SuccessfulDribbles * per90Multiplier, 2);
        decimal progressiveCarriesPer90 = Math.Round(performance.ProgressiveCarries * per90Multiplier, 2);
        decimal savesPer90 = Math.Round(performance.Saves * per90Multiplier, 2);
        decimal goalsConcededPer90 = Math.Round(performance.GoalsConceded * per90Multiplier, 2);

        // 1. Attacking Score (0 - 100)
        decimal attackRaw;
        if (player.Position == Position.GK)
        {
            attackRaw = 10m;
        }
        else
        {
            decimal shotAccuracy = performance.Shots > 0 
                ? (decimal)performance.ShotsOnTarget / performance.Shots 
                : 0.35m;
            decimal dribbleSuccess = performance.Dribbles > 0 
                ? (decimal)performance.SuccessfulDribbles / performance.Dribbles 
                : 0.50m;

            attackRaw = (goalsPer90 * 45m)
                      + (xGPer90 * 25m)
                      + (shotAccuracy * 15m)
                      + (dribblesPer90 * 8m)
                      + (dribbleSuccess * 7m);
        }
        decimal attackingScore = Math.Clamp(attackRaw * 2.2m, 5m, 100m);

        // 2. Playmaking Score (0 - 100)
        decimal passAccNormalized = performance.PassAccuracy > 0 
            ? Math.Clamp(performance.PassAccuracy, 50m, 100m) 
            : 75m;
        decimal playmakingRaw = (assistsPer90 * 40m)
                              + (keyPassesPer90 * 18m)
                              + (progressiveCarriesPer90 * 8m)
                              + ((passAccNormalized - 50m) * 0.7m);
        decimal playmakingScore = Math.Clamp(playmakingRaw * 2.0m, 10m, 100m);

        // 3. Defending Score (0 - 100)
        decimal defendingRaw;
        if (player.Position == Position.GK)
        {
            decimal savePercentage = (performance.Saves + performance.GoalsConceded) > 0
                ? (decimal)performance.Saves / (performance.Saves + performance.GoalsConceded)
                : 0.72m;
            decimal cleanSheetRatio = (decimal)performance.CleanSheets / matches;

            defendingRaw = (savePercentage * 60m) 
                         + (cleanSheetRatio * 30m) 
                         + Math.Max(0m, 10m - (goalsConcededPer90 * 5m));
        }
        else
        {
            decimal cleanSheetRatio = (decimal)performance.CleanSheets / matches;
            decimal duelsRatio = (performance.DuelsWon + 1) / (decimal)(performance.DuelsWon + 2);

            defendingRaw = (tacklesPer90 * 18m)
                         + (interceptionsPer90 * 16m)
                         + (performance.Clearances * per90Multiplier * 6m)
                         + (cleanSheetRatio * 20m)
                         + (duelsRatio * 25m);
        }
        decimal defendingScore = Math.Clamp(defendingRaw * 1.8m, 10m, 100m);

        // 4. Discipline Score (0 - 100)
        decimal cardDeduction = (performance.YellowCards * 4.0m) + (performance.RedCards * 15.0m);
        decimal disciplineScore = Math.Clamp(100m - cardDeduction, 20m, 100m);

        // 5. Minutes Dampening Factor
        decimal minutesDampener;
        if (minutesPlayed >= weights.MinutesThreshold)
        {
            minutesDampener = 1.0m;
        }
        else if (minutesPlayed <= 0)
        {
            minutesDampener = 0.20m;
        }
        else
        {
            // Smooth square-root ramp to allow promising low-minute players while avoiding sample bias
            minutesDampener = Math.Clamp((decimal)Math.Sqrt((double)(minutesPlayed / weights.MinutesThreshold)), 0.25m, 1.0m);
        }

        // 6. League Coefficient Weighting
        decimal leagueWeight = 1.0m;
        if (weights.ApplyLeagueCoefficient)
        {
            string compCode = competition.Code ?? string.Empty;
            if (DefaultLeagueCoefficients.TryGetValue(compCode, out var coeff))
            {
                leagueWeight = coeff;
            }
            else if (competition.CoefficientWeight > 0)
            {
                leagueWeight = competition.CoefficientWeight;
            }
        }

        // 7. Dynamic Role-Based Matrix
        (decimal attackP, decimal playP, decimal defP, decimal discP) = GetRoleWeights(player.Position);

        // Apply evaluator weights
        decimal totalW = (attackP * weights.AttackingWeight)
                       + (playP * weights.PlaymakingWeight)
                       + (defP * weights.DefendingWeight)
                       + (discP * weights.DisciplineWeight);

        decimal weightedRawScore = ((attackingScore * attackP * weights.AttackingWeight)
                                  + (playmakingScore * playP * weights.PlaymakingWeight)
                                  + (defendingScore * defP * weights.DefendingWeight)
                                  + (disciplineScore * discP * weights.DisciplineWeight)) / (totalW > 0 ? totalW : 1.0m);

        // Include match rating boost (Ratings typically 6.0 to 8.5)
        decimal ratingFactor = performance.Rating > 0 ? Math.Clamp(performance.Rating / 8.0m, 0.75m, 1.25m) : 1.0m;

        decimal overallScore = Math.Round(Math.Clamp(weightedRawScore * ratingFactor * minutesDampener * leagueWeight, 10m, 99.9m), 1);

        int age = DateTime.UtcNow.Year - player.BirthDate.Year;

        return new PlayerRankingDto
        {
            PlayerId = player.PlayerId,
            Name = player.FullName,
            Age = age > 0 ? age : 24,
            Nationality = player.Nationality,
            CountryCode = player.CountryCode ?? player.Nationality.Substring(0, Math.Min(3, player.Nationality.Length)).ToUpper(),
            Position = player.Position.ToString(),
            ClubName = player.Club?.Name ?? "Unknown Club",
            ClubLogo = player.Club?.Logo,
            CompetitionName = competition.Name,
            CompetitionCode = competition.Code,
            MarketValueEur = player.MarketValueEur,

            Matches = performance.Matches,
            MinutesPlayed = performance.MinutesPlayed,
            Goals = performance.Goals,
            Assists = performance.Assists,
            Rating = performance.Rating,

            GoalsPer90 = goalsPer90,
            AssistsPer90 = assistsPer90,
            ExpectedGoalsPer90 = xGPer90,
            KeyPassesPer90 = keyPassesPer90,
            TacklesPer90 = tacklesPer90,
            InterceptionsPer90 = interceptionsPer90,
            PassAccuracy = performance.PassAccuracy,

            AttackingScore = Math.Round(attackingScore, 1),
            PlaymakingScore = Math.Round(playmakingScore, 1),
            DefendingScore = Math.Round(defendingScore, 1),
            DisciplineScore = Math.Round(disciplineScore, 1),
            MinutesDampener = Math.Round(minutesDampener, 3),
            LeagueWeight = Math.Round(leagueWeight, 2),
            OverallScore = overallScore
        };
    }

    public ClubRankingDto CalculateClubRanking(
        Club club, 
        Competition competition, 
        IEnumerable<PlayerRankingDto>? playerRankings = null)
    {
        short matches = Math.Max((short)1, club.MatchesPlayed);
        int maxPoints = matches * 3;
        int points = (club.Wins * 3) + club.Draws;
        int goalDiff = club.GoalsFor - club.GoalsAgainst;

        decimal pointsPct = (decimal)points / maxPoints; // 0.0 to 1.0
        decimal gdPerGame = (decimal)goalDiff / matches;  // e.g. -2.0 to +3.0

        // Parse Recent Form e.g. "W,W,D,W,L"
        var formTokens = (club.RecentForm ?? "W,D,W,L,W")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Take(5)
            .ToList();

        decimal formScore = 0m;
        decimal[] formWeights = { 1.5m, 1.3m, 1.1m, 0.9m, 0.7m };
        for (int i = 0; i < formTokens.Count; i++)
        {
            decimal pts = formTokens[i].ToUpper() switch
            {
                "W" => 3.0m,
                "D" => 1.0m,
                _ => 0.0m
            };
            formScore += pts * formWeights[i];
        }
        decimal maxFormScore = 3.0m * formWeights.Take(formTokens.Count).Sum();
        decimal formNormalized = maxFormScore > 0 ? (formScore / maxFormScore) * 100m : 50m;

        // Squad rating
        var playersList = playerRankings?.ToList() ?? new List<PlayerRankingDto>();
        decimal squadAvg = playersList.Any() 
            ? playersList.Take(15).Average(p => p.OverallScore) 
            : 75.0m;

        // Attack & Defense club ratings
        decimal goalsScoredPerGame = (decimal)club.GoalsFor / matches;
        decimal goalsConcededPerGame = (decimal)club.GoalsAgainst / matches;

        decimal attackRating = Math.Clamp(40m + (goalsScoredPerGame * 20m), 30m, 99m);
        decimal defenseRating = Math.Clamp(100m - (goalsConcededPerGame * 25m), 30m, 99m);

        // League coefficient
        string compCode = competition.Code ?? string.Empty;
        decimal leagueCoeff = DefaultLeagueCoefficients.TryGetValue(compCode, out var coeff) 
            ? coeff 
            : (competition.CoefficientWeight > 0 ? competition.CoefficientWeight : 1.0m);

        // Composite Power Rating (0 - 100)
        decimal compositePower = (pointsPct * 35m)
                               + (Math.Clamp((gdPerGame + 2.5m) / 5.0m, 0m, 1m) * 25m)
                               + ((formNormalized / 100m) * 20m)
                               + ((squadAvg / 100m) * 20m);

        decimal finalPowerRating = Math.Round(Math.Clamp(compositePower * leagueCoeff, 20m, 99.5m), 1);

        return new ClubRankingDto
        {
            ClubId = club.ClubId,
            Name = club.Name,
            Logo = club.Logo,
            CompetitionName = competition.Name,
            CompetitionCode = competition.Code,
            MatchesPlayed = club.MatchesPlayed,
            Wins = club.Wins,
            Draws = club.Draws,
            Losses = club.Losses,
            GoalsFor = club.GoalsFor,
            GoalsAgainst = club.GoalsAgainst,
            GoalDifference = goalDiff,
            Points = points,
            PowerRating = finalPowerRating,
            AttackRating = Math.Round(attackRating, 1),
            DefenseRating = Math.Round(defenseRating, 1),
            RecentForm = formTokens,
            SquadAverageScore = Math.Round(squadAvg, 1)
        };
    }

    public IEnumerable<PlayerRankingDto> RankPlayers(
        IEnumerable<(Player Player, Performance Performance, Competition Competition)> playerData, 
        RankingWeightsDto? weights = null)
    {
        var list = playerData
            .Select(data => CalculatePlayerRanking(data.Player, data.Performance, data.Competition, weights))
            .OrderByDescending(p => p.OverallScore)
            .ToList();

        for (int i = 0; i < list.Count; i++)
        {
            list[i].Rank = i + 1;
            // Simulated rank movement for UI showcase (-2 to +3)
            list[i].RankChange = ((list[i].PlayerId * 7) % 5) - 2;
        }

        return list;
    }

    public IEnumerable<ClubRankingDto> RankClubs(
        IEnumerable<(Club Club, Competition Competition, IEnumerable<PlayerRankingDto> PlayerRankings)> clubData)
    {
        var list = clubData
            .Select(data => CalculateClubRanking(data.Club, data.Competition, data.PlayerRankings))
            .OrderByDescending(c => c.PowerRating)
            .ThenByDescending(c => c.GoalDifference)
            .ToList();

        for (int i = 0; i < list.Count; i++)
        {
            list[i].Rank = i + 1;
        }

        return list;
    }

    private static (decimal attackP, decimal playP, decimal defP, decimal discP) GetRoleWeights(Position position)
    {
        return position switch
        {
            Position.GK => (0.05m, 0.15m, 0.70m, 0.10m),
            Position.CB => (0.05m, 0.20m, 0.65m, 0.10m),
            Position.LB or Position.RB => (0.20m, 0.35m, 0.35m, 0.10m),
            Position.CDM => (0.15m, 0.35m, 0.40m, 0.10m),
            Position.CM => (0.25m, 0.45m, 0.20m, 0.10m),
            Position.CAM => (0.45m, 0.40m, 0.05m, 0.10m),
            Position.LW or Position.RW => (0.55m, 0.30m, 0.05m, 0.10m),
            Position.ST => (0.75m, 0.15m, 0.02m, 0.08m),
            _ => (0.25m, 0.25m, 0.40m, 0.10m)
        };
    }
}
