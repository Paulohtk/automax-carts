using Automax.Api.Controllers;
using Automax.Api.DTOs;
using Automax.Api.Models;
using Automax.Api.Repositories;
using Automax.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace Automax.Tests;

public sealed class CartsControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsStoredCarts()
    {
        var controller = CreateController([
            new Cart
            {
                Id = 1,
                UserId = 2,
                Date = new DateTime(2020, 03, 02),
                Products =
                [
                    new CartProduct { ProductId = 10, Quantity = 3 },
                    new CartProduct { ProductId = 20, Quantity = 1 }
                ]
            }
        ]);

        var result = await controller.GetAll(CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var carts = Assert.IsAssignableFrom<IReadOnlyCollection<CartDto>>(okResult.Value);
        var cart = Assert.Single(carts);
        Assert.Equal(1, cart.Id);
        Assert.Equal(4, cart.TotalProducts);
    }

    [Fact]
    public async Task GetById_ReturnsNotFoundWhenCartDoesNotExist()
    {
        var controller = CreateController([]);

        var result = await controller.GetById(99, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    private static CartsController CreateController(IReadOnlyCollection<Cart> carts)
    {
        return new CartsController(
            new InMemoryCartRepository(carts),
            new NoOpCartSyncService(),
            NullLogger<CartsController>.Instance);
    }

    private sealed class InMemoryCartRepository(IReadOnlyCollection<Cart> carts) : ICartRepository
    {
        public Task<IReadOnlyCollection<Cart>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(carts);
        }

        public Task<Cart?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return Task.FromResult(carts.FirstOrDefault(cart => cart.Id == id));
        }

        public Task UpsertManyAsync(IEnumerable<Cart> cartsToSave, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class NoOpCartSyncService : ICartSyncService
    {
        public Task<int> SyncAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
