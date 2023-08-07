using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class DataController : Controller
    {
        
        // GET
        [HttpGet("[action]")]
        public IActionResult PrivateChats()
        {
            return Ok("PrivateChat");
        }
    }
}