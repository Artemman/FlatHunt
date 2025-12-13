using Refit;
using System.Text.Json;
using System.Text.Json.Serialization;
using FlatHunt.Server.Handlers;
using FlatHunt.Server.Services.FlatProviders.Lun.Interfaces;

namespace FlatHunt.Server.DI
{
    public static class FlatSourceClientsRegistrator
    {
        public static void AddFlatSourceClients(this IServiceCollection services, IConfiguration configuration)
        {
            var lunApiUrl = configuration.GetValue<string>("LunApi:Url");

            ArgumentNullException.ThrowIfNull(lunApiUrl);
            services.AddRefitClient<ILunClient>(GetRefitSettings())
             .ConfigureHttpClient((_, client) =>
             {
                 client.BaseAddress = new Uri(lunApiUrl);
             })
             .AddHttpMessageHandler<LunMessageHandler>();
        }

        private static RefitSettings GetRefitSettings()
        {
            return new RefitSettings(new SystemTextJsonContentSerializer(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters =
                {
                    new JsonStringEnumConverter()
                },
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            }));
        }
    }
}
