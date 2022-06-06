using AnL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.Repository.Abstraction
{
    public interface IUser : IRepository<UserLogin>
    {
        Task<UserLogin> GetLogin(string username);
        Task<UserLogin> UpdateOTP(int Otp,string UserID);
        UserLogin getLogindetails(string UserID);
    }
}
