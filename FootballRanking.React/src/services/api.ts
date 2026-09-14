import { PlayerRanking, ClubRanking, Competition, RankingWeights, PlayerFilter, PagedResult } from '../types/football';

const API_BASE = '/api';

export const DEFAULT_WEIGHTS: RankingWeights = {
  attackingWeight: 1.0,
  playmakingWeight: 1.0,
  defendingWeight: 1.0,
  disciplineWeight: 1.0,
  minutesThreshold: 360,
  applyLeagueCoefficient: true
};

export const COMPETITIONS: Competition[] = [
  { competitionId: 1, name: "Premier League", code: "PL", country: "England", type: "League", coefficientWeight: 1.00, logo: "https://media.api-sports.io/football/leagues/39.png", clubCount: 4 },
  { competitionId: 2, name: "La Liga", code: "PD", country: "Spain", type: "League", coefficientWeight: 0.98, logo: "https://media.api-sports.io/football/leagues/140.png", clubCount: 3 },
  { competitionId: 3, name: "Serie A", code: "SA", country: "Italy", type: "League", coefficientWeight: 0.96, logo: "https://media.api-sports.io/football/leagues/135.png", clubCount: 3 },
  { competitionId: 4, name: "Bundesliga", code: "BL1", country: "Germany", type: "League", coefficientWeight: 0.95, logo: "https://media.api-sports.io/football/leagues/78.png", clubCount: 3 },
  { competitionId: 5, name: "Ligue 1", code: "FL1", country: "France", type: "League", coefficientWeight: 0.92, logo: "https://media.api-sports.io/football/leagues/61.png", clubCount: 2 }
];

export const MOCK_CLUBS: ClubRanking[] = [
  { rank: 1, clubId: 1, name: "Real Madrid", logo: "https://media.api-sports.io/football/teams/541.png", competitionName: "La Liga", competitionCode: "PD", matchesPlayed: 27, wins: 21, draws: 4, losses: 2, goalsFor: 66, goalsAgainst: 20, goalDifference: 46, points: 67, powerRating: 95.8, attackRating: 94.2, defenseRating: 91.5, recentForm: ["W","W","W","W","D"], squadAverageScore: 88.4 },
  { rank: 2, clubId: 2, name: "Manchester City", logo: "https://media.api-sports.io/football/teams/50.png", competitionName: "Premier League", competitionCode: "PL", matchesPlayed: 28, wins: 20, draws: 5, losses: 3, goalsFor: 67, goalsAgainst: 28, goalDifference: 39, points: 65, powerRating: 94.6, attackRating: 93.8, defenseRating: 88.0, recentForm: ["W","W","D","W","W"], squadAverageScore: 89.1 },
  { rank: 3, clubId: 3, name: "Bayer Leverkusen", logo: "https://media.api-sports.io/football/teams/168.png", competitionName: "Bundesliga", competitionCode: "BL1", matchesPlayed: 25, wins: 19, draws: 5, losses: 1, goalsFor: 62, goalsAgainst: 22, goalDifference: 40, points: 62, powerRating: 93.2, attackRating: 90.5, defenseRating: 89.2, recentForm: ["W","W","D","W","W"], squadAverageScore: 85.8 },
  { rank: 4, clubId: 4, name: "Inter Milan", logo: "https://media.api-sports.io/football/teams/505.png", competitionName: "Serie A", competitionCode: "SA", matchesPlayed: 27, wins: 21, draws: 4, losses: 2, goalsFor: 68, goalsAgainst: 18, goalDifference: 50, points: 67, powerRating: 92.9, attackRating: 91.0, defenseRating: 92.4, recentForm: ["W","W","W","D","W"], squadAverageScore: 86.2 },
  { rank: 5, clubId: 5, name: "Arsenal", logo: "https://media.api-sports.io/football/teams/42.png", competitionName: "Premier League", competitionCode: "PL", matchesPlayed: 28, wins: 19, draws: 6, losses: 3, goalsFor: 64, goalsAgainst: 24, goalDifference: 40, points: 63, powerRating: 92.4, attackRating: 89.4, defenseRating: 91.0, recentForm: ["W","W","W","D","W"], squadAverageScore: 87.0 },
  { rank: 6, clubId: 6, name: "Bayern Munich", logo: "https://media.api-sports.io/football/teams/157.png", competitionName: "Bundesliga", competitionCode: "BL1", matchesPlayed: 25, wins: 19, draws: 3, losses: 3, goalsFor: 74, goalsAgainst: 24, goalDifference: 50, points: 60, powerRating: 91.8, attackRating: 96.0, defenseRating: 84.5, recentForm: ["W","W","W","L","W"], squadAverageScore: 87.5 },
  { rank: 7, clubId: 7, name: "Barcelona", logo: "https://media.api-sports.io/football/teams/529.png", competitionName: "La Liga", competitionCode: "PD", matchesPlayed: 27, wins: 20, draws: 3, losses: 4, goalsFor: 71, goalsAgainst: 25, goalDifference: 46, points: 63, powerRating: 91.2, attackRating: 94.5, defenseRating: 85.0, recentForm: ["W","W","W","W","W"], squadAverageScore: 86.8 },
  { rank: 8, clubId: 8, name: "Paris Saint-Germain", logo: "https://media.api-sports.io/football/teams/85.png", competitionName: "Ligue 1", competitionCode: "FL1", matchesPlayed: 25, wins: 18, draws: 6, losses: 1, goalsFor: 65, goalsAgainst: 23, goalDifference: 42, points: 60, powerRating: 89.5, attackRating: 88.0, defenseRating: 86.2, recentForm: ["W","W","W","D","W"], squadAverageScore: 84.2 },
  { rank: 9, clubId: 9, name: "Liverpool", logo: "https://media.api-sports.io/football/teams/40.png", competitionName: "Premier League", competitionCode: "PL", matchesPlayed: 28, wins: 19, draws: 5, losses: 4, goalsFor: 65, goalsAgainst: 27, goalDifference: 38, points: 62, powerRating: 89.0, attackRating: 89.2, defenseRating: 85.8, recentForm: ["W","D","W","W","L"], squadAverageScore: 86.0 }
];

