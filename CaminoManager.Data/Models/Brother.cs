namespace CaminoManager.Data.Models;

public class Brother
{
    public Guid PersonId { get; set; }
    public Guid CommunityId { get; set; }

    // Navigation properties
    public virtual Person Person { get; set; } = null!;
    public virtual Community Community { get; set; } = null!;
    public virtual ICollection<Belong> Belongs { get; set; } = new List<Belong>();
}
