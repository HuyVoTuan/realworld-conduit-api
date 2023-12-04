using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;


namespace RealWorldConduit.Application.Users.Commands
{
    public class UserRevokeTokenCommand : IRequestWithBaseResponse { };
    internal class UserRevokeTokenCommandHandler : IRequestWithBaseResponseHandler<UserRevokeTokenCommand>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public UserRevokeTokenCommandHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task<BaseResponseDTO> Handle(UserRevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshTokenLists = await _dbContext.RefreshTokens.Where(x => x.UserId == _currentUser.Id).ToListAsync(cancellationToken);

            _dbContext.RefreshTokens.RemoveRange(refreshTokenLists);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new BaseResponseDTO
            {
                Code = HttpStatusCode.NoContent,
                Message = "Successfully revoke all user tokens"
            };
        }
    }
}
