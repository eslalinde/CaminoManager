using System.ComponentModel.DataAnnotations;

namespace CaminoManager.Data.Models;

public class Parish
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Diocese { get; set; }
    public string Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public Guid CityId { get; set; }

    // Navigation properties
    public City City { get; set; } = null!;
    public ICollection<ParishTeam> ParishTeams { get; set; } = new List<ParishTeam>();
    public ICollection<Priest> Priests { get; set; } = new List<Priest>();
}
