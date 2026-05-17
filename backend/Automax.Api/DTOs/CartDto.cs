namespace Automax.Api.DTOs;

public sealed record CartDto(
    int Id,
    DateTime Date,
    int UserId,
    int TotalProducts,
    IReadOnlyCollection<CartProductDto> Products);
