//using Audit.EntityFramework;
using System;
using System.Collections.Generic;

namespace AnL.Models
{
    //[AuditInclude]
    public partial class ActivityMapping
    {
        public int UniqueId { get; set; }
        public int ProjectId { get; set; }
        public int ActivityId { get; set; }
        public bool IsActive { get; set; }

        public virtual ActivityDetails Activity { get; set; }
        public virtual ProjectDetails Project { get; set; }
    }
}
