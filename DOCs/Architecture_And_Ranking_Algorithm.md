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
$$PPS = \text{round}\left(\text{clamp}\left(WeightedRawScore \times R_{match} \times MNA \times LCE, 10.0, 99.9\right), 1\right)$$

where the role-weighted composite is:
$$WeightedRawScore = \frac{S_{atk} \cdot W_{atk} \cdot u_{atk} + S_{play} \cdot W_{play} \cdot u_{play} + S_{def} \cdot W_{def} \cdot u_{def} + S_{disc} \cdot W_{disc} \cdot u_{disc}}{\sum (W_i \cdot u_i)}$$

1. **Role-Specific Vectors ($W_{role}$)**:
   - **Strikers (ST)**: Attacking 75%, Playmaking 15%, Defending 2%, Discipline 8%
   - **Wingers (LW / RW)**: Attacking 55%, Playmaking 30%, Defending 5%, Discipline 10%
   - **Attacking Midfielders (CAM)**: Attacking 45%, Playmaking 40%, Defending 5%, Discipline 10%
   - **Central Midfielders (CM)**: Attacking 25%, Playmaking 45%, Defending 20%, Discipline 10%
   - **Defensive Midfielders (CDM)**: Attacking 15%, Playmaking 35%, Defending 40%, Discipline 10%
   - **Fullbacks (LB / RB)**: Attacking 20%, Playmaking 35%, Defending 35%, Discipline 10%
   - **Center-Backs (CB)**: Attacking 5%, Playmaking 20%, Defending 65%, Discipline 10%
   - **Goalkeepers (GK)**: Attacking 5%, Playmaking 15%, Defending 70%, Discipline 10%

2. **Per-90 Minute Statistical Normalization**:
   Every counting statistic is normalized against total minutes played:
   $$Stat_{per90} = Stat \times \left(\frac{90}{MinutesPlayed}\right) \quad (\text{yields } 0 \text{ when } MinutesPlayed = 0)$$

3. **Minutes Dampening Factor ($MNA$)**:
   Protects against small-sample distortions (e.g. brief cameos) with explicit lower bounds:
   - If $MinutesPlayed \ge Threshold$: $MNA = 1.0$
   - If $MinutesPlayed \le 0$: $MNA = 0.20$ (unplayed base floor)
   - If $0 < MinutesPlayed < Threshold$: $MNA = \text{clamp}\left(\sqrt{\frac{MinutesPlayed}{Threshold}}, 0.25, 1.0\right)$

4. **Discipline Sub-Score ($S_{disc}$)**:
   Disciplinary infractions reduce a clamped sub-score integrated as a weighted pillar of the player profile:
   $$CardDeduction = (YellowCards \times 4.0) + (RedCards \times 15.0)$$
   $$S_{disc} = \text{clamp}(100.0 - CardDeduction, 20.0, 100.0)$$

5. **Match Rating Scaling ($R_{match}$)**:
   Normalizes average match ratings around an 8.0 baseline:
   $$R_{match} = \text{clamp}\left(\frac{Rating}{8.0}, 0.75, 1.25\right)$$

6. **UEFA League Coefficient Multipliers ($LCE$)**:
   - Premier League (`PL`): $1.00\times$
   - La Liga (`PD`): $0.98\times$
   - Serie A (`SA`): $0.96\times$
   - Bundesliga (`BL1`): $0.95\times$
   - Ligue 1 (`FL1`): $0.92\times$

---

## 4. Club Power Index ($CPR$)
$$CPR = \text{round}\left(\text{clamp}\left(\left(0.35 \times Pct_{points} + 0.25 \times GD_{efficiency} + 0.20 \times Form_{momentum} + 0.20 \times Squad_{rating}\right) \times LCE, 20.0, 99.5\right), 1\right)$$

- **$Pct_{points}$**: Points obtained vs maximum possible points in played matches ($Points / (Matches \times 3)$).
- **$GD_{efficiency}$**: Goal differential per match normalized ($\text{clamp}((GD_{per\_game} + 2.5) / 5.0, 0.0, 1.0)$).
- **$Form_{momentum}$**: Exponentially weighted recent 5-match trajectory with decaying weights $[1.5, 1.3, 1.1, 0.9, 0.7]$ for $W=3$, $D=1$, $L=0$.
- **$Squad_{rating}$**: Average overall score of the club's top 15 squad players.

---

## 5. Resilient API Provider Pattern
- The ingestion layer implements `IFootballDataProvider` with `ExternalFootballApiClient` and `MockFootballDataProvider`.
- When an API key is configured in `appsettings.json`, `ExternalFootballApiClient` executes live HTTP queries against `football-data.org` v4 endpoints (`competitions`, `teams`, `scorers`).
- When no key is configured or when rate limits/network failures occur, the client seamlessly falls back to the high-fidelity simulated Top 5 European league dataset.
- The startup seeder (`DatabaseSeeder.SeedAsync`) receives `IFootballDataProvider` via Dependency Injection to populate the database.

---

## 6. How to Run

### Backend (.NET 8 Web API):
```bash
cd FootballRanking/FootballRanking.API
dotnet run
```
Swagger UI is available at: `https://localhost:7159/swagger` or `http://localhost:5160/swagger`.

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
