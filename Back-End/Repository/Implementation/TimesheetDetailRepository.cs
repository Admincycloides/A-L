using AnL.Models;
using AnL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AnL.Repository.Implementation
{
    public class TimesheetDetailRepository : Repository<TimesheetDetails>, ITimesheetDetail
    {
        private DbContext _context;
        DbSet<TimesheetDetails> dbSet;
        public TimesheetDetailRepository(DbContext context) : base(context)
        {
            _context = context;
            dbSet = context.Set<TimesheetDetails>();
        }


        //public override IQueryable<TimesheetDetails> GetAllByCondition(Expression<Func<TimesheetDetails, bool>> expression)
        //{
        //    return dbSet.Where(expression);
        //}
        public List<TimesheetDetails> GetTimesheetDetails(TimesheetDetails timesheetDetails)
        {

            var details = dbSet.Where(x => x.EmployeeId == timesheetDetails.EmployeeId).ToList();
            // var InvDetails = _mapper.Map<InventoryItemDetailsViewModel>(details);

            if (details != null)
                return details;
            else
                return null;
                
        }

        public void ModifyTimesheetDetails(List<TimesheetDetails> inventoryDetailsList)
        {
            throw new NotImplementedException();
        }
    }
}
