using AnL.Repository.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AnL.ViewModel;
using System;
using Serilog;
using AnL.Constants;

namespace AnL.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProjectController : Controller
    {

        private readonly IUnitOfWork _UOW;
        private readonly IProject _Project;
        public ProjectController(IUnitOfWork UOW)
        {
            _UOW = UOW;
           
        }


        [HttpGet]
        public async Task<ActionResult> GetprojectListbyEmployeeID(string EmployeeID)
        {
           

            try
            {
                BaseResponse rsp = new BaseResponse();
                rsp.Data = await _UOW.ProjectRepository.GetprojectListbyEmployeeID(EmployeeID);
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }
        }


        [HttpGet]
        public async Task<ActionResult> GetActivityList ()
        {
            try
            {
                BaseResponse rsp = new BaseResponse();
                rsp.Data = await _UOW.ProjectRepository.GetActivityList();
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }

        }

        [HttpPost]
        public async Task<ActionResult> AddProject(ProjectViewModel ProjectDetails)
        {
            try
            {
                BaseResponse rsp = new BaseResponse();
                var data = await _UOW.ProjectRepository.AddProject(ProjectDetails);
                if (data == null)
                {
                    rsp.Data = "Project Name already exist.";
                    return Conflict(rsp);
                }
                else
                {
                    rsp.Data = data;
                    rsp.ResponseMessage = MessageConstants.ProjectAdditionSuccess;
                }
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AllocateResources( MapProjectResources Data)
        {
            try
            {
                BaseResponse rsp = new BaseResponse();
                var data = await _UOW.ProjectRepository.AllocateResources(Data);
                if (data == null)
                {
                    rsp.Data = "Invalid Ids";
                    return BadRequest(rsp);
                }
                else
                {
                    rsp.Data = data;
                    rsp.ResponseMessage = MessageConstants.ProjectAllocationSuccess;
                }
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
