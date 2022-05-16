using RssFeedAggregator.DAL.Entities;

namespace RssFeedAggregator.DAL.Repository
{
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task DeleteAsync(int id);

        Task<TEntity?> GetByIdAsync(int id);

        IQueryable<TEntity> GetAll();

        Task InsertAsync(params TEntity[] entities);

        void Update(params TEntity[] entities);

        Task SaveChangesAsync();
    }
}
