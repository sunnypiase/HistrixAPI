using HistrixAPI.DB;
using HistrixAPI.Models.Entities;
using HistrixAPI.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace HistrixAPI.Repository
{
    public class SqlBulkRepository<TEntity>
    : IBulkRepository<TEntity>
    where TEntity : class, IEntity
    {
        protected readonly DataContext _dataContext;
        protected readonly DbSet<TEntity> _entity;

        public SqlBulkRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            _entity = _dataContext.Set<TEntity>();
        }

        public virtual async Task<bool> InsertBatchAsync(IEnumerable<TEntity> entities)
        {
            await _entity.AddRangeAsync(entities);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> UpdateBatchAsync(IEnumerable<TEntity> entitiesToUpdate)
        {
            _entity.UpdateRange(entitiesToUpdate);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> DeleteBatchAsync(IEnumerable<int> ids)
        {
            var entitiesToDelete = _entity.Where(e => ids.Contains(e.Id));
            _entity.RemoveRange(entitiesToDelete);
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}
