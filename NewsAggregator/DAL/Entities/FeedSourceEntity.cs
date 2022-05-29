using Microsoft.EntityFrameworkCore;

namespace RssFeedAggregator.DAL.Entities
{
    /// <summary>
    /// Source of news - url feed
    /// Rich model - it checks post duplicates when new one added
    /// </summary>
    [Index(nameof(Url))]
    public sealed class FeedSourceEntity : BaseEntity
    {
        private ICollection<PostEntity> _posts = new List<PostEntity>();

        /// <summary>
        /// Feed title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Human readable description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Url for obtaining news
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Url originally provided by user
        /// </summary>
        public string OriginalUrl { get; set; } = string.Empty;

        /// <summary>
        /// Related news
        /// </summary>
        [BackingField(nameof(_posts))]
        public IEnumerable<PostEntity> Posts => _posts;

        public void AddPosts(IEnumerable<PostEntity> newPosts)
        {
            foreach (var post in newPosts)
            {
                if (!AlreadyExists(post))
                {
                    post.FeedSource = this;
                    _posts.Add(post);
                }
            }
        }

        private bool AlreadyExists(PostEntity newPost) => Posts.Any(post => post == newPost);
    }
}
