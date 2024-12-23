using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.Dtos;
using Riok.Mapperly.Abstractions;

namespace CaminoManager.ApiService.Mappers;

[Mapper]
public partial class ParishMapper
{
    public partial ParishDto ToDto(Parish parish);
    public partial Parish ToEntity(CreateParishDto createParishDto);
    public partial void UpdateEntity(UpdateParishDto updateParishDto, Parish parish);
}