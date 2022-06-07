using System.Collections.Generic;
using System.Threading.Tasks;
using AnL.Constants;
using AnL.Models;
using AnL.ViewModel;

namespace AnL.Repository.Abstraction
{
    public interface ITimesheetDetail : IRepository<TimesheetDetails>
    {
        public bool ModifyTimesheetDetails(List<TimesheetDetails> inventoryDetailsList);

        public List<TimesheetDetails> GetTimesheetDetails(TimesheetViewModel timesheetDetails);
        public bool AddDetails(List<TimesheetDetails> timesheetDetails);
        public bool SubmitTimesheet(List<TimesheetDetails> items);
        public bool DeleteTimesheetDetails(List<TimesheetDetails> timesheetDetails);
        public Task<List<TimesheetViewModel>> GetReview(string EmployeeID);
        public bool SupervisorAction(List<TimesheetDetails> timesheetDetails, string Action);
    }
}
