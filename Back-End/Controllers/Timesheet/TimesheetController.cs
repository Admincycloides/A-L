using AnL.Models;
using AnL.Repository.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnL.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TimesheetController : Controller
    {
        private readonly IUnitOfWork _UOW;
        public TimesheetController( IUnitOfWork UOW)
        {
            _UOW = UOW;
        }
        [HttpPost]
        public ActionResult<List<TimesheetDetails>> GetDetails(TimesheetDetails timesheetDetails)
        {

            var details =  _UOW.TimesheetDetailRepository.GetTimesheetDetails(timesheetDetails);
            // var InvDetails = _mapper.Map<InventoryItemDetailsViewModel>(details);

            if (details != null)
                return details;
            else
                return null;

        }
    }
}
