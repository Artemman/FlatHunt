using FlatHunt.Server.Repositories;
using FlatHunt.Server.Repositories.Interfaces;

namespace FlatHunt.Server.DI
{
    public static class RepositoriesRegistrator
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICityRepository, CityRepository>();
            services.AddTransient<IAdvertisementRepository, AdvertisementRepository>();
            services.AddTransient<IFlatRepository, FlatRepository>();
        }
    }
}
