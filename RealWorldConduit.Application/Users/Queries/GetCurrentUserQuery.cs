using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealWorldConduit.Application.Users.DTOs;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Users.Queries
{
    public class GetCurrentUserQuery : IRequestWithBaseResponse<UserDTO>
    {

    }
    internal class GetCurrentUserQueryHandler : IRequestWithBaseResponseHandler<GetCurrentUserQuery, UserDTO>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public GetCurrentUserQueryHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }
        public async Task<BaseResponse<UserDTO>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _dbContext.Users.Where(u => u.Id == _currentUser.Id)
                                              .FirstOrDefaultAsync();

            if (currentUser == null)
            {
                throw new RestException(HttpStatusCode.NotFound, "User Not Found");
            }

            return new BaseResponse<UserDTO>
            {
                Code = HttpStatusCode.OK,
                Message = $"Successfully get current {currentUser.Email} user",
                Data = new UserDTO
                {
                    Username = currentUser.Username,
                    Email = currentUser.Email,
                    ProfileImage = currentUser.ProfileImage,
                    Bio = currentUser.Bio
                }
            };
        }
    }
}
