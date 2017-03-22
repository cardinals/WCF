﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class CaseBackGroup : BaseGroup
    {
        public string CaseBackString { get; set; }
        public int? CaseBackInt { get; set; }
        public DateTime? CaseBackTime { get; set; }
    }
}