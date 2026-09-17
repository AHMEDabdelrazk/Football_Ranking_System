# FootballRank Intelligence — Enterprise Sports Analytics Platform

An enterprise-grade sports analytics platform engineered to evaluate and rank football players and clubs across Europe's Top 5 Leagues (**Premier League, La Liga, Serie A, Bundesliga, Ligue 1**).

Built using **.NET 8 Clean Architecture**, **Entity Framework Core**, **xUnit Test Suite**, and a modern **React 18 + TypeScript** analytics dashboard.

---

## 🌐 Live Deployments

- **Backend API (Render)**: [https://football-ranking-system.onrender.com](https://football-ranking-system.onrender.com)
  - **Swagger UI**: [https://football-ranking-system.onrender.com/swagger](https://football-ranking-system.onrender.com/swagger)
- **Frontend Dashboard (Vercel)**: Ready to deploy from `FootballRanking.React` with zero configuration.

---

## 🚀 Quick Start Guide

### 1. Run the Backend (.NET 8 Web API)
```bash
cd FootballRanking.API
dotnet run
```
- The API will start with Swagger documentation available at:
  - `https://localhost:7159/swagger` or `http://localhost:5160/swagger`
- By default, it uses the high-performance **In-Memory database with Top 5 European league seed data**.
- To use SQL Server or add a live API key, update `appsettings.json`.

### 2. Run the Automated Tests (xUnit)
```bash
cd FootballRanking.Tests
dotnet test
```
- Validates the calculation engine, position matrices, minutes dampening factor, UEFA coefficients, and EF Core data queries.

### 3. Run the Frontend (React 18 + TypeScript)
```bash
cd FootballRanking.React
npm install
npm run dev
```
- Open `http://localhost:5173` in your browser.
- Supports interactive role filtering, dynamic weight sliders in the **Scouting Sandbox**, and deep-dive player modals with per-90 metrics.

---

## 🏛️ System Architecture

- **`FootballRanking.Core`**: Domain entities (`Player`, `Club`, `Competition`, `Performance`), DTOs, and the pure mathematical `RankingEngine`.
- **`FootballRanking.Infrastructure`**: EF Core `FootballDbContext`, database migrations/seeders, and `ExternalFootballApiClient` with `MockFootballDataProvider` fallback.
- **`FootballRanking.API`**: RESTful API controllers, Swagger documentation, and RFC 7807 global exception handling middleware.
- **`FootballRanking.Tests`**: Comprehensive unit and integration test suite using xUnit, FluentAssertions, and Moq.
- **`FootballRanking.React`**: Sleek modern dashboard built with React 18, TypeScript, and Tailwind CSS.
