using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.Dtos;
using Riok.Mapperly.Abstractions;

namespace CaminoManager.ApiService.Mappers;

[Mapper]
public partial class CityMapper
{
    public partial CityDto ToDto(City city);
    public partial City ToEntity(CreateCityDto dto);
    public partial void UpdateEntity(UpdateCityDto dto, City entity);
}