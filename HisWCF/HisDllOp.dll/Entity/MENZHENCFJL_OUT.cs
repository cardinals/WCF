using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class MENZHENCFJL_OUT : BaseOutEntity
    {
        public int FEIYONGMXTS { get; set; }
        public List<CHUFANGJL> CHUFANGJL { get; set; }


    }
}
