using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealWorldConduit.Application.Users.DTOs;
using RealWorldConduit.Domain.Entities;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Users.Queries
{
    public record GetCurrentUserQuery() : IRequestWithBaseResponse<ProfileDTO>;
    internal class GetCurrentUserQueryHandler : IRequestWithBaseResponseHandler<GetCurrentUserQuery, ProfileDTO>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public GetCurrentUserQueryHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }
        public async Task<BaseResponseDTO<ProfileDTO>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _dbContext.Users
                                    .AsNoTracking()
                                    .Select(x => new User
                                    {
                                        Id = x.Id,
                                        Slug = x.Slug,
                                        Username = x.Username,
                                        Bio = x.Bio,
                                        Email = x.Email,
                                        ProfileImage = x.ProfileImage,
                                        FollowedUsers = x.FollowedUsers
                                    })
                                    .FirstOrDefaultAsync(x => x.Id == _currentUser.Id, cancellationToken);

            if (currentUser is null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"User not found!");
            }

            return new BaseResponseDTO<ProfileDTO>
            {
                Code = HttpStatusCode.OK,
                Message = $"Successfully get current user",
                Data = new ProfileDTO
                {
                    Slug = currentUser.Slug,
                    Username = currentUser.Username,
                    Bio = currentUser.Bio,
                    Email = currentUser.Email,
                    ProfileImage = currentUser.ProfileImage,
                    IsFollowing = currentUser.FollowedUsers.Any(fl => fl.FollowerId == _currentUser.Id)
                }
            };
        }
    }
}
