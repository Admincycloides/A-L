using System.Collections.Generic;
using AnL.Models;

namespace AnL.Repository.Abstraction
{
    public interface ITimesheetDetail : IRepository<TimesheetDetails>
    {
        public void ModifyTimesheetDetails(List<TimesheetDetails> inventoryDetailsList);

        public List<TimesheetDetails> GetTimesheetDetails(TimesheetDetails timesheetDetails);
    }
}
