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

            Response.ContentType = "application/ms-excel";
            //Response.ContentType = @"application/vnd.ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.TransmitFile(localPath);
            Response.End();

            //HttpResponse respone = HttpContext.Current.Response;
            //respone.ClearContent();
            //respone.Clear();

            ////Response.ContentType = "application/ms-excel";
            ////Response.ContentType = @"application/vnd.ms-excel";
            ////            respone.AppendHeader("Content-Disposition", "attachment; filename=" + "AnnualService.xlsx");
            //respone.ContentType = "application/ms-excel";
            //respone.AppendHeader("Content-Disposition", "attachment; filename=" + Uri.EscapeUriString(fileName));
            
            ////respone.ContentType = "text/plain";
            //respone.TransmitFile(localPath);
            ////respone.WriteFile(localPath);
            //respone.Flush();
            //respone.Close();

            return;
        }
    }
}