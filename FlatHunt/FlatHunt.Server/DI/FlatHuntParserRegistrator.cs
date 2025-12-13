using FlatHunt.Server.Services.Parser;
using FlatHunt.Server.Services.Parser.Interfaces;
using FlatHunt.Server.Services.Sync;
using FlatHunt.Server.Services.Sync.Interfaces;

namespace FlatHunt.Server.DI
{
    public static class FlatHuntParserRegistrator 
    {
        public static void AddParserServices(this IServiceCollection services)
        {
            services.AddTransient<IFlatSyncService, FlatSyncService>();
            services.AddTransient<ILunFlatParserService, LunFlatParserService>();
        }
    }
}
