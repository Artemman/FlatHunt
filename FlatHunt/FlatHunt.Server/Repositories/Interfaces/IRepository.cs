using FlatHunt.Server.Models;
using System.Linq.Expressions;

namespace FlatHunt.Server.Repositories.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : Entity
    {
        TEntity? Add(TEntity entity);

        Task<TEntity?> GetById(int entityId, params Expression<Func<TEntity, object>>[] includes);
    }
}
