using AnL.Constants;
using AnL.Filter;
using AnL.Helpers;
using AnL.Models;
using AnL.Repository.Abstraction;
using AnL.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuditController : Controller
    {
        private readonly IUnitOfWork _UOW;
        private readonly IAudit _Audit;
        private readonly IUriService uriService;
        public AuditController(IUnitOfWork UOW, IUriService uriService)
        {
            _UOW = UOW;
            this.uriService = uriService;

        }

        [HttpGet]
        public async Task<ActionResult> GetAuditLog()
        {
            try
            {
                BaseResponse rsp = new BaseResponse();
                rsp.Data = await _UOW.AuditRepository.GetAuditLog();
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }

        }

        
        [HttpPost]
        public async Task<ActionResult> GetAuditLogDetails([FromQuery] PaginationFilter filter, AuditViewModel model, string employeeID)
        {
            List<AuditViewModel> result = new List<AuditViewModel>();
            try
            {
                
                 
                List<Audit1> data = new List<Audit1>();
                if (!string.IsNullOrEmpty(employeeID))
                {
                    if (!string.IsNullOrEmpty(model.FromDate) || !string.IsNullOrEmpty(model.ToDate))
                    {
                        data = _UOW.AuditRepository.GetAllByCondition(x => x.AuditDateTimeUtc >= DateTime.Parse(model.FromDate) && x.AuditDateTimeUtc <= DateTime.Parse(model.ToDate) && x.AuditType.Contains(model.AuditType) && x.TableName.Contains(model.TableName)).Where(x => x.AuditUser==employeeID).ToList();

                    }
                    else
                    {
                        data = _UOW.AuditRepository.GetAllByCondition(x => x.AuditType.Contains(model.AuditType) && x.TableName.Contains(model.TableName)).Where(x => x.AuditUser ==employeeID).ToList();
                    }
                       //data = _UOW.AuditRepository.GetAllByCondition(x => x.AuditDateTimeUtc >= DateTime.Parse(model.FromDate) && x.AuditDateTimeUtc <= DateTime.Parse(model.ToDate) && x.AuditType.Contains(model.AuditType) && x.TableName.Contains(model.TableName)).Where(x => x.AuditUser.Contains(employeeID)).ToList();
                }
                else if (!string.IsNullOrEmpty(model.FromDate) || !string.IsNullOrEmpty(model.ToDate))
                {
                    data = _UOW.AuditRepository.GetAllByCondition(x => x.AuditDateTimeUtc >= DateTime.Parse(model.FromDate) && x.AuditDateTimeUtc <= DateTime.Parse(model.ToDate) && x.AuditType.Contains(model.AuditType) && x.TableName.Contains(model.TableName)).ToList();
                }
                else
                {
                    data = _UOW.AuditRepository.GetAllByCondition(x => x.AuditType.Contains(model.AuditType) && x.TableName.Contains(model.TableName)).ToList();
                }
                if(data.Count==0)
                {
                    data = _UOW.AuditRepository.GetAll().ToList();
                }
                var AuditLogList = data.Select(x => new
                {
                    x.AuditDateTimeUtc.Date,
                    x.AuditType,
                    x.ChangedColumns,
                    x.OldValues,
                    x.NewValues,
                    x.TableName,
                    EmployeeName = String.Concat(_UOW.EmployeeDetailsRepository.GetById(x.AuditUser).FirstName, " ", _UOW.EmployeeDetailsRepository.GetById(x.AuditUser).LastName)
                }).Distinct().ToList();

                foreach (var logs in AuditLogList)
                {
                    AuditViewModel auditViewModel = new AuditViewModel();
                    auditViewModel.AuditDateTime = logs.Date;
                    auditViewModel.AuditType = logs.AuditType;
                    auditViewModel.OldValues = logs.OldValues;
                    auditViewModel.NewValues = logs.NewValues;
                    auditViewModel.TableName = logs.TableName;
                    auditViewModel.ChangedColumns = logs.ChangedColumns;
                    auditViewModel.AuditUser = logs.EmployeeName;
                    result.Add(auditViewModel);
                }
                BaseResponse response = new BaseResponse();
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var pagedData = result.OrderByDescending(x => x.AuditDateTime).Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                                    .Take(validFilter.PageSize)
                                                    .ToList();
                var totalRecords = result.Count;
                var route = Request.Path.Value;
                var pagedReponse = PaginationHelper.CreatePagedReponse<AuditViewModel>(pagedData, validFilter, totalRecords, uriService, route);
                pagedReponse.ResponseCode = HTTPConstants.OK;
                pagedReponse.ResponseMessage = MessageConstants.AuditLogFetchedSuccess;
                return Ok(pagedReponse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return BadRequest("Oops! Something went wrong!" + ex);
            }
        }
    }
}
