using CaminoManager.ApiService.Endpoints;
using CaminoManager.Data.Contexts;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Camino Manager API",
        Version = "v1",
        Description = "API for managing Camino Manager data",
        Contact = new OpenApiContact
        {
            Name = "Esteban Lalinde",
            Email = "esteban.lalinde@gmail.com"
        }
    });
});

builder.AddNpgsqlDbContext<CaminoManagerDbContext>("postgres",
    configureSettings: settings =>
    {
        // Configure your connection settings here if needed
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Camino Manager API V1");
        c.RoutePrefix = "swagger";
    });
}

app.MapPersonEndpoints();
app.MapCommunityEndpoints();
app.MapBrotherEndpoints();
app.MapCountryEndpoints();
app.MapStateEndpoints();
app.MapCityEndpoints();
app.MapTeamTypeEndpoints();
app.MapParishEndpoints();
app.MapStepWayEndpoints();

app.MapDefaultEndpoints();

app.Run();
