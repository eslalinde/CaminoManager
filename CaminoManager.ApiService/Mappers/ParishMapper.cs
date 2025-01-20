using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.DTOs;
using Riok.Mapperly.Abstractions;

namespace CaminoManager.ApiService.Mappers;

[Mapper]
public partial class ParishMapper
{
    [MapProperty(nameof(Parish.Id), nameof(ParishDto.Id))]
    public partial ParishDto ToDto(Parish parish);

    [MapProperty(nameof(CreateParishDto.CityId), nameof(Parish.CityId))]
    public partial Parish ToEntity(CreateParishDto createParishDto);

    [MapProperty(nameof(UpdateParishDto.CityId), nameof(Parish.CityId))]
    public partial void UpdateEntity(UpdateParishDto updateParishDto, Parish parish);

    private string MapGuidToString(Guid guid) => guid.ToString();
    private Guid MapStringToGuid(string guidString) => Guid.Parse(guidString);
}