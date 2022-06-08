namespace AnL.Repository.Abstraction
{
    public interface IUnitOfWork
    {
        ITimesheetDetail TimesheetDetailRepository { get; }
        IUser UserRepository { get; }
        IEmployeeDetails EmployeeDetailsRepository { get; }
        IProject ProjectRepository { get; }
        void SaveChanges();
    }
}
