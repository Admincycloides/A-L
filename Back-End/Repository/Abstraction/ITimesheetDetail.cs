using System.Collections.Generic;
using System.Threading.Tasks;
using AnL.Models;
using AnL.ViewModel;

namespace AnL.Repository.Abstraction
{
    public interface ITimesheetDetail : IRepository<TimesheetDetails>
    {
        public bool ModifyTimesheetDetails(List<TimesheetViewModel> inventoryDetailsList);

        public List<TimesheetDetails> GetTimesheetDetails(TimesheetViewModel timesheetDetails);
        public bool AddDetails(List<TimesheetViewModel> timesheetDetails);
        public bool DeleteTimesheetDetails(List<TimesheetDetails> timesheetDetails);
    }
}
