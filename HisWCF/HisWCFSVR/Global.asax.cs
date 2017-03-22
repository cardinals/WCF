using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace HisWCFSVR
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //if (System.Configuration.ConfigurationManager.AppSettings["HOUTAISEND"] == "1")
            //{
            //    System.Timers.Timer evtTimer = new System.Timers.Timer(5000);
            //    evtTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnEvtTimer);
            //    evtTimer.Interval = 3600000;
            //    evtTimer.Enabled = true;
            //}
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        private static void OnEvtTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (System.DateTime.Now.Hour.ToString() == "12" || System.DateTime.Now.Hour.ToString() == "17")
            {
                //AUTOSERVICES.Biz.AUTOSEND.send();
            }
        }
    }
}