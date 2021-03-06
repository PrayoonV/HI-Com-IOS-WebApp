﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DevExpress.Web;
using HicomIOS.ClassUtil;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.ApplicationBlocks.Data;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Services;
using System.IO;
using OfficeOpenXml;

namespace HicomIOS.Master
{
    public partial class AnnualServiceList : MasterDetailPage
    {
        public int dataId = 0;
        public override string PageName { get { return "List Annual Service"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }
        public override void OnFilterChanged()
        {

        }
        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            return result.OfType<FilterControlColumn>();
        }
        public class AnnualServiceListData
        {
            public string datepickerFrom { get; set; }
            public string datepickerTo { get; set; }
            public string starttingdateFrom { get; set; }
            public string starttingdateTo { get; set; }
            public string scheduleFrom { get; set; }
            public string scheduleTo { get; set; }
            public string customerid { get; set; }
            public string project_search { get; set; }


            public string expiredateFrom { get; set; }
            public string expiredateTo { get; set; }
            public string customer { get; set; }
            public string model { get; set; }
            public int id { get; set; }
            public string mfg { get; set; }
            public decimal service_fee { get; set; }
            public string project { get; set; }
            public string start_date { get; set; }
            public string starting_date { get; set; }
            public string expire_date { get; set; }

            public string schedule_date { get; set; }

            public string checking_location { get; set; }
            public string checking_remark { get; set; }
        }
        List<AnnualServiceListData> annualServiceListData
        {
            get
            {
                if (Session["SESSION_ANNUALSERVICELIST_DATA"] == null)
                    Session["SESSION_ANNUALSERVICELIST_DATA"] = new List<AnnualServiceListData>();
                return (List<AnnualServiceListData>)Session["SESSION_ANNUALSERVICELIST_DATA"];
            }
            set
            {
                Session["SESSION_ANNUALSERVICELIST_DATA"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
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

                    cbbProject.Items.Insert(0, new ListEditItem("ทั้งหมด", ""));
                    cbbProject.SelectedIndex = 0;

                }
                // Get Permission and if no permission, will redirect to another page.
                if (!Permission.GetPermission())
                    Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

            }
            else
            {
                gridAnnualServiceList.DataSource = (from t in annualServiceListData select t).ToList(); 
                gridAnnualServiceList.FilterExpression = FilterBag.GetExpression(false);
                gridAnnualServiceList.DataBind();
            }
        }

        protected void cbbProject_Callback(object sender, CallbackEventArgsBase e)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                {
                    new SqlParameter("@lang_id", SqlDbType.VarChar, 3) { Value = "tha" },
                    new SqlParameter("@customer_id", SqlDbType.Int) { Value = cbbCustomer.Value },
                };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_annual_customer_project", arrParm.ToArray());

