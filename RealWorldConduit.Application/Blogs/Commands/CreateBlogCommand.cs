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
            var filteredTags = await TagsFilter(request, cancellationToken);
            var isBlogExisted = await IsBlogExisted(request, cancellationToken);

            if (!isBlogExisted)
            {
                throw new RestException(HttpStatusCode.Found, $"A blog with {request.Title} title is already existed!");
            }

            var newBlog = await ProcessBlogAndTags(filteredTags, request, cancellationToken);

            var blogDTO = new BlogDTO
            {
                Title = newBlog.Title,
                Description = newBlog.Description,
                Content = newBlog.Content,
                TagList = newBlog.BlogTags.Select(x => x.Tag.Name).ToList(),
                CreatedAt = newBlog.CreatedAt,
                LastUpdatedAt = newBlog.LastUpdatedAt,
                Profile = new ProfileDTO
                {
                    Username = newBlog.Author.Username,
                    Email = newBlog.Author.Email,
                    Bio = newBlog.Author.Bio,
                    Following = newBlog.Author.FollowedUsers.Any(x => x.FollowerId == _currentUser.Id),
                    ProfileImage = newBlog.Author.ProfileImage
                },
                Favorited = newBlog.FavoriteBlogs.Any(x => x.FavoritedById == _currentUser.Id),
                FavoritesCount = newBlog.FavoriteBlogs.Count()
            };

            return new BaseResponseDTO<BlogDTO>
            {
                Code = HttpStatusCode.OK,
                Message = $"Successfully create new {blogDTO.Title} blog",
                Data = blogDTO
            };
        }
        private async Task<Blog> ProcessBlogAndTags(List<Tag> filteredTags, CreateBlogCommand request, CancellationToken cancellationToken)
        {
            var newBlog = new Blog(
               StringHelper.GenerateSlug(request.Title), request.Description,
               request.Content, (Guid)_currentUser.Id
           );

            _dbContext.Blogs.Add(newBlog);

            // Add range to BlogTags based on filterd tags
            _dbContext.BlogsTag.AddRange(
               filteredTags.Select(tag => new BlogTag { Blog = newBlog, Tag = tag }
           ));

            await _dbContext.SaveChangesAsync(cancellationToken);

            return newBlog;
        }

        private async Task<bool> IsBlogExisted(CreateBlogCommand request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Blogs.AsNoTracking()
                               .AnyAsync(x => x.Title == request.Title, cancellationToken);

            return result;
        }

        private async Task<List<Tag>> TagsFilter(CreateBlogCommand request, CancellationToken cancellationToken)
        {
            var tags = new List<Tag>();
            var processedRequestTags = request.TagList.Distinct().ToList();

            // Check if processed request tag is null
            foreach (var tag in (processedRequestTags ?? Enumerable.Empty<string>()))
            {
                var t = await _dbContext.Tags.FirstOrDefaultAsync(x => x.Name.Equals(tag), cancellationToken);

                if (t is not null)
                {
                    continue;
                }

                t = new Tag
                {
                    Name = tag,
                };

                _dbContext.Tags.Add(t);

                // Save for later on reusability
                await _dbContext.SaveChangesAsync(cancellationToken);

                // Add to tags list after being filterd
                tags.Add(t);
            }

            return tags;
        }
    }
}
