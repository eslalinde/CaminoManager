using CaminoManager.ServiceDefaults.DTOs;
using System.Net.Http.Json;

namespace CaminoManager.Web.Services
{
    public class ParishService
    {
        private readonly HttpClient _httpClient;

        public ParishService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ParishDto>> GetParishesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ParishDto>>("/parishes");
        }

        public async Task<ParishDto> GetParishByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<ParishDto>($"/parishes/{id}");
        }

        public async Task CreateParishAsync(CreateParishDto createDto)
        {
            await _httpClient.PostAsJsonAsync("/parishes", createDto);
        }

        public async Task UpdateParishAsync(string id, UpdateParishDto updateDto)
        {
            await _httpClient.PutAsJsonAsync($"/parishes/{id}", updateDto);
        }

        public async Task DeleteParishAsync(string id)
        {
            await _httpClient.DeleteAsync($"/parishes/{id}");
        }
    }
} 