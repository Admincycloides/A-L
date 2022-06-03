using AnL.Repository.Abstraction;
using AnL.Models;

namespace AnL.Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Tan_DBContext _dbcontext;
        private readonly IUnitOfWork _UOW;

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
                    _TimesheetDetailRepository = new TimesheetDetailRepository(_dbcontext, _UOW);
                }

                return _TimesheetDetailRepository;
            }
        }
        private IEmployeeDetails _EmployeeDetailsRepository;
        public IEmployeeDetails EmployeeDetailsRepository
        {
            get
            {
                if (_EmployeeDetailsRepository == null)
                {
                    _EmployeeDetailsRepository = new EmployeeDetailsRepository(_dbcontext, _UOW);
                }

                return _EmployeeDetailsRepository;
            }
        }
        public void SaveChanges()
        {
            this._dbcontext.SaveChanges();
        }
    }
}
