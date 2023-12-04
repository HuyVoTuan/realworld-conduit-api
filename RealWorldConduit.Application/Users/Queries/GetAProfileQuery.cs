using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealWorldConduit.Application.Users.DTOs;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Application.Users.Queries
{
    // TODO : Implent Validation Later On
    public record GetAProfileQuery(string Username) : IRequestWithBaseResponse<ProfileDTO>;
    internal class GetAProfileQueryHandler : IRequestWithBaseResponseHandler<GetAProfileQuery, ProfileDTO>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public GetAProfileQueryHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }
        public async Task<BaseResponseDTO<ProfileDTO>> Handle(GetAProfileQuery request, CancellationToken cancellationToken)
        {
            var profile = await _dbContext.Users.AsNoTracking()
                                          .Select(x => new ProfileDTO
                                          {
                                              Username = x.Username,
                                              Bio = x.Bio,
                                              Email = x.Email,
                                              ProfileImage = x.ProfileImage,
                                              Following = x.FollowedUsers.Any(fl => fl.FollowerId == _currentUser.Id)
                                          }).FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

            if (profile is null)
            {
                throw new RestException(HttpStatusCode.NotFound, "User not found!");
            }

            return new BaseResponseDTO<ProfileDTO>
            {
                Code = HttpStatusCode.OK,
                Message = $"Successfully get {request.Username} user ",
                Data = profile
            };
        }
    }
}
