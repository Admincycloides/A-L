using AnL.Models;
using System.Collections.Generic;

namespace AnL.Repository.Abstraction
{
    public interface IEmployeeDetails : IRepository<EmployeeDetails>
    {
        public List<EmployeeDetails> getEmployeeDetails(List<string> EmployeeID);
    }
}
