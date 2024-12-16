using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var postgres = builder.AddPostgres("postgres", username, password);

var postgresdb = postgres.AddDatabase("camino");

var migration = builder.AddProject<Projects.CaminoManager_MigrationService>("migrations")
    .WithReference(postgres)
    .WaitFor(postgresdb);

var apiService = builder.AddProject<Projects.CaminoManager_ApiService>("apiservice")
    .WithReference(postgres)
    .WaitForCompletion(migration);

builder.AddProject<Projects.CaminoManager_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
