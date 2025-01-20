namespace CaminoManager.ServiceDefaults.DTOs;

public class StepWayDto : StepWayBaseDto
{
    public Guid Id { get; set; }
}

public class StepWayBaseDto
{
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
}

public class CreateStepWayDto : StepWayBaseDto
{
}

public class UpdateStepWayDto : StepWayBaseDto
{
}