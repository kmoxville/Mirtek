using Microsoft.EntityFrameworkCore;
using RssFeedAggregator.DAL.Entities;
using RssFeedAggregator.DAL.Repository;

namespace RssFeedAggregator.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        Repository<FeedSourceEntity> FeedSources { get; }
        Repository<PostEntity> Posts { get; }

        DatabaseContext Context { get; }

        Task Save();
    }
}