using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HicomIOS.Common
{
    public partial class DownloadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string localPath = Request.QueryString["LocalPath"];
            string fileName = Request.QueryString["FileName"];

            if(!File.Exists(localPath))
            {
                return;
            }

            HttpResponse respone = HttpContext.Current.Response;
            respone.ClearContent();
            respone.Clear();
            respone.AppendHeader("content-disposition", "attachment; filename=" + Uri.EscapeUriString(fileName));
            respone.ContentType = "text/plain";
            respone.WriteFile(localPath);
            respone.Flush();
            respone.Close();

            return;
        }
    }
}