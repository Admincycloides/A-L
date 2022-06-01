using AnL.Repository.Abstraction;
using AnL.Models;

namespace AnL.Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Tan_DBContext _dbcontext;

        public UnitOfWork(Tan_DBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        private ITimesheetDetail _TimesheetDetailRepository;
        public ITimesheetDetail TimesheetDetailRepository
        {
            get
            {
                if (_TimesheetDetailRepository == null)
                {
                    _TimesheetDetailRepository = new TimesheetDetailRepository(_dbcontext);
                }

                return _TimesheetDetailRepository;
            }
        }
        public void SaveChanges()
        {
            this._dbcontext.SaveChanges();
        }
    }
}
