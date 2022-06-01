using AnL.Models;
using AnL.Repository.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _UOW;
        public EmployeeController(IUnitOfWork UOW)
        {
            this._UOW = UOW;
        }
        [HttpGet]
        public EmployeeDetails getEmployeeDetails(string UserID)
        {
            var Result= _UOW.EmployeeDetailRepository.GetById(UserID);
            return Result;
        }
    }
}
