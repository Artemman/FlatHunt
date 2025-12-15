using FlatHunt.Server.Models;
using FlatHunt.Server.Services.Sync.Models;

namespace FlatHunt.Server.Services.Sync.Interfaces
{
    public interface IFlatSyncService
    {
        //TODO use jobs
        Task<int> SyncFlats(FlatSyncFilter filter);
    }
}
