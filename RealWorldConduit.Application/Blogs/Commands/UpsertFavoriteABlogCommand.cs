using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealWorldConduit.Domain.Entities;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Blogs.Commands
{
    public record UpsertFavoriteABlogCommand(string Slug) : IRequestWithBaseResponse;

    internal class UpsertFavoriteABlogCommandHandler : IRequestWithBaseResponseHandler<UpsertFavoriteABlogCommand>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public UpsertFavoriteABlogCommandHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }
        public async Task<BaseResponseDTO> Handle(UpsertFavoriteABlogCommand request, CancellationToken cancellationToken)
        {
            int flag = 0;
            var targetBlog = await _dbContext.Blogs
                                   .Select(x => new Blog
                                   {
                                       Id = x.Id,
                                       Slug = x.Slug,
                                       Title = x.Title,
                                       Description = x.Description,
                                       Content = x.Content,
                                       BlogTags = x.BlogTags,
                                       FavoriteBlogs = x.FavoriteBlogs,
                                       CreatedAt = x.CreatedAt,
                                       LastUpdatedAt = x.LastUpdatedAt
                                   })
                                   .FirstOrDefaultAsync(x => x.Slug.Equals(request.Slug));

            if (targetBlog is null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"{request.Slug} blog is not found!");
            }

            if (!targetBlog.FavoriteBlogs.Any(y => y.FavoritedById == _currentUser.Id))
            {
                flag = 1;
                var newFavoriteBlogRecord = new FavoriteBlog
                {
                    BlogId = targetBlog.Id,
                    FavoritedById = _currentUser.Id.Value
                };

                _dbContext.FavoriteBlogs.Add(newFavoriteBlogRecord);
            }
            else
            {
                _dbContext.FavoriteBlogs.Remove(targetBlog.FavoriteBlogs.FirstOrDefault(y => y.FavoritedById == _currentUser.Id));
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponseDTO
            {
                Code = HttpStatusCode.OK,
                Message = flag == 1 ? $"Successfully like {targetBlog.Slug} blog" : $"Successfully unlike {targetBlog.Slug} blog"
            };
        }
    }
}
