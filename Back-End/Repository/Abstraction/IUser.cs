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
        Task<bool> UpdateOTP(int Otp,int UserID);
        UserLogin getLogindetails(int UserID);
    }
}
