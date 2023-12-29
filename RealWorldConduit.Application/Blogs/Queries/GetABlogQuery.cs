﻿using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealWorldConduit.Application.Articles.DTOs;
using RealWorldConduit.Application.Blogs.DTOs;
using RealWorldConduit.Application.Users.DTOs;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Blogs.Queries
{
    // TODO : Implement Validation Later On
    public record GetABlogQuery(string Slug) : IRequestWithBaseResponse<BlogDTO>;

    internal class GetABlogQueryHandler : IRequestWithBaseResponseHandler<GetABlogQuery, BlogDTO>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public GetABlogQueryHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }
        public async Task<BaseResponseDTO<BlogDTO>> Handle(GetABlogQuery request, CancellationToken cancellationToken)
        {
            var blogDTO = await _dbContext.Blogs
                                .AsNoTracking()
                                .Select(x => new BlogDTO
                                {
                                    MinimalBlogDTO = new MinimalBlogDTO
                                    {
                                        Slug = x.Slug,
                                        Title = x.Title,
                                        Description = x.Description,
                                        Content = x.Content,
                                        TagList = x.BlogTags.Select(x => x.Tag.Name).ToList(),
                                        IsFavorited = x.FavoriteBlogs.Any(x => x.FavoritedById == _currentUser.Id),
                                        FavoritesCount = x.FavoriteBlogs.Count(),
                                        CreatedAt = x.CreatedAt,
                                        LastUpdatedAt = x.LastUpdatedAt,
                                    },
                                    ProfileDTO = new ProfileDTO
                                    {
                                        Slug = x.Author.Slug,
                                        Username = x.Author.Username,
                                        Email = x.Author.Email,
                                        Bio = x.Author.Bio,
                                        IsFollowing = x.Author.FollowedUsers.Any(x => x.FollowerId == _currentUser.Id),
                                        ProfileImage = x.Author.ProfileImage
                                    },
                                })
                                .FirstOrDefaultAsync(x => x.MinimalBlogDTO.Slug.Equals(request.Slug), cancellationToken);

            if (blogDTO is null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"{request.Slug} blog is not found!");
            }

            return new BaseResponseDTO<BlogDTO>
            {
                Code = HttpStatusCode.OK,
                Message = $"Successfully get {blogDTO.MinimalBlogDTO.Slug} blog",
                Data = blogDTO
            };

        }
    }
}
