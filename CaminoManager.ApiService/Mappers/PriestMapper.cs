using Riok.Mapperly.Abstractions;
using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.DTOs;

namespace CaminoManager.ApiService.Mappers;

[Mapper]
public partial class PriestMapper
{
    [MapProperty(nameof(Priest.Parish.Name), nameof(PriestDto.ParishName))]
    [MapProperty(nameof(Priest.Person.PersonName), nameof(PriestDto.Name))]
    public partial PriestDto ToDto(Priest priest);
    public partial Priest ToEntity(CreatePriestDto dto);
    public partial void UpdateEntity(UpdatePriestDto dto, Priest priest);

    private string MapGuidToString(Guid guid) => guid.ToString();
    private Guid MapStringToGuid(string guidString) => Guid.Parse(guidString);
}