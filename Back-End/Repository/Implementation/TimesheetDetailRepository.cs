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
            var querable = timesheetDetails.ToList();
            List<int> iDs = querable.Select(x => x.UniqueId).ToList();
            var result = dbSet.Where(e => iDs.Contains(e.UniqueId)).ToList();
            result.ForEach(x =>
            {
                x.Status = TimeSheetStatus.Submitted;
                x.EmployeeRemarks = timesheetDetails[0].EmployeeRemarks;
                x.SubmittedDate = timesheetDetails[0].SubmittedDate;
                x.SubmittedTo = timesheetDetails[0].SubmittedTo;
            });
            _context.UpdateRange(result);
            //data.ForEach(u =>
            //{
            //    u.Status = TimeSheetStatus.Submitted;
            //    u.EmployeeRemarks = timesheetDetails[0].EmployeeRemarks;
            //    u.SubmittedDate= timesheetDetails[0].SubmittedDate;
            //    u.SubmittedTo = timesheetDetails[0].SubmittedTo;
            //});
            //foreach (var timesheet in timesheetDetails)
            //{
            //    TimesheetDetails existingSheet = this.GetById(timesheet.UniqueId);
            //    existingSheet.Status = TimeSheetStatus.Submitted;
            //    existingSheet.EmployeeRemarks = timesheet.EmployeeRemarks;
            //    existingSheet.SubmittedDate = timesheet.SubmittedDate;
            //    existingSheet.SubmittedTo = timesheet.SubmittedTo;
            //}
            _context.SaveChanges();
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
                var querable = timesheetDetails.ToList();
                List<int> iDs = querable.Select(x => x.UniqueId).ToList();
                var result = dbSet.Where(e => iDs.Contains(e.UniqueId)).ToList();
                result.ForEach(x =>
                {
                    x.Status = TimeSheetStatus.Approved;
                    x.SupervisorRemarks = timesheetDetails[0].SupervisorRemarks;
                    x.ApprovedRejectedBy = timesheetDetails[0].ApprovedRejectedBy;
                });
                _context.UpdateRange(result);
                //Previous Logic
                //foreach (var timesheet in timesheetDetails)
                //{
                //    TimesheetDetails existingSheet = this.GetById(timesheet.UniqueId);
                //    existingSheet.Status = TimeSheetStatus.Approved;
                //    existingSheet.SupervisorRemarks = timesheet.SupervisorRemarks;
                //    existingSheet.ApprovedRejectedBy = timesheet.ApprovedRejectedBy;
                //}
            }
            else
            {
                var querable = timesheetDetails.ToList();
                List<int> iDs = querable.Select(x => x.UniqueId).ToList();
                var result = dbSet.Where(e => iDs.Contains(e.UniqueId)).ToList();
                result.ForEach(x =>
                {
                    x.Status = TimeSheetStatus.Rejected;
                    x.SupervisorRemarks = timesheetDetails[0].SupervisorRemarks;
                    x.ApprovedRejectedBy = timesheetDetails[0].ApprovedRejectedBy;
                });
                _context.UpdateRange(result);
                //Previous Logic
                //foreach (var timesheet in timesheetDetails)
                //{
                //    TimesheetDetails existingSheet = this.GetById(timesheet.UniqueId);
                //    existingSheet.Status = TimeSheetStatus.Rejected;
                //    existingSheet.SupervisorRemarks = timesheet.SupervisorRemarks;
                //    existingSheet.ApprovedRejectedBy = timesheet.ApprovedRejectedBy;
                //}
            }
            this.SaveChanges();
            return true;
        }
        public bool GetTimesheetDetailsForProject(int ProjectId)
        {
            List<TimesheetDetails> details = this.GetAllByCondition(x => x.ProjectId == ProjectId).ToList();
            if(details.Count==0)
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
