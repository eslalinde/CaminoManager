namespace CaminoManager.ServiceDefaults.DTOs;

public class CountryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
}

public class CreateCountryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
}

public class UpdateCountryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
} 