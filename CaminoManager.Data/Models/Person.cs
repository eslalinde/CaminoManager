using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NpgsqlTypes;

namespace CaminoManager.Data.Models;

public class Person
{
    [Key]
    public Guid Id { get; set; }
    public string PersonName { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public PersonType PersonTypeId { get; set; } = PersonType.Single;
    public GenderType GenderId { get; set; }

    // Navigation properties
    public virtual Person? Spouse { get; set; }
    public virtual ICollection<Brother> Brothers { get; set; } = new List<Brother>();

    // Add this property for full-text search
    public NpgsqlTsVector? SearchVector { get; set; }
}