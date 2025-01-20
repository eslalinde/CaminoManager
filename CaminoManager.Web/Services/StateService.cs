using CaminoManager.ServiceDefaults.DTOs;

namespace CaminoManager.Web.Services;

public class StateService : BaseApiService
{
    public StateService(HttpClient httpClient) : base(httpClient) { }

    public async Task<List<StateDto>> GetStatesAsync()
    {
        return await GetListAsync<StateDto>("states");
    }

    public async Task<StateDto?> GetStateAsync(string id)
    {
        return await GetAsync<StateDto>($"states/{id}");
    }

    public async Task CreateStateAsync(StateDto state)
    {
        await PostAsync("states", state);
    }

    public async Task UpdateStateAsync(StateDto state)
    {
        await PutAsync($"states/{state.Id}", state);
    }

    public async Task DeleteStateAsync(string id)
    {
        await DeleteAsync($"states/{id}");
    }
} 