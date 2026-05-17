using Automax.Api.Data;
using Automax.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Automax.Api.Repositories;

public sealed class CartRepository(AutomaxDbContext dbContext) : ICartRepository
{
    public async Task<IReadOnlyCollection<Cart>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Carts
            .AsNoTracking()
            .Include(cart => cart.Products)
            .OrderBy(cart => cart.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Cart?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await dbContext.Carts
            .AsNoTracking()
            .Include(cart => cart.Products)
            .FirstOrDefaultAsync(cart => cart.Id == id, cancellationToken);
    }

    public async Task UpsertManyAsync(IEnumerable<Cart> carts, CancellationToken cancellationToken)
    {
        foreach (var cart in carts)
        {
            var currentCart = await dbContext.Carts
                .Include(existingCart => existingCart.Products)
                .FirstOrDefaultAsync(existingCart => existingCart.Id == cart.Id, cancellationToken);

            if (currentCart is null)
            {
                dbContext.Carts.Add(cart);
                continue;
            }

            currentCart.UserId = cart.UserId;
            currentCart.Date = cart.Date;
            dbContext.CartProducts.RemoveRange(currentCart.Products);
            foreach (var product in cart.Products)
            {
                product.CartId = currentCart.Id;
            }

            currentCart.Products = cart.Products;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
