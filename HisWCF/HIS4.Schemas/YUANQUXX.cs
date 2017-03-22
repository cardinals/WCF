using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class YUANQUXX_IN : MessageIn
    {

    }


    public class YUANQUXX_OUT : MessageOUT
    {
        public List<YUANQUXX> YUANQUMX { get; set; }

        public YUANQUXX_OUT(){
            this.YUANQUMX = new List<YUANQUXX>();
        }
    }

    public class YUANQUXX
    {
        public string YUANQUID { get; set; }
        public string YUANQUMC { get; set; }
    }
}
