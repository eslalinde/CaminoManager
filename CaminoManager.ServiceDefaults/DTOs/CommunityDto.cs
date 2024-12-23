namespace CaminoManager.ServiceDefaults.DTOs;

public class CommunityDto
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime BornDate { get; set; }
    public Guid ParishId { get; set; }
    public int BornBrothers { get; set; }
    public int ActualBrothers { get; set; }
    public Guid StepWayId { get; set; }
    public DateTime? StepWayDate { get; set; }
    public Guid? CatechistTeamId { get; set; }
    public string? OldCatechist { get; set; }
}

public class CreateCommunityDto
{
    public string Number { get; set; } = string.Empty;
    public DateTime BornDate { get; set; }
    public Guid ParishId { get; set; }
    public int BornBrothers { get; set; }
    public int ActualBrothers { get; set; }
    public Guid StepWayId { get; set; }
    public DateTime? StepWayDate { get; set; }
    public Guid? CatechistTeamId { get; set; }
    public string? OldCatechist { get; set; }
}

public class UpdateCommunityDto : CreateCommunityDto
{
    public Guid Id { get; set; }
}