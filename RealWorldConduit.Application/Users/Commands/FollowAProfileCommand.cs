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
    public record FollowAProfileCommand(string Username) : IRequestWithBaseResponse;
    internal class FollowAProfileCommandHandler : IRequestWithBaseResponseHandler<FollowAProfileCommand>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public FollowAProfileCommandHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task<BaseResponseDTO> Handle(FollowAProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

            if (profile is null)
            {
                throw new RestException(HttpStatusCode.NotFound, "User not found!");
            }

            var newFollower = new UserFollower
            {
                FollowedUserId = profile.Id,
                FollowerId = (Guid)_currentUser.Id
            };

            _dbContext.Followers.Add(newFollower);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponseDTO
            {
                Code = HttpStatusCode.OK,
                Message = $"Successfully follow {request.Username} user"
            };
        }
    }
}
