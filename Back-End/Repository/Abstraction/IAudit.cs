using AnL.Filter;
using AnL.Models;
using AnL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.Repository.Abstraction
{
    public interface IAudit : IRepository<Audit1>
    {
        public void AddAuditLogs(string userName);
        public Task<List<AuditViewModel>> GetAuditLog();
        public Task<Object> GetAuditLogDetails([FromQuery] PaginationFilter filter, AuditViewModel model, string search);
    }
}
