using Microsoft.AspNetCore.Mvc;

namespace Api.GraphQL.Controllers
{
    // TO DO: DELETE THIS CONTROLLER LATER
    [Route("api/[controller]")]
    [ApiController]
    public class InitializeController : ControllerBase
    {
        public IActionResult Get()
        {
            return Ok("Test");
        }
    }
}