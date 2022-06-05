using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnL.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProjectController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> GetProjectDetails(string EmployeeID)
        {
            return Ok();
        }
    }
}
