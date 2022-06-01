using Microsoft.AspNetCore.Mvc;
using AnL.Repository.Abstraction;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace TestApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        /// <summary>
        /// Generates OTP using email address.
        /// </summary>
        /// <returns>OTP</returns>
        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult> GenerateOTP()
        {

            return Ok();
        }
    }
}
