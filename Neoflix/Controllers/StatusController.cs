using Microsoft.AspNetCore.Mvc;

namespace Neoflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAsync()
        {
            var result = new
            {
                driver = Neo4j.Driver != null
            };
            return Ok(result);
        }
    }
}