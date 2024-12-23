using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.DTOs;
using Riok.Mapperly.Abstractions;

namespace CaminoManager.ApiService.Mappers;

[Mapper]
public partial class CountryMapper
{
    public partial CountryDto ToDto(Country entity);
    
    public partial Country ToEntity(CreateCountryDto dto);
    
    [MapperIgnoreTarget(nameof(Country.Id))]
    public partial void UpdateEntity(UpdateCountryDto dto, Country entity);
} 