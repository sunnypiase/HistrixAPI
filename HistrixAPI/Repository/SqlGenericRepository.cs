using HistrixAPI.DB;
using HistrixAPI.Models.Entities;
using HistrixAPI.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace HistrixAPI.Repository
{
    public class SqlGenericRepository<TEntity>
        : IGenericRepository<TEntity>
        where TEntity : class, IEntity
    {
        protected readonly DataContext _dataContext;
        protected readonly DbSet<TEntity> _entity;
        public SqlGenericRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            _entity = _dataContext.Set<TEntity>();
        }
        public virtual async Task<bool> DeleteAsync(int id)
        {
            TEntity? entityToDelete = await _entity.FindAsync(id);

            if (entityToDelete is not null)
            {
                _entity.Remove(entityToDelete);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string[]? includeProperties = null)
        {
            IQueryable<TEntity> query = _entity.AsNoTracking();

            if (includeProperties is not null)
            {
                foreach (string? includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty); ;
                }
            }
            if (filter is not null)
            {
                query = query.Where(filter);
            }
            if (orderBy is not null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _entity.FindAsync(id);
        }

        public virtual async Task<bool> InsertAsync(TEntity entity)
        {
            await _entity.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> UpdateAsync(TEntity entityToUpdate)
        {
            var entityInDB = await _entity.FindAsync(entityToUpdate.Id);
            if (entityInDB is null)
            {
                return false;
            }
            _dataContext.Entry(entityInDB).CurrentValues.SetValues(entityToUpdate);
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}
