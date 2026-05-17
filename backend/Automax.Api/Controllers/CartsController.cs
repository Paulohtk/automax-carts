using Automax.Api.DTOs;
using Automax.Api.Models;
using Automax.Api.Repositories;
using Automax.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Automax.Api.Controllers;

[ApiController]
[Route("carts")]
public sealed class CartsController(
    ICartRepository cartRepository,
    ICartSyncService cartSyncService,
    ILogger<CartsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CartDto>>> GetAll(CancellationToken cancellationToken)
    {
        var carts = await cartRepository.GetAllAsync(cancellationToken);
        return Ok(carts.Select(ToDto).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CartDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetByIdAsync(id, cancellationToken);

        if (cart is null)
        {
            return NotFound();
        }

        return Ok(ToDto(cart));
    }

    [HttpPost("sync")]
    public async Task<ActionResult<object>> Sync(CancellationToken cancellationToken)
    {
        try
        {
            var syncedCount = await cartSyncService.SyncAsync(cancellationToken);
            return Ok(new { syncedCount });
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Failed to synchronize carts from Fake Store API.");
            return StatusCode(StatusCodes.Status502BadGateway, new { message = "Erro ao consultar a Fake Store API." });
        }
    }

    private static CartDto ToDto(Cart cart)
    {
        var products = cart.Products
            .Select(product => new CartProductDto(product.ProductId, product.Quantity))
            .ToList();

        return new CartDto(
            cart.Id,
            cart.Date,
            cart.UserId,
            products.Sum(product => product.Quantity),
            products);
    }
}
