using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealworldConduit.Infrastructure.Helpers;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Users.Commands
{
    public class UpdateCurrentUserCommand : IRequestWithBaseResponse
    {
        // TODO : Implement Validation
        public string Email { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public string Bio { get; init; }
        public string ProfileImage { get; init; }
    };
    internal class UpdateCurrentUserCommandHandler : IRequestWithBaseResponseHandler<UpdateCurrentUserCommand>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;
        private readonly IAuthService _authService;

        public UpdateCurrentUserCommandHandler(MainDbContext dbContext, ICurrentUser currentUser, IAuthService authService)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _authService = authService;
        }
        public async Task<BaseResponseDTO> Handle(UpdateCurrentUserCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == _currentUser.Id, cancellationToken);

            currentUser.Slug = StringHelper.GenerateSlug(request.Username);
            currentUser.Email = request.Email;
            currentUser.Username = request.Username;
            currentUser.Password = _authService.HashPassword(request.Password);
            currentUser.Bio = request.Bio;
            currentUser.ProfileImage = request.ProfileImage;

            _dbContext.Update(currentUser);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponseDTO
            {
                Code = HttpStatusCode.NoContent,
                Message = "Successfully update current user"
            };
        }
    }
}

