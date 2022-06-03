using AnL.Models;
using AnL.Repository.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnL.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeeDetailsController : Controller
    {
        private readonly IUnitOfWork _UOW;
        private readonly IEmployeeDetails _timesheetDetail;
        public EmployeeDetailsController(IUnitOfWork UOW)
        {
            _UOW = UOW;
        }
        [HttpGet]
        public ActionResult getEmployeeDetail(List<string> EmployeeID)
        {
            var results = _UOW.EmployeeDetailsRepository.getEmployeeDetails(EmployeeID);
            if (results != null)
            {
                return Ok(results);
            }
            else
            {
                return null;
            }
        }
    }
}
