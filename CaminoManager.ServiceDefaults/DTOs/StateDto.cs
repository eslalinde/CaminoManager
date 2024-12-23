namespace CaminoManager.ServiceDefaults.DTOs;

public class StateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid CountryId { get; set; }
}

public class CreateStateDto
{
    public string Name { get; set; } = null!;
    public Guid CountryId { get; set; }
}

public class UpdateStateDto
{
    public string Name { get; set; } = null!;
    public Guid CountryId { get; set; }
}