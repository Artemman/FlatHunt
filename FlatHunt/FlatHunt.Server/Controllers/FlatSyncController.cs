using FlatHunt.Server.Models;
using FlatHunt.Server.Services.Sync.Interfaces;
using FlatHunt.Server.Services.Sync.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlatHunt.Server.Controllers
{
    //todo add auth
    [ApiController]
    [Route("flat-sync")]
    public class FlatSyncController : ControllerBase
    {
        private readonly IFlatSyncService _flatSyncService;

        public FlatSyncController(IFlatSyncService flatSyncService)
        {
            _flatSyncService = flatSyncService;
        }

        [HttpPost]
        public Task<List<Advertisement>> SyncFlats(FlatSyncFilter filter)
        {
            return _flatSyncService.SyncFlats(filter);
        }
    }
}
