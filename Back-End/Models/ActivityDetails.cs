using System;
using System.Collections.Generic;

namespace TestApplication.Models
{
    public partial class ActivityDetails
    {
        public ActivityDetails()
        {
            ActivityMapping = new HashSet<ActivityMapping>();
            TimesheetDetails = new HashSet<TimesheetDetails>();
        }

        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string ActivityDescription { get; set; }
        public string EnabledFlag { get; set; }

        public virtual ICollection<ActivityMapping> ActivityMapping { get; set; }
        public virtual ICollection<TimesheetDetails> TimesheetDetails { get; set; }
    }
}
