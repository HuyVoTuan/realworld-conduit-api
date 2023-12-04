namespace RealWorldConduit.Application.Blogs.DTOs
{
    internal class MinimalBlogDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public List<string> TagList { get; set; }
    }
}
