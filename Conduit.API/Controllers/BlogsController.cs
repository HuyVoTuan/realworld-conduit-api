using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWorldConduit.Application.Articles.Queries;
using RealWorldConduit.Application.Blogs.Commands;
using RealWorldConduit.Application.Blogs.Queries;

namespace Conduit.API.Controllers
{
    [Route("api/blogs")]
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateNewBlog([FromBody] CreateBlogCommand request, CancellationToken cancellationToken)
        {
            var newBlog = await _mediator.Send(request, cancellationToken);
            return Ok(newBlog);
        }

        [Authorize]
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetABlog([FromRoute] GetABlogQuery request, CancellationToken cancellationToken)
        {
            var blog = await _mediator.Send(request, cancellationToken);
            return Ok(blog);
        }

        [Authorize]
        [HttpPut("{slug}")]
        public async Task<IActionResult> UpdateCurrentBlog([FromBody] UpdateCurrentBlogCommand request, CancellationToken cancellationToken)
        {
            var newBlog = await _mediator.Send(request, cancellationToken);
            return Ok(newBlog);
        }

        [Authorize]
        [HttpDelete("{slug}")]
        public async Task<IActionResult> DeleteCurrentBlog(CancellationToken cancellationToken)
        {
            var newBlog = await _mediator.Send(new DeleteCurrentBlogCommand(), cancellationToken);
            return Ok(newBlog);
        }
    }
}
