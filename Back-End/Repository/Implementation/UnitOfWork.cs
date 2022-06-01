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
        private IUser _UserRepository;
        private IEmployeeDetail _EmployeeDetailRepository;
        public IEmployeeDetail EmployeeDetailRepository
        {
            get
            {
                if (_EmployeeDetailRepository == null)
                {
                    _EmployeeDetailRepository = new EmployeeDetailRepository(_dbcontext);
                }

                return _EmployeeDetailRepository;
            }
        }
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
        public IUser UserRepository
        {
            get
            {
                if (_UserRepository == null)
                {
                    _UserRepository = new UserRepository(_dbcontext);
                }

                return _UserRepository;
            }
        }
        public void SaveChanges()
        {
            this._dbcontext.SaveChanges();
        }
    }
}
