using FlatHunt.Server.Data;
using FlatHunt.Server.Services.FlatProviders.Lun.Interfaces;
using FlatHunt.Server.Services.Parser;
using FlatHunt.Server.Services.Parser.Interfaces;
using FlatHunt.Server.Services.Sync;
using FlatHunt.Server.Services.Sync.Interfaces;
using Microsoft.EntityFrameworkCore;
using Refit;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlatHunt.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


            builder.Services.AddTransient<IFlatSyncService, FlatSyncService>();
            builder.Services.AddTransient<IFlatParserService, LunFlatParserService>();

            var refitSettings = new RefitSettings(new SystemTextJsonContentSerializer(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters =
                {
                    new JsonStringEnumConverter()
                },
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            }));

            //TODO add handler
            builder.Services.AddRefitClient<ILunClient>(refitSettings)
              .ConfigureHttpClient((_, client) =>
              {
                  //todo add to settings 
                  client.BaseAddress = new Uri("https://lun.ua/");
              });

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.MapStaticAssets();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
