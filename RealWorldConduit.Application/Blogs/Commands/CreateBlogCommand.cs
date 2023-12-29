using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealworldConduit.Infrastructure.Helpers;
using RealWorldConduit.Application.Blogs.DTOs;
using RealWorldConduit.Domain.Entities;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Blogs.Commands
{
    public class CreateBlogCommand : IRequestWithBaseResponse<MinimalBlogDTO>
    {
        // TODO : Implement Validation Later
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public List<string> TagList { get; set; }
    };

    internal class CreateBlogCommandHandler : IRequestWithBaseResponseHandler<CreateBlogCommand, MinimalBlogDTO>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public CreateBlogCommandHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }
        public async Task<BaseResponseDTO<MinimalBlogDTO>> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
        {
            var isBlogExisted = await _dbContext.Blogs
                                     .AnyAsync(x => x.Title.Equals(request.Title), cancellationToken);

            if (isBlogExisted)
            {
                throw new RestException(HttpStatusCode.NotFound, $"{request.Title} title has been taken!");
            }

            var newBlog = new Blog
            {
                Slug = StringHelper.GenerateSlug(request.Title),
                Title = request.Title,
                Description = request.Description,
                Content = request.Content,
                AuthorId = _currentUser.Id.Value,
            };

            _dbContext.Blogs.Add(newBlog);

            // Add range to BlogTags based on filtered tags
            var requestFilteredTags = await RequestTagsFilter(request, cancellationToken);
            _dbContext.BlogsTag.AddRange(requestFilteredTags.Select(tag => new BlogTag { Blog = newBlog, Tag = tag }));

            await _dbContext.SaveChangesAsync(cancellationToken);

            var blogDTO = new MinimalBlogDTO
            {
                Slug = newBlog.Slug,
                Title = newBlog.Title,
                Description = newBlog.Description,
                Content = newBlog.Content,
                TagList = newBlog.BlogTags.Select(x => x.Tag.Name).ToList(),
                CreatedAt = newBlog.CreatedAt,
                LastUpdatedAt = newBlog.LastUpdatedAt,
                IsFavorited = false,
                FavoritesCount = 0,
            };

            return new BaseResponseDTO<MinimalBlogDTO>
            {
                Code = HttpStatusCode.OK,
                Message = $"Successfully created a new {blogDTO.Slug} blog",
                Data = blogDTO
            };
        }

        private async Task<List<Tag>> RequestTagsFilter(CreateBlogCommand request, CancellationToken cancellationToken)
        {
            var processedRequestTags = request.TagList.Distinct().ToList();

            var existingTags = await _dbContext.Tags
                                    .Where(x => processedRequestTags.Contains(x.Name))
                                    .ToListAsync(cancellationToken);

            var newTags = processedRequestTags
                         .Except(existingTags.Select(t => t.Name))
                         .Select(tag => new Tag { Name = tag })
                         .ToList();

            _dbContext.Tags.AddRange(newTags);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var allTags = existingTags.Concat(newTags).ToList();

            return allTags;
        }
    }
}
