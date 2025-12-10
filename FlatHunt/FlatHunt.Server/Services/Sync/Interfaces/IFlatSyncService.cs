using FlatHunt.Server.Models;
using FlatHunt.Server.Services.Sync.Models;

namespace FlatHunt.Server.Services.Sync.Interfaces
{
    public interface IFlatSyncService
    {
        //TODO use jobs
        Task<List<Advertisement>> SyncFlats(FlatSyncFilter filter);
    }
}
