using FlatHunt.Server.Data;
using FlatHunt.Server.Models;
using FlatHunt.Server.Repositories.Interfaces;

namespace FlatHunt.Server.Repositories
{
    public class AdvertisementRepository : Repository<Advertisement>, IAdvertisementRepository
    {
        public AdvertisementRepository(AppDbContext context) : base(context)
        {
        }
    }
}
