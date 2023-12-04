using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealWorldConduit.Application.Blogs.Queries;

namespace Conduit.API.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTop10MostUsedTags(CancellationToken cancellationToken)
        {
            var tags = await _mediator.Send(new GetTop10MostUsedTagsQuery(), cancellationToken);
            return Ok(tags);
        }
    }
}
