namespace AnL.Repository.Abstraction
{
    public interface IUnitOfWork
    {
        ITimesheetDetail TimesheetDetailRepository { get; }
        IUser UserRepository { get; }

        IEmployeeDetail EmployeeDetailRepository { get; }
        void SaveChanges();
    }
}
