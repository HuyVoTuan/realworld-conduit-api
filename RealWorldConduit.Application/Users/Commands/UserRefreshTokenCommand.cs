using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealWorldConduit.Domain.Entities;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Users.Commands
{
    public record UserRefreshTokenCommand(string refreshToken) : IRequestWithBaseResponse<AuthDTO>;

    internal class UserRefreshTokenCommandHandler : IRequestWithBaseResponseHandler<UserRefreshTokenCommand, AuthDTO>
    {
        private readonly MainDbContext _dbContext;
        private readonly IAuthService _authService;
        private readonly ICurrentUser _currentUser;

        public UserRefreshTokenCommandHandler(MainDbContext dbContext, IAuthService authService, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _authService = authService;
            _currentUser = currentUser;
        }
        public async Task<BaseResponse<AuthDTO>> Handle(UserRefreshTokenCommand request, CancellationToken cancellationToken)
        {

            // TODO : Implement caching refresh token later

            var existedRefreshToken = await _dbContext.RefreshTokens
                                                      .Include(x => x.User)
                                                      .FirstOrDefaultAsync(x => x.UserId == _currentUser.Id && x.AccessToken.Equals(request.refreshToken));

            if (existedRefreshToken == null)
            {
                throw new RestException(HttpStatusCode.Unauthorized, "Invalid refresh token");
            }

            if (existedRefreshToken.ExpiredDate < DateTime.UtcNow)
            {
                _dbContext.Remove<RefreshToken>(existedRefreshToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                throw new RestException(HttpStatusCode.Unauthorized, "Invalid refresh token");
            }

            _dbContext.Remove<RefreshToken>(existedRefreshToken);

            var newRefreshToken = _authService.GenerateRefreshToken(existedRefreshToken.User);

            _dbContext.RefreshTokens.Add(newRefreshToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponse<AuthDTO>
            {
                Code = HttpStatusCode.OK,
                Message = "Successfully refresh token",
                Data = new AuthDTO
                {
                    AccessToken = _authService.GenerateToken(existedRefreshToken.User),
                    RefreshToken = newRefreshToken.AccessToken
                }
            };
        }
    }
}
