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
    public partial class ExportSummaryQuotationContract : MasterDetailPage
    {
        public class SummaryQuotationContractData
        {
            public int customer_id { get; set; }
            public int customer_group { get; set; }
            public string customer_code { get; set; }
            public string quotation_status { get; set; }
            public string quotation_no { get; set; }
            public string quotation_date { get; set; }
            public string customer_name { get; set; }
            public string project_name { get; set; }
            public string attention { get; set; }
            public string contract_type { get; set; }
            public string contract_no { get; set; }

            public string expire_date_from { get; set; }
            public string expire_date_to { get; set; }
            public string quotation_date_from { get; set; }
            public string quotation_date_to { get; set; }
            public string starting_date { get; set; }
            public string Starting_date_from { get; set; }
            public string Starting_date_to { get; set; }
            public string po_date_from { get; set; }
            public string po_date_to { get; set; }
            public string expire_date { get; set; }
            public string schedule_date_min_mmyy { get; set; }
            public string schedule_date_max_mmyy { get; set; }
        }
        List<SummaryQuotationContractData> summaryQuotationContractData
        {
            get
            {
                if (Session["SESSION_EXPORT_SUMMARY_QUOTATION_CONTRACT"] == null)
                    Session["SESSION_EXPORT_SUMMARY_QUOTATION_CONTRACT"] = new List<SummaryQuotationContractData>();
                return (List<SummaryQuotationContractData>)Session["SESSION_EXPORT_SUMMARY_QUOTATION_CONTRACT"];
            }
            set
            {
                Session["SESSION_EXPORT_SUMMARY_QUOTATION_CONTRACT"] = value;
            }
        }

        private static Excel.Workbook MyBook = null;
        private static Excel.Application MyApp = null;
        private static Excel.Worksheet MySheet = null;
        private static string baseUrl;

        public override string PageName { get { return "SummaryQuotationContractData"; } }
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
                gridView.DataSource = (from t in summaryQuotationContractData select t).ToList();
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
                    SPlanetUtil.BindASPxComboBox(ref cbbCustomer, DataListUtil.DropdownStoreProcedureName.Customer);
                    //SPlanetUtil.BindASPxComboBox(ref cbbEmployee, DataListUtil.DropdownStoreProcedureName.Employee_List);
                    SPlanetUtil.BindASPxComboBox(ref cbbCustomerGroup, DataListUtil.DropdownStoreProcedureName.Customer_Group);


                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@type", SqlDbType.VarChar,2) { Value = "A" },
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
                        cbbCustomerCode.DataSource = dsResult;
                        cbbCustomerCode.TextField = "customer_code";
                        cbbCustomerCode.ValueField = "customer_code";
                        cbbCustomerCode.DataBind();

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
        private enum Column_QuotationSummary_Contract
        {
            NO = 1,
            CONTRACT_NO = 2,
            QUOTATION_No = 3,
            ISSUE_DATE = 4,
            ISSUE_DATE_MM = 5,
            ISSUE_DATE_YY = 6,
            CUSTOMER_CODE = 7,
            CUSTOMER_NAME = 8,
            PROJECT_NAME = 9,
            CONTACT = 10,
            QTY = 11,
            TYPE_OF_CONTRACT = 12,
            STARTIING_DATE = 13,
            QUSTATUS = 14,
            UPDATE_DD = 15,
            UPDATE_MM = 16,
            UPDATE_YY = 17,
            PO_NO = 18,
            PO_DATE = 19,
            AMOUNTPO = 20,
            INVDATE   = 21,
            INVOICE_NO = 22,
            REMARK=23,
        }

        [WebMethod]
        public static string Export_Quotation_Summary_Product(SummaryQuotationContractData[] dataSummaryQuotationContract)
        {
            var strDownloadUrl = "";

            try
            {
                string strTemplatePath = "/Master/Template/";
                string baseUrl = string.Empty;
                String strExcelPath = string.Empty;
                string strSummaryQUTemplateFile = "Template_QuotationSummaryReport-Contract.xlsx";
                int intStartRow = 5;
                DataSet dsResult;


                string excelTemplate = Path.Combine(HttpContext.Current.Server.MapPath(strTemplatePath), strSummaryQUTemplateFile);
                FileInfo templateFile = new FileInfo(excelTemplate);
                ExcelPackage excel = new ExcelPackage(templateFile);
                var templateSheet = excel.Workbook.Worksheets["Sheet1"];
                var workSheet = excel.Workbook.Worksheets.Copy("Sheet1", "SummaryQU");
                workSheet.DeleteRow(intStartRow, 1048576 - intStartRow);

                var data = (from t in dataSummaryQuotationContract select t).FirstOrDefault();
                if (data != null)
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                    {
                        new SqlParameter("@customer_id", SqlDbType.Int) { Value = data.customer_id },
                        new SqlParameter("@customer_code", SqlDbType.VarChar, 50) { Value = data.customer_code },
                        new SqlParameter("@customer_group", SqlDbType.Int) { Value = data.customer_group },
                        new SqlParameter("@contract_no", SqlDbType.VarChar, 50) { Value = data.contract_no },
                        new SqlParameter("@type_of_contract_id", SqlDbType.Int) { Value = data.contract_type },
                        new SqlParameter("@quotation_no", SqlDbType.VarChar, 20) { Value = data.quotation_no },
                        new SqlParameter("@quotation_status", SqlDbType.VarChar,2) { Value = data.quotation_status },
                        new SqlParameter("@quotation_date_from", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.quotation_date_from) },
                        new SqlParameter("@quotation_date_to", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.quotation_date_to)  },
                        new SqlParameter("@po_date_from", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.po_date_from) },
                        new SqlParameter("@po_date_to", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.po_date_to)  },
                        new SqlParameter("@starting_date_from", SqlDbType.VarChar, 20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.Starting_date_from) },
                        new SqlParameter("@starting_date_to", SqlDbType.VarChar, 20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.Starting_date_to) },
                        new SqlParameter("@expire_date_from", SqlDbType.VarChar, 20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.expire_date_from) },
                        new SqlParameter("@expire_date_to", SqlDbType.VarChar, 20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.expire_date_to) },

                    };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_report_quotation_summary_service_contract", arrParm.ToArray());
                        var i = 0;
                        foreach (DataRow row in dsResult.Tables[0].Rows)
                        {
                            i++;
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.NO].Value = i;
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.CONTRACT_NO].Value = row["contract_no"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.QUOTATION_No].Value = row["quotation_no"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.ISSUE_DATE].Value = row["quotation_date_dd"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.ISSUE_DATE_MM].Value = row["quotation_date_mm"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.ISSUE_DATE_YY].Value = row["quotation_date_yy"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.CUSTOMER_CODE].Value = row["customer_code"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.CUSTOMER_NAME].Value = row["customer_name"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.PROJECT_NAME].Value = row["project_name"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.CONTACT].Value = row["attention"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.QTY].Value = row["qty"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.TYPE_OF_CONTRACT].Value = row["remark1"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.STARTIING_DATE].Value = row["remark2"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.QUSTATUS].Value = row["status_name"].ToString();

                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.UPDATE_DD].Value = row["updated_date_dd"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.UPDATE_MM].Value = row["updated_date_mm"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.UPDATE_YY].Value = row["updated_date_yy"].ToString();

                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.PO_NO].Value = row["po_no"].ToString();// row["inv_no"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.PO_DATE].Value = row["po_date"].ToString();// row["po_no"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.AMOUNTPO].Value =  row["total_amount"].ToString();

                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.INVDATE].Value = row["inv_date"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.INVOICE_NO].Value = row["inv_no"].ToString();
                            workSheet.Cells[intStartRow, (int)Column_QuotationSummary_Contract.REMARK].Value = row["remark_status"].ToString();



                            intStartRow++;
                        }
                    }
                }

                templateSheet = excel.Workbook.Worksheets["Sheet1"];
                excel.Workbook.Worksheets.Delete(templateSheet);
                //strDownloadUrl = baseUrl + strTemplatePath + strProductTemplateFile;
                string pathTemp = Path.GetTempFileName();
                excel.SaveAs(new FileInfo(pathTemp));

                strDownloadUrl = string.Format("LocalPath={0}&FileName={1}", pathTemp, "QuotationSummaryReport-Contract.xlsx");
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
            }
            return strDownloadUrl;
        }
        #endregion

        [WebMethod]
        public static void GetViewGrid(SummaryQuotationContractData[] dataSummaryQuotationContract)
        {
            var dsData = new DataSet();
            var summaryQuotationContractData = new List<SummaryQuotationContractData>();
            var data = (from t in dataSummaryQuotationContract select t).FirstOrDefault();
            if (data != null)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                        new SqlParameter("@customer_id", SqlDbType.Int) { Value = data.customer_id },
                        new SqlParameter("@customer_code", SqlDbType.VarChar, 50) { Value = data.customer_code },
                        new SqlParameter("@customer_group", SqlDbType.Int) { Value = data.customer_group },
                        new SqlParameter("@contract_no", SqlDbType.VarChar, 50) { Value = data.contract_no },
                        new SqlParameter("@type_of_contract_id", SqlDbType.Int) { Value = data.contract_type },
                        new SqlParameter("@quotation_no", SqlDbType.VarChar, 20) { Value = data.quotation_no },
                        new SqlParameter("@quotation_status", SqlDbType.VarChar,2) { Value = data.quotation_status },
                        new SqlParameter("@quotation_date_from", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.quotation_date_from) },
                        new SqlParameter("@quotation_date_to", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.quotation_date_to)  },
                        new SqlParameter("@po_date_from", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.po_date_from) },
                        new SqlParameter("@po_date_to", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.po_date_to)  },
                        new SqlParameter("@starting_date_from", SqlDbType.VarChar, 20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.Starting_date_from) },
                        new SqlParameter("@starting_date_to", SqlDbType.VarChar, 20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.Starting_date_to) },
                        new SqlParameter("@expire_date_from", SqlDbType.VarChar, 20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.expire_date_from) },
                        new SqlParameter("@expire_date_to", SqlDbType.VarChar, 20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.expire_date_to) },
                        };
                        conn.Open();
                        dsData = SqlHelper.ExecuteDataset(conn, "sp_report_quotation_summary_service_contract", arrParm.ToArray());

                        if (dsData.Tables[0].Rows.Count > 0)
                        {
                            foreach (var row in dsData.Tables[0].AsEnumerable())
                            {
                                summaryQuotationContractData.Add(new SummaryQuotationContractData()
                                {
                                    contract_no = Convert.IsDBNull(row["contract_no"]) ? string.Empty : Convert.ToString(row["contract_no"]),
                                    quotation_no = Convert.IsDBNull(row["quotation_no"]) ? string.Empty : Convert.ToString(row["quotation_no"]),
                                    quotation_date = Convert.IsDBNull(row["quotation_date"]) ? string.Empty : Convert.ToString(row["quotation_date"]),
                                    customer_name = Convert.IsDBNull(row["customer_name"]) ? string.Empty : Convert.ToString(row["customer_name"]),
                                    project_name = Convert.IsDBNull(row["project_name"]) ? string.Empty : Convert.ToString(row["project_name"]),
                                    attention = Convert.IsDBNull(row["attention"]) ? string.Empty : Convert.ToString(row["attention"]),
                                    contract_type = Convert.IsDBNull(row["contract_type"]) ? string.Empty : Convert.ToString(row["contract_type"]),
                                    //starting_date = Convert.IsDBNull(row["starting_date"]) ? string.Empty : Convert.ToString(row["starting_date"]),
                                    //expire_date = Convert.IsDBNull(row["expire_date"]) ? string.Empty : Convert.ToString(row["expire_date"]),
                                    //schedule_date_min_mmyy = Convert.IsDBNull(row["schedule_date_min_mmyy"]) ? string.Empty : Convert.ToString(row["schedule_date_min_mmyy"]),
                                    //schedule_date_max_mmyy = Convert.IsDBNull(row["schedule_date_max_mmyy"]) ? string.Empty : Convert.ToString(row["schedule_date_max_mmyy"]),
                                });
                            }
                        }
                    }
                    HttpContext.Current.Session["SESSION_EXPORT_SUMMARY_QUOTATION_CONTRACT"] = summaryQuotationContractData;
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }


        }
        protected void gridExportSummaryQuotationContract_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridView.DataSource = (from t in summaryQuotationContractData select t).ToList();
            gridView.DataBind();
        }
    }
}