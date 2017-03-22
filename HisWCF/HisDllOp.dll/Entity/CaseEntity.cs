using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class CaseEntity : BaseInEntity
    {
        public string CaseString { get; set; }
        public int? CaseInt { get; set; }
        public DateTime? CaseTime { get; set; }
        public IList<CaseGroup> CaseGroups { get; set; }
    }
}