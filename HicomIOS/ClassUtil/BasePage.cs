using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using HicomIOS.ClassUtil;

public abstract class BasePage : System.Web.UI.Page
{
    public abstract string PageName { get; }

    #region "Filter Function"
    public abstract FilterBag FilterBag { get; }
    public abstract IEnumerable<FilterControlColumn> GetFilterColumns();
    public abstract void OnFilterChanged();
    public virtual void SaveEditFormChanges(string args) { }
    public virtual void DeleteEntry(string args) { }
    public virtual void RefreshEntry() { }

    #endregion
    public class PermissionScreeen
    {
        public string name_eng { get; set; }
        public bool is_view { get; set; }
        public bool is_create { get; set; }
        public bool is_edit { get; set; }
        public bool is_del { get; set; }
        public bool is_print { get; set; }
    }
    public static string TopMenu
    {
        get { return HttpContext.Current.Session["APP_TOPMENU"] == null ? string.Empty : HttpContext.Current.Session["APP_TOPMENU"].ToString(); }
        set { HttpContext.Current.Session["APP_TOPMENU"] = value; }
    }
    public static string SideMenu
    {
        get { return HttpContext.Current.Session["APP_SIDEMENU"] == null ? string.Empty : HttpContext.Current.Session["APP_SIDEMENU"].ToString(); }
        set { HttpContext.Current.Session["APP_SIDEMENU"] = value; }
    }
    public static string SelectedTopMenu
    {
        get { return HttpContext.Current.Session["APP_SELECTED_TOPMENU"] == null ? string.Empty : HttpContext.Current.Session["APP_SELECTED_TOPMENU"].ToString(); }
        set { HttpContext.Current.Session["APP_SELECTED_TOPMENU"] = value; }
    }
    public static string SizeScreen
    {
        get { return HttpContext.Current.Session["APP_SIZE_SCREEN"] == null ? string.Empty : HttpContext.Current.Session["APP_SIZE_SCREEN"].ToString(); }
        set { HttpContext.Current.Session["APP_SIZE_SCREEN"] = value; }
    }
    public static PermissionScreeen SESSION_PERMISSION_SCREEN
    {
        get { return HttpContext.Current.Session["PERMISSION_SCREEN"] == null ? new PermissionScreeen() : (PermissionScreeen)HttpContext.Current.Session["PERMISSION_SCREEN"]; }
        set { HttpContext.Current.Session["PERMISSION_SCREEN"] = value; }
    }

    public static void mm()
    {
    }

    public BasePage()
    {
        this.Load += new EventHandler(this.Page_Load);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }
}

public abstract class MasterDetailPage : BasePage
{
    public virtual long SelectedItemID { get; set; }
}

public abstract class EditFormUserControl : UserControl
{
    public abstract long SaveChanges(string args);
}
