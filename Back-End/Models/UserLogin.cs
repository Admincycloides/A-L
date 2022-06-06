using System;
using System.Collections.Generic;

namespace AnL.Models
{
    public partial class UserLogin
    {
        public string Username { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public int? Otp { get; set; }
        public DateTime TokenExpiryDate { get; set; }
        public DateTime OtpexpiryDate { get; set; }
        public bool IsActive { get; set; }

        public virtual EmployeeDetails User { get; set; }
    }
}
