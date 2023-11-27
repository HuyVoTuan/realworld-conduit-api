using RealWorldConduit.Domain.Common;

namespace RealWorldConduit.Domain.Entities
{
    public class FavoriteBlog : IAuditEntity
    {
        public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime LastUpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public virtual Blog Blog { get; set; }
        public Guid BlogId { get; set; }
        public virtual User FavoritedBy { get; set; }
        public Guid FavoritedById { get; set; }


    }
}
