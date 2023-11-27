using RealWorldConduit.Domain.Common;

namespace RealWorldConduit.Domain.Entities
{
    public class Tag : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public ICollection<BlogTag> BlogTags { get; set; }
    }
}
