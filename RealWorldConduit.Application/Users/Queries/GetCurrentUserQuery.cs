using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealWorldConduit.Application.Users.DTOs;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Users.Queries
{
    public class GetCurrentUserQuery : IRequestWithBaseResponse<MinimalProfileDTO>
    {

    }
    internal class GetCurrentUserQueryHandler : IRequestWithBaseResponseHandler<GetCurrentUserQuery, MinimalProfileDTO>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public GetCurrentUserQueryHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }
        public async Task<BaseResponseDTO<MinimalProfileDTO>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _dbContext.Users.Where(u => u.Id == _currentUser.Id)
                                              .FirstOrDefaultAsync();

            if (currentUser is null)
            {
                throw new RestException(HttpStatusCode.NotFound, "User Not Found");
            }

            return new BaseResponseDTO<MinimalProfileDTO>
            {
                Code = HttpStatusCode.OK,
                Message = $"Successfully get current {currentUser.Email} user",
                Data = new MinimalProfileDTO
                {
                    Username = currentUser.Username,
                    Email = currentUser.Email,
                    ProfileImage = currentUser.ProfileImage,
                    Bio = currentUser.Bio,
                }
            };
        }
    }
}
