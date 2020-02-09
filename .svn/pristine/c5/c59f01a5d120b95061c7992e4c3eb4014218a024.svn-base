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
    public partial class ExportSummaryQuotationSparePart : MasterDetailPage
    {
        public class SummaryQuotationSparePartData
        {
            public int customer_id { get; set; }
            public int cat_id { get; set; }
            public int shelf_id { get; set; }
            public string quotation_status { get; set; }
            public string quotation_date_from { get; set; }
            public string quotation_date_to { get; set; }
            public string quotation_no { get; set; }
            public string customer_name { get; set; }
            public string project_name { get; set; }
            public string attention_name { get; set; }
            public int id { get; set; }
            public int customer_group { get; set; }
            public string quotation { get; set; }
            public string other { get; set; }
            public string po_date_from { get; set; }
            public string po_date_to { get; set; }
            public string model { get; set; }
            public string mfg { get; set; }
        }
        List<SummaryQuotationSparePartData> summaryQuotationSparePartData
        {
            get
            {
                if (Session["SESSION_EXPORT_SUMMARY_QUOTATION_SPAREPART"] == null)
                    Session["SESSION_EXPORT_SUMMARY_QUOTATION_SPAREPART"] = new List<SummaryQuotationSparePartData>();
                return (List<SummaryQuotationSparePartData>)Session["SESSION_EXPORT_SUMMARY_QUOTATION_SPAREPART"];
            }
            set
            {
                Session["SESSION_EXPORT_SUMMARY_QUOTATION_SPAREPART"] = value;
            }
        }

        private static Excel.Workbook MyBook = null;
        private static Excel.Application MyApp = null;
        private static Excel.Worksheet MySheet = null;
        private static string baseUrl;

        public override string PageName { get { return "SummaryQuotationPSparePartData"; } }
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
                //BindGrid(false);
                gridView.DataSource = (from t in summaryQuotationSparePartData select t).ToList();
                gridView.DataBind();
            }
        }
        protected void ClearWoringSession()
        {
            Session.Remove("SESSION_EXPORT_SUMMARY_QUOTATION_SPAREPART");
        }

        protected void PrepareData()
        {
            try
            {
                if (!Page.IsPostBack)
                {
                   
                    SPlanetUtil.BindASPxComboBox(ref cbbShelf, DataListUtil.DropdownStoreProcedureName.Shelf_List);
                    SPlanetUtil.BindASPxComboBox(ref cbbCustomerGroup, DataListUtil.DropdownStoreProcedureName.Customer_Group);

                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@type", SqlDbType.VarChar,2) { Value = "MSC" },
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
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@lang_id", SqlDbType.VarChar,3) { Value = "tha" },
                            new SqlParameter("@cat_type", SqlDbType.Int) { Value = "SP" },
                        };
                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_product_category_export", arrParm.ToArray());

                        conn.Close();
                        cbbProductCat.DataSource = dsResult;
                        cbbProductCat.TextField = "data_text";
                        cbbProductCat.ValueField = "data_value";
                        cbbProductCat.DataBind();

                    }

                  

                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {

                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = 0 },
                        };
                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_mfg_list", arrParm.ToArray());

                        conn.Close();
                        cbbModel.DataSource = dsResult;
                        cbbModel.TextField = "model";
                        cbbModel.ValueField = "model";
                        cbbModel.DataBind();

                        cbbMfg.DataSource = dsResult;
                        cbbMfg.TextField = "mfg";
                        cbbMfg.ValueField = "mfg";
                        cbbMfg.DataBind();

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
        private enum Column_QuotationSummary_SparePart
        {
            COUNT_CALL = 1,
            QUOTATION_NO = 2,
            QUOTATION_DATE_DD = 3,
            QUOTATION_DATE_MM = 4,
            QUOTATION_DATE_YY = 5,
            CUSTOMER_GROUP = 6,
            CUSTOMER_NAME = 7,
            LOCATION_BILL = 8,
            PROJECT_NAME = 9,
            LOCATION_SETTING1 = 10,
            LOCATION_SETTING2 = 11,
            MODEL = 12,
            MFG = 13,
            MACHINE = 14,
            PRESSURE = 15,
            QUOTATION_TYPE = 16,//S-C
            DETAIL_OF_PART = 17,
            DISCOUNT = 18,
            TOTAL_AMOUNT = 19,
            ATTENTION_NAME = 20,
            QUOTATION_STATUS = 21,
            PO_DATE_DD = 22,
            PO_DATE_MM = 23,
            PO_DATE_YY = 24,
            PO_NO = 25,
            PO_AMOUNT = 26,
            SALE_ORDER_NO = 27,
            DELIVERY_DATE = 28,
            INV = 29,
            STATUS = 30,
            REPORT_NOREPORT = 31,
            ORDER = 32,
            REMARK_STATUS = 33,
            REF_RETURN_NO = 34,
            RETURN_DETAIL = 35,
            RETURN_AMOUNT = 36,
        }
        [WebMethod]
        public static string Export_Quotation_Summary_SparePart(SummaryQuotationSparePartData[] dataSummaryQuotationSparePart)
        {
            string strDownloadUrl = string.Empty;

            try
            {
                string strTemplatePath = "/Master/Template/";
                string baseUrl = string.Empty;
                String strExcelPath = string.Empty;
                string strSummaryQUTemplateFile = "Template_QuotationSummaryReport_SparePart.xlsx";
                int intStartRow = 7;
                DataSet dsResult;


                string excelTemplate = Path.Combine(HttpContext.Current.Server.MapPath(strTemplatePath), strSummaryQUTemplateFile);
                FileInfo templateFile = new FileInfo(excelTemplate);
                ExcelPackage excel = new ExcelPackage(templateFile);
                var templateSheet = excel.Workbook.Worksheets["Sheet1"];
                var workSheet = excel.Workbook.Worksheets.Copy("Sheet1", "SummaryQU");
                workSheet.DeleteRow(intStartRow, 1048576 - intStartRow);
                var data = (from t in dataSummaryQuotationSparePart select t).FirstOrDefault();
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                    {
                        new SqlParameter("@customer_id", SqlDbType.Int) { Value = Convert.ToInt32(data.customer_id) },
                        new SqlParameter("@cat_id", SqlDbType.Int) { Value =   Convert.ToInt32(data.cat_id) },
                        new SqlParameter("@shelf_id", SqlDbType.Int) { Value = Convert.ToInt32(data.shelf_id) },
                        new SqlParameter("@quotation_status", SqlDbType.VarChar,2) { Value = data.quotation_status },
                        new SqlParameter("@customer_group", SqlDbType.Int) { Value = Convert.ToInt32(data.customer_group) },
                        new SqlParameter("@quotation_no", SqlDbType.VarChar,20) { Value = data.quotation },
                        new SqlParameter("@model", SqlDbType.VarChar,20) { Value = data.model },
                        new SqlParameter("@mfg", SqlDbType.VarChar,50) { Value = data.mfg },
                        new SqlParameter("@quotation_date_from", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.quotation_date_from) },
                        new SqlParameter("@quotation_date_to", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.quotation_date_to) },
                        new SqlParameter("@po_date_from", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.po_date_from) },
                        new SqlParameter("@po_date_to", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.po_date_to) },
                        new SqlParameter("@other", SqlDbType.VarChar,200) { Value = data.other },
                    };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_report_quotation_summary_spare_part", arrParm.ToArray());
                    var i = 0;
                    foreach (DataRow row in dsResult.Tables[0].Rows)
                    {
                        i++;
                        templateSheet.SelectedRange["A7:XFA7"].Copy(workSheet.SelectedRange[intStartRow, 1]);
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.COUNT_CALL].Value = i;
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.QUOTATION_NO].Value = row["quotation_no"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.QUOTATION_DATE_DD].Value = row["quotation_date_dd"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.QUOTATION_DATE_MM].Value = row["quotation_date_mm"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.QUOTATION_DATE_YY].Value = row["quotation_date_yy"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.CUSTOMER_GROUP].Value = row["group_name"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.CUSTOMER_NAME].Value = row["customer_name"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.LOCATION_BILL].Value = row["location_bill"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.PROJECT_NAME].Value = row["project_name"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.LOCATION_SETTING1].Value = row["location_setting"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.LOCATION_SETTING2].Value = row["location_setting2"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.MODEL].Value = row["model"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.MFG].Value = row["mfg"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.MACHINE].Value = row["machine"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.PRESSURE].Value = row["pressure"];
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.QUOTATION_TYPE].Value = row["quotation_type"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.DETAIL_OF_PART].Value = row["detail_of_part"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.DISCOUNT].Value = row["discount"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.TOTAL_AMOUNT].Value = row["total_amount"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.ATTENTION_NAME].Value = row["attention_name"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.QUOTATION_STATUS].Value = row["quotation_status"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.PO_DATE_DD].Value = row["po_date_dd"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.PO_DATE_MM].Value = row["po_date_mm"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.PO_DATE_YY].Value = row["po_date_yy"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.PO_NO].Value = row["po_no"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.PO_AMOUNT].Value = row["po_amount"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.SALE_ORDER_NO].Value = row["sale_order_no"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.DELIVERY_DATE].Value = row["delivery_date"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.INV].Value = row["inv_no"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.STATUS].Value = row["status_all"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.REPORT_NOREPORT].Value = row["report"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.ORDER].Value = row["order_hicom"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.REMARK_STATUS].Value = row["remark_status"].ToString();

                        intStartRow++;
                    }
                }

                templateSheet = excel.Workbook.Worksheets["Sheet1"];
                excel.Workbook.Worksheets.Delete(templateSheet);
                //strDownloadUrl = baseUrl + strTemplatePath + strProductTemplateFile;
                string pathTemp = Path.GetTempFileName();
                excel.SaveAs(new FileInfo(pathTemp));

                strDownloadUrl = string.Format("LocalPath={0}&FileName={1}", pathTemp, "QuotationSummaryReport_SparePart.xlsx");
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
            }
            return strDownloadUrl;
        }
        #endregion

        [WebMethod]
        public static void GetViewGrid(SummaryQuotationSparePartData[] dataSummaryQuotationSparePart)
        {
            var dsData = new DataSet();
            var selectSummaryQuotationSparePartData = new List<SummaryQuotationSparePartData>();
            var data = (from t in dataSummaryQuotationSparePart select t).FirstOrDefault();
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
                             new SqlParameter("@cat_id", SqlDbType.Int) { Value =   Convert.ToInt32(data.cat_id) },
                             new SqlParameter("@shelf_id", SqlDbType.Int) { Value = Convert.ToInt32(data.shelf_id) },
                             new SqlParameter("@quotation_status", SqlDbType.VarChar,2) { Value = data.quotation_status },
                             new SqlParameter("@customer_group", SqlDbType.Int) { Value = Convert.ToInt32(data.customer_group) },
                             new SqlParameter("@quotation_no", SqlDbType.VarChar,20) { Value = data.quotation },
                             new SqlParameter("@model", SqlDbType.VarChar,20) { Value = data.model },
                             new SqlParameter("@mfg", SqlDbType.VarChar,50) { Value = data.mfg },
                             new SqlParameter("@quotation_date_from", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.quotation_date_from) },
                             new SqlParameter("@quotation_date_to", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.quotation_date_to) },
                             new SqlParameter("@po_date_from", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.po_date_from) },
                             new SqlParameter("@po_date_to", SqlDbType.VarChar,20) { Value = SPlanetUtil.Convert_ddmmyyyy_to_yyyymmdd(data.po_date_to) },
                             new SqlParameter("@other", SqlDbType.VarChar,200) { Value = data.other },

                        };
                        conn.Open();
                        dsData = SqlHelper.ExecuteDataset(conn, "sp_report_quotation_summary_spare_part", arrParm.ToArray());

                        if (dsData.Tables[0].Rows.Count > 0)
                        {
                            foreach (var row in dsData.Tables[0].AsEnumerable())
                            {
                                selectSummaryQuotationSparePartData.Add(new SummaryQuotationSparePartData()
                                {
                                    id = Convert.ToInt32(row["id"]),
                                    quotation_no = Convert.IsDBNull(row["quotation_no"]) ? string.Empty : Convert.ToString(row["quotation_no"]),
                                    customer_name = Convert.IsDBNull(row["customer_name"]) ? string.Empty : Convert.ToString(row["customer_name"]),
                                    project_name = Convert.IsDBNull(row["project_name"]) ? string.Empty : Convert.ToString(row["project_name"]),
                                    attention_name = Convert.IsDBNull(row["attention_name"]) ? string.Empty : Convert.ToString(row["attention_name"]),
                                });
                            }
                        }
                    }
                    HttpContext.Current.Session["SESSION_EXPORT_SUMMARY_QUOTATION_SPAREPART"] = selectSummaryQuotationSparePartData;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


        }
        protected void gridExportSummaryQuotationSparePart_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridView.DataSource = (from t in summaryQuotationSparePartData select t).ToList();
            gridView.DataBind();
        }
    }
}