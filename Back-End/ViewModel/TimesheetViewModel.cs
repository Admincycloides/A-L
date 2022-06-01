using System;

namespace AnL.ViewModel
{
    public class TimesheetViewModel
    {
        public DateTime Date { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int ProjectId { get; set; }
        public int ActivityId { get; set; }
        public int ActivityName { get; set; }
        public double NumberOfHours { get; set; }
        public string Remarks { get; set; }
        public int UniqueId { get; set; }
    }
}
