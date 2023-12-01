using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealWorldConduit.Application.Articles.Queries;
using RealWorldConduit.Application.Users.Queries;

namespace Conduit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetGlobalBlogs([FromQuery] GetPagingGlobalBlogsQuery request, CancellationToken cancellationToken)
        {
            var globalBlogs = await _mediator.Send(request, cancellationToken);
            return Ok(globalBlogs);
        }
    }
}
