using System;

namespace CaminoManager.Data.Models;

public class ParishTeam
{
    public Guid ParishId { get; set; }
    public Guid TeamId { get; set; }

    // Navigation properties
    public Parish Parish { get; set; } = null!;
    public Team Team { get; set; } = null!;
}
