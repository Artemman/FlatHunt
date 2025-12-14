using FlatHunt.Server.Services.Flats;
using FlatHunt.Server.Services.Flats.Interfaces;

namespace FlatHunt.Server.DI
{
    public static class FlatHuntBusinessRegistrator
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IFlatsService, FlatsService>();
        }
    }
}
