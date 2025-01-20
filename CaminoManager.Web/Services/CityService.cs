using CaminoManager.ServiceDefaults.DTOs;
using System.Net.Http.Json;

namespace CaminoManager.Web.Services
{
    public class CityService
    {
        private readonly HttpClient _httpClient;

        public CityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CityDto>> GetCitiesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CityDto>>("/cities") ?? new List<CityDto>();
        }

        public async Task CreateCityAsync(CreateCityDto city)
        {
            await _httpClient.PostAsJsonAsync("/cities", city);
        }

        public async Task UpdateCityAsync(CityDto city)
        {
            await _httpClient.PutAsJsonAsync($"/cities/{city.Id}", city);
        }

        public async Task DeleteCityAsync(string cityId)
        {
            await _httpClient.DeleteAsync($"/cities/{cityId}");
        }
    }
} 