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
    public partial class ExportSummaryQuotationDesign : MasterDetailPage
    {
        public class SummaryQuotationDesignData
        {
            public int customer_id { get; set; }
            public int cat_id { get; set; }
            public int employee_id { get; set; }
            public int customer_group { get; set; }
            public string quotation_status { get; set; }
            public string quotation_date_from { get; set; }
            public string quotation_date_to { get; set; }
            public string quotation_no { get; set; }
            public int id { get; set; }
            public string customer_name { get; set; }
            public string model_list_air_compressor { get; set; }
            public string model_list_air_presure { get; set; }
            public string model_list_air_dryer { get; set; }
            public string model_list_line_filter { get; set; }
            public int amount_po { get; set; }
            public string project_name { get; set; }
            public string issue_date { get; set; }
            public string po_date_from { get; set; }
            public string po_date_to { get; set; }
            public string other { get; set; }
        }
        List<SummaryQuotationDesignData> summaryQuotationDesignData
        {
            get
            {
                if (Session["SESSION_EXPORT_SUMMARY_QUOTATION_DESIGN"] == null)
                    Session["SESSION_EXPORT_SUMMARY_QUOTATION_DESIGN"] = new List<SummaryQuotationDesignData>();
                return (List<SummaryQuotationDesignData>)Session["SESSION_EXPORT_SUMMARY_QUOTATION_DESIGN"];
            }
            set
            {
                Session["SESSION_EXPORT_SUMMARY_QUOTATION_DESIGN"] = value;
            }
        }

        private static string baseUrl;
        private static Excel.Workbook MyBook = null;
        private static Excel.Application MyApp = null;
        private static Excel.Worksheet MySheet = null;


        public override string PageName { get { return "SummaryQuotationDesign"; } }
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
                gridView.DataSource = (from t in summaryQuotationDesignData select t).ToList();
                gridView.DataBind();
                //BindGrid(false);
            }
        }
        protected void ClearWoringSession()
        {
            Session.Remove("SESSION_EXPORT_SUMMARY_QUOTATION_DESIGN");
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
                            new SqlParameter("@type", SqlDbType.VarChar,2) { Value = "D" },
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
        #region "QUOTATION SUMMARY DESIGN REPORT"
        private enum Column_QuotationSummary_Design
        {
            COUNT_CALL = 1,
            QUOTATION_NO = 2,
            ISSUE_DD = 3,
            ISSUE_MM = 4,
            ISSUE_YY = 5,
            CUSTOMER_NAME = 6,
            PROJECT_NAME = 7,
            SUbJECT = 8,
            DESCRIPTION = 9,
            SALES = 10,
            PRICE_LIST = 11,
            DISCOUNT = 12,
            FINAL_PRICE = 13,
            CONTACT_PERSON = 14,
            QUSTATUS = 15,
            UPDATE_DD= 16,
            UPDATE_MM= 17,
            UPDATE_YY = 18,
            PO_NO = 19,
            PODATE = 20,
            INVOICE = 21,
            TEL = 22,
        }

        [WebMethod]
        public static string Export_Quotation_Summary_Design(SummaryQuotationDesignData[] dataSummaryQuotationDesign)
        {
            var strDownloadUrl = "";

            try
            {
                string strTemplatePath = "/Master/Template/";
                string baseUrl = string.Empty;
                String strExcelPath = string.Empty;
                string strSummaryQUTemplateFile = "Template_QuotationSummaryReport-Design.xlsx";
                int intStartRow = 5;
                DataSet dsResult;


                string excelTemplate = Path.Combine(HttpContext.Current.Server.MapPath(strTemplatePath), strSummaryQUTemplateFile);
                FileInfo templateFile = new FileInfo(excelTemplate);
                ExcelPackage excel = new ExcelPackage(templateFile);
                var templateSheet = excel.Workbook.Worksheets["Sheet1"];
                var workSheet = excel.Workbook.Worksheets.Copy("Sheet1", "SummaryQU");
                workSheet.DeleteRow(intStartRow, 1048576 - intStartRow);

                var data = (from t in dataSummaryQuotationDesign select t).FirstOrDefault();
                if (data != null)
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = Convert.ToInt32(data.customer_id) },
                            new SqlParameter("@customer_group", SqlDbType.Int) { Value = Convert.ToInt32(data.customer_group) },
                            new SqlParameter("@employee_id", SqlDbType.Int) { Value = Convert.ToInt32(data.employee_id) },
                            new SqlParameter("@quotation_status", SqlDbType.VarChar, 2) { Value = data.quotation_status },
                            new SqlParameter("@quotation_date_from", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.quotation_date_from) },
                            new SqlParameter("@quotation_date_to", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.quotation_date_to) },
                            new SqlParameter("@po_date_from", SqlDbType.VarChar, 20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.po_date_from) },
                            new SqlParameter("@po_date_to", SqlDbType.VarChar, 20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.po_date_to) },
                            new SqlParameter("@quotation_no", SqlDbType.VarChar, 20) { Value = data.quotation_no },
                            new SqlParameter("@other", SqlDbType.VarChar,200) { Value = data.other },
                            //quotation_no: quotation_no,
                            //other: other,
                        };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_report_quotation_summary_design", arrParm.ToArray());
                        var i = 2;
                        foreach (DataRow row in dsResult.Tables[0].Rows)
                        {
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.COUNT_CALL].Value = i - 1;
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.QUOTATION_NO].Value = row["quotation_no"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.ISSUE_DD].Value = row["quotation_date_dd"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.ISSUE_MM].Value = row["quotation_date_mm"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.ISSUE_YY].Value = row["quotation_date_yy"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.CUSTOMER_NAME].Value = row["customer_name"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.PROJECT_NAME].Value = row["project_name"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.SUbJECT].Value = row["quotation_subject"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.DESCRIPTION].Value = "";
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.SALES].Value = row["created_by"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.PRICE_LIST].Value = row["total_amount"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.DISCOUNT].Value = row["total_discount"].ToString();
                               workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.FINAL_PRICE].Value = row["grand_total"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.CONTACT_PERSON].Value = row["contact_name"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.QUSTATUS].Value = row["status_name"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.UPDATE_DD].Value = row["updated_date_dd"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.UPDATE_MM].Value = row["updated_date_mm"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.UPDATE_YY].Value = row["updated_date_yy"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.PO_NO].Value = "";
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.PODATE].Value = "";
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.INVOICE].Value = "";
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Design.TEL].Value = row["customer_tel"].ToString();

                            intStartRow++; i++;
                        }
                    }
                }

                templateSheet = excel.Workbook.Worksheets["Sheet1"];
                excel.Workbook.Worksheets.Delete(templateSheet);
                //strDownloadUrl = baseUrl + strTemplatePath + strProductTemplateFile;
                string pathTemp = Path.GetTempFileName();
                excel.SaveAs(new FileInfo(pathTemp));

                strDownloadUrl = string.Format("LocalPath={0}&FileName={1}", pathTemp, "QuotationSummaryReport-Design.xlsx");
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
            }
            return strDownloadUrl;
        }
        #endregion

        [WebMethod]
        public static void GetViewGrid(SummaryQuotationDesignData[] dataSummaryQuotationDesign)
        {
            var dsData = new DataSet();
            var summaryQuotationDesignData = new List<SummaryQuotationDesignData>();
            var data = (from t in dataSummaryQuotationDesign select t).FirstOrDefault();
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
                            new SqlParameter("@employee_id", SqlDbType.Int) { Value = Convert.ToInt32(data.employee_id) },
                            new SqlParameter("@quotation_status", SqlDbType.VarChar, 2) { Value = data.quotation_status },
                            new SqlParameter("@quotation_date_from", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.quotation_date_from) },
                            new SqlParameter("@quotation_date_to", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.quotation_date_to) },
                            new SqlParameter("@po_date_from", SqlDbType.VarChar, 20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.po_date_from) },
                            new SqlParameter("@po_date_to", SqlDbType.VarChar, 20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.po_date_to) },
                            new SqlParameter("@quotation_no", SqlDbType.VarChar, 20) { Value = data.quotation_no },
                            new SqlParameter("@other", SqlDbType.VarChar,200) { Value = data.other },
                        };
                        conn.Open();
                        dsData = SqlHelper.ExecuteDataset(conn, "sp_report_quotation_summary_design", arrParm.ToArray());

                        if (dsData.Tables[0].Rows.Count > 0)
                        {
                            foreach (var row in dsData.Tables[0].AsEnumerable())
                            {
                                summaryQuotationDesignData.Add(new SummaryQuotationDesignData()
                                {
                                     id = Convert.ToInt32(row["id"]),
                                     quotation_no = Convert.IsDBNull(row["quotation_no"]) ? string.Empty : Convert.ToString(row["quotation_no"]),
                                     issue_date = Convert.IsDBNull(row["issue_stock_date"]) ? string.Empty : Convert.ToString(row["issue_stock_date"]),
                                     customer_name = Convert.IsDBNull(row["customer_name"]) ? string.Empty : Convert.ToString(row["customer_name"]),
                                     project_name = Convert.IsDBNull(row["project_name"]) ? string.Empty : Convert.ToString(row["project_name"]),
                                   
                                     amount_po  = Convert.IsDBNull(row["amount_po"]) ? 0 : Convert.ToInt32(row["amount_po"]),
                                });
                            }
                        }
                    }
                    HttpContext.Current.Session["SESSION_EXPORT_SUMMARY_QUOTATION_DESIGN"] = summaryQuotationDesignData;
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
            
        }
        protected void gridExportSummaryQuotationProduct_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridView.DataSource = (from t in summaryQuotationDesignData select t).ToList();
            gridView.DataBind();
        }

    }
}