using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class BASEINFO : BaseGroup
    {
        public string CAOZUOYDM { get; set; }
        public string CAOZUOYXM { get; set; }
        public DateTime? CAOZUORQ { get; set; }
        public int? XITONGBS { get; set; }
        public int? FENYUANDM { get; set; }
        public int? JIGOUDM { get; set; }
        public int? JESHOUJGDM { get; set; }
        public string ZHONGDUANJBH { get; set; }
        public string ZHONGDUANLSH { get; set; }
        public string MessageId { get; set; }
    }
}