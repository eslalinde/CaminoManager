using Aspire.Hosting;

public class TestFixture : IAsyncLifetime
{
    private DistributedApplication? _app;
    public HttpClient? HttpClient { get; private set; }

    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.CaminoManager_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        _app = await appHost.BuildAsync();
        var resourceNotificationService = _app.Services.GetRequiredService<ResourceNotificationService>();
        await _app.StartAsync();

        HttpClient = _app.CreateHttpClient("apiservice");
        await resourceNotificationService.WaitForResourceAsync("apiservice", KnownResourceStates.Running)
            .WaitAsync(TimeSpan.FromSeconds(30));
    }

    public async Task DisposeAsync()
    {
        if (_app != null)
        {
            await _app.DisposeAsync();
        }
    }
}