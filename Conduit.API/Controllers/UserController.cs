using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Get([FromBody] string a)
        {
            if (a == "a")
            {
                throw new Exception();
            }
            return Ok();
        }
    }
}
