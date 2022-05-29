using Microsoft.EntityFrameworkCore;

namespace RssFeedAggregator.DAL.Entities
{
    /// <summary>
    /// Item from rss feed, guid calculated as MD5 of summary
    /// </summary>
    [Index(nameof(Guid))]
    public sealed class PostEntity : BaseEntity, IEquatable<PostEntity>
    {
        private string _guid = string.Empty;
        private string _description = string.Empty;

        public string Title { get; set; } = string.Empty;

        [BackingField(nameof(_description))]
        public string Description 
        {
            get => _description; 
            set
            {
                _description = value;
                _guid = GetGuid();
            }
        }

        public string Link { get; set; } = string.Empty;

        public DateTime PublishedAt { get; set; }

        [BackingField(nameof(_guid))]
        public string Guid => _guid;

        public int FeedSourceEntityId { get; private set; }

        public FeedSourceEntity FeedSource { get; set; } = null!;

        private string GetGuid() => MD5Hash.Hash.Content(Description);

        public bool Equals(PostEntity? other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Guid == other.Guid;
        }

        public static bool operator ==(PostEntity left, PostEntity right) => Equals(left, right);

        public static bool operator !=(PostEntity left, PostEntity right) => !Equals(left, right);

        public override bool Equals(object? obj) => Equals(obj as PostEntity);

        public override int GetHashCode() => GetGuid().GetHashCode();
    }
}
