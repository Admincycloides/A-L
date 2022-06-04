namespace AnL.Repository.Abstraction
{
    public interface IUnitOfWork
    {
        ITimesheetDetail TimesheetDetailRepository { get; }
        IUser UserRepository { get; }
        IEmployeeDetails EmployeeDetailsRepository { get; }
        void SaveChanges();
    }
}