                conn.Close();
                if (dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        cbbProject.DataSource = dsResult;
                        cbbProject.TextField = "data_text";
                        cbbProject.ValueField = "data_text";
                        cbbProject.DataBind();

                        cbbProject.Items.Insert(0, new ListEditItem("ทั้งหมด", ""));
                        cbbProject.SelectedIndex = 0;
                    }
                }
            }
        }


        [WebMethod]
        public static void GetViewGrid(AnnualServiceListData[] masterData)
        {
            var dsData = new DataSet();
            var AnnualServiceListData = new List<AnnualServiceListData>();
            var datalist = (from t in masterData select t).FirstOrDefault();
            if (datalist != null)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                        new SqlParameter("@start_up_date_from", SqlDbType.DateTime) { Value = (string.IsNullOrEmpty(datalist.datepickerFrom) ? (Object)DBNull.Value : DateTime.ParseExact(datalist.datepickerFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture)) },
                        new SqlParameter("@start_up_date_to", SqlDbType.DateTime) { Value = (string.IsNullOrEmpty(datalist.datepickerTo) ? (Object)DBNull.Value : DateTime.ParseExact(datalist.datepickerTo, "dd/MM/yyyy", CultureInfo.InvariantCulture)) },
                        new SqlParameter("@starting_date_from", SqlDbType.DateTime) { Value = (string.IsNullOrEmpty(datalist.starttingdateFrom) ? (Object)DBNull.Value : DateTime.ParseExact(datalist.starttingdateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture)) },
                        new SqlParameter("@starting_date_to", SqlDbType.DateTime) { Value = (string.IsNullOrEmpty(datalist.starttingdateTo) ? (Object)DBNull.Value : DateTime.ParseExact(datalist.starttingdateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture)) },
                        new SqlParameter("@expire_date_from", SqlDbType.DateTime) { Value = (string.IsNullOrEmpty(datalist.expiredateFrom) ? (Object)DBNull.Value : DateTime.ParseExact(datalist.expiredateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture)) },
                        new SqlParameter("@expire_date_to", SqlDbType.DateTime) { Value = (string.IsNullOrEmpty(datalist.expiredateTo) ? (Object)DBNull.Value : DateTime.ParseExact(datalist.expiredateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture)) },
                        new SqlParameter("@schedule_date_from", SqlDbType.DateTime) { Value = (string.IsNullOrEmpty(datalist.scheduleFrom) ? (Object)DBNull.Value : DateTime.ParseExact(datalist.scheduleFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture)) },
                        new SqlParameter("@schedule_date_to", SqlDbType.DateTime) { Value = (string.IsNullOrEmpty(datalist.scheduleTo) ? (Object)DBNull.Value : DateTime.ParseExact(datalist.scheduleTo, "dd/MM/yyyy", CultureInfo.InvariantCulture)) },
                        new SqlParameter("@customer_id", SqlDbType.Int) { Value = (string.IsNullOrEmpty(datalist.customerid) ? (Object)DBNull.Value : Convert.ToInt32(datalist.customerid)) },
                        new SqlParameter("@projecT_s", SqlDbType.VarChar,200) { Value = (string.IsNullOrEmpty(datalist.project_search) ? (Object)DBNull.Value : datalist.project_search) },

                    };
                        conn.Open();
                        dsData = SqlHelper.ExecuteDataset(conn, "sp_annual_service_list", arrParm.ToArray());

                        if (dsData.Tables[0].Rows.Count > 0)
                        {
                            foreach (var row in dsData.Tables[0].AsEnumerable())
                            {
                                AnnualServiceListData.Add(new AnnualServiceListData()
                                {

                                    id = Convert.ToInt32(row["customer_id"]),
                                    mfg = Convert.IsDBNull(row["mfg"]) ? string.Empty : Convert.ToString(row["mfg"]),
                                    start_date = Convert.IsDBNull(row["start_up_date"]) ? string.Empty : Convert.ToString(row["start_up_date"]),
                                    project = Convert.IsDBNull(row["project"]) ? string.Empty : Convert.ToString(row["project"]),
                                    service_fee = Convert.IsDBNull(row["service_fee"]) ? 0 : Convert.ToDecimal(row["service_fee"]),
                                    starting_date = Convert.IsDBNull(row["starting_date"]) ? string.Empty : Convert.ToString(row["starting_date"]),
                                    expire_date = Convert.IsDBNull(row["expire_date"]) ? string.Empty : Convert.ToString(row["expire_date"]),
                                    schedule_date = Convert.IsDBNull(row["schedule_date"]) ? string.Empty : Convert.ToString(row["schedule_date"]),
                                    customer = Convert.IsDBNull(row["cust_name"]) ? string.Empty : Convert.ToString(row["cust_name"]),
                                    model = Convert.IsDBNull(row["model"]) ? string.Empty : Convert.ToString(row["model"]),
                                    checking_location = Convert.IsDBNull(row["checking_location"]) ? string.Empty : Convert.ToString(row["checking_location"]),
                                    checking_remark = Convert.IsDBNull(row["checking_remark"]) ? string.Empty : Convert.ToString(row["checking_remark"]),
                                });
                            }
                        }
                    }
                    HttpContext.Current.Session["SESSION_ANNUALSERVICELIST_DATA"] = AnnualServiceListData;
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }


        }

        protected void gridAnnualServiceList_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridAnnualServiceList.DataSource = (from t in annualServiceListData select t).ToList();
            gridAnnualServiceList.DataBind();
        }

        private enum Column_AnnualServiceList
        {
            COUNT_CALL = 1,

            CUSTOMER_NAME = 2,
            PROJECT = 3,
            MODEL = 4,
            MFG = 5,
            SCHEDULE_DATE = 6,

            TEST_RUN_DATE = 7,
            STARTING_DATE = 8,
            EXPIRE_DATE = 9,

            REMARK = 12,
            LOCATION = 11,

        }

        [WebMethod]
        public static string ExportAnnualServiceList(AnnualServiceListData[] annualServiceListData)
        {
            string strDownloadUrl = string.Empty;

            try
            {
                string strTemplatePath = "/Master/Template/";
                string baseUrl = string.Empty;
                String strExcelPath = string.Empty;
                string strSummaryQUTemplateFile = "AnnualService-Template.xlsx";
                int intStartRow = 2;
                DataSet dsResult;


                string excelTemplate = Path.Combine(HttpContext.Current.Server.MapPath(strTemplatePath), strSummaryQUTemplateFile);
                FileInfo templateFile = new FileInfo(excelTemplate);
                ExcelPackage excel = new ExcelPackage(templateFile);
                var templateSheet = excel.Workbook.Worksheets["Sheet1"];
                var workSheet = excel.Workbook.Worksheets.Copy("Sheet1", "AnnualService");
                workSheet.DeleteRow(5, 1048576 - 5);
                var data = (from t in annualServiceListData select t).FirstOrDefault();
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    /*List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                        new SqlParameter("@start_up_date_from", SqlDbType.VarChar,200) { Value = data.datepickerFrom },
                        new SqlParameter("@start_up_date_to", SqlDbType.VarChar,200) { Value = data.datepickerTo  },
                        new SqlParameter("@starting_date_from", SqlDbType.VarChar,200) { Value = data.starttingdateFrom },
                        new SqlParameter("@starting_date_to", SqlDbType.VarChar, 200) { Value = data.starttingdateTo },
                        new SqlParameter("@expire_date_from", SqlDbType.VarChar,200) { Value = data.expiredateFrom },
                        new SqlParameter("@expire_date_to", SqlDbType.VarChar, 200) { Value = data.expiredateTo },
                        new SqlParameter("@schedule_date_from", SqlDbType.VarChar,200) { Value = data.scheduleFrom },
                        new SqlParameter("@schedule_date_to", SqlDbType.VarChar, 200) { Value = data.scheduleTo },

                        };*/
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                        new SqlParameter("@start_up_date_from", SqlDbType.DateTime) { Value = (string.IsNullOrEmpty(data.datepickerFrom) ? (Object)DBNull.Value : DateTime.ParseExact(data.datepickerFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture)) },
                        new SqlParameter("@start_up_date_to", SqlDbType.DateTime) { Value = (string.IsNullOrEmpty(data.datepickerTo) ? (Object)DBNull.Value : DateTime.ParseExact(data.datepickerTo, "dd/MM/yyyy", CultureInfo.InvariantCulture)) },
                        new SqlParameter("@starting_date_from", SqlDbType.DateTime) { Value = (string.IsNullOrEmpty(data.starttingdateFrom) ? (Object)DBNull.Value : DateTime.ParseExact(data.starttingdateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture)) },
                        new SqlParameter("@starting_date_to", SqlDbType.DateTime) { Value = (string.IsNullOrEmpty(data.starttingdateTo) ? (Object)DBNull.Value : DateTime.ParseExact(data.starttingdateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture)) },
                        new SqlParameter("@expire_date_from", SqlDbType.DateTime) { Value = (string.IsNullOrEmpty(data.expiredateFrom) ? (Object)DBNull.Value : DateTime.ParseExact(data.expiredateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture)) },
                        new SqlParameter("@expire_date_to", SqlDbType.DateTime) { Value = (string.IsNullOrEmpty(data.expiredateTo) ? (Object)DBNull.Value : DateTime.ParseExact(data.expiredateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture)) },
                        new SqlParameter("@schedule_date_from", SqlDbType.DateTime) { Value = (string.IsNullOrEmpty(data.scheduleFrom) ? (Object)DBNull.Value : DateTime.ParseExact(data.scheduleFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture)) },
                        new SqlParameter("@schedule_date_to", SqlDbType.DateTime) { Value = (string.IsNullOrEmpty(data.scheduleTo) ? (Object)DBNull.Value : DateTime.ParseExact(data.scheduleTo, "dd/MM/yyyy", CultureInfo.InvariantCulture)) },
                        new SqlParameter("@customer_id", SqlDbType.Int) { Value = (string.IsNullOrEmpty(data.customerid) ? (Object)DBNull.Value : Convert.ToInt32(data.customerid)) },
                        new SqlParameter("@projecT_s", SqlDbType.VarChar,200) { Value = (string.IsNullOrEmpty(data.project_search) ? (Object)DBNull.Value : data.project_search) },
                    };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_annual_service_list", arrParm.ToArray());
                    var i = 0;
                    foreach (DataRow row in dsResult.Tables[0].Rows)
                    {
                        i++;
                        templateSheet.SelectedRange["A5:XFA5"].Copy(workSheet.SelectedRange[intStartRow, 1]);

                        workSheet.Cells[intStartRow, (int)Column_AnnualServiceList.COUNT_CALL].Value = i;
                        workSheet.Cells[intStartRow, (int)Column_AnnualServiceList.CUSTOMER_NAME].Value = row["cust_name"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_AnnualServiceList.PROJECT].Value = row["project"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_AnnualServiceList.MODEL].Value = row["model"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_AnnualServiceList.MFG].Value = row["mfg"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_AnnualServiceList.SCHEDULE_DATE].Value = row["schedule_date"].ToString();

                        workSheet.Cells[intStartRow, (int)Column_AnnualServiceList.TEST_RUN_DATE].Value = row["test_run_date"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_AnnualServiceList.STARTING_DATE].Value = row["starting_date"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_AnnualServiceList.EXPIRE_DATE].Value = row["expire_date"].ToString();

                        workSheet.Cells[intStartRow, (int)Column_AnnualServiceList.LOCATION].Value = row["checking_location"].ToString();
                        workSheet.Cells[intStartRow, (int)Column_AnnualServiceList.REMARK].Value = row["checking_remark"].ToString();

                        intStartRow++;
                    }
                }

                templateSheet = excel.Workbook.Worksheets["Sheet1"];
                excel.Workbook.Worksheets.Delete(templateSheet);
                strDownloadUrl = baseUrl + strTemplatePath + "AnnualService.xlsx";
                strDownloadUrl = Path.Combine(HttpContext.Current.Server.MapPath(strTemplatePath), "AnnualService.xlsx");
                string pathTemp = Path.GetTempFileName();
                excel.SaveAs(new FileInfo(strDownloadUrl));
                //excel.SaveAs(new FileInfo(pathTemp));

                strDownloadUrl = string.Format("LocalPath={0}&FileName={1}", strDownloadUrl, "AnnualService.xlsx");

            }


            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
            }
            return strDownloadUrl;
        }

        protected void test_Click(object sender, EventArgs e)
        {
            string asdstrTemplatePath = "/Master/Template/";
            string asdstrTemplatePath7 = "/Master/Template/";
            string baseUrl = string.Empty;
            String strExcelPath = string.Empty;
            string strSummaryQUTemplateFile = "AnnualService-Template.xlsx";
            int intStartRow = 2;
            DataSet dsResult;


            string excelTemplate = Path.Combine(HttpContext.Current.Server.MapPath(asdstrTemplatePath), strSummaryQUTemplateFile);
            FileInfo templateFile = new FileInfo(excelTemplate);
            ExcelPackage excel = new ExcelPackage(templateFile);
            var templateSheet = excel.Workbook.Worksheets["Sheet1"];
            var workSheet = excel.Workbook.Worksheets.Copy("Sheet1", "AnnualService");
            workSheet.DeleteRow(5, 1048576 - 5);
            var data = (from t in annualServiceListData select t).FirstOrDefault();

            templateSheet = excel.Workbook.Worksheets["Sheet1"];
            excel.Workbook.Worksheets.Delete(templateSheet);
            asdstrTemplatePath = baseUrl + asdstrTemplatePath + "AnnualService.xlsx";
            asdstrTemplatePath = Path.Combine(HttpContext.Current.Server.MapPath(asdstrTemplatePath7), "AnnualService.xlsx");
            string pathTemp = Path.GetTempFileName();
            excel.SaveAs(new FileInfo(asdstrTemplatePath));
            //excel.SaveAs(new FileInfo(pathTemp));

            Response.ContentType = "application/ms-excel";
            //Response.ContentType = @"application/vnd.ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + "AnnualService.xlsx");
            Response.TransmitFile(asdstrTemplatePath);
            Response.End();
        }
    }
}