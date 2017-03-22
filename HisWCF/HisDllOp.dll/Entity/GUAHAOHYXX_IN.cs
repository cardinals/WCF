using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class GUAHAOHYXX_IN : BaseInEntity
    {
        public int? GUAHAOFS { get; set; }
        public DateTime? RIQI { get; set; }
        public int? GUAHAOBC { get; set; }
        public string KESHIDM { get; set; }
        public string YISHENGDM { get; set; }
    }
}