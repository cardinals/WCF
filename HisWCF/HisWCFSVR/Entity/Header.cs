using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HisWCFSVR.Entity
{
    public class Header
    {
        public string SendSystemId { get; set; }
        public string OrganizationId { get; set; }
        public string DocumentID { get; set; }
        public string UserId { get; set; }
        public string Pwd { get; set; }
        public string RequestTime { get; set; }
        public string Client_IP { get; set; }
        public string lient_Mac { get; set; }
    }
}