using AnL.ViewModel;
using System.Collections.Generic;

namespace AnL.Repository.Abstraction
{
    public interface IUnitOfWork
    {
        ITimesheetDetail TimesheetDetailRepository { get; }
        IUser UserRepository { get; }
        IEmployeeDetails EmployeeDetailsRepository { get; }
        IProject ProjectRepository { get; }
        public List<SPViewModel> ExcecuteSP(string procedureName, Dictionary<string, string[]> parameters);
        void SaveChanges();
    }
}
