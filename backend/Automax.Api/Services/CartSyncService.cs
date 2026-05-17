using System.Text.Json;
using Automax.Api.Configuration;
using Automax.Api.DTOs;
using Automax.Api.Models;
using Automax.Api.Repositories;
using Microsoft.Extensions.Options;

namespace Automax.Api.Services;

public sealed class CartSyncService(
    HttpClient httpClient,
    ICartRepository cartRepository,
    IOptions<FakeStoreOptions> options) : ICartSyncService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<int> SyncAsync(CancellationToken cancellationToken)
    {
        var cartsUrl = options.Value.CartsUrl;
        using var response = await httpClient.GetAsync(cartsUrl, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var externalCarts = await JsonSerializer.DeserializeAsync<List<FakeStoreCartDto>>(
            stream,
            JsonOptions,
            cancellationToken) ?? [];

        var carts = externalCarts.Select(cart => new Cart
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Date = cart.Date,
            Products = cart.Products.Select(product => new CartProduct
            {
                CartId = cart.Id,
                ProductId = product.ProductId,
                Quantity = product.Quantity
            }).ToList()
        });

        await cartRepository.UpsertManyAsync(carts, cancellationToken);
        return externalCarts.Count;
    }
}
