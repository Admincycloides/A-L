using System;
using System.Collections.Generic;

namespace AnL.ViewModel
{
    public class TimeSpentperDay
    {
        public DateTime Date { get; set; }
        public double NumberOfHours { get; set; }
    }
    public class ReportViewModel
    {
        public string ProjectName { get; set; }
        public string EmployeeName { get; set; }
        public List<TimeSpentperDay> TimeSpent { get; set; } = new List<TimeSpentperDay>();

    }
    public class ReportRequest
    {
        public List<int> ProjectIds { get; set; }
        public List<string> EmployeeId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public double NumberOfHours { get; set; }
    }

    public class SPViewModel
    {
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public double NumberOfHours { get; set; }
    }
}
