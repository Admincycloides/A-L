namespace AnL.Repository.Abstraction
{
    public interface IUnitOfWork
    {
        ITimesheetDetail TimesheetDetailRepository { get; }
        IEmployeeDetails EmployeeDetailsRepository { get; }
        void SaveChanges();
    }
}
