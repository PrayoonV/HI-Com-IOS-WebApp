using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using DevExpress.Web;
using DevExpress.Data;
using HicomIOS.ClassUtil;
using DevExpress.XtraReports.Security;

namespace HicomIOS
{
    public class Global_asax : System.Web.HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            DevExpress.Web.ASPxWebControl.CallbackError += new EventHandler(Application_Error);
            ScriptPermissionManager.GlobalInstance = new ScriptPermissionManager(ExecutionMode.Unrestricted);

            DevExpress.XtraReports.Web.WebDocumentViewer.Native.WebDocumentViewerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;
            DevExpress.Utils.UrlAccessSecurityLevelSetting.SecurityLevel = DevExpress.Utils.UrlAccessSecurityLevel.Unrestricted;

        }

        void Application_End(object sender, EventArgs e)
        {
            // Code that runs on application shutdown
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            //Session[ConstantClass.SESSION_USER_ID] = 1;
           // BasePage.SelectedTopMenu = "";
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
            //Session[ConstantClass.SESSION_USER_ID] = null;
            Session.Clear();
            Session.Abandon();
        }
    }
}