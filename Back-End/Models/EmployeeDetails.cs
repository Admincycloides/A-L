using System;
using System.Collections.Generic;

namespace AnL.Models
{
    public partial class EmployeeDetails
    {
        public EmployeeDetails()
        {
            ProjectMappingEmployee = new HashSet<ProjectMapping>();
            ProjectMappingLastUpdatedByNavigation = new HashSet<ProjectMapping>();
        }

        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string SupervisorFlag { get; set; }
        public string ManagerId { get; set; }
        public string EnabledFlag { get; set; }

        public virtual ICollection<ProjectMapping> ProjectMappingEmployee { get; set; }
        public virtual ICollection<ProjectMapping> ProjectMappingLastUpdatedByNavigation { get; set; }
    }
}
