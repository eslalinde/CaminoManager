using System;
using System.ComponentModel.DataAnnotations;

namespace CaminoManager.Data.Models;

public class State
{
    [Key]
    public Guid Id { get; set; }    
    public string Name { get; set; }    
    public Guid CountryId { get; set; }

    // Navigation properties
    public Country Country { get; set; } = null!;
    public ICollection<City> Cities { get; set; } = new List<City>();
}
