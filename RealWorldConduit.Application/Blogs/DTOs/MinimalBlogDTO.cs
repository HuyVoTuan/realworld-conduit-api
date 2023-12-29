namespace RealWorldConduit.Application.Blogs.DTOs
{
    internal class MinimalBlogDTO
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public List<string> TagList { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public bool IsFavorited { get; set; }
        public int FavoritesCount { get; set; }
    }
}
