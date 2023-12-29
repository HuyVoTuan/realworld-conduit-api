using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealWorldConduit.Domain.Entities;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;


namespace RealWorldConduit.Application.Users.Commands
{
    // TODO : Implement Validation Later On
    public record UpsertFollowAProfileCommand(string Slug) : IRequestWithBaseResponse;
    internal class FollowAProfileCommandHandler : IRequestWithBaseResponseHandler<UpsertFollowAProfileCommand>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public FollowAProfileCommandHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task<BaseResponseDTO> Handle(UpsertFollowAProfileCommand request, CancellationToken cancellationToken)
        {
            int flag = 0;
            var profile = await _dbContext.Users
                                .Select(x => new User
                                {
                                    Id = x.Id,
                                    Slug = x.Slug,
                                    FollowedUsers = x.FollowedUsers,
                                })
                                .FirstOrDefaultAsync(x => x.Slug.Equals(request.Slug), cancellationToken);

            if (profile is null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"User {request.Slug} not found!");
            }

            if (!profile.FollowedUsers.Any(y => y.FollowerId == _currentUser.Id))
            {
                flag = 1;
                var newFollower = new UserFollower
                {
                    FollowedUserId = profile.Id,
                    FollowerId = _currentUser.Id.Value
                };
                _dbContext.Followers.Add(newFollower);
            }
            else
            {
                _dbContext.Followers.Remove(profile.FollowedUsers.FirstOrDefault(y => y.FollowerId == _currentUser.Id));
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponseDTO
            {
                Code = HttpStatusCode.OK,
                Message = flag == 1 ? $"Successfully follow {request.Slug} user" : $"Successfully unfollow {request.Slug} user"
            };
        }
    }
}
