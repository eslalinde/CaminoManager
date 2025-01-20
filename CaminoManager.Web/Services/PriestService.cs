using CaminoManager.ServiceDefaults.DTOs;

namespace CaminoManager.Web.Services;

public class PriestService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "priests";

    public PriestService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PriestDto>> GetPriestsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<PriestDto>>($"{BaseUrl}") ?? new List<PriestDto>();
    }

    public async Task<PriestDto?> GetPriestAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<PriestDto>($"{BaseUrl}/{id}");
    }

    public async Task<PriestDto> CreatePriestAsync(CreatePriestDto priest)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseUrl, priest);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PriestDto>() ?? throw new Exception("Failed to create priest");
    }

    public async Task UpdatePriestAsync(UpdatePriestDto priest)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{priest.Id}", priest);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeletePriestAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        response.EnsureSuccessStatusCode();
    }
} 