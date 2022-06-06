using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.ViewModel
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public int? Otp { get; set; }
        public DateTime TokenExpiryDate { get; set; }
        public DateTime OtpexpiryDate { get; set; }
        public bool IsActive { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SupervisorFlag { get; set; }
        public string ManagerId { get; set; }
    }
}
