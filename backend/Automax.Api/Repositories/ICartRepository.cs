using Automax.Api.Models;

namespace Automax.Api.Repositories;

public interface ICartRepository
{
    Task<IReadOnlyCollection<Cart>> GetAllAsync(CancellationToken cancellationToken);

    Task<Cart?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task UpsertManyAsync(IEnumerable<Cart> carts, CancellationToken cancellationToken);
}
