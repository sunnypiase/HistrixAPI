using HistrixAPI.Models.Entities;
using System.Linq.Expressions;

namespace HistrixAPI.Repository.Abstract
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        public Task<IEnumerable<TEntity>> GetAsync(
           Expression<Func<TEntity, bool>>? filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
           string[]? includeProperties = null);
        public Task<TEntity?> GetByIdAsync(int id);
        public Task<bool> InsertAsync(TEntity entity);
        public Task<bool> UpdateAsync(TEntity entityToUpdate);
        public Task<bool> DeleteAsync(int id);
    }
}
