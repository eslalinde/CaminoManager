using System;
using System.ComponentModel.DataAnnotations;

namespace CaminoManager.Data.Models;

public class City
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid StateId { get; set; }

    public Guid CountryId { get; set; }

    // Navigation properties
    public State State { get; set; } = null!;
    public Country Country { get; set; } = null!;
}
