using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Users.Commands
{
    public class UserLoginCommand : IRequestWithBaseResponse<AuthDTO>
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }

    public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
    {
        private readonly MainDbContext _dbContext;
        public UserLoginCommandValidator(MainDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Email).NotEmpty()
                                 .EmailAddress()
                                 .OverridePropertyName("email")
                                 .WithMessage("Invalid email");

            RuleFor(x => x.Password).NotEmpty()
                                    .MinimumLength(6)
                                    .OverridePropertyName("password")
                                    .WithMessage("Invalid password or password must be more than 6 characters");
        }
    }

    internal class UserLoginCommandHandler : IRequestWithBaseResponseHandler<UserLoginCommand, AuthDTO>
    {
        private readonly MainDbContext _dbContext;
        private readonly IAuthService _authService;

        public UserLoginCommandHandler(MainDbContext dbContext, IAuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }
        public async Task<BaseResponse<AuthDTO>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var existedUser = await _dbContext.Users
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Email.Equals(request.Email), cancellationToken);

            if (existedUser == null || !_authService.VerifyPassword(request.Password, existedUser.Password))
            {
                throw new RestException(HttpStatusCode.NotFound, "User not found!");
            }


            var newRefreshToken = _authService.GenerateRefreshToken(existedUser);
            _dbContext.RefreshTokens.Add(newRefreshToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponse<AuthDTO>
            {
                Code = HttpStatusCode.OK,
                Message = "Successfully logged in",
                Data = new AuthDTO
                {
                    AccessToken = _authService.GenerateToken(existedUser),
                    RefreshToken = newRefreshToken.AccessToken
                }
            };
        }
    }
}
