﻿using DevExpress.Web;
using HicomIOS.ClassUtil;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HicomIOS.Master
{
    public partial class Customer : MasterDetailPage
    {
        private DataSet dsResult;
        private DataTable dtProvince = new DataTable();
        private DataTable dtAmphur = new DataTable();
        private DataTable dtDistrict = new DataTable();

        public class CustomerList
        {
            public int id { get; set; }
            public string customer_code { get; set; }
            public string company_name_tha { get; set; }
            public string company_name_eng { get; set; }
            public string customer_name_other { get; set; }
            public string address_bill_tha { get; set; }
            public string address_bill_eng { get; set; }
            public string address_install_tha { get; set; }
            public string address_install_eng { get; set; }
            public int customer_group_id { get; set; }
            public int customer_business_id { get; set; }
            public int customer_industry_id { get; set; }
            public string tax_id { get; set; }
            public string branch_name { get; set; }
            public string branch_code { get; set; }
            public int geo_id { get; set; }
            public int province_id { get; set; }
            public int amphur_id { get; set; }
            public int district_id { get; set; }
            public string zipcode { get; set; }
            public string tel { get; set; }
            public string mobile { get; set; }
            public string fax { get; set; }
            public string first_contact { get; set; }
            public decimal vat { get; set; }
            public int credit_term { get; set; }
            public string bill_date { get; set; }
            public string remark { get; set; }
            public bool is_enable { get; set; }
            public bool is_delete { get; set; }
            public int created_by { get; set; }
            public DateTime created_date { get; set; }
            public int updated_by { get; set; }
            public DateTime updated_date { get; set; }
        }


        public class CustomerAttention
        {
            public int id { get; set; }
            public int customer_id { get; set; }
            public string attention_name { get; set; }
            public string attention_tel { get; set; }
            public string attention_email { get; set; }
            public bool is_delete { get; set; }
        }
        public class CustomerMFG
        {
            public int id { get; set; }
            public int customer_id { get; set; }
            public int product_id { get; set; }
            public string model { get; set; }
            public string project { get; set; }
            public string unitwarraty { get; set; }
            public string airendwarraty { get; set; }
            public decimal service_fee { get; set; }
            public bool is_demo { get; set; }
            public bool is_delete { get; set; }
            public string mfg { get; set; }
            public string date { get; set; }
            public string pressure { get; set; }
            public int hz { get; set; }
            public string power_supply { get; set; }
            public int phase { get; set; }
        }
        List<CustomerAttention> customerAttentionList
        {
            get
            {
                if (Session["SESSION_CUSTOMER_ATTENTION_CUSTOMER"] == null)
                    Session["SESSION_CUSTOMER_ATTENTION_CUSTOMER"] = new List<CustomerAttention>();
                return (List<CustomerAttention>)Session["SESSION_CUSTOMER_ATTENTION_CUSTOMER"];
            }
            set
            {
                Session["SESSION_CUSTOMER_ATTENTION_CUSTOMER"] = value;
            }
        }

        List<CustomerMFG> customerMFGList
        {
            get
            {
                if (Session["SESSION_CUSTOMER_MFG_CUSTOMER"] == null)
                    Session["SESSION_CUSTOMER_MFG_CUSTOMER"] = new List<CustomerMFG>();
                return (List<CustomerMFG>)Session["SESSION_CUSTOMER_MFG_CUSTOMER"];
            }
            set
            {
                Session["SESSION_CUSTOMER_MFG_CUSTOMER"] = value;
            }
        }
        List<CustomerMFG> customerMFGListSearch
        {
            get
            {
                if (Session["SESSION_CUSTOMER_MFG_CUSTOMER_Search"] == null)
                    Session["SESSION_CUSTOMER_MFG_CUSTOMER_Search"] = new List<CustomerMFG>();
                return (List<CustomerMFG>)Session["SESSION_CUSTOMER_MFG_CUSTOMER_Search"];
            }
            set
            {
                Session["SESSION_CUSTOMER_MFG_CUSTOMER_Search"] = value;
            }
        }
        public override string PageName { get { return "Customer"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }
        public override long SelectedItemID
        {
            get
            {
                var ItemID = gridView.GetRowValues(gridView.FocusedRowIndex, gridView.KeyFieldName);
                return ItemID != null ? (int)ItemID : DataListUtil.emptyEntryID;
            }
            set
            {
                gridView.FocusedRowIndex = gridView.FindVisibleIndexByKeyValue(value);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Bind Data into GridView
            if (!Page.IsPostBack)
            {
                // Get Permission and if no permission, will redirect to another page.
                if (!Permission.GetPermission())
                    Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

                ClearWorkingSession();
                Session.Remove("SESSION_CUSTOMER_ATTENTION_CUSTOMER");
                // Load Combobox Customer Group Data
                SPlanetUtil.BindASPxComboBox(ref cboCustomerGroup, DataListUtil.DropdownStoreProcedureName.Customer_Group);
                cboCustomerGroup.Items.Insert(0, new ListEditItem("--โปรดเลือก--", "0"));

                // Load Combobox Customer Industry Type Data
                SPlanetUtil.BindASPxComboBox(ref cboIndustry, DataListUtil.DropdownStoreProcedureName.Customer_IndustryType);
                cboIndustry.Items.Insert(0, new ListEditItem("--โปรดเลือก--", "0"));

                // Load Combobox Customer Branch Data
                SPlanetUtil.BindASPxComboBox(ref cboCustomerBusiness, DataListUtil.DropdownStoreProcedureName.Customer_Business);
                cboCustomerBusiness.Items.Insert(0, new ListEditItem("--โปรดเลือก--", "0"));

                // Load Combobox Thailand Geo Data
                SPlanetUtil.BindASPxComboBox(ref cboGEO, DataListUtil.DropdownStoreProcedureName.Thailand_Geo);
                cboGEO.Items.Insert(0, new ListEditItem("--โปรดเลือก--", "0"));

                // Setting height gridView
                gridView.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen);
                gridView.SettingsBehavior.AllowFocusedRow = true;

                dsResult = null;
                BindGrid(true);
            }
            else
            {
                dsResult = (DataSet)Session["SESSION_CUSTOMER_MASTER"];
                BindGrid(false);
                BindGridMFG();
            }
        }

        private void ClearWorkingSession()
        {
            Session.Remove("SESSION_CUSTOMER_MFG_CUSTOMER");
            Session.Remove("SESSION_CUSTOMER_ATTENTION_CUSTOMER");
        }


        #region "Inherited Event of Filter Control"
        public override void OnFilterChanged()
        {
            BindGrid();
        }
        public override void RefreshEntry()     //Popup Menu Select Refresh Data
        {
            BindGrid(true);
        }

        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            return result.OfType<FilterControlColumn>();
        }
        #endregion

        protected void BindGrid(bool isForceRefreshData = false)
        {
            try
            {
                if (!Page.IsPostBack || isForceRefreshData)
                {
                    string search = "";
                    if (Session["SEARCH"] != null)
                    {
                        search = Session["SEARCH"].ToString();
                        txtSearchBoxData.Value = search;
                    }

                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = search },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 }
                        };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_list", arrParm.ToArray());
                        conn.Close();

                        /*int i = 0;
                        int rowId = 0;
                        if (Session["ROW_ID"] != null)
                        {
                            rowId = Convert.ToInt32(Session["ROW_ID"].ToString());
                            //  If -1, Find the most value
                            if (rowId == -1)
                            {
                                foreach (var row in dsResult.Tables[0].AsEnumerable())
                                {
                                    int id = Convert.ToInt32(row["id"]);
                                    if (rowId < id)
                                    {
                                        rowId = id;
                                    }
                                }
                            }
                        }
                        foreach (var row in dsResult.Tables[0].AsEnumerable())
                        {
                            if (rowId == Convert.ToInt32(row["id"]))
                            {
                                int selectedRow = i;
                                int prevRow = Convert.ToInt32(Session["ROW"]);
                                int pageSize = gridView.SettingsPager.PageSize;
                                int pageIndex = (int)(selectedRow / pageSize);
                                int prevPageIndex = Convert.ToInt32(Session["PAGE"]);
                                if (prevRow == selectedRow)
                                {
                                    Session["PAGE"] = prevPageIndex;
                                    Session["ROW"] = prevPageIndex * pageSize;
                                }
                                else
                                {
                                    Session["PAGE"] = pageIndex;
                                    Session["ROW"] = selectedRow;
                                }
                            }
                            i++;
                        }*/

                        Session["SESSION_CUSTOMER_MASTER"] = dsResult;
                    }
                }

                //Bind data into GridView
                gridView.DataSource = dsResult;
                gridView.DataBind();

                //  Check page from session
                if (Session["ROW_ID"] != null)
                {
                    int row = Convert.ToInt32(Session["ROW"]);
                    gridView.FocusedRowIndex = row;
                }
                if (Session["PAGE"] != null)
                {
                    int page = Convert.ToInt32(Session["PAGE"]);
                    gridView.PageIndex = page;
                }
                if (!Page.IsPostBack && Session["COLUMN"] != null && Session["ORDER"] != null)
                {
                    int order = Convert.ToInt32(Session["ORDER"]);
                    if (order == 1)
                    {
                        ((GridViewDataColumn)gridView.Columns[Session["COLUMN"].ToString()]).SortAscending();
                    }
                    else
                    {
                        ((GridViewDataColumn)gridView.Columns[Session["COLUMN"].ToString()]).SortDescending();
                    }
                }
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }

        protected void cboProvince_Callback(object source, CallbackEventArgsBase e)
        {
            FillProvinceCombo(e.Parameter);
        }
        protected void cboAmphur_Callback(object source, CallbackEventArgsBase e)
        {
            FillAmphurCombo(e.Parameter);
        }
        protected void cboDistrict_Callback(object source, CallbackEventArgsBase e)
        {
            FillDistrictCombo(e.Parameter);
        }

        protected void FillProvinceCombo(string geoId)
        {
            try
            {
                dtProvince = new DataTable();
                dtProvince.Columns.Add("data_text", typeof(string));
                dtProvince.Columns.Add("data_value", typeof(int));
                dtProvince.Columns.Add("geo_id", typeof(int));

                if (!string.IsNullOrEmpty(geoId) && geoId != "0")
                {
                    var dtSource = new DataTable();
                    dtSource.Columns.Add("data_value", typeof(int));
                    dtSource.Columns.Add("data_text", typeof(string));
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        conn.Open();
                        using (DataSet dsResult = SqlHelper.ExecuteDataset(SPlanetUtil.GetConnectionString(), CommandType.StoredProcedure, DataListUtil.DropdownStoreProcedureName.Thailand_Province))
                        {
                            if (dsResult != null)
                            {
                                foreach (var row in dsResult.Tables[0].AsEnumerable())
                                {
                                    if (Convert.ToString(row["geo_id"]) == geoId)
                                    {
                                        dtProvince.Rows.Add(row[1], Convert.ToInt32(row[0]), Convert.ToInt32(row[2]));
                                    }
                                }
                            }

                        }
                        conn.Close();
                    }

                    var provinceFilter = (from t in dtProvince.AsEnumerable()
                                          where t.Field<int>("geo_id") == Convert.ToInt32(geoId)
                                          select t).ToList();
                    foreach (var row in provinceFilter)
                    {
                        dtSource.Rows.Add(row[1], row[0]);
                    }
                    cboProvince.DataSource = dtSource;
                    cboProvince.DataBind();
                }
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        protected void FillAmphurCombo(string provinceId)
        {
            try
            {
                dtAmphur = new DataTable();
                dtAmphur.Columns.Add("data_text", typeof(string));
                dtAmphur.Columns.Add("data_value", typeof(int));
                dtAmphur.Columns.Add("province_id", typeof(int));

                if (!string.IsNullOrEmpty(provinceId) && provinceId != "0")
                {
                    var dtSource = new DataTable();
                    dtSource.Columns.Add("data_value", typeof(int));
                    dtSource.Columns.Add("data_text", typeof(string));
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        conn.Open();
                        using (DataSet dsResult = SqlHelper.ExecuteDataset(SPlanetUtil.GetConnectionString(), CommandType.StoredProcedure, DataListUtil.DropdownStoreProcedureName.Thailand_Amphur))
                        {
                            if (dsResult != null)
                            {
                                foreach (var row in dsResult.Tables[0].AsEnumerable())
                                {
                                    if (Convert.ToString(row["province_id"]) == provinceId)
                                    {
                                        dtAmphur.Rows.Add(row[1], Convert.ToInt32(row[0]), Convert.ToInt32(row[2]));
                                    }
                                }
                            }

                        }
                        conn.Close();
                    }

                    var amphurFilter = (from t in dtAmphur.AsEnumerable()
                                        where t.Field<int>("province_id") == Convert.ToInt32(provinceId)
                                        select t).ToList();
                    foreach (var row in amphurFilter)
                    {
                        dtSource.Rows.Add(row[1], row[0]);
                    }
                    cboAmphur.DataSource = dtSource;
                    cboAmphur.DataBind();
                }
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        protected void FillDistrictCombo(string amphurId)
        {
            try
            {
                dtDistrict = new DataTable();
                dtDistrict.Columns.Add("data_text", typeof(string));
                dtDistrict.Columns.Add("data_value", typeof(int));
                dtDistrict.Columns.Add("amphur_id", typeof(int));

                if (!string.IsNullOrEmpty(amphurId) && amphurId != "0")
                {
                    var dtSource = new DataTable();
                    dtSource.Columns.Add("data_value", typeof(int));
                    dtSource.Columns.Add("data_text", typeof(string));

                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        conn.Open();
                        using (DataSet dsResult = SqlHelper.ExecuteDataset(SPlanetUtil.GetConnectionString(), CommandType.StoredProcedure, DataListUtil.DropdownStoreProcedureName.Thailand_District))
                        {
                            if (dsResult != null)
                            {
                                foreach (var row in dsResult.Tables[0].AsEnumerable())
                                {
                                    if (Convert.ToString(row["amphur_id"]) == amphurId)
                                    {
                                        dtDistrict.Rows.Add(row[1], Convert.ToInt32(row[0]), Convert.ToInt32(row[2]));
                                    }
                                }
                            }

                        }
                        conn.Close();
                    }

                    var districtFilter = (from t in dtDistrict.AsEnumerable()
                                          where t.Field<int>("amphur_id") == Convert.ToInt32(amphurId)
                                          select t).ToList();
                    foreach (var row in districtFilter)
                    {
                        dtSource.Rows.Add(row[1], row[0]);
                    }
                    cboDistrict.DataSource = dtSource;
                    cboDistrict.DataBind();
                }
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }

        [WebMethod]
        public static string InsertCustomer(CustomerList[] customerAddData)
        {
            int newId = 0;
            var customerAttentionList = (List<CustomerAttention>)HttpContext.Current.Session["SESSION_CUSTOMER_ATTENTION_CUSTOMER"];
            var mfgCustomer = (List<CustomerMFG>)HttpContext.Current.Session["SESSION_CUSTOMER_MFG_CUSTOMER"];



            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        var row = (from t in customerAddData select t).FirstOrDefault();
                        if (row != null)
                        {

                            DateTime? first_contact_date = null;
                            if (!string.IsNullOrEmpty(row.first_contact))
                            {
                                first_contact_date = DateTime.ParseExact(row.first_contact, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }

                            if (row.id == 0)
                            {

                                using (SqlCommand cmd = new SqlCommand("sp_customer_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@customer_code", SqlDbType.VarChar, 50).Value = row.customer_code;
                                    cmd.Parameters.Add("@company_name_tha", SqlDbType.VarChar, 200).Value = row.company_name_tha;
                                    cmd.Parameters.Add("@company_name_eng", SqlDbType.VarChar, 200).Value = row.company_name_eng;
                                    cmd.Parameters.Add("@customer_name_other", SqlDbType.VarChar, 200).Value = row.customer_name_other;
                                    cmd.Parameters.Add("@address_bill_tha", SqlDbType.VarChar, 200).Value = row.address_bill_tha;
                                    cmd.Parameters.Add("@address_bill_eng", SqlDbType.VarChar, 200).Value = row.address_bill_eng;
                                    cmd.Parameters.Add("@address_install_tha", SqlDbType.VarChar, 200).Value = "";
                                    cmd.Parameters.Add("@address_install_eng", SqlDbType.VarChar, 200).Value = "";
                                    cmd.Parameters.Add("@customer_group_id", SqlDbType.Int).Value = row.customer_group_id;
                                    cmd.Parameters.Add("@customer_business_id", SqlDbType.Int).Value = row.customer_business_id;
                                    cmd.Parameters.Add("@customer_industry_id", SqlDbType.Int).Value = row.customer_industry_id;
                                    cmd.Parameters.Add("@tax_id", SqlDbType.VarChar, 50).Value = row.tax_id;
                                    cmd.Parameters.Add("@branch_name", SqlDbType.VarChar, 100).Value = row.branch_name;
                                    cmd.Parameters.Add("@branch_code", SqlDbType.VarChar, 50).Value = row.branch_code;
                                    cmd.Parameters.Add("@geo_id", SqlDbType.Int).Value = row.geo_id;
                                    cmd.Parameters.Add("@province_id", SqlDbType.Int).Value = row.province_id;
                                    cmd.Parameters.Add("@amphur_id", SqlDbType.Int).Value = row.amphur_id;
                                    cmd.Parameters.Add("@district_id", SqlDbType.Int).Value = row.district_id;
                                    cmd.Parameters.Add("@zipcode", SqlDbType.VarChar, 10).Value = row.zipcode;
                                    cmd.Parameters.Add("@tel", SqlDbType.VarChar, 50).Value = row.tel;
                                    cmd.Parameters.Add("@mobile", SqlDbType.VarChar, 50).Value = row.mobile;
                                    cmd.Parameters.Add("@fax", SqlDbType.VarChar, 50).Value = row.fax;
                                    cmd.Parameters.Add("@first_contact", SqlDbType.Date).Value = first_contact_date;
                                    cmd.Parameters.Add("@vat", SqlDbType.Int).Value = row.vat;
                                    cmd.Parameters.Add("@credit_term", SqlDbType.Int).Value = row.credit_term;
                                    cmd.Parameters.Add("@bill_date", SqlDbType.VarChar, 100).Value = row.bill_date;
                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 50).Value = row.remark;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                    newId = Convert.ToInt32(cmd.ExecuteScalar());
                                }

                                foreach (var rowAttention in customerAttentionList)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_customer_attention_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = newId;
                                        cmd.Parameters.Add("@attention_name", SqlDbType.VarChar, 200).Value = rowAttention.attention_name;
                                        cmd.Parameters.Add("@attention_tel", SqlDbType.VarChar, 100).Value = rowAttention.attention_tel;
                                        cmd.Parameters.Add("@attention_email", SqlDbType.VarChar, 100).Value = rowAttention.attention_email;

                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        cmd.ExecuteNonQuery();

                                    }
                                }

                                foreach (var rowMFG in mfgCustomer)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_customer_mfg_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = newId;
                                        cmd.Parameters.Add("@model", SqlDbType.VarChar, 50).Value = rowMFG.model;
                                        cmd.Parameters.Add("@mfg", SqlDbType.VarChar, 50).Value = rowMFG.mfg == "" ? string.Empty : rowMFG.mfg;

                                        cmd.Parameters.Add("@pressure", SqlDbType.VarChar, 50).Value = rowMFG.pressure;
                                        cmd.Parameters.Add("@power_supply", SqlDbType.VarChar, 50).Value = rowMFG.power_supply;
                                        cmd.Parameters.Add("@phase", SqlDbType.Int).Value = rowMFG.phase;
                                        cmd.Parameters.Add("@hz", SqlDbType.Int).Value = rowMFG.hz;

                                        cmd.Parameters.Add("@unitwarraty", SqlDbType.VarChar, 20).Value = rowMFG.unitwarraty;
                                        cmd.Parameters.Add("@airendwarraty", SqlDbType.VarChar, 20).Value = rowMFG.airendwarraty;
                                        cmd.Parameters.Add("@service_fee", SqlDbType.VarChar, 20).Value = rowMFG.service_fee;
                                        cmd.Parameters.Add("@project", SqlDbType.VarChar, 200).Value = rowMFG.project;
                                        cmd.Parameters.Add("@is_demo", SqlDbType.Bit).Value = rowMFG.is_demo;

                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {

                                newId = row.id;
                                using (SqlCommand cmd = new SqlCommand("sp_customer_edit", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(row.id);
                                    cmd.Parameters.Add("@customer_code", SqlDbType.VarChar, 50).Value = row.customer_code;
                                    cmd.Parameters.Add("@company_name_tha", SqlDbType.VarChar, 200).Value = row.company_name_tha;
                                    cmd.Parameters.Add("@company_name_eng", SqlDbType.VarChar, 200).Value = row.company_name_eng;
                                    cmd.Parameters.Add("@customer_name_other", SqlDbType.VarChar, 200).Value = row.customer_name_other;
                                    cmd.Parameters.Add("@address_bill_tha", SqlDbType.VarChar, 200).Value = row.address_bill_tha;
                                    cmd.Parameters.Add("@address_bill_eng", SqlDbType.VarChar, 200).Value = row.address_bill_eng;
                                    cmd.Parameters.Add("@address_install_tha", SqlDbType.VarChar, 200).Value = "";
                                    cmd.Parameters.Add("@address_install_eng", SqlDbType.VarChar, 200).Value = "";
                                    cmd.Parameters.Add("@customer_group_id", SqlDbType.Int).Value = row.customer_group_id;
                                    cmd.Parameters.Add("@customer_business_id", SqlDbType.Int).Value = row.customer_business_id;
                                    cmd.Parameters.Add("@customer_industry_id", SqlDbType.Int).Value = row.customer_industry_id;
                                    cmd.Parameters.Add("@tax_id", SqlDbType.VarChar, 50).Value = row.tax_id;
                                    cmd.Parameters.Add("@branch_name", SqlDbType.VarChar, 100).Value = row.branch_name;
                                    cmd.Parameters.Add("@branch_code", SqlDbType.VarChar, 50).Value = row.branch_code;
                                    cmd.Parameters.Add("@geo_id", SqlDbType.Int).Value = row.geo_id;
                                    cmd.Parameters.Add("@province_id", SqlDbType.Int).Value = row.province_id;
                                    cmd.Parameters.Add("@amphur_id", SqlDbType.Int).Value = row.amphur_id;
                                    cmd.Parameters.Add("@district_id", SqlDbType.Int).Value = row.district_id;
                                    cmd.Parameters.Add("@zipcode", SqlDbType.VarChar, 10).Value = row.zipcode;
                                    cmd.Parameters.Add("@tel", SqlDbType.VarChar, 50).Value = row.tel;
                                    cmd.Parameters.Add("@mobile", SqlDbType.VarChar, 50).Value = row.mobile;
                                    cmd.Parameters.Add("@fax", SqlDbType.VarChar, 50).Value = row.fax;
                                    cmd.Parameters.Add("@first_contact", SqlDbType.Date).Value = first_contact_date;
                                    cmd.Parameters.Add("@vat", SqlDbType.Int).Value = row.vat;
                                    cmd.Parameters.Add("@credit_term", SqlDbType.Int).Value = row.credit_term;
                                    cmd.Parameters.Add("@bill_date", SqlDbType.VarChar, 100).Value = row.bill_date;
                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 50).Value = row.remark;
                                    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                    cmd.ExecuteNonQuery();

                                }
                                foreach (var rowAttention in customerAttentionList)
                                {
                                    if (rowAttention.id < 0)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_customer_attention_add", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = newId;
                                            cmd.Parameters.Add("@attention_name", SqlDbType.VarChar, 200).Value = rowAttention.attention_name;
                                            cmd.Parameters.Add("@attention_tel", SqlDbType.VarChar, 100).Value = rowAttention.attention_tel;
                                            cmd.Parameters.Add("@attention_email", SqlDbType.VarChar, 100).Value = rowAttention.attention_email;

                                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                    else if (rowAttention.id > 0 && rowAttention.is_delete)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_customer_attention_delete", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowAttention.id;
                                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                    else
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_customer_attention_edit", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowAttention.id;
                                            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = newId;
                                            cmd.Parameters.Add("@attention_name", SqlDbType.VarChar, 200).Value = rowAttention.attention_name;
                                            cmd.Parameters.Add("@attention_tel", SqlDbType.VarChar, 100).Value = rowAttention.attention_tel;
                                            cmd.Parameters.Add("@attention_email", SqlDbType.VarChar, 100).Value = rowAttention.attention_email;

                                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                }

                                foreach (var rowMFG in mfgCustomer)
                                {
                                    if (rowMFG.id < 0 && rowMFG.is_delete == false)
                                    {

                                        using (SqlCommand cmd = new SqlCommand("sp_customer_mfg_add", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = newId;
                                            cmd.Parameters.Add("@model", SqlDbType.VarChar, 50).Value = rowMFG.model;
                                            cmd.Parameters.Add("@mfg", SqlDbType.VarChar, 50).Value = rowMFG.mfg == "" ? string.Empty : rowMFG.mfg;

                                            cmd.Parameters.Add("@pressure", SqlDbType.VarChar, 50).Value = rowMFG.pressure;
                                            cmd.Parameters.Add("@power_supply", SqlDbType.VarChar, 50).Value = rowMFG.power_supply;
                                            cmd.Parameters.Add("@phase", SqlDbType.Int).Value = rowMFG.phase;
                                            cmd.Parameters.Add("@hz", SqlDbType.Int).Value = rowMFG.hz;

                                            cmd.Parameters.Add("@unitwarraty", SqlDbType.VarChar, 20).Value = rowMFG.unitwarraty;
                                            cmd.Parameters.Add("@airendwarraty", SqlDbType.VarChar, 20).Value = rowMFG.airendwarraty;
                                            cmd.Parameters.Add("@service_fee", SqlDbType.VarChar, 20).Value = rowMFG.service_fee;
                                            cmd.Parameters.Add("@project", SqlDbType.VarChar, 200).Value = rowMFG.project;
                                            cmd.Parameters.Add("@is_demo", SqlDbType.Bit).Value = rowMFG.is_demo;

                                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                            cmd.ExecuteNonQuery();

                                        }

                                    }
                                    else
                                    {
                                        if (rowMFG.is_delete == false)
                                        {
                                            using (SqlCommand cmd = new SqlCommand("sp_customer_mfg_edit", conn, tran))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowMFG.id;
                                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = newId;
                                                cmd.Parameters.Add("@model", SqlDbType.VarChar, 50).Value = rowMFG.model;
                                                cmd.Parameters.Add("@mfg", SqlDbType.VarChar, 50).Value = rowMFG.mfg;

                                                if (rowMFG.product_id == 0)
                                                {
                                                    cmd.Parameters.Add("@pressure", SqlDbType.VarChar, 50).Value = rowMFG.pressure;
                                                    cmd.Parameters.Add("@power_supply", SqlDbType.VarChar, 50).Value = rowMFG.power_supply;
                                                    cmd.Parameters.Add("@phase", SqlDbType.Int).Value = rowMFG.phase;
                                                    cmd.Parameters.Add("@hz", SqlDbType.Int).Value = rowMFG.hz;
                                                }
                                                cmd.Parameters.Add("@unitwarraty", SqlDbType.VarChar, 20).Value = rowMFG.unitwarraty;
                                                cmd.Parameters.Add("@airendwarraty", SqlDbType.VarChar, 20).Value = rowMFG.airendwarraty;
                                                cmd.Parameters.Add("@service_fee", SqlDbType.VarChar, 20).Value = rowMFG.service_fee;
                                                cmd.Parameters.Add("@project", SqlDbType.VarChar, 200).Value = rowMFG.project;
                                                cmd.Parameters.Add("@is_demo", SqlDbType.Bit).Value = rowMFG.is_demo;
                                                cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                                cmd.ExecuteNonQuery();

                                            }
                                        }
                                        else
                                        {
                                            using (SqlCommand cmd = new SqlCommand("sp_customer_mfg_delete", conn, tran))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowMFG.id;
                                                cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                                cmd.ExecuteNonQuery();

                                            }
                                        }
                                    }
                                }

                            }
                        }

                        //  Set new id
                        HttpContext.Current.Session["ROW_ID"] = Convert.ToString(newId);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        tran.Dispose();
                        conn.Close();
                        //throw ex;
                        return ex.Message.ToString();
                    }
                    finally
                    {
                        if (!conn.State.Equals(ConnectionState.Closed))
                        {

                            tran.Commit();
                            tran.Dispose();
                            conn.Close();
                        }
                    }
                }
                return "success";
            }
        }

        [WebMethod]
        public static CustomerList GetEditCustomerData(string id, int index)
        {
            //  Set active row
            HttpContext.Current.Session["ROW_ID"] = id;
            HttpContext.Current.Session["ROW"] = index;

            var customerData = new CustomerList();
            var dsDataCustomer = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(id) }
                        };
                    conn.Open();
                    dsDataCustomer = SqlHelper.ExecuteDataset(conn, "sp_customer_list", arrParm.ToArray());
                    conn.Close();


                    if (dsDataCustomer.Tables.Count > 0)
                    {
                        var row = dsDataCustomer.Tables[0].Rows[0];

                        customerData.id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]);
                        customerData.customer_code = Convert.IsDBNull(row["customer_code"]) ? null : Convert.ToString(row["customer_code"]);
                        customerData.company_name_tha = Convert.IsDBNull(row["company_name_tha"]) ? null : Convert.ToString(row["company_name_tha"]);
                        customerData.company_name_eng = Convert.IsDBNull(row["company_name_eng"]) ? null : Convert.ToString(row["company_name_eng"]);
                        customerData.customer_name_other = Convert.IsDBNull(row["customer_name_other"]) ? null : Convert.ToString(row["customer_name_other"]);
                        customerData.address_bill_tha = Convert.IsDBNull(row["address_bill_tha"]) ? null : Convert.ToString(row["address_bill_tha"]);
                        customerData.address_bill_eng = Convert.IsDBNull(row["address_bill_eng"]) ? null : Convert.ToString(row["address_bill_eng"]);

                        customerData.customer_group_id = Convert.IsDBNull(row["customer_group_id"]) ? 0 : Convert.ToInt32(row["customer_group_id"]);
                        customerData.customer_business_id = Convert.IsDBNull(row["customer_business_id"]) ? 0 : Convert.ToInt32(row["customer_business_id"]);
                        customerData.customer_industry_id = Convert.IsDBNull(row["customer_industry_id"]) ? 0 : Convert.ToInt32(row["customer_industry_id"]);
                        customerData.tax_id = Convert.IsDBNull(row["tax_id"]) ? null : Convert.ToString(row["tax_id"]);
                        customerData.branch_name = Convert.IsDBNull(row["branch_name"]) ? null : Convert.ToString(row["branch_name"]);
                        customerData.branch_code = Convert.IsDBNull(row["branch_code"]) ? null : Convert.ToString(row["branch_code"]);
                        customerData.geo_id = Convert.IsDBNull(row["geo_id"]) ? 0 : Convert.ToInt32(row["geo_id"]);
                        customerData.province_id = Convert.IsDBNull(row["province_id"]) ? 0 : Convert.ToInt32(row["province_id"]);
                        customerData.amphur_id = Convert.IsDBNull(row["amphur_id"]) ? 0 : Convert.ToInt32(row["amphur_id"]);
                        customerData.district_id = Convert.IsDBNull(row["district_id"]) ? 0 : Convert.ToInt32(row["district_id"]);
                        customerData.zipcode = Convert.IsDBNull(row["zipcode"]) ? "" : Convert.ToString(row["zipcode"]);
                        customerData.tel = Convert.IsDBNull(row["tel"]) ? null : Convert.ToString(row["tel"]);
                        customerData.mobile = Convert.IsDBNull(row["mobile"]) ? null : Convert.ToString(row["mobile"]);
                        customerData.fax = Convert.IsDBNull(row["fax"]) ? null : Convert.ToString(row["fax"]);
                        customerData.first_contact = Convert.IsDBNull(row["first_contact"]) ? null : Convert.ToString(row["first_contact"]);
                        customerData.credit_term = Convert.IsDBNull(row["credit_term"]) ? 0 : Convert.ToInt32(row["credit_term"]);
                        customerData.vat = Convert.IsDBNull(row["vat"]) ? 0 : Convert.ToDecimal(row["vat"]);
                        customerData.bill_date = Convert.IsDBNull(row["bill_date"]) ? null : Convert.ToString(row["bill_date"]);
                        customerData.remark = Convert.IsDBNull(row["remark"]) ? null : Convert.ToString(row["remark"]);

                    }
                    var customerAttentionList = new List<CustomerAttention>();
                    List<SqlParameter> arrParm2 = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("customer_id" , SqlDbType.Int) {Value = Convert.ToInt32(id) }
                        };
                    conn.Open();
                    var dsAttention = SqlHelper.ExecuteDataset(conn, "sp_customer_attention_list", arrParm2.ToArray());
                    conn.Close();

                    if (dsAttention.Tables.Count > 0)
                    {
                        var data = (from t in dsAttention.Tables[0].AsEnumerable() select t).ToList();
                        if (data != null)
                        {
                            foreach (var row in data)
                            {
                                customerAttentionList.Add(new CustomerAttention()
                                {
                                    attention_email = Convert.IsDBNull(row["attention_email"]) ? string.Empty : Convert.ToString(row["attention_email"]),
                                    attention_name = Convert.IsDBNull(row["attention_name"]) ? string.Empty : Convert.ToString(row["attention_name"]),
                                    attention_tel = Convert.IsDBNull(row["attention_tel"]) ? string.Empty : Convert.ToString(row["attention_tel"]),
                                    customer_id = Convert.IsDBNull(row["customer_id"]) ? 0 : Convert.ToInt32(row["customer_id"]),
                                    id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                    is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                });
                            }
                        }
                    }

                    var customerMFGList = new List<CustomerMFG>();
                    List<SqlParameter> arrParm4 = new List<SqlParameter>
                        {
                        new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = id },
                        };
                    conn.Open();
                    var dsResult4 = SqlHelper.ExecuteDataset(conn, "sp_customer_mfg_list", arrParm4.ToArray());
                    conn.Close();
                    if (dsResult4.Tables.Count > 0)
                    {
                        var data = (from t in dsResult4.Tables[0].AsEnumerable() select t).ToList();
                        if (data != null)
                        {
                            foreach (var row in data)
                            {
                                customerMFGList.Add(new CustomerMFG()
                                {
                                    id = Convert.ToInt32(row["id"]),
                                    customer_id = Convert.ToInt32(row["customer_id"]),
                                    model = Convert.IsDBNull("model") ? string.Empty : Convert.ToString(row["model"]),
                                    mfg = Convert.IsDBNull("mfg") ? string.Empty : Convert.ToString(row["mfg"]),
                                    date = Convert.IsDBNull("date_mfg") ? string.Empty : Convert.ToString(row["date_mfg"]),

                                    product_id = DBNull.Value.Equals(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),

                                    unitwarraty = DBNull.Value.Equals(row["unitwarraty"]) ? "" : Convert.ToString(row["unitwarraty"]),
                                    airendwarraty = DBNull.Value.Equals(row["airendwarraty"]) ? "" : Convert.ToString(row["airendwarraty"]),
                                    service_fee = DBNull.Value.Equals(row["service_fee"]) ? 0m : Convert.ToDecimal(row["service_fee"]),
                                    project = Convert.IsDBNull("project") ? string.Empty : Convert.ToString(row["project"]),
                                    is_demo = DBNull.Value.Equals(row["is_demo"]) ? false : Convert.ToBoolean(row["is_demo"]),

                                    pressure = Convert.IsDBNull("pressure") ? string.Empty : Convert.ToString(row["pressure"]),
                                    hz = Convert.IsDBNull("hz") ? 0 : Convert.ToInt32(row["hz"]),
                                    power_supply = Convert.IsDBNull("power_supply") ? string.Empty : Convert.ToString(row["power_supply"]),
                                    phase = Convert.IsDBNull("phase") ? 0 : Convert.ToInt32(row["phase"]),
                                });

                            }
                        }
                    }
                    HttpContext.Current.Session["SESSION_CUSTOMER_ATTENTION_CUSTOMER"] = customerAttentionList;
                    HttpContext.Current.Session["SESSION_CUSTOMER_MFG_CUSTOMER"] = customerMFGList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return customerData;
        }

        [WebMethod]
        public static string DeleteCustomer(string id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_customer_delete", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(id);
                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "success";
        }

        [WebMethod]
        public static string DeleteMFG(string id)
        {
            var customerMFGList = (List<CustomerMFG>)HttpContext.Current.Session["SESSION_CUSTOMER_MFG_CUSTOMER"];
            if (customerMFGList != null)
            {

                var row = (from t in customerMFGList where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (row != null)
                {

                    row.is_delete = true;
                }
            }
            return "SUCCESS";

        }
        protected void gridCustomerAttention_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridCustomerAttention.DataSource = (from t in customerAttentionList where !t.is_delete select t).ToList();
            gridCustomerAttention.DataBind();
        }

        [WebMethod]
        public static CustomerAttention EditCustomerAttention(int id)
        {
            try
            {
                var row = new CustomerAttention();
                var customerAttentionList = (List<CustomerAttention>)HttpContext.Current.Session["SESSION_CUSTOMER_ATTENTION_CUSTOMER"];
                if (customerAttentionList != null)
                {
                    row = (from t in customerAttentionList where t.id == id select t).FirstOrDefault();
                }
                return row;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static string SubmitCustomerAttention(int id, string name, string tel, string email)
        {
            try
            {
                var returnData = string.Empty;
                var customerAttentionList = (List<CustomerAttention>)HttpContext.Current.Session["SESSION_CUSTOMER_ATTENTION_CUSTOMER"];
                if (id == 0)
                {
                    if (customerAttentionList == null)
                    {
                        customerAttentionList = new List<CustomerAttention>();
                    }
                    customerAttentionList.Add(new CustomerAttention()
                    {
                        attention_email = email,
                        attention_name = name,
                        attention_tel = tel,
                        is_delete = false,
                        id = (customerAttentionList.Count + 1) * -1
                    });
                }
                else
                {
                    var row = (from t in customerAttentionList where t.id == id select t).FirstOrDefault();
                    if (row != null)
                    {
                        row.attention_email = email;
                        row.attention_tel = tel;
                        row.attention_name = name;
                    }
                }
                HttpContext.Current.Session["SESSION_CUSTOMER_ATTENTION_CUSTOMER"] = customerAttentionList;

                return returnData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static string DeleteCustomerAttention(int id)
        {
            var returnData = string.Empty;
            try
            {
                var row = new CustomerAttention();
                var customerAttentionList = (List<CustomerAttention>)HttpContext.Current.Session["SESSION_CUSTOMER_ATTENTION_CUSTOMER"];
                if (customerAttentionList != null)
                {
                    row = (from t in customerAttentionList where t.id == id select t).FirstOrDefault();
                    if (row != null)
                    {
                        if (row.id < 0)
                        {
                            customerAttentionList.Remove(row);
                        }
                        else
                        {
                            row.is_delete = true;
                        }
                    }
                }
                HttpContext.Current.Session["SESSION_CUSTOMER_ATTENTION_CUSTOMER"] = customerAttentionList;
                return returnData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gridMFG_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                List<CustomerMFG> customer = (from t in customerMFGList where t.is_delete == false select t).ToList();
                List<CustomerMFG> search = new List<CustomerMFG>();
                search = customer.Where(a => a.model.Contains(e.Parameters.ToString()) || a.project.Contains(e.Parameters.ToString()) || a.mfg.Contains(e.Parameters.ToString())).ToList();

                //Bind data into GridView
                gridMFG.DataSource = search;
                gridMFG.DataBind();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }

            
        }
        protected void BindGridMFG()
        {
            gridMFG.DataSource = customerMFGList;
            gridMFG.DataBind();
        }

        [WebMethod]
        public static void NewCustomerData()
        {
            HttpContext.Current.Session.Remove("SESSION_CUSTOMER_ATTENTION_CUSTOMER");
            HttpContext.Current.Session.Remove("SESSION_CUSTOMER_MFG_CUSTOMER");

        }

        [WebMethod]
        public static string AddMFG(string model, string mfg, string is_demo, string project, string unitwarraty, string airendwarraty, decimal service_fee, string customer_id, string pressure, string power_supply, int phase, int hz)
        {
            var returnData = string.Empty;

            var customerMFGList = (List<CustomerMFG>)HttpContext.Current.Session["SESSION_CUSTOMER_MFG_CUSTOMER"];


            if (customerMFGList != null)
            {
                var checkExist = (from t in customerMFGList
                                  where t.mfg == mfg
                                  select t).FirstOrDefault();
                if (checkExist == null)
                {
                    var ds = new DataSet();
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                        {
                            List<SqlParameter> arrParm = new List<SqlParameter>
                            {
                                new SqlParameter("@customer_id", SqlDbType.VarChar,200) { Value = customer_id },
                                new SqlParameter("@mfg", SqlDbType.VarChar,200) { Value = mfg }
                            };
                            conn.Open();
                            ds = SqlHelper.ExecuteDataset(conn, "sp_customer_mfg_checkexist", arrParm.ToArray());
                            conn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    //  Add to list
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        customerMFGList.Add(new CustomerMFG()
                        {
                            id = (customerMFGList.Count + 1) * -1,

                            model = model,
                            mfg = mfg,
                            is_demo = is_demo == "0" ? false : true,
                            project = project,
                            pressure = pressure,
                            power_supply = power_supply,
                            phase = phase,
                            hz = hz,
                            unitwarraty = unitwarraty,
                            airendwarraty = airendwarraty,
                            service_fee = service_fee
                        });
                    }
                    else
                    {
                        returnData = "-1";
                    }
                }
                else
                {
                    if (checkExist.is_delete || checkExist.mfg.Equals(""))
                    {
                        customerMFGList.Add(new CustomerMFG()
                        {
                            id = (customerMFGList.Count + 1) * -1,

                            model = model,
                            mfg = mfg,
                            is_demo = is_demo == "0" ? false : true,
                            project = project,
                            pressure = pressure,
                            power_supply = power_supply,
                            phase = phase,
                            hz = hz,
                            unitwarraty = unitwarraty,
                            airendwarraty = airendwarraty,
                            service_fee = service_fee
                        });
                    }
                    else
                    {
                        returnData = "-1";
                    }
                }
            }
            else
            {
                customerMFGList = new List<CustomerMFG>();
                customerMFGList.Add(new CustomerMFG()
                {
                    id = (customerMFGList.Count + 1) * -1,
                    model = model,
                    mfg = mfg,
                    is_demo = is_demo == "0" ? false : true,
                    project = project,
                    pressure = pressure,
                    power_supply = power_supply,
                    phase = phase,
                    hz = hz,
                    unitwarraty = unitwarraty,
                    airendwarraty = airendwarraty,
                    service_fee = service_fee
                });

            }
            HttpContext.Current.Session["SESSION_CUSTOMER_MFG_CUSTOMER"] = customerMFGList;


            return returnData;
        }
        [WebMethod]
        public static CustomerMFG EditMFG(int id)
        {
            var customerMFGList = (List<CustomerMFG>)HttpContext.Current.Session["SESSION_CUSTOMER_MFG_CUSTOMER"];
            if (customerMFGList != null)
            {
                var row = (from t in customerMFGList where t.id == id select t).FirstOrDefault();
                if (row != null)
                {
                    return row;
                }
            }
            return new CustomerMFG(); // ไม่มี Data
        }

        [WebMethod]
        public static string SubmitEditMFG(string mfg_id, string model, string mfg, string is_demo, string project, string unitwarraty, string airendwarraty, decimal service_fee, string pressure, string power_supply, int phase, int hz)
        {
            var customerMFGList = (List<CustomerMFG>)HttpContext.Current.Session["SESSION_CUSTOMER_MFG_CUSTOMER"];
            if (customerMFGList != null)
            {

                var row = (from t in customerMFGList where t.id == Convert.ToInt32(mfg_id) select t).FirstOrDefault();
                if (row != null)
                {
                    row.model = model;
                    row.mfg = mfg;
                    row.is_demo = is_demo == "0" ? false : true;
                    row.project = project;
                    row.pressure = pressure;
                    row.power_supply = power_supply;
                    row.phase = phase;
                    row.hz = hz;
                    row.unitwarraty = unitwarraty;
                    row.airendwarraty = airendwarraty;
                    row.service_fee = service_fee;
                }
            }
            return "SUCCESS";
        }

        [WebMethod]
        public static string ValidateAttentionData()
        {
            var returnMessage = string.Empty;
            var attentionData = (List<CustomerAttention>)HttpContext.Current.Session["SESSION_CUSTOMER_ATTENTION_CUSTOMER"];
            //var sparePartShelfList = (List<SparePartShelf>)HttpContext.Current.Session["SESSION_PART_SHELF_SPAREPART"];

            if (attentionData.Count == 0)
            {
                returnMessage += "error";
            }

            //if (sparePartShelfList.Count == 0)
            //{
            //    returnMessage += "- กรุณาเลือก Shelf อย่างน้อย 1 รายการ <br>";
            //}
            return returnMessage;
        }

        protected void gridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var dsResult = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm = new List<SqlParameter>
                    {
                        new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = e.Parameters.ToString() },
                        new SqlParameter("@id", SqlDbType.Int) { Value = 0 }
                    };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_list", arrParm.ToArray());
                    conn.Close();
                    Session["SESSION_CUSTOMER_MASTER"] = dsResult;

                    Session["SEARCH"] = e.Parameters.ToString();
                }

                //Bind data into GridView
                gridView.DataSource = dsResult;
                gridView.DataBind();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }

        protected void gridView_PageIndexChanged(object sender, EventArgs e)
        {
            int pageIndex = (sender as ASPxGridView).PageIndex;
            Session["PAGE"] = pageIndex;
        }

        protected void gridView_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            string column = e.Column.FieldName;
            int order = Convert.ToInt32(e.Column.SortOrder);

            Session["COLUMN"] = column;
            Session["ORDER"] = order;
        }
    }
}