using Microsoft.AspNetCore.Mvc;

namespace SoutheastRides.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Hello, SouthwestRides!");
    }
}
