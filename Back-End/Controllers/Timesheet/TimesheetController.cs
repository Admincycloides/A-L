using AnL.Constants;
using AnL.Filter;
using AnL.Helpers;
using AnL.Models;
using AnL.Repository.Abstraction;
using AnL.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AnL.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TimesheetController : Controller
    {
        private readonly IUnitOfWork _UOW;
        private readonly IConfiguration _configuration;
        private readonly IUriService uriService;
        public TimesheetController( IUnitOfWork UOW, IUriService uriService, IConfiguration configuration)
        {
            _UOW = UOW;
            this._configuration= configuration;
            this.uriService = uriService;
        }
        /// <summary>
        /// Fetch Timesheet Details based on the user Logged in
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> GetDetails([FromQuery] PaginationFilter filter, TimesheetViewModel timesheetDetails)
        {
            try
            {
                List<Details> details = new List<Details>();
                var data = _UOW.TimesheetDetailRepository.GetAllByCondition(x => x.EmployeeId == timesheetDetails.EmployeeId && x.Date >= DateTime.Parse(timesheetDetails.FromDate) && x.Date <= DateTime.Parse(timesheetDetails.ToDate)).Include(x => x.Project).Include(x => x.Activity);
                foreach (var a in data.Select(x => x.ProjectId).Distinct())
                {
                    foreach (var b in data.Select(x => x.ActivityId).Distinct())
                    {
                        var recd = data.Where(x => x.ProjectId == a && x.ActivityId == b).Select(x => new Details
                        {
                            ProjectId = x.ProjectId,
                            ProjectName = x.Project.ProjectName,
                            ActivityId = x.ActivityId,
                            ActivityName = x.Activity.ActivityName,
                            Status = x.Status,
                            Remarks = x.Remarks,
                            EmployeeRemarks = x.EmployeeRemarks,
                            SupervisorRemarks = x.SupervisorRemarks,
                            TimeTaken = (data.Where(y => y.ProjectId == a && y.ActivityId == x.ActivityId).Select(z => new TimeSpent { Date = z.Date, NumberOfHours = z.NumberOfHours, UniqueId = z.UniqueId })).ToList()
                        }).Distinct().ToList();
                        details.AddRange(recd);
                    }
                }
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var pagedData = details.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                                    .Take(validFilter.PageSize)
                                                    .ToList();
                var totalRecords = details.Count;
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
        /// Fetch Timesheet Details based on the user Logged in
        /// </summary>
        [NonAction]
        public async Task<ActionResult> TestGetDetails([FromQuery] PaginationFilter filter,TimesheetViewModel timesheetDetails)
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
                var existingProjectActivity = _UOW.TimesheetDetailRepository.GetAllByCondition(x=>x.EmployeeId==EmployeeId && x.ProjectId == timesheetDetails.ProjectId && x.ActivityId == timesheetDetails.ActivityId && x.Date.Date == timesheetDetails.TimeTaken[2].Date.Date);
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
                //Check for existing record in DB for project and activity
                int randomUniqueID = timesheetDetails.TimeTaken[0].UniqueId;
                BaseResponse response = new BaseResponse();
                var existingProjectActivity = _UOW.TimesheetDetailRepository.GetAllByCondition(x => x.EmployeeId == _UOW.TimesheetDetailRepository.GetById(randomUniqueID).EmployeeId && x.ProjectId == timesheetDetails.ProjectId && x.ActivityId == timesheetDetails.ActivityId && x.Date.Date == timesheetDetails.TimeTaken[0].Date.Date);
                if (existingProjectActivity.Count() > 0 && existingProjectActivity.Select(x => x.UniqueId).FirstOrDefault() != randomUniqueID)
                {
                    response.Data = null;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.TimesheetAlreadyExists;
                    return BadRequest(response);
                }
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
        /// <summary>
        /// Submit timesheet to Supervisor
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> SubmitTimesheetDetails(List<Details> timesheetDetails,string ManagerID,string EmployeeID)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var managerDetails = _UOW.EmployeeDetailsRepository.GetById(ManagerID);
                var employeeDetails = _UOW.EmployeeDetailsRepository.GetById(EmployeeID);
                bool result = false;
                bool mailSent = false;
                var data = timesheetDetails.ToList();
                string employeeRemarks = data.Select(x => x.EmployeeRemarks).FirstOrDefault();
                List<TimesheetDetails> timesheetDetailsList = new List<TimesheetDetails>();
                foreach (var a in data.Select(x => x.ProjectId).Distinct())
                {
                    foreach (var b in data.Where(y => y.ProjectId == a).Select(y => y.ActivityId).Distinct())
                    {
                        var recd = data.Where(y => y.ActivityId == b && y.ProjectId == a).SelectMany(y => y.TimeTaken).Select(z => new TimesheetDetails
                        {
                            UniqueId = z.UniqueId,
                            ProjectId = a,
                            ActivityId = b,
                            Date = z.Date,
                            Status = TimeSheetStatus.Submitted,
                            EmployeeRemarks = employeeRemarks,
                            SubmittedTo = ManagerID,
                            SubmittedDate = DateTime.UtcNow
                        });
                        timesheetDetailsList.AddRange(recd.ToList());
                    }
                }

                result = _UOW.TimesheetDetailRepository.SubmitTimesheet(timesheetDetailsList);
                string subject = "Timesheet Submitted";
                string body = "Dear " + String.Concat(employeeDetails.FirstName, " ", employeeDetails.LastName) +
                    "\n\nYour Timesheet has been submitted successfully to Supervisor " + String.Concat(managerDetails.FirstName, " ", managerDetails.LastName) + ".\n\n with remarks as below : \n\n" + timesheetDetails[0].EmployeeRemarks;
                mailSent = SendMail(managerDetails.EmailAddress, subject, body);
                if (result && mailSent)
                {
                    response.Data = result;
                    response.ResponseCode = HTTPConstants.OK;
                    response.ResponseMessage = MessageConstants.TimesheetSubmissionSuccess;
                }
                else if (result && !mailSent)
                {
                    response.Data = result;
                    response.ResponseCode = HTTPConstants.OK;
                    response.ResponseMessage = MessageConstants.TimesheetSubmissionSuccessMailFailure;
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
        /// <summary>
        /// Listing records of timesheet that are submitted along with their status for supervisor
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetReviewTimesheet([FromQuery] PaginationFilter filter, string EmployeeID,string searchValue)
        {
            List<TimesheetViewModel> result = new List<TimesheetViewModel>();
            try
            {
                var data = _UOW.TimesheetDetailRepository.GetAllByCondition(x => x.SubmittedTo==EmployeeID).Include(x => x.Project).ToList();
                if (!string.IsNullOrEmpty(searchValue))
                {   
                    data = _UOW.TimesheetDetailRepository.GetAllByCondition(x => x.SubmittedTo==EmployeeID).Include(x => x.Project).Where(x=>x.Project.ProjectName.Contains(searchValue)).ToList();
                }
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

        /// <summary>
        /// Listing records of timesheet that can be Approved/Rejected by supervisor
        /// </summary>
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
        /// <summary>
        /// Changes the status of the Timesheet based on Supervisor Decision and generates email
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> SupervisorDecision( List<Details> model,string SupervisorID, string EmployeeID, string Action)
        {
            try
            {
                bool result = false;
                List<TimesheetDetails> timesheetDetailsList = new List<TimesheetDetails>();
                
                var data = model.ToList();
                string supervisorRemarks = data.Select(x => x.SupervisorRemarks).FirstOrDefault();
                foreach (var b in data.Select(y => y.ActivityId).Distinct())
                {
                    var recd = data.Where(y => y.ActivityId == b).SelectMany(y => y.TimeTaken).Select(z => new TimesheetDetails
                    {
                        UniqueId = z.UniqueId,
                        ProjectId = data.Select(x=>x.ProjectId).FirstOrDefault(),
                        ActivityId = b,
                        Date = z.Date,
                        ApprovedRejectedBy=SupervisorID,
                        SupervisorRemarks = supervisorRemarks
                    });
                    timesheetDetailsList.AddRange(recd.ToList());
                }
                var managerDetails = _UOW.EmployeeDetailsRepository.GetById(SupervisorID);
                var employeeDetails = _UOW.EmployeeDetailsRepository.GetById(EmployeeID);
                if (Action == TimeSheetStatus.Approved)
                {
                    string subject = "Timesheet Approved";
                    string body = "Dear " + String.Concat(employeeDetails.FirstName, " ", employeeDetails.LastName) +
                        "\n\nYour Timesheet has been approved by Supervisor " + String.Concat(managerDetails.FirstName, " ", managerDetails.LastName) + " with remarks as below : \n\n" + model[0].SupervisorRemarks;
                    SendMail(employeeDetails.EmailAddress, subject, body);
                    result = _UOW.TimesheetDetailRepository.SupervisorAction(timesheetDetailsList, TimeSheetStatus.Approved);
                }
                else if (Action == TimeSheetStatus.Rejected)
                {
                    string subject = "Timesheet Rejected";
                    string body = "Dear " + String.Concat(employeeDetails.FirstName, " ", employeeDetails.LastName) +
                        "\n\nYour Timesheet has been rejected by Supervisor " + String.Concat(managerDetails.FirstName, " ", managerDetails.LastName) + " with remarks as below : \n\n" + model[0].SupervisorRemarks;
                    SendMail(employeeDetails.EmailAddress, subject, body);
                    result = _UOW.TimesheetDetailRepository.SupervisorAction(timesheetDetailsList, TimeSheetStatus.Rejected);
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

        /// <summary>
        /// Generates Timesheet Report
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> GetTimesheetReport(ReportRequest model)
        {
            Dictionary<string, string[]> parameters = new Dictionary<string, string[]>();
            parameters.Add("EMPLOYEE_ID_LIST",    new string[] { string.Join(",", model.EmployeeId), "nvarchar" });   
            parameters.Add("PROJECT_ID",    new string[] { string.Join(",", model.ProjectIds), "nvarchar" });
            parameters.Add("FROM_DATE",   new string[] { model.FromDate, "date" });
            parameters.Add("TO_DATE",    new string[] { model.ToDate, "date" });
            List<SPViewModel> countList = _UOW.ExcecuteSP("USP_GET_TIMESHEET_REPORT", parameters);
            var queryable = countList.ToList();
          
            List<ReportViewModel> reports = new List<ReportViewModel>();
            List<ReportDayWiseTotal> days = new List<ReportDayWiseTotal>();
            foreach (var a in queryable.Select(x => x.ProjectId).Distinct())
            {

                var recd = queryable.Where(X => X.ProjectId == a).Select(X => new ReportViewModel
                {
                    ProjectName = X.ProjectName,
                    EmployeeName = X.EmployeeName,
                    TimeSpent = ( queryable.Where(y => y.ProjectId == a && y.EmployeeId== X.EmployeeId)
                         .Select(z=> new TimeSpentperDay {  Date=z.Date, NumberOfHours=z.NumberOfHours}).ToList()
                               )

                }).GroupBy(x=>x.EmployeeName).Select(y=>y.First()).ToList();
                reports.AddRange(recd);

                
            }
            //Populates hours spent on each day
            var dateRange = reports.SelectMany(X => X.TimeSpent).ToList();

            var abc= (from dr in dateRange
                     group dr by dr.Date into g

                     select new ReportDayWiseTotal
                     {
                          Date = g.Key.Date,
                          NumberOfHours = g.Sum(x => x.NumberOfHours),
                        
                     }).ToList();

            ProjectLevelReport projectLevelReport = new ProjectLevelReport();
            projectLevelReport.reportViewModels= reports;
            projectLevelReport.reportDayWiseTotals=abc;
            return Ok(projectLevelReport);
        }

        [NonAction]
        public bool SendMail(string mailto, string subject, string body)
        {
            bool f = false;
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(mailto);
                mailMessage.From = new MailAddress(_configuration["Smtp:Sender"]);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient()
                {
                    Host = _configuration["Smtp:Server"],
                    Port = Convert.ToInt32(_configuration["Smtp:Port"]),
                    EnableSsl = true,
                    Credentials = new System.Net.NetworkCredential(_configuration["Smtp:Sender"], _configuration["Smtp:Password"])

                };
                smtp.Send(mailMessage);
                f = true;
            }
            catch (Exception ex)
            {
                f = false;
            }
            return f;
        }
        

            [NonAction]
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
