using System.ComponentModel.DataAnnotations;

namespace CaminoManager.Data.Models;

public class Country
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Code { get; set; }

    // Navigation properties
    public ICollection<State> States { get; set; } = new List<State>();
    public ICollection<City> Cities { get; set; } = new List<City>();
}
