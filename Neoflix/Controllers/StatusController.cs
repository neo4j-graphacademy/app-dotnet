using Microsoft.AspNetCore.Mvc;

namespace Neoflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        /// <summary>
        /// Get some basic information about the status of the API, <br/>
        /// including whether the API has been defined and whether a transaction has been bound to the request. <br/><br/>
        /// This is for debugging purposes only and isn't used within the course
        /// </summary>
        /// <returns>Http Result</returns>
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