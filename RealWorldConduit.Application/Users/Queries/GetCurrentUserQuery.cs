using MediatR;
using Microsoft.EntityFrameworkCore;
using RealworldConduit.Infrastructure.Common;
using RealWorldConduit.Domain.Entities;
using RealWorldConduit.Infrastructure;
using RealWorldConduit.Infrastructure.Auth;
using RealWorldConduit.Infrastructure.Common;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWorldConduit.Application.Users.Queries
{
    public class GetCurrentUserQuery : IRequestWithBaseResponse<User>
    {

    }
    public class GetCurrentUserQueryHandler : IRequestWithBaseResponseHandler<GetCurrentUserQuery, User>
    {
        private readonly MainDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public GetCurrentUserQueryHandler(MainDbContext dbContext, ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }
        public async Task<BaseResponse<User>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _dbContext.Users.Where(u => u.Id == _currentUser.Id)
                                              .FirstOrDefaultAsync();

            if (currentUser == null)
            {
                throw new RestException(HttpStatusCode.NotFound, "User Not Found");
            }

            return new BaseResponse<User> {
                Code = HttpStatusCode.OK,
                Message = "Successfully get current user",
                Data = currentUser
            };
        }
    }
}
