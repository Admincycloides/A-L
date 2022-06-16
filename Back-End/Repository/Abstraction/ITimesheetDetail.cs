using System.Collections.Generic;
using System.Threading.Tasks;
using AnL.Constants;
using AnL.Models;
using AnL.ViewModel;

namespace AnL.Repository.Abstraction
{
    public interface ITimesheetDetail : IRepository<TimesheetDetails>
    {
        public bool ModifyTimesheetDetails(List<TimesheetDetails> inventoryDetailsList, string userid);

        public List<TimesheetDetails> GetTimesheetDetails(TimesheetViewModel timesheetDetails);
        public bool AddDetails(List<TimesheetDetails> timesheetDetails, string userid);
        public bool SubmitTimesheet(List<TimesheetDetails> items, string userid);
        public bool DeleteTimesheetDetails(List<TimesheetDetails> timesheetDetails, string userid);
        public bool GetTimesheetDetailsForProject(int ProjectId);
        public bool GetTimesheetDetailsForActivity(int Activityid);
        public Task<List<TimesheetViewModel>> GetReview(string EmployeeID);
        public bool SupervisorAction(List<TimesheetDetails> timesheetDetails, string Action,string userid);
    }
}
