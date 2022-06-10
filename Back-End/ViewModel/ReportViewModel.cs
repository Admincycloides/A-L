using System;
using System.Collections.Generic;

namespace AnL.ViewModel
{
    public class TimeSpentperDay
    {
        public DateTime Date { get; set; }
        public double NumberOfHours { get; set; }
        public int UniqueId { get; set; }
    }
    public class ReportViewModel
    {
        public string ProjectName { get; set; }
        public string EmployeeName { get; set; }
        public List<TimeSpentperDay> TimeSpent { get; set; }

    }
    public class ReportRequest
    {
        public List<int> ProjectIds { get; set; }
        public List<string> EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double NumberOfHours { get; set; }
    }
}
