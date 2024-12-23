using CaminoManager.ServiceDefaults.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace CaminoManager.Tests;

public class PersonEndpointsTests : IClassFixture<TestFixture>
{
    private readonly HttpClient _httpClient;

    public PersonEndpointsTests(TestFixture fixture)
    {
        _httpClient = fixture.HttpClient ?? throw new ArgumentNullException(nameof(fixture.HttpClient));
    }

    [Fact]
    public async Task GetAll_ReturnsAllPeople()
    {
        // Act
        var response = await _httpClient.GetAsync("/people");
        var content = await response.Content.ReadAsStringAsync();
        var people = JsonSerializer.Deserialize<List<PersonDto>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(people);
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsPerson()
    {
        // Arrange
        var newPerson = new PersonDto
        {
            PersonName = "John Doe",
            Email = "john@example.com"
        };

        var createResponse = await _httpClient.PostAsJsonAsync("/people", newPerson);
        var createdPerson = await createResponse.Content.ReadFromJsonAsync<PersonDto>();

        // Act
        var response = await _httpClient.GetAsync($"/people/{createdPerson.Id}");
        var person = await response.Content.ReadFromJsonAsync<PersonDetailDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(person);
        Assert.Equal(createdPerson.Id, person.Id);
        Assert.Equal("John Doe", person.PersonName);
    }

    [Fact]
    public async Task Create_ValidPerson_ReturnsCreatedResult()
    {
        // Arrange
        var newPerson = new PersonDto
        {
            PersonName = "John Doe",
            Email = "john@example.com"
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/people", newPerson);
        var createdPerson = await response.Content.ReadFromJsonAsync<PersonDto>();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(createdPerson);
        Assert.Equal(newPerson.PersonName, createdPerson.PersonName);
        Assert.Equal(newPerson.Email, createdPerson.Email);
    }

    [Fact]
    public async Task Update_ExistingPerson_ReturnsNoContent()
    {
        // Arrange
        var newPerson = new PersonDto
        {
            PersonName = "John Doe",
            Email = "john@example.com"
        };

        var createResponse = await _httpClient.PostAsJsonAsync("/people", newPerson);
        var createdPerson = await createResponse.Content.ReadFromJsonAsync<PersonDto>();

        // Prepare update
        createdPerson.PersonName = "John Doe Updated";

        // Act
        var response = await _httpClient.PutAsJsonAsync($"/people/{createdPerson.Id}", createdPerson);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify the update
        var getResponse = await _httpClient.GetAsync($"/people/{createdPerson.Id}");
        var updatedPerson = await getResponse.Content.ReadFromJsonAsync<PersonDetailDto>();
        Assert.Equal("John Doe Updated", updatedPerson.PersonName);
    }

    [Fact]
    public async Task Delete_ExistingPerson_ReturnsNoContent()
    {
        // Arrange
        var newPerson = new PersonDto
        {
            PersonName = "John Doe",
            Email = "john@example.com"
        };

        var createResponse = await _httpClient.PostAsJsonAsync("/people", newPerson);
        var createdPerson = await createResponse.Content.ReadFromJsonAsync<PersonDto>();

        // Act
        var response = await _httpClient.DeleteAsync($"/people/{createdPerson.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify the delete
        var getResponse = await _httpClient.GetAsync($"/people/{createdPerson.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}