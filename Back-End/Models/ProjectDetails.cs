using System;
using System.Collections.Generic;

namespace TestApplication.Models
{
    public partial class ProjectDetails
    {
        public ProjectDetails()
        {
            ActivityMapping = new HashSet<ActivityMapping>();
            TimesheetDetails = new HashSet<TimesheetDetails>();
        }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public int ClientId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CurrentStatus { get; set; }
        public string SredProject { get; set; }
        public string EnabledFlag { get; set; }

        public virtual ClientDetails Client { get; set; }
        public virtual ICollection<ActivityMapping> ActivityMapping { get; set; }
        public virtual ICollection<TimesheetDetails> TimesheetDetails { get; set; }
    }
}
