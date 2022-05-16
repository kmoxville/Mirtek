using Microsoft.EntityFrameworkCore;
using RssFeedAggregator.DAL.Entities;

namespace RssFeedAggregator.DAL.Repository
{
    public sealed class Repository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll() => _dbSet.AsNoTracking().Where(e => !e.IsDeleted);

        public Task<TEntity?> GetByIdAsync(int id)
        {
            return GetAll().Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task InsertAsync(params TEntity[] entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(params TEntity[] entities)
        {
            _context.UpdateRange(entities);
        }

        public async Task DeleteAsync(int id)
        {
            TEntity? entityToDelete = await _dbSet.FindAsync(id);
            entityToDelete?.Delete();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
