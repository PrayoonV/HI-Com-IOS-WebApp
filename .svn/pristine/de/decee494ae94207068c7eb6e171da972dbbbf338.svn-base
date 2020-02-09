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
using DevExpress.Web;
using HicomIOS.ClassUtil;
using System.Collections;
using System.IO;
using OfficeOpenXml;

namespace HicomIOS.Master
{
    public partial class ExportSummaryQuotationProduct : MasterDetailPage
    {
        public class SummaryQuotationProductData
        {
            public int customer_id { get; set; }
            public int cat_id { get; set; }
            public int id { get; set; }
            public int employee_id { get; set; }
            public int customer_group { get; set; }
            public string quotation_status { get; set; }
            public string quotation_date_from { get; set; }
            public string quotation_date_to { get; set; }
            public string quotation_no { get; set; }
            public string customer_name { get; set; }
            public string model_list_air_compressor { get; set; }
            public string model_list_air_presure { get; set; }
            public string model_list_air_dryer { get; set; }
            public string model_list_line_filter { get; set; }
            public string model_list_mist_filter { get; set; }
            public string model_list_air_tank { get; set; }
            public string model_list_other { get; set; }
            public string po_date_from { get; set; }
            public string po_date_to { get; set; }
            public string other { get; set; }
            public string grand_total { get; set; }
            public string po_no { get; set; }
            public string amount_po { get; set; }

        }
        List<SummaryQuotationProductData> summaryQuotationProductData
        {
            get
            {
                if (Session["SESSION_EXPORT_SUMMARY_QUOTATION_CONTRACT"] == null)
                    Session["SESSION_EXPORT_SUMMARY_QUOTATION_CONTRACT"] = new List<SummaryQuotationProductData>();
                return (List<SummaryQuotationProductData>)Session["SESSION_EXPORT_SUMMARY_QUOTATION_CONTRACT"];
            }
            set
            {
                Session["SESSION_EXPORT_SUMMARY_QUOTATION_CONTRACT"] = value;
            }
        }

        //private static Excel.Workbook MyBook = null;
        //private static Excel.Application MyApp = null;
        //private static Excel.Worksheet MySheet = null;
        private static string baseUrl;

        public override string PageName { get { return "SummaryQuotationProductData"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }
        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            return result.OfType<FilterControlColumn>();
        }

        public override void OnFilterChanged()
        {
            //BindGrid();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            baseUrl = SPlanetUtil.GetBaseURL(Page.Request);
            if (!Page.IsPostBack)
            {
                // Get Permission and if no permission, will redirect to another page.
                if (!Permission.GetPermission())
                    Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

                PrepareData();
                ClearWoringSession();
            }
            else
            {
                gridView.DataSource = (from t in summaryQuotationProductData select t).ToList();
                gridView.DataBind();
                //BindGrid(false);
            }
        }
        protected void ClearWoringSession()
        {
            Session.Remove("SESSION_EXPORT_SUMMARY_QUOTATION_CONTRACT");
        }

        protected void PrepareData()
        {
            try
            {
                if (!Page.IsPostBack)
                {

                    SPlanetUtil.BindASPxComboBox(ref cbbEmployee, DataListUtil.DropdownStoreProcedureName.Employee_List);
                    SPlanetUtil.BindASPxComboBox(ref cbbCustomerGroup, DataListUtil.DropdownStoreProcedureName.Customer_Group);

                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@type", SqlDbType.VarChar,2) { Value = "PO" },
                        };
                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_quotation_type", arrParm.ToArray());

                        conn.Close();
                        cbbQuotation.DataSource = dsResult;
                        cbbQuotation.TextField = "data_text";
                        cbbQuotation.ValueField = "data_value";
                        cbbQuotation.DataBind();

                    }
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@cat_cade", SqlDbType.VarChar,2) { Value = "AC" },
                        };
                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_product_model", arrParm.ToArray());

