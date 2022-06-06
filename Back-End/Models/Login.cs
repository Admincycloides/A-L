using System;
using System.Collections.Generic;

namespace AnL.Models
{
    public partial class Login
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public Guid? UserId { get; set; }
        public string Token { get; set; }
        public string Otp { get; set; }
        public DateTime? OtpexpiryTime { get; set; }
        public DateTime? TokenExpiryTime { get; set; }
        public bool? Isactive { get; set; }
    }
}
