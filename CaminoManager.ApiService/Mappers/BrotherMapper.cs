using Riok.Mapperly.Abstractions;
using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.DTOs;

namespace CaminoManager.ApiService.Mappers;

[Mapper]
public partial class BrotherMapper
{
    public partial BrotherDto ToDto(Brother brother);
    public partial Brother ToEntity(CreateBrotherDto dto);
    public partial void UpdateEntity(UpdateBrotherDto dto, Brother entity);
} 