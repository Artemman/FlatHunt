using FlatHunt.Server.Models;
using FlatHunt.Server.Services.Parser.Models;

namespace FlatHunt.Server.Services.Sync.Models
{
    public class FlatSyncFilter : FlatParseFilter
    {
        public FlatSourceType FlatSourceType { get; set; }

    }
}
