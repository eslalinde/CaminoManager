using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.Dtos;
using Riok.Mapperly.Abstractions;

namespace CaminoManager.ApiService.Mappers;

[Mapper]
public partial class TeamTypeMapper
{
    public partial TeamTypeDto ToDto(TeamType model);
    public partial TeamType ToModel(CreateTeamTypeDto dto);
    public partial void UpdateModel(UpdateTeamTypeDto dto, TeamType model);
}