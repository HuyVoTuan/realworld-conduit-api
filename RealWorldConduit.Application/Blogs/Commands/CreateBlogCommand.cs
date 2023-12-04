using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealworldConduit.Infrastructure.Helpers;
using RealWorldConduit.Application.Articles.DTOs;
using RealWorldConduit.Application.Users.DTOs;
using RealWorldConduit.Domain.Entities;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Blogs.Commands
{
    public class CreateBlogCommand : IRequestWithBaseResponse<BlogDTO>
    {
        // TODO : Implement Validation Later
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public List<string> TagList { get; set; }
    };

    internal class CreateBlogCommandHandler : IRequestWithBaseResponseHandler<CreateBlogCommand, BlogDTO>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public CreateBlogCommandHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }
        public async Task<BaseResponseDTO<BlogDTO>> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
        {
            var requestFilteredTags = await RequestTagsFilter(request, cancellationToken);

            var blog = await _dbContext.Blogs.FirstOrDefaultAsync(x => x.Title.Equals(request.Title), cancellationToken);

            var author = await _dbContext.Users.AsNoTracking()
                                               .Where(u => u.Id == _currentUser.Id)
                                               .Select(u => new ProfileDTO
                                               {
                                                  Username = u.Username,
                                                  Email = u.Email,
                                                  Bio = u.Bio,
                                                  ProfileImage = u.ProfileImage,
                                                  Following = u.FollowedUsers.Any(f => f.FollowerId == _currentUser.Id)
                                               }).FirstOrDefaultAsync(cancellationToken);

            if (blog is not null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"A blog with {request.Title} title is existed!");
            }

            blog = new Blog
            {
                Title = request.Title,
                Description = request.Description,
                Content = request.Content,
                AuthorId = (Guid)_currentUser.Id,
            };

            _dbContext.Blogs.Add(blog);

            // Add range to BlogTags based on filterd tags
            _dbContext.BlogsTag.AddRange(requestFilteredTags.Select(tag => new BlogTag { Blog = blog, Tag = tag }));

            await _dbContext.SaveChangesAsync(cancellationToken);

            var blogDTO = new BlogDTO
            {
                Title = blog.Title,
                Description = blog.Description,
                Content = blog.Content,
                TagList = blog.BlogTags.Select(x => x.Tag.Name).ToList(),
                CreatedAt = blog.CreatedAt,
                LastUpdatedAt = blog.LastUpdatedAt,
                Profile = author,
                Favorited = blog.FavoriteBlogs?.Any(x => x.FavoritedById == _currentUser.Id) ?? false,
                FavoritesCount = blog.FavoriteBlogs?.Count() ?? 0,
            };

            return new BaseResponseDTO<BlogDTO>
            {
                Code = HttpStatusCode.OK,
                Message = $"Successfully create new {blogDTO.Title} blog",
                Data = blogDTO
            };
        }

        private async Task<List<Tag>> RequestTagsFilter(CreateBlogCommand request, CancellationToken cancellationToken)
        {
            var tags = new List<Tag>();
            var processedRequestTags = request.TagList.Distinct().ToList();

            // Check if processed request tag is null
            foreach (var tag in (processedRequestTags ?? Enumerable.Empty<string>()))
            {
                var t = await _dbContext.Tags.FirstOrDefaultAsync(x => x.Name.Equals(tag), cancellationToken);

                if (t is null)
                {
                    t = new Tag
                    {
                        Name = tag,
                    };

                    _dbContext.Tags.Add(t);

                    // Save for later on reusability
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }
                tags.Add(t);
            }
            return tags;
        }
    }
}
