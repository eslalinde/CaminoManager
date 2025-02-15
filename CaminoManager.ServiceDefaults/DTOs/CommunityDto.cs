namespace CaminoManager.ServiceDefaults.DTOs;

public class CommunityDtoBase
{
    public string Number { get; set; } = string.Empty;
    public DateTime BornDate { get; set; }
    public string ParishId { get; set; } = string.Empty;
    public int BornBrothers { get; set; }
    public int ActualBrothers { get; set; }
    public List<BrotherDto> Brothers { get; set; } = new();
    public string StepWayId { get; set; } = string.Empty;
    public DateTime? StepWayDate { get; set; }
    public string? CatechistTeamId { get; set; }
    public string? OldCatechist { get; set; }
}

public class CommunityDto : CommunityDtoBase
{
    public string Id { get; set; } = string.Empty;
}

public class CreateCommunityDto : CommunityDtoBase
{
}

public class UpdateCommunityDto : CreateCommunityDto
{
    public string Id { get; set; } = string.Empty;
}