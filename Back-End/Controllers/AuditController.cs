using AnL.Repository.Abstraction;
using AnL.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuditController : Controller
    {
        private readonly IUnitOfWork _UOW;
        private readonly IAudit _Audit;
        public AuditController(IUnitOfWork UOW)
        {
            _UOW = UOW;

        }

        [HttpGet]
        public async Task<ActionResult> GetAuditLog()
        {
            try
            {
                BaseResponse rsp = new BaseResponse();
                rsp.Data = await _UOW.AuditRepository.GetAuditLog();
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }

        }
    }
}
