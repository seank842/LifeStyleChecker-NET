var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.LifestyleChecker_ApiService>("apiservice")
    .WithReference(cache);

builder.AddProject<Projects.LifestyleChecker_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithEnvironment("ApiBaseUrl", apiService.GetEndpoint("https"));

builder.Build().Run();
