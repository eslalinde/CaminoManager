using CaminoManager.Web.Components;
using CaminoManager.Web.Services;
using MudBlazor.Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure common HTTP client settings
var apiClientBuilder = builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https+http://apiservice");
});

// Register services using the common HTTP client
builder.Services.AddScoped(sp => new PersonService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));
builder.Services.AddScoped(sp => new CommunityService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));
builder.Services.AddScoped(sp => new CountryService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));
builder.Services.AddScoped(sp => new StateService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));
builder.Services.AddScoped(sp => new CityService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));
builder.Services.AddScoped(sp => new ParishService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));
builder.Services.AddScoped(sp => new PriestService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));

builder.Services.AddMudServices();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var supportedCultures = new[]
{
    new CultureInfo("es"),
    new CultureInfo("en")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("es");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    
    // Add URL query string parameter provider
    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    };
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.UseRequestLocalization(new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture("es"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    }
});

app.Run();
