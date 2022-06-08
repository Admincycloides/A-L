using AnL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.Repository.Abstraction
{
    public interface IUser : IRepository<UserLogin>
    {
        public Task<UserLogin> GetLogin(string username);
        public Task<UserLogin> UpdateOTP(int Otp,string UserID);
        public UserLogin getLogindetails(string UserID);
    }
}
