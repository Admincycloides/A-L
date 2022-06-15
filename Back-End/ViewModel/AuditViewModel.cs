using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.ViewModel
{
    public class AuditViewModel
    {
        public DateTime? AuditDateTime { get; set; }
        public string AuditType { get; set; }
        public string AuditUser { get; set; }
        public string TableName { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string ChangedColumns { get; set; }
    }
}
