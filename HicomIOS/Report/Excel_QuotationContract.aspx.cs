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

namespace HicomIOS.Report
{
    public partial class Excel_QuotationContract : System.Web.UI.Page
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
            Export_QuotationSummary_Spare_Part();
        }
        private enum Column_QuotationSummary_Spare_Part
        {
            NO = 1,
            CONTRACT_NO = 2,
            QUOTATION_NO = 3,
            ISSUE_DATE_DD = 4,
            ISSUE_DATE_MM = 5,
            ISSUE_DATE_YY = 6,
            CODE = 7,
            CUSTOMER_NAME = 8,
            PROJECTY = 9,
            PRODUCT = 10,
            CONTACT = 11,
            QTY = 12,
            UNIT_PRICE = 13,
            DISCOUNT1 = 14,
            DISCOUNT2 = 15,
            TOTAL_AMOUNT = 16,
            Type_Of_Contract = 17,
            STARTIING_DD = 18,
            STARTIING_MM = 19,
            STARTIING_YY = 20,
            EXPIRE_DATE_DD = 21,
            EXPIRE_DATE_MM = 22,
            EXPIRE_DATE_YY = 23,
            DETAIL_STATUS = 24,
            DETAIL_DATE_DD = 25,
            DETAIL_DATE_MM = 26,
            DETAIL_DATE_YY = 27,
            No_PO = 28,
            AMOUNT_PO = 29,
            INVOICE_DATE_DD = 30,
            INVOICE_DATE_MM = 31,
            INVOICE_DATE_YY = 32,
            INV_No = 33,
            SCHEDULE_MM1 = 34,
            SCHEDULE_YY1 = 35,
            SCHEDULE_MM2 = 36,
            SCHEDULE_YY2 = 37,
            NEW_CONTRACT_MM = 38,
            NEW_CONTRACT_YY = 39,
           
        }
        private void Export_QuotationSummary_Spare_Part()
        {
            string strTemplateFile = "Template_QuotationSummaryReport_Contract.xlsx";
            int intStartRow = 7;
            int i = 1;
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
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_report_quotation_summary_service_contract", arrParm.ToArray());
                    foreach (DataRow row in dsResult.Tables[0].Rows)
                    {
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.NO] = intStartRow-i;
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.CONTRACT_NO] = row["contract_no"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.QUOTATION_NO] = row["quotation_no"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.ISSUE_DATE_DD] = row["issue_date_DD"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.ISSUE_DATE_MM] = row["issue_date_MM"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.ISSUE_DATE_YY] = row["issue_date_YY"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.CODE] = row["customer_code"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.CUSTOMER_NAME] = row["customer_name"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.PROJECTY] = row["project_name"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.PRODUCT] = row["product"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.CONTACT] = row["contact_name"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.QTY] = row["qty"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.UNIT_PRICE] = row["unit_price"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.DISCOUNT1] = row["Discount1"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.DISCOUNT2] = row["Discount2"].ToString(); ;
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.TOTAL_AMOUNT] = row["amount"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.Type_Of_Contract] = row["type_of_contract"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.STARTIING_DD] = row["startiing_date_DD"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.STARTIING_MM] = row["startiing_date_MM"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.STARTIING_YY] = row["startiing_date_YY"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.EXPIRE_DATE_DD] = row["expire_date_DD"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.EXPIRE_DATE_MM] = row["expire_date_MM"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.EXPIRE_DATE_YY] = row["expire_date_YY"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.DETAIL_STATUS] = row["status_name"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.DETAIL_DATE_DD] = row["detail_date_DD"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.DETAIL_DATE_MM] = row["detail_date_MM"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.DETAIL_DATE_YY] = row["detail_date_YY"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.No_PO] = row["po_no"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.AMOUNT_PO] = row["amount_no"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.INVOICE_DATE_DD] = row["inv_date_DD"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.INVOICE_DATE_MM] = row["inv_date_MM"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.INVOICE_DATE_YY] = row["inv_date_YY"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.INV_No] = row["inv_no"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.SCHEDULE_MM1] = row["schedule_from_MM"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.SCHEDULE_YY1] = row["schedule_from_YY"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.SCHEDULE_MM2] = row["schedule_to_MM"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.SCHEDULE_YY2] = row["schedule_to_YY"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.NEW_CONTRACT_MM] = row["new_contract_date_MM"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Spare_Part.NEW_CONTRACT_YY] = row["new_contract_date_YY"].ToString();
                        i++ ;
                        intStartRow++;
                    }
                }
                if (System.IO.File.Exists(strExcelPath + "Temp_quotation_summary_Contract.xlsx"))
                    System.IO.File.Delete(strExcelPath + "Temp_quotation_summary_Contract.xlsx");
                MyBook.SaveAs(strExcelPath + "Temp_quotation_summary_Contract.xlsx");
                MyApp.Quit();
                MyApp = null;

                string strDownloadUrl = baseUrl + strTemplatePath + "Temp_quotation_summary_Contract.xlsx";
                Response.Redirect(strDownloadUrl);

            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
            }
        }

    }
}

