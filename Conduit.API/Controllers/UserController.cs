using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealWorldConduit.Application.Users.Queries;

namespace Conduit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var currentUser = await _mediator.Send(new GetCurrentUserQuery());
            return Ok(currentUser);
        }
    }
}
