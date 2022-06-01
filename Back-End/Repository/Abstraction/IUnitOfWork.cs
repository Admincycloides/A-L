namespace AnL.Repository.Abstraction
{
    public interface IUnitOfWork
    {
        ITimesheetDetail TimesheetDetailRepository { get; }
        void SaveChanges();
    }
}
