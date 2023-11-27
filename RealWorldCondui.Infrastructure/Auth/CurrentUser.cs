using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Security.Claims;

namespace RealWorldCondui.Infrastructure.Auth
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContext;

        public CurrentUser(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public Guid? Id => _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null
            ? new Guid(_httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
            : null;
    }
}
