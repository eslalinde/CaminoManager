using CaminoManager.ServiceDefaults.DTOs;

namespace CaminoManager.Web.Services;

public class CommunityService : BaseApiService
{
    public CommunityService(HttpClient httpClient) : base(httpClient) { }

    public async Task<List<CommunityDto>> GetCommunitiesAsync()
    {
        return await GetListAsync<CommunityDto>("communities");
    }

    public async Task<CommunityDto?> GetCommunityAsync(string id)
    {
        return await GetAsync<CommunityDto>($"communities/{id}");
    }

    public async Task CreateCommunityAsync(CreateCommunityDto community)
    {
        await PostAsync("communities", community);
    }

    public async Task UpdateCommunityAsync(UpdateCommunityDto community)
    {
        await PutAsync($"communities/{community.Id}", community);
    }

    public async Task DeleteCommunityAsync(string id)
    {
        await DeleteAsync($"communities/{id}");
    }
}