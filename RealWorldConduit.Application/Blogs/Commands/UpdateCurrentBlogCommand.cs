using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealWorldConduit.Application.Articles.DTOs;
using RealWorldConduit.Application.Users.DTOs;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Blogs.Commands
{
    public class UpdateCurrentBlogCommand : IRequestWithBaseResponse<BlogDTO>
    {
        // TODO : Implement Validation Later
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }

    internal class UpdateBlogCommandHandler : IRequestWithBaseResponseHandler<UpdateCurrentBlogCommand, BlogDTO>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public UpdateBlogCommandHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task<BaseResponseDTO<BlogDTO>> Handle(UpdateCurrentBlogCommand request, CancellationToken cancellationToken)
        {
            var oldBlog = await _dbContext.Blogs.FirstOrDefaultAsync(x => x.Title.Equals(request.Title) && x.AuthorId == _currentUser.Id, cancellationToken);
           
            if (oldBlog is null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"A blog with {request.Title} title is not found!");
            }

            oldBlog.Title = request.Title;
            oldBlog.Description = request.Description;
            oldBlog.Content = request.Content;

            // Implement update tag list later
            //oldBlog.BlogTags.Select(x => x.Tag.Name).ToList(),

            _dbContext.Blogs.Update(oldBlog);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var blogDTO = new BlogDTO
            {
                Title = oldBlog.Title,
                Description = oldBlog.Description,
                Content = oldBlog.Content,
                TagList = oldBlog.BlogTags.Select(x => x.Tag.Name).ToList(),
                CreatedAt = oldBlog.CreatedAt,
                LastUpdatedAt = oldBlog.LastUpdatedAt,
                Profile = new ProfileDTO
                {
                    Username = oldBlog.Author.Username,
                    Email = oldBlog.Author.Email,
                    Bio = oldBlog.Author.Bio,
                    Following = oldBlog.Author.FollowedUsers.Any(x => x.FollowerId == _currentUser.Id),
                    ProfileImage = oldBlog.Author.ProfileImage
                },
                Favorited = oldBlog.FavoriteBlogs.Any(x => x.FavoritedById == _currentUser.Id),
                FavoritesCount = oldBlog.FavoriteBlogs.Count()
            };

            return new BaseResponseDTO<BlogDTO>
            {
                Code = HttpStatusCode.OK,
                Message = $"Successfully update {blogDTO.Title} blog",
                Data = blogDTO
            };
        }
    }
}
