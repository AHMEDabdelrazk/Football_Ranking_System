export interface PlayerRanking {
  rank: number;
  playerId: number;
  name: string;
  age: number;
  nationality: string;
  countryCode?: string;
  position: string;
  clubName: string;
  clubLogo?: string;
  competitionName: string;
  competitionCode?: string;
  marketValueEur: number;

  matches: number;
  minutesPlayed: number;
  goals: number;
  assists: number;
  rating: number;

  goalsPer90: number;
  assistsPer90: number;
  expectedGoalsPer90: number;
  keyPassesPer90: number;
  tacklesPer90: number;
  interceptionsPer90: number;
  passAccuracy: number;

  overallScore: number;
  attackingScore: number;
  playmakingScore: number;
  defendingScore: number;
  disciplineScore: number;
  minutesDampener: number;
  leagueWeight: number;

  rankChange: number;
}

export interface ClubRanking {
  rank: number;
  clubId: number;
  name: string;
  logo?: string;
  competitionName: string;
  competitionCode?: string;

  matchesPlayed: number;
  wins: number;
  draws: number;
  losses: number;
  goalsFor: number;
  goalsAgainst: number;
  goalDifference: number;
  points: number;

  powerRating: number;
  attackRating: number;
  defenseRating: number;
  recentForm: string[];
  squadAverageScore: number;
}

export interface RankingWeights {
  attackingWeight: number;
  playmakingWeight: number;
  defendingWeight: number;
  disciplineWeight: number;
  minutesThreshold: number;
  applyLeagueCoefficient: boolean;
}

export interface Competition {
  competitionId: number;
  name: string;
  type: string;
  country?: string;
  code?: string;
  coefficientWeight: number;
  logo?: string;
  clubCount: number;
}

export interface PlayerFilter {
  position?: string;
  competitionCode?: string;
  minMinutes?: number;
  searchQuery?: string;
  sortBy?: string;
  sortDescending?: boolean;
  page?: number;
  pageSize?: number;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}
