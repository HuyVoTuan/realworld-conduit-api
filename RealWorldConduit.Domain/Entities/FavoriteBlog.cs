namespace RealWorldConduit.Domain.Entities
{
    public class FavoriteBlog : BaseEntity
    {
        public Blog Blog { get; set; }
        public Guid BlogId { get; set; }
        public User FavoritedBy { get; set; }
        public Guid FavoritedById { get; set; }
    }
}
