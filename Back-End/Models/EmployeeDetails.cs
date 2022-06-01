using System;
using System.Collections.Generic;

namespace AnL.Models
{
    public partial class EmployeeDetails
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string SupervisorFlag { get; set; }
        public string ManagerId { get; set; }
        public string EnabledFlag { get; set; }
    }
}
