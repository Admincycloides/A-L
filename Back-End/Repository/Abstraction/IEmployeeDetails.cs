using AnL.Models;
using System.Collections.Generic;

namespace AnL.Repository.Abstraction
{
    public interface IEmployeeDetails : IRepository<EmployeeDetails>
    {
        public List<EmployeeDetails> GetEmployeeDetails(List<string> EmployeeID);
        EmployeeDetails GetById(object Id);
    }
}
