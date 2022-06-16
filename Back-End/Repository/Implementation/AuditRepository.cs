using AnL.Filter;
using AnL.Helpers.AuditTrail;
using AnL.Models;
using AnL.Repository.Abstraction;
using AnL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.Repository.Implementation
{
    public class AuditRepository : Repository<Audit1>, IAudit
    {
        private DbContext _context;
        private readonly IUnitOfWork _UOW;
        DbSet<Audit1> dbSet;

        public AuditRepository(DbContext context, IUnitOfWork UOW) : base(context)
        {
            this._context = context;
            this._UOW = UOW;
            dbSet = context.Set<Audit1>();
        }

        public void AddAuditLogs(string userName)
        {
            _context.ChangeTracker.DetectChanges();
            List<AuditEntry> auditEntries = new List<AuditEntry>();
            foreach (EntityEntry entry in _context.ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached ||
                    entry.State == EntityState.Unchanged)
                {
                    continue;
                }
                var auditEntry = new AuditEntry(entry, userName);
                auditEntries.Add(auditEntry);
            }

            if (auditEntries.Any())
            {
                var logs = auditEntries.Select(x => x.ToAudit());
                _context.Set<Audit1>().AddRange(logs);
            }
        }
        public async Task<List<AuditViewModel>> GetAuditLog()
        {
            List<AuditViewModel> rsp = new List<AuditViewModel>();
            try
            {
                await Task.Run(() =>
                {
                    rsp = (_context.Set<Audit1>().Select(

                        X => new AuditViewModel
                        {
                            AuditDateTime= X.AuditDateTimeUtc,
                            AuditType =X.AuditType,
                            AuditUser=X.AuditUser,
                            TableName=X.TableName,
                            OldValues=X.OldValues,
                            NewValues=X.NewValues,
                            ChangedColumns=X.ChangedColumns

                        }
                        )).ToList();

                });
                return rsp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Object> GetAuditLogDetails([FromQuery] PaginationFilter filter, AuditViewModel model, string search)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
