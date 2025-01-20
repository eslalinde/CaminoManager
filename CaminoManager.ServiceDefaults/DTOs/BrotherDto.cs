namespace CaminoManager.ServiceDefaults.DTOs;

public class BrotherBaseDto
{
    public Guid PersonId { get; init; }
    public Guid CommunityId { get; init; }
}

public class BrotherDto : BrotherBaseDto;

public class CreateBrotherDto : BrotherBaseDto;

public class UpdateBrotherDto : BrotherBaseDto; 