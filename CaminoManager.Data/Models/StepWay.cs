using System.ComponentModel.DataAnnotations;
namespace CaminoManager.Data.Models;

public class StepWay
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
}
