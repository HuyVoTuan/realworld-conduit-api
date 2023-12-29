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
        public UserLoginCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty()
                                 .EmailAddress()
                                 .OverridePropertyName("email")
                                 .WithMessage("Invalid email!");

            RuleFor(x => x.Password).NotEmpty()
                                    .MinimumLength(6)
                                    .OverridePropertyName("password")
                                    .WithMessage("Password must be more than 6 characters!");
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
        public async Task<BaseResponseDTO<AuthDTO>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var existedUser = await _dbContext.Users
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Email.Equals(request.Email), cancellationToken);

            if (existedUser is null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"User {existedUser.Slug} not found!");
            }

            if (!_authService.VerifyPassword(request.Password, existedUser.Password))
            {
                throw new RestException(HttpStatusCode.NotFound, "Invalid password!");
            }

            var newRefreshToken = _authService.GenerateRefreshToken(existedUser);
            _dbContext.RefreshTokens.Add(newRefreshToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponseDTO<AuthDTO>
            {
                Code = HttpStatusCode.OK,
                Message = "Successfully log in",
                Data = new AuthDTO
                {
                    AccessToken = _authService.GenerateToken(existedUser),
                    RefreshToken = newRefreshToken.Token
                }
            };
        }
    }
}
