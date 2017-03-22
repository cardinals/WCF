using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDI.SIIM.SelfServiceWeb
{
    public class Step
    {
        public string WolkflowDiscription { get; set; }
        public string Discription { get; set; }
        public string Number { get; set; }
        public string URL { get; set; }
        public string ClassName { get; set; }
        public IList<string> Properties { get; set; }
    }
}