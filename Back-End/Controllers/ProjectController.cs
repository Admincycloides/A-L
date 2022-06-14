using AnL.Repository.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AnL.ViewModel;
using System;
using Serilog;
using AnL.Constants;
using AnL.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AnL.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
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
        [HttpGet]
        public async Task<ActionResult> GetClientList()
        {
            try
            {
                BaseResponse rsp = new BaseResponse();
                rsp.Data = await _UOW.ProjectRepository.GetClientList();
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }

        }

        [HttpPost]
        public async Task<ActionResult> AddProject(List<ProjectViewModel> ProjectDetails)
        {
            var user = HttpContext.User;
            var Userid = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                BaseResponse rsp = new BaseResponse();
                var data = await _UOW.ProjectRepository.AddProject(ProjectDetails,Userid);
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
        public async Task<ActionResult> AddActivity(List<ActivityMaster> ActivityDetails)
        {
            try
            {
                BaseResponse rsp = new BaseResponse();
                var data = await _UOW.ProjectRepository.AddActivity(ActivityDetails);
                if (data != null)
                {
                    rsp.Data = "Activity Name already exist.";
                    return Conflict(rsp);
                }
                else
                {
                    rsp.Data = data;
                    rsp.ResponseMessage = MessageConstants.ActivityAdditionSuccess;
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
        public async Task<ActionResult> EditActivity(List<ActivityMaster> ActivityDetails)
        {
            try
            {
                BaseResponse rsp = new BaseResponse();
                var data = await _UOW.ProjectRepository.EditActivity(ActivityDetails);
                if (data != null)
                {
                    rsp.Data = "Activity Name already exist.";
                    return Conflict(rsp);
                }
                else
                {
                    rsp.Data = data;
                    rsp.ResponseMessage = MessageConstants.ActivityAdditionSuccess;
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
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public async Task<ActionResult> EditProject(EditProjectView project)
        {
            BaseResponse response = new BaseResponse();
            if (project == null)
            {
                response.Data = project;
                response.ResponseCode = HTTPConstants.BAD_REQUEST;
                response.ResponseMessage = MessageConstants.ProjectDeletionFailed;
            }
            var EditProjectResponse = _UOW.ProjectRepository.EditProject(project);

            if (EditProjectResponse != null)
            {
                response.Data = EditProjectResponse;
                response.ResponseCode = HTTPConstants.OK;
                response.ResponseMessage = MessageConstants.ProjectDeletionSuccess;

            }
            else
            {
                response.Data = EditProjectResponse;
                response.ResponseCode = HTTPConstants.BAD_REQUEST;
                response.ResponseMessage = MessageConstants.ProjectDeletionFailed;
                return BadRequest(response);
            }
            return Ok(response);


        

}
        [HttpPost]
        public async Task<ActionResult> EditProjectDetails(EditProjectView project)
        {
            BaseResponse response = new BaseResponse();
            if (project == null)
            {
                response.Data = project;
                response.ResponseCode = HTTPConstants.BAD_REQUEST;
                response.ResponseMessage = MessageConstants.ProjectDeletionFailed;
            }
            var EditProjectResponse = _UOW.ProjectRepository.EditProjectDetails(project);

            if (EditProjectResponse != null)
            {
                response.Data = EditProjectResponse;
                response.ResponseCode = HTTPConstants.OK;
                response.ResponseMessage = MessageConstants.EditProjectSuccess;

            }
            else
            {
                response.Data = EditProjectResponse;
                response.ResponseCode = HTTPConstants.BAD_REQUEST;
                response.ResponseMessage = MessageConstants.EditProjectSuccess;
                return BadRequest(response);
            }
            return Ok(response);




        }


        [HttpPost]
        public async Task<ActionResult> EditProjectActive(EditProjectView project)
        {
            BaseResponse response = new BaseResponse();
            if (project == null)
            {
                response.Data = project;
                response.ResponseCode = HTTPConstants.BAD_REQUEST;
                response.ResponseMessage = MessageConstants.ProjectDeletionFailed;
            }
            var EditProjectResponse = _UOW.ProjectRepository.EditProjectActive(project);

            if (EditProjectResponse != null)
            {
                response.Data = EditProjectResponse;
                response.ResponseCode = HTTPConstants.OK;
                response.ResponseMessage = MessageConstants.EditProjectSuccess;

            }
            else
            {
                response.Data = EditProjectResponse;
                response.ResponseCode = HTTPConstants.BAD_REQUEST;
                response.ResponseMessage = MessageConstants.EditProjectSuccess;
                return BadRequest(response);
            }
            return Ok(response);




        }



        [HttpPost]
        public async Task<ActionResult> DeleteProject(int projectID)
        {
            try
            {
                BaseResponse response = new BaseResponse();
                if (projectID==0)
                {
                    response.Data = projectID;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.ProjectDeletionFailed;
                }
                var DeleteProjectResponse = _UOW.ProjectRepository.DeleteProject(projectID);
                
                if (DeleteProjectResponse)
                {
                    response.Data = DeleteProjectResponse;
                    response.ResponseCode = HTTPConstants.OK;
                    response.ResponseMessage = MessageConstants.ProjectDeletionSuccess;

                }
                else
                {
                    response.Data = DeleteProjectResponse;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.ProjectDeletionFailed;
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }
        }
       
 [HttpGet]
        public async Task<ActionResult> GetprojectDetailsByID(int ProjectID)
        {
            try
            {
                BaseResponse rsp = new BaseResponse();
                rsp.Data = await _UOW.ProjectRepository.GetprojectDetailsByID(ProjectID);
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }

        } 
        [HttpGet]
        public async Task<ActionResult> GetAllProjectList()
        {
            try
            {
                BaseResponse rsp = new BaseResponse();
                rsp.Data = await _UOW.ProjectRepository.GetAllProject();
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }

        } 
        [HttpGet]
        public async Task<ActionResult> GetProjectList(string EmpID, string ProjectName)
        {
            try
            {
                BaseResponse rsp = new BaseResponse();
                rsp.Data = await _UOW.ProjectRepository.GetProjectList( EmpID, ProjectName);
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
