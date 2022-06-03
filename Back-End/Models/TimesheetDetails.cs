using System;
using System.Collections.Generic;

namespace AnL.Models
{
    public partial class TimesheetDetails
    {
        public DateTime Date { get; set; }
        public string EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public int ActivityId { get; set; }
        public double NumberOfHours { get; set; }
        public string Remarks { get; set; }
        public int UniqueId { get; set; }
        public string Status { get; set; }

        public virtual ActivityDetails Activity { get; set; }
        public virtual ProjectDetails Project { get; set; }
    }
}
