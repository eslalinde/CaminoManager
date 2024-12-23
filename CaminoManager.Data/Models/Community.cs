using System.ComponentModel.DataAnnotations;

namespace CaminoManager.Data.Models;

public class Community
{
    [Key]
    public Guid Id { get; set; }
    public string Number { get; set; }
    public DateTime BornDate { get; set; }
    public Guid ParishId { get; set; }
    public int BornBrothers { get; set; }
    public int ActualBrothers { get; set; }
    public Guid StepWayId { get; set; }
    public DateTime? StepWayDate { get; set; }
    public Guid? CatechistTeamId { get; set; }
    public string? OldCatechist { get; set; }

    // Navigation property
    public virtual ICollection<Brother> Brothers { get; set; } = new List<Brother>();
}