using AnL.Models;
using AnL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

        public List<EmployeeDetails> getEmployeeDetails(List<string> EmployeeID)
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
    }
}
