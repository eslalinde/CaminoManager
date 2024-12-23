using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.DTOs;
using Riok.Mapperly.Abstractions;

namespace CaminoManager.ApiService.Mappers;

[Mapper]
public partial class PersonMapper
{
    public partial PersonDto ToDto(Person person);

    public partial PersonDetailDto ToDetailDto(Person person);

    public partial Person ToEntity(PersonDto dto);

    //private string MapPersonTypeName(PersonType personType) => personType.ToString();
    //private string MapGenderName(GenderType gender) => gender.ToString();
}