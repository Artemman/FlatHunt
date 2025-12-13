using FlatHunt.Server.Auth;
using FlatHunt.Server.Data;
using FlatHunt.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlatHunt.Server.DI
{
    public static class DbContextRegistrator
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("Default")));

            ConfigureIdentity(services);
        }

        private static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentityCore<User>(o => { o.User.RequireUniqueEmail = true; })
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddHostedService<RoleSeeder>();
        }
    }
}
