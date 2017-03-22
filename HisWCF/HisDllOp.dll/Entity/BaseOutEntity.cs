using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public abstract class BaseOutEntity : BaseEntity
    {
        public OUTMSG OUTMSG { get; set; }
    }
}