using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class GUAHAOKSXX_OUT : BaseOutEntity
    {
        public IList<KESHIXX> KESHIMX { get; set; }
    }
}