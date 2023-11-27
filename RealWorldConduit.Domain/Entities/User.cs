using RealWorldConduit.Domain.Common;

namespace RealWorldConduit.Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string ProfileImage { get; set; }
        public string Password { get; set; }
        public string Bio { get; set; }
        public ICollection<Blog> Blogs { get; set; }
        public ICollection<FavoriteBlog> FavoriteBlogs { get; set; }
        public ICollection<UserFollower> FollowedUsers { get; set; }
        public ICollection<UserFollower> Followers { get; set; }
        public ICollection<RefreshToken> RefreshToken { get; set; }
    }
}
