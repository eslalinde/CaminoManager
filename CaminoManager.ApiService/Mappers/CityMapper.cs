using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.DTOs;
using Riok.Mapperly.Abstractions;

namespace CaminoManager.ApiService.Mappers;

[Mapper]
public partial class CityMapper
{
    public partial CityDto ToDto(City city);
    public partial City ToEntity(CreateCityDto dto);
    public partial void UpdateEntity(UpdateCityDto dto, City entity);

    private string MapGuidToString(Guid guid) => guid.ToString();
    private Guid MapStringToGuid(string guidString) => Guid.Parse(guidString);
}