using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnL.ViewModel
{
    public class ClientViewModel
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string PointOfContactName { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmailAddress { get; set; }
    }
}
