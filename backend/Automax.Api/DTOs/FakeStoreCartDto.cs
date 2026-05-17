using System.Text.Json.Serialization;

namespace Automax.Api.DTOs;

public sealed record FakeStoreCartDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("userId")] int UserId,
    [property: JsonPropertyName("date")] DateTime Date,
    [property: JsonPropertyName("products")] IReadOnlyCollection<FakeStoreCartProductDto> Products);

public sealed record FakeStoreCartProductDto(
    [property: JsonPropertyName("productId")] int ProductId,
    [property: JsonPropertyName("quantity")] int Quantity);
