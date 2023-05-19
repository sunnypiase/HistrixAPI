namespace HistrixAPI.Repository.Abstract
{
    public interface IBulkRepository<TEntity>
    where TEntity : class
    {
        Task<bool> InsertBatchAsync(IEnumerable<TEntity> entities);
        Task<bool> UpdateBatchAsync(IEnumerable<TEntity> entitiesToUpdate);
        Task<bool> DeleteBatchAsync(IEnumerable<int> ids);
    }
}
