namespace CaminoManager.ServiceDefaults.Dtos;

public record ParishDto(
    Guid Id,
    string Name,
    string Diocese,
    string Address,
    string? Phone,
    string? Email,
    Guid CityId
);

public record CreateParishDto(
    string Name,
    string Diocese,
    string Address,
    string? Phone,
    string? Email,
    Guid CityId
);

public record UpdateParishDto(
    string Name,
    string Diocese,
    string Address,
    string? Phone,
    string? Email,
    Guid CityId
); 