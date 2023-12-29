using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealWorldConduit.Application.Users.DTOs;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Users.Queries
{
    // TODO : Implent Validation Later On
    public record GetAProfileQuery(string Slug) : IRequestWithBaseResponse<ProfileDTO>;
    internal class GetAProfileQueryHandler : IRequestWithBaseResponseHandler<GetAProfileQuery, ProfileDTO>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public GetAProfileQueryHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }
        public async Task<BaseResponseDTO<ProfileDTO>> Handle(GetAProfileQuery request, CancellationToken cancellationToken)
        {
            var targetUserDTO = await _dbContext.Users
                                .AsNoTracking()
                                .Select(x => new ProfileDTO
                                {
                                    Slug = x.Slug,
                                    Username = x.Username,
                                    Bio = x.Bio,
                                    Email = x.Email,
                                    ProfileImage = x.ProfileImage,
                                    IsFollowing = x.FollowedUsers.Any(fl => fl.FollowerId == _currentUser.Id)
                                })
                                .FirstOrDefaultAsync(x => x.Slug.Equals(request.Slug), cancellationToken);

            if (targetUserDTO is null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"User {request.Slug} not found!");
            }

            return new BaseResponseDTO<ProfileDTO>
            {
                Code = HttpStatusCode.OK,
                Message = $"Successfully get user {targetUserDTO.Slug}",
                Data = targetUserDTO
            };
        }
    }
}
