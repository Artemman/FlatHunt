using FlatHunt.Server.Auth;
using FlatHunt.Server.Services.Auth;
using FlatHunt.Server.Services.Auth.Interfaces;
using FlatHunt.Server.Services.Auth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
                                opt.MapInboundClaims = false;
                                opt.TokenValidationParameters = new()
                                {
                                    ValidIssuer = jwt.Issuer,
                                    ValidAudience = jwt.Audience,
                                    IssuerSigningKey = key,
                                    ValidateIssuerSigningKey = true,
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ClockSkew = TimeSpan.Zero,
                                    RoleClaimType = CustomClaimTypes.Role,
                                    NameClaimType = CustomClaimTypes.Name
                                };
                            });

            services.AddAuthorization();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
        }
    }
}
