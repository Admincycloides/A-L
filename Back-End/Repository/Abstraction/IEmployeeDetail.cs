using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnL.Models;

namespace AnL.Repository.Abstraction
{
    public interface IEmployeeDetail : IRepository<EmployeeDetails>
    {
         EmployeeDetails GetById(object Id);
    }
}
