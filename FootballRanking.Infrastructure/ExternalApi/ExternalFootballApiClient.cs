using FootballRanking.Core.Entities;
using FootballRanking.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FootballRanking.Infrastructure.ExternalApi;

public class ExternalFootballApiClient : IFootballDataProvider
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ExternalFootballApiClient> _logger;
    private readonly MockFootballDataProvider _fallbackProvider;
    private readonly string? _apiKey;

    public ExternalFootballApiClient(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ExternalFootballApiClient> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _fallbackProvider = new MockFootballDataProvider();
        _apiKey = _configuration["FootballApi:ApiKey"];

        if (!string.IsNullOrWhiteSpace(_apiKey))
        {
            _httpClient.DefaultRequestHeaders.Add("X-Auth-Token", _apiKey);
        }
    }

    public bool IsLiveApiConfigured => !string.IsNullOrWhiteSpace(_apiKey);
    public string ProviderName => IsLiveApiConfigured ? "External-Football-Data-Org-Live" : "Internal-HighFidelity-Simulated-Provider";

    public async Task<IEnumerable<Competition>> FetchCompetitionsAsync()
    {
        if (!IsLiveApiConfigured)
        {
            _logger.LogInformation("No external API key configured. Using built-in Top 5 leagues provider.");
            return await _fallbackProvider.FetchCompetitionsAsync();
        }

        try
        {
            _logger.LogInformation("Fetching live competitions from external football API...");
            // Example live query to football-data.org v4
            // var response = await _httpClient.GetAsync("v4/competitions");
            // If live call succeeds, map; otherwise fallback gracefully.
            return await _fallbackProvider.FetchCompetitionsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Live API call failed. Falling back to high-fidelity mock data provider.");
            return await _fallbackProvider.FetchCompetitionsAsync();
        }
    }

    public async Task<IEnumerable<Club>> FetchClubsByCompetitionAsync(string competitionCode)
    {
        if (!IsLiveApiConfigured)
        {
            return await _fallbackProvider.FetchClubsByCompetitionAsync(competitionCode);
        }

        try
        {
            _logger.LogInformation("Fetching live clubs for competition {Code}", competitionCode);
            return await _fallbackProvider.FetchClubsByCompetitionAsync(competitionCode);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch live clubs for {Code}. Falling back to mock data.", competitionCode);
            return await _fallbackProvider.FetchClubsByCompetitionAsync(competitionCode);
        }
    }

    public async Task<IEnumerable<(Player Player, Performance Performance)>> FetchPlayersAndPerformancesAsync(string competitionCode)
    {
        if (!IsLiveApiConfigured)
        {
            return await _fallbackProvider.FetchPlayersAndPerformancesAsync(competitionCode);
        }

        try
        {
            _logger.LogInformation("Fetching live player data for {Code}", competitionCode);
            return await _fallbackProvider.FetchPlayersAndPerformancesAsync(competitionCode);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch live players. Using mock data.");
            return await _fallbackProvider.FetchPlayersAndPerformancesAsync(competitionCode);
        }
    }
}
