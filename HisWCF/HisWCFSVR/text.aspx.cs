using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HisWCFSVR
{
    public partial class text : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request["id"];
            string tt = Request["tt"];
            Response.Write("success");
        }
    }
}