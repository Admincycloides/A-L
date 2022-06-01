using AnL.Models;
using AnL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.Repository.Implementation
{
    public class UserRepository : Repository<UserLogin>, IUser
    {
        private Tan_DBContext _context;
        DbSet<UserLogin> dbSet;
        public UserRepository(Tan_DBContext context) : base(context)
        {
            _context = context;
            dbSet = context.Set<UserLogin>();
        }

        public async Task<UserLogin> GetLogin(string username)
        {
            return await Task.Run(() =>
            {
            var details = dbSet.Where(x => x.Username == username).FirstOrDefault();
            return details;
            });

        }
        public async Task<bool> UpdateOTP(int Otp,int Userid)
        {
            return await Task.Run(() =>
            {
                var details = getLogindetails(Userid);
                details.Otp = Otp;
                details.OtpexpiryDate = DateTime.UtcNow;
                _context.SaveChanges();
                return true;
            });
            
        }
        public UserLogin getLogindetails(int UserID)
        {
            return(dbSet.Where(x => x.UserId == UserID).FirstOrDefault());
        }

    }
}
