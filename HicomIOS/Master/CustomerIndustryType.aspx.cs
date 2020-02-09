﻿using System;
using System.Web.UI;
//Custom using namespace
using DevExpress.Web;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using HicomIOS.ClassUtil;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace HicomIOS.Master
{
    public partial class CustomerIndustryType : MasterDetailPage
    {
        public class CustomerIndustryList
        {
            public int id { get; set; }
            public string name_tha { get; set; }
            public string name_eng { get; set; }
            public int parent_id { get; set; }
            public string begin_char_thai { get; set; }
            public bool is_enable { get; set; }
            public bool is_delete { get; set; }
            public int created_by { get; set; }
            public DateTime created_date { get; set; }
            public int updated_by { get; set; }
            public DateTime updated_date { get; set; }
        }

        private DataSet dsResult;
        public override string PageName { get { return "Customer Industry"; } }
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

        // ClientInstanceName that use in Script.js
        private string strClientInstanceName_gridView;
        private string strClientInstanceName_UserControlEditForm = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Setting height gridView
            gridView.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen);
            gridView.SettingsBehavior.AllowFocusedRow = true;

            //Bind Data into GridView
            if (!Page.IsPostBack)
            {
                // Get Permission and if no permission, will redirect to another page.
                if (!Permission.GetPermission())
                    Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

                dsResult = null;
                BindGrid(true);
            }
            else
            {
                dsResult = (DataSet)Session["SESSION_CUSTOMER_INDUSTRY_MASTER"];
                BindGrid(false);
            }
        }
        #region "Inherited Event of Filter Control"
        public override void OnFilterChanged()
        {
            BindGrid();
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
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = search }
                        };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_industry_list", arrParm.ToArray());
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

                        Session["SESSION_CUSTOMER_INDUSTRY_MASTER"] = dsResult;
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

        [WebMethod]
        public static string InsertIndustryType(CustomerIndustryList[] industryTypeAddData)
        {
            var row = (from t in industryTypeAddData select t).FirstOrDefault();
            if (row != null)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_customer_industry_add", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@name_tha", SqlDbType.VarChar, 200).Value = row.name_tha;
                            cmd.Parameters.Add("@name_eng", SqlDbType.VarChar, 200).Value = row.name_eng;
                            cmd.Parameters.Add("@parent_id", SqlDbType.Int).Value = 0;
                            cmd.Parameters.Add("@begin_char_thai", SqlDbType.VarChar, 2).Value = "ก";
                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }

                    //  Set new id
                    HttpContext.Current.Session["ROW_ID"] = "-1";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return "success";
        }

        [WebMethod]
        public static CustomerIndustryList GetEditIndustryTypeData(string id, int index)
        {
            //  Set active row
            HttpContext.Current.Session["ROW_ID"] = id;
            HttpContext.Current.Session["ROW"] = index;

            var IndustryTypeData = new CustomerIndustryList();
            var dsDataIndustryType = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm = new List<SqlParameter>
                    {
                        new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(id) },
                        new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" }
                        
                    };
                    conn.Open();
                    dsDataIndustryType = SqlHelper.ExecuteDataset(conn, "sp_customer_industry_list", arrParm.ToArray());
                    conn.Close();
                }

                if (dsDataIndustryType.Tables.Count > 0)
                {
                    var row = dsDataIndustryType.Tables[0].Rows[0];
                    IndustryTypeData.id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]);
                    IndustryTypeData.name_tha = Convert.IsDBNull(row["name_tha"]) ? null : Convert.ToString(row["name_tha"]);
                    IndustryTypeData.name_eng = Convert.IsDBNull(row["name_eng"]) ? null : Convert.ToString(row["name_eng"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IndustryTypeData;
        }

        [WebMethod]
        public static string UpdateIndustryType(CustomerIndustryList[] industryTypeUpdateData)
        {
            var row = (from t in industryTypeUpdateData select t).FirstOrDefault();
            if (row != null)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_customer_industry_edit", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(row.id);
                            cmd.Parameters.Add("@name_tha", SqlDbType.VarChar, 200).Value = row.name_tha;
                            cmd.Parameters.Add("@name_eng", SqlDbType.VarChar, 200).Value = row.name_eng;
                            cmd.Parameters.Add("@parent_id", SqlDbType.Int).Value = 0;
                            cmd.Parameters.Add("@begin_char_thai", SqlDbType.VarChar, 2).Value = "ก";
                            cmd.Parameters.Add("@is_enable", SqlDbType.Bit).Value = true;
                            cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = false;
                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }

                    //  Set new id
                    HttpContext.Current.Session["ROW_ID"] = Convert.ToString(row.id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return "success";
        }

        [WebMethod]
        public static string DeleteIndustryType(string id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_customer_industry_delete", conn))
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

        protected void gridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var dsResult = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm = new List<SqlParameter>
                    {
                        new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                        new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = e.Parameters.ToString() }
                    };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_industry_list", arrParm.ToArray());
                    conn.Close();
                    Session["SESSION_CUSTOMER_INDUSTRY_MASTER"] = dsResult;

                    Session["SEARCH"] = e.Parameters.ToString();
                }

                //Bind data into GridView
                gridView.DataSource = dsResult;
                gridView.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
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