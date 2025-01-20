namespace CaminoManager.ServiceDefaults.DTOs;

public class TeamTypeDto : TeamTypeBaseDto
{
    public Guid Id { get; set; }
}

public class TeamTypeBaseDto
{
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
}

public class CreateTeamTypeDto : TeamTypeBaseDto
{
}

public class UpdateTeamTypeDto : TeamTypeBaseDto
{
}