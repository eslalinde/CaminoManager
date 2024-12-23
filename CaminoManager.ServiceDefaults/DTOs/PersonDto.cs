namespace CaminoManager.ServiceDefaults.DTOs;

public class PersonDto
{
    public Guid Id { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int PersonTypeId { get; set; }
    public string PersonTypeName { get; set; } = string.Empty;
    public int GenderId { get; set; }
    public string GenderName { get; set; } = string.Empty;
}

public class PersonDetailDto : PersonDto
{
    public Guid? SpouseId { get; set; }
}