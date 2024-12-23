using System.ComponentModel.DataAnnotations;

namespace CaminoManager.Data.Models;

public class Team
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid TeamTypeId { get; set; }
    public Guid? CommunityId { get; set; }

    // Navigation properties
    public TeamType TeamType { get; set; } = null!;
    public Community? Community { get; set; }
    public ICollection<ParishTeam> ParishTeams { get; set; } = new List<ParishTeam>();
}
