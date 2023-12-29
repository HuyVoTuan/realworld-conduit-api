using RealWorldConduit.Application.Blogs.DTOs;
using RealWorldConduit.Application.Users.DTOs;

namespace RealWorldConduit.Application.Articles.DTOs
{
    internal class BlogDTO
    {
        public MinimalBlogDTO MinimalBlogDTO { get; set; }
        public ProfileDTO ProfileDTO { get; set; }
    }
}
