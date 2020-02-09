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
    public partial class UserPermissionAprroveDocument : MasterDetailPage
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
        public override string PageName { get { return "UserPermissionAprroveDocument"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }

        // ClientInstanceName that use in Script.js
        private string strClientInstanceName_gridView;
        private string strClientInstanceName_UserControlEditForm = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Set Permission
            Permission.GetPermission(Convert.ToInt32(ConstantClass.SESSION_USER_GROUP_ID), PageName);
            // Load Combobox Customer Group Data
            //SPlanetUtil.BindASPxComboBox(ref cboUserGroup, DataListUtil.DropdownStoreProcedureName.Security_User_Group);

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
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@group_id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@ip_address", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@absolute_path", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@path_query", SqlDbType.VarChar,500) { Value = "" },
                            new SqlParameter("@user_agent", SqlDbType.VarChar,500) { Value = "" },
                            new SqlParameter("@browser_version", SqlDbType.VarChar,200) { Value = "" }
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