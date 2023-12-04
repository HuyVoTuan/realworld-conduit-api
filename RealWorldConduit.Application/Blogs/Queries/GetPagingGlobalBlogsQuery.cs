using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealworldConduit.Infrastructure.Linq;
using RealWorldConduit.Application.Articles.DTOs;
using RealWorldConduit.Application.Users.DTOs;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Articles.Queries
{
    public class GetPagingGlobalBlogsQuery : PagingRequestDTO, IRequestWithBaseResponse<PagingResponseDTO<BlogDTO>>
    {

    };
    internal class GetGlobalArticlesQueryHandler : IRequestWithBaseResponseHandler<GetPagingGlobalBlogsQuery, PagingResponseDTO<BlogDTO>>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public GetGlobalArticlesQueryHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }
        public async Task<BaseResponseDTO<PagingResponseDTO<BlogDTO>>> Handle(GetPagingGlobalBlogsQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Blogs.AsNoTracking()
                                        .Select(x => new BlogDTO
                                        {
                                            Title = x.Title,
                                            Description = x.Description,
                                            Content = x.Content,
                                            TagList = x.BlogTags.Select(x => x.Tag.Name).ToList(),
                                            CreatedAt = x.CreatedAt,
                                            LastUpdatedAt = x.LastUpdatedAt,
                                            Profile = new ProfileDTO
                                            {
                                                Username = x.Author.Username,
                                                Email = x.Author.Email,
                                                Bio = x.Author.Bio,
                                                Following = x.Author.FollowedUsers.Any(x => x.FollowerId == _currentUser.Id),
                                                ProfileImage = x.Author.ProfileImage
                                            },
                                            Favorited = x.FavoriteBlogs.Any(x => x.FavoritedById == _currentUser.Id),
                                            FavoritesCount = x.FavoriteBlogs.Count()
                                        })
                                        .OrderBy(x => x.LastUpdatedAt);

            var totalBlogs = await query.CountAsync(cancellationToken);

            var pagedBlogs = await query
                                   .Page(request.PageIndex, request.PageSize)
                                   .ToListAsync(cancellationToken);

            return new BaseResponseDTO<PagingResponseDTO<BlogDTO>>
            {
                Code = HttpStatusCode.OK,
                Message = "Successfully get global blogs",
                Data = new PagingResponseDTO<BlogDTO>
                {
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalCount = totalBlogs,
                    Data = pagedBlogs
                }
            };
        }
    }
}
