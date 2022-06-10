using AnL.Models;
using AnL.Repository.Abstraction;
using AnL.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.Repository.Implementation
{
    public class EmployeeDetailsRepository : Repository<EmployeeDetails>, IEmployeeDetails
    {
        private DbContext _context;
        private readonly IUnitOfWork _UOW;

        DbSet<EmployeeDetails> dbSet;
        public EmployeeDetailsRepository(DbContext context, IUnitOfWork UOW) : base(context)
        {
            this._context = context;
            this._UOW = UOW;
            dbSet = context.Set<EmployeeDetails>();
        }

        public List<EmployeeDetails> GetEmployeeDetails(List<string> EmployeeID)
        {
            List<EmployeeDetails> empDetails=new List<EmployeeDetails>();
            foreach(string empID in EmployeeID)
            {
                empDetails.Add(this.GetById(empID));
            }
            if (empDetails != null || empDetails.Count > 0 )
            {
                return empDetails;
            }
            else
            {
                return null;
            }
        }
        //Single Employee maps to Multiple Managers get function
        public List<EmployeeDetails> TestGetEmployeeDetails(List<string> EmployeeID)
        {
            List<EmployeeDetails> empDetails = new List<EmployeeDetails>();
            string[] subs = this.GetById(EmployeeID[0]).ManagerId.Split(',');
            foreach (string empID in subs)
            {
                empDetails.Add(this.GetById(empID));
            }
            if (empDetails != null || empDetails.Count > 0)
            {
                return empDetails;
            }
            else
            {
                return null;
            }
        }
        public override EmployeeDetails GetById(object Id)
        {
            //Guid ASNId = (Guid)Id;
            EmployeeDetails temp = dbSet.Find(Id);
            return temp;
        }
        public async Task<List<EmployeeListViewModel>> GetAllEmployee()
        {
            List<EmployeeListViewModel> rsp = new List<EmployeeListViewModel>();
            try
            {
                await Task.Run(() =>
                {
                    rsp = (_context.Set<EmployeeDetails>().Select(

                       X => new EmployeeListViewModel
                       {
                           EmployeeId=X.EmployeeId,
                           EmployeeName=String.Concat(X.FirstName," ", X.LastName)
                       }
                       )).ToList();
                });
                return rsp.Count > 0 ? rsp : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
