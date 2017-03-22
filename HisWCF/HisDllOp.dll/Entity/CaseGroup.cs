using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class CaseGroup : BaseGroup
    {
        public CaseBackGroup CaseBackGroup { get; set; }
        public string CaseGroupString { get; set; }
        public int? CaseGroupInt { get; set; }
        public IList<CaseGroup> CaseGroups { get; set; }
    }
}