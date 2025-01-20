namespace CaminoManager.ServiceDefaults.DTOs;

public class CityBaseDto
{
    public string Name { get; set; } = null!;
    public string StateId { get; set; } = null!;
    public string CountryId { get; set; } = null!;
}

public class CityDto : CityBaseDto
{
    public string Id { get; set; } = null!;
}

public class CreateCityDto : CityBaseDto
{
}

public class UpdateCityDto : CityBaseDto
{
}