
namespace RealWorldConduit.Domain.Entities
{
    public class Blog : BaseEntity<Guid>
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public User Author { get; set; }
        public Guid AuthorId { get; set; }
        public ICollection<FavoriteBlog> FavoriteBlogs { get; set; }
        public ICollection<BlogTag> BlogTags { get; set; }
    }
}
