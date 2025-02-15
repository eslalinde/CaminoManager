using Riok.Mapperly.Abstractions;
using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.DTOs;

namespace CaminoManager.ApiService.Mappers;

[Mapper]
public partial class CommunityMapper
{
    public partial CommunityDto ToCommunityDto(Community community);
    
    [MapProperty(nameof(CreateCommunityDto.ParishId), nameof(Community.ParishId))]
    [MapProperty(nameof(CreateCommunityDto.StepWayId), nameof(Community.StepWayId))]
    [MapProperty(nameof(CreateCommunityDto.CatechistTeamId), nameof(Community.CatechistTeamId))]
    public partial Community ToEntity(CreateCommunityDto dto);
    
    public partial void UpdateEntity(UpdateCommunityDto dto, Community entity);
} 