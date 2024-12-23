namespace CaminoManager.Data.Models;

public class Belong
{
    public Guid PersonId { get; set; }
    public Guid CommunityId { get; set; }
    public Guid TeamId { get; set; }
    public bool IsResponsibleForTheTeam { get; set; }

    // Navigation properties    
    public virtual Team Team { get; set; } = null!;
    public virtual Community Community { get; set; } = null!;
    public virtual Person Person { get; set; } = null!;
}
