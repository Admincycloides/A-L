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
using AnL.Filter;
using AnL.Helpers;
using System.Linq;

namespace AnL.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProjectController : Controller
    {

        private readonly IUnitOfWork _UOW;
        private readonly IProject _Project;
        private readonly IUriService uriService;


        public ProjectController(IUnitOfWork UOW, IUriService uriService)
        {
            _UOW = UOW;
            this.uriService = uriService;
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

        [HttpPost]
        public async Task<ActionResult> AddProject(List<ProjectViewModel> ProjectDetails)
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
        public async Task<ActionResult> AddActivity(List<ActivityMaster> ActivityDetails)
        {
            try
            {
                BaseResponse rsp = new BaseResponse();
                var data = await _UOW.ProjectRepository.AddActivity(ActivityDetails);
                if (data == null)
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

        [HttpPost]
        public async Task<ActionResult> DeleteProject(List<ProjectViewModel> project )
        {
            try
            {
                BaseResponse response = new BaseResponse();
                if (project==null)
                {
                    response.Data = project;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.TimesheetDeletionSuccess;
                }
                var DeleteProjectResponse = _UOW.ProjectRepository.DeleteProject(project);
                
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
        public async Task<ActionResult> GetprojectDetailsByID([FromQuery] PaginationFilter filter, int ProjectID, string searchValue)
        {
            try
            {
                var result = await _UOW.ProjectRepository.GetprojectDetailsByID(ProjectID,searchValue);
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var pagedData = result.Activities.OrderByDescending(x => x.ActivityId).Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                                    .Take(validFilter.PageSize)
                                                    .ToList();
                var totalRecords = result.Activities.Count;
                var route = Request.Path.Value;
                var pagedReponse = PaginationHelper.CreatePagedReponse<ProjectActivities>(pagedData, validFilter, totalRecords, uriService, route);
                pagedReponse.ResponseCode = HTTPConstants.OK;
                pagedReponse.ResponseMessage = MessageConstants.ActivityFetchSuccess;
                return Ok(pagedReponse);
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
        public async Task<ActionResult> GetProjectList([FromQuery] PaginationFilter filter, string EmpID, string ProjectName)
        {
            try
            {
                BaseResponse rsp = new BaseResponse();
                var response = await _UOW.ProjectRepository.GetProjectList( EmpID, ProjectName);
                if (response.Count > 0)
                {
                    var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                    var pagedData = response.OrderByDescending(x => x.ProjectId).Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                                        .Take(validFilter.PageSize)
                                                        .ToList();
                    var totalRecords = response.Count;
                    var route = Request.Path.Value;
                    var pagedReponse = PaginationHelper.CreatePagedReponse<ProjectListingViewModel>(pagedData, validFilter, totalRecords, uriService, route);
                    pagedReponse.ResponseCode = HTTPConstants.OK;
                    pagedReponse.ResponseMessage = MessageConstants.ReviewTimesheetListingSuccess;
                    return Ok(pagedReponse);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }

        }

    }
}
