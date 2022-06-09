using AnL.Constants;
using AnL.Filter;
using AnL.Helpers;
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
        private readonly IUriService uriService;
        public TimesheetController( IUnitOfWork UOW, IUriService uriService)
        {
            _UOW = UOW;
            this.uriService = uriService;
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
                    response.ResponseMessage = MessageConstants.TimesheetListingSuccess;
                    return Ok(response);
                }
                else
                {
                    response.Data = null;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.TimesheetListingFailed;
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
        public async Task<ActionResult> GetDetails([FromQuery] PaginationFilter filter,TimesheetViewModel timesheetDetails)
        {
            try
            {
                var data = _UOW.TimesheetDetailRepository.GetAllByCondition(x => x.EmployeeId == timesheetDetails.EmployeeId && x.Date >= DateTime.Parse(timesheetDetails.FromDate) && x.Date <= DateTime.Parse(timesheetDetails.ToDate)).Include(x=>x.Project).Include(x=>x.Activity);

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
                    details.ProjectName = data.Where(x => x.ProjectId == tuple.Item1).Select(x => x.Project.ProjectName).FirstOrDefault();
                    details.ActivityId = tuple.Item2;
                    details.ActivityName= data.Where(x => x.ActivityId == tuple.Item2).Select(x=>x.Activity.ActivityName).FirstOrDefault();
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
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var pagedData=model.TimesheetDetails.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                                    .Take(validFilter.PageSize)
                                                    .ToList();
                var totalRecords = model.TimesheetDetails.Count;
                var route = Request.Path.Value;
                var pagedReponse = PaginationHelper.CreatePagedReponse<Details>(pagedData, validFilter, totalRecords, uriService, route);
                return Ok(pagedReponse);
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
                //Add check for existing record in DB for project and activity
                BaseResponse response = new BaseResponse();
                var existingProjectActivity = _UOW.TimesheetDetailRepository.GetAllByCondition(x => x.ProjectId == timesheetDetails.ProjectId && x.ActivityId == timesheetDetails.ActivityId && x.Date.Date == timesheetDetailsList[2].Date.Date);
                if(existingProjectActivity.Count() > 0)
                {
                    response.Data = null;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.TimesheetAlreadyExists;
                    return BadRequest(response);
                }
                var AddTimesheetDetailsResponse = _UOW.TimesheetDetailRepository.AddDetails(timesheetDetailsList);
                if (AddTimesheetDetailsResponse)
                {
                    response.Data = AddTimesheetDetailsResponse;
                    response.ResponseCode = HTTPConstants.OK;
                    response.ResponseMessage = MessageConstants.TimesheetAdditionSuccess;

                }
                else
                {
                    response.Data = AddTimesheetDetailsResponse;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.TimesheetAdditionFailed;
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
                    response.ResponseMessage = MessageConstants.TimesheetModificationSuccess;
                }
                else
                {
                    response.Data = ModifyTimesheetResponse;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.TimesheetModificationFailed;
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
                    response.ResponseMessage = MessageConstants.TimesheetDeletionSuccess;
                }
                else
                {
                    response.Data = DeleteTimesheetResponse;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.TimesheetDeletionFailed;
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
                        details.SubmittedDate = DateTime.UtcNow;
                        details.EmployeeRemarks = listrecords.EmployeeRemarks;
                        if(details.UniqueId!=0)
                        {
                            timesheetDetailsList.Add(details);
                        }
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
        [HttpGet]
        public async Task<ActionResult> GetReviewTimesheet([FromQuery] PaginationFilter filter, string EmployeeID,string searchValue)
        {
            List<TimesheetViewModel> result = new List<TimesheetViewModel>();
            try
            {
                var data = _UOW.TimesheetDetailRepository.GetAllByCondition(x => x.SubmittedTo.Contains(EmployeeID)).Include(x => x.Project).ToList();
                if (!string.IsNullOrEmpty(searchValue))
                {   
                    data = _UOW.TimesheetDetailRepository.GetAllByCondition(x => x.SubmittedTo.Contains(EmployeeID)).Include(x => x.Project).Where(x=>x.Project.ProjectName.Contains(searchValue)).ToList();
                }
                //var data = _UOW.TimesheetDetailRepository.GetAllByCondition(x => x.SubmittedTo.Contains(EmployeeID)).Include(x => x.Project).ToList();
                var employeeList = data.Select(x => new
                {
                    x.Project.ProjectName,
                    x.ProjectId,
                    x.EmployeeId,
                    x.SubmittedDate.Value.Date,
                    x.Status,
                    EmployeeName = String.Concat(_UOW.EmployeeDetailsRepository.GetById(x.EmployeeId).FirstName, " ", _UOW.EmployeeDetailsRepository.GetById(x.EmployeeId).LastName)
                }).Distinct().ToList();
                foreach (var employee in employeeList)
                {
                    TimesheetViewModel timesheetViewModel = new TimesheetViewModel();
                    timesheetViewModel.ProjectName = employee.ProjectName;
                    timesheetViewModel.ProjectId = employee.ProjectId;
                    timesheetViewModel.EmployeeId = employee.EmployeeId;
                    timesheetViewModel.EmployeeName = employee.EmployeeName;
                    timesheetViewModel.Date = employee.Date;
                    timesheetViewModel.Status = employee.Status;
                    result.Add(timesheetViewModel);
                }
                BaseResponse response = new BaseResponse();
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var pagedData = result.OrderByDescending(x=>x.Date).Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                                    .Take(validFilter.PageSize)
                                                    .ToList();
                var totalRecords = result.Count;
                var route = Request.Path.Value;
                var pagedReponse = PaginationHelper.CreatePagedReponse<TimesheetViewModel>(pagedData, validFilter, totalRecords, uriService, route);
                pagedReponse.ResponseCode=HTTPConstants.OK;
                pagedReponse.ResponseMessage = MessageConstants.ReviewTimesheetListingSuccess;
                return Ok(pagedReponse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }
        }


        [HttpPost]
        public async Task<ActionResult> GetReviewTimesheetDetails([FromQuery] PaginationFilter filter, TimesheetViewModel model,string search)
        {
            try
            {
                var data = _UOW.TimesheetDetailRepository.GetAllByCondition(x => x.EmployeeId == model.EmployeeId && x.ProjectId == model.ProjectId && x.SubmittedDate.Value.Date == model.Date.Date).Include(x => x.Project).Include(x => x.Activity).ToList();
                if (!string.IsNullOrEmpty(search))
                {
                    data = _UOW.TimesheetDetailRepository.GetAllByCondition(x => x.EmployeeId == model.EmployeeId && x.ProjectId == model.ProjectId && x.SubmittedDate.Value.Date == model.Date.Date).Include(x => x.Project).Include(x => x.Activity).Where(x=>x.Activity.ActivityName.Contains(search)).ToList();
                }
                List<Details> weekDetails = new List<Details>();
                //Tuple for project and its activity
                var projectList = data.Select(x => Tuple.Create(x.ProjectId, x.ActivityId)).Distinct().ToList();
                foreach (Tuple<int, int> tuple in projectList)
                {
                    Details details = new Details();
                    details.ProjectName = model.ProjectName;
                    details.ProjectId = tuple.Item1;
                    details.ActivityId = tuple.Item2;
                    details.ActivityName = data.Where(x => x.ProjectId == tuple.Item1 && x.ActivityId == tuple.Item2).Select(x=>x.Activity.ActivityName).FirstOrDefault();
                    details.Remarks = data.Where(x => x.ProjectId == tuple.Item1 && x.ActivityId == tuple.Item2 && x.SubmittedDate.Value.Date == model.Date.Date).Select(x => x.Remarks).FirstOrDefault();
                    details.Status = data.Where(x => x.ProjectId == tuple.Item1 && x.ActivityId == tuple.Item2).Select(x => x.Status).FirstOrDefault();
                    List<TimeSpent> timeSpentList = new List<TimeSpent>();
                    var fromDate = data.Where(x => x.ProjectId == tuple.Item1 && x.ActivityId == tuple.Item2).Min(x => x.Date);
                    var toDate = data.Where(x => x.ProjectId == tuple.Item1 && x.ActivityId == tuple.Item2).Max(x => x.Date);
                    foreach (DateTime day in EachDay(fromDate, toDate))
                    {
                        TimeSpent timeSpent = new TimeSpent();
                        timeSpent.Date = day;
                        timeSpent.NumberOfHours = data.Where(x => x.ProjectId == tuple.Item1 && x.ActivityId == tuple.Item2 && x.Date==day.Date).Select(x => x.NumberOfHours).SingleOrDefault();
                        timeSpent.UniqueId = data.Where(x => x.ProjectId == tuple.Item1 && x.ActivityId == tuple.Item2 && x.Date == day.Date).Select(x => x.UniqueId).FirstOrDefault();
                        timeSpentList.Add(timeSpent);
                        details.TimeTaken = timeSpentList;
                    }
                    weekDetails.Add(details);
                }
                BaseResponse response = new BaseResponse();
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var pagedData = weekDetails.OrderByDescending(x => x.ActivityId).Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                                    .Take(validFilter.PageSize)
                                                    .ToList();
                var totalRecords = weekDetails.Count;
                var route = Request.Path.Value;
                var pagedReponse = PaginationHelper.CreatePagedReponse<Details>(pagedData, validFilter, totalRecords, uriService, route);
                pagedReponse.ResponseCode = HTTPConstants.OK;
                pagedReponse.ResponseMessage = MessageConstants.ReviewTimesheetListingSuccess;
                return Ok(pagedReponse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SupervisorDecision( List<Details> model,string SupervisorID,string Action)
        {
            try
            {
                bool result = false;
                List<TimesheetDetails> timesheetDetailsList = new List<TimesheetDetails>();
                foreach (var listrecords in model)
                {
                    foreach (var timesheet in listrecords.TimeTaken)
                    {
                        TimesheetDetails details = new TimesheetDetails();
                        details.UniqueId = timesheet.UniqueId;
                        details.ApprovedRejectedBy = SupervisorID;
                        details.SupervisorRemarks = listrecords.SupervisorRemarks;
                        if (details.UniqueId != 0)
                        {
                            timesheetDetailsList.Add(details);
                        }
                    }
                }
                if (Action == TimeSheetStatus.Approved)
                {
                    result =  _UOW.TimesheetDetailRepository.SupervisorAction(timesheetDetailsList, TimeSheetStatus.Approved);
                }
                else if (Action == TimeSheetStatus.Rejected)
                {
                    result =  _UOW.TimesheetDetailRepository.SupervisorAction(timesheetDetailsList, TimeSheetStatus.Rejected);
                }
                else
                {
                    return BadRequest("Supervisor Action Status " + Action + "not acceptable!!");
                }
                BaseResponse response = new BaseResponse();
                if (result)
                {
                    response.Data = result;
                    response.ResponseCode = HTTPConstants.OK;
                    if(Action == TimeSheetStatus.Approved)
                        response.ResponseMessage = MessageConstants.SupervisorApproved;
                    if(Action==TimeSheetStatus.Rejected)
                        response.ResponseMessage = MessageConstants.SupervisorRejected;
                }
                else
                {
                    response.Data = result;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.SupervisorActionFailed;
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
        public async Task<ActionResult> GetTimesheetReport(ReportRequest request)
        {
            //IQueryable data = _UOW.TimesheetDetailRepository.GetAll().Where(x => x.ProjectId == request.ProjectIds[0] && x.EmployeeId == request.EmployeeId[0] && x.Date.Date>=request.FromDate.Date && x.Date.Date <= request.ToDate.Date)
            //                    .Select(x=> new ReportViewModel { });
            //var result=data.AsQueryable().whe
            return Ok();
        }


            [NonAction]
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
