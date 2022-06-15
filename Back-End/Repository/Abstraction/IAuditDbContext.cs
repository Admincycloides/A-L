using AnL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.Repository.Abstraction
{
    public interface IAuditDbContext
    {
        DbSet<Audit1> Audit { get; set; }
        ChangeTracker ChangeTracker { get; }
    }
}
