namespace CaminoManager.ServiceDefaults.Dtos;

public class TeamTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
}

public class CreateTeamTypeDto
{
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
}

public class UpdateTeamTypeDto
{
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
} 