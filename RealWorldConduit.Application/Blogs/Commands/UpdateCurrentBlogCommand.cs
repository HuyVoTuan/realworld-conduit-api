using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealworldConduit.Infrastructure.Helpers;
using RealWorldConduit.Application.Blogs.DTOs;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Blogs.Commands
{
    public class UpdateCurrentBlogCommand : IRequestWithBaseResponse<MinimalBlogDTO>
    {
        // TODO : Implement Validation Later
        public string Slug { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string Content { get; init; }
    }

    internal class UpdateBlogCommandHandler : IRequestWithBaseResponseHandler<UpdateCurrentBlogCommand, MinimalBlogDTO>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public UpdateBlogCommandHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task<BaseResponseDTO<MinimalBlogDTO>> Handle(UpdateCurrentBlogCommand request, CancellationToken cancellationToken)
        {
            var oldBlog = await _dbContext.Blogs
                                .FirstOrDefaultAsync(x => x.Slug.Equals(request.Slug) && x.AuthorId == _currentUser.Id, cancellationToken);

            if (oldBlog is null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"{request.Slug} blog is not found!");
            }

            oldBlog.Slug = StringHelper.GenerateSlug(request.Title);
            oldBlog.Title = request.Title;
            oldBlog.Description = request.Description;
            oldBlog.Content = request.Content;

            // TODO: Implement update tag list later

            _dbContext.Blogs.Update(oldBlog);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var blogDTO = new MinimalBlogDTO
            {
                Slug = oldBlog.Slug,
                Title = oldBlog.Title,
                Description = oldBlog.Description,
                Content = oldBlog.Content,
                TagList = oldBlog.BlogTags.Select(x => x.Tag.Name).ToList(),
                IsFavorited = oldBlog.FavoriteBlogs.Any(x => x.FavoritedById == _currentUser.Id),
                FavoritesCount = oldBlog.FavoriteBlogs.Count(),
                CreatedAt = oldBlog.CreatedAt,
                LastUpdatedAt = oldBlog.LastUpdatedAt,
            };

            return new BaseResponseDTO<MinimalBlogDTO>
            {
                Code = HttpStatusCode.OK,
                Message = $"{blogDTO.Title} blog has been successfully updated",
                Data = blogDTO
            };
        }
    }
}
