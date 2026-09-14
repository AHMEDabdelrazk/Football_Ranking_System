using FootballRanking.API.Middleware;
using FootballRanking.API.Services;
using FootballRanking.Core.Interfaces;
using FootballRanking.Core.Services;
using FootballRanking.Infrastructure.Data;
using FootballRanking.Infrastructure.ExternalApi;
using Microsoft.EntityFrameworkCore;

namespace FootballRanking.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new()
            {
                Title = "Football Ranking API",
                Version = "v1",
                Description = "High-performance enterprise ranking engine for Top 5 European Football Leagues."
            });
        });

        // CORS configuration for React frontend
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins(
                    "http://localhost:5173", 
                    "http://localhost:3000", 
                    "http://127.0.0.1:5173", 
                    "http://127.0.0.1:3000")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        // Database Configuration with In-Memory fallback for easy evaluation
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        bool useInMemory = builder.Configuration.GetValue<bool>("Database:UseInMemory") 
                           || string.IsNullOrWhiteSpace(connectionString)
                           || connectionString.Contains("YOUR_SERVER", StringComparison.OrdinalIgnoreCase);

        if (useInMemory)
        {
            builder.Services.AddDbContext<FootballDbContext>(options =>
            {
                options.UseInMemoryDatabase("FootballRankingDb");
            });
        }
        else
        {
            builder.Services.AddDbContext<FootballDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(3);
                });
            });
        }

        // Core Ranking Engine and Services
        builder.Services.AddSingleton<IRankingEngine, RankingEngine>();
        builder.Services.AddScoped<IPlayerService, PlayerService>();
        builder.Services.AddScoped<IClubService, ClubService>();
        builder.Services.AddScoped<ICompetitionService, CompetitionService>();

        // External Football Data API Client & Resilient Fallback Provider
        builder.Services.AddHttpClient<ExternalFootballApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.football-data.org/v4/");
            client.Timeout = TimeSpan.FromSeconds(10);
        });
        builder.Services.AddScoped<IFootballDataProvider, ExternalFootballApiClient>();

        var app = builder.Build();

        // Database Migration / Seed execution
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                var context = services.GetRequiredService<FootballDbContext>();
                if (!useInMemory && context.Database.IsRelational())
                {
                    await context.Database.MigrateAsync();
                }
                await DatabaseSeeder.SeedAsync(context);
                logger.LogInformation("Database seeded successfully with Top 5 leagues, clubs, and player stats.");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Relational DB migration skipped or failed. Falling back to in-memory seeding.");
                try
                {
                    var inMemoryOptions = new DbContextOptionsBuilder<FootballDbContext>()
                        .UseInMemoryDatabase("FootballRankingDb_Fallback")
                        .Options;
                    using var fallbackContext = new FootballDbContext(inMemoryOptions);
                    await DatabaseSeeder.SeedAsync(fallbackContext);
                }
                catch (Exception seedEx)
                {
                    logger.LogError(seedEx, "Failed to initialize fallback database.");
                }
            }
        }

        // Global Exception Handling Middleware (RFC 7807)
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        // Configure HTTP request pipeline
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Football Ranking API v1");
            c.RoutePrefix = "swagger";
        });

        //app.UseHttpsRedirection();
        app.UseCors("AllowFrontend");
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
