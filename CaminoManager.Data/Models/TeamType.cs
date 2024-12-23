using System.ComponentModel.DataAnnotations;
namespace CaminoManager.Data.Models;

public class TeamType
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }

    // Navigation properties
    public ICollection<Team> Teams { get; set; } = new List<Team>();
}
