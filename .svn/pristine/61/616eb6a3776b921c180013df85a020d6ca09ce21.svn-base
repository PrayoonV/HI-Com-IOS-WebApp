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
    public partial class Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery",
            new ScriptResourceDefinition
            {
                Path = "~/scripts/jquery-1.7.2.min.js",
                DebugPath = "~/scripts/jquery-1.7.2.min.js",
                CdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.1.min.js",
                CdnDebugPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.1.js"
            });
        }

        [WebMethod]
        public static string CheckLogin(string username, string password)
        {
            string status = "error";
            try
            {
                var encode_password = SPlanetUtil.Encrypt(password); //md5
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@username", SqlDbType.NChar,20) { Value = username },
                            new SqlParameter("@password", SqlDbType.Char,32) { Value = encode_password },

                        };
                    conn.Open();
                    //dsResult = SqlHelper.ExecuteDataset(conn, "sp_security_user_list", arrParm.ToArray());
                    using (DataSet dsResult = SqlHelper.ExecuteDataset(conn, "sp_security_user_list", arrParm.ToArray()))
                    {
                        if (dsResult.Tables.Count > 0)
                        {
                            DataRow objRow;
                            objRow = (from t in dsResult.Tables[0].AsEnumerable() select t).FirstOrDefault();

                            if (objRow != null)
                            {
                                status = "success";
                                ConstantClass.SESSION_USER_GROUP_ID = objRow["group_id"].ToString();
                                ConstantClass.SESSION_USER_CODE = objRow["employee_code"].ToString(); //session =>ConstantClass.cs
                                ConstantClass.SESSION_FIRST_NAME = objRow["first_name"].ToString();
                                ConstantClass.SESSION_LAST_NAME = objRow["last_name"].ToString();
                                ConstantClass.SESSION_USER_ID = objRow["id"].ToString();
                                ConstantClass.SESSION_PICTURE = objRow["cover_image"].ToString();
                                ConstantClass.SESSION_DEPARTMENT_NAME = objRow["department_name"].ToString();
                                ConstantClass.SESSION_DEPARTMENT_SERVICE_TYPE = objRow["department_service_type"].ToString();
                            }
                        }
                    }
                    
                    using (DataSet dsResult = SqlHelper.ExecuteDataset(conn, "sp_information_company"))
                    {
                        DataRow objRow;
                        if (dsResult.Tables.Count > 0)
                        {
                            objRow = (from t in dsResult.Tables[0].AsEnumerable() select t).FirstOrDefault();

                            if (objRow != null)
                            {
                                ConstantClass.VAT = Convert.ToInt32(objRow["vat"]);
                            }
                        }
                    }
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
            }
            return status;
        }

        [WebMethod]
        public static string CheckLogout()
        {
            string status = "success";

            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();

            return status;
        }
    }
}