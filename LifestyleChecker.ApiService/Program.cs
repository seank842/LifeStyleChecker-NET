using LifestyleChecker.ApiService.Attrbutes;
using LifestyleChecker.ApiService.Conventions;
using LifestyleChecker.ApiService.Filters;
using LifestyleChecker.ApiService.Services;
using LifestyleChecker.Infrastructure.Persistence;
using LifestyleChecker.Infrastructure.Persistence.Seeding;
using LifestyleChecker.Service.Scoring;
using LifestyleChecker.Services.PatientService;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace LifestyleChecker.ApiService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add service defaults & Aspire client integrations.
            builder.AddServiceDefaults();
            var logger = builder.Logging.Services.BuildServiceProvider()
                                .GetRequiredService<ILoggerFactory>()
                                .CreateLogger("Startup");
            logger.LogInformation("Starting LifestyleChecker API Service...");
            // Add services to the container.
            builder.Services.AddProblemDetails();

            // Add custom filters
            builder.Services.AddControllers(options =>
            {
                options.Conventions.Add(new AdminAuthoriseProducesConvention());
                options.Conventions.Add(new PatientAuthoriseProducesConvention());
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

            // Add redis or fallback to InMemory cache service
            try
            {
                var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("cache"));
                builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
                builder.Services.AddSingleton<ICacheService, RedisCacheService>();
                logger.LogInformation("Redis cache service configured successfully.");
            }
            catch
            {
                builder.Services.AddMemoryCache();
                builder.Services.AddSingleton<ICacheService, InMemoryCacheService>();
                logger.LogWarning("Redis cache service failed to connect, falling back to InMemory cache service.");
            }

            // Add DbContext
            builder.Services.AddDbContext<LifestyleCheckerDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("LifestyleCheckerDb"));
            });

            builder.Services.AddHttpClient<IPatientLookup, PatientLookup>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["PatientLookupAPI:BaseUrl"]);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", builder.Configuration["PatientLookupAPI:ApiKey"]);
            });

            builder.Services.AddTransient<IScoringService, ScoringService>();

            // Add Swagger/OpenAPI with Basic Auth support
            builder.Services.AddSwaggerGen(options =>
            {
                // Basic Auth security scheme
                options.AddSecurityDefinition("basic Admin", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Admin Authorisation",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Basic Authentication header for admin access"
                });

                options.AddSecurityDefinition("basic Patient", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Patient Authorisation",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Basic Authentication header for patient access, username: NHSNumber|Surname, password: Date of Birth dd-mm-yyyy format"
                });

                // Only require Basic Auth for endpoints with [AdminAuthorise]
                options.OperationFilter<AdminAuthoriseOperationFilter>();
                options.OperationFilter<PatientAuthoriseOperationFilter>();
            });

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<LifestyleCheckerDbContext>();
            // Ensure the database is created (due to sqlite db file) and apply migrations

            //await dbContext.Database.EnsureDeletedAsync();
            //await dbContext.Database.EnsureCreatedAsync();
            await dbContext.Database.MigrateAsync();

            // Check if the database is empty and seed it with initial data
            if (!dbContext.Questionnaires.Any())
                await DataSeeder.SeedAsync(dbContext);

            app.UseExceptionHandler();

            // Enable Swagger and Swagger UI in Development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                });
            }

            app.UseRouting();
            app.MapDefaultEndpoints();
            app.MapControllers();

            app.Run();
        }
    }
}