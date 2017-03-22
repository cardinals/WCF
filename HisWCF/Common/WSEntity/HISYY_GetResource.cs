using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WSEntity
{
    public class HISYY_GetResource : WSEntityRequest
    {
        public HISYY_GetResource()
        {
            BespeakExamine = new List<BespeakExamine>();
        }

        public string HospitalCode { get; set; }
        public string HospitalName { get; set; }
        public string BespeakDate { get; set; }
        public IList<BespeakExamine> BespeakExamine { get; set; }
        public string AdmissionSource { get; set; }
        public string StudiesDepartMentCode { get; set; }
        public string StudiesDepartMentName { get; set; }
        public string IsZQ { get; set; }
        public string IsJZ { get; set; }
        public string IsLS { get; set; }
    }
}
