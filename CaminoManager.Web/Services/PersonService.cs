using CaminoManager.Data.Models;

namespace CaminoManager.Web.Services;

public class PersonService : BaseApiService
{
    public PersonService(HttpClient httpClient) : base(httpClient) { }

    public async Task<List<Person>> GetPersonsAsync()
    {
        return await GetListAsync<Person>("people");
    }

    public async Task<Person?> GetPersonAsync(Guid id)
    {
        return await GetAsync<Person>($"people/{id}");
    }

    public async Task CreatePersonAsync(Person person)
    {
        await PostAsync("people", person);
    }

    public async Task UpdatePersonAsync(Person person)
    {
        await PutAsync($"people/{person.Id}", person);
    }

    public async Task DeletePersonAsync(Guid id)
    {
        await DeleteAsync($"people/{id}");
    }
}