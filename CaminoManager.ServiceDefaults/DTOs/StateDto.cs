namespace CaminoManager.ServiceDefaults.DTOs;

public class StateBaseDto
{
    public string Name { get; set; } = null!;
    public string CountryId { get; set; } = null!;
}

public class StateDto : StateBaseDto
{
    public string Id { get; set; } = null!;
}

public class CreateStateDto : StateBaseDto
{
}

public class UpdateStateDto : StateBaseDto
{
}