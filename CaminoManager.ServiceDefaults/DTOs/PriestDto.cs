namespace CaminoManager.ServiceDefaults.DTOs
{
    public class PriestBaseDto
    {
        public string Name { get; set; }
        public string ParishId { get; set; }
        public bool IsParishPriest { get; set; }
        public string PersonId { get; set; }
    }

    public class PriestDto : PriestBaseDto
    {
        public string Id { get; set; }
        public string ParishName { get; set; }
    }

    public class CreatePriestDto : PriestBaseDto
    {
        // Add properties specific to creation, if any
    }

    public class UpdatePriestDto : PriestDto
    {
        // Add properties specific to updates, if any
    }
} 