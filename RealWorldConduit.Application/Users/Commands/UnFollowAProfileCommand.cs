using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Users.Commands
{
    // TODO : Implement Validation Later On
    public record UnfollowAProfileCommand(string Username) : IRequestWithBaseResponse;
    internal class UnFollowAProfileCommandHandler : IRequestWithBaseResponseHandler<UnfollowAProfileCommand>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public UnFollowAProfileCommandHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }
        public async Task<BaseResponseDTO> Handle(UnfollowAProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await _dbContext.Followers.FirstOrDefaultAsync(
                x => x.FollowedUser.Username == request.Username && x.FollowerId == _currentUser.Id,
                cancellationToken
            );

            if (profile is null)
            {
                throw new RestException(HttpStatusCode.NotFound, "User not found!");
            }

            _dbContext.Followers.Remove(profile);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponseDTO
            {
                Code = HttpStatusCode.NoContent,
                Message = $"Successfully unfollow {request.Username} user"
            };
        }
    }
}
