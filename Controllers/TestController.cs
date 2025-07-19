using Microsoft.AspNetCore.Mvc;

namespace migration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Simple test works!");
        }
    }
}