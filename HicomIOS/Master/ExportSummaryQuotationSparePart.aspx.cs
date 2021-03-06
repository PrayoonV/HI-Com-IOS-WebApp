﻿using System;
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
            public string producttype { get; set; }
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
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
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
                    var dtSource1 = new DataTable();
                    dtSource1.Columns.Add("data_value", typeof(string));
                    dtSource1.Columns.Add("data_text", typeof(string));
                    dtSource1.Rows.Add("S", "Spare Part");
                    dtSource1.Rows.Add("M", "Maintenance");
                    dtSource1.Rows.Add("C", "Service Charge");

                    cbbproducttype.DataSource = dtSource1;
                    cbbproducttype.DataBind();

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
            CUSTOMER_NAME = 6,
            PROJECT_NAME = 7,
            Subject = 8,
            MODEL = 9,
            MFG = 10,
            Hour = 11,
            MACHINE = 12,
            PRESSURE = 13,
            QUOTATION_TYPE = 14,//S-C
            DETAIL_OF_PART = 15,
            DISCOUNT = 16,
            TOTAL_AMOUNT = 17,
            ATTENTION_NAME = 18,
            QUOTATION_STATUS = 19,
            UPDATE_DD = 20,
            UPDATE_MM = 21,
            UPDATE_YY = 22,
            PO_NO = 23,
            PO_DATE = 24,
            REMARK= 25,
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
                        new SqlParameter("@producttype", SqlDbType.VarChar,200) { Value = data.producttype },
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
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.QUOTATION_DATE_DD].Value = row["created_date_dd"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.QUOTATION_DATE_MM].Value = row["created_date_mm"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.QUOTATION_DATE_YY].Value = row["created_date_yy"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.CUSTOMER_NAME].Value = row["customer_name"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.PROJECT_NAME].Value = row["project_name"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.Subject].Value = row["quotation_subject"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.MODEL].Value = row["model"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.MFG].Value = row["mfg"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.Hour].Value = row["hour_amount"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.MACHINE].Value = row["machine"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.PRESSURE].Value = row["pressure"];
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.QUOTATION_TYPE].Value = row["quotation_type"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.DETAIL_OF_PART].Value = row["detail_of_part"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.DISCOUNT].Value = row["discount"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.TOTAL_AMOUNT].Value = row["total_amount"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.ATTENTION_NAME].Value = row["attention_name"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.QUOTATION_STATUS].Value = row["quotation_status"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.UPDATE_DD].Value = row["updated_date_dd"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.UPDATE_MM].Value = row["updated_date_mm"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.UPDATE_YY].Value = row["updated_date_yy"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.PO_NO].Value = row["po_no"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.PO_DATE].Value = row["ref_po_date"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_QuotationSummary_SparePart.REMARK].Value = row["remark_status"].ToString();

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
                             new SqlParameter("@producttype", SqlDbType.VarChar,200) { Value = data.producttype },

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