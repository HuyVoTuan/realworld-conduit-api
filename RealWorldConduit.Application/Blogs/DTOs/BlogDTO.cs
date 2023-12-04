using RealWorldConduit.Application.Users.DTOs;

namespace RealWorldConduit.Application.Articles.DTOs
{
    internal class BlogDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public List<string> TagList { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public bool Favorited { get; set; }
        public int FavoritesCount { get; set; }
        public ProfileDTO Profile { get; set; }
    }
}
