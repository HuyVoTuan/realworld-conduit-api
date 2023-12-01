using MediatR;
using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealworldConduit.Infrastructure.Services;
using RealWorldConduit.Domain.Entities;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Users.Commands
{
    public record UserRefreshTokenCommand(string RefreshToken) : IRequestWithBaseResponse<AuthDTO>;

    internal class UserRefreshTokenCommandHandler : IRequestWithBaseResponseHandler<UserRefreshTokenCommand, AuthDTO>
    {
        private readonly MainDbContext _dbContext;
        private readonly IAuthService _authService;
        private readonly ICurrentUser _currentUser;
        private readonly ICacheService _cacheService;

        public UserRefreshTokenCommandHandler(
            MainDbContext dbContext,
            IAuthService authService,
            ICurrentUser currentUser,
            ICacheService cacheService
        )
        {
            _dbContext = dbContext;
            _authService = authService;
            _currentUser = currentUser;
            _cacheService = cacheService;
        }
        public async Task<BaseResponse<AuthDTO>> Handle(UserRefreshTokenCommand request, CancellationToken cancellationToken)
        {

            var oldRefreshToken = await _dbContext.RefreshTokens
                                      .Include(x => x.User)
                                      .FirstOrDefaultAsync(x => x.AccessToken == request.RefreshToken && x.UserId == _currentUser.Id);

            // Check if refresh token is not in database.
            if (oldRefreshToken == null)
            {
                // Check if previous refresh token is still in the cache.
                return CachedRefreshTokenHandler(request.RefreshToken);
            }

            _dbContext.RefreshTokens.Remove(oldRefreshToken);
            var newAccessToken = _authService.GenerateToken(oldRefreshToken.User);
            var newRefreshToken = _authService.GenerateRefreshToken(oldRefreshToken.User);
            _dbContext.RefreshTokens.Add(newRefreshToken);

            var authDTO = new AuthDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.AccessToken
            };

            // Set the old refresh token with new authDTO response in the cache
            _cacheService.SetItem<AuthDTO>($"refreshTokenResponse-{oldRefreshToken.AccessToken}", authDTO, TimeSpan.FromSeconds(20));

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponse<AuthDTO>(authDTO);
        }

        private BaseResponse<AuthDTO> CachedRefreshTokenHandler(string refreshToken)
        {
            var cachedRefreshToken = _cacheService.GetItem<AuthDTO>($"refreshTokenResponse-{refreshToken}");

            if (cachedRefreshToken == null)
            {
                throw new RestException(HttpStatusCode.Unauthorized, "Invalid refresh token");
            }

            return new BaseResponse<AuthDTO>(cachedRefreshToken);
        }
    }
}
