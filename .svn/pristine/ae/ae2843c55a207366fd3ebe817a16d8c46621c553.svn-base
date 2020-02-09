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
using System.Web.Services;

namespace HicomIOS.Report
{
    public partial class ExportExcel : System.Web.UI.Page
    {
        private static Excel.Workbook MyBook = null;
        private static Excel.Application MyApp = null;
        private static Excel.Worksheet MySheet = null;
        private static string baseUrl;


     

        protected void Page_Load(object sender, EventArgs e)
        {
            baseUrl = SPlanetUtil.GetBaseURL(Page.Request);
        }

        

        #region "QUOTATION SUMMARY PRODCUT REPORT"
        private enum Column_QuotationSummary_Product
        {
            QUOTATION_NO = 1,
            QUOTATION_DATE_DD = 2,
            QUOTATION_DATE_MM = 3,
            QUOTATION_DATE_YY = 4,
            CUSTOMER_NAME = 5,
            PROJECT_NAME = 6,
            PRODUCT_NAME = 7,
            PROVINCE_NAME = 8,
            CUSTOMER_ADDRESS = 9,
            CATEGORY_ALIAS_LIST = 10,
            MODEL_LIST_AIR_COMPRESSOR = 11,
            MODEL_LIST_AIR_COMPRESSOR_PRESSURE = 12,
            MODEL_LIST_AIR_COMPRESSOR_QTY = 13,
            MODEL_LIST_AIR_DRYER = 14,
            MODEL_LIST_AIR_DRYER_QTY = 15,
            MODEL_LIST_LINE_FILTER = 16,
            MODEL_LIST_LINE_FILTER_QTY = 17,
            MODEL_LIST_MIST_FILTER = 18,
            MODEL_LIST_MIST_FILTER_QTY = 19,
            MODEL_LIST_AIR_TANK = 20,
            MODEL_LIST_AIR_TANK_QTY = 21,
            MODEL_LIST_OTHER = 22,
            MODEL_LIST_OTHER_QTY = 23,
            SALES_NAME = 24,
            TOTAL_AMOUNT = 25,
            TOTAL_DISCOUNT = 26,
            GRAND_TOTAL = 27,
            CONTACT_NAME = 28,
            STATUS_NAME = 29,
            STATUS_DATE_DD = 30,
            STATUS_DATE_MM = 31,
            STATUS_DATE_YY = 32,
            PO_NUMBER = 33,
            PO_AMOUNT = 34,
            CUSTOMER_REQUEST_BY = 35,
            CUSTOMER_REQUEST_DATE_DD = 36,
            CUSTOMER_REQUEST_DATE_MM = 37,
            CUSTOMER_REQUEST_DATE_YY = 38,
            TEST_RUN_DATE_DD = 39,
            TEST_RUN_DATE_MM = 40,
            TEST_RUN_DATE_YY = 41,
            CUSTOMER_TEL = 42,
            CUSTOMER_FAX = 43,
            CUSTOMER_EMAIL = 44,
            REMARK = 45
        }

        [WebMethod]
        public static string Export_Quotation_Summary_Product(string datepickerFrom, string datepickerTo)
        {
            var strDownloadUrl = "";
            string strTemplateFile = "Template_QuotationSummaryReport-Product.xlsx";
            int intStartRow = 5; 
           DataSet dsResult;

            try
            {
                // Open Excel Application
                MyApp = new Excel.Application();
                MyApp.Visible = false;
                var strTemplatePath = "/Report/Template/"; ;
                var strExcelPath = HttpContext.Current.Server.MapPath(strTemplatePath);
                MyBook = MyApp.Workbooks.Open(strExcelPath + strTemplateFile);
                MySheet = (Excel.Worksheet)MyBook.Sheets[1]; // Explicit cast is not required here
                                                             //int intStartRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;

                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                    {
                        new SqlParameter("@quotation_date_from", SqlDbType.VarChar,20) { Value = datepickerFrom },
                        new SqlParameter("@quotation_date_to", SqlDbType.VarChar,20) { Value =datepickerTo }
                    };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_report_quotation_summary_product", arrParm.ToArray());
                    foreach (DataRow row in dsResult.Tables[0].Rows)
                    {
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.QUOTATION_NO] = row["quotation_no"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.QUOTATION_DATE_DD] = row["quotation_date_dd"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.QUOTATION_DATE_MM] = row["quotation_date_mm"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.QUOTATION_DATE_YY] = row["quotation_date_yy"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.CUSTOMER_NAME] = row["customer_name"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.PROJECT_NAME] = row["project_name"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.PROVINCE_NAME] = row["province_name"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.CUSTOMER_ADDRESS] = row["customer_address"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.MODEL_LIST_AIR_COMPRESSOR] = row["model_list_air_compressor"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.MODEL_LIST_AIR_COMPRESSOR_PRESSURE] = row["model_list_air_compressor_pressure"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.MODEL_LIST_AIR_COMPRESSOR_QTY] = row["model_list_air_compressor_qty"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.MODEL_LIST_AIR_DRYER] = row["model_list_air_dryer"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.MODEL_LIST_AIR_DRYER_QTY] = row["model_list_air_dryer_qty"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.MODEL_LIST_LINE_FILTER] = row["model_list_line_filter"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.MODEL_LIST_LINE_FILTER_QTY] = row["model_list_line_filter_qty"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.MODEL_LIST_MIST_FILTER] = row["model_list_mist_filter"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.MODEL_LIST_MIST_FILTER_QTY] = row["model_list_mist_filter_qty"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.MODEL_LIST_AIR_TANK] = row["model_list_air_tank"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.MODEL_LIST_AIR_TANK_QTY] = row["model_list_air_tank_qty"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.MODEL_LIST_OTHER] = row["model_list_other"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.MODEL_LIST_OTHER_QTY] = row["model_list_other_qty"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.SALES_NAME] = row["sales_name"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.TOTAL_AMOUNT] = row["total_amount"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.TOTAL_DISCOUNT] = row["total_discount"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.GRAND_TOTAL] = row["grand_total"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.CONTACT_NAME] = row["contact_name"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.STATUS_NAME] = row["status_name"].ToString();
                        //MySheet.Cells[intStartRow, Column_QuotationSummary_Product.STATUS_DATE_DD] = row["status_date_dd"].ToString();
                        //MySheet.Cells[intStartRow, Column_QuotationSummary_Product.STATUS_DATE_MM] = row["status_date_mm"].ToString();
                        //MySheet.Cells[intStartRow, Column_QuotationSummary_Product.STATUS_DATE_YY] = row["status_date_yy"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.CUSTOMER_TEL] = row["customer_tel"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.CUSTOMER_FAX] = row["customer_fax"].ToString();
                        MySheet.Cells[intStartRow, Column_QuotationSummary_Product.CUSTOMER_EMAIL] = row["customer_email"].ToString();
                        //MySheet.Cells[intStartRow, Column_QuotationSummary_Product.REMARK] = row["remark"].ToString();

                        intStartRow++;
                    }
                }
                if (System.IO.File.Exists(strExcelPath + "Temp_quotation_summary_product.xlsx"))
                    System.IO.File.Delete(strExcelPath + "Temp_quotation_summary_product.xlsx");
                MyBook.SaveAs(strExcelPath + "Temp_quotation_summary_product.xlsx");
                MyApp.Quit();
                MyApp = null;

                strDownloadUrl = baseUrl + strTemplatePath + "Temp_quotation_summary_product.xlsx";

                

            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
            }
            return strDownloadUrl;
        }
        #endregion
    }
}