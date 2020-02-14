using DevExpress.XtraReports.UI;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace HicomIOS.Report
{
    public partial class DocumentViewer : System.Web.UI.Page
    {
        const string LoadReportArgsKey = "ReportArgs";
        protected void Page_Load(object sender, EventArgs e)
        {
            XtraReport mainReport;
            var query = Request.QueryString[LoadReportArgsKey];

            ArrayList paymentIds = new ArrayList();

            //  If sale order
            if (query.Contains("Sale_Order_Product"))
            {
                var args = SPlanetUtil.DeserializeCallbackArgs(query);
                DataSet dsSaleOrderData;
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@sale_order_no", SqlDbType.VarChar, 20) { Value = args[1].ToString() },
                        };
                    conn.Open();
                    dsSaleOrderData = SqlHelper.ExecuteDataset(conn, "sp_sale_order_list_data_by_no", arrParm.ToArray());
                    conn.Close();
                }
                if (dsSaleOrderData.Tables.Count > 0)
                {
                    #region SaleOrder Payment
                    // Quotation Notification
                    foreach (var row in dsSaleOrderData.Tables[3].AsEnumerable())
                    {
                        paymentIds.Add(Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]));
                    }
                    #endregion SaleOrder Payment

                    if (paymentIds.Count == 0)
                    {
                        paymentIds.Add(0);
                    }
                }
                
                mainReport = new XtraReport();

                for (int i = 0; i < paymentIds.Count; i++)
                {
                    var report = HicomReportUtil.CreateReport(query + "|" + paymentIds[i] + "|" + (i + 1));
                    if (report != null)
                    {
                        //DocumentViewerControl.Report = report;
                        report.CreateDocument();

                        mainReport.Pages.AddRange(report.Pages);
                    }
                }
            }
            else
            {
                mainReport = HicomReportUtil.CreateReport(query);
            }

            DocumentViewerControl.Report = mainReport;

            var isTableLayout = true;
            if (query.Contains("Stock_Card")
                || query.Contains("Stock_Inventory_Products")
                )
            {
                isTableLayout = false;
            }
            DocumentViewerControl.SettingsReportViewer.TableLayout = isTableLayout;
            
            DocumentViewerControl.Report.RequestParameters = false;
            //DocumentViewerControl.Report.par
            //DocumentViewerControl.Report.Parameters.
        }

    }

    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // Set the resource directory
            string contentPath = Server.MapPath("");
            AppDomain.CurrentDomain.SetData("DXResourceDirectory", contentPath);
        }
        // ...
    }
}