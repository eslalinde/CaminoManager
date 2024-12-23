namespace CaminoManager.Web.Services;

public abstract class BaseApiService
{
    protected readonly HttpClient HttpClient;

    protected BaseApiService(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    protected async Task<T?> GetAsync<T>(string endpoint)
    {
        return await HttpClient.GetFromJsonAsync<T>(endpoint);
    }

    protected async Task<List<T>> GetListAsync<T>(string endpoint)
    {
        return await HttpClient.GetFromJsonAsync<List<T>>(endpoint) ?? new();
    }

    protected async Task PostAsync<T>(string endpoint, T data)
    {
        var response = await HttpClient.PostAsJsonAsync(endpoint, data);
        response.EnsureSuccessStatusCode();
    }

    protected async Task PutAsync<T>(string endpoint, T data)
    {
        var response = await HttpClient.PutAsJsonAsync(endpoint, data);
        response.EnsureSuccessStatusCode();
    }

    protected async Task DeleteAsync(string endpoint)
    {
        var response = await HttpClient.DeleteAsync(endpoint);
        response.EnsureSuccessStatusCode();
    }
}