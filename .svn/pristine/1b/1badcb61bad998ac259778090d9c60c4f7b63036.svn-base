using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;
//Custom using namespace

namespace HicomIOS.Report
{
    public partial class Excel_QuotationSparePart : System.Web.UI.Page
    {
        private static Excel.Workbook MyBook = null;
        private static Excel.Application MyApp = null;
        private static Excel.Worksheet MySheet = null;
        private string strTemplatePath = "/Report/Template/";
        private string baseUrl = "";
        private String strExcelPath = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            baseUrl = SPlanetUtil.GetBaseURL(Page.Request);
        }
        protected void Export_Click(object sender, EventArgs e)
        {
            var dateFrom = datepickerFrom.Value;
            var dateTo = datepickerTo.Value;
            Export_QuotationSummary_Spare_Part();
        }
        private enum Column_QuotationSummary_Spare_Part
        {
            QUOTATION_NO = 1,
            QUOTATION_DATE_DD = 2,
            QUOTATION_DATE_MM = 3,
            QUOTATION_DATE_YY = 4,
            CUSTOMER_NAME = 5,
            H = 6,
            MODEL = 7,
            MFG_NO = 8,
            MACHIN_NO = 9,
            PRESSURE = 10,
            DETAIL_OF_PART = 11,
            DISCOUNT1 = 12,
            DISCOUNT2 = 13,
            AMOUNT = 14,
            CONTACT = 15,
            DETAIL_STATUS = 16,
            DETAIL_DD = 17,
            DETAIL_MM = 18,
            DETAIL_YY = 19,
            No_PO = 20,
            AMOUNT_PO = 21,
            SALE_ORDER_DATE = 22,
            DELIVERY_DATE = 23,
            INV_NO = 24,
            STATUS = 25,
            //REPORT = 26,
            //REMARK = 27,
        }
        private void Export_QuotationSummary_Spare_Part()
        {
            string strTemplateFile = "Template_QuotationSummaryReport_Spare_Part.xlsx";
            int intStartRow = 7;
            DataSet dsResult;

            try
            {
                // Open Excel Application
                MyApp = new Excel.Application();
                MyApp.Visible = false;
                strExcelPath = Server.MapPath(strTemplatePath);
                MyBook = MyApp.Workbooks.Open(strExcelPath + strTemplateFile);
                MySheet = (Excel.Worksheet)MyBook.Sheets[1];


                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                    {
                        new SqlParameter("@quotation_date_from", SqlDbType.VarChar,20) { Value = "" },
                        new SqlParameter("@quotation_date_to", SqlDbType.VarChar,20) { Value = "" }
                    };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_report_quotation_summary_spare_part", arrParm.ToArray());
                    foreach (DataRow row in dsResult.Tables[0].Rows)
                    {
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.QUOTATION_NO] = row["quotation_no"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.QUOTATION_DATE_DD] = row["quotation_date_DD"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.QUOTATION_DATE_MM] = row["quotation_date_MM"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.QUOTATION_DATE_YY] = row["quotation_date_YY"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.CUSTOMER_NAME] = row["customer_name"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.H] = row["h"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.MODEL] = row["model"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.MFG_NO] = row["mfg"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.MACHIN_NO] = row["machine"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.PRESSURE] = row["pressure"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.DETAIL_OF_PART] = row["detail_of_part"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.DISCOUNT1] = row["Discount1"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.DISCOUNT2] = row["Discount2"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.AMOUNT] = row["amount"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.CONTACT] = row["contact_name"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.DETAIL_STATUS] = row["qu_status"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.DETAIL_DD] = row["status_date_DD"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.DETAIL_MM] = row["status_date_MM"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.DETAIL_YY] = row["status_date_YY"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.No_PO] = row["po_no"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.AMOUNT_PO] = row["amount_no"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.SALE_ORDER_DATE] = row["sales_order_date"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.DELIVERY_DATE] = row["delivery_date"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.INV_NO] = row["inv_no"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.STATUS] = row["all_status"].ToString();
                        

                        intStartRow++;
                    }
                }
                if (System.IO.File.Exists(strExcelPath + "Temp_quotation_summary_Spare_Part.xlsx"))
                    System.IO.File.Delete(strExcelPath + "Temp_quotation_summary_Spare_Part.xlsx");
                MyBook.SaveAs(strExcelPath + "Temp_quotation_summary_Spare_Part.xlsx");
                MyApp.Quit();
                MyApp = null;

                string strDownloadUrl = baseUrl + strTemplatePath + "Temp_quotation_summary_Spare_Part.xlsx";
                Response.Redirect(strDownloadUrl);

            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
            }
        }

    }
}