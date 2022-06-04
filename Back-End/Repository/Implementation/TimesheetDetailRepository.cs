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
            this._UOW= UOW;
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
            if (empDetails[0].SupervisorFlag=="Y" && empDetails.Count==1 )
            {
                subEmpDetails = _UOW.EmployeeDetailsRepository.GetAllByCondition(x => x.SupervisorFlag == "N").Select(x=>x.EmployeeId).ToList();
                foreach (var emp in subEmpDetails)
                {
                    var result = this.GetAllByCondition(x => x.EmployeeId == timesheetDetails.EmployeeId && x.Date >= DateTime.Parse( timesheetDetails.FromDate) && x.Date <= DateTime.Parse(timesheetDetails.ToDate)).ToList();
                    foreach(var data in result)
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
            if(sheetDetails.Count>0)
                return sheetDetails;
            else
                return null;
        }

        public bool ModifyTimesheetDetails(List<TimesheetViewModel> timesheetDetails)
        {
            //foreach (var timesheet in timesheetDetails)
            //{
            //    TimesheetDetails existingSheet = _UOW.TimesheetDetailRepository.GetById(timesheet.UniqueId);
            //    existingSheet.ProjectId = timesheet.ProjectId;
            //    existingSheet.ActivityId = timesheet.ActivityId;
            //    existingSheet.NumberOfHours = timesheet.NumberOfHours;
            //    existingSheet.Remarks = timesheet.Remarks;
            //}
            this.SaveChanges();
            return true;
        }

        public bool AddDetails(List<TimesheetViewModel> timesheetDetails)
        {
            foreach (var timesheet in timesheetDetails)
            {
                TimesheetDetails data = new TimesheetDetails();
                data.Date = DateTime.UtcNow;
                data.EmployeeId = timesheet.EmployeeId;
                data.ProjectId = timesheet.ProjectId;
                data.ActivityId = timesheet.ActivityId;
                data.NumberOfHours = timesheet.NumberOfHours;
                if (String.IsNullOrEmpty(timesheet.Remarks))
                {
                    data.Remarks = "None";
                }
                else
                {
                    data.Remarks = timesheet.Remarks;
                }
                data.Status = TimeSheetStatus.InProgress;
                this.Add(data);
            }
            this.SaveChanges();
            return true;
        }

        public bool DeleteTimesheetDetails(List<TimesheetDetails> timesheetDetails)
        {
            foreach(var details in timesheetDetails)
            {
                this.Delete(details);
                _context.SaveChanges();
            }
            return true;
        }
    }
}
