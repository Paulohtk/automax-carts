using System.Net;
using Automax.Api.Configuration;
using Automax.Api.Models;
using Automax.Api.Repositories;
using Automax.Api.Services;
using Microsoft.Extensions.Options;

namespace Automax.Tests;

public sealed class CartSyncServiceTests
{
    [Fact]
    public async Task SyncAsync_FetchesExternalCartsAndStoresThem()
    {
        const string responseJson = """
        [
          {
            "id": 1,
            "userId": 2,
            "date": "2020-03-02T00:00:00.000Z",
            "products": [
              { "productId": 10, "quantity": 3 },
              { "productId": 20, "quantity": 1 }
            ]
          }
        ]
        """;
        var repository = new InMemoryCartRepository();
        var service = new CartSyncService(
            new HttpClient(new JsonResponseHandler(responseJson)),
            repository,
            Options.Create(new FakeStoreOptions { CartsUrl = "https://example.test/carts" }));

        var syncedCount = await service.SyncAsync(CancellationToken.None);

        Assert.Equal(1, syncedCount);
        var cart = Assert.Single(repository.Carts);
        Assert.Equal(1, cart.Id);
        Assert.Equal(2, cart.UserId);
        Assert.Equal(4, cart.Products.Sum(product => product.Quantity));
    }

    private sealed class InMemoryCartRepository : ICartRepository
    {
        public List<Cart> Carts { get; } = [];

        public Task<IReadOnlyCollection<Cart>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<IReadOnlyCollection<Cart>>(Carts);
        }

        public Task<Cart?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return Task.FromResult(Carts.FirstOrDefault(cart => cart.Id == id));
        }

        public Task UpsertManyAsync(IEnumerable<Cart> carts, CancellationToken cancellationToken)
        {
            Carts.Clear();
            Carts.AddRange(carts);
            return Task.CompletedTask;
        }
    }

    private sealed class JsonResponseHandler(string json) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };

            return Task.FromResult(response);
        }
    }
}
