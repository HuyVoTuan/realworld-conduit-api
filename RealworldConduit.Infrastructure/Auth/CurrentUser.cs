using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace RealWorldConduit.Infrastructure.Auth
{
    // TODO Implement CurrentUser Later On
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContext;

        public CurrentUser(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public Guid? Id
        {
            get
            {
                var userId = _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                bool isGuidParsed = Guid.TryParse(userId, out Guid id);

                if (userId == null || !isGuidParsed)
                {
                    return null;
                }
                return id;
            }
        }
    }
}
