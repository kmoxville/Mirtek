namespace RssFeedAggregator.DAL.Entities
{
    public class BaseEntity
    {
        public int Id { get; private set; }

        public bool IsDeleted { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime DeletedAt { get; private set; }

        public void Delete()
        {
            if (IsDeleted)
                return;

            IsDeleted = true;
            DeletedAt = DateTime.Now;
        }
    }
}
