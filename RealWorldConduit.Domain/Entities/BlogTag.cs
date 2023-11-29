using RealWorldConduit.Domain.Common;

namespace RealWorldConduit.Domain.Entities
{
    public class BlogTag : IAuditEntity
    {     
        public virtual Blog Blog { get; set; }
        public Guid BlogId { get; set; }
        public virtual Tag Tag { get; set; }
        public Guid TagId { get; set; }
        public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime LastUpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
