using System;
using System.Collections.Generic;

namespace AnL.Models
{
    public partial class Audit1
    {
        public DateTime AuditDateTimeUtc { get; set; }
        public string AuditType { get; set; }
        public string AuditUser { get; set; }
        public string TableName { get; set; }
        public string KeyValues { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string ChangedColumns { get; set; }
        public int Pid { get; set; }
    }
}
