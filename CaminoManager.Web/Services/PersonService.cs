using CaminoManager.Data.Models;
using System.Net.Http.Json;

namespace CaminoManager.Web.Services;

public class PersonService
{
    private readonly HttpClient _httpClient;

    public PersonService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Person>> GetPersonsAsync(int page = 1, int pageSize = 10)
    {
        // TODO: Update API endpoint to support pagination
        return await _httpClient.GetFromJsonAsync<List<Person>>("/people") ?? new List<Person>();
    }

    public async Task<Person?> GetPersonAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<Person>($"/people/{id}");
    }

    public async Task<Person?> CreatePersonAsync(Person person)
    {
        var response = await _httpClient.PostAsJsonAsync("/people", person);
        return await response.Content.ReadFromJsonAsync<Person>();
    }

    public async Task UpdatePersonAsync(Person person)
    {
        await _httpClient.PutAsJsonAsync($"/people/{person.Id}", person);
    }

    public async Task DeletePersonAsync(Guid id)
    {
        await _httpClient.DeleteAsync($"/people/{id}");
    }
} 