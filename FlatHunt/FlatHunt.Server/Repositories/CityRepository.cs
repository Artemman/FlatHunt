using FlatHunt.Server.Data;
using FlatHunt.Server.Models;
using FlatHunt.Server.Repositories.Interfaces;

namespace FlatHunt.Server.Repositories
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(AppDbContext context) : base(context)
        {
        }
    }
}
