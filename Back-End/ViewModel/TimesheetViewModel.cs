using System;
using System.Collections.Generic;

namespace AnL.ViewModel
{

    public class TimesheetViewModel
    {
        public DateTime Date { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public double NumberOfHours { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int UniqueId { get; set; }
    }
    public class TimeSpent
    {
        public DateTime Date { get; set; }
        public double NumberOfHours { get; set; }
        public int UniqueId { get; set; }
    }
    public class Details
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string EmployeeRemarks { get; set; }
        public string SupervisorRemarks { get; set; }
        public List<TimeSpent> TimeTaken { get; set; }
    }
    public class MasterTimesheetViewModel
    {
        public string ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string TimesheetStatus { get; set; }
        public List<Details> TimesheetDetails { get; set; }
    }

}
