using FlatHunt.Server.DI;

namespace FlatHunt.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services, builder.Configuration);

            builder.Services.AddCors(b => b.AddDefaultPolicy(p =>
                   p.WithOrigins("https://localhost:60107")
                   .AllowCredentials()
                   .AllowAnyMethod()
                   .AllowAnyHeader())
            );
            builder.Services.AddControllers();
            AuthRegistrator.AddAuth(builder.Services, builder.Configuration);
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

            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            DbContextRegistrator.ConfigureDbContext(services, configuration);
            FlatHuntParserRegistrator.AddParserServices(services);
            FlatSourceClientsRegistrator.AddFlatSourceClients(services, configuration);
            RepositoriesRegistrator.AddRepositories(services);
            FlatHuntBusinessRegistrator.ConfigureServices(services);
        }
    }
}
