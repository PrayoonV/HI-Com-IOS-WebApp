﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Xml.Linq;
using System.Xml;
using HicomIOS.ClassUtil;
using System.Web.Services;

namespace HicomIOS
{
    public partial class RootMaster : System.Web.UI.MasterPage
    {
  
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(ConstantClass.SESSION_USER_ID)) //call session ,check NullOrEmpty
            //{
            //    Response.Redirect("/Login.aspx");
            //}
            //else 
            //{
            //    first_name.InnerText = ConstantClass.SESSION_FIRST_NAME + " " + " " + ConstantClass.SESSION_LAST_NAME;
            //}
            //ASPxLabel2.Text = DateTime.Now.Year + Server.HtmlDecode(" &copy; Copyright by Hi-Com Corporation (Thailand).");
            //LoadTopMenu();

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                var pageNameScript = string.Format("<script type='text/javascript'>HicomPageName = '{0}';</script>", PageName);

                Page.Header.Controls.AddAt(0, new LiteralControl(pageNameScript));

                if (string.IsNullOrEmpty(ConstantClass.SESSION_USER_ID)) //call session ,check NullOrEmpty
                {
                    string TARGET_URL = "/Login.aspx";
                    if (Page.IsCallback)
                        DevExpress.Web.ASPxWebControl.RedirectOnCallback(TARGET_URL);
                    else
                        Response.Redirect(TARGET_URL);
                }
                else
                {
                    first_name.InnerText = ConstantClass.SESSION_FIRST_NAME + " " + " " + ConstantClass.SESSION_LAST_NAME;
                    img_user.Src = "/Images/" +
                        (string.IsNullOrEmpty(ConstantClass.SESSION_PICTURE) ? "Default.png" : ConstantClass.SESSION_PICTURE);
                }
                ASPxLabel2.Text = DateTime.Now.Year + Server.HtmlDecode(" &copy; Copyright by Hi-Com Corporation (Thailand). [เว็บไซต์นี้เหมาะกับจอความละเอียดกว้าง 1366px ขึ้นไป]");
                LoadTopMenu();
            }
            catch (Exception ex)
            {
                //throw ex;
                string TARGET_URL = "/Login.aspx";
                if (Page.IsCallback)
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback(TARGET_URL);
                else
                    Response.Redirect(TARGET_URL);
            }
        }

        protected string PageName
        {
            get
            {
                var page = Page as BasePage;
                return page != null ? page.PageName : string.Empty;
            }
        }


        protected void HeadLoginStatus_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
        private void LoadTopMenu()
        {
            var dsDataPermission = new DataSet();
            try
            {
                System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
                string strBrowserInfo = browser.Browser + " (version " + browser.Version + ")";
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    conn.Open();
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
                        dsDataPermission = SqlHelper.ExecuteDataset(conn, "sp_security_permission_screen_list", arrParm.ToArray());

                    string groupId = "";
                    if (dsDataPermission.Tables[0].Rows.Count > 0)
                    {
                        var row = dsDataPermission.Tables[0].Rows[0];
                        groupId = Convert.IsDBNull(row["screen_group_id"]) ? null : Convert.ToString(row["screen_group_id"]);
                    }
                    else
                    {
                        if (Convert.ToString(HttpContext.Current.Request.Url.AbsolutePath).Contains("Default"))
                        {
                            groupId = "2";
                        }
                    }
                    
                    //  Screen list
                    List<SqlParameter> arrParm2 = new List<SqlParameter>
                    {
                        new SqlParameter("@user_id", SqlDbType.Int) { Value =  ConstantClass.SESSION_USER_ID },
                        new SqlParameter("@lang_id", SqlDbType.VarChar,3) { Value = "tha" }
                    };
                    using (DataSet dsResult = SqlHelper.ExecuteDataset(conn, "sp_security_screen_list", arrParm2.ToArray()))
                    {
                        int i = 0;

                        string newTopMenu = "";
                        string newSideMenu = "";
                        string isActiveTopMenu = "";
                        foreach (var objRow in (from t in dsResult.Tables[0].AsEnumerable()
                                                select new
                                                {
                                                    screen_group_id = t["screen_group_id"],
                                                    screen_group_name_eng = t["screen_group_name_eng"]
                                                }).Distinct().ToList())
                        {
                            if (String.IsNullOrEmpty(BasePage.SelectedTopMenu))
                            {
                                BasePage.SelectedTopMenu = objRow.screen_group_id.ToString();
                            }

                            var menuTitle = (objRow.screen_group_id.ToString().Equals("2") ? ConstantClass.SESSION_DEPARTMENT_NAME : objRow.screen_group_name_eng.ToString());
                            var strOnClick = "\"" + objRow.screen_group_id.ToString() + "\"";
                            isActiveTopMenu = objRow.screen_group_id.ToString() == groupId ? "dxm-selected" : string.Empty;
                            newTopMenu += "<a class='header_menu' onclick='selectTopMenu(" + strOnClick + ")'  >"
                                    + "<li class='dxm-content dxm-hasText dx dxm-item " + isActiveTopMenu + "' style='min-width: 5%'  id='" + objRow.screen_group_id.ToString() + "'>"
                                    + "<span class='dx-vam' >" + menuTitle + "</span></li>"
                                    + "<li class='dxm-separator' id='HeaderMenu_DXI" + i + "_IS' style='height: 23px;'><b></b></li></a>";
                        }

                        BasePage.TopMenu = newTopMenu;
                        string currentParent = "";
                        int currentChildIndex = 0;
                        int currentParentIndex = 0;
                        string isActive = "";
                        string isIn = "";

                        /*foreach (var objRow in (from t in dsResult.Tables[0].AsEnumerable()
                                                where t["screen_group_id"].ToString() == BasePage.SelectedTopMenu
                                                        && Convert.ToInt32(t["parent_id"]) == 0
                                                select t).ToList())*/
                        foreach (var objRow in (from t in dsResult.Tables[0].AsEnumerable()
                                                where Convert.ToInt32(t["parent_id"]) == 0
                                                select t).ToList())
                        {

                            if (Convert.ToBoolean(objRow["is_head_menu"]))
                            {
                                string hidden = "";
                                if (objRow["screen_group_id"].ToString() != groupId)
                                {
                                    hidden = " hidden";
                                }

                                if (currentParentIndex > 0)
                                {
                                    //newSideMenu += "</ul></li>";
                                }
                                //if (Convert.ToInt32(objRow["parent_id"]) == 0 && Convert.ToString(objRow["screen_group_name_tha"]) != "Master")
                                if (string.IsNullOrEmpty(Convert.ToString(objRow["navigate_url"])))
                                {
                                    /*newSideMenu += "<li><a class='hidden menu_group_" + Convert.ToString(objRow["screen_group_id"]) + "' href='" + (string.IsNullOrEmpty(Convert.ToString(objRow["navigate_url"])) ? "javascript:void(0);" : objRow["navigate_url"].ToString()) + "' " + (Convert.ToInt32(objRow["screen_id"]) != 60 ? "" : "target='_blank'") + ">" +
                                                    "<li data-toggle='collapse' class='" + isActive + "' data-target='#m" + objRow["screen_id"].ToString() + "'>"
                                                + objRow["screen_name"].ToString() + "</li></a>";*/
                                    newSideMenu += "<li data-toggle='collapse' data-target='#m" + objRow["screen_id"].ToString() + "' class='collapsed " + isActive + hidden + "'><a href='#' class='menu_group_" + Convert.ToString(objRow["screen_group_id"]) + "'>"
                                    + objRow["screen_name"].ToString() + "</a>";
                                }
                                else
                                {
                                    /*newSideMenu += "<li><a href='javascript:void(0);'><li data-toggle='collapse' class='" + isActive + "' data-target='#m" + objRow["screen_id"].ToString() + "'>"
                                                     + objRow["screen_name"].ToString() + "</li></a></li>";*/
                                    newSideMenu += "<li class='" + isActive + hidden + "'><a class='menu_group_" + Convert.ToString(objRow["screen_group_id"]) + "' href='" + objRow["navigate_url"].ToString() + "'>" + objRow["screen_name"].ToString() + "</a></li>";
                                }
                                currentParent = objRow["screen_id"].ToString();
                                currentChildIndex = 0;
                                currentParentIndex++;
                                /*foreach (var objRowChild in (from t in dsResult.Tables[0].AsEnumerable()
                                                             where t["screen_group_id"].ToString() == BasePage.SelectedTopMenu
                                                                     && Convert.ToInt32(t["parent_id"]) == Convert.ToInt32(currentParent)
                                                             select t).ToList())*/
                                foreach (var objRowChild in (from t in dsResult.Tables[0].AsEnumerable()
                                                             where Convert.ToInt32(t["parent_id"]) == Convert.ToInt32(currentParent)
                                                             select t).ToList())
                                {

                                    //isActive = "";
                                    isIn = "";
                                    string isCurrentParent = (from t in dsResult.Tables[0].AsEnumerable()
                                                              where t["name_eng"].ToString() == PageName
                                                              select t["parent_id"].ToString()).FirstOrDefault();
                                    string pageName = objRow["name_eng"].ToString().Trim();
                                    string titleName = objRowChild["title_name"].ToString().Trim();
                                    isIn = (!string.IsNullOrEmpty(isActive) || isCurrentParent == currentParent) ? "in" : string.Empty;

                                    isActive = (pageName == PageName && Convert.ToInt32(objRow["parent_id"]) > 0) ? "active" : string.Empty;

                                    string subMenuActive = (titleName.Equals(PageName) ? "active" : string.Empty);
                                    if (!string.IsNullOrEmpty(subMenuActive))
                                    {
                                        subMenuActive = "active";
                                    }

                                    if (currentChildIndex == 0 && objRowChild["parent_id"].ToString() == currentParent)
                                    {

                                        newSideMenu += "<ul class='sub-menu collapse " + isIn + "' id='m" + currentParent + "'>"
                                                        + "<li class='" + subMenuActive + "'><a class='menu_link_" + objRow["screen_group_id"].ToString() + "' href='" + objRowChild["navigate_url"].ToString() + "'>" + objRowChild["screen_name"].ToString() + "</a></li>";
                                        currentChildIndex++;
                                    }
                                    else
                                    {
                                        newSideMenu += "<li class='" + subMenuActive + "'><a class='menu_link_" + objRow["screen_group_id"].ToString() + "' href='" + objRowChild["navigate_url"].ToString() + "'>" + objRowChild["screen_name"].ToString() + "</a></li>";
                                    }
                                }

                                if (currentChildIndex > 0) {
                                    newSideMenu += "</ul></li>";
                                }
                            }
                            else
                            {

                            }

                        }
                        newSideMenu += "</li>";
                        if (BasePage.TopMenu.Length < 10)
                        {
                            newSideMenu = "No Authentication";
                        }
                        BasePage.SideMenu = newSideMenu;
                        //LoadSideMenu();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       
    }
}