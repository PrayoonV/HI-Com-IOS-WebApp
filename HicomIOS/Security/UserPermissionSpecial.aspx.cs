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

namespace HicomIOS.Security
{
    public partial class UserPermission : MasterDetailPage
    {
        private DataSet dsResult;
        public class UserPermissionList
        {
            public int id { get; set; }
            public int is_head_menu { get; set; }
            public int parent_id { get; set; }
            public int screen_id { get; set; }
            public string name_tha { get; set; }
            public bool is_view { get; set; }
            public bool is_create { get; set; }
            public bool is_edit { get; set; }
            public bool is_del { get; set; }
        }
        List<UserPermissionList> UserGroupDetailList
        {
            get
            {
                if (Session["SESSION_USER_PERMISSION"] == null)
                    Session["SESSION_USER_PERMISSION"] = new List<UserPermissionList>();
                return (List<UserPermissionList>)Session["SESSION_USER_PERMISSION"];
            }
            set
            {
                Session["SESSION_USER_PERMISSION"] = value;
            }
        }
        public override string PageName { get { return "UserGroup"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }

        // ClientInstanceName that use in Script.js
        private string strClientInstanceName_gridView;
        private string strClientInstanceName_UserControlEditForm = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Load Combobox Customer Group Data
            SPlanetUtil.BindASPxComboBox(ref cboUserGroup, DataListUtil.DropdownStoreProcedureName.Security_User_Group);

            // Setting height gridView
            gridView.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen);

            //Bind Data into GridView
            if (!Page.IsPostBack)
            {
                Session.Remove("SESSION_USER_PERMISSION");
                dsResult = null;
                BindGrid(true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "SetCheckboxSwitch", "setTimeout(function() { setCheckboxSwitch() }, 150);", true);
                dsResult = (DataSet)ViewState["dsResult"];
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
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                             new SqlParameter("@id", SqlDbType.Int) { Value = 0 }
                        };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_security_permission_screen_list", arrParm.ToArray());
                        //ViewState["dsResult"] = dsResult;
                    }
                }

                //Bind data into GridView
                gridView.DataSource = UserGroupDetailList;
                gridView.FilterExpression = FilterBag.GetExpression(false);
                gridView.DataBind();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }

        [WebMethod]
        public static List<UserPermissionList> SelectUserGroupList(string id)
        {
            List<UserPermissionList> data = (List<UserPermissionList>)HttpContext.Current.Session["SESSION_USER_PERMISSION"];
            var dsResult = new DataSet();

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                // Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(id) },
                            new SqlParameter("@screen_name", SqlDbType.VarChar, 200) { Value = "" }
                        };
                conn.Open();
                dsResult = SqlHelper.ExecuteDataset(conn, "sp_security_permission_screen_list", arrParm.ToArray());
            }

            if (dsResult.Tables[0].Rows.Count > 0)
            {
                data = new List<UserPermissionList>();
                foreach (var dataUserPermission in dsResult.Tables[0].AsEnumerable())
                {
                    data.Add(new UserPermissionList
                    {
                        id = Convert.ToInt32(dataUserPermission["id"]),
                        screen_id = Convert.ToInt32(dataUserPermission["screen_id"]),
                        is_head_menu = Convert.ToInt32(dataUserPermission["is_head_menu"]),
                        parent_id = Convert.ToInt32(dataUserPermission["parent_id"]),
                        name_tha = Convert.ToString(dataUserPermission["name_tha"]),
                        is_view = Convert.ToBoolean(dataUserPermission["is_view"]),
                        is_create = Convert.ToBoolean(dataUserPermission["is_create"]),
                        is_edit = Convert.ToBoolean(dataUserPermission["is_edit"]),
                        is_del = Convert.ToBoolean(dataUserPermission["is_del"])
                    });
                }
            }

            HttpContext.Current.Session["SESSION_USER_PERMISSION"] = data;
            return data;
        }

        protected void gridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridView.DataSource = UserGroupDetailList;
            gridView.FilterExpression = FilterBag.GetExpression(false);
            gridView.DataBind();
        }

        [WebMethod]
        public static string SetPermissionUserGroup(string id, string column, string is_enable)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_security_set_permission_group", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(id);
                        cmd.Parameters.Add("@column", SqlDbType.VarChar, 150).Value = Convert.ToString(column);
                        cmd.Parameters.Add("@is_enable", SqlDbType.Bit).Value = Convert.ToInt32(is_enable);

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

    }
}