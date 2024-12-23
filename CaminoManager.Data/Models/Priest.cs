using System;
using System.ComponentModel.DataAnnotations;
namespace CaminoManager.Data.Models;

public class Priest
{
    [Key]
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }    
    public bool IsParishPriest { get; set; }

    // Navigation properties
    public Person Person { get; set; } = null!;
    public Parish Parish { get; set; } = null!;
}
