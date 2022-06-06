using System;
using System.Collections.Generic;

namespace AnL.Models
{
    public partial class ClientDetails
    {
        public ClientDetails()
        {
            ProjectDetails = new HashSet<ProjectDetails>();
        }

        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string PointOfContactName { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmailAddress { get; set; }

        public virtual ICollection<ProjectDetails> ProjectDetails { get; set; }
    }
}