                        conn.Close();
                        cbbAirCompressorModel.DataSource = dsResult;
                        cbbAirCompressorModel.TextField = "product_model";
                        cbbAirCompressorModel.ValueField = "product_model";
                        cbbAirCompressorModel.DataBind();

                    }
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@cat_cade", SqlDbType.VarChar,2) { Value = "D" },
                        };
                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_product_model", arrParm.ToArray());

                        conn.Close();
                        cbbAirDryerModel.DataSource = dsResult;
                        cbbAirDryerModel.TextField = "product_model";
                        cbbAirDryerModel.ValueField = "product_model";
                        cbbAirDryerModel.DataBind();

                    }
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@cat_cade", SqlDbType.VarChar,2) { Value = "L" },
                        };
                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_product_model", arrParm.ToArray());

                        conn.Close();
                        cbbLineFilterModel.DataSource = dsResult;
                        cbbLineFilterModel.TextField = "product_model";
                        cbbLineFilterModel.ValueField = "product_model";
                        cbbLineFilterModel.DataBind();

                    }
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@cat_cade", SqlDbType.VarChar,2) { Value = "M" },
                        };
                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_product_model", arrParm.ToArray());

                        conn.Close();
                        cbbMistFilterModel.DataSource = dsResult;
                        cbbMistFilterModel.TextField = "product_model";
                        cbbMistFilterModel.ValueField = "product_model";
                        cbbMistFilterModel.DataBind();

                    }
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@cat_cade", SqlDbType.VarChar,2) { Value = "T" },
                        };
                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_product_model", arrParm.ToArray());

                        conn.Close();
                        cbbAirTankModel.DataSource = dsResult;
                        cbbAirTankModel.TextField = "product_model";
                        cbbAirTankModel.ValueField = "product_model";
                        cbbAirTankModel.DataBind();

                    }
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@cat_cade", SqlDbType.VarChar,2) { Value = "O" },
                        };
                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_product_model", arrParm.ToArray());

                        conn.Close();
                        cbbOtherModel.DataSource = dsResult;
                        cbbOtherModel.TextField = "product_model";
                        cbbOtherModel.ValueField = "product_model";
                        cbbOtherModel.DataBind();

                    }

                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {

                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_product_pressure");

                        conn.Close();
                        cbbAirCompressorPresure.DataSource = dsResult;
                        cbbAirCompressorPresure.TextField = "pressure";
                        cbbAirCompressorPresure.ValueField = "pressure";
                        cbbAirCompressorPresure.DataBind();

                    }

                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                        };
                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_list", arrParm.ToArray());

                        conn.Close();
                        cbbCustomer.DataSource = dsResult;
                        cbbCustomer.TextField = "code_name";
                        cbbCustomer.ValueField = "id";
                        cbbCustomer.DataBind();

                    }

                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@lang_id", SqlDbType.VarChar,3) { Value = "tha" },
                            new SqlParameter("@cat_type", SqlDbType.Int) { Value = "PP" },
                        };
                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_product_category_export", arrParm.ToArray());

                        conn.Close();
                        cbbProductCat.DataSource = dsResult;
                        cbbProductCat.TextField = "data_text";
                        cbbProductCat.ValueField = "data_value";
                        cbbProductCat.DataBind();

                    }

                    var dtSource = new DataTable();
                    dtSource.Columns.Add("data_value", typeof(string));
                    dtSource.Columns.Add("data_text", typeof(string));
                    dtSource.Rows.Add("FL", "Follow");
                    dtSource.Rows.Add("CF", "Confirm");
                    dtSource.Rows.Add("PO", "PO");
                    dtSource.Rows.Add("DL", "Delivery");
                    dtSource.Rows.Add("CP", "Completed");
                    dtSource.Rows.Add("LS", "Lose");
                    cbbStatus.DataSource = dtSource;
                    cbbStatus.DataBind();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region "QUOTATION SUMMARY PRODCUT REPORT"
        private enum Column_QuotationSummary_Product
        {
            COUNT_CALL = 1,
            QUOTATION_NO = 2,
            QUOTATION_DATE_DD = 3,
            QUOTATION_DATE_MM = 4,
            QUOTATION_DATE_YY = 5,
            CUSTOMER_NAME = 6,
            PROJECT_NAME = 7,
            PRODUCT_NAME = 8,
            PROVINCE_NAME = 9,
            CUSTOMER_ADDRESS = 10,
            CATEGORY_ALIAS_LIST = 11,
            MODEL_LIST_AIR_COMPRESSOR = 12,
            MODEL_LIST_AIR_COMPRESSOR_PRESSURE = 13,
            MODEL_LIST_AIR_COMPRESSOR_QTY = 14,
            MODEL_LIST_AIR_DRYER = 15,
            MODEL_LIST_AIR_DRYER_QTY = 16,
            MODEL_LIST_LINE_FILTER = 17,
            MODEL_LIST_LINE_FILTER_QTY = 18,
            MODEL_LIST_MIST_FILTER = 19,
            MODEL_LIST_MIST_FILTER_QTY = 20,
            MODEL_LIST_AIR_TANK = 21,
            MODEL_LIST_AIR_TANK_QTY = 22,
            MODEL_LIST_OTHER = 23,
            MODEL_LIST_OTHER_QTY = 24,
            SALES_NAME = 25,
            PRICE_LIST = 26,
            TOTAL_DISCOUNT = 27,
            PRICE = 28,
            CONTACT_NAME = 29,
            STATUS_NAME = 30,
            PO_NUMBER = 31,
            PO_AMOUNT = 32,
            CUSTOMER_REQUEST_BY = 33,
            CUSTOMER_REQUEST_DATE_DD = 34,
            CUSTOMER_REQUEST_DATE_MM = 35,
            CUSTOMER_REQUEST_DATE_YY = 36,
            TEST_RUN_DATE_DD = 37,
            TEST_RUN_DATE_MM = 38,
            TEST_RUN_DATE_YY = 39,
            CUSTOMER_TEL = 40,
            CUSTOMER_FAX = 41,
            CUSTOMER_EMAIL = 42,
            REMARK = 43
        }

        [WebMethod]
        public static string Export_Quotation_Summary_Product(SummaryQuotationProductData[] dataSummaryQuotationProduct)
        {
            string strDownloadUrl = string.Empty;

            try
            {
                string strTemplatePath = "/Master/Template/";
                string baseUrl = string.Empty;
                String strExcelPath = string.Empty;
                string strSummaryQUTemplateFile = "Template_QuotationSummaryReport-Product.xlsx";
                int intStartRow = 5;
                DataSet dsResult;
              

                string excelTemplate = Path.Combine(HttpContext.Current.Server.MapPath(strTemplatePath), strSummaryQUTemplateFile);
                FileInfo templateFile = new FileInfo(excelTemplate);
                ExcelPackage excel = new ExcelPackage(templateFile);
                var templateSheet = excel.Workbook.Worksheets["Sheet1"];
                var workSheet = excel.Workbook.Worksheets.Copy("Sheet1", "SummaryQU");
                workSheet.DeleteRow(5, 1048576 - 5);
                var data = (from t in dataSummaryQuotationProduct select t).FirstOrDefault();
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                    {
                        new SqlParameter("@customer_id", SqlDbType.Int) { Value = Convert.ToInt32(data.customer_id) },
                        new SqlParameter("@customer_group", SqlDbType.Int) { Value = Convert.ToInt32(data.customer_group) },
                        new SqlParameter("@cat_id", SqlDbType.Int) { Value = Convert.ToInt32(data.cat_id) },
                        new SqlParameter("@employee_id", SqlDbType.Int) { Value = Convert.ToInt32(data.employee_id) },
                        new SqlParameter("@quotation_status", SqlDbType.VarChar,2) { Value = data.quotation_status },
                        new SqlParameter("@quotation_date_from", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.quotation_date_from) },
                        new SqlParameter("@quotation_date_to", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.quotation_date_to) },
                        new SqlParameter("@model_list_air_compressor", SqlDbType.VarChar,200) { Value = data.model_list_air_compressor},
                        new SqlParameter("@model_list_air_presure", SqlDbType.VarChar,200) { Value = data.model_list_air_presure },
                        new SqlParameter("@model_list_air_dryer", SqlDbType.VarChar,200) { Value =   data.model_list_air_dryer },
                        new SqlParameter("@model_list_line_filter", SqlDbType.VarChar,200) { Value = data.model_list_line_filter  },
                        new SqlParameter("@model_list_mist_filter", SqlDbType.VarChar,200) { Value = data.model_list_mist_filter },
                        new SqlParameter("@model_list_air_tank", SqlDbType.VarChar,200) { Value = data.model_list_air_tank },
                        new SqlParameter("@model_list_other", SqlDbType.VarChar,200) { Value = data.model_list_other },
                        new SqlParameter("@other", SqlDbType.VarChar,200) { Value = data.other },
                        new SqlParameter("@po_date_from", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.po_date_from) },
                        new SqlParameter("@po_date_to", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.po_date_to) },
                        new SqlParameter("@quotation_no", SqlDbType.VarChar, 20) { Value = data.quotation_no },
                    };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_report_quotation_summary_product", arrParm.ToArray());
                    var i = 0;
                    foreach (DataRow row in dsResult.Tables[0].Rows)
                    {
                        i++;
                        templateSheet.SelectedRange["A5:XFA5"].Copy(workSheet.SelectedRange[intStartRow, 1]);
                        //workSheet.Cells[intStartRow, 1].Value = i;
                        //workSheet.Cells[intStartRow, 2].Value = Convert.IsDBNull(row["id"]) ? string.Empty : Convert.ToString(row["id"]);
                        //workSheet.Cells[intStartRow, 3].Value = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]);
                        //workSheet.Cells[intStartRow, 4].Value = Convert.IsDBNull(row["product_name_tha"]) ? string.Empty : Convert.ToString(row["product_name_tha"]);
                        //workSheet.Cells[intStartRow, 5].Value = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]);
                        //workSheet.Cells[intStartRow, 6].Value = Convert.IsDBNull(row["quantity"]) ? string.Empty : Convert.ToString(row["quantity"]);
                        //workSheet.Cells[intStartRow, 7].Value = Convert.IsDBNull(row["quantity_reserve"]) ? string.Empty : Convert.ToString(row["quantity_reserve"]);
                        //workSheet.Cells[intStartRow, 8].Value = 0;
                        //workSheet.Cells[intStartRow, 9].Value = string.Empty;

                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.COUNT_CALL].Value = i;
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.QUOTATION_NO].Value = row["quotation_no"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.QUOTATION_DATE_DD].Value = row["quotation_date_dd"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.QUOTATION_DATE_MM].Value = row["quotation_date_mm"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.QUOTATION_DATE_YY].Value = row["quotation_date_yy"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.CUSTOMER_NAME].Value = row["customer_name"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.PROJECT_NAME].Value = row["project_name"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.PRODUCT_NAME].Value = row["product_name"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.PROVINCE_NAME].Value = row["province_name"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.CUSTOMER_ADDRESS].Value = row["customer_address"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.CATEGORY_ALIAS_LIST].Value = row["cat_code"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.MODEL_LIST_AIR_COMPRESSOR].Value = row["model_list_air_compressor"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.MODEL_LIST_AIR_COMPRESSOR_PRESSURE].Value = row["model_list_air_compressor_pressure"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.MODEL_LIST_AIR_COMPRESSOR_QTY].Value = row["model_list_air_compressor_qty"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.MODEL_LIST_AIR_DRYER].Value = row["model_list_air_dryer"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.MODEL_LIST_AIR_DRYER_QTY].Value = row["model_list_air_dryer_qty"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.MODEL_LIST_LINE_FILTER].Value = row["model_list_line_filter"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.MODEL_LIST_LINE_FILTER_QTY].Value = row["model_list_line_filter_qty"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.MODEL_LIST_MIST_FILTER].Value = row["model_list_mist_filter"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.MODEL_LIST_MIST_FILTER_QTY].Value = row["model_list_mist_filter_qty"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.MODEL_LIST_AIR_TANK].Value = row["model_list_air_tank"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.MODEL_LIST_AIR_TANK_QTY].Value = row["model_list_air_tank_qty"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.MODEL_LIST_OTHER].Value = row["model_list_other"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.MODEL_LIST_OTHER_QTY].Value = row["model_list_other_qty"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.SALES_NAME].Value = row["sales_name"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.PRICE_LIST].Value = row["total_amount"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.TOTAL_DISCOUNT].Value = row["total_discount"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.PRICE].Value = row["grand_total"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.CONTACT_NAME].Value = row["contact_name"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.STATUS_NAME].Value = row["status_name"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.PO_NUMBER].Value = row["po_no"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.PO_AMOUNT].Value = row["amount_po"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.CUSTOMER_REQUEST_BY].Value = row["sent_by"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.CUSTOMER_REQUEST_DATE_DD].Value = row["customer_request_date"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.CUSTOMER_REQUEST_DATE_MM].Value = row["customer_request_month"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.CUSTOMER_REQUEST_DATE_YY].Value = row["customer_request_year"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.TEST_RUN_DATE_DD].Value = row["test_run_date"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.TEST_RUN_DATE_MM].Value = row["test_run_month"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.TEST_RUN_DATE_YY].Value = row["test_run_year"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.CUSTOMER_TEL].Value = row["customer_tel"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.CUSTOMER_FAX].Value = row["customer_fax"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.CUSTOMER_EMAIL].Value = row["customer_email"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Product.REMARK].Value = row["remark"].ToString();

                        intStartRow++;
                    }
                }

                templateSheet = excel.Workbook.Worksheets["Sheet1"];
                excel.Workbook.Worksheets.Delete(templateSheet);
                //strDownloadUrl = baseUrl + strTemplatePath + strProductTemplateFile;
                string pathTemp = Path.GetTempFileName();
                excel.SaveAs(new FileInfo(pathTemp));

                strDownloadUrl = string.Format("LocalPath={0}&FileName={1}", pathTemp, "QuotationSummaryReport_Product.xlsx");
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
            }
            return strDownloadUrl;
        }
        #endregion

        [WebMethod]
        public static void GetViewGrid(SummaryQuotationProductData[] dataSummaryQuotationProduct)
        {
            var dsData = new DataSet();
            var SummaryQuotationProductData = new List<SummaryQuotationProductData>();
            var data = (from t in dataSummaryQuotationProduct select t).FirstOrDefault();
            if (data != null)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = Convert.ToInt32(data.customer_id) },
                            new SqlParameter("@customer_group", SqlDbType.Int) { Value = Convert.ToInt32(data.customer_group) },
                            new SqlParameter("@cat_id", SqlDbType.Int) { Value = Convert.ToInt32(data.cat_id) },
                            new SqlParameter("@employee_id", SqlDbType.Int) { Value = Convert.ToInt32(data.employee_id) },
                            new SqlParameter("@quotation_status", SqlDbType.VarChar,2) { Value = data.quotation_status },
                            new SqlParameter("@quotation_date_from", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.quotation_date_from) },
                            new SqlParameter("@quotation_date_to", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.quotation_date_to) },
                            new SqlParameter("@model_list_air_compressor", SqlDbType.VarChar,200) { Value = data.model_list_air_compressor},
                            new SqlParameter("@model_list_air_presure", SqlDbType.VarChar,200) { Value = data.model_list_air_presure },
                            new SqlParameter("@model_list_air_dryer", SqlDbType.VarChar,200) { Value =   data.model_list_air_dryer },
                            new SqlParameter("@model_list_line_filter", SqlDbType.VarChar,200) { Value = data.model_list_line_filter  },
                            new SqlParameter("@model_list_mist_filter", SqlDbType.VarChar,200) { Value = data.model_list_mist_filter },
                            new SqlParameter("@model_list_air_tank", SqlDbType.VarChar,200) { Value = data.model_list_air_tank },
                            new SqlParameter("@model_list_other", SqlDbType.VarChar,200) { Value = data.model_list_other },
                            new SqlParameter("@other", SqlDbType.VarChar,200) { Value = data.other },
                            new SqlParameter("@po_date_from", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.po_date_from) },
                            new SqlParameter("@po_date_to", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.po_date_to) },
                            new SqlParameter("@quotation_no", SqlDbType.VarChar, 20) { Value = data.quotation_no },
                        };
                        conn.Open();
                        dsData = SqlHelper.ExecuteDataset(conn, "sp_report_quotation_summary_product", arrParm.ToArray());

                        if (dsData.Tables[0].Rows.Count > 0)
                        {
                            foreach (var row in dsData.Tables[0].AsEnumerable())
                            {
                                SummaryQuotationProductData.Add(new SummaryQuotationProductData()
                                {
                                    id = Convert.ToInt32(row["id"]),
                                    quotation_no = Convert.IsDBNull(row["quotation_no"]) ? string.Empty : Convert.ToString(row["quotation_no"]),
                                    quotation_date_from = Convert.IsDBNull(row["quotation_date"]) ? string.Empty : Convert.ToString(row["quotation_date"]),
                                    customer_name = Convert.IsDBNull(row["customer_name"]) ? string.Empty : Convert.ToString(row["customer_name"]),
                                    grand_total = Convert.IsDBNull(row["grand_total"]) ? string.Empty : Convert.ToString(row["grand_total"]),
                                    po_no = Convert.IsDBNull(row["po_no"]) ? string.Empty : Convert.ToString(row["po_no"]),
                                    amount_po = Convert.IsDBNull(row["amount_po"]) ? string.Empty : Convert.ToString(row["amount_po"]),

                                });
                            }
                        }
                    }
                    HttpContext.Current.Session["SESSION_EXPORT_SUMMARY_QUOTATION_CONTRACT"] = SummaryQuotationProductData;
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }


        }
        protected void gridExportSummaryQuotationProduct_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridView.DataSource = (from t in summaryQuotationProductData select t).ToList();
            gridView.DataBind();
        }
    }
}