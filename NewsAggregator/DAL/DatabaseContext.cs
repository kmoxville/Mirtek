using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using RssFeedAggregator.DAL.Entities;
using RssFeedAggregator.Utils.Options;

namespace RssFeedAggregator.DAL
{
    public sealed class DatabaseContext : DbContext
    {
        public DbSet<PostEntity> News { get; set; } = null!;

        public DbSet<FeedSourceEntity> NewsSources { get; set; } = null!;

        private readonly GeneralOptions _generalOptions;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IOptions<GeneralOptions> generalOptions) 
            : base(options)
        {
            _generalOptions = generalOptions.Value;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new NewsEntityConfiguration(_generalOptions));
            modelBuilder.ApplyConfiguration(new NewsSourceEntityConfiguration(_generalOptions));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
            .UseSnakeCaseNamingConvention();
    }

    public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.DeletedAt)
                .HasColumnType("datetime");
        }
    }

    public sealed class NewsEntityConfiguration : BaseEntityConfiguration<PostEntity>
    {
        private readonly GeneralOptions _generalOptions;

        public NewsEntityConfiguration(GeneralOptions options)
        {
            _generalOptions = options;
        }

        public override void Configure(EntityTypeBuilder<PostEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.PublishedAt)
                .HasColumnType("datetime");

            builder.Property(e => e.Link)
                .HasMaxLength(_generalOptions.MaxDescriptionLength);

            builder.Property(e => e.Description)
                .HasMaxLength(_generalOptions.MaxDescriptionLength);

            builder.Property(e => e.Title)
                .HasMaxLength(_generalOptions.MaxDescriptionLength);

            builder.Property(e => e.Guid)
                .HasMaxLength(100);
        }
    }

    public sealed class NewsSourceEntityConfiguration : BaseEntityConfiguration<FeedSourceEntity>
    {
        private readonly GeneralOptions _generalOptions;

        public NewsSourceEntityConfiguration(GeneralOptions options)
        {
            _generalOptions = options;
        }

        public override void Configure(EntityTypeBuilder<FeedSourceEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Url)
                .HasMaxLength(_generalOptions.MaxUrlLength);

            builder.Property(e => e.OriginalUrl)
                .HasMaxLength(_generalOptions.MaxUrlLength);

            builder.Property(e => e.Description)
                .HasMaxLength(_generalOptions.MaxDescriptionLength);

            builder.Property(e => e.Title)
                .HasMaxLength(_generalOptions.MaxDescriptionLength);
        }
    }
}
