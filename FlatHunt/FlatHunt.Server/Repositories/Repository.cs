using FlatHunt.Server.Data;
using FlatHunt.Server.Models;
using FlatHunt.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FlatHunt.Server.Repositories
{
    public class Repository<TEntity>(AppDbContext context) : IRepository<TEntity>
        where TEntity : Entity
    {
        protected readonly AppDbContext _context = context;
        protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

        public virtual TEntity Add(TEntity entity)
        {
            _dbSet.Add(entity);

            return entity;
        }

        public Task<TEntity?> GetById(int entityId, params Expression<Func<TEntity, object>>[] includes)
        {
            return Get(c => c.Id.Equals(entityId), includes);
        }

        public Task<TEntity?> Get(Expression<Func<TEntity, bool>> criteria, params Expression<Func<TEntity, object>>[] includes)
        {
            return Query(includes)
                .Where(criteria)
                .FirstOrDefaultAsync();
        }

        protected virtual IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();

            if (includes == null)
            {
                return query;
            }

            if (includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            

            return query;
        }

        public Task SaveChanges()
        {
            return _context.SaveChangesAsync();
        }

        public Task<List<TEntity>> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            return Query(includes).ToListAsync();
        }
    }
}
