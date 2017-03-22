using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class GUAHAOHYXX_OUT : BaseOutEntity
    {
        public IList<HAOYUANXX> HAOYUANMX { get; set; }
    }
}