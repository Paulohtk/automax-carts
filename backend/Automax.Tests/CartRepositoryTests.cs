using Automax.Api.Data;
using Automax.Api.Models;
using Automax.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Automax.Tests;

public sealed class CartRepositoryTests
{
    [Fact]
    public async Task UpsertManyAsync_PersistsCartWithProducts()
    {
        await using var dbContext = CreateDbContext();
        var repository = new CartRepository(dbContext);
        var carts = new[]
        {
            new Cart
            {
                Id = 1,
                UserId = 2,
                Date = new DateTime(2020, 03, 02),
                Products =
                [
                    new CartProduct { CartId = 1, ProductId = 10, Quantity = 3 },
                    new CartProduct { CartId = 1, ProductId = 20, Quantity = 1 }
                ]
            }
        };

        await repository.UpsertManyAsync(carts, CancellationToken.None);

        var savedCart = await repository.GetByIdAsync(1, CancellationToken.None);
        Assert.NotNull(savedCart);
        Assert.Equal(2, savedCart.UserId);
        Assert.Equal(4, savedCart.Products.Sum(product => product.Quantity));
    }

    private static AutomaxDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AutomaxDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AutomaxDbContext(options);
    }
}
