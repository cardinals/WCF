using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class GUAHAOYSXX_OUT : BaseOutEntity
    {
        public IList<YISHENGXX> YISHENGMX { get; set; }
    }
}