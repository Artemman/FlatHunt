using System.Text;
using FlatHunt.Server.Services.Auth;
using Microsoft.IdentityModel.Tokens;
using FlatHunt.Server.Services.Auth.Models;
using FlatHunt.Server.Services.Auth.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FlatHunt.Server.DI
{
    public static class AuthRegistrator
    {
        private const string JwtOptionsSection = "Jwt";
        public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptionsSection));
            var jwt = configuration.GetSection("Jwt").Get<JwtOptions>();
            if (string.IsNullOrEmpty(jwt?.Key))
            {
                throw new ArgumentException("JWT Key is not configured.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(opt =>
                            {
                                opt.TokenValidationParameters = new()
                                {
                                    ValidIssuer = jwt.Issuer,
                                    ValidAudience = jwt.Audience,
                                    IssuerSigningKey = key,
                                    ValidateIssuerSigningKey = true,
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ClockSkew = TimeSpan.Zero
                                };
                            });

            services.AddAuthorization();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
        }
    }
}
