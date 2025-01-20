namespace CaminoManager.ServiceDefaults.DTOs;

public class CountryDto : CountryBaseDto
{
    public string Id { get; set; }
}

public class CountryBaseDto
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
}

public class CreateCountryDto : CountryBaseDto
{
}

public class UpdateCountryDto : CountryBaseDto
{
}