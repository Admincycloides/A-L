using AnL.Models;
using AnL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.Repository.Implementation
{
    public class EmployeeDetailRepository : Repository<EmployeeDetails>, IEmployeeDetail
    {
        private Tan_DBContext _context;
        DbSet<EmployeeDetails> dbSet;
        public EmployeeDetailRepository(Tan_DBContext context) : base(context)
        {
            _context = context;
            dbSet = context.Set<EmployeeDetails>();
        }

        public override EmployeeDetails GetById(object Id)
        {
            //Guid ASNId = (Guid)Id;
            EmployeeDetails temp = dbSet.Find(Id);
            return temp;
        }

    }
}
