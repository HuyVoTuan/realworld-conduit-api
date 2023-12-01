namespace RealWorldConduit.Domain.Entities
{
    public class FavoriteBlog : BaseEntity
    {
        public virtual Blog Blog { get; set; }
        public Guid BlogId { get; set; }
        public virtual User FavoritedBy { get; set; }
        public Guid FavoritedById { get; set; }
        public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime LastUpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
