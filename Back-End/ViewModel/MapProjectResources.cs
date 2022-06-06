using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.ViewModel
{
    public class MapProjectResources
    {
        public int ProjectID { get; set; }

        public List<string> SupervisorIds { get; set; }

        public List<string> EmployeeIds { get; set; }

        public string LoggedUserID { get; set; }

    }
}
