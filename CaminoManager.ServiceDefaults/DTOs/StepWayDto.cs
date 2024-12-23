namespace CaminoManager.ServiceDefaults.Dtos;

public class StepWayDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
}

public class CreateStepWayDto
{
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
}

public class UpdateStepWayDto
{
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
} 