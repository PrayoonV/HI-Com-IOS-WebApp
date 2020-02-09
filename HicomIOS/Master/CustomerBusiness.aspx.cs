using System;
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
    public partial class CustomerBusiness : MasterDetailPage
    {
        public class CustomerBusinessList
        {
            public int id { get; set; }
            public string name_tha { get; set; }
            public string name_eng { get; set; }
            public bool is_enable { get; set; }
            public int is_delete { get; set; }
            public int created_by { get; set; }
            public DateTime created_date { get; set; }
            public int updated_by { get; set; }
            public DateTime updated_date { get; set; }
        }
        private DataSet dsResult;
        public override string PageName { get { return "Customer Business"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }

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
                dsResult = (DataSet)Session["SESSION_CUSTOMER_BUSINESS_MASTER"];
                BindGrid(false);
            }
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
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_business_list", arrParm.ToArray());

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

                        Session["SESSION_CUSTOMER_BUSINESS_MASTER"] = dsResult;
                    }
                }

                //Bind data into GridView
                gridView.DataSource = dsResult;
                gridView.FilterExpression = FilterBag.GetExpression(false);
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
        public static string InsertCustomerBusiness(CustomerBusinessList[] customerBusinessAddData)
        {
            var row = (from t in customerBusinessAddData select t).FirstOrDefault();
            if (row != null)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_customer_business_add", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@name_tha", SqlDbType.VarChar, 100).Value = row.name_tha;
                            cmd.Parameters.Add("@name_eng", SqlDbType.VarChar, 100).Value = row.name_eng;
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
        public static CustomerBusinessList GetEditCustomerBusinessData(string id, int index)
        {
            //  Set active row
            HttpContext.Current.Session["ROW_ID"] = id;
            HttpContext.Current.Session["ROW"] = index;

            var customerBusinessData = new CustomerBusinessList();
            var dsDataCustomerBusiness = new DataSet();
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
                    dsDataCustomerBusiness = SqlHelper.ExecuteDataset(conn, "sp_customer_business_list", arrParm.ToArray());
                    conn.Close();
                }

                if (dsDataCustomerBusiness.Tables.Count > 0)
                {
                    var row = dsDataCustomerBusiness.Tables[0].Rows[0];
                    customerBusinessData.id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]);
                    customerBusinessData.name_tha = Convert.IsDBNull(row["name_tha"]) ? null : Convert.ToString(row["name_tha"]);
                    customerBusinessData.name_eng = Convert.IsDBNull(row["name_eng"]) ? null : Convert.ToString(row["name_eng"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return customerBusinessData;
        }

        [WebMethod]
        public static string UpdateCustomerBusiness(CustomerBusinessList[] customerBusinessUpdateData)
        {
            var row = (from t in customerBusinessUpdateData select t).FirstOrDefault();
            if (row != null)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_customer_business_edit", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(row.id);
                            cmd.Parameters.Add("@name_tha", SqlDbType.VarChar, 100).Value = row.name_tha;
                            cmd.Parameters.Add("@name_eng", SqlDbType.VarChar, 100).Value = row.name_eng;
                            cmd.Parameters.Add("@is_enable", SqlDbType.Bit).Value = true;
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
        public static string DeleteCustomerBusiness(string id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_customer_business_delete", conn))
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
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = e.Parameters.ToString() },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 }
                        };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_business_list", arrParm.ToArray());
                    conn.Close();
                    Session["SESSION_CUSTOMER_BUSINESS_MASTER"] = dsResult;

                    Session["SEARCH"] = e.Parameters.ToString();
                }

                //Bind data into GridView
                gridView.DataSource = dsResult;
                gridView.FilterExpression = FilterBag.GetExpression(false);
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