namespace Automax.Api.Services;

public interface ICartSyncService
{
    Task<int> SyncAsync(CancellationToken cancellationToken);
}
