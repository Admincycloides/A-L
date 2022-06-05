using AnL.Constants;
using AnL.Models;
using AnL.Repository.Abstraction;
using AnL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AnL.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TimesheetController : Controller
    {
        private readonly IUnitOfWork _UOW;
        private readonly IConfiguration _configuration;
        public TimesheetController( IUnitOfWork UOW)
        {
            _UOW = UOW;
        }
        /// <summary>
        /// Fetch Timesheet Details based on the user Logged in
        /// </summary>
        [NonAction]
        public async Task<ActionResult> TestGetDetails(TimesheetViewModel timesheetDetails)
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
                //model.TimesheetDetails = sheetDetails2;
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
        /// Fetch Timesheet Details based on the user Logged in
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> GetDetails(TimesheetViewModel timesheetDetails)
        {
            try
            {
                var data = _UOW.TimesheetDetailRepository.GetAllByCondition(x => x.EmployeeId == timesheetDetails.EmployeeId && x.Date >= DateTime.Parse(timesheetDetails.FromDate) && x.Date <= DateTime.Parse(timesheetDetails.ToDate));

                var projectList = data.Select(x => Tuple.Create(x.ProjectId, x.ActivityId)).Distinct().ToList();
                MasterTimesheetViewModel model = new MasterTimesheetViewModel();
                List<string> employeeIDs = new List<string>();
                employeeIDs.Add(timesheetDetails.EmployeeId);
                var empDetails = _UOW.EmployeeDetailsRepository.GetEmployeeDetails(employeeIDs);
                model.ManagerId = empDetails[0].ManagerId;
                model.ManagerName = String.Concat(empDetails[0].FirstName, empDetails[0].LastName);
                List<Details> weekDetails = new List<Details>();
                foreach (Tuple<int, int> tuple in projectList)
                {
                    Details details = new Details();
                    details.ProjectId = tuple.Item1;
                    //details.ProjectName=
                    details.ActivityId = tuple.Item2;
                    details.Remarks = data.Where(x => x.ProjectId == tuple.Item1 && x.ActivityId == tuple.Item2).Select(x => x.Remarks).FirstOrDefault();
                    details.Status = data.Where(x => x.ProjectId == tuple.Item1 && x.ActivityId == tuple.Item2).Select(x => x.Status).FirstOrDefault();
                    List<TimeSpent> timeSpentList = new List<TimeSpent>();
                    foreach (DateTime day in EachDay(DateTime.Parse(timesheetDetails.FromDate), DateTime.Parse(timesheetDetails.ToDate)))
                    {
                        //details.UniqueId=data
                        TimeSpent timeSpent = new TimeSpent();
                        timeSpent.Date = day;
                        timeSpent.NumberOfHours = data.Where(x => x.ProjectId == tuple.Item1 && x.ActivityId == tuple.Item2 && x.Date.Date == day.Date).Select(x => x.NumberOfHours).ToList().AsQueryable().Sum();
                        timeSpent.UniqueId = data.Where(x => x.ProjectId == tuple.Item1 && x.ActivityId == tuple.Item2 && x.Date.Date == day.Date).Select(x => x.UniqueId).FirstOrDefault();
                        timeSpentList.Add(timeSpent);

                        details.TimeTaken = timeSpentList;
                        //weekDetails.Add(details);
                    }
                    weekDetails.Add(details);
                }
                model.TimesheetDetails = weekDetails;
                BaseResponse response = new BaseResponse();
                if (model.TimesheetDetails.Count>0)
                {
                    response.Data = model;
                    response.ResponseCode = HTTPConstants.OK;
                    response.ResponseMessage = MessageConstants.TimsheetListingSuccess;
                }
                else if (model.TimesheetDetails.Count == 0)
                {
                    response.Data =null;
                    response.ResponseCode = HTTPConstants.OK;
                    response.ResponseMessage = MessageConstants.TimesheetListingNoRecords;
                    return Ok(response);
                }
                else
                {
                    response.Data = null;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.TimsheetListingFailed;
                    return BadRequest(response);
                }
                return Ok(model);
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
        public async Task<ActionResult> AddTimesheetDetails(Details timesheetDetails,string EmployeeId)
        {
            try
            {
                List<TimesheetDetails> timesheetDetailsList = new List<TimesheetDetails>();
                foreach (var timesheet in timesheetDetails.TimeTaken)
                {
                    TimesheetDetails data = new TimesheetDetails();
                    data.ProjectId = timesheetDetails.ProjectId;
                    data.ActivityId = timesheetDetails.ActivityId;
                    data.EmployeeId = EmployeeId;
                    data.Remarks = timesheetDetails.Remarks;
                    data.Status = TimeSheetStatus.InProgress;
                    data.Date = timesheet.Date;
                    data.NumberOfHours = timesheet.NumberOfHours;
                    timesheetDetailsList.Add(data);
                }
                var AddTimesheetDetailsResponse = _UOW.TimesheetDetailRepository.AddDetails(timesheetDetailsList);
                BaseResponse response = new BaseResponse();
                if (AddTimesheetDetailsResponse)
                {
                    response.Data = AddTimesheetDetailsResponse;
                    response.ResponseCode = HTTPConstants.OK;
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
        public async Task<ActionResult> ModifyTimesheet(Details timesheetDetails)
        {
            try
            {
                List<TimesheetDetails> timesheetDetailsList = new List<TimesheetDetails>();

                foreach (var timesheet in timesheetDetails.TimeTaken)
                {
                    TimesheetDetails details = new TimesheetDetails();
                    details.UniqueId= timesheet.UniqueId;
                    details.ProjectId = timesheetDetails.ProjectId;
                    details.ActivityId=timesheetDetails.ActivityId;
                    details.NumberOfHours = timesheet.NumberOfHours;
                    details.Remarks=timesheetDetails.Remarks;
                    timesheetDetailsList.Add(details);
                }
                var ModifyTimesheetResponse = _UOW.TimesheetDetailRepository.ModifyTimesheetDetails(timesheetDetailsList);
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
        public async Task<ActionResult> DeleteTimesheet(Details timesheetDetails)
        {
            try
            {
                List<TimesheetDetails> details = new List<TimesheetDetails>();
                foreach (var timesheet in timesheetDetails.TimeTaken)
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
        [HttpPost]
        public async Task<ActionResult> SubmitTimesheetDetails(List<Details> timesheetDetails,string ManagerID,string EmployeeName)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var managerDetails = _UOW.EmployeeDetailsRepository.GetById(ManagerID);
                bool result=false;
                foreach (var listrecords in timesheetDetails)
                {
                    List<TimesheetDetails> timesheetDetailsList = new List<TimesheetDetails>();

                    foreach (var timesheet in listrecords.TimeTaken)
                    {
                        TimesheetDetails details = new TimesheetDetails();
                        details.UniqueId = timesheet.UniqueId;
                        details.SubmittedTo = ManagerID;
                        timesheetDetailsList.Add (details);
                    }
                    result = _UOW.TimesheetDetailRepository.SubmitTimesheet(timesheetDetailsList);
                }
                if (result)
                {
                    response.Data = result;
                    response.ResponseCode = HTTPConstants.OK;
                    response.ResponseMessage = MessageConstants.TimesheetSubmissionSuccess;
                }
                else
                {
                    response.Data = result;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.TimesheetSubmissionFailed;
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

        [NonAction]
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
