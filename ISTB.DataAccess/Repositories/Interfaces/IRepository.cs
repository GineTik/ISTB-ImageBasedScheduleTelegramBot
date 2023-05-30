namespace ISTB.DataAccess.Repositories.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync(int id);
        Task RemoveById(int id);
    }
}
