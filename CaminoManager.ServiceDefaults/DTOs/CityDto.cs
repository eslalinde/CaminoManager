namespace CaminoManager.ServiceDefaults.Dtos;

public class CityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid StateId { get; set; }
    public Guid CountryId { get; set; }
}

public class CreateCityDto
{
    public string Name { get; set; } = null!;
    public Guid StateId { get; set; }
    public Guid CountryId { get; set; }
}

public class UpdateCityDto
{
    public string Name { get; set; } = null!;
    public Guid StateId { get; set; }
    public Guid CountryId { get; set; }
} 