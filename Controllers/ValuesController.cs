using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // This endpoint requires authentication
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            // Your authenticated logic here
            // User information is available through User.Identity.Name
            return Ok(new { Message = "Authenticated endpoint", User.Identity.Name });
        }
    }
}
