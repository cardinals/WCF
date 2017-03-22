using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb.Entity
{
    [Serializable]
    public class FEIYONGMX_IN : BaseInEntity
    {
        public string MINGXIXH { get; set; }
        public string FEIYONGLX { get; set; }
        public string XIANGMUXH { get; set; }
        public string XIANGMUCDDM { get; set; }
        public string XIANGMUMC { get; set; }
        public string XIANGMUGL { get; set; }
        public string XIANGMUGLMC { get; set; }
        public string XIANGMUGG { get; set; }
        public string XIANGMUJX { get; set; }
        public string XIANGMUDW { get; set; }
        public string XIANGMUCDMC { get; set; }
        public decimal? DANJIA { get; set; }
        public int? SHULIANG { get; set; }
        public decimal? JINE { get; set; }
        public string YIBAODJ { get; set; }
        public string YIBAODM { get; set; }
        public string YIBAOZFBL { get; set; }
        public string XIANGMUXJ { get; set; }
        public decimal? ZIFEIJE { get; set; }
        public decimal? ZILIJE { get; set; }
        public string SHENGPIBH { get; set; }
        public string ZIFEIBZ { get; set; }
        public string ZIFUBL { get; set; }
    }
}