export const MOCK_PLAYERS: PlayerRanking[] = [
  { rank: 1, playerId: 1, name: "Erling Haaland", age: 24, nationality: "Norway", countryCode: "NOR", position: "ST", clubName: "Manchester City", clubLogo: "https://media.api-sports.io/football/teams/50.png", competitionName: "Premier League", competitionCode: "PL", marketValueEur: 180000000, matches: 26, minutesPlayed: 2250, goals: 24, assists: 4, rating: 8.25, goalsPer90: 0.96, assistsPer90: 0.16, expectedGoalsPer90: 0.86, keyPassesPer90: 1.12, tacklesPer90: 0.32, interceptionsPer90: 0.16, passAccuracy: 76.5, overallScore: 94.8, attackingScore: 96.5, playmakingScore: 42.0, defendingScore: 28.5, disciplineScore: 92.0, minutesDampener: 1.0, leagueWeight: 1.0, rankChange: 0 },
  { rank: 2, playerId: 10, name: "Vinicius Junior", age: 24, nationality: "Brazil", countryCode: "BRA", position: "LW", clubName: "Real Madrid", clubLogo: "https://media.api-sports.io/football/teams/541.png", competitionName: "La Liga", competitionCode: "PD", marketValueEur: 200000000, matches: 25, minutesPlayed: 2120, goals: 18, assists: 11, rating: 8.45, goalsPer90: 0.76, assistsPer90: 0.47, expectedGoalsPer90: 0.66, keyPassesPer90: 2.63, tacklesPer90: 1.02, interceptionsPer90: 0.64, passAccuracy: 81.6, overallScore: 94.2, attackingScore: 94.8, playmakingScore: 82.5, defendingScore: 36.2, disciplineScore: 88.0, minutesDampener: 1.0, leagueWeight: 0.98, rankChange: 1 },
  { rank: 3, playerId: 24, name: "Florian Wirtz", age: 21, nationality: "Germany", countryCode: "GER", position: "CAM", clubName: "Bayer Leverkusen", clubLogo: "https://media.api-sports.io/football/teams/168.png", competitionName: "Bundesliga", competitionCode: "BL1", marketValueEur: 130000000, matches: 24, minutesPlayed: 2010, goals: 13, assists: 12, rating: 8.48, goalsPer90: 0.58, assistsPer90: 0.54, expectedGoalsPer90: 0.51, keyPassesPer90: 3.31, tacklesPer90: 1.61, interceptionsPer90: 0.90, passAccuracy: 87.5, overallScore: 93.6, attackingScore: 88.2, playmakingScore: 95.4, defendingScore: 48.0, disciplineScore: 96.0, minutesDampener: 1.0, leagueWeight: 0.95, rankChange: 2 },
  { rank: 4, playerId: 3, name: "Rodri", age: 28, nationality: "Spain", countryCode: "ESP", position: "CDM", clubName: "Manchester City", clubLogo: "https://media.api-sports.io/football/teams/50.png", competitionName: "Premier League", competitionCode: "PL", marketValueEur: 130000000, matches: 27, minutesPlayed: 2380, goals: 7, assists: 8, rating: 8.38, goalsPer90: 0.26, assistsPer90: 0.30, expectedGoalsPer90: 0.18, keyPassesPer90: 1.66, tacklesPer90: 2.57, interceptionsPer90: 1.44, passAccuracy: 92.8, overallScore: 93.1, attackingScore: 68.4, playmakingScore: 89.2, defendingScore: 92.5, disciplineScore: 84.0, minutesDampener: 1.0, leagueWeight: 1.0, rankChange: -1 },
  { rank: 5, playerId: 22, name: "Harry Kane", age: 31, nationality: "England", countryCode: "ENG", position: "ST", clubName: "Bayern Munich", clubLogo: "https://media.api-sports.io/football/teams/157.png", competitionName: "Bundesliga", competitionCode: "BL1", marketValueEur: 100000000, matches: 24, minutesPlayed: 2100, goals: 25, assists: 7, rating: 8.52, goalsPer90: 1.07, assistsPer90: 0.30, expectedGoalsPer90: 0.98, keyPassesPer90: 1.89, tacklesPer90: 0.69, interceptionsPer90: 0.39, passAccuracy: 81.0, overallScore: 92.8, attackingScore: 97.2, playmakingScore: 74.0, defendingScore: 30.5, disciplineScore: 94.0, minutesDampener: 1.0, leagueWeight: 0.95, rankChange: 0 },
  { rank: 6, playerId: 11, name: "Jude Bellingham", age: 21, nationality: "England", countryCode: "ENG", position: "CAM", clubName: "Real Madrid", clubLogo: "https://media.api-sports.io/football/teams/541.png", competitionName: "La Liga", competitionCode: "PD", marketValueEur: 180000000, matches: 24, minutesPlayed: 2080, goals: 15, assists: 8, rating: 8.36, goalsPer90: 0.65, assistsPer90: 0.35, expectedGoalsPer90: 0.57, keyPassesPer90: 2.08, tacklesPer90: 1.95, interceptionsPer90: 1.21, passAccuracy: 88.3, overallScore: 92.5, attackingScore: 86.4, playmakingScore: 88.0, defendingScore: 62.0, disciplineScore: 82.0, minutesDampener: 1.0, leagueWeight: 0.98, rankChange: -2 },
  { rank: 7, playerId: 7, name: "Mohamed Salah", age: 32, nationality: "Egypt", countryCode: "EGY", position: "RW", clubName: "Liverpool", clubLogo: "https://media.api-sports.io/football/teams/40.png", competitionName: "Premier League", competitionCode: "PL", marketValueEur: 65000000, matches: 27, minutesPlayed: 2300, goals: 19, assists: 12, rating: 8.35, goalsPer90: 0.74, assistsPer90: 0.47, expectedGoalsPer90: 0.66, keyPassesPer90: 2.50, tacklesPer90: 0.78, interceptionsPer90: 0.47, passAccuracy: 79.5, overallScore: 92.0, attackingScore: 93.0, playmakingScore: 85.5, defendingScore: 32.0, disciplineScore: 98.0, minutesDampener: 1.0, leagueWeight: 1.0, rankChange: 1 },
  { rank: 8, playerId: 13, name: "Lamine Yamal", age: 17, nationality: "Spain", countryCode: "ESP", position: "RW", clubName: "Barcelona", clubLogo: "https://media.api-sports.io/football/teams/529.png", competitionName: "La Liga", competitionCode: "PD", marketValueEur: 150000000, matches: 25, minutesPlayed: 1980, goals: 9, assists: 13, rating: 8.38, goalsPer90: 0.41, assistsPer90: 0.59, expectedGoalsPer90: 0.38, keyPassesPer90: 2.95, tacklesPer90: 1.45, interceptionsPer90: 0.82, passAccuracy: 82.5, overallScore: 91.6, attackingScore: 87.5, playmakingScore: 92.0, defendingScore: 42.0, disciplineScore: 96.0, minutesDampener: 1.0, leagueWeight: 0.98, rankChange: 3 },
  { rank: 9, playerId: 4, name: "Bukayo Saka", age: 23, nationality: "England", countryCode: "ENG", position: "RW", clubName: "Arsenal", clubLogo: "https://media.api-sports.io/football/teams/42.png", competitionName: "Premier League", competitionCode: "PL", marketValueEur: 140000000, matches: 27, minutesPlayed: 2310, goals: 14, assists: 11, rating: 8.18, goalsPer90: 0.55, assistsPer90: 0.43, expectedGoalsPer90: 0.47, keyPassesPer90: 2.81, tacklesPer90: 1.64, interceptionsPer90: 0.82, passAccuracy: 84.1, overallScore: 91.2, attackingScore: 89.0, playmakingScore: 88.5, defendingScore: 45.0, disciplineScore: 94.0, minutesDampener: 1.0, leagueWeight: 1.0, rankChange: 0 },
  { rank: 10, playerId: 5, name: "William Saliba", age: 23, nationality: "France", countryCode: "FRA", position: "CB", clubName: "Arsenal", clubLogo: "https://media.api-sports.io/football/teams/42.png", competitionName: "Premier League", competitionCode: "PL", marketValueEur: 85000000, matches: 28, minutesPlayed: 2520, goals: 2, assists: 1, rating: 8.10, goalsPer90: 0.07, assistsPer90: 0.04, expectedGoalsPer90: 0.04, keyPassesPer90: 0.43, tacklesPer90: 1.93, interceptionsPer90: 1.14, passAccuracy: 93.1, overallScore: 90.5, attackingScore: 28.0, playmakingScore: 65.0, defendingScore: 94.5, disciplineScore: 95.0, minutesDampener: 1.0, leagueWeight: 1.0, rankChange: 1 },
  { rank: 11, playerId: 17, name: "Lautaro Martinez", age: 27, nationality: "Argentina", countryCode: "ARG", position: "ST", clubName: "Inter Milan", clubLogo: "https://media.api-sports.io/football/teams/505.png", competitionName: "Serie A", competitionCode: "SA", marketValueEur: 110000000, matches: 26, minutesPlayed: 2240, goals: 21, assists: 5, rating: 8.32, goalsPer90: 0.84, assistsPer90: 0.20, expectedGoalsPer90: 0.75, keyPassesPer90: 1.53, tacklesPer90: 0.88, interceptionsPer90: 0.56, passAccuracy: 78.4, overallScore: 90.2, attackingScore: 93.5, playmakingScore: 62.0, defendingScore: 35.0, disciplineScore: 88.0, minutesDampener: 1.0, leagueWeight: 0.96, rankChange: -1 },
  { rank: 12, playerId: 6, name: "David Raya", age: 29, nationality: "Spain", countryCode: "ESP", position: "GK", clubName: "Arsenal", clubLogo: "https://media.api-sports.io/football/teams/42.png", competitionName: "Premier League", competitionCode: "PL", marketValueEur: 40000000, matches: 28, minutesPlayed: 2520, goals: 0, assists: 0, rating: 7.80, goalsPer90: 0, assistsPer90: 0, expectedGoalsPer90: 0, keyPassesPer90: 0.14, tacklesPer90: 0.07, interceptionsPer90: 0.04, passAccuracy: 81.2, overallScore: 88.4, attackingScore: 10.0, playmakingScore: 40.0, defendingScore: 91.2, disciplineScore: 96.0, minutesDampener: 1.0, leagueWeight: 1.0, rankChange: 2 },
  { rank: 13, playerId: 28, name: "Bradley Barcola", age: 22, nationality: "France", countryCode: "FRA", position: "LW", clubName: "Paris Saint-Germain", clubLogo: "https://media.api-sports.io/football/teams/85.png", competitionName: "Ligue 1", competitionCode: "FL1", marketValueEur: 65000000, matches: 24, minutesPlayed: 1900, goals: 14, assists: 7, rating: 8.15, goalsPer90: 0.66, assistsPer90: 0.33, expectedGoalsPer90: 0.58, keyPassesPer90: 2.13, tacklesPer90: 1.33, interceptionsPer90: 0.76, passAccuracy: 83.5, overallScore: 88.0, attackingScore: 88.4, playmakingScore: 78.0, defendingScore: 38.5, disciplineScore: 94.0, minutesDampener: 1.0, leagueWeight: 0.92, rankChange: 1 }
];

