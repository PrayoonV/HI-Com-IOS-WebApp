using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace HicomIOS.ClassUtil
{
    public class Permission
    {
        public static void GetPermission(int user_group_id, string page_name)
        {
            BasePage.SESSION_PERMISSION_SCREEN = new BasePage.PermissionScreeen();
            BasePage.SESSION_PERMISSION_SCREEN.name_eng = page_name;
            BasePage.SESSION_PERMISSION_SCREEN.is_view = true;
            BasePage.SESSION_PERMISSION_SCREEN.is_create = true;
            BasePage.SESSION_PERMISSION_SCREEN.is_edit = true;
            BasePage.SESSION_PERMISSION_SCREEN.is_del = true;
            BasePage.SESSION_PERMISSION_SCREEN.is_print = true;
        }
        public static bool GetPermission()
        {
            var dsDataPermission = new DataSet();
            try
            {
                System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
                string strBrowserInfo = browser.Browser + " (version " + browser.Version + ")";
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID) },
                            new SqlParameter("@group_id", SqlDbType.Int) { Value = Convert.ToInt32(ConstantClass.SESSION_USER_GROUP_ID) },
                            new SqlParameter("@ip_address", SqlDbType.VarChar,200) { Value = Convert.ToString(SPlanetUtil.GetClientIP()) },
                            new SqlParameter("@absolute_path", SqlDbType.VarChar,200) { Value = Convert.ToString(HttpContext.Current.Request.Url.AbsolutePath) },
                            new SqlParameter("@path_query", SqlDbType.VarChar,500) { Value = Convert.ToString(HttpContext.Current.Request.Url.PathAndQuery) },
                            new SqlParameter("@user_agent", SqlDbType.VarChar,500) { Value = Convert.ToString(HttpContext.Current.Request.UserAgent) },
                            new SqlParameter("@browser_version", SqlDbType.VarChar,200) { Value = Convert.ToString(strBrowserInfo) }
                        };
                    conn.Open();
                    dsDataPermission = SqlHelper.ExecuteDataset(conn, "sp_security_permission_screen_list", arrParm.ToArray());
                    conn.Close();
                }

                if (dsDataPermission.Tables[0].Rows.Count > 0)
                {
                    var row = dsDataPermission.Tables[0].Rows[0];
                    BasePage.SESSION_PERMISSION_SCREEN = new BasePage.PermissionScreeen();
                    BasePage.SESSION_PERMISSION_SCREEN.name_eng = Convert.IsDBNull(row["name_eng"]) ? null : Convert.ToString(row["name_eng"]);
                    BasePage.SESSION_PERMISSION_SCREEN.is_view = Convert.IsDBNull(row["is_view"]) ? false : Convert.ToBoolean(row["is_view"]);
                    BasePage.SESSION_PERMISSION_SCREEN.is_create = Convert.IsDBNull(row["is_create"]) ? false : Convert.ToBoolean(row["is_create"]);
                    BasePage.SESSION_PERMISSION_SCREEN.is_edit = Convert.IsDBNull(row["is_edit"]) ? false : Convert.ToBoolean(row["is_edit"]);
                    BasePage.SESSION_PERMISSION_SCREEN.is_del = Convert.IsDBNull(row["is_del"]) ? false : Convert.ToBoolean(row["is_del"]);
                    BasePage.SESSION_PERMISSION_SCREEN.is_print = Convert.IsDBNull(row["is_print"]) ? false : Convert.ToBoolean(row["is_print"]);
                    if (BasePage.SESSION_PERMISSION_SCREEN.is_view)
                        return true;
                    else
                        return false;
                }
                else {
                    BasePage.SESSION_PERMISSION_SCREEN = new BasePage.PermissionScreeen();
                    BasePage.SESSION_PERMISSION_SCREEN.name_eng = "No Permission";
                    BasePage.SESSION_PERMISSION_SCREEN.is_view = false;
                    BasePage.SESSION_PERMISSION_SCREEN.is_create = false;
                    BasePage.SESSION_PERMISSION_SCREEN.is_edit = false;
                    BasePage.SESSION_PERMISSION_SCREEN.is_del = false;
                    BasePage.SESSION_PERMISSION_SCREEN.is_print = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                //ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
                return false;
            }
        }
    }
}