using System;
using System.Collections.Generic;

namespace AnL.Models
{
    public partial class ProjectMapping
    {
        public int ProjectMappingId { get; set; }
        public int ProjectId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool Active { get; set; }
        public string LastUpdatedBy { get; set; }

        public virtual EmployeeDetails Employee { get; set; }
        public virtual EmployeeDetails LastUpdatedByNavigation { get; set; }
        public virtual ProjectDetails Project { get; set; }
    }
}
