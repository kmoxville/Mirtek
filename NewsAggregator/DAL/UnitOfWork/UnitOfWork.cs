using Microsoft.EntityFrameworkCore;
using RssFeedAggregator.DAL.Entities;
using RssFeedAggregator.DAL.Repository;
using RssFeedAggregator.DAL;

namespace RssFeedAggregator.DAL.UnitOfWork
{
    public sealed class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private bool _disposed = false;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
            FeedSources = new Repository<FeedSourceEntity>(_context);
            Posts = new Repository<PostEntity>(_context);
        }

        public Repository<FeedSourceEntity> FeedSources { get; private set; }

        public Repository<PostEntity> Posts { get; private set; }

        public DatabaseContext Context => _context;

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }
    }
}
