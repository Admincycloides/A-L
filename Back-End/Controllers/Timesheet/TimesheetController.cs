using AnL.Constants;
using AnL.Models;
using AnL.Repository.Abstraction;
using AnL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TimesheetController : Controller
    {
        private readonly IUnitOfWork _UOW;
        private readonly ITimesheetDetail _timesheetDetail;
        public TimesheetController( IUnitOfWork UOW)
        {
            _UOW = UOW;
        }
        /// <summary>
        /// Fetch Timesheet Details based on the user Logged in
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> GetDetails(TimesheetViewModel timesheetDetails)
        {
            try
            {
                //Repo Code

                List<string> employeeIDs = new List<string>();
                employeeIDs.Add(timesheetDetails.EmployeeId);
                var empDetails = _UOW.EmployeeDetailsRepository.GetEmployeeDetails(employeeIDs);
                List<string> subEmpDetails = new List<string>();
                List<TimesheetDetails> sheetDetails = new List<TimesheetDetails>();
                //Get Manager and all employee Timesheet Details based on dates
                if (empDetails[0].SupervisorFlag == "Y" && empDetails.Count == 1)
                {
                    //Get Manager Timesheet Details
                    var managerTimeSheetDetails = _UOW.TimesheetDetailRepository.GetAllByCondition(x => x.EmployeeId == empDetails[0].EmployeeId && x.Date >= DateTime.Parse(timesheetDetails.FromDate) && x.Date <= DateTime.Parse(timesheetDetails.ToDate)).Include(x => x.Project).Include(x => x.Activity).ToList();
                    foreach (var data in managerTimeSheetDetails)
                    {
                        sheetDetails.Add(data);
                    }
                    //Get all employee Timeesheet Details
                    subEmpDetails = _UOW.EmployeeDetailsRepository.GetAllByCondition(x => x.SupervisorFlag == "N").Select(x => x.EmployeeId).ToList();
                    foreach (var emp in subEmpDetails)
                    {
                        var employeeTimeSheetDetails = _UOW.TimesheetDetailRepository.GetAllByCondition(x => x.EmployeeId == emp && x.Date >= DateTime.Parse(timesheetDetails.FromDate) && x.Date <= DateTime.Parse(timesheetDetails.ToDate)).Include(x=>x.Project).Include(x=>x.Activity).ToList();
                        foreach (var data in employeeTimeSheetDetails)
                        {
                            sheetDetails.Add(data);
                        }
                    }
                }
                else
                {
                    //sheetDetails.Add(_UOW.TimesheetDetailRepository.GetById(empDetails[0]));
                    var result = _UOW.TimesheetDetailRepository.GetAllByCondition(x => x.EmployeeId == empDetails[0].EmployeeId && x.Date >= DateTime.Parse(timesheetDetails.FromDate) && x.Date <= DateTime.Parse(timesheetDetails.ToDate)).Include(x=>x.Project).Include(x=>x.Activity).ToList();
                    foreach (var data in result)
                    {
                        sheetDetails.Add(data);
                    }
                }
                //Conversion to data friendly to pass to UI
                MasterTimesheetViewModel model =new MasterTimesheetViewModel();
                List<TimesheetViewModel> sheetDetails2 = new List<TimesheetViewModel>();
                foreach (var data in sheetDetails)
                {
                    TimesheetViewModel viewModel=new TimesheetViewModel();
                    viewModel.Date= data.Date;
                    viewModel.EmployeeId= data.EmployeeId;
                    //viewModel.EmployeeName = data.EmployeeName;
                    viewModel.ProjectId= data.ProjectId;
                    viewModel.ProjectName = data.Project.ProjectName;
                    viewModel.ActivityId= data.ActivityId;
                    viewModel.ActivityName = data.Activity.ActivityName;
                    viewModel.NumberOfHours= data.NumberOfHours;
                    viewModel.Remarks= data.Remarks;
                    viewModel.Status= data.Status;
                    //viewModel.LastUpdatedDate = data.LastUpdatedDate;
                    //viewModel.LastUpdatedBy = data.LastUpdatedBy;
                    viewModel.UniqueId=data.UniqueId;
                    sheetDetails2.Add(viewModel);
                }
                model.TimesheetDetails = sheetDetails2;
                model.ManagerId = empDetails[0].ManagerId;
                model.ManagerName = String.Concat(empDetails[0].FirstName, empDetails[0].LastName);
                BaseResponse response = new BaseResponse();
                //if (subEmpDetails != null || subEmpDetails.Count() > 0)
                if (sheetDetails.Count > 0)
                {
                    response.Data = model;
                    response.ResponseCode = HTTPConstants.OK;
                    response.ResponseMessage = MessageConstants.TimsheetListingSuccess;
                    return Ok(response);
                }
                else
                {
                    response.Data = null;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.TimsheetListingFailed;
                    return BadRequest(response);
                }
                //Controller Code
                ////var details =  _UOW.TimesheetDetailRepository.GetTimesheetDetails(timesheetDetails);

                ////if (details != null)
                ////    return Ok(details);
                ////else
                ////    return BadRequest("No Details");
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }
        }

        /// <summary>
        /// Adding Timesheet records based on Project and its Activities
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AddTimesheetDetails(List<TimesheetViewModel> timesheetDetails)
        {
            try
            {
                var AddTimesheetDetailsResponse=_UOW.TimesheetDetailRepository.AddDetails(timesheetDetails);
                BaseResponse response = new BaseResponse();
                if (AddTimesheetDetailsResponse)
                {
                    response.Data = AddTimesheetDetailsResponse;
                    response.ResponseCode= HTTPConstants.OK;
                    response.ResponseMessage = MessageConstants.TimsheetAdditionSuccess;

                }
                else
                {
                    response.Data = AddTimesheetDetailsResponse;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.TimsheetAdditionFailed;
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

        /// <summary>
        /// Modifying Timesheet records based on Project and its Activities
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> ModifyTimesheet(List<TimesheetViewModel> timesheetDetails)
        {
            try
            {
                foreach (var timesheet in timesheetDetails)
                {
                    TimesheetDetails existingSheet = _UOW.TimesheetDetailRepository.GetById(timesheet.UniqueId);
                    existingSheet.ProjectId = timesheet.ProjectId;
                    existingSheet.ActivityId = timesheet.ActivityId;
                    existingSheet.NumberOfHours = timesheet.NumberOfHours;
                    existingSheet.Remarks = timesheet.Remarks;
                }
                var ModifyTimesheetResponse = _UOW.TimesheetDetailRepository.ModifyTimesheetDetails(timesheetDetails);
                BaseResponse response = new BaseResponse();
                if (ModifyTimesheetResponse)
                {
                    response.Data = ModifyTimesheetResponse;
                    response.ResponseCode = HTTPConstants.OK;
                    response.ResponseMessage = MessageConstants.TimsheetModificationSuccess;
                }
                else
                {
                    response.Data = ModifyTimesheetResponse;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.TimsheetModificationFailed;
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

        /// <summary>
        /// Deletion of Timesheet records based on Project and its Activities
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> DeleteTimesheet(List<TimesheetViewModel> timesheetDetails)
        {
            try
            {
                List<TimesheetDetails> details = new List<TimesheetDetails>();
                foreach (var timesheet in timesheetDetails)
                {
                    TimesheetDetails eachItem = new TimesheetDetails() { UniqueId = timesheet.UniqueId };
                    details.Add(eachItem);
                }
                var DeleteTimesheetResponse=_UOW.TimesheetDetailRepository.DeleteTimesheetDetails(details);
                BaseResponse response = new BaseResponse();
                if (DeleteTimesheetResponse)
                {
                    response.Data = DeleteTimesheetResponse;
                    response.ResponseCode = HTTPConstants.OK;
                    response.ResponseMessage = MessageConstants.TimsheetDeletionSuccess;

                }
                else
                {
                    response.Data = DeleteTimesheetResponse;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.TimsheetDeletionFailed;
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
    }
}
