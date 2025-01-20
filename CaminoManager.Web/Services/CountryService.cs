using CaminoManager.ServiceDefaults.DTOs;

namespace CaminoManager.Web.Services;

public class CountryService : BaseApiService
{
    public CountryService(HttpClient httpClient) : base(httpClient) { }

    public async Task<List<CountryDto>> GetCountriesAsync()
    {
        return await GetListAsync<CountryDto>("countries");
    }

    public async Task<CountryDto?> GetCountryAsync(string id)
    {
        return await GetAsync<CountryDto>($"countries/{id}");
    }

    public async Task CreateCountryAsync(CountryDto country)
    {
        await PostAsync("countries", country);
    }

    public async Task UpdateCountryAsync(CountryDto country)
    {
        await PutAsync($"countries/{country.Id}", country);
    }

    public async Task DeleteCountryAsync(string id)
    {
        await DeleteAsync($"countries/{id}");
    }
} 