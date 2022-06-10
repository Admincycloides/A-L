using AnL.Models;
using AnL.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnL.Repository.Abstraction
{
    public interface IEmployeeDetails : IRepository<EmployeeDetails>
    {
        public List<EmployeeDetails> GetEmployeeDetails(List<string> EmployeeID);
        EmployeeDetails GetById(object Id);
        public List<EmployeeDetails> TestGetEmployeeDetails(List<string> EmployeeID);
        public Task<List<EmployeeListViewModel>> GetAllEmployee();
    }
}
