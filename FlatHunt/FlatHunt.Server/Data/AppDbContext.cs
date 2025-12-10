using FlatHunt.Server.Auth;
using FlatHunt.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlatHunt.Server.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public DbSet<Advertisement> Advertisements => Set<Advertisement>();

        public DbSet<Flat> Flats => Set<Flat>();

        public DbSet<City> Cities => Set<City>();

        public DbSet<LunCity> LunCities => Set<LunCity>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);
            b.Entity<RefreshToken>(cfg =>
            {
                cfg.HasKey(x => x.Id);
                cfg.HasIndex(x => x.Token).IsUnique();
                cfg.Property(x => x.Token).IsRequired();
            });

            b.Entity<Advertisement>(cfg =>
            {
                cfg.HasKey(x => x.Id);
                cfg.Property(x => x.ExternalId).HasMaxLength(100);
                cfg.Property(x => x.Url).HasMaxLength(500);
                cfg.Property(x => x.Title).HasMaxLength(200);
                cfg.Property(x => x.Currency).HasMaxLength(50);
                cfg.Property(x => x.Address).HasMaxLength(200);
                cfg.Property(x => x.RawData).HasMaxLength(500);
            });

            b.Entity<City>(cfg =>
            {
                cfg.HasKey(x => x.Id);
                cfg.Property(x => x.Name).HasMaxLength(50);
            });

            b.Entity<LunCity>(cfg =>
            {
                cfg.HasKey(x => x.Id);
                cfg.Property(x => x.Name).HasMaxLength(50);
                cfg.Property(x => x.GeoType).HasMaxLength(50);
                cfg.Property(x => x.Url).HasMaxLength(150);
            });
        }
    }
}
