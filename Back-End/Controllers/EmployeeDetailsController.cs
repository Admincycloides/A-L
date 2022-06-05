﻿using AnL.Constants;
using AnL.Models;
using AnL.Repository.Abstraction;
using AnL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeeDetailsController : Controller
    {
        private readonly IUnitOfWork _UOW;
        private readonly IEmployeeDetails _timesheetDetail;
        public EmployeeDetailsController(IUnitOfWork UOW)
        {
            _UOW = UOW;
        }
        [HttpGet]
        public ActionResult getEmployeeDetail(List<string> EmployeeID)
        {
            var results = _UOW.EmployeeDetailsRepository.GetEmployeeDetails(EmployeeID);
            if (results != null)
            {
                return Ok(results);
            }
            else
            {
                return null;
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetSupervisorDetails()
        {
            try
            {
                var superevisorDetails = _UOW.EmployeeDetailsRepository.GetAllByCondition(x => x.SupervisorFlag == "Y"&& x.EnabledFlag=="Enabled").ToList();
                BaseResponse response = new BaseResponse();
                if (superevisorDetails != null || superevisorDetails.Count > 0)
                {
                    response.Data = superevisorDetails;
                    response.ResponseCode = HTTPConstants.OK;
                    response.ResponseMessage = MessageConstants.SupervisorListingSuccess;
                }
                else
                {
                    response.Data = superevisorDetails;
                    response.ResponseCode = HTTPConstants.BAD_REQUEST;
                    response.ResponseMessage = MessageConstants.SupervisorListingFailed;
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }
        }
    }
}
