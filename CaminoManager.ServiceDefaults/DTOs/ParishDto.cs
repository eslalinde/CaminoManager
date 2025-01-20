public class ParishBaseDto
{
    public string Name { get; set; }
    public string Diocese { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string CityId { get; set; }
    public List<ParishPriestDto> Priests { get; set; } = new();
}

public class ParishDto : ParishBaseDto
{
    public string Id { get; set; }
    public string CityName { get; set; }
}

public class ParishPriestDto
{
    public string PersonId { get; set; }
    public string PersonName { get; set; }
    public bool IsParishPriest { get; set; }
}

public class CreateParishDto : ParishBaseDto
{
}

public class UpdateParishDto : ParishBaseDto
{
} 