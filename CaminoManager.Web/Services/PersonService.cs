using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.DTOs;

namespace CaminoManager.Web.Services;

public class PersonService : BaseApiService
{
    public PersonService(HttpClient httpClient) : base(httpClient) { }

    public async Task<List<PersonDto>> GetPeopleAsync(PersonType? personType = null)
    {
        var url = "people";
        if (personType.HasValue)
        {
            url += $"?personType={(int)personType}";
        }
        return await GetListAsync<PersonDto>(url);
    }

    public async Task<PersonDto?> GetPersonAsync(string id)
    {
        return await GetAsync<PersonDto>($"people/{id}");
    }

    public async Task CreatePersonAsync(PersonDto person)
    {
        await PostAsync("people", person);
    }

    public async Task UpdatePersonAsync(PersonDto person)
    {
        await PutAsync($"people/{person.Id}", person);
    }

    public async Task DeletePersonAsync(string id)
    {
        await DeleteAsync($"people/{id}");
    }
}