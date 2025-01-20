using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.DTOs;
using Riok.Mapperly.Abstractions;

namespace CaminoManager.ApiService.Mappers;

[Mapper]
public partial class StepWayMapper
{
    public partial StepWayDto ToDto(StepWay entity);
    public partial StepWay ToEntity(CreateStepWayDto dto);
    public partial void UpdateEntity(UpdateStepWayDto dto, StepWay entity);
}