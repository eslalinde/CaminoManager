using Riok.Mapperly.Abstractions;
using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.Dtos;

namespace CaminoManager.ApiService.Mappers;

[Mapper]
public partial class StateMapper
{
    public partial StateDto ToDto(State state);
    public partial State ToEntity(CreateStateDto dto);
    public partial void UpdateEntity(UpdateStateDto dto, State entity);
} 