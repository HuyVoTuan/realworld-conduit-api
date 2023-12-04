namespace RealWorldConduit.Domain.Entities
{
    public class BlogTag : BaseEntity
    {
        public virtual Blog Blog { get; set; }
        public Guid BlogId { get; set; }
        public virtual Tag Tag { get; set; }
        public Guid TagId { get; set; }
    }
}