export async function fetchPlayers(filter: PlayerFilter, weights: RankingWeights = DEFAULT_WEIGHTS): Promise<PagedResult<PlayerRanking>> {
  try {
    const params = new URLSearchParams();
    if (filter.position) params.append('position', filter.position);
    if (filter.competitionCode) params.append('competitionCode', filter.competitionCode);
    if (filter.searchQuery) params.append('searchQuery', filter.searchQuery);
    if (filter.sortBy) params.append('sortBy', filter.sortBy);
    if (filter.sortDescending !== undefined) params.append('sortDescending', String(filter.sortDescending));
    if (filter.page) params.append('page', String(filter.page));
    if (filter.pageSize) params.append('pageSize', String(filter.pageSize));

    params.append('attackingWeight', String(weights.attackingWeight));
    params.append('playmakingWeight', String(weights.playmakingWeight));
    params.append('defendingWeight', String(weights.defendingWeight));
    params.append('disciplineWeight', String(weights.disciplineWeight));
    params.append('minutesThreshold', String(weights.minutesThreshold));
    params.append('applyLeagueCoefficient', String(weights.applyLeagueCoefficient));

    const response = await fetch(`${API_BASE}/rankings/players?${params.toString()}`);
    if (!response.ok) throw new Error(`HTTP ${response.status}`);
    return await response.json();
  } catch {
    // Graceful fallback to client-side ranked mock data
    let list = [...MOCK_PLAYERS];

    if (filter.position) {
      if (filter.position === 'FWD') list = list.filter(p => ['ST', 'LW', 'RW'].includes(p.position));
      else if (filter.position === 'MID') list = list.filter(p => ['CAM', 'CM', 'CDM'].includes(p.position));
      else if (filter.position === 'DEF') list = list.filter(p => ['CB', 'LB', 'RB'].includes(p.position));
      else list = list.filter(p => p.position === filter.position);
    }

    if (filter.competitionCode) {
      list = list.filter(p => p.competitionCode === filter.competitionCode);
    }

    if (filter.searchQuery) {
      const q = filter.searchQuery.toLowerCase();
      list = list.filter(p => p.name.toLowerCase().includes(q) || p.clubName.toLowerCase().includes(q) || p.nationality.toLowerCase().includes(q));
    }

    // Client-side simulation weighting
    list = list.map(p => {
      const wTotal = weights.attackingWeight + weights.playmakingWeight + weights.defendingWeight + weights.disciplineWeight;
      const customScore = (
        (p.attackingScore * weights.attackingWeight) +
        (p.playmakingScore * weights.playmakingWeight) +
        (p.defendingScore * weights.defendingWeight) +
        (p.disciplineScore * weights.disciplineWeight)
      ) / (wTotal > 0 ? wTotal : 1);

      const leagueAdj = weights.applyLeagueCoefficient ? p.leagueWeight : 1.0;
      const overall = Math.round(Math.min(99.9, Math.max(10, customScore * leagueAdj)) * 10) / 10;
      return { ...p, overallScore: overall };
    });

    list.sort((a, b) => b.overallScore - a.overallScore);
    list.forEach((p, idx) => { p.rank = idx + 1; });

    return {
      items: list,
      totalCount: list.length,
      page: 1,
      pageSize: 50,
      totalPages: 1
    };
  }
}

export async function fetchClubs(competitionCode?: string): Promise<ClubRanking[]> {
  try {
    const url = competitionCode ? `${API_BASE}/rankings/clubs?competitionCode=${competitionCode}` : `${API_BASE}/rankings/clubs`;
    const response = await fetch(url);
    if (!response.ok) throw new Error(`HTTP ${response.status}`);
    return await response.json();
  } catch {
    if (competitionCode) {
      return MOCK_CLUBS.filter(c => c.competitionCode === competitionCode);
    }
    return MOCK_CLUBS;
  }
}

export async function fetchCompetitions(): Promise<Competition[]> {
  try {
    const response = await fetch(`${API_BASE}/competitions`);
    if (!response.ok) throw new Error(`HTTP ${response.status}`);
    return await response.json();
  } catch {
    return COMPETITIONS;
  }
}
