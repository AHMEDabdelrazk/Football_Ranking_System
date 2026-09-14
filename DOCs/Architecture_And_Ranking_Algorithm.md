# Football Ranking Engine - Architecture & Engineering Documentation

## 1. Overview & System Mission
The **Football Ranking Engine** is an enterprise-grade sports analytics platform engineered to rank football players and clubs across Europe's Top 5 Leagues:
1. **Premier League (England)**
2. **La Liga (Spain)**
3. **Serie A (Italy)**
4. **Bundesliga (Germany)**
5. **Ligue 1 (France)**

Unlike naive aggregators that merely count goals or assists, this system implements a mathematically rigorous multi-factor scoring model that accounts for positional roles, cross-league difficulty variance, sample size dampening, and disciplinary conduct.

---

## 2. High-Level Architecture (Clean Architecture & DDD)

```
FootballRanking
│
├── FootballRanking.Core (Domain & Business Logic)
│   ├── Entities/               # Domain Models: Player, Club, Competition, Performance, Stadium
│   ├── DTOs/                   # Data Transfer Objects, Filters, PagedResult, Custom Weight Profiles
│   ├── Interfaces/             # Service Contracts: IRankingEngine, IPlayerService, IClubService, ICompetitionService, IFootballDataProvider
│   └── Services/               # Core Ranking Engine implementation (pure, zero-infrastructure logic)
│
├── FootballRanking.Infrastructure (Data & External Integrations)
│   ├── Data/                   # EF Core DbContext with Fluent API configs & DatabaseSeeder
│   └── ExternalApi/            # External Football API client (HTTP) + High-fidelity simulated provider fallback
│
├── FootballRanking.API (Presentation & REST API)
│   ├── Controllers/            # REST Controllers: RankingsController, PlayersController, ClubsController, CompetitionsController
│   ├── Middleware/             # RFC 7807 Global Exception Handling Middleware
│   └── Program.cs              # Dependency Injection, CORS, Swagger, Resilient in-memory/relational DB initialization
│
├── FootballRanking.Tests (Automated xUnit Test Suite)
│   ├── RankingEngineTests.cs   # Unit tests verifying position matrices, minutes dampener, UEFA coefficient, discipline deductions
│   └── PlayerServiceTests.cs   # Unit & service tests with In-Memory EF Core
│
└── FootballRanking.React (Modern Interactive Web Dashboard)
    ├── src/
    │   ├── components/         # Navbar, PlayerLeaderboard, ClubPowerRankings, AlgorithmSandbox, PlayerModal, AlgorithmExplainer
    │   ├── services/           # Typed API Client with resilient client-side fallback
    │   └── types/              # TypeScript contracts matching .NET models
    ├── package.json
    ├── tailwind.config.js
    └── vite.config.ts
```

---

## 3. Mathematical Ranking Algorithm Breakdown

### A. Player Performance Score ($PPS$)
$$PPS = \left(\frac{\sum (MetricScore_i \times W_{role, i})}{\sum W_{role}} \times R_{match}\right) \times LCE \times MNA - DP$$

1. **Role-Specific Vectors ($W_{role}$)**:
   - **Strikers (ST)**: Attacking 75%, Playmaking 15%, Defending 2%, Discipline 8%.
   - **Wingers / CAMs**: Attacking 50%, Playmaking 35%, Defending 5%, Discipline 10%.
   - **Midfielders / CDMs**: Playmaking 45%, Defending 35%, Attacking 10%, Discipline 10%.
   - **Defenders (CB/FB)**: Defending 70%, Playmaking 15%, Attacking 5%, Discipline 10%.
   - **Goalkeepers (GK)**: Defending 70% (Saves %, Clean Sheets %, Goals Conceded/90), Playmaking 15%, Attacking 5%, Discipline 10%.

2. **Per-90 Minute Statistical Normalization**:
   Every counting statistic is normalized against total minutes played:
   $$Stat_{per90} = Stat \times \left(\frac{90}{MinutesPlayed}\right)$$

3. **Minutes Dampening Factor ($MNA$)**:
   To prevent small-sample anomalies (e.g., a substitute scoring in a 15-minute cameo):
   $$MNA = \min\left(1.0, \sqrt{\frac{MinutesPlayed}{Threshold}}\right)$$

4. **UEFA League Coefficient Multipliers ($LCE$)**:
   - Premier League: $1.00\times$
   - La Liga: $0.98\times$
   - Serie A: $0.96\times$
   - Bundesliga: $0.95\times$
   - Ligue 1: $0.92\times$

5. **Discipline Deductions ($DP$)**:
   - Yellow Card: $-4.0$ points
   - Red Card: $-15.0$ points

---

## 4. Club Power Index ($CPR$)
$$CPR = \left(0.35 \times Pct_{points} + 0.25 \times GD_{efficiency} + 0.20 \times Form_{momentum} + 0.20 \times Squad_{rating}\right) \times LCE$$

- **$Pct_{points}$**: Points obtained vs maximum possible points in played matches.
- **$GD_{efficiency}$**: Goal differential per match normalized.
- **$Form_{momentum}$**: Exponentially weighted recent 5-match trajectory (latest match has highest weight).
- **$Squad_{rating}$**: Average top-15 player performance scores in the squad.

---

## 5. Resilient API Provider Pattern
If no API key is supplied:
- The system automatically engages the built-in **High-Fidelity Top 5 Leagues Simulated Data Provider**.
- Real clubs (Man City, Real Madrid, Bayern Munich, Inter Milan, PSG, Arsenal, Barcelona, etc.) and authentic star player records (Haaland, Mbappe, Vinicius, Bellingham, Salah, Kane, Wirtz, Rodri, Saliba, Yamal) populate the system out-of-the-box.
- When an API key (e.g. from `api.football-data.org`) is set in `appsettings.json`, the client automatically routes live HTTP requests with circuit-breaker fallback.

---

## 6. How to Run

### Backend (.NET 8 Web API):
```bash
cd FootballRanking/FootballRanking.API
dotnet run
```
Swagger UI will be available at: `https://localhost:7198/swagger` or `http://localhost:5000/swagger`.

### Automated Tests:
```bash
cd FootballRanking/FootballRanking.Tests
dotnet test
```

### Frontend (React + TypeScript):
```bash
cd FootballRanking/FootballRanking.React
npm install
npm run dev
```
Dashboard will be available at: `http://localhost:5173`.
