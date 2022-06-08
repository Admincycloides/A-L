using AnL.Constants;
using AnL.Models;
using AnL.Repository.Abstraction;
using AnL.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AnL.Repository.Implementation
{
    public class TimesheetDetailRepository : Repository<TimesheetDetails>, ITimesheetDetail
    {
        private DbContext _context;
        private IUnitOfWork _UOW;

        DbSet<TimesheetDetails> dbSet;
        public TimesheetDetailRepository(DbContext context, IUnitOfWork UOW) : base(context)
        {
            this._UOW = UOW;
            this._context = context;
            dbSet = context.Set<TimesheetDetails>();
        }


        public List<TimesheetDetails> GetTimesheetDetails(TimesheetViewModel timesheetDetails)
        {
            List<string> employeeIDs = new List<string>();
            employeeIDs.Add(timesheetDetails.EmployeeId);
            var empDetails = _UOW.EmployeeDetailsRepository.GetEmployeeDetails(employeeIDs);
            List<string> subEmpDetails = new List<string>();
            List<TimesheetDetails> sheetDetails = new List<TimesheetDetails>();
            if (empDetails[0].SupervisorFlag == "Y" && empDetails.Count == 1)
            {
                subEmpDetails = _UOW.EmployeeDetailsRepository.GetAllByCondition(x => x.SupervisorFlag == "N").Select(x => x.EmployeeId).ToList();
                foreach (var emp in subEmpDetails)
                {
                    var result = this.GetAllByCondition(x => x.EmployeeId == timesheetDetails.EmployeeId && x.Date >= DateTime.Parse(timesheetDetails.FromDate) && x.Date <= DateTime.Parse(timesheetDetails.ToDate)).ToList();
                    foreach (var data in result)
                    {
                        sheetDetails.Add(data);
                    }
                }
            }
            else
            {
                sheetDetails.Add(this.GetById(empDetails[0]));
            }
            //if (subEmpDetails != null || subEmpDetails.Count() > 0)
            if (sheetDetails.Count > 0)
                return sheetDetails;
            else
                return null;
        }

        public bool ModifyTimesheetDetails(List<TimesheetDetails> timesheetDetails)
        {
            foreach (var timesheet in timesheetDetails)
            {
                TimesheetDetails existingSheet = this.GetById(timesheet.UniqueId);
                existingSheet.ProjectId = timesheet.ProjectId;
                existingSheet.ActivityId = timesheet.ActivityId;
                existingSheet.NumberOfHours = timesheet.NumberOfHours;
                existingSheet.Remarks = timesheet.Remarks;
            }
            this.SaveChanges();
            return true;
        }

        public bool AddDetails(List<TimesheetDetails> timesheetdetails)
        {
            foreach (var timesheet in timesheetdetails)
            {
                this.Add(timesheet);
            }
            this.SaveChanges();
            return true;
        }

        public bool DeleteTimesheetDetails(List<TimesheetDetails> timesheetDetails)
        {
            foreach (var details in timesheetDetails)
            {
                this.Delete(details);
                _context.SaveChanges();
            }
            return true;
        }

        public bool SubmitTimesheet(List<TimesheetDetails> timesheetDetails)
        {
            foreach (var timesheet in timesheetDetails)
            {
                TimesheetDetails existingSheet = this.GetById(timesheet.UniqueId);
                existingSheet.Status = TimeSheetStatus.Submitted;
                existingSheet.EmployeeRemarks = timesheet.EmployeeRemarks;
                existingSheet.SubmittedDate = timesheet.SubmittedDate;
                existingSheet.SubmittedTo = timesheet.SubmittedTo;
            }
            this.SaveChanges();
            return true;
        }

        public async Task<List<TimesheetViewModel>> GetReview(string EmployeeID)
        {
            List<TimesheetViewModel> result = new List<TimesheetViewModel>();
            await Task.Run(() =>
            {
                var data = this.GetAllByCondition(x => x.SubmittedTo.Contains(EmployeeID)).Include(x => x.Project).ToList();
            var employeeList = data.Select(x => new
            {
                x.Project.ProjectName,
                x.ProjectId,
                x.EmployeeId,
                x.SubmittedDate.Value.Date,
                x.Status,
                EmployeeName = String.Concat(_UOW.EmployeeDetailsRepository.GetById(EmployeeID).FirstName, " ", _UOW.EmployeeDetailsRepository.GetById(EmployeeID).LastName)
            }).Distinct().ToList();
            foreach(var employee in employeeList)
            {
                TimesheetViewModel timesheetViewModel = new TimesheetViewModel();
                timesheetViewModel.ProjectName = employee.ProjectName;
                timesheetViewModel.ProjectId = employee.ProjectId;
                timesheetViewModel.EmployeeId = employee.EmployeeId;
                timesheetViewModel.EmployeeName= employee.EmployeeName;
                timesheetViewModel.Date= employee.Date;
                timesheetViewModel.Status= employee.Status;
                result.Add(timesheetViewModel);
            }
            });
            return result;
        }

        public bool SupervisorAction(List<TimesheetDetails> timesheetDetails,string Action)
        {
            if (Action == TimeSheetStatus.Approved)
            {
                foreach (var timesheet in timesheetDetails)
                {
                    TimesheetDetails existingSheet = this.GetById(timesheet.UniqueId);
                    existingSheet.Status = TimeSheetStatus.Approved;
                    existingSheet.EmployeeRemarks = timesheet.EmployeeRemarks;
                    existingSheet.SubmittedDate = timesheet.SubmittedDate;
                    existingSheet.SubmittedTo = timesheet.SubmittedTo;
                }
            }
            else
            {
                foreach (var timesheet in timesheetDetails)
                {
                    TimesheetDetails existingSheet = this.GetById(timesheet.UniqueId);
                    existingSheet.Status = TimeSheetStatus.Rejected;
                    existingSheet.EmployeeRemarks = timesheet.EmployeeRemarks;
                    existingSheet.SubmittedDate = timesheet.SubmittedDate;
                    existingSheet.SubmittedTo = timesheet.SubmittedTo;
                }
            }
            this.SaveChanges();
            return true;
        }
        public bool GetTimesheetDetailsForProject(int ProjectId)
        {
            List<TimesheetDetails> details = this.GetAllByCondition(x => x.ProjectId == ProjectId).ToList();
            if(details==null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
