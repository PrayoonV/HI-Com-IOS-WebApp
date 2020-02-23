using DevExpress.Web;
using HicomIOS.ClassUtil;
using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HicomIOS.Master
{
    public partial class Quotation : MasterDetailPage
    {

        public override string PageName { get { return "Create Quotation"; } }

        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }

        #region Members

        private int dataId = 0;
        private int cloneId = 0;
        private int revisionId = 0;
        public class FreeBies
        {
            public int id { get; set; }
            public string description { get; set; }
            public string quatation_no { get; set; }
            public int sort_no { get; set; }
            public bool is_deleted { get; set; }
            public DateTime? notice_date { get; set; }
            public string display_notice_date { get; set; }
        }
        public class Notification
        {
            public int id { get; set; }
            public string notice_type { get; set; }
            public DateTime notice_date { get; set; }
            public string display_notice_date { get; set; }
            public int reference_id { get; set; }
            public string reference_no { get; set; }
            public string topic { get; set; }
            public string subject { get; set; }
            public string description { get; set; }
            public bool is_deleted { get; set; }
            public bool is_read { get; set; }
        }
        public class QuotationDetail
        {
            public int id_org { get; set; }
            public int parant_id_org { get; set; }
            public int id { get; set; }
            public string quotation_no { get; set; }
            public string product_no { get; set; }
            public int product_id { get; set; }
            public int model_id { get; set; }
            public string model_name { get; set; }
            public int package_id { get; set; }
            public string running_hours { get; set; }

            public string product_type { get; set; }
            public string quotation_description { get; set; }
            public string quotation_line { get; set; }
            public string unit_code { get; set; }
            public int qty { get; set; }
            public decimal unit_price { get; set; }
            public decimal min_unit_price { get; set; }
            public string discount_type { get; set; }
            public decimal discount_percentage { get; set; }
            public decimal discount_amount { get; set; }
            public decimal discount_total { get; set; }
            public decimal total_amount { get; set; }
            public string issue_stock_no { get; set; }
            public string delivery_note_no { get; set; }
            public bool is_deleted { get; set; }
            public int sort_no { get; set; }
            public int parant_id { get; set; }

            public bool is_history_issue_mfg { get; set; }
            public string mfg_no { get; set; }
            public string quotation_line_1 { get; set; }
            public string quotation_line_2 { get; set; }
            public string quotation_line_3 { get; set; }
            public string quotation_line_4 { get; set; }
            public string quotation_line_5 { get; set; }
            public string quotation_line_6 { get; set; }
            public string quotation_line_7 { get; set; }
            public string quotation_line_8 { get; set; }
            public string quotation_line_9 { get; set; }
            public string quotation_line_10 { get; set; }

        }
        public class ProductDetailValue
        {
            public decimal total { get; set; }
            public decimal discount_total { get; set; }
        }
        public class CustomerData
        {
            public int customer_id { get; set; }
            public string customer_name_tha { get; set; }
            public string customer_name_eng { get; set; }
            public string customer_address { get; set; }
            public string contract_name { get; set; }
            public string tel { get; set; }
        }
        public class MaintenanceServiceDetail
        {
            public bool is_selected { get; set; }
            public int model_id { get; set; }
            public string model_name { get; set; }
            public int package_id { get; set; }
            public int count_year { get; set; }
            public string item_of_part { get; set; }
            public string running_hour { get; set; }
            public int part_list_id { get; set; }
            public string item_no { get; set; }
            public string part_no { get; set; }
            public string part_name_tha { get; set; }
            public string part_name_eng { get; set; }
            public int cat_id { get; set; }
            public int brand_id { get; set; }
            //public string description_tha { get; set; }
            //public string description_eng { get; set; }
            //public string stock_count_type { get; set; }
            public int quantity { get; set; }
            public int quantity_package { get; set; }
            public int quantity_reserve { get; set; }
            //public int min_qty { get; set; }
            public int unit_id { get; set; }
            public string unit_code { get; set; }
            public decimal selling_price { get; set; }
            public decimal min_selling_price { get; set; }
            public int supplier_id { get; set; }
            public string remark { get; set; }
        }
        public class AnnualServiceItem
        {
            public bool is_selected { get; set; }
            public string product_name_tha { get; set; }
            public int product_id { get; set; }
            public string mfg_no { get; set; }
            public string product_no { get; set; }
            public int qty { get; set; }
            public string unit_code { get; set; }
            public int unit_id { get; set; }
            public string project { get; set; }
        }
        public class CustomerMFG
        {
            public int id { get; set; }
            public string mfg { get; set; }
            public string model { get; set; }

        }
        public class ConfigDocument
        {
            public int id { get; set; }
            public string document_description { get; set; }
        }

        List<CustomerMFG> customerMFG
        {
            get
            {
                if (Session["SESSION_CUSTOMERMFG_QUATATION"] == null)
                    Session["SESSION_CUSTOMERMFG_QUATATION"] = new List<CustomerMFG>();
                return (List<CustomerMFG>)Session["SESSION_CUSTOMERMFG_QUATATION"];
            }
            set
            {
                Session["SESSION_CUSTOMERMFG_QUATATION"] = value;
            }
        }
        List<FreeBies> freeBieList
        {
            get
            {
                if (Session["SESSION_FREEBIE_QUATATION"] == null)
                    Session["SESSION_FREEBIE_QUATATION"] = new List<FreeBies>();
                return (List<FreeBies>)Session["SESSION_FREEBIE_QUATATION"];
            }
            set
            {
                Session["SESSION_FREEBIE_QUATATION"] = value;
            }
        }
        FreeBies selectedFreeBieList
        {
            get
            {
                if (Session["SESSION_SELECTED_FREEBIE_QUATATION"] == null)
                    Session["SESSION_SELECTED_FREEBIE_QUATATION"] = new FreeBies();
                return (FreeBies)Session["SESSION_SELECTED_FREEBIE_QUATATION"];
            }
            set
            {
                Session["SESSION_SELECTED_FREEBIE_QUATATION"] = value;
            }
        }
        List<Notification> notificationList
        {
            get
            {

                if (Session["SESSION_NOTIFICATION_QUATATION"] == null)
                    Session["SESSION_NOTIFICATION_QUATATION"] = new List<Notification>();
                return (List<Notification>)Session["SESSION_NOTIFICATION_QUATATION"];
            }
            set
            {
                Session["SESSION_NOTIFICATION_QUATATION"] = value;
            }
        }
        Notification selectedNotificationList
        {
            get
            {

                if (Session["SESSION_SELECTED_NOTIFICATION_QUATATION"] == null)
                    Session["SESSION_SELECTED_NOTIFICATION_QUATATION"] = new Notification();
                return (Notification)Session["SESSION_SELECTED_NOTIFICATION_QUATATION"];
            }
            set
            {
                Session["SESSION_SELECTED_NOTIFICATION_QUATATION"] = value;
            }
        }
        List<QuotationDetail> quotationDetailList
        {
            get
            {

                if (Session["SESSION_QUOTATION_DETAIL_QUATATION"] == null)
                    Session["SESSION_QUOTATION_DETAIL_QUATATION"] = new List<QuotationDetail>();
                return (List<QuotationDetail>)Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            }
            set
            {
                Session["SESSION_QUOTATION_DETAIL_QUATATION"] = value;
            }
        }
        List<QuotationDetail> selectedProductList
        {
            get
            {

                if (Session["SESSION_SELECTED_PRODUCT_QUATATION"] == null)
                    Session["SESSION_SELECTED_PRODUCT_QUATATION"] = new List<QuotationDetail>();
                return (List<QuotationDetail>)Session["SESSION_SELECTED_PRODUCT_QUATATION"];
            }
            set
            {
                Session["SESSION_SELECTED_PRODUCT_QUATATION"] = value;
            }
        }
        QuotationDetail selectedProductEdit
        {
            get
            {

                if (Session["SESSION_SELECTED_PRODUCT_EDIT_QUATATION"] == null)
                    Session["SESSION_SELECTED_PRODUCT_EDIT_QUATATION"] = new QuotationDetail();
                return (QuotationDetail)Session["SESSION_SELECTED_PRODUCT_EDIT_QUATATION"];
            }
            set
            {
                Session["SESSION_SELECTED_PRODUCT_EDIT_QUATATION"] = value;
            }
        }
        List<MaintenanceServiceDetail> maintenanceServicePackage
        {
            get
            {

                if (Session["SESSION_MAINTENANCE_PACKAGE_QUATATION"] == null)
                    Session["SESSION_MAINTENANCE_PACKAGE_QUATATION"] = new List<MaintenanceServiceDetail>();
                return (List<MaintenanceServiceDetail>)Session["SESSION_MAINTENANCE_PACKAGE_QUATATION"];
            }
            set
            {
                Session["SESSION_MAINTENANCE_PACKAGE_QUATATION"] = value;
            }
        }
        List<MaintenanceServiceDetail> selectedMaintenanceServicePackage
        {
            get
            {

                if (Session["SESSION_SELECTED_MAINTENANCE_PACKAGE_QUATATION"] == null)
                    Session["SESSION_SELECTED_MAINTENANCE_PACKAGE_QUATATION"] = new List<MaintenanceServiceDetail>();
                return (List<MaintenanceServiceDetail>)Session["SESSION_SELECTED_MAINTENANCE_PACKAGE_QUATATION"];
            }
            set
            {
                Session["SESSION_SELECTED_MAINTENANCE_PACKAGE_QUATATION"] = value;
            }
        }
        List<MaintenanceServiceDetail> maintenanceServicePartList
        {
            get
            {

                if (Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"] == null)
                    Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"] = new List<MaintenanceServiceDetail>();
                return (List<MaintenanceServiceDetail>)Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"];
            }
            set
            {
                Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"] = value;
            }
        }
        List<MaintenanceServiceDetail> selectedMaintenanceServicePartList
        {
            get
            {

                if (Session["SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION"] == null)
                    Session["SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION"] = new List<MaintenanceServiceDetail>();
                return (List<MaintenanceServiceDetail>)Session["SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION"];
            }
            set
            {
                Session["SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION"] = value;
            }
        }

        List<AnnualServiceItem> historyAnnualServiceItemList
        {
            get
            {

                if (Session["SESSION_HISTORY_ANNUAL_SERVICE_ITEM_QUATATION"] == null)
                    Session["SESSION_HISTORY_ANNUAL_SERVICE_ITEM_QUATATION"] = new List<AnnualServiceItem>();
                return (List<AnnualServiceItem>)Session["SESSION_HISTORY_ANNUAL_SERVICE_ITEM_QUATATION"];
            }
            set
            {
                Session["SESSION_HISTORY_ANNUAL_SERVICE_ITEM_QUATATION"] = value;
            }
        }
        List<AnnualServiceItem> annualServiceItemList
        {
            get
            {

                if (Session["SESSION_ANNUAL_SERVICE_ITEM_QUATATION"] == null)
                    Session["SESSION_ANNUAL_SERVICE_ITEM_QUATATION"] = new List<AnnualServiceItem>();
                return (List<AnnualServiceItem>)Session["SESSION_ANNUAL_SERVICE_ITEM_QUATATION"];
            }
            set
            {
                Session["SESSION_ANNUAL_SERVICE_ITEM_QUATATION"] = value;
            }
        }
        List<AnnualServiceItem> selectedAnnualServiceItemList
        {
            get
            {

                if (Session["SESSION_SELECTED_ANNUAL_SERVICE_ITEM_QUATATION"] == null)
                    Session["SESSION_SELECTED_ANNUAL_SERVICE_ITEM_QUATATION"] = new List<AnnualServiceItem>();
                return (List<AnnualServiceItem>)Session["SESSION_SELECTED_ANNUAL_SERVICE_ITEM_QUATATION"];
            }
            set
            {
                Session["SESSION_SELECTED_ANNUAL_SERVICE_ITEM_QUATATION"] = value;
            }
        }
        DataSet dtProductToSelect
        {
            get
            {

                if (Session["SESSION_PRODUCT_TO_SELECT_QUATATION"] == null)
                    Session["SESSION_PRODUCT_TO_SELECT_QUATATION"] = new DataSet();
                return (DataSet)Session["SESSION_PRODUCT_TO_SELECT_QUATATION"];
            }
            set
            {
                Session["SESSION_PRODUCT_TO_SELECT_QUATATION"] = value;
            }
        }
        DataSet dtMFGProduct
        {
            get
            {

                if (Session["SESSION_MFG_PRODUCT_QUATATION"] == null)
                    Session["SESSION_MFG_PRODUCT_QUATATION"] = new DataSet();
                return (DataSet)Session["SESSION_MFG_PRODUCT_QUATATION"];
            }
            set
            {
                Session["SESSION_MFG_PRODUCT_QUATATION"] = value;
            }
        }
        DataSet dsMaintenanceModel
        {
            get
            {

                if (Session["SESSION_PRODUCT_MA_TO_SELECT_QUATATION"] == null)
                    Session["SESSION_PRODUCT_MA_TO_SELECT_QUATATION"] = new DataSet();
                return (DataSet)Session["SESSION_PRODUCT_MA_TO_SELECT_QUATATION"];
            }
            set
            {
                Session["SESSION_PRODUCT_MA_TO_SELECT_QUATATION"] = value;
            }
        }

        private DataSet dsOrderHistory = new DataSet();
        private DataSet dsTempData = new DataSet();
        //private DataSet dtProductToSelect = new DataSet();
        private DataSet dsQuataionData = new DataSet();
        //private DataSet dsMaintenanceModel = new DataSet();
        private DataSet dsRemark = new DataSet();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // setting height gridView
            //gridQuotationDetail.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen) - 280;
            //gridViewService.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen) - 150;
            gridViewOrderHistory.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen) - 150;
            gridViewSparepart.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen) - 100;
            gridViewMaintenancePackage.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen) - 100;
            gridViewMaintenance.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen) - 100;
            gridviewAnnualServiceHistory.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen) - 100;
            //gridViewAnnualServiceItem.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen) - 100;
            gridView2.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen) - 100;

            try
            {
                dataId = Convert.ToInt32(Request.QueryString["dataId"]);
                cloneId = Convert.ToInt32(Request.QueryString["cloneId"]);
                revisionId = Convert.ToInt32(Request.QueryString["revisionId"]);
                if (!Page.IsPostBack)
                {
                    // Get Permission and if no permission, will redirect to another page.
                    if (!Permission.GetPermission())
                        Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

                    ClearWorkingSession();
                    dsOrderHistory = null;
                    PrepareData();
                    LoadData();
                    BindGridOrderHistory(true);
                    CloneQuotation(cloneId);
                    RevisionQuotation(revisionId);
                }
                else
                {
                    dsOrderHistory = (DataSet)ViewState["dsOrderHistory"];
                    //dtProductToSelect = (DataSet)ViewState["dtProductToSelect"];
                    //dsMaintenanceModel = (DataSet)ViewState["dsMaintenanceModel"];
                    dsRemark = (DataSet)ViewState["dsRemark"];
                    BindGridOrderHistory(false);
                    BindGridQuotationDetail(false);
                    BindGridFreeBies(false);
                    BindGridNotification(false);
                    BindGridMaintenance(false);
                    BindGridService(false);
                    BindGridPartList(false);
                    BindGridProduct();
                    BindGridSparePart();
                    //BindGridAnnualServiceItem();
                    BindGridHistoryAnnualService();

                }

                //  Check status Follow
                if (quotationStatus.Value == "FL" || quotationStatus.Value == "PO")
                {
                    gridViewService.Columns[0].Visible = false;
                    btnNew.Visible = true;
                } else
                {
                    btnNew.Visible = false;
                }

                //  Check discount by item
                if (cbDiscountByItem.Checked)
                {
                    if (cbbDiscountByItem.Value == "P")
                    {
                        gridViewService.Columns[8].Visible = false;
                        gridViewService.Columns[9].Visible = true;
                        gridViewService.Columns[10].Visible = true;
                    }
                    else if (cbbDiscountByItem.Value == "A")
                    {
                        gridViewService.Columns[8].Visible = true;
                        gridViewService.Columns[9].Visible = false;
                        gridViewService.Columns[10].Visible = true;
                    }
                }
                else
                {
                    gridViewService.Columns[8].Visible = false;
                    gridViewService.Columns[9].Visible = false;
                    gridViewService.Columns[10].Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Prepare Methods
        private void PermissionQuotation()
        {
            var userId = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

        }

        protected void ClearWorkingSession()
        {
            Session.Remove("SESSION_NOTIFICATION_QUATATION");
            Session.Remove("SESSION_FREEBIE_QUATATION");
            Session.Remove("SESSION_QUOTATION_DETAIL_QUATATION");
            Session.Remove("SESSION_SELECTED_PRODUCT_QUATATION");
            Session.Remove("SESSION_SELECTED_NOTIFICATION_QUATATION");
            Session.Remove("SESSION_SELECTED_FREEBIE_QUATATION");
            Session.Remove("SESSION_SELECTED_PRODUCT_EDIT_QUATATION");
            Session.Remove("SESSION_SELECTED_MAINTENANCE_PACKAGE_QUATATION");
            Session.Remove("SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION");
            Session.Remove("SESSION_MAINTENANCE_PARTLIST_QUATATION");
            Session.Remove("SESSION_MAINTENANCE_PACKAGE_QUATATION");
            Session.Remove("SESSION_PRODUCT_TO_SELECT_QUATATION");
            Session.Remove("SESSION_HISTORY_ANNUAL_SERVICE_ITEM_QUATATION");
            Session.Remove("SESSION_SELECTED_ANNUAL_SERVICE_ITEM_QUATATION");
            Session.Remove("SESSION_ANNUAL_SERVICE_ITEM_QUATATION");
            Session.Remove("SESSION_MFG_PRODUCT_QUATATION");

        }

        protected void PrepareData()
        {
            try
            {
                if (!Page.IsPostBack)
                {

                    SPlanetUtil.BindASPxComboBox(ref cbbCustomerID, DataListUtil.DropdownStoreProcedureName.Customer);


                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@lang_id", SqlDbType.VarChar,3) { Value = "eng" },
                        };
                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_product_unit", arrParm.ToArray());

                        conn.Close();
                        cbbUnitType.DataSource = dsResult;
                        cbbUnitType.TextField = "data_text";
                        cbbUnitType.ValueField = "data_value";
                        cbbUnitType.DataBind();


                    }
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@lang_id", SqlDbType.VarChar,3) { Value = "eng" },
                        };
                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_product_unit", arrParm.ToArray());

                        conn.Close();
                        cbbUnitTypeServiceDetail.DataSource = dsResult;
                        cbbUnitTypeServiceDetail.TextField = "data_text";
                        cbbUnitTypeServiceDetail.ValueField = "data_value";
                        cbbUnitTypeServiceDetail.DataBind();


                    }

                    #region Load Product List From Database
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@product_no", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                        };
                        conn.Open();
                        dtProductToSelect = SqlHelper.ExecuteDataset(conn, "sp_product_list", arrParm.ToArray());

                        dtProductToSelect.Tables[0].Columns.Add("is_selected", typeof(bool));
                        //ViewState["dtProductToSelect"] = dtProductToSelect;

                        conn.Close();
                    }
                    #endregion Load Product List From Database

                    #region Load Maintenance List From Database
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                             new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },

                        };
                        conn.Open();
                        dsMaintenanceModel = SqlHelper.ExecuteDataset(conn, "sp_maintenance_model_list_popup", arrParm.ToArray());
                        dsMaintenanceModel.Tables[0].Columns.Add("is_selected", typeof(bool));
                        //ViewState["dsMaintenanceModel"] = dsMaintenanceModel;
                        conn.Close();
                    }
                    #endregion  Load Maintenance List From Database

                    #region Load Remark List From Database
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                             new SqlParameter("@remark_type", SqlDbType.NVarChar,2) { Value = "QU" },
                            new SqlParameter("@remark_type_document", SqlDbType.NVarChar,2) { Value = "P" },

                        };
                        conn.Open();
                        dsRemark = SqlHelper.ExecuteDataset(conn, "sp_remark_list", arrParm.ToArray());
                        conn.Close();
                        ViewState["dsRemark"] = dsRemark;

                    }
                    #endregion Load Remark List From Database

                    #region Load Config From Database
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {

                            new SqlParameter("@config_document", SqlDbType.Char,2) { Value = "QU" },
                            new SqlParameter("@config_type", SqlDbType.Char,1) { Value = cbbQuotationType.Value },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                        };
                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_config_list", arrParm.ToArray());
                        conn.Close();
                        var data = new List<ConfigDocument>();
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            //txtSubject.Value = Convert.ToString(dsResult.Tables[0].Rows[0]["config_description"]);
                            foreach (var row in dsResult.Tables[0].AsEnumerable())
                            {
                                data.Add(new ConfigDocument()
                                {
                                    id = Convert.ToInt32(row["id"]),
                                    document_description = Convert.ToString(row["config_description"])
                                });
                            }
                        }
                        cbbSubject.ValueField = "id";
                        cbbSubject.TextField = "document_description";
                        cbbSubject.DataSource = data;
                        cbbSubject.DataBind();
                    }
                    #endregion Load Config From Database



                    //Bind data into GridView
                    gridView2.DataSource = dtProductToSelect;
                    gridView2.DataBind();

                    //gridViewMaintenance.DataSource = dsMaintenanceModel;
                    //gridViewMaintenance.DataBind();

                    quotationDetailList = new List<QuotationDetail>();
                    gridQuotationDetail.DataSource = quotationDetailList;
                    gridQuotationDetail.DataBind();
                    selectedProductList = new List<QuotationDetail>();
                    notificationList = new List<Notification>();
                    freeBieList = new List<FreeBies>();

                    txtQuatationDate.Value = DateTime.UtcNow.Date.ToString("dd/MM/yyyy");

                    rdoQuotationLine.Checked = true;
                    gridQuotationDetail.Columns[3].Visible = true;
                    gridQuotationDetail.Columns[4].Visible = false;
                    gridQuotationDetail.Columns[5].Visible = false;
                    gridQuotationDetail.Columns[8].Visible = false;
                    gridQuotationDetail.Columns[9].Visible = false;
                    gridView2.Columns[3].Visible = false;
                    //dvGridService.Style.Add("display", "none");
                    dvGridService.Attributes["style"] = "display:none";

                    #region Remark Binding
                    for (var i = 0; i < dsRemark.Tables[0].AsEnumerable().Count(); i++)
                    {
                        if (i == 0)
                        {
                            txtRemark1.Value = Convert.ToString(dsRemark.Tables[0].Rows[i]["remark_description"]);
                        }
                        else if (i == 1)
                        {
                            txtRemark2.Value = Convert.ToString(dsRemark.Tables[0].Rows[i]["remark_description"]);
                        }
                        else if (i == 2)
                        {
                            txtRemark3.Value = Convert.ToString(dsRemark.Tables[0].Rows[i]["remark_description"]);
                        }
                        else if (i == 3)
                        {
                            txtRemark4.Value = Convert.ToString(dsRemark.Tables[0].Rows[i]["remark_description"]);
                        }
                        else if (i == 4)
                        {
                            txtRemark5.Value = Convert.ToString(dsRemark.Tables[0].Rows[i]["remark_description"]);
                        }
                        else if (i == 5)
                        {
                            txtRemark6.Value = Convert.ToString(dsRemark.Tables[0].Rows[i]["remark_description"]);
                        }
                        else if (i == 6)
                        {
                            txtRemark7.Value = Convert.ToString(dsRemark.Tables[0].Rows[i]["remark_description"]);
                        }
                        else if (i == 7)
                        {
                            txtRemark8.Value = Convert.ToString(dsRemark.Tables[0].Rows[i]["remark_description"]);
                        }
                        else if (i == 8)
                        {
                            txtRemark9.Value = Convert.ToString(dsRemark.Tables[0].Rows[i]["remark_description"]);
                        }
                        else if (i == 9)
                        {
                            txtRemarkOther.Value = Convert.ToString(dsRemark.Tables[0].Rows[i]["remark_description"]);
                        }
                    }
                    #endregion


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void LoadData()
        {
            if (dataId != 0) // Edit Mode
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = dataId },
                        };
                    conn.Open();
                    dsQuataionData = SqlHelper.ExecuteDataset(conn, "sp_quotation_list_data_qu", arrParm.ToArray());
                    ViewState["dsQuataionData"] = dsQuataionData;
                    conn.Close();
                }
                if (dsQuataionData.Tables.Count > 0)
                {
                    #region Quotation Header
                    // Quotation Header
                    var data = dsQuataionData.Tables[0].AsEnumerable().FirstOrDefault();
                    if (data != null)
                    {
                        cbbQuotationType.Value = Convert.ToString(data["quotation_type"]);
                         cbbSubject_Callback(null, null);

                        txtAddress.Value = Convert.IsDBNull(data["address_bill_tha"]) ? string.Empty : Convert.ToString(data["address_bill_tha"]);
                        //txtAttention.Value = Convert.IsDBNull(data["attention_name"]) ? string.Empty : Convert.ToString(data["attention_name"]);

                        txtGrandTotal.Value = Convert.IsDBNull(data["grand_total"]) ? string.Empty : String.Format("{0:n}", Convert.ToDouble(data["grand_total"]));
                        txtProject.Value = Convert.IsDBNull(data["project_name"]) ? string.Empty : Convert.ToString(data["project_name"]);
                        txtQuatationDate.Value = Convert.ToString(Convert.ToDateTime(data["quotation_date"]).ToString("dd/MM/yyyy"));
                        txtQuatationNo.Value = Convert.IsDBNull(data["quotation_no"]) ? string.Empty : Convert.ToString(data["quotation_no"]);
                        cbbSubject.Text = Convert.IsDBNull(data["quotation_subject"]) ? string.Empty : Convert.ToString(data["quotation_subject"]);
                        //cbbModel.Text = Convert.IsDBNull(data["model"]) ? "" : Convert.ToString(data["model"]) == "0" ? "" : Convert.ToString(data["model"]);
                        //cbbModel.Value = Convert.IsDBNull(data["model"]) ? "" : Convert.ToString(data["model"]) == "0" ? "" : Convert.ToString(data["model"]);
                        cbbCustomerID.Value = Convert.ToString(data["customer_id"]);
                        //cbbQuotationType.Value = Convert.ToString(data["quotation_type"]);
                        Cbbcurrency.Value = Convert.ToString(data["currency"]);
                        cbDiscountByItem.Checked = Convert.IsDBNull(data["is_discount_by_item"]) ? false : Convert.ToBoolean(data["is_discount_by_item"]);
                        cbDiscountBottomBill1.Checked = string.IsNullOrEmpty(Convert.ToString(data["discount1_type"])) ? false :
                                                                                            (Convert.ToString(data["discount1_type"]) == "N" ? false : true);
                        cbDiscountBottomBill2.Checked = string.IsNullOrEmpty(Convert.ToString(data["discount2_type"])) ? false :
                                                                                            (Convert.ToString(data["discount2_type"]) == "N" ? false : true);

                        cbbDiscountBottomBill1.Value = Convert.IsDBNull(data["discount1_type"]) ? string.Empty : Convert.ToString(data["discount1_type"]);
                        cbbDiscountBottomBill2.Value = Convert.IsDBNull(data["discount2_type"]) ? string.Empty : Convert.ToString(data["discount2_type"]);

                        txtTotal.Value = Convert.IsDBNull(data["total_amount"]) ? string.Empty : String.Format("{0:n}", Convert.ToDouble(data["total_amount"]));

                        rdoLumpsum.Checked = Convert.IsDBNull(data["is_lump_sum"]) ? false : Convert.ToBoolean(data["is_lump_sum"]);
                        rdoNotLumpsum.Checked = Convert.IsDBNull(data["is_lump_sum"]) ? false : !Convert.ToBoolean(data["is_lump_sum"]);

                        cbShowVat.Checked = Convert.IsDBNull(data["is_vat"]) ? false : Convert.ToBoolean(data["is_vat"]);
                        cbIsNet.Checked = Convert.IsDBNull(data["is_net"]) ? false : Convert.ToBoolean(data["is_net"]);
                        cbContactor.Checked = Convert.IsDBNull(data["is_contactor"]) ? false : Convert.ToBoolean(data["is_contactor"]);
                        rdoDescription.Checked = Convert.IsDBNull(data["is_product_description"]) ? false : !Convert.ToBoolean(data["is_product_description"]);
                        rdoQuotationLine.Checked = Convert.IsDBNull(data["is_product_description"]) ? false : Convert.ToBoolean(data["is_product_description"]);
                        txtContractNo.Value = Convert.IsDBNull(data["contract_no"]) ? string.Empty : Convert.ToString(data["contract_no"]);
                        txtAttentionTel.Value = Convert.IsDBNull(data["attention_tel"]) ? string.Empty : Convert.ToString(data["attention_tel"]);
                        txtAttentionEmail.Value = Convert.IsDBNull(data["attention_email"]) ? string.Empty : Convert.ToString(data["attention_email"]);

                        txtHour.Value = Convert.IsDBNull(data["hour_amount"]) ? string.Empty : Convert.ToString(data["hour_amount"]);
                        txtPressure.Value = Convert.IsDBNull(data["pressure"]) ? string.Empty : Convert.ToString(data["pressure"]);
                        txtMachine.Value = Convert.IsDBNull(data["machine"]) ? string.Empty : Convert.ToString(data["machine"]);

                        txtShowMFG.Value = Convert.IsDBNull(data["mfg"]) ? string.Empty : Convert.ToString(data["mfg"]);
                        //cbbModel.Text += ("; " + txtShowMFG.Value);
                        //cbbModel.Value = Convert.IsDBNull(data["model_id"]) ? string.Empty : Convert.ToString(data["model_id"]) == "0" ? "" : Convert.ToString(data["model_id"]);

                        var sales_id = Convert.IsDBNull(data["sales_id"]) ? 0 : Convert.ToInt32(data["sales_id"]);
                        CheckPermission(sales_id);
                        txtCloneQuotation.Value = Convert.IsDBNull(data["refer_no_for_repeat"]) ? string.Empty : Convert.ToString(data["refer_no_for_repeat"]);
                        txtRevision.Value = Convert.IsDBNull(data["revision"]) ? string.Empty : Convert.ToString(data["revision"]);
                        txtLocation1.Value = Convert.IsDBNull(data["location1"]) ? string.Empty : Convert.ToString(data["location1"]);
                        txtLocation2.Value = Convert.IsDBNull(data["location2"]) ? string.Empty : Convert.ToString(data["location2"]);
                        quotationStatus.Value = Convert.IsDBNull(data["quotation_status"]) ? string.Empty : Convert.ToString(data["quotation_status"]);
                        //StatusQuotation.Value = Convert.IsDBNull(data["quotation_status"]) ? string.Empty : Convert.ToString(data["quotation_status"]);
                        //txtStatusQuotation.Value = Convert.IsDBNull(data["remark_status"]) ? string.Empty : Convert.ToString(data["remark_status"]);
                        /////////////////////////
                        if (Convert.ToString(data["quotation_status"]) == "DR" || Convert.ToString(data["quotation_status"]) == "RE") // DRAFT
                        {
                            btnDraft.Visible = true;
                            btnSave.Visible = true;
                            dvApprove1Button.Visible = false;
                            dvApprove2Button.Visible = false;

                        }
                        if (Convert.ToString(data["quotation_status"]) == "W") // SEND APPROVE
                        {
                            btnDraft.Visible = false;
                            btnSave.Visible = false;
                            dvApprove1Button.Visible = true;
                            dvApprove2Button.Visible = false;
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ReadOnly Input", "readOnyInput()", true);
                            btnConfirm.Enabled = true;
                            btnAddQuotationDetail.Visible = false;
                            btnAddServiceDetail.Visible = false;
                            btnAddPartList.Visible = false;

                        }
                        if (Convert.ToString(data["quotation_status"]) == "CP") // COMPLETE
                        {
                            //status_Quotation.Attributes["style"] = "display:none";
                            btnDraft.Visible = false;
                            btnSave.Visible = false;
                            dvApprove1Button.Visible = false;
                            dvApprove2Button.Visible = false;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "Disabled Input", "disableInput()", true);
                            btnAddQuotationDetail.Visible = false;
                            btnAddServiceDetail.Visible = false;
                            btnAddPartList.Visible = false;
                            // txtStatusQuotation.Visible = false;
                        }
                        if (Convert.ToString(data["quotation_status"]) == "FL")  // APPROVE
                        {
                            //status_Quotation.Attributes["style"] = "display:none";
                            //txtStatusQuotation.Disabled = true;
                            dvApprove1Button.Visible = false;
                            dvApprove2Button.Visible = true;
                            btnCancel.Enabled = true;
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ReadOnly Input", "readOnyInput()", true);
                            btnConfirm.Enabled = true;
                            btnAddQuotationDetail.Visible = false;
                            btnAddServiceDetail.Visible = false;
                            btnAddPartList.Visible = false;
                            btnDraft.Visible = true;
                            btnSave.Visible = false;
                        }

                        ///////////////////////////////////////////////////////////////////
                        if (Convert.ToString(data["quotation_status"]) == "PO") // PO
                        {
                            btnDraft.Visible = false;
                            btnSave.Visible = false;
                            //txtStatusQuotation.Attributes["style"] = "display:none";
                            //StatusQuotation.Disabled = true;
                            dvApprove1Button.Visible = false;
                            dvApprove2Button.Visible = true;
                            btnCancel.Enabled = true;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "Disabled Input", "readOnyInput()", true);
                            btnAddQuotationDetail.Visible = false;
                            btnAddServiceDetail.Visible = false;
                            btnAddPartList.Visible = false;
                            Button28.Visible = false;
                            Button26.Visible = false;
                        }
                        if (Convert.ToString(data["quotation_status"]) == "CC") // CANCEL
                        {
                            btnDraft.Visible = false;
                            btnSave.Visible = false;
                            //StatusQuotation.Disabled = true;
                            // txtStatusQuotation.Disabled = true;
                            dvApprove1Button.Visible = false;
                            dvApprove2Button.Visible = false;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "Disabled Input", "disableInput()", true);
                            btnAddQuotationDetail.Visible = false;
                            btnAddServiceDetail.Visible = false;
                            btnAddPartList.Visible = false;
                        }
                        if (Convert.ToString(data["quotation_status"]) == "LO") // LOST
                        {
                            btnDraft.Visible = false;
                            btnSave.Visible = false;
                            dvApprove1Button.Visible = false;
                            dvApprove2Button.Visible = false;
                            //StatusQuotation.Disabled = true;
                            //txtStatusQuotation.Disabled = true;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "Disabled Input", "disableInput()", true);
                            btnAddQuotationDetail.Visible = false;
                            btnAddServiceDetail.Visible = false;
                            btnAddPartList.Visible = false;

                        }
                        ////////////////////////////////////////////////////////////

                        if (Convert.ToString(data["discount1_type"]) == "P")
                        {

                            txtDiscountBottomBill1.Value = Convert.ToString(data["discount1_percentage"]);
                        }
                        else if (Convert.ToString(data["discount1_type"]) == "A")
                        {
                            txtDiscountBottomBill1.Value = Convert.ToString(data["discount1_amount"]);
                        }
                        if (Convert.ToString(data["discount2_type"]) == "P")
                        {
                            txtDiscountBottomBill2.Value = Convert.ToString(data["discount2_percentage"]);
                        }
                        else if (Convert.ToString(data["discount2_type"]) == "A")
                        {
                            txtDiscountBottomBill2.Value = Convert.ToString(data["discount2_amount"]);
                        }
                        txtSumDiscount1.Value = Convert.IsDBNull(data["discount1_total"]) ? "0" : String.Format("{0:n}", Convert.ToDouble(data["discount1_total"]));
                        txtSumDiscount2.Value = Convert.IsDBNull(data["discount2_total"]) ? "0" : String.Format("{0:n}", Convert.ToDouble(data["discount2_total"]));

                        using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                        {
                            //Create array of Parameters
                            List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = cbbCustomerID.Value }, //4
                        };
                            conn.Open();
                            var dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_attention_list", arrParm.ToArray());

                            conn.Close();
                            if (dsResult.Tables.Count > 0)
                            {
                                if (dsResult.Tables[0].Rows.Count > 0)
                                {
                                    cbbCustomerAttention.DataSource = dsResult;
                                    cbbCustomerAttention.TextField = "attention_name";
                                    cbbCustomerAttention.ValueField = "id";
                                    cbbCustomerAttention.DataBind();
                                }
                            }
                        }
                        cbbCustomerAttention.Value = Convert.IsDBNull(data["customer_attention_id"]) ? string.Empty : Convert.ToString(data["customer_attention_id"]);

                        using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                        {
                            //Create array of Parameters
                            List<SqlParameter> arrParm = new List<SqlParameter>
                                {
                                    new SqlParameter("@customer_id", SqlDbType.Int) { Value = cbbCustomerID.Value },
                                };
                            conn.Open();
                            var dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_mfg_list_quotation", arrParm.ToArray());

                            conn.Close();
                            if (dsResult.Tables.Count > 0)
                            {
                                if (dsResult.Tables[0].Rows.Count > 0)
                                {
                                    cbbModel.DataSource = dsResult;
                                    cbbModel.TextField = "model";
                                    cbbModel.ValueField = "id";
                                    cbbModel.DataBind();
                                }
                            }
                        }

                        cbbModel.Text = Convert.IsDBNull(data["model"]) ? "" : Convert.ToString(data["model"]) == "0" ? "" : Convert.ToString(data["model"]);
                        cbbModel.Value = Convert.IsDBNull(data["model"]) ? "" : Convert.ToString(data["model"]) == "0" ? "" : Convert.ToString(data["model"]);
                        hdModelName.Value = cbbModel.Text;

                        cbbModel.Text += ("; " + txtShowMFG.Value);
                        cbbModel.Value = Convert.IsDBNull(data["model_id"]) ? string.Empty : Convert.ToString(data["model_id"]) == "0" ? "" : Convert.ToString(data["model_id"]);
                        //cbbModel.Value = Convert.IsDBNull(data["id"]) ? "0" : Convert.ToString(data["id"]);

                        var created_by = Convert.IsDBNull(data["created_by"]) ? 0 : Convert.ToInt32(data["created_by"]);
                        CheckPermission(created_by);
                    }
                    #endregion Quotation Header

                    #region Bind Grid When Load Header
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {

                            new SqlParameter("@config_document", SqlDbType.Char,2) { Value = "QU" },
                            new SqlParameter("@config_type", SqlDbType.Char,1) { Value = cbbQuotationType.Value },
                             new SqlParameter("@id", SqlDbType.Int) { Value = 0},
                        };
                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_config_list", arrParm.ToArray());
                        conn.Close();
                        if (dsResult.Tables.Count > 0)
                        {
                            if (dsResult.Tables[0].Rows.Count > 0)
                            {
                                cbbSubject.DataSource = dsResult;
                                cbbSubject.TextField = "config_description";
                                cbbSubject.ValueField = "id";
                                cbbSubject.DataBind();
                            }
                        }
                    }
                    if (cbbQuotationType.Value == "D") // Service Change
                    {

                        contactor.Attributes["style"] = "display:none";
                        dvGridProduct.Attributes["style"] = "display:none";
                        dvGridService.Attributes["style"] = "display:''";
                        dvIsService.Attributes["style"] = "display:''";
                        dvIsProductDescription.Attributes["style"] = "display:none";
                        gridQuotationDetail.Columns[4].Visible = true;
                        gridQuotationDetail.Columns[5].Visible = false;
                        gridQuotationDetail.Columns[6].Visible = false;
                        dvIsAnnualService.Attributes["style"] = "display:none";
                        tabOtherDetail.Attributes["style"] = "display:none";

                    }
                    else if (cbbQuotationType.Value == "C") // Service Change
                    {

                        contactor.Attributes["style"] = "display:none";
                        dvGridProduct.Attributes["style"] = "display:none";
                        dvGridService.Attributes["style"] = "display:''";
                        dvIsService.Attributes["style"] = "display:''";
                        dvIsProductDescription.Attributes["style"] = "display:none";
                        gridQuotationDetail.Columns[4].Visible = true;
                        gridQuotationDetail.Columns[5].Visible = false;
                        gridQuotationDetail.Columns[6].Visible = false;
                        dvIsAnnualService.Attributes["style"] = "display:none";


                    }
                    else if (cbbQuotationType.Value == "O") // Other
                    {
                        dvGridProduct.Attributes["style"] = "display:none";
                        dvGridService.Attributes["style"] = "display:''";
                        dvIsService.Attributes["style"] = "display:''";
                        dvIsProductDescription.Attributes["style"] = "display:none";
                        gridQuotationDetail.Columns[4].Visible = true;
                        gridQuotationDetail.Columns[5].Visible = false;
                        gridQuotationDetail.Columns[6].Visible = false;
                        dvIsAnnualService.Attributes["style"] = "display:none";
                        tabOtherDetail.Attributes["style"] = "display:none";

                    }

                    else if (cbbQuotationType.Value == "P") // Product
                    {

                        dvGridProduct.Attributes["style"] = "display:''";
                        dvGridService.Attributes["style"] = "display:none";
                        dvIsService.Attributes["style"] = "display:none";
                        dvIsAnnualService.Attributes["style"] = "display:none";
                        dvIsProductDescription.Attributes["style"] = "display:none";
                        if (rdoDescription.Checked)
                        {
                            gridQuotationDetail.Columns[4].Visible = true;
                            gridQuotationDetail.Columns[5].Visible = false;
                        }
                        else
                        {
                            gridQuotationDetail.Columns[4].Visible = false;
                            gridQuotationDetail.Columns[5].Visible = true;
                        }
                        gridQuotationDetail.Columns[6].Visible = false;
                        tabOtherDetail.Attributes["style"] = "display:none";
                    }
                    else if (cbbQuotationType.Value == "S") // Spare Part
                    {
                        contactor.Attributes["style"] = "display:none";
                        dvGridProduct.Attributes["style"] = "display:''";
                        dvIsAnnualService.Attributes["style"] = "display:none";
                        dvGridService.Attributes["style"] = "display:none";
                        dvIsService.Attributes["style"] = "display:none";
                        dvIsProductDescription.Attributes["style"] = "display:none";
                        gridQuotationDetail.Columns[4].Visible = true;
                        gridQuotationDetail.Columns[5].Visible = false;
                        gridQuotationDetail.Columns[6].Visible = false;
                    }
                    else if (cbbQuotationType.Value == "M") // Maintenance
                    {
                        contactor.Attributes["style"] = "display:none";
                        dvGridProduct.Attributes["style"] = "display:''";
                        dvIsAnnualService.Attributes["style"] = "display:none";
                        dvGridService.Attributes["style"] = "display:none";
                        dvIsService.Attributes["style"] = "display:none";
                        dvIsProductDescription.Attributes["style"] = "display:none";
                        gridQuotationDetail.Columns[4].Visible = true;
                        gridQuotationDetail.Columns[5].Visible = false;
                        gridQuotationDetail.Columns[6].Visible = false;
                        btnAddPartList.Attributes["style"] = "display:''";
                        btnAddAllPartList.Attributes["style"] = "display:''";
                    }
                    else if (cbbQuotationType.Value == "A") // Annual Service
                    {

                        using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                        {
                            //Create array of Parameters
                            List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = cbbCustomerID.Value },
                            new SqlParameter("@search_text", SqlDbType.VarChar,500) {Value = "" }
                        };
                            conn.Open();
                            var dsAnnualServiceHistory = SqlHelper.ExecuteDataset(conn, "sp_annual_service_list_by_customer", arrParm.ToArray());
                            conn.Close();
                            if (dsAnnualServiceHistory != null)
                            {
                                foreach (var row in dsAnnualServiceHistory.Tables[0].AsEnumerable())
                                {
                                    historyAnnualServiceItemList.Add(new AnnualServiceItem()
                                    {
                                        is_selected = false,
                                        mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                        product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                        product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                        project = Convert.IsDBNull(row["project"]) ? string.Empty : Convert.ToString(row["project"]),
                                        unit_code = "unit",
                                    });
                                }
                            }


                        }
                        contactor.Attributes["style"] = "display:none";
                        dvIsAnnualService.Attributes["style"] = "display:''";
                        dvGridProduct.Attributes["style"] = "display:''";
                        dvGridService.Attributes["style"] = "display:none";
                        dvIsService.Attributes["style"] = "display:none";
//                        dvIsAnnualService_Discount.Attributes["style"] = "display:none";
                        dvIsProductDescription.Attributes["style"] = "display:none";
                        gridQuotationDetail.Columns[4].Visible = false;
                        gridQuotationDetail.Columns[5].Visible = false;
                        gridQuotationDetail.Columns[6].Visible = true;
                        tabOtherDetail.Attributes["style"] = "display:''";

                        //dvIsService.Style.Add("display", "none");
                    }
                    #endregion

                    #region Quotation Detail 
                    // Quotation Detail List
                    string modelName = "";
                    string runningHours = "";
                    foreach (var row in dsQuataionData.Tables[1].AsEnumerable())
                    {
                        quotationDetailList.Add(new QuotationDetail()
                        {
                            //delivery_note_no = Convert.ToString(row["delivery_note_no"]),
                            id_org = Convert.ToInt32(row["id"]),
                            parant_id_org = Convert.ToInt32(row["parent_id"]),
                            discount_amount = Convert.IsDBNull(row["discount_amount"]) ? 0 : Convert.ToDecimal(row["discount_amount"]),
                            discount_total = Convert.IsDBNull(row["discount_total"]) ? 0 : Convert.ToDecimal(row["discount_total"]),
                            discount_percentage = Convert.IsDBNull(row["discount_percentage"]) ? 0 : Convert.ToDecimal(row["discount_percentage"]),
                            discount_type = Convert.IsDBNull(row["discount_type"]) ? string.Empty : Convert.ToString(row["discount_type"]),
                            id = Convert.ToInt32(row["id"]),
                            package_id = Convert.IsDBNull(row["package_id"]) ? 0 : Convert.ToInt32(row["package_id"]),
                            running_hours = Convert.IsDBNull(row["running_hours"]) ? "" : Convert.ToString(row["running_hours"]),
                            model_id = Convert.IsDBNull(row["model_id"]) ? 0 : Convert.ToInt32(row["model_id"]),
                            model_name = Convert.IsDBNull(row["model_name"]) ? "" : Convert.ToString(row["model_name"]),
                            min_unit_price = Convert.IsDBNull(row["min_unit_price"]) ? 0.0m : Convert.ToDecimal(row["min_unit_price"]),
                            product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                            product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                            qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["qty"]),
                            quotation_description = Convert.IsDBNull(row["quotation_description"]) ? string.Empty : Convert.ToString(row["quotation_description"]),
                            quotation_no = Convert.IsDBNull(row["quotation_no"]) ? string.Empty : Convert.ToString(row["quotation_no"]),
                            total_amount = Convert.IsDBNull(row["total_amount"]) ? 0.0m : Convert.ToDecimal(row["total_amount"]),
                            unit_code = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]),
                            unit_price = Convert.IsDBNull(row["unit_price"]) ? 0.0m : Convert.ToDecimal(row["unit_price"]),
                            is_deleted = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                            sort_no = Convert.IsDBNull(row["sort_no"]) ? 1 : Convert.ToInt32(row["sort_no"]),
                            parant_id = Convert.IsDBNull(row["parent_id"]) ? 0 : Convert.ToInt32(row["parent_id"]),
                            quotation_line = Convert.IsDBNull(row["quotation_line"]) ? string.Empty : Convert.ToString(row["quotation_line"]),
                            mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                            is_history_issue_mfg = Convert.IsDBNull(row["is_history_issue_mfg"]) ? false : Convert.ToBoolean(row["is_history_issue_mfg"]),
                            quotation_line_1 = Convert.IsDBNull(row["quotation_line_1"]) ? string.Empty : Convert.ToString(row["quotation_line_1"]),
                            quotation_line_2 = Convert.IsDBNull(row["quotation_line_2"]) ? string.Empty : Convert.ToString(row["quotation_line_2"]),
                            quotation_line_3 = Convert.IsDBNull(row["quotation_line_3"]) ? string.Empty : Convert.ToString(row["quotation_line_3"]),
                            quotation_line_4 = Convert.IsDBNull(row["quotation_line_4"]) ? string.Empty : Convert.ToString(row["quotation_line_4"]),
                            quotation_line_5 = Convert.IsDBNull(row["quotation_line_5"]) ? string.Empty : Convert.ToString(row["quotation_line_5"]),
                            quotation_line_6 = Convert.IsDBNull(row["quotation_line_6"]) ? string.Empty : Convert.ToString(row["quotation_line_6"]),
                            quotation_line_7 = Convert.IsDBNull(row["quotation_line_7"]) ? string.Empty : Convert.ToString(row["quotation_line_7"]),
                            quotation_line_8 = Convert.IsDBNull(row["quotation_line_8"]) ? string.Empty : Convert.ToString(row["quotation_line_8"]),
                            quotation_line_9 = Convert.IsDBNull(row["quotation_line_9"]) ? string.Empty : Convert.ToString(row["quotation_line_9"]),
                            quotation_line_10 = Convert.IsDBNull(row["quotation_line_10"]) ? string.Empty : Convert.ToString(row["quotation_line_10"]),

                        });

                        if (quotationDetailList[quotationDetailList.Count - 1].model_name != "" && modelName == "")
                        {
                            modelName = quotationDetailList[quotationDetailList.Count - 1].model_name;
                        }
                        if (quotationDetailList[quotationDetailList.Count - 1].running_hours != "" && runningHours == "")
                        {
                            runningHours = quotationDetailList[quotationDetailList.Count - 1].running_hours;
                        }
                    }
                    HttpContext.Current.Session["SESSION_SELECTED_PRODUCT_EDIT_QUATATION"] = quotationDetailList;
                    var dataDetail = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_SELECTED_PRODUCT_EDIT_QUATATION"];
                    if (dataDetail.Count > 0)
                    {
                        var list = (from t in dataDetail select t).FirstOrDefault();
                        hdMaintenanceModelId.Value = Convert.ToString(list.model_id);
                        hdPackageId.Value = Convert.ToString(list.package_id);
                        if (list.discount_type == "P")
                        {
                            if (cbbQuotationType.Value == "P" || cbbQuotationType.Value == "S" || cbbQuotationType.Value == "M" || cbbQuotationType.Value == "A")

                            {
                                gridQuotationDetail.Columns[9].Visible = true;
                                gridQuotationDetail.Columns[10].Visible = false;
                                gridQuotationDetail.Columns[11].Visible = true;
                            }
                            else
                            {
                                gridViewService.Columns[8].Visible = false;
                                gridViewService.Columns[9].Visible = true;
                                gridViewService.Columns[10].Visible = true;

                            }

                            txtDiscountByItem.Value = Convert.ToString(list.discount_percentage);

                        }
                        else if (list.discount_type == "A")
                        {
                            if (cbbQuotationType.Value == "P" || cbbQuotationType.Value == "S" || cbbQuotationType.Value == "M" || cbbQuotationType.Value == "A")

                            {
                                gridQuotationDetail.Columns[9].Visible = true;
                                gridQuotationDetail.Columns[10].Visible = true;
                                gridQuotationDetail.Columns[11].Visible = false;

                            }
                            else
                            {
                                gridViewService.Columns[8].Visible = true;
                                gridViewService.Columns[9].Visible = false;
                                gridViewService.Columns[10].Visible = true;
                            }
                            txtDiscountByItem.Value = Convert.ToString(list.discount_amount);
                        }
                        else
                        {
                            if (cbbQuotationType.Value == "P" || cbbQuotationType.Value == "S" || cbbQuotationType.Value == "M" || cbbQuotationType.Value == "A")

                            {

                                gridQuotationDetail.Columns[10].Visible = false;
                                gridQuotationDetail.Columns[11].Visible = false;

                            }
                            else
                            {

                                gridViewService.Columns[8].Visible = false;
                                gridViewService.Columns[9].Visible = false;
                            }
                        }

                        if (cbbQuotationType.Value == "M")
                        {
                            lblModelPackage.Text = "Model : " + modelName + ", Running hour : " + runningHours;
                        }

                    }

                    #endregion Quotation Detail

                    #region Quotation Remark
                    // Qutation Remark
                    foreach (var row in dsQuataionData.Tables[2].AsEnumerable())
                    {
                        if (Convert.ToInt32(row["sort_no"]) == 1)
                        {
                            txtRemark1.Value = Convert.ToString(row["remark"]);
                        }
                        else if (Convert.ToInt32(row["sort_no"]) == 2)
                        {
                            txtRemark2.Value = Convert.ToString(row["remark"]);
                        }
                        else if (Convert.ToInt32(row["sort_no"]) == 3)
                        {
                            txtRemark3.Value = Convert.ToString(row["remark"]);
                        }
                        else if (Convert.ToInt32(row["sort_no"]) == 4)
                        {
                            txtRemark4.Value = Convert.ToString(row["remark"]);
                        }
                        else if (Convert.ToInt32(row["sort_no"]) == 5)
                        {
                            txtRemark5.Value = Convert.ToString(row["remark"]);
                        }
                        else if (Convert.ToInt32(row["sort_no"]) == 6)
                        {
                            txtRemark6.Value = Convert.ToString(row["remark"]);
                        }
                        else if (Convert.ToInt32(row["sort_no"]) == 7)
                        {
                            txtRemark7.Value = Convert.ToString(row["remark"]);
                        }

                        else if (Convert.ToInt32(row["sort_no"]) == 8)
                        {
                            txtRemark8.Value = Convert.ToString(row["remark"]);
                        }
                        else if (Convert.ToInt32(row["sort_no"]) == 9)
                        {
                            txtRemark9.Value = Convert.ToString(row["remark"]);
                        }
                        else if (Convert.ToInt32(row["sort_no"]) == 10)
                        {
                            txtRemark10.Value = Convert.ToString(row["remark"]);
                        }
                        else if (Convert.ToInt32(row["sort_no"]) == 11)
                        {
                            txtRemark11.Value = Convert.ToString(row["remark"]);
                        }
                        else if (Convert.ToInt32(row["sort_no"]) == 12)
                        {
                            txtRemarkOther.Value = Convert.ToString(row["remark"]);
                        }
                    }
                    #endregion Quotation Remark

                    #region Quotation Freebies
                    //Quotation Freebies
                    //foreach (var row in dsQuataionData.Tables[3].AsEnumerable())
                    //{
                    //    freeBieList.Add(new FreeBies()
                    //    {
                    //        description = Convert.ToString(row["description"]),
                    //        display_notice_date = Convert.ToString(row["display_notice_date"]),
                    //        id = Convert.ToInt32(row["id"]),
                    //        is_deleted = Convert.ToBoolean(row["is_delete"]),
                    //        notice_date = Convert.ToDateTime(row["notice_date"]),
                    //        quatation_no = Convert.ToString(row["quotation_no"]),
                    //        sort_no = Convert.ToInt32(row["sort_no"])
                    //    });
                    //}
                    #endregion Quotation Freebies

                    #region Quotation Notification
                    // Quotation Notification
                    foreach (var row in dsQuataionData.Tables[3].AsEnumerable())
                    {
                        notificationList.Add(new Notification()
                        {
                            description = Convert.ToString(row["description"]),
                            display_notice_date = Convert.ToDateTime(row["notice_date"]).ToString("dd/MM/yyyy HH:mm"),//Convert.ToString(row["display_notice_date"]),
                            id = Convert.ToInt32(row["id"]),
                            is_deleted = Convert.ToBoolean(row["is_delete"]),
                            notice_date = Convert.ToDateTime(row["notice_date"]),
                            notice_type = Convert.ToString(row["description"]),
                            reference_id = Convert.ToInt32(row["reference_id"]),
                            reference_no = Convert.ToString(row["reference_no"]),
                            subject = Convert.ToString(row["subject"]),
                            topic = Convert.ToString(row["topic"]),
                            is_read = Convert.ToBoolean(row["is_read"]),
                        });
                    }
                    #endregion Quotation Notification
                }

                // Bind Summary Textbox

                if (!cbDiscountByItem.Checked)
                {
                    //txtDiscountByItem.Attributes["disabled"] = "disabled";
                    cbbDiscountByItem.Attributes["disabled"] = "disabled";
                }
                if (!cbDiscountBottomBill1.Checked)
                {
                    txtDiscountBottomBill1.Attributes["disabled"] = "disabled";
                    cbbDiscountBottomBill1.Attributes["disabled"] = "disabled";

                    cbDiscountBottomBill2.Attributes["disabled"] = "disabled";
                    txtDiscountBottomBill2.Attributes["disabled"] = "disabled";
                    cbbDiscountBottomBill2.Attributes["disabled"] = "disabled";
                }
                if (!cbDiscountBottomBill2.Checked)
                {
                    txtDiscountBottomBill2.Attributes["disabled"] = "disabled";
                    cbbDiscountBottomBill2.Attributes["disabled"] = "disabled";
                }
                if (quotationDetailList.Count > 0)
                {
                    cbbDiscountByItem.Value = quotationDetailList[0].discount_type;
                }


                //cbbQuotationType.Style["disabled"] = "disabled";
                cbbQuotationType.Attributes["disabled"] = "disabled";
                cbbCustomerID.Attributes["disabled"] = "disabled";

                var quStatus = quotationStatus.Value;
                if (quStatus != "DR" && quStatus != "RE" && quStatus != "W") // DRAFT
                {
                    rdoLumpsum.Attributes["disabled"] = "disabled";
                    rdoNotLumpsum.Attributes["disabled"] = "disabled";
                }
                    
                cbContactor.Attributes["disabled"] = "disabled";
                BindGridQuotationDetail(true);
                BindGridFreeBies(false);
                BindGridNotification(false);
                //BindGridAnnualServiceItem();
                BindGridHistoryAnnualService();

                PermissionQuotation(); // APPROVE


            }
            else // New mode
            {
                // status_Quotation.Attributes["style"] = "display:none";

                if (!cbDiscountByItem.Checked)
                {
                    txtDiscountByItem.Attributes["disabled"] = "disabled";
                    cbbDiscountByItem.Attributes["disabled"] = "disabled";
                }
                if (!cbDiscountBottomBill1.Checked)
                {
                    txtDiscountBottomBill1.Attributes["disabled"] = "disabled";
                    cbbDiscountBottomBill1.Attributes["disabled"] = "disabled";

                    cbDiscountBottomBill2.Attributes["disabled"] = "disabled";
                    txtDiscountBottomBill2.Attributes["disabled"] = "disabled";
                    cbbDiscountBottomBill2.Attributes["disabled"] = "disabled";
                }
                if (!cbDiscountBottomBill2.Checked)
                {
                    txtDiscountBottomBill2.Attributes["disabled"] = "disabled";
                    cbbDiscountBottomBill2.Attributes["disabled"] = "disabled";
                }
                if (quotationDetailList.Count > 0)
                {
                    cbbDiscountByItem.Value = quotationDetailList[0].discount_type;
                }


                dvIsService.Attributes["style"] = "display:none";
                dvIsAnnualService.Attributes["style"] = "display:none";
                btnReportClient.Visible = false;
                btnSave.Visible = false;
                //txtStatusQuotation.Visible = false;
                tabOtherDetail.Attributes["style"] = "display:''";
                hdMaintenanceModelId.Value = "0";
                dvApprove1Button.Visible = false;
                dvApprove2Button.Visible = false;

            }
        }
        protected void CheckPermission(int created_by)
        {
            try
            {
                if (created_by != Convert.ToInt32(ConstantClass.SESSION_USER_ID))
                {
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "Disabled Input", "disableInput()", true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void BindGridOrderHistory(bool isForceRefreshData = false)
        {
            try
            {
                if (!Page.IsPostBack || isForceRefreshData)
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        var customerId = cbbCustomerID.Value == null ? "0" : cbbCustomerID.Value;
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = customerId },
                        };
                        conn.Open();
                        dsOrderHistory = SqlHelper.ExecuteDataset(conn, "sp_view_order_history_list", arrParm.ToArray());
                        ViewState["dsOrderHistory"] = dsOrderHistory;
                        conn.Close();
                    }
                }
                //Bind data into GridView
                //var data = (from t in dsOrderHistory.Tables[0].AsEnumerable()
                //            where t.Field<string>("quotation_status") == "CF"
                //            select t).ToList();
                gridViewOrderHistory.DataSource = dsOrderHistory;
                gridViewOrderHistory.FilterExpression = FilterBag.GetExpression(false);
                gridViewOrderHistory.DataBind();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        protected void BindGridFreeBies(bool isForceRefreshData = false)
        {
            try
            {
                if (!Page.IsPostBack || isForceRefreshData)
                {
                    //using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    //{
                    //    //Create array of Parameters
                    //    List<SqlParameter> arrParm = new List<SqlParameter>
                    //    {
                    //        new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID },
                    //        new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                    //          new SqlParameter("@customer_id", SqlDbType.Int) { Value = cbbCustomerID.Value },
                    //    };
                    //    conn.Open();
                    //    dsOrderHistory = SqlHelper.ExecuteDataset(conn, "sp_quotation_header_list", arrParm.ToArray());
                    //    ViewState["dsOrderHistory"] = dsOrderHistory;
                    //}
                }
                //Bind data into GridView
                gridViewFreeBie.DataSource = (from t in freeBieList where t.is_deleted == false select t).ToList();
                gridViewFreeBie.FilterExpression = FilterBag.GetExpression(false);
                gridViewFreeBie.DataBind();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        protected void BindGridNotification(bool isForceRefreshData = false)
        {
            try
            {
                if (!Page.IsPostBack || isForceRefreshData)
                {
                    //using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    //{
                    //    //Create array of Parameters
                    //    List<SqlParameter> arrParm = new List<SqlParameter>
                    //    {
                    //        new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID },
                    //        new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                    //    };
                    //    conn.Open();
                    //    dsOrderHistory = SqlHelper.ExecuteDataset(conn, "sp_quotation_header_list", arrParm.ToArray());
                    //    ViewState["dsOrderHistory"] = dsOrderHistory;
                    //}
                }
                //Bind data into GridView
                gridViewNotice.DataSource = (from t in notificationList where t.is_deleted == false && t.topic.Trim() == "QU" select t).ToList();
                gridViewNotice.FilterExpression = FilterBag.GetExpression(false);
                gridViewNotice.DataBind();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        protected void BindGridQuotationDetail(bool isForceRefreshData = false)
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
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@search_name", SqlDbType.Int) { Value = "" },
                            new SqlParameter("@service_type", SqlDbType.VarChar, 100) { Value = ConstantClass.SESSION_DEPARTMENT_SERVICE_TYPE },
                        };
                        conn.Open();
                        dsOrderHistory = SqlHelper.ExecuteDataset(conn, "sp_quotation_header_list", arrParm.ToArray());
                        ViewState["dsOrderHistory"] = dsOrderHistory;
                        conn.Close();
                    }
                }
                //Bind data into GridView
                if (cbbQuotationType.Value == "C" || cbbQuotationType.Value == "D" || cbbQuotationType.Value == "O")
                {
                    gridViewService.DataSource = (from t in quotationDetailList
                                                  where t.parant_id == 0
                                                  && t.is_deleted == false
                                                  select t).ToList();
                    gridViewService.DataSource = (from t in quotationDetailList
                                              where t.is_deleted == false &&
                                              t.parant_id == 0
                                              select t).ToList();


                    gridViewService.FilterExpression = FilterBag.GetExpression(false);
                    gridViewService.DataBind();

                }
                else if (cbbQuotationType.Value == "P" || cbbQuotationType.Value == "S")
                {
                    gridQuotationDetail.DataSource = (from t in quotationDetailList where t.is_deleted == false select t).ToList();
                    gridQuotationDetail.FilterExpression = FilterBag.GetExpression(false);
                    gridQuotationDetail.DataBind();
                }
                else if (cbbQuotationType.Value == "M")
                {
                    gridQuotationDetail.DataSource = (from t in quotationDetailList where t.is_deleted == false select t).ToList();
                    gridQuotationDetail.FilterExpression = FilterBag.GetExpression(false);
                    gridQuotationDetail.DataBind();
                }
                else if (cbbQuotationType.Value == "A")
                {
                    gridQuotationDetail.DataSource = (from t in quotationDetailList where t.is_deleted == false select t).OrderBy(b => b.sort_no).ToList();
                    gridQuotationDetail.FilterExpression = FilterBag.GetExpression(false);
                    gridQuotationDetail.DataBind();
                }
                if (cbDiscountByItem.Checked)
                {
                    if (cbbDiscountByItem.Value == "P")
                    {
                        gridQuotationDetail.Columns[8].Visible = false;
                        gridQuotationDetail.Columns[9].Visible = true;
                    }
                    else if (cbbDiscountByItem.Value == "A")
                    {
                        gridQuotationDetail.Columns[8].Visible = true;
                        gridQuotationDetail.Columns[9].Visible = false;
                    }
                }
                else
                {
                    if (cbbQuotationType.Value == "P" || cbbQuotationType.Value == "S" || cbbQuotationType.Value == "M" || cbbQuotationType.Value == "A")
                    {
                        gridQuotationDetail.Columns[8].Visible = false;
                        gridQuotationDetail.Columns[9].Visible = true;
                    } else
                    {
                        gridQuotationDetail.Columns[8].Visible = false;
                        gridQuotationDetail.Columns[9].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        protected void BindGridMaintenance(bool isForceRefreshData = false)
        {
            try
            {
                if (!Page.IsPostBack || isForceRefreshData)
                {

                }
                if (cbbQuotationType.Value == "S" || cbbQuotationType.Value == "M")
                {
                    //Bind data into GridView
                    gridViewMaintenance.DataSource = dsMaintenanceModel;
                    gridViewMaintenance.DataBind();

                    if (cbbQuotationType.Value == "M")
                    {
                        gridViewMaintenancePackage.DataSource = maintenanceServicePackage;
                        gridViewMaintenancePackage.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        protected void BindGridPartList(bool isForceRefreshData = false)
        {
            try
            {

                if (!Page.IsPostBack || isForceRefreshData)
                {

                }
                //Bind data into GridView
                if (cbbQuotationType.Value == "M" || cbbQuotationType.Value == "S")
                {
                    gridViewPartList.DataSource = maintenanceServicePartList;
                    gridViewPartList.DataBind();
                }

            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        protected void BindGridService(bool isForceRefreshData = false)
        {
            try
            {
                if (cbbQuotationType.Value == "C" || cbbQuotationType.Value == "D" || cbbQuotationType.Value == "O")
                {
                    gridViewService.DataSource = (from t in quotationDetailList
                                                  where t.is_deleted == false &&
                                                  t.parant_id == 0
                                                  select t).ToList();
                    gridViewService.FilterExpression = FilterBag.GetExpression(false);
                    gridViewService.DataBind();


                }
                else if (cbbQuotationType.Value == "A")
                {
                    gridQuotationDetail.DataSource = (from t in quotationDetailList where t.is_deleted == false select t).ToList();
                    gridQuotationDetail.FilterExpression = FilterBag.GetExpression(false);
                    gridQuotationDetail.DataBind();
                }
                else
                {
                    gridViewService.DataSource = null;
                    gridViewService.FilterExpression = FilterBag.GetExpression(false);
                    gridViewService.DataBind();
                }
                
                //  Check status Follow
                if (quotationStatus.Value == "FL" || quotationStatus.Value == "PO")
                {
                    gridViewService.Columns[0].Visible = false;
                }
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        protected void BindGridProduct()
        {

            if (cbbQuotationType.Value == "P")
            {
                if (dtProductToSelect.Tables[0].Rows.Count > 0)
                {
                    foreach (var row in selectedProductList.Where(t => !t.is_deleted))
                    {
                        var existItem = (from t in dtProductToSelect.Tables[0].AsEnumerable() where t.Field<int>("id") == row.product_id select t).FirstOrDefault();
                        if (existItem != null)
                        {
                            existItem["is_selected"] = true;
                        }
                    }

                }
                if (rdoDescription.Checked)
                {
                    gridView2.Columns[2].Visible = true;
                    gridView2.Columns[3].Visible = false;

                }
                else
                {

                    gridView2.Columns[2].Visible = false;
                    gridView2.Columns[3].Visible = true;
                }
                gridView2.DataSource = dtProductToSelect;
                gridView2.DataBind();
            }
        }
        protected void BindGridSparePart()
        {
            if (cbbQuotationType.Value == "S")
            {
                foreach (var row in selectedMaintenanceServicePartList)
                {
                    var existItem = (from t in maintenanceServicePartList where t.part_list_id == row.part_list_id select t).FirstOrDefault();
                    if (existItem != null)
                    {
                        existItem.is_selected = true;
                    }
                }


                gridViewSparepart.DataSource = maintenanceServicePartList;
                gridViewSparepart.DataBind();
            }
        }
        protected void BindGridHistoryAnnualService()
        {
            if (cbbQuotationType.Value == "A")
            {
                gridviewAnnualServiceHistory.DataSource = historyAnnualServiceItemList;
                gridviewAnnualServiceHistory.DataBind();
            }
        }
        //protected void BindGridAnnualServiceItem()
        //{
        //    if (cbbQuotationType.Value == "A")
        //    {
        //        gridViewAnnualServiceItem.DataSource = dtProductToSelect;
        //        gridViewAnnualServiceItem.DataBind();
        //    }
        //}
        #endregion

        #region Inherited Events

        #region "Inherited Event of Filter Control"
        public override void OnFilterChanged()
        {

        }
        #endregion

        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            return result.OfType<FilterControlColumn>();
        }

        #endregion

        #region Quotation Detail
        protected void gridQuotationDetail_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                // เปลี่ยนไปใช้ method SubmitAddProductQuotation
                //if (selectedProductList.Count > 0)
                //{
                //    foreach (var row in selectedProductList)
                //    {
                //        var checkExist = (from t in quotationDetailList where t.product_id == row.product_id select t).FirstOrDefault();
                //        if (checkExist == null)
                //        {
                //            if (row.id < 0)
                //            {
                //                row.id = (quotationDetailList.Count + 1) * -1;
                //                row.total_amount = row.qty * row.unit_price;
                //            }
                //            quotationDetailList.Add(row);
                //        }
                //        else
                //        {
                //            checkExist.qty += 1;
                //            checkExist.total_amount = checkExist.qty * checkExist.unit_price;
                //        }
                //    }
                //}

                #region Calcurate Discount 
                if (cbDiscountByItem.Checked)
                {
                    foreach (var row in quotationDetailList)
                    {
                        var totalAmount = (row.unit_price * row.qty);
                        if (cbbDiscountByItem.Value == "P")
                        {

                            row.discount_amount = 0;
                            row.total_amount = totalAmount - ((totalAmount * (Convert.ToInt32(row.discount_percentage) > 100 ? 100 : Convert.ToDecimal(row.discount_percentage))) / 100);

                        }
                        else if (cbbDiscountByItem.Value == "A")
                        {
                            row.discount_percentage = 0;
                            row.total_amount = totalAmount - Convert.ToInt32(row.discount_amount);
                        }
                    }
                }
                #endregion
                if (cbbQuotationType.Value == "P")
                {
                    if (rdoDescription.Checked)
                    {
                        gridQuotationDetail.Columns[4].Visible = true;
                        gridQuotationDetail.Columns[5].Visible = false;
                        gridQuotationDetail.Columns[6].Visible = false;

                    }
                    else if (rdoQuotationLine.Checked)
                    {
                        gridQuotationDetail.Columns[4].Visible = false;
                        gridQuotationDetail.Columns[5].Visible = true;
                        gridQuotationDetail.Columns[6].Visible = false;

                    }
                }
                if (cbDiscountByItem.Checked)
                {
                    if (cbbDiscountByItem.Value == "P")
                    {
                        gridQuotationDetail.Columns[9].Visible = true;
                        gridQuotationDetail.Columns[10].Visible = false;
                        gridQuotationDetail.Columns[11].Visible = true;
                    }
                    else if (cbbDiscountByItem.Value == "A")
                    {
                        gridQuotationDetail.Columns[9].Visible = true;
                        gridQuotationDetail.Columns[10].Visible = true;
                        gridQuotationDetail.Columns[11].Visible = false;
                    }
                }
                else
                {
                    gridQuotationDetail.Columns[10].Visible = false;
                    gridQuotationDetail.Columns[11].Visible = false;
                }

                //selectedProductList.Clear();

                if (cbbQuotationType.Value == "P")
                {
                    gridQuotationDetail.DataSource = (from t in quotationDetailList
                                                      where t.is_deleted == false
                                                      orderby t.sort_no ascending
                                                      select t).ToList();
                    gridQuotationDetail.FilterExpression = FilterBag.GetExpression(false);
                    gridQuotationDetail.DataBind();
                }
                else if (cbbQuotationType.Value == "M")
                {
                    gridQuotationDetail.Columns[4].Visible = true;
                    gridQuotationDetail.Columns[5].Visible = false;
                    gridQuotationDetail.Columns[6].Visible = false;
                    gridQuotationDetail.DataSource = (from t in quotationDetailList
                                                      where t.is_deleted == false
                                                      orderby t.sort_no ascending
                                                      select t).ToList();
                    gridQuotationDetail.FilterExpression = FilterBag.GetExpression(false);
                    gridQuotationDetail.DataBind();
                }
                else if (cbbQuotationType.Value == "S")
                {
                    gridQuotationDetail.Columns[4].Visible = true;
                    gridQuotationDetail.Columns[5].Visible = false;
                    gridQuotationDetail.Columns[6].Visible = false;
                    gridQuotationDetail.DataSource = (from t in quotationDetailList
                                                      where t.is_deleted == false
                                                      orderby t.sort_no ascending
                                                      select t).ToList();
                    gridQuotationDetail.FilterExpression = FilterBag.GetExpression(false);
                    gridQuotationDetail.DataBind();
                }
                else if (cbbQuotationType.Value == "A")
                {
                    gridQuotationDetail.Columns[4].Visible = false;
                    gridQuotationDetail.Columns[5].Visible = false;
                    gridQuotationDetail.Columns[6].Visible = true;
                    gridQuotationDetail.DataSource = (from t in quotationDetailList
                                                      where t.is_deleted == false
                                                      orderby t.sort_no ascending
                                                      select t).ToList();
                    gridQuotationDetail.FilterExpression = FilterBag.GetExpression(false);
                    gridQuotationDetail.DataBind();
                }
                else
                {
                    gridQuotationDetail.DataSource = null;
                    gridQuotationDetail.FilterExpression = FilterBag.GetExpression(false);
                    gridQuotationDetail.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gridView2_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.ToString().Contains("Search"))
            {

                var splitStr = e.Parameters.ToString().Split('|');
                string searchText = string.Empty;
                string productType = string.Empty;

                if (splitStr.Length > 0)
                {
                    searchText = splitStr[2];
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = searchText },
                            new SqlParameter("@product_no", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                        };
                        conn.Open();
                        dtProductToSelect = SqlHelper.ExecuteDataset(conn, "sp_product_list", arrParm.ToArray());
                        conn.Close();

                        dtProductToSelect.Tables[0].Columns.Add("is_selected", typeof(bool));
                    }
                }
            }
            if (dtProductToSelect.Tables[0].Rows.Count > 0)
            {
                foreach (var row in selectedProductList.Where(t => !t.is_deleted))
                {
                    var existItem = (from t in dtProductToSelect.Tables[0].AsEnumerable() where t.Field<int>("id") == row.product_id select t).FirstOrDefault();
                    if (existItem != null)
                    {
                        existItem["is_selected"] = true;
                    }
                }

            }
            if (cbbQuotationType.Value == "P")
            {
                if (rdoDescription.Checked)
                {
                    gridView2.Columns[2].Visible = true;
                    gridView2.Columns[3].Visible = false;

                }
                else if (rdoQuotationLine.Checked)
                {

                    gridView2.Columns[2].Visible = false;
                    gridView2.Columns[3].Visible = true;
                }
                gridView2.DataSource = dtProductToSelect;
                gridView2.DataBind();
                gridView2.PageIndex = 0;
            }
        }

        [WebMethod]
        public static string PopupSelectProduct()
        {
            DataSet productDs;
            List<QuotationDetail> selectedProductList = new List<QuotationDetail>();
            List<QuotationDetail> dataQuotation = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@product_no", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                        };
                conn.Open();
                productDs = SqlHelper.ExecuteDataset(conn, "sp_product_list_popup", arrParm.ToArray());
                conn.Close();

                productDs.Tables[0].Columns.Add("is_selected", typeof(bool));
            }

            selectedProductList.AddRange(dataQuotation);

            if (productDs.Tables[0].Rows.Count > 0)
            {
                foreach (var row in selectedProductList.Where(t => !t.is_deleted))
                {
                    var existItem = (from t in productDs.Tables[0].AsEnumerable() where t.Field<int>("id") == row.product_id select t).FirstOrDefault();
                    if (existItem != null)
                    {
                        existItem["is_selected"] = true;
                    }
                }

            }
            HttpContext.Current.Session["SESSION_SELECTED_PRODUCT_QUATATION"] = selectedProductList;
            HttpContext.Current.Session["SESSION_PRODUCT_TO_SELECT_QUATATION"] = productDs;
            return "Product Popup";
        }

        [WebMethod]
        public static List<QuotationDetail> SelectedProduct(string id, bool isSelected)
        {
            List<QuotationDetail> data = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_SELECTED_PRODUCT_QUATATION"];
            List<QuotationDetail> dataQuotation = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            DataSet productDs = (DataSet)HttpContext.Current.Session["SESSION_PRODUCT_TO_SELECT_QUATATION"];

            var dsResult = new DataSet();

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@product_no", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(id) },
                        };
                conn.Open();
                dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_list", arrParm.ToArray());
                conn.Close();
            }
            if (dsResult.Tables[0].Rows.Count > 0)
            {

            }
            if (data == null)
            {

                data = new List<QuotationDetail>();
                data.Add(new QuotationDetail
                {
                    id = (data.Count + 1) * -1,
                    quotation_no = "",
                    parant_id = 0,
                    sort_no = 1,
                    product_id = Convert.ToInt32(id),
                    quotation_description = dsResult.Tables[0].Rows[0]["product_name_tha"].ToString(),
                    product_no = dsResult.Tables[0].Rows[0]["product_no"].ToString(),
                    unit_code = dsResult.Tables[0].Rows[0]["unit_code"].ToString(),
                    qty = 1,//Convert.ToInt32(dsResult.Tables[0].Rows[0]["quantity"]),
                    unit_price = Convert.ToInt32(dsResult.Tables[0].Rows[0]["selling_price"]),
                    min_unit_price = Convert.ToInt32(dsResult.Tables[0].Rows[0]["min_selling_price"]),
                    quotation_line = Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_line"]),
                    package_id = 0,
                    total_amount = 1 * Convert.ToInt32(dsResult.Tables[0].Rows[0]["selling_price"]),
                    quotation_line_1 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line1"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line1"]),
                    quotation_line_2 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line2"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line2"]),
                    quotation_line_3 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line3"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line3"]),
                    quotation_line_4 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line4"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line4"]),
                    quotation_line_5 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line5"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line5"]),
                    quotation_line_6 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line6"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line6"]),
                    quotation_line_7 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line7"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line7"]),
                    quotation_line_8 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line8"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line8"]),
                    quotation_line_9 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line9"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line9"]),
                    quotation_line_10 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line10"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line10"]),
                    product_type = "P"
                });

            }
            else
            {
                if (isSelected)
                {
                    data.Add(new QuotationDetail
                    {
                        id = (data.Count + 1) * -1,
                        quotation_no = "",
                        parant_id = 0,
                        sort_no = data.Count > 0 ? (from t in data select t.sort_no).Max() + 1 : (dataQuotation.Count == 0 ? 1 : (from t in dataQuotation select t.sort_no).Max() + 1),
                        product_id = Convert.ToInt32(id),
                        quotation_description = dsResult.Tables[0].Rows[0]["product_name_tha"].ToString(),
                        product_no = dsResult.Tables[0].Rows[0]["product_no"].ToString(),
                        unit_code = dsResult.Tables[0].Rows[0]["unit_code"].ToString(),
                        qty = 1,//Convert.ToInt32(dsResult.Tables[0].Rows[0]["quantity"]),
                        unit_price = Convert.ToInt32(dsResult.Tables[0].Rows[0]["selling_price"]),
                        min_unit_price = Convert.ToInt32(dsResult.Tables[0].Rows[0]["min_selling_price"]),
                        quotation_line = Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_line"]),
                        package_id = 0,
                        total_amount = 1 * Convert.ToInt32(dsResult.Tables[0].Rows[0]["selling_price"]),
                        quotation_line_1 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line1"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line1"]),
                        quotation_line_2 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line2"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line2"]),
                        quotation_line_3 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line3"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line3"]),
                        quotation_line_4 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line4"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line4"]),
                        quotation_line_5 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line5"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line5"]),
                        quotation_line_6 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line6"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line6"]),
                        quotation_line_7 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line7"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line7"]),
                        quotation_line_8 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line8"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line8"]),
                        quotation_line_9 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line9"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line9"]),
                        quotation_line_10 = Convert.IsDBNull(dsResult.Tables[0].Rows[0]["quotation_desc_line10"]) ? string.Empty : Convert.ToString(dsResult.Tables[0].Rows[0]["quotation_desc_line10"]),
                        product_type = "P"
                    });
                }
                else
                {
                    // DELETE WHEN REMOVE CHECK
                    var existItem = (from t in data where t.product_id == Convert.ToInt32(id) && !t.is_deleted select t).FirstOrDefault();
                    if (existItem != null)
                    {

                        existItem.is_deleted = true;

                    }

                }

            }
            // ยัด is_selected ให้กับ ProductDs
            if (productDs != null)
            {
                var row = (from t in productDs.Tables[0].AsEnumerable() where t["id"].ToString() == id select t).FirstOrDefault();
                if (row != null)
                {
                    row["is_selected"] = Convert.ToBoolean(isSelected);
                }
            }
            HttpContext.Current.Session["SESSION_PRODUCT_TO_SELECT_QUATATION"] = productDs;
            HttpContext.Current.Session["SESSION_SELECTED_PRODUCT_QUATATION"] = data;
            return data;
        }

        [WebMethod]
        public static string SubmitAddProductQuotation()
        {
            var selectedProduct = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_SELECTED_PRODUCT_QUATATION"];
            var quotationDetail = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];

            if (selectedProduct != null)
            {
                foreach (var row in selectedProduct)
                {
                    var exist = quotationDetail.Where(t => t.product_id == row.product_id && !t.is_deleted).FirstOrDefault();
                    if (exist == null)
                    {
                        if (!row.is_deleted)
                        {
                            quotationDetail.Add(row);
                        }
                    }
                    else
                    {
                        if (exist.is_deleted)
                        {
                            quotationDetail.Remove(row);
                        }
                        else
                        {
                            //quotationDetail.Add(row);
                        }
                    }
                }
            }
            HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"] = quotationDetail;
            HttpContext.Current.Session["SESSION_SELECTED_PRODUCT_QUATATION"] = null;

            return "Add Product";
        }

        [WebMethod]
        public static QuotationDetail EditProductDetail(string id)
        {
            List<QuotationDetail> data = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            QuotationDetail selectedData = new QuotationDetail();
            DataSet mfgData = new DataSet();
            if (data != null)
            {
                selectedData = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault();

            }
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@product_id", SqlDbType.Int) { Value = selectedData.product_id },
                        };
                conn.Open();
                mfgData = SqlHelper.ExecuteDataset(conn, "sp_dropdown_product_mfg", arrParm.ToArray());
                conn.Close();
                HttpContext.Current.Session["SESSION_MFG_PRODUCT_QUATATION"] = mfgData;
            }
            HttpContext.Current.Session["SESSION_SELECTED_PRODUCT_EDIT_QUATATION"] = selectedData;
            return selectedData;

        }

        [WebMethod]
        public static List<QuotationDetail> DeleteProductDetail(string id)
        {
            List<QuotationDetail> data = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            if (data != null)
            {
                var row = (from t in data
                           where t.id == Convert.ToInt32(id)
                           select t).FirstOrDefault();
                if (Convert.ToInt32(id) < 0)
                {
                    data.Remove(row);
                }
                else
                {
                    row.is_deleted = true;
                }

            }

            HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"] = data;
            return data;
        }

        [WebMethod]
        public static List<QuotationDetail> SubmitProductDetail(string id, string qty, string unit_price,
                                                                string amount, string percent, string discountType, string mfgNo,
                                                                string description, string line1, string line2, string line3, string line4
                                                                 , string line5, string line6, string line7, string line8, string line9, string line10)
        {
            List<QuotationDetail> data = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];

            if (data != null)
            {
                var row = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (row != null)
                {
                    var totalAmount = Convert.ToInt32(qty) * Convert.ToDecimal(unit_price);
                    row.discount_percentage = Convert.ToInt32(percent) > 100 ? 100 : Convert.ToDecimal(percent);
                    row.discount_amount = Convert.ToInt32(amount);
                    row.qty = Convert.ToInt32(qty);
                    row.unit_price = Convert.ToDecimal(unit_price);
                    if (discountType == "P")
                    {
                        row.total_amount = totalAmount - ((totalAmount * (Convert.ToInt32(percent) > 100 ? 100 : Convert.ToDecimal(percent))) / 100);
                    }
                    else if (discountType == "A")
                    {
                        row.total_amount = totalAmount - Convert.ToInt32(amount);
                    }
                    else
                    {
                        row.total_amount = totalAmount;
                    }
                    row.mfg_no = mfgNo == "null" ? string.Empty : mfgNo;
                    row.quotation_description = description;
                    row.quotation_line_1 = line1;
                    row.quotation_line_2 = line2;
                    row.quotation_line_3 = line3;
                    row.quotation_line_4 = line4;
                    row.quotation_line_5 = line5;
                    row.quotation_line_6 = line6;
                    row.quotation_line_7 = line7;
                    row.quotation_line_8 = line8;
                    row.quotation_line_9 = line9;
                    row.quotation_line_10 = line10;
                    row.quotation_line_1 = line1;

                    row.quotation_line = row.quotation_line_1 + '\n' + row.quotation_line_2 + '\n' + row.quotation_line_3 + '\n' + row.quotation_line_4 + '\n'
                                        + row.quotation_line_5 + '\n' + row.quotation_line_6 + '\n' + row.quotation_line_7 + '\n' + row.quotation_line_8 + '\n'
                                        + row.quotation_line_9 + '\n' + row.quotation_line_10 + '\n';

                }
            }


            HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"] = data;
            HttpContext.Current.Session["SESSION_SELECTED_PRODUCT_EDIT_QUATATION"] = null;
            return data;

        }



        //public static List<QuotationDetail> EditProductDetail(string id, string value, string col)
        //{
        //    List<QuotationDetail> data = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
        //    if (data != null)
        //    {
        //        var row = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
        //        if (row != null)
        //        {
        //            if (col == "qty")
        //            {
        //                row.qty = Convert.ToInt32(value);
        //                row.total_amount = (row.qty * row.unit_price);
        //            }
        //            else if (col == "unit_price")
        //            {
        //                row.unit_price = Convert.ToInt32(value);
        //                row.total_amount = (row.qty * row.unit_price);
        //            }
        //            else if (col == "discount_percent")
        //            {
        //                row.discount_percentage = Convert.ToInt32(value);
        //                row.discount_amount = 0;
        //                row.total_amount = (row.qty * row.unit_price) - (((row.qty * row.unit_price) * Convert.ToInt32(value)) / 100);
        //            }
        //            else if (col == "discount_amount")
        //            {
        //                row.discount_amount = Convert.ToInt32(value);
        //                row.discount_percentage = 0;
        //                row.total_amount = (row.qty * row.unit_price) - Convert.ToInt32(value);
        //            }
        //        }
        //    }

        //    HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"] = data;
        //    return data;

        //}

        [WebMethod]
        public static ProductDetailValue CalcurateDiscount(string discount_type)
        {
            List<QuotationDetail> data = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            var returnData = new ProductDetailValue();
            if (data != null)
            {
                foreach (var row in (from t in data where !t.is_deleted select t).ToList())
                {

                    var totalAmount = row.unit_price * row.qty;
                    returnData.total += totalAmount;
                    if (discount_type == "P")
                    {
                        returnData.discount_total += (totalAmount * row.discount_percentage) / 100;
                        row.discount_total = (totalAmount * row.discount_percentage) / 100;
                    }
                    else if (discount_type == "A")
                    {
                        returnData.discount_total += row.discount_amount;
                        row.discount_total = row.discount_amount;
                    }
                    else
                    {
                        returnData.discount_total = 0;
                        row.discount_total = 0;
                    }
                }
            }
            return returnData;
        }

        [WebMethod]
        public static string ClearDataQuotationDetail(string quStatus)
        {
            if (quStatus == "DR" || quStatus == "RE" || quStatus == "") // DRAFT
            {
                List<QuotationDetail> data = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
                if (data != null)
                {
                    data.Clear();
                }
                HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"] = data;
            }
            return "SUCCESS";
        }


        #endregion

        #region Notification
        protected void gridViewNotice_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                gridViewNotice.DataSource = (from t in notificationList where t.is_deleted == false && t.topic == "QU" select t).ToList();
                gridViewNotice.FilterExpression = FilterBag.GetExpression(false);
                gridViewNotice.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static List<Notification> AddNotice(string id, string subject, string description, string date)
        {
            try
            {
                List<Notification> dataNotification = (List<Notification>)HttpContext.Current.Session["SESSION_NOTIFICATION_QUATATION"];

                //
                IFormatProvider culture = new System.Globalization.CultureInfo("th-TH", true);
                //DateTime dateTime = DateTime.Parse(date, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                var dateee = Convert.ToDateTime(date);
                if (dataNotification == null) // Insert mode only
                {
                    dataNotification = new List<Notification>();

                    dataNotification.Add(new Notification
                    {
                        id = (dataNotification.Count + 1) * -1,
                        subject = subject,
                        description = description,
                        reference_id = -1,
                        notice_type = "RE",
                        topic = "QU",
                        notice_date = dateee,//dateTime,
                        is_deleted = false,
                        display_notice_date = dateee.ToString("dd/MM/yyyy HH:mm")
                    });

                }
                else
                {
                    if (Convert.ToInt32(id) == 0) // Insert mode when id == 0
                    {
                        dataNotification.Add(new Notification
                        {
                            id = (dataNotification.Count + 1) * -1,
                            subject = subject,
                            description = description,
                            reference_id = -1,
                            notice_type = "RE",
                            topic = "QU",
                            notice_date = dateee,//dateTime,
                            is_deleted = false,
                            display_notice_date = dateee.ToString("dd/MM/yyyy HH:mm")
                        });
                    }
                    else
                    {
                        var row = (from t in dataNotification where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                        row.subject = subject;
                        row.description = description;
                        row.reference_id = -1;
                        row.notice_type = "RE";
                        row.topic = "QU";
                        row.notice_date = dateee;//dateTime,
                        row.is_deleted = false;
                        row.display_notice_date = dateee.ToString("dd/MM/yyyy HH:mm");
                    }

                }


                HttpContext.Current.Session["SESSION_NOTIFICATION_QUATATION"] = dataNotification;

                return (from t in dataNotification where t.is_deleted == false select t).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [WebMethod]
        public static Notification ShowEditNotice(string id)
        {
            try
            {
                List<Notification> dataNotification = (List<Notification>)HttpContext.Current.Session["SESSION_NOTIFICATION_QUATATION"];
                Notification dataEditNotification = new Notification();

                if (dataNotification != null)
                {
                    var selectedData = (from t in dataNotification where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (selectedData != null)
                    {
                        dataEditNotification = selectedData;
                    }
                }

                HttpContext.Current.Session["SESSION_SELECTED_NOTIFICATION_QUATATION"] = dataEditNotification;

                return dataEditNotification;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static List<Notification> DeleteNotice(string id)
        {
            try
            {
                List<Notification> dataNotification = (List<Notification>)HttpContext.Current.Session["SESSION_NOTIFICATION_QUATATION"];

                if (dataNotification != null)
                {
                    var selectedData = (from t in dataNotification where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (selectedData != null)
                    {
                        if (Convert.ToInt32(id) < 0)
                        {
                            dataNotification.Remove(selectedData);
                        }
                        else
                        {
                            selectedData.is_deleted = true;
                        }
                    }
                }

                HttpContext.Current.Session["SESSION_NOTIFICATION_QUATATION"] = dataNotification;

                return dataNotification;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region FreeBies
        protected void gridViewFreeBie_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                gridViewFreeBie.DataSource = (from t in freeBieList where t.is_deleted == false select t).ToList();
                gridViewFreeBie.FilterExpression = FilterBag.GetExpression(false);
                gridViewFreeBie.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static List<FreeBies> AddFreeBie(string id, string description, string date)
        {
            try
            {
                List<FreeBies> dataFreebies = (List<FreeBies>)HttpContext.Current.Session["SESSION_FREEBIE_QUATATION"];
                List<Notification> dataNotification = (List<Notification>)HttpContext.Current.Session["SESSION_NOTIFICATION_QUATATION"];
                var genId = 0;
                if (dataFreebies == null) // Insert mode only
                {
                    dataFreebies = new List<FreeBies>();
                    genId = (dataFreebies.Count + 1) * -1;
                    dataFreebies.Add(new FreeBies
                    {
                        id = genId,
                        description = description,
                        is_deleted = false,
                        sort_no = (dataFreebies.Count + 1),
                        notice_date = string.IsNullOrEmpty(date) ? DateTime.MinValue : DateTime.Now,
                        display_notice_date = date

                    });
                }
                else
                {
                    if (Convert.ToInt32(id) == 0) // Insert mode when id == 0
                    {
                        genId = (dataFreebies.Count + 1) * -1;
                        dataFreebies.Add(new FreeBies
                        {
                            id = genId,
                            description = description,
                            is_deleted = false,
                            sort_no = (dataFreebies.Count + 1),
                            notice_date = string.IsNullOrEmpty(date) ? DateTime.MinValue : DateTime.Now,
                            display_notice_date = date
                        });
                    }
                    else // Edit mode
                    {
                        var row = (from t in dataFreebies where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                        row.description = description;
                        row.notice_date = string.IsNullOrEmpty(date) ? DateTime.MinValue : DateTime.Now;
                        row.is_deleted = false;
                        row.display_notice_date = date;
                    }

                }
                // Notification For Freebies
                if (!string.IsNullOrEmpty(date))
                {
                    IFormatProvider culture = new System.Globalization.CultureInfo("th-TH", true);
                    //DateTime dateTime = DateTime.Parse(date, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                    if (Convert.ToInt32(id) == 0) // Insert Mode
                    {
                        if (dataNotification == null) // Check Notice is null . Insert mode
                        {
                            dataNotification = new List<Notification>();

                            dataNotification.Add(new Notification
                            {
                                id = (dataNotification.Count + 1) * -1,
                                subject = "แจ้งเตือนของแถม",
                                description = description,
                                reference_id = genId,
                                notice_type = "RE",
                                topic = "FB",
                                notice_date = DateTime.Now,//dateTime,
                                display_notice_date = date,
                                is_deleted = false
                            });
                        }
                        else // Check has notice
                        {
                            var row = (from t in dataNotification where t.reference_id == Convert.ToInt32(id) select t).FirstOrDefault();
                            if (row != null) // edit notice child
                            {
                                row.subject = "แจ้งเตือนของแถม";
                                row.description = description;
                                row.notice_date = DateTime.Now; //dateTime,
                                row.is_deleted = false;
                                row.display_notice_date = date;
                            }
                            else // insert new notice
                            {
                                dataNotification.Add(new Notification
                                {
                                    id = (dataNotification.Count + 1) * -1,
                                    subject = "แจ้งเตือนของแถม",
                                    description = description,
                                    reference_id = genId,
                                    notice_type = "RE",
                                    topic = "FB",
                                    notice_date = DateTime.Now, //dateTime,
                                    is_deleted = false,
                                    display_notice_date = date
                                });
                            }

                        }

                    }
                    else // edit mode
                    {
                        var row = (from t in dataNotification where t.reference_id == Convert.ToInt32(id) select t).FirstOrDefault();
                        if (row != null) // edit
                        {
                            row.subject = "แจ้งเตือนของแถม";
                            row.description = description;
                            row.notice_date = DateTime.Now; //dateTime,
                            row.is_deleted = false;
                            row.display_notice_date = date;
                        }
                        else // insert
                        {
                            dataNotification.Add(new Notification
                            {
                                id = (dataNotification.Count + 1) * -1,
                                subject = "แจ้งเตือนของแถม",
                                description = description,
                                reference_id = genId,
                                notice_type = "RE",
                                topic = "FB",
                                notice_date = DateTime.Now, //dateTime,
                                is_deleted = false,
                                display_notice_date = date
                            });
                        }

                    }
                }

                HttpContext.Current.Session["SESSION_FREEBIE_QUATATION"] = dataFreebies;
                HttpContext.Current.Session["SESSION_NOTIFICATION_QUATATION"] = dataNotification;

                return (from t in dataFreebies where t.is_deleted == false select t).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [WebMethod]
        public static FreeBies ShowEditFreebies(string id)
        {
            try
            {
                List<FreeBies> dataFreebies = (List<FreeBies>)HttpContext.Current.Session["SESSION_FREEBIE_QUATATION"];
                FreeBies dataEditFreebies = new FreeBies();

                if (dataFreebies != null)
                {
                    var selectedData = (from t in dataFreebies where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (selectedData != null)
                    {
                        dataEditFreebies = selectedData;
                    }
                }

                HttpContext.Current.Session["SESSION_SELECTED_FREEBIE_QUATATION"] = dataEditFreebies;

                return dataEditFreebies;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static List<FreeBies> DeleteFreebies(string id)
        {
            try
            {
                List<FreeBies> dataFreebies = (List<FreeBies>)HttpContext.Current.Session["SESSION_FREEBIE_QUATATION"];

                if (dataFreebies != null)
                {
                    var selectedData = (from t in dataFreebies where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (selectedData != null)
                    {
                        if (Convert.ToInt32(id) < 0)
                        {
                            dataFreebies.Remove(selectedData);
                        }
                        else
                        {
                            selectedData.is_deleted = true;
                        }
                    }
                }

                HttpContext.Current.Session["SESSION_FREEBIE_QUATATION"] = dataFreebies;

                return dataFreebies;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Events
        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            SaveData("DR", "");
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            var quStatus = quotationStatus.Value;
            //var Status = StatusQuotation.Value;
            //var textStatus = "";
            //if (dataId < 0)
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "ConfirmMessage", "confirmMessage(" + txtQuatationNo.Value + ",'E')", true);
            //}
            if (quStatus == "DR" || quStatus == "RE")
            {
                SaveData("W", "");
            }
            else if (quStatus == "W")
            {
                SaveData("FL", "DR");
                //if (txtStatusQuotation.Value != null && txtStatusQuotation.Value != "")
                //{

                //    SaveData(Convert.ToString(Status), txtStatusQuotation.Value);
                //}
                //else
                //{
                //    SaveData(Convert.ToString(Status), textStatus);
                //}

            }
            //else if (quStatus == "FL")
            //{

            //}
            else
            {
                // DO NOTHING
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('ไม่สามารถทำรายการได้','E')", true);
            }


        }

        private void SaveData(string status, string textStatus)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                conn.Open();
                int newID = DataListUtil.emptyEntryID;
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {

                        var quotationNo = string.Empty;
                        var total = 0.0m;
                        var totalWithDiscount = 0.0m;
                        var grandTotal = 0.0m;
                        var discountBottom1 = (string.IsNullOrEmpty(txtDiscountBottomBill1.Value) ? 0 : Convert.ToDecimal(txtDiscountBottomBill1.Value));
                        var discountBottom2 = (string.IsNullOrEmpty(txtDiscountBottomBill2.Value) ? 0 : Convert.ToDecimal(txtDiscountBottomBill2.Value));
                        var sumDiscount1 = 0.0m;
                        var sumDiscount2 = 0.0m;
                        var totalVat = 0.0m;
                        foreach (var row in quotationDetailList)
                        {
                            if (!row.is_deleted)
                            {
                                total += row.total_amount;//row.unit_price * row.qty;
                                totalWithDiscount += row.total_amount;
                            }
                        }

                        //------- cal ส่วนลดท้ายบิล 1
                        if (cbDiscountBottomBill1.Checked && cbbDiscountBottomBill1.Value == "P")
                        {
                            sumDiscount1 = Math.Round((totalWithDiscount * discountBottom1) / 100, 2, MidpointRounding.AwayFromZero);
                            grandTotal = Math.Round(totalWithDiscount - ((totalWithDiscount * discountBottom1) / 100), 2, MidpointRounding.AwayFromZero);
                        }
                        else if (cbDiscountBottomBill1.Checked && cbbDiscountBottomBill1.Value == "A")
                        {
                            sumDiscount1 = discountBottom1;
                            grandTotal = Math.Round(totalWithDiscount - discountBottom1, 2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            grandTotal = totalWithDiscount;
                        }

                        //-------- cal ส่วนลดท้ายบิล 2 
                        if (cbDiscountBottomBill2.Checked && cbbDiscountBottomBill2.Value == "P")
                        {
                            sumDiscount2 = Math.Round(((totalWithDiscount - sumDiscount1) * discountBottom2) / 100, 2, MidpointRounding.AwayFromZero);
                            //grandTotal = grandTotal - ((totalWithDiscount - sumDiscount2) * discountBottom2) / 100;
                            grandTotal = Math.Round((totalWithDiscount - sumDiscount1 - sumDiscount2), 2, MidpointRounding.AwayFromZero);
                        }
                        else if (cbDiscountBottomBill2.Checked && cbbDiscountBottomBill2.Value == "A")
                        {
                            sumDiscount2 = discountBottom2;
                            grandTotal = Math.Round(grandTotal - discountBottom2, 2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            //grandTotal = totalWithDiscount;
                        }

                        //-------- cal vat 
                        if (cbShowVat.Checked)
                        {
                            var to = (grandTotal * ConstantClass.VAT) / 100;
                            totalVat = Math.Round(to, 2, MidpointRounding.AwayFromZero);
                            grandTotal = Math.Round(grandTotal + totalVat, 2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            totalVat = 0;
                            grandTotal = Math.Round(grandTotal + totalVat, 2, MidpointRounding.AwayFromZero);
                        }

                        if (dataId == 0) // Insert Mode
                        {

                            #region Quotation Header
                            //Quotation Header
                            using (SqlCommand cmd = new SqlCommand("sp_quotation_header_add", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = revisionId > 0 ? txtCloneQuotation.Value : string.Empty;
                                cmd.Parameters.Add("@refer_no_for_repeat", SqlDbType.VarChar, 20).Value = txtCloneQuotation.Value;
                                cmd.Parameters.Add("@quotation_date", SqlDbType.Date).Value = DateTime.UtcNow;
                                cmd.Parameters.Add("@quotation_status", SqlDbType.VarChar, 2).Value = status;//FL = Follow , CF = Confirm
                                cmd.Parameters.Add("@remark_status", SqlDbType.VarChar, 100).Value = textStatus;//FL = Follow , CF = Confirm
                                cmd.Parameters.Add("@quotation_type", SqlDbType.VarChar, 20).Value = cbbQuotationType.Value;
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbCustomerID.Value;
                                cmd.Parameters.Add("@customer_code", SqlDbType.VarChar, 20).Value = string.Empty; // หาจาก base
                                cmd.Parameters.Add("@customer_name", SqlDbType.VarChar, 200).Value = cbbCustomerID.Text;
                                cmd.Parameters.Add("@sales_id", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                cmd.Parameters.Add("@attention_name", SqlDbType.VarChar, 200).Value = cbbCustomerAttention.Text;
                                cmd.Parameters.Add("@attention_tel", SqlDbType.VarChar, 100).Value = txtAttentionTel.Value;
                                cmd.Parameters.Add("@attention_email", SqlDbType.VarChar, 100).Value = txtAttentionEmail.Value;
                                cmd.Parameters.Add("@customer_attention_id", SqlDbType.Int).Value = cbbCustomerAttention.Value;
                                cmd.Parameters.Add("@quotation_subject", SqlDbType.VarChar, 200).Value = cbbSubject.Text;
                                cmd.Parameters.Add("@project_name", SqlDbType.VarChar, 200).Value = txtProject.Value;
                                cmd.Parameters.Add("@is_discount_by_item", SqlDbType.Bit).Value = cbDiscountByItem.Checked;
                                cmd.Parameters.Add("@discount1_type", SqlDbType.VarChar, 1).Value = cbDiscountBottomBill1.Checked ? cbbDiscountBottomBill1.Value : string.Empty;
                                cmd.Parameters.Add("@discount1_percentage", SqlDbType.Decimal).Value = string.IsNullOrEmpty(txtDiscountBottomBill1.Value) ? 0
                                                                                                       : (cbbDiscountBottomBill1.Value == "P" ? Convert.ToDecimal(txtDiscountBottomBill1.Value) : 0);
                                cmd.Parameters.Add("@discount1_amount", SqlDbType.Decimal).Value = string.IsNullOrEmpty(txtDiscountBottomBill1.Value) ? 0
                                                                                                       : (cbbDiscountBottomBill1.Value == "A" ? Convert.ToDecimal(txtDiscountBottomBill1.Value) : 0);
                                cmd.Parameters.Add("@discount1_total", SqlDbType.Decimal).Value = sumDiscount1;
                                cmd.Parameters.Add("@discount2_type", SqlDbType.VarChar, 1).Value = cbDiscountBottomBill2.Checked ? cbbDiscountBottomBill2.Value : string.Empty;
                                cmd.Parameters.Add("@discount2_percentage", SqlDbType.Decimal).Value = string.IsNullOrEmpty(txtDiscountBottomBill2.Value) ? 0
                                                                                                       : (cbbDiscountBottomBill2.Value == "P" ? Convert.ToDecimal(txtDiscountBottomBill2.Value) : 0);
                                cmd.Parameters.Add("@discount2_amount", SqlDbType.Decimal).Value = string.IsNullOrEmpty(txtDiscountBottomBill1.Value) ? 0
                                                                                                       : (cbbDiscountBottomBill2.Value == "A" ? Convert.ToDecimal(txtDiscountBottomBill2.Value) : 0);
                                cmd.Parameters.Add("@discount2_total", SqlDbType.Decimal).Value = sumDiscount2;
                                cmd.Parameters.Add("@total_amount", SqlDbType.Decimal).Value = total;
                                cmd.Parameters.Add("@grand_total", SqlDbType.Decimal).Value = grandTotal;
                                cmd.Parameters.Add("@is_vat", SqlDbType.Bit).Value = cbShowVat.Checked;
                                cmd.Parameters.Add("@is_net", SqlDbType.Bit).Value = cbIsNet.Checked;
                                cmd.Parameters.Add("@vat_total", SqlDbType.Decimal).Value = totalVat;
                                cmd.Parameters.Add("@remark_id", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@sale_order_no", SqlDbType.VarChar, 20).Value = DBNull.Value;
                                cmd.Parameters.Add("@issue_stock_no", SqlDbType.VarChar, 100).Value = DBNull.Value;
                                cmd.Parameters.Add("@delivery_note_no", SqlDbType.VarChar, 100).Value = DBNull.Value;
                                cmd.Parameters.Add("@invoice_no", SqlDbType.VarChar, 100).Value = DBNull.Value;
                                cmd.Parameters.Add("@quotation_completed_date", SqlDbType.DateTime).Value = DBNull.Value;
                                cmd.Parameters.Add("@is_lump_sum", SqlDbType.Bit).Value = rdoLumpsum.Checked ? true : false;
                                cmd.Parameters.Add("@is_product_description", SqlDbType.Bit).Value = cbbQuotationType.Value == "P" ? (rdoDescription.Checked ? false : true) : false;

                                cmd.Parameters.Add("@contract_no", SqlDbType.VarChar, 50).Value = txtContractNo.Value;
                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                cmd.Parameters.Add("@hour_amount", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(txtHour.Value) ? string.Empty : txtHour.Value;
                                cmd.Parameters.Add("@pressure", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(txtPressure.Value) ? "" : txtPressure.Value; //Convert.ToDecimal(txtPressure.Value)*/;
                                cmd.Parameters.Add("@machine", SqlDbType.VarChar, 50).Value = txtMachine.Value;

                                cmd.Parameters.Add("@mfg", SqlDbType.VarChar, 50).Value = txtShowMFG.Value;
                                cmd.Parameters.Add("@model_id", SqlDbType.Int).Value = cbbModel.Value;
                                cmd.Parameters.Add("@model", SqlDbType.VarChar, 20).Value = cbbModel.Text == "" ? "" : cbbModel.Text == "0" ? "" : cbbModel.Text.Split(';')[0];
                                cmd.Parameters.Add("@revision", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@clone_type", SqlDbType.VarChar, 1).Value = cloneId > 0 ? "C" : (revisionId > 0 ? "R" : "");
                                cmd.Parameters.Add("@vat", SqlDbType.Decimal).Value = ConstantClass.VAT;
                                cmd.Parameters.Add("@location1", SqlDbType.VarChar, 200).Value = txtLocation1.Value;
                                cmd.Parameters.Add("@location2", SqlDbType.VarChar, 200).Value = txtLocation2.Value;
                                cmd.Parameters.Add("@is_contactor", SqlDbType.Bit).Value = cbContactor.Checked;
                                cmd.Parameters.Add("@currency", SqlDbType.Int).Value = 1;// Cbbcurrency.Value;
                                
                                newID = Convert.ToInt32(cmd.ExecuteScalar());

                            }
                            #endregion Quatation Header

                            //Create array of Parameters

                            List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID },
                            new SqlParameter("@id", SqlDbType.Int) { Value = newID },
                            new SqlParameter("@search_name", SqlDbType.Int) { Value = "" },
                            new SqlParameter("@service_type", SqlDbType.VarChar, 100) { Value = "" },
                        };

                            var lastDataInsert = SqlHelper.ExecuteDataset(tran, "sp_quotation_header_list", arrParm.ToArray());


                            if (lastDataInsert != null)
                            {
                                quotationNo = (from t in lastDataInsert.Tables[0].AsEnumerable()
                                               select t.Field<string>("quotation_no")).FirstOrDefault();
                            }

                            #region Quotation Detail
                            // Quotation Detail
                            var newDetailId = 0;
                            var oldDetailId = 0;
                            if (quotationDetailList != null)
                            {
                                foreach (var row in quotationDetailList)
                                {
                                    oldDetailId = row.id;

                                    if (cbbDiscountByItem.Value == "P")
                                    {
                                        row.discount_total = ((row.unit_price * row.qty) * row.discount_percentage) / 100;
                                    }
                                    else if (cbbDiscountByItem.Value == "A")
                                    {
                                        row.discount_total = row.discount_amount;
                                    }
                                    using (SqlCommand cmd = new SqlCommand("sp_quotation_detail_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = quotationNo;
                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                        cmd.Parameters.Add("@product_no", SqlDbType.VarChar, 50).Value = row.product_no;
                                        cmd.Parameters.Add("@quotation_description", SqlDbType.VarChar, 200).Value = row.quotation_description;
                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar).Value = string.IsNullOrEmpty(row.mfg_no) ? string.Empty : row.mfg_no;
                                        cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 200).Value = row.unit_code;
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                        cmd.Parameters.Add("@unit_price", SqlDbType.Decimal).Value = row.unit_price;
                                        cmd.Parameters.Add("@discount_type", SqlDbType.VarChar, 1).Value = cbDiscountByItem.Checked ? cbbDiscountByItem.Value : string.Empty;
                                        cmd.Parameters.Add("@discount_percentage", SqlDbType.Decimal).Value = Convert.ToString(row.total_amount) == "0.00" ? 0 : row.discount_percentage;
                                        cmd.Parameters.Add("@discount_amount", SqlDbType.Decimal).Value = Convert.ToString(row.total_amount) == "0.00" ? 0 : Convert.ToDecimal(row.discount_amount);
                                        cmd.Parameters.Add("@discount_total", SqlDbType.Decimal).Value = Convert.ToDecimal(row.discount_total);
                                        cmd.Parameters.Add("@total_amount", SqlDbType.Decimal).Value = row.total_amount;
                                        cmd.Parameters.Add("@issue_stock_no", SqlDbType.VarChar, 20).Value = DBNull.Value;
                                        cmd.Parameters.Add("@is_history_issue_mfg", SqlDbType.Bit).Value = row.is_history_issue_mfg;
                                        cmd.Parameters.Add("@delivery_note_no", SqlDbType.VarChar, 20).Value = DBNull.Value;
                                        cmd.Parameters.Add("@parent_id", SqlDbType.Int).Value = row.parant_id;
                                        cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                        cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 2).Value = row.product_type;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        cmd.Parameters.Add("@quotation_line_1", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_1) ? string.Empty : row.quotation_line_1;
                                        cmd.Parameters.Add("@quotation_line_2", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_2) ? string.Empty : row.quotation_line_2;
                                        cmd.Parameters.Add("@quotation_line_3", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_3) ? string.Empty : row.quotation_line_3;
                                        cmd.Parameters.Add("@quotation_line_4", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_4) ? string.Empty : row.quotation_line_4;
                                        cmd.Parameters.Add("@quotation_line_5", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_5) ? string.Empty : row.quotation_line_5;
                                        cmd.Parameters.Add("@quotation_line_6", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_6) ? string.Empty : row.quotation_line_6;
                                        cmd.Parameters.Add("@quotation_line_7", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_7) ? string.Empty : row.quotation_line_7;
                                        cmd.Parameters.Add("@quotation_line_8", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_8) ? string.Empty : row.quotation_line_8;
                                        cmd.Parameters.Add("@quotation_line_9", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_9) ? string.Empty : row.quotation_line_9;
                                        cmd.Parameters.Add("@quotation_line_10", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_10) ? string.Empty : row.quotation_line_10;
                                        cmd.Parameters.Add("@model_id", SqlDbType.Int).Value = row.model_id;
                                        cmd.Parameters.Add("@package_id", SqlDbType.Int).Value = row.package_id;
                                        newDetailId = Convert.ToInt32(cmd.ExecuteScalar());

                                    }
                                    var childData = (from t in quotationDetailList where t.parant_id == oldDetailId select t).ToList();
                                    foreach (var rowDetail in childData)
                                    {
                                        rowDetail.parant_id = newDetailId;
                                    }
                                }
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('XXXXs','E')", true);
                            }
                            #endregion Quotation Detail

                            #region Quotation Remark
                            // Remark
                            List<string> listRemark = new List<string>();
                            listRemark.Add(txtRemark1.Value);
                            listRemark.Add(txtRemark2.Value);
                            listRemark.Add(txtRemark3.Value);
                            listRemark.Add(txtRemark4.Value);
                            listRemark.Add(txtRemark5.Value);
                            listRemark.Add(txtRemark6.Value);
                            listRemark.Add(txtRemark7.Value);
                            listRemark.Add(txtRemark8.Value);
                            listRemark.Add(txtRemark9.Value);
                            listRemark.Add(txtRemark10.Value);
                            listRemark.Add(txtRemark11.Value);
                            listRemark.Add(txtRemarkOther.Value);

                            for (var i = 0; i < listRemark.Count; i++)
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_quotation_remark_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = quotationNo;
                                    cmd.Parameters.Add("@quotation_description", SqlDbType.VarChar, 200).Value = listRemark[i];
                                    cmd.Parameters.Add("@sortno", SqlDbType.VarChar, 50).Value = i + 1;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                    cmd.ExecuteNonQuery();

                                }
                                //i++;
                            }
                            #endregion Quotation Remark

                            #region Quotation Notification
                            // Notification
                            foreach (var row in (from t in notificationList
                                                 where t.topic == "QU" && t.is_deleted == false
                                                 select t).ToList())
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_notification_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@subject", SqlDbType.VarChar, 100).Value = row.subject;
                                    cmd.Parameters.Add("@description", SqlDbType.VarChar, 200).Value = row.description;
                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbCustomerID.Value;
                                    cmd.Parameters.Add("@topic", SqlDbType.Char, 3).Value = row.topic;
                                    cmd.Parameters.Add("@reference_id", SqlDbType.Int).Value = newID;
                                    cmd.Parameters.Add("@reference_no", SqlDbType.VarChar, 100).Value = quotationNo;
                                    cmd.Parameters.Add("@notice_type", SqlDbType.VarChar, 3).Value = row.notice_type;
                                    cmd.Parameters.Add("@notice_date", SqlDbType.DateTime).Value = row.notice_date;
                                    cmd.Parameters.Add("@is_read", SqlDbType.Bit).Value = 0;
                                    cmd.Parameters.Add("@is_enable", SqlDbType.Bit).Value = 1;
                                    cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = 0;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                    cmd.ExecuteNonQuery();

                                }
                                #endregion Quotation Notification

                            }
                        }
                        else // Edit Mode
                        {
                            newID = dataId;
                            if (status == "DR" && quotationStatus.Value == "FL")
                            {
                                status = "FL";
                            } 

                            #region Quotation Header
                            //Quotation Header
                            using (SqlCommand cmd = new SqlCommand("sp_quotation_header_edit", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("id", SqlDbType.Int).Value = dataId;
                                cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = txtQuatationNo.Value;
                                cmd.Parameters.Add("@refer_no_for_repeat", SqlDbType.VarChar, 20).Value = DBNull.Value;//txtQuatationNo.Value;
                                cmd.Parameters.Add("@quotation_date", SqlDbType.Date).Value = DateTime.UtcNow;
                                cmd.Parameters.Add("@quotation_status", SqlDbType.VarChar, 2).Value = status;//FL = Follow , CF = Confirm
                                cmd.Parameters.Add("@remark_status", SqlDbType.VarChar, 100).Value = textStatus;                                                                            //cmd.Parameters.Add("@quotation_type", SqlDbType.VarChar, 20).Value = cbbQuotationType.Value;
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbCustomerID.Value;
                                cmd.Parameters.Add("@customer_code", SqlDbType.VarChar, 20).Value = string.Empty; // หาจาก base
                                cmd.Parameters.Add("@customer_name", SqlDbType.VarChar, 200).Value = cbbCustomerID.Text;
                                cmd.Parameters.Add("@sales_id", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                 cmd.Parameters.Add("@attention_name", SqlDbType.VarChar, 200).Value = cbbCustomerAttention.Text;
                                cmd.Parameters.Add("@attention_email", SqlDbType.VarChar, 50).Value = txtAttentionEmail.Value;
                                cmd.Parameters.Add("@customer_attention_id", SqlDbType.Int).Value = cbbCustomerAttention.Value;
                                cmd.Parameters.Add("@quotation_subject", SqlDbType.VarChar, 200).Value = cbbSubject.Text;
                                cmd.Parameters.Add("@project_name", SqlDbType.VarChar, 200).Value = txtProject.Value;

                                cmd.Parameters.Add("@is_discount_by_item", SqlDbType.Bit).Value = cbDiscountByItem.Checked;
                                cmd.Parameters.Add("@discount1_type", SqlDbType.VarChar, 1).Value = cbDiscountBottomBill1.Checked ? cbbDiscountBottomBill1.Value : string.Empty;
                                cmd.Parameters.Add("@discount1_percentage", SqlDbType.Decimal).Value = string.IsNullOrEmpty(txtDiscountBottomBill1.Value) ? 0
                                                                                                       : (cbbDiscountBottomBill1.Value == "P" ? Convert.ToDecimal(txtDiscountBottomBill1.Value) : 0);
                                cmd.Parameters.Add("@discount1_amount", SqlDbType.Decimal).Value = string.IsNullOrEmpty(txtDiscountBottomBill1.Value) ? 0
                                                                                                       : (cbbDiscountBottomBill1.Value == "A" ? Convert.ToDecimal(txtDiscountBottomBill1.Value) : 0);
                                cmd.Parameters.Add("@discount2_type", SqlDbType.VarChar, 1).Value = cbDiscountBottomBill2.Checked ? cbbDiscountBottomBill2.Value : string.Empty;
                                cmd.Parameters.Add("@discount2_percentage", SqlDbType.Decimal).Value = string.IsNullOrEmpty(txtDiscountBottomBill2.Value) ? 0
                                                                                                       : (cbbDiscountBottomBill2.Value == "P" ? Convert.ToDecimal(txtDiscountBottomBill2.Value) : 0);
                                cmd.Parameters.Add("@discount2_amount", SqlDbType.Decimal).Value = string.IsNullOrEmpty(txtDiscountBottomBill1.Value) ? 0
                                                                                                       : (cbbDiscountBottomBill2.Value == "A" ? Convert.ToDecimal(txtDiscountBottomBill2.Value) : 0);
                                cmd.Parameters.Add("@discount1_total", SqlDbType.Decimal).Value = sumDiscount1;
                                cmd.Parameters.Add("@discount2_total", SqlDbType.Decimal).Value = sumDiscount2;
                                cmd.Parameters.Add("@total_amount", SqlDbType.Decimal).Value = total;
                                cmd.Parameters.Add("@grand_total", SqlDbType.Decimal).Value = grandTotal;
                                cmd.Parameters.Add("@is_vat", SqlDbType.Bit).Value = cbShowVat.Checked;
                                cmd.Parameters.Add("@is_net", SqlDbType.Bit).Value = cbIsNet.Checked;
                                cmd.Parameters.Add("@vat_total", SqlDbType.Decimal).Value = totalVat;
                                cmd.Parameters.Add("@attention_tel", SqlDbType.VarChar).Value = txtAttentionTel.Value;
                                cmd.Parameters.Add("@remark_id", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@sale_order_no", SqlDbType.VarChar, 20).Value = DBNull.Value;
                                cmd.Parameters.Add("@issue_stock_no", SqlDbType.VarChar, 100).Value = DBNull.Value;
                                cmd.Parameters.Add("@delivery_note_no", SqlDbType.VarChar, 100).Value = DBNull.Value;
                                cmd.Parameters.Add("@invoice_no", SqlDbType.VarChar, 100).Value = DBNull.Value;
                                cmd.Parameters.Add("@quotation_completed_date", SqlDbType.DateTime).Value = DBNull.Value;
                                cmd.Parameters.Add("@is_lump_sum", SqlDbType.Bit).Value = rdoLumpsum.Checked ? true : false;
                                cmd.Parameters.Add("@is_product_description", SqlDbType.Bit).Value = cbbQuotationType.Value == "P" ? (rdoDescription.Checked ? false : true) : false;
                                cmd.Parameters.Add("@contract_no", SqlDbType.VarChar, 50).Value = txtContractNo.Value;
                                cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                cmd.Parameters.Add("@hour_amount", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(txtHour.Value) ? string.Empty : txtHour.Value;
                                cmd.Parameters.Add("@pressure", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(txtPressure.Value) ? string.Empty : txtPressure.Value;
                                cmd.Parameters.Add("@machine", SqlDbType.VarChar, 50).Value = txtMachine.Value;

                                cmd.Parameters.Add("@mfg", SqlDbType.VarChar, 50).Value = txtShowMFG.Value;
                                cmd.Parameters.Add("@model_id", SqlDbType.Int).Value = cbbModel.Value;
                                if ((status.Equals("FL") && textStatus.Equals("DR")) 
                                    || status.Equals("PO") && textStatus.Equals("")
                                    || status.Equals("LO") && textStatus.Equals("")
                                    || status.Equals("CC") && textStatus.Equals(""))
                                {
                                    cmd.Parameters.Add("@model", SqlDbType.VarChar, 20).Value = hdModelName.Value;
                                } else
                                {
                                    cmd.Parameters.Add("@model", SqlDbType.VarChar, 20).Value = cbbModel.Text == "" ? "" : cbbModel.Text == "0" ? "" : cbbModel.Text.Split(';')[0];
                                }
                                cmd.Parameters.Add("@location1", SqlDbType.VarChar, 200).Value = txtLocation1.Value;
                                cmd.Parameters.Add("@location2", SqlDbType.VarChar, 200).Value = txtLocation2.Value;
                                cmd.Parameters.Add("@currency", SqlDbType.Int).Value = Cbbcurrency.Value;
                                cmd.Parameters.Add("@is_contactor", SqlDbType.Bit).Value = cbContactor.Checked;

                                cmd.ExecuteNonQuery();

                            }
                            #endregion Quatation Header

                            #region Quotation Detail

                            // Quotation Detail
                            var newDetailId = 0;
                            var oldDetailId = 0;
                            foreach (var row in quotationDetailList)
                            {
                                oldDetailId = row.id;
                                if (cbbDiscountByItem.Value == "P")
                                {
                                    row.discount_total = ((row.unit_price * row.qty) * row.discount_percentage) / 100;
                                }
                                else if (cbbDiscountByItem.Value == "A")
                                {
                                    row.discount_total = row.discount_amount;
                                }
                                if (row.id < 0)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_quotation_detail_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = txtQuatationNo.Value;
                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                        cmd.Parameters.Add("@product_no", SqlDbType.VarChar, 50).Value = row.product_no;
                                        cmd.Parameters.Add("@quotation_description", SqlDbType.VarChar, 200).Value = row.quotation_description;
                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar).Value = string.IsNullOrEmpty(row.mfg_no) ? string.Empty : row.mfg_no;
                                        cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 200).Value = row.unit_code;
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                        cmd.Parameters.Add("@unit_price", SqlDbType.Decimal).Value = row.unit_price;
                                        cmd.Parameters.Add("@discount_type", SqlDbType.VarChar, 1).Value = cbDiscountByItem.Checked ? cbbDiscountByItem.Value : string.Empty;
                                        cmd.Parameters.Add("@discount_percentage", SqlDbType.Decimal).Value = Convert.ToString(row.total_amount) == "0.00" ? 0 : row.discount_percentage;
                                        cmd.Parameters.Add("@discount_amount", SqlDbType.Decimal).Value = Convert.ToString(row.total_amount) == "0.00" ? 0 : Convert.ToDecimal(row.discount_amount);
                                        cmd.Parameters.Add("@discount_total", SqlDbType.Decimal).Value = Convert.ToDecimal(row.discount_total);
                                        cmd.Parameters.Add("@total_amount", SqlDbType.Decimal).Value = row.total_amount;
                                        cmd.Parameters.Add("@issue_stock_no", SqlDbType.VarChar, 20).Value = DBNull.Value;
                                        cmd.Parameters.Add("@is_history_issue_mfg", SqlDbType.Bit).Value = row.is_history_issue_mfg;
                                        cmd.Parameters.Add("@delivery_note_no", SqlDbType.VarChar, 20).Value = DBNull.Value;
                                        cmd.Parameters.Add("@parent_id", SqlDbType.Int).Value = row.parant_id;
                                        cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                        cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 2).Value = row.product_type;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        cmd.Parameters.Add("@quotation_line_1", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_1) ? string.Empty : row.quotation_line_1;
                                        cmd.Parameters.Add("@quotation_line_2", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_2) ? string.Empty : row.quotation_line_2;
                                        cmd.Parameters.Add("@quotation_line_3", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_3) ? string.Empty : row.quotation_line_3;
                                        cmd.Parameters.Add("@quotation_line_4", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_4) ? string.Empty : row.quotation_line_4;
                                        cmd.Parameters.Add("@quotation_line_5", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_5) ? string.Empty : row.quotation_line_5;
                                        cmd.Parameters.Add("@quotation_line_6", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_6) ? string.Empty : row.quotation_line_6;
                                        cmd.Parameters.Add("@quotation_line_7", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_7) ? string.Empty : row.quotation_line_7;
                                        cmd.Parameters.Add("@quotation_line_8", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_8) ? string.Empty : row.quotation_line_8;
                                        cmd.Parameters.Add("@quotation_line_9", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_9) ? string.Empty : row.quotation_line_9;
                                        cmd.Parameters.Add("@quotation_line_10", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(row.quotation_line_10) ? string.Empty : row.quotation_line_10;
                                        cmd.Parameters.Add("@model_id", SqlDbType.Int).Value = row.model_id;
                                        cmd.Parameters.Add("@package_id", SqlDbType.Int).Value = row.package_id;
                                        newDetailId = Convert.ToInt32(cmd.ExecuteScalar());

                                    }
                                    var childData = (from t in quotationDetailList where t.parant_id == oldDetailId select t).ToList();
                                    foreach (var rowDetail in childData)
                                    {
                                        rowDetail.parant_id = newDetailId;
                                    }
                                }
                                else
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_quotation_detail_edit", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                        cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = txtQuatationNo.Value;
                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                        cmd.Parameters.Add("@product_no", SqlDbType.VarChar, 50).Value = row.product_no;
                                        cmd.Parameters.Add("@quotation_description", SqlDbType.VarChar, 200).Value = row.quotation_description;
                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar).Value = string.IsNullOrEmpty(row.mfg_no) ? string.Empty : row.mfg_no;
                                        cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 10).Value = row.unit_code;
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                        cmd.Parameters.Add("@unit_price", SqlDbType.Decimal).Value = row.unit_price;
                                        cmd.Parameters.Add("@discount_type", SqlDbType.VarChar, 1).Value = cbDiscountByItem.Checked ? cbbDiscountByItem.Value : string.Empty;
                                        cmd.Parameters.Add("@discount_percentage", SqlDbType.Decimal).Value = Convert.ToString(row.total_amount) == "0.00" ? 0 : row.discount_percentage;
                                        cmd.Parameters.Add("@discount_amount", SqlDbType.Decimal).Value = Convert.ToString(row.total_amount) == "0.00" ? 0 : Convert.ToDecimal(row.discount_amount);
                                        cmd.Parameters.Add("@discount_total", SqlDbType.Decimal).Value = Convert.ToDecimal(row.discount_total);
                                        cmd.Parameters.Add("@total_amount", SqlDbType.Decimal).Value = row.total_amount;
                                        cmd.Parameters.Add("@issue_stock_no", SqlDbType.VarChar, 20).Value = DBNull.Value;
                                        cmd.Parameters.Add("@delivery_note_no", SqlDbType.VarChar, 20).Value = DBNull.Value;
                                        cmd.Parameters.Add("@parent_id", SqlDbType.Int).Value = row.parant_id;
                                        cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = row.is_deleted;
                                        cmd.Parameters.Add("@quotation_line_1", SqlDbType.VarChar, 500).Value = row.quotation_line_1;
                                        cmd.Parameters.Add("@quotation_line_2", SqlDbType.VarChar, 500).Value = row.quotation_line_2;
                                        cmd.Parameters.Add("@quotation_line_3", SqlDbType.VarChar, 500).Value = row.quotation_line_3;
                                        cmd.Parameters.Add("@quotation_line_4", SqlDbType.VarChar, 500).Value = row.quotation_line_4;
                                        cmd.Parameters.Add("@quotation_line_5", SqlDbType.VarChar, 500).Value = row.quotation_line_5;
                                        cmd.Parameters.Add("@quotation_line_6", SqlDbType.VarChar, 500).Value = row.quotation_line_6;
                                        cmd.Parameters.Add("@quotation_line_7", SqlDbType.VarChar, 500).Value = row.quotation_line_7;
                                        cmd.Parameters.Add("@quotation_line_8", SqlDbType.VarChar, 500).Value = row.quotation_line_8;
                                        cmd.Parameters.Add("@quotation_line_9", SqlDbType.VarChar, 500).Value = row.quotation_line_9;
                                        cmd.Parameters.Add("@quotation_line_10", SqlDbType.VarChar, 500).Value = row.quotation_line_10;
                                        cmd.Parameters.Add("@model_id", SqlDbType.Int).Value = row.model_id;
                                        cmd.Parameters.Add("@package_id", SqlDbType.Int).Value = row.package_id;
                                        cmd.ExecuteNonQuery();

                                    }
                                    var childData = (from t in quotationDetailList where t.parant_id == oldDetailId select t).ToList();
                                    foreach (var rowDetail in childData)
                                    {
                                        rowDetail.parant_id = oldDetailId;
                                    }
                                }
                            }
                            #endregion Quotation Detail

                            #region Quotation Remark
                            // Remark 
                            List<string> listRemark = new List<string>();
                            listRemark.Add(txtRemark1.Value);
                            listRemark.Add(txtRemark2.Value);
                            listRemark.Add(txtRemark3.Value);
                            listRemark.Add(txtRemark4.Value);
                            listRemark.Add(txtRemark5.Value);
                            listRemark.Add(txtRemark6.Value);
                            listRemark.Add(txtRemark7.Value);
                            listRemark.Add(txtRemark8.Value);
                            listRemark.Add(txtRemark9.Value);
                            listRemark.Add(txtRemark10.Value);
                            listRemark.Add(txtRemark11.Value);
                            listRemark.Add(txtRemarkOther.Value);

                            // Case Edit : Delete old records and Insert new records.
                            using (SqlCommand cmd = new SqlCommand("sp_quotation_remark_delete_edit", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = txtQuatationNo.Value;

                                cmd.ExecuteNonQuery();

                            }

                            for (var i = 0; i < listRemark.Count; i++)
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_quotation_remark_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = txtQuatationNo.Value;
                                    cmd.Parameters.Add("@quotation_description", SqlDbType.VarChar, 200).Value = listRemark[i];
                                    cmd.Parameters.Add("@sortno", SqlDbType.VarChar, 50).Value = i + 1;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                    cmd.ExecuteNonQuery();

                                }
                                //i++;
                            }
                            #endregion Quotation Remark

                            #region Quotation FreeBies
                            #endregion Quotation FreeBies

                            #region Quotation Notification
                            using (SqlCommand cmd = new SqlCommand("sp_quotation_notification_delete_edit", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = txtQuatationNo.Value;

                                cmd.ExecuteNonQuery();

                            }
                            // Notification
                            foreach (var row in (from t in notificationList
                                                 where t.topic.Trim() == "QU" && t.is_deleted == false
                                                 select t).ToList())
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_notification_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@subject", SqlDbType.VarChar, 100).Value = row.subject;
                                    cmd.Parameters.Add("@description", SqlDbType.VarChar, 200).Value = row.description;
                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbCustomerID.Value;
                                    cmd.Parameters.Add("@topic", SqlDbType.NVarChar, 3).Value = row.topic;
                                    cmd.Parameters.Add("@reference_id", SqlDbType.Int).Value = dataId;
                                    cmd.Parameters.Add("@reference_no", SqlDbType.VarChar, 100).Value = txtQuatationNo.Value;
                                    cmd.Parameters.Add("@notice_type", SqlDbType.VarChar, 3).Value = row.notice_type;
                                    cmd.Parameters.Add("@notice_date", SqlDbType.DateTime).Value = row.notice_date;
                                    cmd.Parameters.Add("@is_read", SqlDbType.Bit).Value = row.is_read;
                                    cmd.Parameters.Add("@is_enable", SqlDbType.Bit).Value = 1;
                                    cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = 0;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                    cmd.ExecuteNonQuery();

                                }
                                #endregion Quotation Notification

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        tran.Dispose();
                        conn.Close();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('เกิดผิดพลาดในการบันทึกข้อมูล','E')", true);
                        //throw ex;
                    }
                    finally
                    {
                        if (!conn.State.Equals(ConnectionState.Closed))
                        {

                            tran.Commit();
                            tran.Dispose();
                            conn.Close();

                            Response.Redirect("Quotation.aspx?dataId=" + newID);
                        }
                    }
                }
            }
        }
        #endregion

        #region Other Event
        protected void gridViewOrderHistory_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var customerId = cbbCustomerID.Value == null ? "0" : cbbCustomerID.Value;

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = customerId },
                        };
                conn.Open();
                dsOrderHistory = SqlHelper.ExecuteDataset(conn, "sp_view_order_history_list", arrParm.ToArray());
                ViewState["dsOrderHistory"] = dsOrderHistory;
                conn.Close();
            }
            gridViewOrderHistory.DataSource = dsOrderHistory;
            gridViewOrderHistory.FilterExpression = FilterBag.GetExpression(false);
            gridViewOrderHistory.DataBind();
        }

        [WebMethod]
        public static CustomerData GetCustomerData(string id)
        {
            CustomerData customerData = new CustomerData();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = string.Empty },
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },
                        };
                conn.Open();
                var data = SqlHelper.ExecuteDataset(conn, "sp_customer_list", arrParm.ToArray());
                conn.Close();
                if (data.Tables.Count > 0)
                {
                    if (data.Tables[0].Rows.Count > 0)
                    {
                        customerData.customer_id = Convert.IsDBNull(data.Tables[0].Rows[0]["id"]) ? 0 : Convert.ToInt32(data.Tables[0].Rows[0]["id"]);
                        customerData.customer_address = Convert.IsDBNull(data.Tables[0].Rows[0]["address_bill_eng"]) ? string.Empty : Convert.ToString(data.Tables[0].Rows[0]["address_bill_eng"]);
                        customerData.customer_name_eng = Convert.IsDBNull(data.Tables[0].Rows[0]["company_name_eng"]) ? string.Empty : Convert.ToString(data.Tables[0].Rows[0]["company_name_eng"]);
                        customerData.customer_name_tha = Convert.IsDBNull(data.Tables[0].Rows[0]["company_name_tha"]) ? string.Empty : Convert.ToString(data.Tables[0].Rows[0]["company_name_tha"]);
                        customerData.contract_name = Convert.IsDBNull(data.Tables[0].Rows[0]["contact_name"]) ? string.Empty : Convert.ToString(data.Tables[0].Rows[0]["contact_name"]);
                        customerData.tel = Convert.IsDBNull(data.Tables[0].Rows[0]["tel"]) ? string.Empty : Convert.ToString(data.Tables[0].Rows[0]["tel"]);
                    }
                }
            }

            return customerData;
        }



        [WebMethod]
        public static List<ConfigDocument> GetConfig(string quotationType)
        {
            var data = new List<ConfigDocument>();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {

                            new SqlParameter("@config_document", SqlDbType.Char,2) { Value = "QU" },
                            new SqlParameter("@config_type", SqlDbType.Char,1) { Value = quotationType },
                             new SqlParameter("@id", SqlDbType.Int) { Value = 0},
                        };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_config_list", arrParm.ToArray());
                conn.Close();
                if (dsResult.Tables.Count > 0)
                {
                    foreach (var row in dsResult.Tables[0].AsEnumerable())
                    {
                        data.Add(new ConfigDocument()
                        {
                            document_description = Convert.ToString(row["config_description"]),
                            id = Convert.ToInt32(row["id"])
                        });
                    }
                }
            }

            return data;
        }

        [WebMethod]
        public static List<string> GetRemark(string quotationType)
        {
            var data = new List<string>();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                             new SqlParameter("@remark_type", SqlDbType.NVarChar,2) { Value = "QU" },
                            new SqlParameter("@remark_type_document", SqlDbType.NVarChar,2) { Value = quotationType },
                        };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_remark_list", arrParm.ToArray());
                conn.Close();
                if (dsResult.Tables.Count > 0)
                {
                    foreach (var row in dsResult.Tables[0].AsEnumerable())
                    {
                        data.Add(Convert.ToString(row["remark_description"]));
                    }
                }
            }

            return data;
        }

        #endregion

        #region Quotation Service
        [WebMethod]
        public static List<QuotationDetail> AddServiceParent(string id, string description, string qty, string unitPrice, string serviceType, string unitType, string discountValue, string discountType)
        {
            var discountVal = Convert.ToDecimal(!string.IsNullOrEmpty(discountValue) ? discountValue : "0");
            List<QuotationDetail> data = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            if (data == null)
            {
                data.Add(new QuotationDetail()
                {
                    id = (data.Count + 1) * -1,
                    parant_id = 0,
                    product_no = string.Empty,
                    unit_code = unitType,
                    sort_no = (data.Count == 0) ? 1 : (from t in data select t.sort_no).Max() + 1,
                    quotation_description = description,
                    qty = string.IsNullOrEmpty(serviceType) ? 0 : Convert.ToInt32(qty),
                    discount_percentage = discountType == "P" ? Convert.ToDecimal(discountVal) : 0m,
                    discount_amount = discountType == "A" ? Convert.ToDecimal(discountVal) : 0,


                    unit_price = string.IsNullOrEmpty(serviceType) ? 0 : Convert.ToDecimal(unitPrice),
                    total_amount = string.IsNullOrEmpty(serviceType) ? 0 : Convert.ToInt32(qty) * Convert.ToDecimal(unitPrice),
                    package_id = 0,


                });
            }
            else
            {
                if (Convert.ToInt32(id) == 0)
                {
                    data.Add(new QuotationDetail()
                    {
                        id = (data.Count + 1) * -1,
                        parant_id = 0,
                        product_no = string.Empty,
                        unit_code = unitType,
                        sort_no = (data.Count == 0) ? 1 : (from t in data select t.sort_no).Max() + 1,
                        quotation_description = description,
                        qty = string.IsNullOrEmpty(serviceType) ? 0 : Convert.ToInt32(qty),
                        discount_percentage = discountType == "P" ? Convert.ToDecimal(discountVal) : 0m,
                        discount_amount = discountType == "A" ? Convert.ToDecimal(discountVal) : 0,
                        unit_price = string.IsNullOrEmpty(serviceType) ? 0 : Convert.ToDecimal(unitPrice),
                        total_amount = string.IsNullOrEmpty(serviceType) ? 0 : Math.Round(Convert.ToInt32(qty) * Convert.ToDecimal(unitPrice), 2),
                        package_id = 0

                    });
                }
                else
                {
                    var row = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (row != null)
                    {
                        row.quotation_description = description;
                        row.qty = string.IsNullOrEmpty(serviceType) ? 0 : Convert.ToInt32(qty);
                        row.unit_price = string.IsNullOrEmpty(serviceType) ? 0 : Convert.ToDecimal(unitPrice);
                        row.discount_percentage = discountType == "P" ? Convert.ToDecimal(discountVal) : 0m;
                        row.discount_amount = discountType == "A" ? Convert.ToDecimal(discountVal) : 0;
                        row.unit_code = unitType;
                        row.total_amount = string.IsNullOrEmpty(serviceType) ? 0 : Math.Round(Convert.ToInt32(qty) * Convert.ToDecimal(unitPrice), 2);

                        if (discountType == "P")
                        {
                            row.discount_total = (row.total_amount * row.discount_percentage) / 100;
                        }
                        else if (discountType == "A")
                        {
                            row.discount_total = row.discount_amount;
                        }
                    }
                }
            }
            HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"] = data;
            return data;
        }
        [WebMethod]
        public static List<QuotationDetail> AddServiceDetail(string id, string parentId, string description, string qty, string unitPrice, string serviceType, string unitType, string discountValue, string discountType)
        {
            var discountVal = Convert.ToDecimal(!string.IsNullOrEmpty(discountValue) ? discountValue : "0");
            List<QuotationDetail> data = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            if (data == null)
            {
                data.Add(new QuotationDetail()
                {
                    id = (data.Count + 1) * -1,
                    parant_id = Convert.ToInt32(parentId),
                    product_no = string.Empty,
                    unit_code = unitType,
                    sort_no = (data.Count == 0) ? 1 : (from t in data select t.sort_no).Max() + 1,
                    quotation_description = description,
                    qty = !string.IsNullOrEmpty(serviceType) ? 0 : Convert.ToInt32(qty),
                    unit_price = !string.IsNullOrEmpty(serviceType) ? 0 : Convert.ToDecimal(unitPrice),
                    total_amount = !string.IsNullOrEmpty(serviceType) ? 0 : Math.Round(Convert.ToInt32(qty) * Convert.ToDecimal(unitPrice), 2),
                    package_id = 0,
                    discount_amount = !string.IsNullOrEmpty(serviceType) ? 0 : (discountType == "A" ? Convert.ToDecimal(discountVal) : 0),
                    discount_percentage = !string.IsNullOrEmpty(serviceType) ? 0 : (discountType == "P" ? Convert.ToDecimal(discountVal) : 0)

                });
            }
            else
            {
                if (Convert.ToInt32(id) == 0)
                {
                    data.Add(new QuotationDetail()
                    {
                        id = (data.Count + 1) * -1,
                        parant_id = Convert.ToInt32(parentId),
                        sort_no = ((from t in data where t.parant_id == Convert.ToInt32(parentId) select t).ToList().Count == 0) ? 1
                                    : (from t in data where t.parant_id == Convert.ToInt32(parentId) select t.sort_no).Max() + 1,
                        product_no = string.Empty,
                        unit_code = unitType,
                        quotation_description = description,
                        qty = !string.IsNullOrEmpty(serviceType) ? 0 : Convert.ToInt32(qty),
                        unit_price = !string.IsNullOrEmpty(serviceType) ? 0 : Convert.ToDecimal(unitPrice),
                        total_amount = !string.IsNullOrEmpty(serviceType) ? 0 : Math.Round(Convert.ToInt32(qty) * Convert.ToDecimal(unitPrice), 2),
                        package_id = 0,
                        discount_amount = !string.IsNullOrEmpty(serviceType) ? 0 : (discountType == "A" ? Convert.ToDecimal(discountVal) : 0),
                        discount_percentage = !string.IsNullOrEmpty(serviceType) ? 0 : (discountType == "P" ? Convert.ToDecimal(discountVal) : 0)
                    });
                }
                else
                {
                    var row = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (row != null)
                    {
                        row.quotation_description = description;
                        row.qty = !string.IsNullOrEmpty(serviceType) ? 0 : Convert.ToInt32(qty);
                        row.unit_price = !string.IsNullOrEmpty(serviceType) ? 0 : Convert.ToDecimal(unitPrice);
                        row.discount_percentage = discountType == "P" ? Convert.ToDecimal(discountVal) : 0m;
                        row.discount_amount = discountType == "A" ? Convert.ToDecimal(discountVal) : 0;
                        row.total_amount = !string.IsNullOrEmpty(serviceType) ? 0 : Math.Round(Convert.ToInt32(qty) * Convert.ToDecimal(unitPrice), 2);

                        if (discountType == "P")
                        {
                            row.discount_total = (row.total_amount * row.discount_percentage) / 100;
                        }
                        else if (discountType == "A")
                        {
                            row.discount_total = row.discount_amount;
                        }
                    }
                }
            }
            HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"] = data;
            return data;
        }
        [WebMethod]
        public static List<QuotationDetail> DeleteServiceDetail(string id)
        {
            List<QuotationDetail> data = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            if (data != null)
            {
                var selectedData = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (Convert.ToInt32(id) < 0)
                {
                    if (selectedData.parant_id == 0)
                    {
                        foreach (var row in (from t in data
                                             where t.parant_id == Convert.ToInt32(id)
                                             select t).ToList())
                        {
                            data.Remove(row);
                        }
                        data.Remove(selectedData);
                    }
                    else
                    {
                        data.Remove(selectedData);
                    }

                }
                else
                {
                    if (selectedData.parant_id == 0)
                    {
                        foreach (var row in (from t in data
                                             where t.parant_id == Convert.ToInt32(id)
                                             select t).ToList())
                        {
                            row.is_deleted = true;
                        }
                        selectedData.is_deleted = true;
                    }
                    else
                    {
                        selectedData.is_deleted = true;
                    }
                }
            }
            HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"] = data;
            return data;
        }
        [WebMethod]
        public static QuotationDetail ShowEditServiceDetail(string id)
        {
            List<QuotationDetail> data = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            QuotationDetail selectedData = selectedData = new QuotationDetail();

            if (data != null)
            {
                selectedData = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
            }
            HttpContext.Current.Session["SESSION_SELECTED_PRODUCT_EDIT_QUATATION"] = selectedData;
            return selectedData;
        }
        protected void gridViewService_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (cbbQuotationType.Value == "D" || cbbQuotationType.Value == "C" || cbbQuotationType.Value == "O")
            {
                gridViewService.DataSource = (from t in quotationDetailList
                                              where t.is_deleted == false &&
                                              t.parant_id == 0
                                              select t).ToList();
                //if (rdoLumpsum.Checked)
                //{
                    if (cbDiscountByItem.Checked)
                    {
                        if (cbbDiscountByItem.Value == "P")
                        {
                            gridViewService.Columns[8].Visible = false;
                            gridViewService.Columns[9].Visible = true;
                            gridViewService.Columns[10].Visible = true;
                        }
                        else if (cbbDiscountByItem.Value == "A")
                        {
                            gridViewService.Columns[8].Visible = true;
                            gridViewService.Columns[9].Visible = false;
                            gridViewService.Columns[10].Visible = true;
                        }
                    }
                //}
                else
                {
                    gridViewService.Columns[8].Visible = false;
                    gridViewService.Columns[9].Visible = false;
                    gridViewService.Columns[10].Visible = false;
                }
                gridViewService.FilterExpression = FilterBag.GetExpression(false);
                gridViewService.DataBind();
                gridViewService.DetailRows.ExpandRow(0);


            }

            //else
            //{
            //    if (cbDiscountByItem.Checked)
            //    {
            //        if (cbbDiscountByItem.Value == "P")
            //        {
            //            gridViewService.Columns[7].Visible = false;
            //            gridViewService.Columns[8].Visible = true;
            //            gridViewService.Columns[9].Visible = true;
            //        }
            //        else if (cbbDiscountByItem.Value == "A")
            //        {
            //            gridViewService.Columns[7].Visible = true;
            //            gridViewService.Columns[8].Visible = false;
            //            gridViewService.Columns[9].Visible = true;
            //        }
            //    }
            //    gridViewService.DataSource = null;
            //    gridViewService.FilterExpression = FilterBag.GetExpression(false);
            //    gridViewService.DataBind();
            //}
        }
        protected void detailServiceGrid_BeforePerformDataSelect(object sender, EventArgs e)
        {
            Session["parentId"] = (sender as ASPxGridView).GetMasterRowKeyValue();
            var parentId = (sender as ASPxGridView).GetMasterRowKeyValue();
            if (parentId != null)
            {
                var detailData = (from t in quotationDetailList
                                  where t.parant_id == Convert.ToInt32(parentId) &&
                                  t.is_deleted == false
                                  select t).ToList();
            }
            gridViewService.SettingsDetail.ShowDetailRow = true;
            gridViewService.DetailRows.ExpandRowByKey(parentId);

        }
        protected void detailServiceGrid_Init(object sender, EventArgs e)
        {

            gridViewService.DetailRows.CollapseAllRows();
            var detailServiceGrid = sender as ASPxGridView;
            if (detailServiceGrid != null)
            {
                var detailData = (List<QuotationDetail>)ViewState["ChildServiceData"];
                detailServiceGrid.DataSource = detailData;
                detailServiceGrid.DataBind();
            }

        }
        protected void detailServiceGrid_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {

        }
        #endregion

        #region Quotation Maintenance
        protected void gridViewMaintenance_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string searchText = string.Empty;
            if (e.Parameters.ToString().Contains("Search"))
            {
                var splitStr = e.Parameters.ToString().Split('|');
                string productType = string.Empty;

                if (splitStr.Length > 0)
                {
                    searchText = splitStr[2];
                    /*using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@search_name", SqlDbType.VarChar,500) {Value = searchText  }

                        };
                        conn.Open();
                        //dsMaintenanceModel = SqlHelper.ExecuteDataset(conn, "sp_maintanance_mapping_list_search", arrParm.ToArray());
                        dsMaintenanceModel = SqlHelper.ExecuteDataset(conn, "sp_maintenance_model_list_popup", arrParm.ToArray());
                        dsMaintenanceModel.Tables[0].Columns.Add("is_selected", typeof(bool));
                        ViewState["dsMaintenanceModel"] = dsMaintenanceModel;
                        conn.Close();
                    }*/
                }
            }

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                             new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = searchText },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },

                        };
                conn.Open();
                dsMaintenanceModel = SqlHelper.ExecuteDataset(conn, "sp_maintenance_model_list_popup", arrParm.ToArray());
                dsMaintenanceModel.Tables[0].Columns.Add("is_selected", typeof(bool));
                //ViewState["dsMaintenanceModel"] = dsMaintenanceModel;
                conn.Close();
            }

            gridViewMaintenance.DataSource = dsMaintenanceModel;
            gridViewMaintenance.DataBind();
            gridViewMaintenance.PageIndex = 0;
        }

        [WebMethod]
        public static string GetMaintenanceServicePackage(string id)
        {
            //var data = (List<MaintenanceServiceDetail>)HttpContext.Current.Session["SESSION_SELECTED_MAINTENANCE_DETAIL_QUATATION"];
            var data = new List<MaintenanceServiceDetail>();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@model_id", SqlDbType.Int) { Value = id },
                            new SqlParameter("@package_id", SqlDbType.Int) { Value = 0 },
                        };
                conn.Open();
                var dsServiceMaintenance = SqlHelper.ExecuteDataset(conn, "sp_maintenance_package_list", arrParm.ToArray());
                conn.Close();
                if (dsServiceMaintenance != null)
                {
                    foreach (var row in dsServiceMaintenance.Tables[0].AsEnumerable())
                    {
                        var checkExist = (from t in data
                                          where t.package_id == Convert.ToInt32(row["package_id"])
                                          select t).FirstOrDefault();
                        if (checkExist == null)
                        {
                            data.Add(new MaintenanceServiceDetail()
                            {
                                model_id = Convert.IsDBNull(row["model_id"]) ? 0 : Convert.ToInt32(row["model_id"]),
                                model_name = Convert.IsDBNull(row["model_name"]) ? string.Empty : Convert.ToString(row["model_name"]),
                                package_id = Convert.IsDBNull(row["package_id"]) ? 0 : Convert.ToInt32(row["package_id"]),
                                count_year = Convert.IsDBNull(row["count_year"]) ? 0 : Convert.ToInt32(row["count_year"]),
                                item_of_part = Convert.IsDBNull(row["item_of_part"]) ? string.Empty : Convert.ToString(row["item_of_part"]),
                                running_hour = Convert.IsDBNull(row["running_hours"]) ? string.Empty : Convert.ToString(row["running_hours"]),
                            });
                        }
                    }
                }
                HttpContext.Current.Session["SESSION_MAINTENANCE_PACKAGE_QUATATION"] = data;

            }
            return "SUCESS";
        }

        protected void gridViewMaintenancePackage_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridViewMaintenancePackage.DataSource = maintenanceServicePackage;
            gridViewMaintenancePackage.DataBind();
        }

        [WebMethod]
        public static List<MaintenanceServiceDetail> SelectedMaintenancePackage(string id, string model_id)
        {
            var data = (List<MaintenanceServiceDetail>)HttpContext.Current.Session["SESSION_SELECTED_MAINTENANCE_PACKAGE_QUATATION"];
            var maintenanceData = (List<MaintenanceServiceDetail>)HttpContext.Current.Session["SESSION_MAINTENANCE_PACKAGE_QUATATION"];
            var productDetail = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION_TEMP"];

            //data.Clear();

            data = new List<MaintenanceServiceDetail>();
            var row = (from t in maintenanceData where t.package_id == Convert.ToInt32(id) select t).FirstOrDefault();
            if (row != null)
            {
                data.Add(row);
            }
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@model_id", SqlDbType.Int) { Value = model_id },
                            new SqlParameter("@package_id", SqlDbType.Int) { Value = id },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = "" },
                        };
                conn.Open();
                var dsServicePartList = SqlHelper.ExecuteDataset(conn, "sp_maintanance_mapping_list", arrParm.ToArray());
                conn.Close();
                productDetail = new List<QuotationDetail>();
                if (dsServicePartList != null)
                {
                    foreach (var detailRow in dsServicePartList.Tables[0].AsEnumerable())
                    {
                        var quantity_package = Convert.IsDBNull(detailRow["quantity_package"]) ? 1 : Convert.ToInt32(detailRow["quantity_package"]);
                        productDetail.Add(new QuotationDetail()
                        {
                            id = (productDetail.Count + 1) * -1,
                            package_id = Convert.ToInt32(id),
                            product_no = Convert.IsDBNull(detailRow["part_no"]) ? string.Empty : Convert.ToString(detailRow["part_no"]),
                            model_id = Convert.ToInt32(model_id),//Convert.IsDBNull(detailRow["model_id"]) ? 0 : Convert.ToInt32(detailRow["model_id"]),
                            product_id = Convert.IsDBNull(detailRow["part_id"]) ? 0 : Convert.ToInt32(detailRow["part_id"]),
                            quotation_description = Convert.IsDBNull(detailRow["part_no"]) ? string.Empty : Convert.ToString(detailRow["part_name_tha"]),
                            qty = Convert.IsDBNull(detailRow["quantity_package"]) ? 1 : Convert.ToInt32(detailRow["quantity_package"]),
                            unit_price = Convert.IsDBNull(detailRow["selling_price"]) ? 0 : Convert.ToDecimal(detailRow["selling_price"]),
                            unit_code = Convert.IsDBNull(detailRow["unit_code"]) ? string.Empty : Convert.ToString(detailRow["unit_code"]),
                            min_unit_price = Convert.IsDBNull(detailRow["min_selling_price"]) ? 0 : Convert.ToDecimal(detailRow["min_selling_price"]),
                            total_amount = quantity_package * (Convert.IsDBNull(detailRow["selling_price"]) ? 0 : Convert.ToDecimal(detailRow["selling_price"])),
                            //sort_no = (productDetail.Count == 0) ? 1 : (from t in productDetail select t.sort_no).Max() + 1,
                            sort_no = Convert.IsDBNull(detailRow["sort_no"]) ? 0 : Convert.ToInt32(detailRow["sort_no"]),
                            product_type = "S"
                        });

                    }
                    var temp = productDetail.OrderBy(o => o.sort_no).ToList();
                    productDetail = temp;
                }
            }
            HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION_TEMP"] = productDetail;
            HttpContext.Current.Session["SESSION_SELECTED_MAINTENANCE_PACKAGE_QUATATION"] = data;

            return data;
        }

        [WebMethod]
        public static string GetPartListData(string id, string package_id)
        {
            var data = (List<MaintenanceServiceDetail>)HttpContext.Current.Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"];
            var productDetail = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            data = new List<MaintenanceServiceDetail>();
            if (productDetail != null)
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@model_id", SqlDbType.Int) { Value = Convert.ToInt32(id) },
                            new SqlParameter("@package_id", SqlDbType.Int) { Value = package_id == "" ? 0 : Convert.ToInt32(package_id) },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = "" },
                        };
                    conn.Open();
                    var dsServicePartList = SqlHelper.ExecuteDataset(conn, "sp_maintanance_mapping_list", arrParm.ToArray());
                    conn.Close();
                    if (dsServicePartList != null)
                    {
                        foreach (var detailRow in dsServicePartList.Tables[0].AsEnumerable())
                        {
                            data.Add(new MaintenanceServiceDetail()
                            {
                                part_list_id = Convert.IsDBNull(detailRow["part_id"]) ? 0 : Convert.ToInt32(detailRow["part_id"]),
                                item_no = Convert.IsDBNull(detailRow["item_no"]) ? string.Empty : Convert.ToString(detailRow["item_no"]),
                                part_no = Convert.IsDBNull(detailRow["part_no"]) ? string.Empty : Convert.ToString(detailRow["part_no"]),
                                part_name_tha = Convert.IsDBNull(detailRow["part_name_tha"]) ? string.Empty : Convert.ToString(detailRow["part_name_tha"]),
                                part_name_eng = Convert.IsDBNull(detailRow["part_name_eng"]) ? string.Empty : Convert.ToString(detailRow["part_name_eng"]),
                                unit_code = Convert.IsDBNull(detailRow["unit_code"]) ? string.Empty : Convert.ToString(detailRow["unit_code"]),
                                quantity_package = Convert.IsDBNull(detailRow["quantity_package"]) ? 1 : Convert.ToInt32(detailRow["quantity_package"]),
                                //description_tha = Convert.IsDBNull(detailRow["description_tha"]) ? string.Empty : Convert.ToString(detailRow["description_tha"]),
                                //description_eng = Convert.IsDBNull(detailRow["description_eng"]) ? string.Empty : Convert.ToString(detailRow["description_eng"]),
                                selling_price = Convert.IsDBNull(detailRow["selling_price"]) ? 0 : Convert.ToInt32(detailRow["selling_price"]),
                                min_selling_price = Convert.IsDBNull(detailRow["min_selling_price"]) ? 0 : Convert.ToInt32(detailRow["min_selling_price"]) //-- Aey 1/4/18
                            });

                        }
                    }
                }
                HttpContext.Current.Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"] = data;

            }
            return "SUCCESS";
        }
        [WebMethod]
        public static List<MaintenanceServiceDetail> AddPartListData(string id)
        {
            var data = (List<MaintenanceServiceDetail>)HttpContext.Current.Session["SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION"];
            var partListData = (List<MaintenanceServiceDetail>)HttpContext.Current.Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"];
            var row = (from t in partListData where t.part_list_id == Convert.ToInt32(id) select t).FirstOrDefault();
            if (data == null)
            {
                if (row != null)
                {
                    data = new List<MaintenanceServiceDetail>();
                    data.Add(row);
                    row.is_selected = true; // check partlist
                }

            }
            else
            {
                var checkExist = (from t in data where t.part_list_id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (checkExist == null)
                {
                    if (row != null)
                    {
                        data.Add(row);
                        row.is_selected = true; // check partlist
                    }
                }
                else
                {
                    data.Remove(checkExist);
                    row.is_selected = false; // uncheck partlist
                }
            }
            HttpContext.Current.Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"] = partListData;
            HttpContext.Current.Session["SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION"] = data;
            return data;
        }

        [WebMethod]
        public static List<QuotationDetail> SubmitPartList(string quotationType)
        {
            var selectedData = (List<MaintenanceServiceDetail>)HttpContext.Current.Session["SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION"];
            var productDetail = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];

            if (selectedData != null)
            {
                foreach (var detailRow in selectedData)
                {
                    var existData = (from t in productDetail where t.product_id == detailRow.part_list_id select t).FirstOrDefault();
                    if (existData != null)
                    {
                        existData.qty += 1;
                    }
                    else
                    {
                        decimal discount_amount = 0;
                        decimal discount_percentage = 0;
                        string discount_type = "0";
                        if (productDetail.Count > 0)
                        {
                            discount_amount = productDetail[0].discount_amount;
                            discount_percentage = productDetail[0].discount_percentage;
                            discount_type = productDetail[0].discount_type;
                        }

                        productDetail.Add(new QuotationDetail()
                        {
                            id = (productDetail.Count + 1) * -1,
                            package_id = 0,
                            product_no = detailRow.part_no,
                            product_id = detailRow.part_list_id,
                            unit_code = detailRow.unit_code,
                            quotation_description = detailRow.part_name_tha,
                            qty = detailRow.quantity_package,
                            unit_price = detailRow.selling_price,
                            min_unit_price = detailRow.min_selling_price,
                            total_amount = detailRow.quantity_package * detailRow.selling_price,
                            sort_no = (productDetail.Count == 0) ? 1 : (from t in productDetail select t.sort_no).Max() + 1,
                            product_type = "S",
                            discount_amount = discount_amount,
                            discount_percentage = discount_percentage,
                            discount_type = discount_type

                        });

                        //  Re id for all object only MA
                        /*if (quotationType == "M")
                        {
                            for (int i = 0; i < productDetail.Count; i++)
                            {
                                productDetail[i].id = (i + 1) * -1;
                            }
                        }*/
                    }

                }
            }
            HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"] = productDetail;
            HttpContext.Current.Session["SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION"] = null;
            return productDetail;
        }

        protected void gridViewPartList_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.ToString().Contains("Search"))
            {
                var splitStr = e.Parameters.ToString().Split('|');
                string searchText = string.Empty;
                string productType = string.Empty;
                int model_id = 0;
                if (splitStr.Length > 0)
                {
                    searchText = splitStr[2];
                    model_id = Convert.ToInt32(splitStr[3]);
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@model_id", SqlDbType.Int) { Value = model_id },
                            new SqlParameter("@package_id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@search_text",SqlDbType.VarChar,500) {Value = searchText }
                        };
                        conn.Open();
                        var dsServicePartList = SqlHelper.ExecuteDataset(conn, "sp_maintanance_mapping_list_search", arrParm.ToArray());
                        conn.Close();
                        maintenanceServicePartList = new List<MaintenanceServiceDetail>();
                        if (dsServicePartList != null)
                        {
                            foreach (var detailRow in dsServicePartList.Tables[0].AsEnumerable())
                            {
                                maintenanceServicePartList.Add(new MaintenanceServiceDetail()
                                {
                                    part_list_id = Convert.IsDBNull(detailRow["part_id"]) ? 0 : Convert.ToInt32(detailRow["part_id"]),
                                    item_no = Convert.IsDBNull(detailRow["item_no"]) ? string.Empty : Convert.ToString(detailRow["item_no"]),
                                    part_no = Convert.IsDBNull(detailRow["part_no"]) ? string.Empty : Convert.ToString(detailRow["part_no"]),
                                    part_name_tha = Convert.IsDBNull(detailRow["part_name_tha"]) ? string.Empty : Convert.ToString(detailRow["part_name_tha"]),
                                    part_name_eng = Convert.IsDBNull(detailRow["part_name_eng"]) ? string.Empty : Convert.ToString(detailRow["part_name_eng"]),
                                    unit_code = Convert.IsDBNull(detailRow["unit_code"]) ? string.Empty : Convert.ToString(detailRow["unit_code"]),
                                    selling_price = Convert.IsDBNull(detailRow["selling_price"]) ? 0 : Convert.ToInt32(detailRow["selling_price"]),
                                    quantity_package = Convert.IsDBNull(detailRow["quantity_package"]) || model_id == 0 ? 1 : Convert.ToInt32(detailRow["quantity_package"]),
                                });
                                HttpContext.Current.Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"] = maintenanceServicePartList;

                            }
                        }
                        if (maintenanceServicePartList.Count > 0)
                        {
                            foreach (var row in selectedMaintenanceServicePartList)
                            {
                                var existItem = (from t in maintenanceServicePartList where t.part_list_id == row.part_list_id select t).FirstOrDefault();
                                if (existItem != null)
                                {
                                    existItem.is_selected = true;
                                }
                            }
                        }
                    }
                }
            }
            gridViewPartList.DataSource = maintenanceServicePartList;
            gridViewPartList.DataBind();
            gridViewPartList.PageIndex = 0;
        }

        [WebMethod]
        public static string SelectAllPartList(bool selected)
        {
            var SelectpartListData = (List<MaintenanceServiceDetail>)HttpContext.Current.Session["SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION"];
            var data = (List<MaintenanceServiceDetail>)HttpContext.Current.Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"];
            
            if (data != null)
            {
                if (selected)
                {
                    SelectpartListData = new List<MaintenanceServiceDetail>();
                    foreach (var row in data)
                    {
                        SelectpartListData.Add(row);
                        row.is_selected = true;
                    }
                }
                else
                {
                    SelectpartListData = new List<MaintenanceServiceDetail>();
                    foreach (var row in data)
                    {
                        row.is_selected = false;
                    }
                }
               
            }
            HttpContext.Current.Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"] = data;
            HttpContext.Current.Session["SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION"] = SelectpartListData;

            return "SUCCESS";
        }
        #endregion

        #region Quotation Spare Part
        [WebMethod]
        public static string GetSparePartData() // ใช้ Dataset เดียวกับ PartList ของ Maintenance
        {
            var quotationDetail = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            var selectedData = new List<MaintenanceServiceDetail>();
            var data = (List<MaintenanceServiceDetail>)HttpContext.Current.Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"];
            data = new List<MaintenanceServiceDetail>();

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                             new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                        };
                conn.Open();
                var dsServicePartList = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_list_popup", arrParm.ToArray());
                conn.Close();
                if (dsServicePartList != null)
                {
                    foreach (var detailRow in dsServicePartList.Tables[0].AsEnumerable())
                    {
                        data.Add(new MaintenanceServiceDetail()
                        {
                            part_list_id = Convert.IsDBNull(detailRow["id"]) ? 0 : Convert.ToInt32(detailRow["id"]),
                            //item_no = Convert.IsDBNull(detailRow["item_no"]) ? string.Empty : Convert.ToString(detailRow["item_no"]),
                            part_no = Convert.IsDBNull(detailRow["part_no"]) ? string.Empty : Convert.ToString(detailRow["part_no"]),
                            part_name_tha = Convert.IsDBNull(detailRow["part_name_tha"]) ? string.Empty : Convert.ToString(detailRow["part_name_tha"]),
                            part_name_eng = Convert.IsDBNull(detailRow["part_name_eng"]) ? string.Empty : Convert.ToString(detailRow["part_name_eng"]),
                            unit_code = Convert.IsDBNull(detailRow["unit_code"]) ? string.Empty : Convert.ToString(detailRow["unit_code"]),
                            //quantity_package = Convert.IsDBNull(detailRow["quantity_package"]) ? 0 : Convert.ToInt32(detailRow["quantity_package"]),
                            //description_tha = Convert.IsDBNull(detailRow["description_tha"]) ? string.Empty : Convert.ToString(detailRow["description_tha"]),
                            //description_eng = Convert.IsDBNull(detailRow["description_eng"]) ? string.Empty : Convert.ToString(detailRow["description_eng"]),
                            selling_price = Convert.IsDBNull(detailRow["selling_price"]) ? 0 : Convert.ToInt32(detailRow["selling_price"]),
                            min_selling_price = Convert.IsDBNull(detailRow["min_selling_price"]) ? 0 : Convert.ToInt32(detailRow["min_selling_price"]) //-- Aey 1/4/18                            
                        });

                    }
                }
            }
            selectedData.AddRange(data);
            foreach (var row in quotationDetail.Where(t => !t.is_deleted).ToList())
            {
                var isSelected = selectedData.Where(t => t.part_list_id == row.product_id).FirstOrDefault();
                if (isSelected != null)
                {
                    isSelected.is_selected = true;
                }
            }
            HttpContext.Current.Session["SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION"] = selectedData.Where(t => t.is_selected).ToList();
            HttpContext.Current.Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"] = data;


            return "SUCCESS";
        }

        [WebMethod]
        public static List<MaintenanceServiceDetail> AddSparePartData(string id) // ใช้ Dataset เดียวกับ PartList ของ Maintenance
        {
            var data = (List<MaintenanceServiceDetail>)HttpContext.Current.Session["SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION"];
            var partListData = (List<MaintenanceServiceDetail>)HttpContext.Current.Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"];
            var row = (from t in partListData where t.part_list_id == Convert.ToInt32(id) select t).FirstOrDefault();
            if (data == null || data.Count() == 0)
            {
                if (row != null)
                {
                    data = new List<MaintenanceServiceDetail>();
                    data.Add(row);
                    row.is_selected = true;
                }
            }
            else
            {
                var checkExist = (from t in data where t.part_list_id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (checkExist == null)
                {
                    if (row != null)
                    {
                        data.Add(row);
                        row.is_selected = true;
                    }
                }
                else
                {
                    data.Remove(checkExist);
                    row.is_selected = false;
                }
            }
            HttpContext.Current.Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"] = partListData;
            HttpContext.Current.Session["SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION"] = data;
            return data;
        }

        [WebMethod]
        public static List<QuotationDetail> SubmitSparePart()  // ใช้ Dataset เดียวกับ PartList ของ Maintenance
        {
            var selectedData = (List<MaintenanceServiceDetail>)HttpContext.Current.Session["SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION"];
            var productDetail = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            if (selectedData != null)
            {
                //productDetail = new List<QuotationDetail>();
                foreach (var detailRow in selectedData)
                {
                    var existData = (from t in productDetail where t.product_id == detailRow.part_list_id select t).FirstOrDefault();
                    if (existData != null)
                    {
                        // existData.qty += 1;
                        //existData.total_amount = existData.qty * existData.unit_price;
                    }
                    else
                    {

                        productDetail.Add(new QuotationDetail()
                        {
                            id = (productDetail.Count + 1) * -1,
                            package_id = 0,
                            product_no = detailRow.part_no,
                            product_id = detailRow.part_list_id,
                            unit_code = detailRow.unit_code,
                            quotation_description = detailRow.part_name_tha,
                            qty = 1,//detailRow.quantity_package,
                            unit_price = detailRow.selling_price,
                            min_unit_price = detailRow.min_selling_price,
                            total_amount = 1 * detailRow.selling_price,
                            sort_no = (productDetail.Count == 0) ? 1 : (from t in productDetail select t.sort_no).Max() + 1,
                            product_type = "S"
                        });
                    }

                }
                var tempQUDetail = new List<QuotationDetail>();
                tempQUDetail.AddRange(productDetail);
                foreach (var item in tempQUDetail)
                {
                    var rowDeleted = selectedData.Where(t => t.part_list_id == item.product_id).FirstOrDefault();
                    if (rowDeleted == null)
                    {
                        if (item.id > 0)
                        {
                            item.is_deleted = true;
                        }
                        else
                        {
                            productDetail.Remove(item);
                        }
                    }
                }
                //DELETE

            }
            HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"] = productDetail;
            HttpContext.Current.Session["SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION"] = null;
            return productDetail;
        }

        [WebMethod]
        public static void CancelSparePart()
        {
            HttpContext.Current.Session["SESSION_SELECTED_MAINTENANCE_PARTLIST_QUATATION"] = null;
        }

        protected void gridViewSparepart_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.ToString().Contains("Search"))
            {
                var splitStr = e.Parameters.ToString().Split('|');
                string searchText = string.Empty;
                string productType = string.Empty;

                if (splitStr.Length > 0)
                {
                    searchText = splitStr[2];
                    maintenanceServicePartList = new List<MaintenanceServiceDetail>();
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                             new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = searchText },
                        };
                        conn.Open();
                        var dsServicePartList = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_list", arrParm.ToArray());
                        conn.Close();

                        if (dsServicePartList != null)
                        {
                            foreach (var detailRow in dsServicePartList.Tables[0].AsEnumerable())
                            {
                                maintenanceServicePartList.Add(new MaintenanceServiceDetail()
                                {
                                    part_list_id = Convert.IsDBNull(detailRow["id"]) ? 0 : Convert.ToInt32(detailRow["id"]),
                                    part_no = Convert.IsDBNull(detailRow["part_no"]) ? string.Empty : Convert.ToString(detailRow["part_no"]),
                                    part_name_tha = Convert.IsDBNull(detailRow["part_name_tha"]) ? string.Empty : Convert.ToString(detailRow["part_name_tha"]),
                                    part_name_eng = Convert.IsDBNull(detailRow["part_name_eng"]) ? string.Empty : Convert.ToString(detailRow["part_name_eng"]),
                                    unit_code = Convert.IsDBNull(detailRow["unit_code"]) ? string.Empty : Convert.ToString(detailRow["unit_code"]),
                                    selling_price = Convert.IsDBNull(detailRow["selling_price"]) ? 0 : Convert.ToInt32(detailRow["selling_price"]),
                                    min_selling_price = Convert.IsDBNull(detailRow["min_selling_price"]) ? 0 : Convert.ToInt32(detailRow["min_selling_price"]) //-- Aey 1/4/18
                                });

                            }
                        }

                        if (maintenanceServicePartList.Count > 0)
                        {
                            foreach (var row in selectedProductList.Where(t => !t.is_deleted))
                            {
                                var existItem = (from t in maintenanceServicePackage where t.part_list_id == row.product_id select t).FirstOrDefault();
                                if (existItem != null)
                                {
                                    existItem.is_selected = true;
                                }
                            }
                        }
                    }
                }
            }
            gridViewSparepart.DataSource = maintenanceServicePartList;
            gridViewSparepart.DataBind();
        }

        #endregion

        protected void gridViewService_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {
            gridViewService.DetailRows.CollapseAllRows();
            var detailServiceGrid = sender as ASPxGridView;
            var focusParentGrid = gridViewService.FocusedRowIndex;
            var key = gridViewService.GetRowValues(e.VisibleIndex, "id");
            if (key != null)
            {
                var detailData = (from t in quotationDetailList
                                  where t.parant_id == Convert.ToInt32(key) &&
                                  t.is_deleted == false
                                  select t).ToList();
                ViewState["ChildServiceData"] = detailData;
                detailServiceGrid.DetailRows.ExpandRow(e.VisibleIndex);// = detailData;
                detailServiceGrid.DataBind();
            }
            else
            {
                detailServiceGrid.DataSource = null;
                detailServiceGrid.DataBind();
            }
        }

        protected void gridView2_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxCheckBox checkBox = gridView2.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkBox") as ASPxCheckBox;
                if (checkBox != null)
                {
                    if (e.DataColumn.FieldName == "is_selected")
                    {
                        if (dtProductToSelect != null)
                        {
                            var row = (from t in dtProductToSelect.Tables[0].AsEnumerable() where t["id"].ToString() == e.KeyValue.ToString() select t).FirstOrDefault();
                            if (row != null)
                            {
                                checkBox.Checked = (row["is_selected"] == DBNull.Value ? false : Convert.ToBoolean(row["is_selected"]));
                            }
                        }
                        //checkBox.Checked = (row["is_selected"]) == DBNull.Value ? false : true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gridViewSparepart_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxCheckBox checkBox = gridViewSparepart.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkBox") as ASPxCheckBox;
                if (checkBox != null)
                {
                    if (e.DataColumn.FieldName == "is_selected")
                    {
                        if (maintenanceServicePartList != null)
                        {
                            var row = (from t in maintenanceServicePartList where t.part_list_id == Convert.ToInt32(e.KeyValue) select t).FirstOrDefault();
                            if (row != null)
                            {
                                checkBox.Checked = row.is_selected;
                            }
                        }
                        //checkBox.Checked = (row["is_selected"]) == DBNull.Value ? false : true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gridViewPartList_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            ASPxCheckBox checkBox = gridViewPartList.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkBox") as ASPxCheckBox;
            if (checkBox != null)
            {
                if (e.DataColumn.FieldName == "is_selected")
                {
                    if (maintenanceServicePartList != null)
                    {
                        var row = (from t in maintenanceServicePartList where t.part_list_id == Convert.ToInt32(e.KeyValue) select t).FirstOrDefault();
                        if (row != null)
                        {
                            checkBox.Checked = row.is_selected;
                        }
                    }
                    //checkBox.Checked = (row["is_selected"]) == DBNull.Value ? false : true;
                }
            }
        }

        [WebMethod]
        public static string ClearSelectedCheckBox(string quotationType)
        {
            var productDs = (DataSet)HttpContext.Current.Session["SESSION_PRODUCT_TO_SELECT_QUATATION"];
            var partListData = (List<MaintenanceServiceDetail>)HttpContext.Current.Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"];
            var annualHistoryData = (List<AnnualServiceItem>)HttpContext.Current.Session["SESSION_HISTORY_ANNUAL_SERVICE_ITEM_QUATATION"];
            if (quotationType == "P")
            {
                if (productDs != null)
                {
                    foreach (var row in productDs.Tables[0].AsEnumerable())
                    {
                        row["is_selected"] = false;
                    }
                }
            }
            else if (quotationType == "S")
            {
                if (partListData != null)
                {
                    foreach (var row in partListData)
                    {
                        row.is_selected = false;
                    }
                }
            }
            else if (quotationType == "C" || quotationType == "D" || quotationType == "O")
            {

            }
            else if (quotationType == "M")
            {
                if (partListData != null)
                {
                    foreach (var row in partListData)
                    {
                        row.is_selected = false;
                    }
                }
            }
            else if (quotationType == "A")
            {
                if (productDs != null)
                {
                    foreach (var row in productDs.Tables[0].AsEnumerable())
                    {
                        row["is_selected"] = false;
                    }
                }
                if (annualHistoryData != null)
                {
                    foreach (var row in annualHistoryData)
                    {
                        row.is_selected = false;
                    }
                }
            }
            HttpContext.Current.Session["SESSION_PRODUCT_TO_SELECT_QUATATION"] = productDs;
            HttpContext.Current.Session["SESSION_MAINTENANCE_PARTLIST_QUATATION"] = partListData;
            HttpContext.Current.Session["SESSION_HISTORY_ANNUAL_SERVICE_ITEM_QUATATION"] = annualHistoryData;
            return "SUCCESS";
        }
        [WebMethod]
        public static List<QuotationDetail> moveUpProductDetail(string id)
        {
            List<QuotationDetail> data = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            if (data != null)
            {
                var row = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (row != null)
                {
                    if (row.sort_no != 1)
                    {
                        row.sort_no = row.sort_no - 1;
                    }
                    else
                    {
                        row.sort_no = 1;
                    }
                    var sortNoLower = (from t in data where t.sort_no == row.sort_no && t.id != Convert.ToInt32(id) select t).FirstOrDefault();
                    if (sortNoLower != null)
                    {
                        sortNoLower.sort_no = sortNoLower.sort_no + 1;
                    }
                }
            }
            HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"] = data;
            return data;
        }
        [WebMethod]
        public static List<QuotationDetail> moveDownProductDetail(string id)
        {
            List<QuotationDetail> data = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            if (data != null)
            {
                var row = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (row != null)
                {
                    if (row.sort_no != data.Count)
                    {
                        row.sort_no = row.sort_no + 1;
                    }
                    else
                    {
                        row.sort_no = data.Count;
                    }
                    var sortNoLower = (from t in data where t.sort_no == row.sort_no && t.id != Convert.ToInt32(id) select t).FirstOrDefault();
                    if (sortNoLower != null)
                    {
                        sortNoLower.sort_no = sortNoLower.sort_no - 1;
                    }
                }
            }
            HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"] = data;
            return data;
        }

        #region Quotation Annual Service
        //protected void gridViewAnnualServiceItem_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    gridViewAnnualServiceItem.DataSource = dtProductToSelect;
        //    gridViewAnnualServiceItem.DataBind();
        //}

        //protected void gridViewAnnualServiceItem_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        //{
        //    ASPxCheckBox checkBox = gridViewAnnualServiceItem.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkBox") as ASPxCheckBox;
        //    if (checkBox != null)
        //    {
        //        if (e.DataColumn.FieldName == "is_selected")
        //        {
        //            if (dtProductToSelect != null)
        //            {
        //                var row = (from t in dtProductToSelect.Tables[0].AsEnumerable() where t["id"].ToString() == e.KeyValue.ToString() select t).FirstOrDefault();
        //                if (row != null)
        //                {
        //                    checkBox.Checked = (row["is_selected"] == DBNull.Value ? false : Convert.ToBoolean(row["is_selected"]));
        //                }
        //            }
        //            //checkBox.Checked = (row["is_selected"]) == DBNull.Value ? false : true;
        //        }
        //    }
        //}
        protected void gridviewAnnualServiceHistory_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.ToString().Contains("Search"))
            {
                var splitStr = e.Parameters.ToString().Split('|');
                string searchText = string.Empty;
                string productType = string.Empty;

                if (splitStr.Length > 0)
                {
                    searchText = splitStr[2];
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = cbbCustomerID.Value },
                            new SqlParameter("@search_text", SqlDbType.VarChar,500) {Value = searchText }
                        };
                        conn.Open();
                        var dsAnnualServiceHistory = SqlHelper.ExecuteDataset(conn, "sp_annual_service_list_by_customer", arrParm.ToArray());
                        conn.Close();
                        if (dsAnnualServiceHistory != null)
                        {
                            if (historyAnnualServiceItemList != null)
                            {
                                historyAnnualServiceItemList.Clear();
                            }
                            foreach (var row in dsAnnualServiceHistory.Tables[0].AsEnumerable())
                            {
                                historyAnnualServiceItemList.Add(new AnnualServiceItem()
                                {
                                    is_selected = false,
                                    mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                    product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                    product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                    project = Convert.IsDBNull(row["project"]) ? string.Empty : Convert.ToString(row["project"]),
                                    unit_code = "unit",
                                });
                            }
                        }
                    }
                    if (historyAnnualServiceItemList.Count > 0)
                    {
                        foreach (var row in selectedProductList.Where(t => !t.is_deleted))
                        {
                            var existItem = (from t in historyAnnualServiceItemList where t.product_id == row.product_id select t).FirstOrDefault();
                            if (existItem != null)
                            {
                                existItem.is_selected = true;
                            }
                        }
                    }
                }
            }
            gridviewAnnualServiceHistory.DataSource = historyAnnualServiceItemList;
            gridviewAnnualServiceHistory.DataBind();
        }
        protected void gridviewAnnualServiceHistory_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            ASPxCheckBox checkBox = gridviewAnnualServiceHistory.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkBox") as ASPxCheckBox;
            if (checkBox != null)
            {
                if (e.DataColumn.FieldName == "is_selected")
                {
                    if (historyAnnualServiceItemList != null)
                    {
                        var row = (from t in historyAnnualServiceItemList where t.mfg_no == e.KeyValue select t).FirstOrDefault();
                        if (row != null)
                        {
                            checkBox.Checked = row.is_selected;
                        }
                    }
                    //checkBox.Checked = (row["is_selected"]) == DBNull.Value ? false : true;
                }
            }
        }

        [WebMethod]
        public static string GetAnnualServiceHistory(string customerId)
        {
            var data = new List<AnnualServiceItem>();

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = customerId },
                            new SqlParameter("@search_text", SqlDbType.VarChar,500) {Value = "" }
                        };
                conn.Open();
                var dsAnnualServiceHistory = SqlHelper.ExecuteDataset(conn, "sp_annual_service_list_by_customer", arrParm.ToArray());
                conn.Close();
                if (dsAnnualServiceHistory != null)
                {
                    foreach (var row in dsAnnualServiceHistory.Tables[0].AsEnumerable())
                    {
                        data.Add(new AnnualServiceItem()
                        {
                            is_selected = false,
                            mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                            product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                            product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                            project = Convert.IsDBNull(row["project"]) ? string.Empty : Convert.ToString(row["project"]),
                            unit_code = "unit",
                        });
                    }
                }
                HttpContext.Current.Session["SESSION_HISTORY_ANNUAL_SERVICE_ITEM_QUATATION"] = data;

            }
            return "SUCCESS";
        }

        [WebMethod]
        public static string PopupSelectAnnualService()
        {
            var data = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            var selectedData = (List<AnnualServiceItem>)HttpContext.Current.Session["SESSION_SELECTED_ANNUAL_SERVICE_ITEM_QUATATION"];
            var annualHistoryData = (List<AnnualServiceItem>)HttpContext.Current.Session["SESSION_HISTORY_ANNUAL_SERVICE_ITEM_QUATATION"];
            selectedData = new List<AnnualServiceItem>();
            foreach (var row in annualHistoryData)
            {
                if (data != null)
                {
                    var isSelect = data.Where(t => t.mfg_no == row.mfg_no && t.mfg_no == row.mfg_no).FirstOrDefault();
                    if (isSelect != null)
                    {
                        row.is_selected = true;
                        selectedData.Add(row);
                    }
                }
            }
            HttpContext.Current.Session["SESSION_SELECTED_ANNUAL_SERVICE_ITEM_QUATATION"] = selectedData;

            return "ANNUAL SERVICE";
        }

        [WebMethod]
        public static List<AnnualServiceItem> SelectAllAnnualService(bool isSelect)
        {
            var data = (List<AnnualServiceItem>)HttpContext.Current.Session["SESSION_SELECTED_ANNUAL_SERVICE_ITEM_QUATATION"];

            var annualHistoryData = (List<AnnualServiceItem>)HttpContext.Current.Session["SESSION_HISTORY_ANNUAL_SERVICE_ITEM_QUATATION"];
            var annualItemData = (DataSet)HttpContext.Current.Session["SESSION_PRODUCT_TO_SELECT_QUATATION"];

            foreach (var item in annualHistoryData)
            {
                if (data == null)
                {
                    data = new List<AnnualServiceItem>();
                }
                var checkExist = (from t in data
                                  where !string.IsNullOrEmpty(t.mfg_no) && t.mfg_no == (item.mfg_no)
                                  select t).FirstOrDefault();
                if (checkExist == null)
                {
                    if (isSelect)
                    {
                        data.Add(item);
                        item.is_selected = true;
                    }
                    else
                    {
                        data.Remove(item);
                        item.is_selected = false;
                    }
                }
                else
                {
                    if (isSelect)
                    {
                        //data.Add(item);
                        //item.is_selected = true;
                    }
                    else
                    {
                        data.Remove(item);
                        item.is_selected = false;
                    }
                }

            }
            HttpContext.Current.Session["SESSION_HISTORY_ANNUAL_SERVICE_ITEM_QUATATION"] = annualHistoryData;
            HttpContext.Current.Session["SESSION_PRODUCT_TO_SELECT_QUATATION"] = annualItemData;
            HttpContext.Current.Session["SESSION_SELECTED_ANNUAL_SERVICE_ITEM_QUATATION"] = data;

            return data;
        }

        [WebMethod]
        public static List<AnnualServiceItem> AddAnnualService(string id, string type)
        {
            var data = (List<AnnualServiceItem>)HttpContext.Current.Session["SESSION_SELECTED_ANNUAL_SERVICE_ITEM_QUATATION"];
            var annualHistoryData = (List<AnnualServiceItem>)HttpContext.Current.Session["SESSION_HISTORY_ANNUAL_SERVICE_ITEM_QUATATION"];
            var annualItemData = (DataSet)HttpContext.Current.Session["SESSION_PRODUCT_TO_SELECT_QUATATION"];
            if (type == "history")
            {
                var row = (from t in annualHistoryData
                           where t.mfg_no.Equals(id)
                           select t).FirstOrDefault();
                if (data == null)
                {
                    data = new List<AnnualServiceItem>();
                    data.Add(row);
                    row.is_selected = true;
                }
                else
                {
                    var checkExist = (from t in data
                                      where t.mfg_no.Equals(id) && !string.IsNullOrEmpty(t.mfg_no)
                                      select t).FirstOrDefault();
                    if (checkExist != null)
                    {
                        data.Remove(row);
                        row.is_selected = false;
                    }
                    else
                    {
                        data.Add(row);
                        row.is_selected = true;
                    }
                }
            }
            else if (type == "product")
            {
                var row = (from t in annualItemData.Tables[0].AsEnumerable()
                           where t["id"].ToString() == id
                           select t).FirstOrDefault();
                if (data == null)
                {
                    data = new List<AnnualServiceItem>();
                    data.Add(new AnnualServiceItem()
                    {
                        mfg_no = string.Empty,
                        product_id = Convert.ToInt32(row["id"]),
                        product_name_tha = Convert.ToString(row["product_name_tha"]),
                        product_no = Convert.ToString(row["product_no"]),
                        unit_code = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]),
                        qty = 1
                    });
                    row["is_selected"] = true;
                }
                else
                {
                    var checkExist = (from t in data
                                      where t.product_id == Convert.ToInt32(id) && string.IsNullOrEmpty(t.mfg_no)
                                      select t).FirstOrDefault();
                    if (checkExist != null)
                    {
                        data.Remove(checkExist);
                        row["is_selected"] = false;
                    }
                    else
                    {
                        data.Add(new AnnualServiceItem()
                        {
                            mfg_no = string.Empty,
                            product_id = Convert.ToInt32(row["id"]),
                            product_name_tha = Convert.ToString(row["product_name_tha"]),
                            product_no = Convert.ToString(row["product_no"]),
                            unit_code = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]),
                            qty = 1
                        });
                        row["is_selected"] = true;
                    }
                }

            }
            HttpContext.Current.Session["SESSION_HISTORY_ANNUAL_SERVICE_ITEM_QUATATION"] = annualHistoryData;
            HttpContext.Current.Session["SESSION_PRODUCT_TO_SELECT_QUATATION"] = annualItemData;
            HttpContext.Current.Session["SESSION_SELECTED_ANNUAL_SERVICE_ITEM_QUATATION"] = data;

            return data;
        }

        [WebMethod]
        public static List<QuotationDetail> SubmitAnnualService()
        {
            var selectedData = (List<AnnualServiceItem>)HttpContext.Current.Session["SESSION_SELECTED_ANNUAL_SERVICE_ITEM_QUATATION"];
            var productDetail = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            var annualHistoryData = (List<AnnualServiceItem>)HttpContext.Current.Session["SESSION_HISTORY_ANNUAL_SERVICE_ITEM_QUATATION"];
            var annualItemData = (DataSet)HttpContext.Current.Session["SESSION_PRODUCT_TO_SELECT_QUATATION"];
            if (selectedData != null)
            {
                //productDetail = new List<QuotationDetail>();
                foreach (var detailRow in selectedData)
                {
                    var existData = (from t in productDetail
                                     where t.mfg_no == detailRow.mfg_no
             && t.mfg_no == detailRow.mfg_no
                                     select t).FirstOrDefault();
                    if (existData != null)
                    {
                        //existData.qty += 1;
                    }
                    else
                    {
                        productDetail.Add(new QuotationDetail()
                        {
                            id = (productDetail.Count + 1) * -1,
                            package_id = 0,
                            product_no = detailRow.product_no,
                            product_id = detailRow.product_id,
                            unit_code = detailRow.unit_code,
                            quotation_description = detailRow.product_no,
                            qty = 1,//detailRow.quantity_package,
                            unit_price = 0,
                            min_unit_price = 0,
                            total_amount = 0,
                            mfg_no = detailRow.mfg_no,
                            is_history_issue_mfg = string.IsNullOrEmpty(detailRow.mfg_no) ? false : true,
                            sort_no = (productDetail.Count == 0) ? 1 : (from t in productDetail select t.sort_no).Max() + 1,
                            product_type = "P"
                        });
                    }
                }
                var delItem = new List<QuotationDetail>();
                delItem.AddRange(productDetail);
                foreach (var row in productDetail)
                {
                    var checkDelete = selectedData.Where(t => t.mfg_no == row.mfg_no && t.mfg_no == row.mfg_no).FirstOrDefault();
                    if (checkDelete == null)
                    {
                        delItem.Remove(row);
                    }
                }
                productDetail = delItem;
            }

            HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"] = productDetail;
            HttpContext.Current.Session["SESSION_SELECTED_ANNUAL_SERVICE_ITEM_QUATATION"] = null;

            return productDetail;
        }
        #endregion

        protected void cbMFGNo_Callback(object sender, CallbackEventArgsBase e)
        {
            cbMFGNo.DataSource = dtMFGProduct;
            cbMFGNo.DataBind();
        }

        protected void CloneQuotation(int id)
        {
            try
            {
                if (dataId == 0 && id > 0)
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },
                        };
                        conn.Open();
                        dsQuataionData = SqlHelper.ExecuteDataset(conn, "sp_quotation_list_data_qu", arrParm.ToArray());
                        ViewState["dsQuataionData"] = dsQuataionData;
                        conn.Close();
                    }
                    if (dsQuataionData.Tables.Count > 0)
                    {
                        #region Quotation Header
                        // Quotation Header
                        var data = dsQuataionData.Tables[0].AsEnumerable().FirstOrDefault();
                        if (data != null)
                        {
                            txtAddress.Value = Convert.IsDBNull(data["address_bill_tha"]) ? string.Empty : Convert.ToString(data["address_bill_tha"]);
                            //txtAttention.Value = Convert.IsDBNull(data["attention_name"]) ? string.Empty : Convert.ToString(data["attention_name"]);

                            txtGrandTotal.Value = Convert.IsDBNull(data["grand_total"]) ? string.Empty : String.Format("{0:n}", Convert.ToDouble(data["grand_total"]));
                            txtProject.Value = Convert.IsDBNull(data["project_name"]) ? string.Empty : Convert.ToString(data["project_name"]);
                            txtQuatationDate.Value = DateTime.Now.ToString("dd/MM/yyyy");
                            txtCloneQuotation.Value = Convert.IsDBNull(data["quotation_no"]) ? string.Empty : Convert.ToString(data["quotation_no"]);
                            cbbSubject.Text = Convert.IsDBNull(data["quotation_subject"]) ? string.Empty : Convert.ToString(data["quotation_subject"]);

                            cbbCustomerID.Value = Convert.ToString(data["customer_id"]);
                            cbbQuotationType.Value = Convert.ToString(data["quotation_type"]);
                            Cbbcurrency.Value = Convert.ToString(data["currency"]);
                            cbDiscountByItem.Checked = Convert.IsDBNull(data["is_discount_by_item"]) ? false : Convert.ToBoolean(data["is_discount_by_item"]);
                            cbDiscountBottomBill1.Checked = string.IsNullOrEmpty(Convert.ToString(data["discount1_type"])) ? false :
                                                                                                (Convert.ToString(data["discount1_type"]) == "N" ? false : true);
                            cbDiscountBottomBill2.Checked = string.IsNullOrEmpty(Convert.ToString(data["discount2_type"])) ? false :
                                                                                                (Convert.ToString(data["discount2_type"]) == "N" ? false : true);

                            cbbDiscountBottomBill1.Value = Convert.IsDBNull(data["discount1_type"]) ? string.Empty : Convert.ToString(data["discount1_type"]);
                            cbbDiscountBottomBill2.Value = Convert.IsDBNull(data["discount2_type"]) ? string.Empty : Convert.ToString(data["discount2_type"]);

                            txtTotal.Value = Convert.IsDBNull(data["total_amount"]) ? string.Empty : String.Format("{0:n}", Convert.ToDouble(data["total_amount"]));

                            rdoLumpsum.Checked = Convert.IsDBNull(data["is_lump_sum"]) ? false : Convert.ToBoolean(data["is_lump_sum"]);
                            rdoNotLumpsum.Checked = Convert.IsDBNull(data["is_lump_sum"]) ? false : !Convert.ToBoolean(data["is_lump_sum"]);

                            cbContactor.Checked = Convert.IsDBNull(data["is_contactor"]) ? false : Convert.ToBoolean(data["is_contactor"]);
                            cbShowVat.Checked = Convert.IsDBNull(data["is_vat"]) ? false : Convert.ToBoolean(data["is_vat"]);
                            rdoDescription.Checked = Convert.IsDBNull(data["is_product_description"]) ? false : !Convert.ToBoolean(data["is_product_description"]);
                            rdoQuotationLine.Checked = Convert.IsDBNull(data["is_product_description"]) ? false : Convert.ToBoolean(data["is_product_description"]);
                            txtContractNo.Value = Convert.IsDBNull(data["contract_no"]) ? string.Empty : Convert.ToString(data["contract_no"]);
                            txtAttentionTel.Value = Convert.IsDBNull(data["attention_tel"]) ? string.Empty : Convert.ToString(data["attention_tel"]);
                            txtAttentionEmail.Value = Convert.IsDBNull(data["attention_email"]) ? string.Empty : Convert.ToString(data["attention_email"]);
                            cbIsNet.Checked = Convert.IsDBNull(data["is_net"]) ? false : Convert.ToBoolean(data["is_net"]);
                            txtHour.Value = Convert.IsDBNull(data["hour_amount"]) ? string.Empty : Convert.ToString(data["hour_amount"]);
                            txtPressure.Value = Convert.IsDBNull(data["pressure"]) ? string.Empty : Convert.ToString(data["pressure"]);
                            txtMachine.Value = Convert.IsDBNull(data["machine"]) ? string.Empty : Convert.ToString(data["machine"]);
                            txtShowMFG.Value = Convert.IsDBNull(data["mfg"]) ? string.Empty : Convert.ToString(data["mfg"]);
                            //cbbModel.Value = Convert.IsDBNull(data["model"]) ? string.Empty : Convert.ToString(data["model"]);
                            txtLocation1.Value = Convert.IsDBNull(data["location1"]) ? string.Empty : Convert.ToString(data["location1"]);
                            txtLocation2.Value = Convert.IsDBNull(data["location2"]) ? string.Empty : Convert.ToString(data["location2"]);

                            //cbbEmployee.Value = Convert.IsDBNull(data["sales_id"]) ? string.Empty : Convert.ToString(data["sales_id"]);
                            txtRevision.Value = "0";//Convert.IsDBNull(data["revision"]) ? "0" : Convert.ToString(Convert.ToInt32(data["revision"]) + 1);

                            if (Convert.ToString(data["discount1_type"]) == "P")
                            {
                                txtDiscountBottomBill1.Value = Convert.ToString(data["discount1_percentage"]);
                            }
                            else if (Convert.ToString(data["discount1_type"]) == "A")
                            {
                                txtDiscountBottomBill1.Value = Convert.ToString(data["discount1_amount"]);
                            }
                            if (Convert.ToString(data["discount2_type"]) == "P")
                            {
                                txtDiscountBottomBill2.Value = Convert.ToString(data["discount2_percentage"]);
                            }
                            else if (Convert.ToString(data["discount2_type"]) == "A")
                            {
                                txtDiscountBottomBill2.Value = Convert.ToString(data["discount2_amount"]);
                            }
                            txtSumDiscount1.Value = Convert.IsDBNull(data["discount1_total"]) ? "0" : String.Format("{0:n}", Convert.ToDouble(data["discount1_total"]));
                            txtSumDiscount2.Value = Convert.IsDBNull(data["discount2_total"]) ? "0" : String.Format("{0:n}", Convert.ToDouble(data["discount2_total"]));

                            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                            {
                                //Create array of Parameters
                                List<SqlParameter> arrParm = new List<SqlParameter>
                                {
                                    new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                                    new SqlParameter("@customer_id", SqlDbType.Int) { Value = cbbCustomerID.Value }, //4
                                };
                                conn.Open();
                                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_attention_list", arrParm.ToArray());

                                conn.Close();
                                if (dsResult.Tables.Count > 0)
                                {
                                    if (dsResult.Tables[0].Rows.Count > 0)
                                    {
                                        cbbCustomerAttention.DataSource = dsResult;
                                        cbbCustomerAttention.TextField = "attention_name";
                                        cbbCustomerAttention.ValueField = "id";
                                        cbbCustomerAttention.DataBind();
                                    }
                                }
                            }

                            cbbCustomerAttention.Value = Convert.IsDBNull(data["customer_attention_id"]) ? "0" : Convert.ToString(data["customer_attention_id"]);

                            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                            {
                                //Create array of Parameters
                                List<SqlParameter> arrParm = new List<SqlParameter>
                                {
                                    new SqlParameter("@customer_id", SqlDbType.Int) { Value = cbbCustomerID.Value },
                                };
                                conn.Open();
                                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_mfg_list_quotation", arrParm.ToArray());

                                conn.Close();
                                if (dsResult.Tables.Count > 0)
                                {
                                    if (dsResult.Tables[0].Rows.Count > 0)
                                    {
                                        cbbModel.DataSource = dsResult;
                                        cbbModel.TextField = "model";
                                        cbbModel.ValueField = "id";
                                        cbbModel.DataBind();
                                    }
                                }
                            }

                            //cbbModel.Value = Convert.IsDBNull(data["id"]) ? "0" : Convert.ToString(data["id"]);
                            cbbModel.Text = Convert.IsDBNull(data["model"]) ? "" : Convert.ToString(data["model"]) == "0" ? "" : Convert.ToString(data["model"]);
                            cbbModel.Value = Convert.IsDBNull(data["model"]) ? "" : Convert.ToString(data["model"]) == "0" ? "" : Convert.ToString(data["model"]);
                            hdModelName.Value = cbbModel.Text;

                            cbbModel.Text += ("; " + txtShowMFG.Value);
                            cbbModel.Value = Convert.IsDBNull(data["model_id"]) ? string.Empty : Convert.ToString(data["model_id"]) == "0" ? "" : Convert.ToString(data["model_id"]);

                            //txtSumDiscount1.Value = Convert.ToString(data["address_bill_tha"]);
                            // txtSumDiscount2.Value = Convert.ToString(data["address_bill_tha"]);


                            //hdSumDiscountDetail.Value = Convert.ToString(data["address_bill_tha"]);
                            //hdTotal.Value = Convert.ToString(data["address_bill_tha"]);
                            //hdTotalWithDiscountDetail.Value = Convert.ToString(data["address_bill_tha"]);
                        }
                        #endregion Quotation Header

                        #region Bind Grid When Load Header
                        if (cbbQuotationType.Value == "D") // Service Change
                        {

                            contactor.Attributes["style"] = "display:none";
                            dvGridProduct.Attributes["style"] = "display:none";
                            dvGridService.Attributes["style"] = "display:''";
                            dvIsService.Attributes["style"] = "display:''";
                            dvIsProductDescription.Attributes["style"] = "display:none";
                            gridQuotationDetail.Columns[4].Visible = true;
                            gridQuotationDetail.Columns[5].Visible = false;
                            gridQuotationDetail.Columns[6].Visible = false;
                            dvIsAnnualService.Attributes["style"] = "display:none";
                            tabOtherDetail.Attributes["style"] = "display:none";

                        }
                        else if (cbbQuotationType.Value == "C") // Service Change
                        {

                            contactor.Attributes["style"] = "display:none";
                            dvGridProduct.Attributes["style"] = "display:none";
                            dvGridService.Attributes["style"] = "display:''";
                            dvIsService.Attributes["style"] = "display:''";
                            dvIsProductDescription.Attributes["style"] = "display:none";
                            gridQuotationDetail.Columns[4].Visible = true;
                            gridQuotationDetail.Columns[5].Visible = false;
                            gridQuotationDetail.Columns[6].Visible = false;
                            dvIsAnnualService.Attributes["style"] = "display:none";
                            tabOtherDetail.Attributes["style"] = "display:''";

                        }
                        else if (cbbQuotationType.Value == "O") // Other
                        {
                            dvGridProduct.Attributes["style"] = "display:none";
                            dvGridService.Attributes["style"] = "display:''";
                            dvIsService.Attributes["style"] = "display:''";
                            dvIsProductDescription.Attributes["style"] = "display:none";
                            gridQuotationDetail.Columns[4].Visible = true;
                            gridQuotationDetail.Columns[5].Visible = false;
                            gridQuotationDetail.Columns[6].Visible = false;
                            dvIsAnnualService.Attributes["style"] = "display:none";
                            tabOtherDetail.Attributes["style"] = "display:none";

                        }

                        else if (cbbQuotationType.Value == "P") // Product
                        {

                            dvGridProduct.Attributes["style"] = "display:''";
                            dvGridService.Attributes["style"] = "display:none";
                            dvIsService.Attributes["style"] = "display:none";
                            dvIsAnnualService.Attributes["style"] = "display:none";
                            dvIsProductDescription.Attributes["style"] = "display:none";
                            if (rdoDescription.Checked)
                            {
                                gridQuotationDetail.Columns[4].Visible = true;
                                gridQuotationDetail.Columns[5].Visible = false;
                            }
                            else
                            {
                                gridQuotationDetail.Columns[4].Visible = false;
                                gridQuotationDetail.Columns[5].Visible = true;
                            }
                            gridQuotationDetail.Columns[6].Visible = false;
                            tabOtherDetail.Attributes["style"] = "display:none";
                        }
                        else if (cbbQuotationType.Value == "S") // Spare Part
                        {
                            contactor.Attributes["style"] = "display:none";
                            dvGridProduct.Attributes["style"] = "display:''";
                            dvIsAnnualService.Attributes["style"] = "display:none";
                            dvGridService.Attributes["style"] = "display:none";
                            dvIsService.Attributes["style"] = "display:none";
                            dvIsProductDescription.Attributes["style"] = "display:none";
                            gridQuotationDetail.Columns[4].Visible = true;
                            gridQuotationDetail.Columns[5].Visible = false;
                            gridQuotationDetail.Columns[6].Visible = false;
                        }
                        else if (cbbQuotationType.Value == "M") // Maintenance
                        {
                            contactor.Attributes["style"] = "display:none";
                            dvGridProduct.Attributes["style"] = "display:''";
                            dvIsAnnualService.Attributes["style"] = "display:none";
                            dvGridService.Attributes["style"] = "display:none";
                            dvIsService.Attributes["style"] = "display:none";
                            dvIsProductDescription.Attributes["style"] = "display:none";
                            gridQuotationDetail.Columns[4].Visible = true;
                            gridQuotationDetail.Columns[5].Visible = false;
                            gridQuotationDetail.Columns[6].Visible = false;
                            btnAddPartList.Visible = true;
                            btnAddAllPartList.Visible = true;
                            //dvIsService.Style.Add("display", "none");
                        }
                        else if (cbbQuotationType.Value == "A") // Annual Service
                        {

                            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                            {
                                //Create array of Parameters
                                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = cbbCustomerID.Value },
                            new SqlParameter("@search_text", SqlDbType.VarChar,500) {Value = "" }
                        };
                                conn.Open();
                                var dsAnnualServiceHistory = SqlHelper.ExecuteDataset(conn, "sp_annual_service_list_by_customer", arrParm.ToArray());
                                conn.Close();
                                if (dsAnnualServiceHistory != null)
                                {
                                    foreach (var row in dsAnnualServiceHistory.Tables[0].AsEnumerable())
                                    {
                                        historyAnnualServiceItemList.Add(new AnnualServiceItem()
                                        {
                                            is_selected = false,
                                            mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                            product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                            product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                            project = Convert.IsDBNull(row["project"]) ? string.Empty : Convert.ToString(row["project"]),
                                            unit_code = "unit",
                                        });
                                    }
                                }


                            }
                            contactor.Attributes["style"] = "display:none";
                            dvIsAnnualService.Attributes["style"] = "display:''";
                            dvGridProduct.Attributes["style"] = "display:''";
                            dvGridService.Attributes["style"] = "display:none";
                            dvIsService.Attributes["style"] = "display:none";
                            dvIsAnnualService_Discount.Attributes["style"] = "display:''";
                            dvIsProductDescription.Attributes["style"] = "display:none";
                            gridQuotationDetail.Columns[4].Visible = false;
                            gridQuotationDetail.Columns[5].Visible = false;
                            gridQuotationDetail.Columns[6].Visible = true;
                            tabOtherDetail.Attributes["style"] = "display:''";

                            //dvIsService.Style.Add("display", "none");
                        }
                        #endregion

                        #region Quotation Detail 
                        // Quotation Detail List
                        string modelName = "";
                        string runningHours = "";
                        foreach (var row in dsQuataionData.Tables[1].AsEnumerable())
                        {
                            var dataParent_id = Convert.ToInt32(row["parent_id"]);
                            var idOrg = Convert.ToInt32(row["id"]);
                            var newId = (quotationDetailList.Count + 1) * -1;

                            var qd = (from t in quotationDetailList where t.id_org == dataParent_id select t).FirstOrDefault();
                            if (qd != null)
                            {
                                dataParent_id = qd.id;

                            }
                            quotationDetailList.Add(new QuotationDetail()
                            {
                                //delivery_note_no = Convert.ToString(row["delivery_note_no"]),
                                id_org = Convert.ToInt32(row["id"]),
                                parant_id_org = Convert.ToInt32(row["parent_id"]),
                                discount_amount = Convert.IsDBNull(row["discount_amount"]) ? 0 : Convert.ToDecimal(row["discount_amount"]),
                                discount_percentage = Convert.IsDBNull(row["discount_percentage"]) ? 0 : Convert.ToDecimal(row["discount_percentage"]),
                                discount_type = Convert.IsDBNull(row["discount_type"]) ? string.Empty : Convert.ToString(row["discount_type"]),
                                id = newId,
                                parant_id = dataParent_id,
                                min_unit_price = Convert.IsDBNull(row["min_unit_price"]) ? 0.0m : Convert.ToDecimal(row["min_unit_price"]),
                                product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                product_type = Convert.IsDBNull(row["product_type"]) ? string.Empty : Convert.ToString(row["product_type"]),
                                qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["qty"]),
                                quotation_description = Convert.IsDBNull(row["quotation_description"]) ? string.Empty : Convert.ToString(row["quotation_description"]),
                                quotation_no = Convert.IsDBNull(row["quotation_no"]) ? string.Empty : Convert.ToString(row["quotation_no"]),
                                total_amount = Convert.IsDBNull(row["total_amount"]) ? 0.0m : Convert.ToDecimal(row["total_amount"]),
                                unit_code = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]),
                                unit_price = Convert.IsDBNull(row["unit_price"]) ? 0.0m : Convert.ToDecimal(row["unit_price"]),
                                is_deleted = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                sort_no = Convert.IsDBNull(row["sort_no"]) ? 1 : Convert.ToInt32(row["sort_no"]),

                                quotation_line = Convert.IsDBNull(row["quotation_line"]) ? string.Empty : Convert.ToString(row["quotation_line"]),
                                mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                is_history_issue_mfg = Convert.IsDBNull(row["is_history_issue_mfg"]) ? false : Convert.ToBoolean(row["is_history_issue_mfg"]),
                                quotation_line_1 = Convert.IsDBNull(row["quotation_line_1"]) ? string.Empty : Convert.ToString(row["quotation_line_1"]),
                                quotation_line_2 = Convert.IsDBNull(row["quotation_line_2"]) ? string.Empty : Convert.ToString(row["quotation_line_2"]),
                                quotation_line_3 = Convert.IsDBNull(row["quotation_line_3"]) ? string.Empty : Convert.ToString(row["quotation_line_3"]),
                                quotation_line_4 = Convert.IsDBNull(row["quotation_line_4"]) ? string.Empty : Convert.ToString(row["quotation_line_4"]),
                                quotation_line_5 = Convert.IsDBNull(row["quotation_line_5"]) ? string.Empty : Convert.ToString(row["quotation_line_5"]),
                                quotation_line_6 = Convert.IsDBNull(row["quotation_line_6"]) ? string.Empty : Convert.ToString(row["quotation_line_6"]),
                                quotation_line_7 = Convert.IsDBNull(row["quotation_line_7"]) ? string.Empty : Convert.ToString(row["quotation_line_7"]),
                                quotation_line_8 = Convert.IsDBNull(row["quotation_line_8"]) ? string.Empty : Convert.ToString(row["quotation_line_8"]),
                                quotation_line_9 = Convert.IsDBNull(row["quotation_line_9"]) ? string.Empty : Convert.ToString(row["quotation_line_9"]),
                                quotation_line_10 = Convert.IsDBNull(row["quotation_line_10"]) ? string.Empty : Convert.ToString(row["quotation_line_10"]),

                                package_id = Convert.IsDBNull(row["package_id"]) ? 0 : Convert.ToInt32(row["package_id"]),
                                running_hours = Convert.IsDBNull(row["running_hours"]) ? "" : Convert.ToString(row["running_hours"]),
                                model_id = Convert.IsDBNull(row["model_id"]) ? 0 : Convert.ToInt32(row["model_id"]),
                                model_name = Convert.IsDBNull(row["model_name"]) ? "" : Convert.ToString(row["model_name"]),

                            });
                            var aRow = quotationDetailList[quotationDetailList.Count - 1];
                            if (aRow.discount_type.Equals("P"))
                            {
                                aRow.discount_total = ((aRow.qty * aRow.unit_price) * aRow.discount_percentage) / 100;
                            } else
                            {
                                aRow.discount_total = ((aRow.qty * aRow.unit_price) - aRow.discount_amount);
                            }

                            if (quotationDetailList[quotationDetailList.Count - 1].model_name != "" && modelName == "")
                            {
                                modelName = quotationDetailList[quotationDetailList.Count - 1].model_name;
                            }
                            if (quotationDetailList[quotationDetailList.Count - 1].running_hours != "" && runningHours == "")
                            {
                                runningHours = quotationDetailList[quotationDetailList.Count - 1].running_hours;
                            }
                        }

                        if (cbbQuotationType.Value == "M")
                        {
                            lblModelPackage.Text = "Model : " + modelName + ", Running hour : " + runningHours;
                        }

                        var dataDetail = quotationDetailList;
                        if (dataDetail.Count > 0)
                        {
                            var list = (from t in dataDetail select t).FirstOrDefault();
                            hdMaintenanceModelId.Value = Convert.ToString(list.model_id);
                            hdPackageId.Value = Convert.ToString(list.package_id);
                            if (list.discount_type == "P")
                            {
                                if (cbbQuotationType.Value == "P" || cbbQuotationType.Value == "S" || cbbQuotationType.Value == "M" || cbbQuotationType.Value == "A")

                                {
                                    gridQuotationDetail.Columns[9].Visible = true;
                                    gridQuotationDetail.Columns[10].Visible = false;
                                    gridQuotationDetail.Columns[11].Visible = true;
                                }
                                else
                                {
                                    gridViewService.Columns[8].Visible = false;
                                    gridViewService.Columns[9].Visible = true;
                                    gridViewService.Columns[10].Visible = true;

                                }

                                txtDiscountByItem.Value = Convert.ToString(list.discount_percentage);

                            }
                            else if (list.discount_type == "A")
                            {
                                if (cbbQuotationType.Value == "P" || cbbQuotationType.Value == "S" || cbbQuotationType.Value == "M" || cbbQuotationType.Value == "A")

                                {
                                    gridQuotationDetail.Columns[9].Visible = true;
                                    gridQuotationDetail.Columns[10].Visible = true;
                                    gridQuotationDetail.Columns[11].Visible = false;

                                }
                                else
                                {
                                    gridViewService.Columns[8].Visible = true;
                                    gridViewService.Columns[9].Visible = false;
                                    gridViewService.Columns[10].Visible = true;
                                }
                                txtDiscountByItem.Value = Convert.ToString(list.discount_amount);
                            }
                            else
                            {
                                if (cbbQuotationType.Value == "P" || cbbQuotationType.Value == "S" || cbbQuotationType.Value == "M" || cbbQuotationType.Value == "A")

                                {

                                    gridQuotationDetail.Columns[10].Visible = false;
                                    gridQuotationDetail.Columns[11].Visible = false;

                                }
                                else
                                {

                                    gridViewService.Columns[8].Visible = false;
                                    gridViewService.Columns[9].Visible = false;
                                }
                            }


                        }

                        #endregion Quotation Detail

                        #region Quotation Remark
                        // Qutation Remark
                        foreach (var row in dsQuataionData.Tables[2].AsEnumerable())
                        {
                            if (Convert.ToInt32(row["sort_no"]) == 1)
                            {
                                txtRemark1.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 2)
                            {
                                txtRemark2.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 3)
                            {
                                txtRemark3.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 4)
                            {
                                txtRemark4.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 5)
                            {
                                txtRemark5.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 6)
                            {
                                txtRemark6.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 7)
                            {
                                txtRemark7.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 8)
                            {
                                txtRemark8.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 9)
                            {
                                txtRemark9.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 10)
                            {
                                txtRemark9.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) ==11)
                            {
                                txtRemark9.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 12)
                            {
                                txtRemarkOther.Value = Convert.ToString(row["remark"]);
                            }
                        }
                        #endregion Quotation Remark

                        #region Quotation Notification
                        // Quotation Notification
                        foreach (var row in dsQuataionData.Tables[3].AsEnumerable())
                        {
                            notificationList.Add(new Notification()
                            {
                                description = Convert.ToString(row["description"]),
                                display_notice_date = Convert.ToDateTime(row["notice_date"]).ToString("dd/MM/yyyy HH:mm"),//Convert.ToString(row["display_notice_date"]),
                                id = (notificationList.Count + 1) * -1,
                                is_deleted = Convert.ToBoolean(row["is_delete"]),
                                notice_date = Convert.ToDateTime(row["notice_date"]),
                                notice_type = Convert.ToString(row["description"]),
                                reference_id = Convert.ToInt32(row["reference_id"]),
                                reference_no = Convert.ToString(row["reference_no"]),
                                subject = Convert.ToString(row["subject"]),
                                topic = Convert.ToString(row["topic"])
                            });
                        }
                        #endregion Quotation Notification
                    }

                    // Bind Summary Textbox

                    if (!cbDiscountByItem.Checked)
                    {
                        cbbDiscountByItem.Attributes["disabled"] = "disabled";
                    }
                    if (cbDiscountBottomBill1.Checked)
                    {
                        txtDiscountBottomBill1.Attributes.Remove("disabled");
                        cbbDiscountBottomBill1.Attributes.Remove("disabled");

                        cbDiscountBottomBill2.Attributes.Remove("disabled");
                    }
                    else
                    {
                        txtDiscountBottomBill1.Attributes["disabled"] = "disabled";
                        cbbDiscountBottomBill1.Attributes["disabled"] = "disabled";
                    }
                    if (cbDiscountBottomBill2.Checked)
                    {
                        cbDiscountBottomBill2.Attributes.Remove("disabled");
                        txtDiscountBottomBill2.Attributes.Remove("disabled");
                        cbbDiscountBottomBill2.Attributes.Remove("disabled");
                    }
                    else
                    {
                        txtDiscountBottomBill2.Attributes["disabled"] = "disabled";
                        cbbDiscountBottomBill2.Attributes["disabled"] = "disabled";
                    }
                    if (quotationDetailList.Count > 0)
                    {
                        cbbDiscountByItem.Value = quotationDetailList[0].discount_type;
                        txtDiscountByItem.Value = (quotationDetailList[0].discount_type.Equals("P") ? quotationDetailList[0].discount_percentage.ToString() : quotationDetailList[0].discount_amount.ToString());
                        cbbDiscountByItem.Disabled = false;
                    }


                    //cbbQuotationType.Style["disabled"] = "disabled";
                    cbbQuotationType.Attributes["disabled"] = "disabled";
                    cbbCustomerID.Attributes["disabled"] = "disabled";
                    rdoLumpsum.Attributes["disabled"] = "disabled";
                    rdoNotLumpsum.Attributes["disabled"] = "disabled";
                    BindGridQuotationDetail(false);
                    BindGridFreeBies(false);
                    BindGridNotification(false);
                    //BindGridAnnualServiceItem();
                    BindGridHistoryAnnualService();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void RevisionQuotation(int id)
        {
            try
            {
                if (dataId == 0 && id > 0)
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },
                        };
                        conn.Open();
                        dsQuataionData = SqlHelper.ExecuteDataset(conn, "sp_quotation_list_data_qu", arrParm.ToArray());
                        ViewState["dsQuataionData"] = dsQuataionData;
                        conn.Close();
                    }
                    if (dsQuataionData.Tables.Count > 0)
                    {
                        #region Quotation Header
                        // Quotation Header
                        var data = dsQuataionData.Tables[0].AsEnumerable().FirstOrDefault();
                        if (data != null)
                        {

                            txtAddress.Value = Convert.IsDBNull(data["address_bill_tha"]) ? string.Empty : Convert.ToString(data["address_bill_tha"]);
                            //txtAttention.Value = Convert.IsDBNull(data["attention_name"]) ? string.Empty : Convert.ToString(data["attention_name"]);

                            txtGrandTotal.Value = Convert.IsDBNull(data["grand_total"]) ? string.Empty : String.Format("{0:n}", Convert.ToDouble(data["grand_total"]));
                            txtProject.Value = Convert.IsDBNull(data["project_name"]) ? string.Empty : Convert.ToString(data["project_name"]);
                            txtQuatationDate.Value = DateTime.Now.ToString("dd/MM/yyyy");
                            txtCloneQuotation.Value = Convert.IsDBNull(data["quotation_no"]) ? string.Empty : Convert.ToString(data["quotation_no"]);
                            cbbSubject.Text = Convert.IsDBNull(data["quotation_subject"]) ? string.Empty : Convert.ToString(data["quotation_subject"]);

                            cbbCustomerID.Items.Clear();
                            cbbCustomerID.Items.Add(new ListEditItem(Convert.ToString(data["customer_name"]), Convert.ToString(data["customer_id"])));
                            cbbCustomerID.Value = Convert.ToString(data["customer_id"]);
                            cbbQuotationType.Value = Convert.ToString(data["quotation_type"]);
                            Cbbcurrency.Value = Convert.ToString(data["currency"]);
                            cbDiscountByItem.Checked = Convert.IsDBNull(data["is_discount_by_item"]) ? false : Convert.ToBoolean(data["is_discount_by_item"]);
                            cbDiscountBottomBill1.Checked = string.IsNullOrEmpty(Convert.ToString(data["discount1_type"])) ? false :
                                                                                                (Convert.ToString(data["discount1_type"]) == "N" ? false : true);
                            cbDiscountBottomBill2.Checked = string.IsNullOrEmpty(Convert.ToString(data["discount2_type"])) ? false :
                                                                                                (Convert.ToString(data["discount2_type"]) == "N" ? false : true);

                            cbbDiscountBottomBill1.Value = Convert.IsDBNull(data["discount1_type"]) ? string.Empty : Convert.ToString(data["discount1_type"]);
                            cbbDiscountBottomBill2.Value = Convert.IsDBNull(data["discount2_type"]) ? string.Empty : Convert.ToString(data["discount2_type"]);

                            txtTotal.Value = Convert.IsDBNull(data["total_amount"]) ? string.Empty : String.Format("{0:n}", Convert.ToDouble(data["total_amount"]));

                            rdoLumpsum.Checked = Convert.IsDBNull(data["is_lump_sum"]) ? false : Convert.ToBoolean(data["is_lump_sum"]);
                            rdoNotLumpsum.Checked = Convert.IsDBNull(data["is_lump_sum"]) ? false : !Convert.ToBoolean(data["is_lump_sum"]);

                            cbContactor.Checked = Convert.IsDBNull(data["is_contactor"]) ? false : Convert.ToBoolean(data["is_contactor"]);
                            cbShowVat.Checked = Convert.IsDBNull(data["is_vat"]) ? false : Convert.ToBoolean(data["is_vat"]);
                            rdoDescription.Checked = Convert.IsDBNull(data["is_product_description"]) ? false : !Convert.ToBoolean(data["is_product_description"]);
                            rdoQuotationLine.Checked = Convert.IsDBNull(data["is_product_description"]) ? false : Convert.ToBoolean(data["is_product_description"]);
                            txtContractNo.Value = Convert.IsDBNull(data["contract_no"]) ? string.Empty : Convert.ToString(data["contract_no"]);
                            txtAttentionTel.Value = Convert.IsDBNull(data["attention_tel"]) ? string.Empty : Convert.ToString(data["attention_tel"]);
                            txtAttentionEmail.Value = Convert.IsDBNull(data["attention_email"]) ? string.Empty : Convert.ToString(data["attention_email"]);
                            cbIsNet.Checked = Convert.IsDBNull(data["is_net"]) ? false : Convert.ToBoolean(data["is_net"]);
                            txtHour.Value = Convert.IsDBNull(data["hour_amount"]) ? string.Empty : Convert.ToString(data["hour_amount"]);
                            txtPressure.Value = Convert.IsDBNull(data["pressure"]) ? string.Empty : Convert.ToString(data["pressure"]);
                            txtMachine.Value = Convert.IsDBNull(data["machine"]) ? string.Empty : Convert.ToString(data["machine"]);
                            txtShowMFG.Value = Convert.IsDBNull(data["mfg"]) ? string.Empty : Convert.ToString(data["mfg"]);
                            txtLocation1.Value = Convert.IsDBNull(data["location1"]) ? string.Empty : Convert.ToString(data["location1"]);
                            txtLocation2.Value = Convert.IsDBNull(data["location2"]) ? string.Empty : Convert.ToString(data["location2"]);

                            //cbbEmployee.Value = Convert.IsDBNull(data["sales_id"]) ? string.Empty : Convert.ToString(data["sales_id"]);
                            txtRevision.Value = "0";//Convert.IsDBNull(data["revision"]) ? "0" : Convert.ToString(Convert.ToInt32(data["revision"]) + 1);

                            if (Convert.ToString(data["discount1_type"]) == "P")
                            {
                                txtDiscountBottomBill1.Value = Convert.ToString(data["discount1_percentage"]);
                            }
                            else if (Convert.ToString(data["discount1_type"]) == "A")
                            {
                                txtDiscountBottomBill1.Value = Convert.ToString(data["discount1_amount"]);
                            }
                            if (Convert.ToString(data["discount2_type"]) == "P")
                            {
                                txtDiscountBottomBill2.Value = Convert.ToString(data["discount2_percentage"]);
                            }
                            else if (Convert.ToString(data["discount2_type"]) == "A")
                            {
                                txtDiscountBottomBill2.Value = Convert.ToString(data["discount2_amount"]);
                            }
                            txtSumDiscount1.Value = Convert.IsDBNull(data["discount1_total"]) ? "0" : String.Format("{0:n}", Convert.ToDouble(data["discount1_total"]));
                            txtSumDiscount2.Value = Convert.IsDBNull(data["discount2_total"]) ? "0" : String.Format("{0:n}", Convert.ToDouble(data["discount2_total"]));

                            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                            {
                                //Create array of Parameters
                                List<SqlParameter> arrParm = new List<SqlParameter>
                                {
                                    new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                                    new SqlParameter("@customer_id", SqlDbType.Int) { Value = cbbCustomerID.Value }, //4
                                };
                                conn.Open();
                                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_attention_list", arrParm.ToArray());

                                conn.Close();
                                if (dsResult.Tables.Count > 0)
                                {
                                    if (dsResult.Tables[0].Rows.Count > 0)
                                    {
                                        cbbCustomerAttention.DataSource = dsResult;
                                        cbbCustomerAttention.TextField = "attention_name";
                                        cbbCustomerAttention.ValueField = "id";
                                        cbbCustomerAttention.DataBind();
                                    }
                                }
                            }
                            cbbCustomerAttention.Value = Convert.IsDBNull(data["customer_attention_id"]) ? "0" : Convert.ToString(data["customer_attention_id"]);
                            //txtSumDiscount1.Value = Convert.ToString(data["address_bill_tha"]);
                            // txtSumDiscount2.Value = Convert.ToString(data["address_bill_tha"]);
                            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                            {
                                //Create array of Parameters
                                List<SqlParameter> arrParm = new List<SqlParameter>
                                {
                                    new SqlParameter("@customer_id", SqlDbType.Int) { Value = cbbCustomerID.Value },
                                };
                                conn.Open();
                                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_mfg_list_quotation", arrParm.ToArray());

                                conn.Close();
                                if (dsResult.Tables.Count > 0)
                                {
                                    if (dsResult.Tables[0].Rows.Count > 0)
                                    {
                                        cbbModel.DataSource = dsResult;
                                        cbbModel.TextField = "model";
                                        cbbModel.ValueField = "id";
                                        cbbModel.DataBind();
                                    }
                                }
                            }

                            //cbbModel.Value = Convert.IsDBNull(data["id"]) ? "0" : Convert.ToString(data["id"]);
                            cbbModel.Text = Convert.IsDBNull(data["model"]) ? "" : Convert.ToString(data["model"]) == "0" ? "" : Convert.ToString(data["model"]);
                            cbbModel.Value = Convert.IsDBNull(data["model"]) ? "" : Convert.ToString(data["model"]) == "0" ? "" : Convert.ToString(data["model"]);
                            hdModelName.Value = cbbModel.Text;

                            cbbModel.Text += ("; " + txtShowMFG.Value);
                            cbbModel.Value = Convert.IsDBNull(data["model_id"]) ? string.Empty : Convert.ToString(data["model_id"]) == "0" ? "" : Convert.ToString(data["model_id"]);
                            //hdSumDiscountDetail.Value = Convert.ToString(data["address_bill_tha"]);
                            //hdTotal.Value = Convert.ToString(data["address_bill_tha"]);
                            //hdTotalWithDiscountDetail.Value = Convert.ToString(data["address_bill_tha"]);
                        }
                        #endregion Quotation Header

                        #region Bind Grid When Load Header
                        if (cbbQuotationType.Value == "D") // Service Change
                        {

                            contactor.Attributes["style"] = "display:none";
                            dvGridProduct.Attributes["style"] = "display:none";
                            dvGridService.Attributes["style"] = "display:''";
                            dvIsService.Attributes["style"] = "display:''";
                            dvIsProductDescription.Attributes["style"] = "display:none";
                            gridQuotationDetail.Columns[4].Visible = true;
                            gridQuotationDetail.Columns[5].Visible = false;
                            gridQuotationDetail.Columns[6].Visible = false;
                            dvIsAnnualService.Attributes["style"] = "display:none";
                            tabOtherDetail.Attributes["style"] = "display:none";

                        }
                        else if (cbbQuotationType.Value == "C") // Service Change
                        {

                            contactor.Attributes["style"] = "display:none";
                            dvGridProduct.Attributes["style"] = "display:none";
                            dvGridService.Attributes["style"] = "display:''";
                            dvIsService.Attributes["style"] = "display:''";
                            dvIsProductDescription.Attributes["style"] = "display:none";
                            gridQuotationDetail.Columns[4].Visible = true;
                            gridQuotationDetail.Columns[5].Visible = false;
                            gridQuotationDetail.Columns[6].Visible = false;
                            dvIsAnnualService.Attributes["style"] = "display:none";
                            tabOtherDetail.Attributes["style"] = "display:''";

                        }
                        else if (cbbQuotationType.Value == "O") // Other
                        {
                            dvGridProduct.Attributes["style"] = "display:none";
                            dvGridService.Attributes["style"] = "display:''";
                            dvIsService.Attributes["style"] = "display:''";
                            dvIsProductDescription.Attributes["style"] = "display:none";
                            gridQuotationDetail.Columns[4].Visible = true;
                            gridQuotationDetail.Columns[5].Visible = false;
                            gridQuotationDetail.Columns[6].Visible = false;
                            dvIsAnnualService.Attributes["style"] = "display:none";
                            tabOtherDetail.Attributes["style"] = "display:none";

                        }

                        else if (cbbQuotationType.Value == "P") // Product
                        {

                            dvGridProduct.Attributes["style"] = "display:''";
                            dvGridService.Attributes["style"] = "display:none";
                            dvIsService.Attributes["style"] = "display:none";
                            dvIsAnnualService.Attributes["style"] = "display:none";
                            dvIsProductDescription.Attributes["style"] = "display:none";
                            if (rdoDescription.Checked)
                            {
                                gridQuotationDetail.Columns[4].Visible = true;
                                gridQuotationDetail.Columns[5].Visible = false;
                            }
                            else
                            {
                                gridQuotationDetail.Columns[4].Visible = false;
                                gridQuotationDetail.Columns[5].Visible = true;
                            }
                            gridQuotationDetail.Columns[6].Visible = false;
                            tabOtherDetail.Attributes["style"] = "display:none";
                        }
                        else if (cbbQuotationType.Value == "S") // Spare Part
                        {
                            contactor.Attributes["style"] = "display:none";
                            dvGridProduct.Attributes["style"] = "display:''";
                            dvIsAnnualService.Attributes["style"] = "display:none";
                            dvGridService.Attributes["style"] = "display:none";
                            dvIsService.Attributes["style"] = "display:none";
                            dvIsProductDescription.Attributes["style"] = "display:none";
                            gridQuotationDetail.Columns[4].Visible = true;
                            gridQuotationDetail.Columns[5].Visible = false;
                            gridQuotationDetail.Columns[6].Visible = false;
                        }
                        else if (cbbQuotationType.Value == "M") // Maintenance
                        {
                            contactor.Attributes["style"] = "display:none";
                            dvGridProduct.Attributes["style"] = "display:''";
                            dvIsAnnualService.Attributes["style"] = "display:none";
                            dvGridService.Attributes["style"] = "display:none";
                            dvIsService.Attributes["style"] = "display:none";
                            dvIsProductDescription.Attributes["style"] = "display:none";
                            gridQuotationDetail.Columns[4].Visible = true;
                            gridQuotationDetail.Columns[5].Visible = false;
                            gridQuotationDetail.Columns[6].Visible = false;
                            btnAddPartList.Visible = true;
                            btnAddAllPartList.Visible = true;
                            //dvIsService.Style.Add("display", "none");
                        }
                        else if (cbbQuotationType.Value == "A") // Annual Service
                        {

                            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                            {
                                //Create array of Parameters
                                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = cbbCustomerID.Value },
                            new SqlParameter("@search_text", SqlDbType.VarChar,500) {Value = "" }
                        };
                                conn.Open();
                                var dsAnnualServiceHistory = SqlHelper.ExecuteDataset(conn, "sp_annual_service_list_by_customer", arrParm.ToArray());
                                conn.Close();
                                if (dsAnnualServiceHistory != null)
                                {
                                    foreach (var row in dsAnnualServiceHistory.Tables[0].AsEnumerable())
                                    {
                                        historyAnnualServiceItemList.Add(new AnnualServiceItem()
                                        {
                                            is_selected = false,
                                            mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                            product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                            product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                            project = Convert.IsDBNull(row["project"]) ? string.Empty : Convert.ToString(row["project"]),
                                            unit_code = "unit",
                                        });
                                    }
                                }


                            }
                            contactor.Attributes["style"] = "display:none";
                            dvIsAnnualService.Attributes["style"] = "display:''";
                            dvGridProduct.Attributes["style"] = "display:''";
                            dvGridService.Attributes["style"] = "display:none";
                            dvIsService.Attributes["style"] = "display:none";
                            dvIsAnnualService_Discount.Attributes["style"] = "display:''";
                            dvIsProductDescription.Attributes["style"] = "display:none";
                            gridQuotationDetail.Columns[4].Visible = false;
                            gridQuotationDetail.Columns[5].Visible = false;
                            gridQuotationDetail.Columns[6].Visible = true;
                            tabOtherDetail.Attributes["style"] = "display:''";

                            //dvIsService.Style.Add("display", "none");
                        }
                        #endregion

                        #region Quotation Detail 
                        // Quotation Detail List
                        string modelName = "";
                        string runningHours = "";
                        foreach (var row in dsQuataionData.Tables[1].AsEnumerable())
                        {
                            var dataParent_id = Convert.ToInt32(row["parent_id"]);
                            var idOrg = Convert.ToInt32(row["id"]);
                            var newId = (quotationDetailList.Count + 1) * -1;

                            var qd = (from t in quotationDetailList where t.id_org == dataParent_id select t).FirstOrDefault();
                            if (qd != null)
                            {
                                dataParent_id = qd.id;

                            }
                            quotationDetailList.Add(new QuotationDetail()
                            {
                                //delivery_note_no = Convert.ToString(row["delivery_note_no"]),
                                id_org = Convert.ToInt32(row["id"]),
                                parant_id_org = Convert.ToInt32(row["parent_id"]),
                                discount_amount = Convert.IsDBNull(row["discount_amount"]) ? 0 : Convert.ToDecimal(row["discount_amount"]),
                                discount_percentage = Convert.IsDBNull(row["discount_percentage"]) ? 0 : Convert.ToDecimal(row["discount_percentage"]),
                                discount_type = Convert.IsDBNull(row["discount_type"]) ? string.Empty : Convert.ToString(row["discount_type"]),
                                id = newId,
                                parant_id = dataParent_id,
                                min_unit_price = Convert.IsDBNull(row["min_unit_price"]) ? 0.0m : Convert.ToDecimal(row["min_unit_price"]),
                                product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                product_type = Convert.IsDBNull(row["product_type"]) ? string.Empty : Convert.ToString(row["product_type"]),
                                product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["qty"]),
                                quotation_description = Convert.IsDBNull(row["quotation_description"]) ? string.Empty : Convert.ToString(row["quotation_description"]),
                                quotation_no = Convert.IsDBNull(row["quotation_no"]) ? string.Empty : Convert.ToString(row["quotation_no"]),
                                total_amount = Convert.IsDBNull(row["total_amount"]) ? 0.0m : Convert.ToDecimal(row["total_amount"]),
                                unit_code = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]),
                                unit_price = Convert.IsDBNull(row["unit_price"]) ? 0.0m : Convert.ToDecimal(row["unit_price"]),
                                is_deleted = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                sort_no = Convert.IsDBNull(row["sort_no"]) ? 1 : Convert.ToInt32(row["sort_no"]),

                                quotation_line = Convert.IsDBNull(row["quotation_line"]) ? string.Empty : Convert.ToString(row["quotation_line"]),
                                mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                is_history_issue_mfg = Convert.IsDBNull(row["is_history_issue_mfg"]) ? false : Convert.ToBoolean(row["is_history_issue_mfg"]),
                                quotation_line_1 = Convert.IsDBNull(row["quotation_line_1"]) ? string.Empty : Convert.ToString(row["quotation_line_1"]),
                                quotation_line_2 = Convert.IsDBNull(row["quotation_line_2"]) ? string.Empty : Convert.ToString(row["quotation_line_2"]),
                                quotation_line_3 = Convert.IsDBNull(row["quotation_line_3"]) ? string.Empty : Convert.ToString(row["quotation_line_3"]),
                                quotation_line_4 = Convert.IsDBNull(row["quotation_line_4"]) ? string.Empty : Convert.ToString(row["quotation_line_4"]),
                                quotation_line_5 = Convert.IsDBNull(row["quotation_line_5"]) ? string.Empty : Convert.ToString(row["quotation_line_5"]),
                                quotation_line_6 = Convert.IsDBNull(row["quotation_line_6"]) ? string.Empty : Convert.ToString(row["quotation_line_6"]),
                                quotation_line_7 = Convert.IsDBNull(row["quotation_line_7"]) ? string.Empty : Convert.ToString(row["quotation_line_7"]),
                                quotation_line_8 = Convert.IsDBNull(row["quotation_line_8"]) ? string.Empty : Convert.ToString(row["quotation_line_8"]),
                                quotation_line_9 = Convert.IsDBNull(row["quotation_line_9"]) ? string.Empty : Convert.ToString(row["quotation_line_9"]),
                                quotation_line_10 = Convert.IsDBNull(row["quotation_line_10"]) ? string.Empty : Convert.ToString(row["quotation_line_10"]),

                                package_id = Convert.IsDBNull(row["package_id"]) ? 0 : Convert.ToInt32(row["package_id"]),
                                running_hours = Convert.IsDBNull(row["running_hours"]) ? "" : Convert.ToString(row["running_hours"]),
                                model_id = Convert.IsDBNull(row["model_id"]) ? 0 : Convert.ToInt32(row["model_id"]),
                                model_name = Convert.IsDBNull(row["model_name"]) ? "" : Convert.ToString(row["model_name"]),

                            });

                            if (quotationDetailList[quotationDetailList.Count - 1].model_name != "" && modelName == "")
                            {
                                modelName = quotationDetailList[quotationDetailList.Count - 1].model_name;
                            }
                            if (quotationDetailList[quotationDetailList.Count - 1].running_hours != "" && runningHours == "")
                            {
                                runningHours = quotationDetailList[quotationDetailList.Count - 1].running_hours;
                            }

                        }

                        if (cbbQuotationType.Value == "M")
                        {
                            lblModelPackage.Text = "Model : " + modelName + ", Running hour : " + runningHours;
                        }
                        #endregion Quotation Detail

                        #region Quotation Remark
                        // Qutation Remark
                        foreach (var row in dsQuataionData.Tables[2].AsEnumerable())
                        {
                            if (Convert.ToInt32(row["sort_no"]) == 1)
                            {
                                txtRemark1.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 2)
                            {
                                txtRemark2.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 3)
                            {
                                txtRemark3.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 4)
                            {
                                txtRemark4.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 5)
                            {
                                txtRemark5.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 6)
                            {
                                txtRemark6.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 7)
                            {
                                txtRemark7.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 8)
                            {
                                txtRemark8.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 9)
                            {
                                txtRemark9.Value = Convert.ToString(row["remark"]);
                            }
                            else if (Convert.ToInt32(row["sort_no"]) == 10)
                            {
                                txtRemarkOther.Value = Convert.ToString(row["remark"]);
                            }
                        }
                        #endregion Quotation Remark

                        #region Quotation Notification
                        // Quotation Notification
                        foreach (var row in dsQuataionData.Tables[3].AsEnumerable())
                        {
                            notificationList.Add(new Notification()
                            {
                                description = Convert.ToString(row["description"]),
                                display_notice_date = Convert.ToDateTime(row["notice_date"]).ToString("dd/MM/yyyy HH:mm"),//Convert.ToString(row["display_notice_date"]),
                                id = (notificationList.Count + 1) * -1,
                                is_deleted = Convert.ToBoolean(row["is_delete"]),
                                notice_date = Convert.ToDateTime(row["notice_date"]),
                                notice_type = Convert.ToString(row["description"]),
                                reference_id = Convert.ToInt32(row["reference_id"]),
                                reference_no = Convert.ToString(row["reference_no"]),
                                subject = Convert.ToString(row["subject"]),
                                topic = Convert.ToString(row["topic"])
                            });
                        }
                        #endregion Quotation Notification
                    }

                    // Bind Summary Textbox

                    if (!cbDiscountByItem.Checked)
                    {
                        cbbDiscountByItem.Attributes["disabled"] = "disabled";
                    }
                    if (cbDiscountBottomBill1.Checked)
                    {
                        txtDiscountBottomBill1.Attributes.Remove("disabled");
                        cbbDiscountBottomBill1.Attributes.Remove("disabled");

                        cbDiscountBottomBill2.Attributes.Remove("disabled");
                    }
                    else
                    {
                        txtDiscountBottomBill1.Attributes["disabled"] = "disabled";
                        cbbDiscountBottomBill1.Attributes["disabled"] = "disabled";
                    }
                    if (cbDiscountBottomBill2.Checked)
                    {
                        cbDiscountBottomBill2.Attributes.Remove("disabled");
                        txtDiscountBottomBill2.Attributes.Remove("disabled");
                        cbbDiscountBottomBill2.Attributes.Remove("disabled");
                    }
                    else
                    {
                        txtDiscountBottomBill2.Attributes["disabled"] = "disabled";
                        cbbDiscountBottomBill2.Attributes["disabled"] = "disabled";
                    }
                    if (quotationDetailList.Count > 0)
                    {
                        cbbDiscountByItem.Value = quotationDetailList[0].discount_type;
                        txtDiscountByItem.Value = (quotationDetailList[0].discount_type.Equals("P") ? quotationDetailList[0].discount_percentage.ToString() : quotationDetailList[0].discount_amount.ToString());
                        cbbDiscountByItem.Disabled = false;
                    }


                    //cbbQuotationType.Style["disabled"] = "disabled";
                    cbbQuotationType.Attributes["disabled"] = "disabled";
                    //cbbCustomerID.Attributes["disabled"] = "disabled";
                    rdoLumpsum.Attributes["disabled"] = "disabled";
                    rdoNotLumpsum.Attributes["disabled"] = "disabled";
                    BindGridQuotationDetail(false);
                    BindGridFreeBies(false);
                    BindGridNotification(false);
                    //BindGridAnnualServiceItem();
                    BindGridHistoryAnnualService();

                    //cbbCustomerID.Attributes["disabled"] = "disabled";

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static string GetQuotationDesignText(string quotationType)
        {
            var returnData = new List<string>();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {

                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@quotationType", SqlDbType.VarChar,1) { Value = quotationType },
                        };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_quotation_design_text", arrParm.ToArray());
                conn.Close();
                if (dsResult != null)
                {
                    foreach (var row in dsResult.Tables[0].AsEnumerable())
                    {
                        returnData.Add(Convert.IsDBNull(row["quotation_description"]) ? string.Empty : Convert.ToString(row["quotation_description"]));
                    }
                }
            }
            var json = JsonConvert.SerializeObject(returnData);
            return json;

        }
        [WebMethod]
        public static string SetDiscountByItemAll(string discount_item_qty, string discount_item_type)
        {
            var returnData = string.Empty;
            var quotationDetail = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            if (quotationDetail != null)
            {
                if (discount_item_type == "P")
                {
                    foreach (var row in quotationDetail)
                    {
                        if ((row.qty * row.unit_price) > 0)
                        {
                            row.discount_percentage = Convert.ToDecimal(discount_item_qty);
                            row.discount_total = ((row.qty * row.unit_price) * row.discount_percentage) / 100;
                        }
                        else
                        {
                            row.discount_percentage = row.discount_total = 0;
                        }
                    }
                }
                if (discount_item_type == "A")
                {
                    foreach (var row in quotationDetail)
                    {
                        if ((row.qty * row.unit_price) > 0)
                        {
                            row.discount_amount = Convert.ToDecimal(discount_item_qty);
                            row.discount_total = (row.qty * row.unit_price) - row.discount_amount;
                        }
                        else
                        {
                            row.discount_amount = row.discount_total = 0;
                        }
                    }
                }
            }
            return returnData;
        }


        [WebMethod]
        public static string AddCustomerAttention(string customer_id, string name, string tel, string email)
        {
            var returnData = string.Empty;

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("sp_customer_attention_add", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToInt32(customer_id);
                    cmd.Parameters.Add("@attention_name", SqlDbType.VarChar, 200).Value = name;
                    cmd.Parameters.Add("@attention_tel", SqlDbType.VarChar, 100).Value = tel;
                    cmd.Parameters.Add("@attention_email", SqlDbType.VarChar, 100).Value = email;
                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            return returnData;

        }

        protected void cbbCustomerAttention_Callback(object sender, CallbackEventArgsBase e)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = cbbCustomerID.Value }, //4
                        };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_attention_list", arrParm.ToArray());

                conn.Close();
                cbbCustomerAttention.DataSource = dsResult;
                cbbCustomerAttention.TextField = "attention_name";
                cbbCustomerAttention.ValueField = "id";
                cbbCustomerAttention.DataBind();


            }
        }
        protected void cbbModel_Callback(object sender, CallbackEventArgsBase e)
        {
            var customerMFG = (List<CustomerMFG>)HttpContext.Current.Session["SESSION_CUSTOMERMFG_QUATATION"];
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                                {
                                    new SqlParameter("@customer_id", SqlDbType.Int) { Value = cbbCustomerID.Value },
                                };
                    conn.Open();
                    var dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_mfg_list_quotation", arrParm.ToArray());

                    conn.Close();
                    cbbModel.DataSource = dsResult;
                    cbbModel.TextField = "model";
                    cbbModel.ValueField = "id";
                    cbbModel.DataBind();

                    //////////////////add session//////////
                    if (dsResult != null)
                    {
                        customerMFG = new List<CustomerMFG>();
                        foreach (var row in dsResult.Tables[0].AsEnumerable())
                        {
                            customerMFG.Add(new CustomerMFG()
                            {
                                mfg = Convert.ToString(row["mfg"]),
                                id = Convert.ToInt32(row["id"]),
                                model = Convert.ToString(row["model"])
                            });
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            HttpContext.Current.Session["SESSION_CUSTOMERMFG_QUATATION"] = customerMFG;

        }

        [WebMethod]
        public static string GetModelData(string id)
        {
            var customerMFG = (List<CustomerMFG>)HttpContext.Current.Session["SESSION_CUSTOMERMFG_QUATATION"];
            var modelData = "";
            if (customerMFG != null && customerMFG.Count != 0)
            {
                modelData = (from t in customerMFG where t.id == Convert.ToInt32(id) select t.mfg).FirstOrDefault();
            }

            return modelData;
        }


        [WebMethod]
        public static string SelectCustomerAttention(string id)
        {
            var returnData = string.Empty;

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = 0 }, //4
                        };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_attention_list", arrParm.ToArray());

                conn.Close();
                if (dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        returnData = Convert.ToString(dsResult.Tables[0].Rows[0]["attention_tel"]) + "|" +
                                     Convert.ToString(dsResult.Tables[0].Rows[0]["attention_email"]);
                    }
                }
            }
            return returnData;
        }
        [WebMethod]
        public static string GetQuotationModelMFG(string customer_id)
        {
            var returnData = new List<string>();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = customer_id }, //4
                            new SqlParameter("@model", SqlDbType.VarChar,200) { Value = string.Empty }, //4
                        };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_quotation_header_model_mfg", arrParm.ToArray());
                conn.Close();
                if (dsResult != null)
                {
                    foreach (var row in dsResult.Tables[0].AsEnumerable())
                    {
                        //returnData.Add(Convert.IsDBNull(row["model"]) ? string.Empty : Convert.ToString(row["model"]));
                        //returnData.Add(Convert.IsDBNull(row["mfg"]) ? string.Empty : Convert.ToString(row["mfg"]));
                        returnData.Add(Convert.IsDBNull(row["model"]) ? string.Empty : Convert.ToString(row["model"]));
                    }
                }
            }
            var json = JsonConvert.SerializeObject(returnData);
            return json;

        }
        [WebMethod]
        public static string GetQuotationMFG(string customer_id, string model)
        {
            var returnData = new List<string>();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = customer_id }, //4
                            new SqlParameter("@model", SqlDbType.VarChar,200) { Value = model }, //4
                        };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_quotation_header_model_mfg", arrParm.ToArray());
                conn.Close();
                if (dsResult != null)
                {
                    foreach (var row in dsResult.Tables[0].AsEnumerable())
                    {
                        //returnData.Add(Convert.IsDBNull(row["model"]) ? string.Empty : Convert.ToString(row["model"]));
                        //returnData.Add(Convert.IsDBNull(row["mfg"]) ? string.Empty : Convert.ToString(row["mfg"]));
                        returnData.Add(Convert.IsDBNull(row["mfg"]) ? string.Empty : Convert.ToString(row["mfg"]));
                    }
                }
            }
            var json = JsonConvert.SerializeObject(returnData);
            return json;

        }

        [WebMethod]
        public static string AddCustomerModel(string customer_id, string model, string mfg)
        {
            var returnData = string.Empty;

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("sp_customer_mfg_add", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToInt32(customer_id);
                    cmd.Parameters.Add("@model", SqlDbType.VarChar, 50).Value = model;
                    cmd.Parameters.Add("@mfg", SqlDbType.VarChar, 50).Value = mfg;
                    cmd.Parameters.Add("@unitwarraty", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@airendwarraty", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@service_fee", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@project", SqlDbType.VarChar, 200).Value = "";
                    cmd.Parameters.Add("@is_demo", SqlDbType.Bit).Value = 0;
                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            return returnData;

        }

        [WebMethod]
        public static string CheckedDiscountByItem(int check, int typeDiscount)
        {
            var returnData = "Null";
            var Datalist = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];

            if (Datalist.Count == 0)
            {
                return returnData;
            }
            else
            {
                if (check == 0)
                {
                    if (typeDiscount == 0)
                    {
                        foreach (var row in Datalist)
                        {
                            row.total_amount = row.total_amount + row.discount_total;
                            row.discount_amount = 0;
                            row.discount_type = "";
                            row.discount_percentage = 0;
                            row.discount_total = 0;
                        }

                        HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"] = Datalist;
                    }
                    else if (typeDiscount == 1)
                    {

                    }
                    else if (typeDiscount == 2)
                    {

                    }
                }

                return "Not Null";
            }
        }
        [WebMethod]
        public static void SlectOnCheckedDiscountByItem()
        {
            var Datalist = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];

            foreach (var row in Datalist)
            {
                row.total_amount = row.total_amount + row.discount_amount;
                row.discount_amount = 0;
                row.discount_type = "";
                row.discount_percentage = 0;
                row.discount_total = 0;
            }
        }

        [WebMethod]
        public static void RefreshMAPackage()
        {
            List<QuotationDetail> prevData = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            if (prevData != null)
            {
                for (int i = 0; i < prevData.Count; i++)
                {
                    if (prevData[i].id < 0)
                    {
                        prevData.RemoveAt(i);
                    }
                    else
                    {
                        prevData[i].is_deleted = true;
                    }
                }
            }

            var productDetail = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION_TEMP"];
            productDetail.AddRange(prevData);
            HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"] = productDetail;
        }

        protected void gridViewServiceDetail_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameters))
            {
                int parentId = Convert.ToInt32(e.Parameters);
                if (parentId != 0)
                {
                    gridViewServiceDetail.DataSource = (from t in quotationDetailList
                                                        where t.parant_id == parentId
                                                        && t.is_deleted == false
                                                        select t).ToList();

                    if (rdoNotLumpsum.Checked)
                    {
                        if (cbDiscountByItem.Checked)
                        {
                            if (cbbDiscountByItem.Value == "P")
                            {
                                gridViewServiceDetail.Columns[6].Visible = false;
                                gridViewServiceDetail.Columns[7].Visible = true;
                                gridViewServiceDetail.Columns[8].Visible = true;
                            }
                            else if (cbbDiscountByItem.Value == "A")
                            {
                                gridViewServiceDetail.Columns[6].Visible = true;
                                gridViewServiceDetail.Columns[7].Visible = false;
                                gridViewServiceDetail.Columns[8].Visible = true;
                            }
                        }
                    }
                    else
                    {
                        gridViewServiceDetail.Columns[2].Visible = true;
                        gridViewServiceDetail.Columns[3].Visible = false;
                        gridViewServiceDetail.Columns[4].Visible = false;
                        gridViewServiceDetail.Columns[5].Visible = false;

                        gridViewServiceDetail.Columns[6].Visible = false;
                        gridViewServiceDetail.Columns[7].Visible = false;
                        gridViewServiceDetail.Columns[8].Visible = false;
                    }

                    gridViewServiceDetail.FilterExpression = FilterBag.GetExpression(false);
                    gridViewServiceDetail.DataBind();
                }
                else
                {
                    gridViewServiceDetail.DataSource = null;
                    gridViewServiceDetail.DataBind();
                }
            }
            else
            {
                gridViewServiceDetail.Columns[6].Visible = false;
                gridViewServiceDetail.Columns[7].Visible = false;
                gridViewServiceDetail.Columns[8].Visible = false;
                
                gridViewServiceDetail.FilterExpression = FilterBag.GetExpression(false);
                gridViewServiceDetail.DataBind();
            }

            //  Check status Follow
            if (quotationStatus.Value == "FL" || quotationStatus.Value == "PO")
            {
                gridViewServiceDetail.Columns[0].Visible = false;
                gridViewServiceDetail.Columns[1].Visible = false;
            }

        }

        #region Validate Data
        [WebMethod]
        public static string ValidateData(string type)
        {
            string msg = string.Empty;
            var quotationDetail = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_QUATATION"];
            if (quotationDetail.Count == 0)
            {
                msg = "กรุณาเลือกราย สินค้า อย่างน้อย 1 รายการ \n";
            }
            return msg;
        }

        #endregion

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            var quStatus = quotationStatus.Value;

            if (quStatus == "FL" || quStatus == "PO")
            {
                SaveData("CC", "");
            }

            else
            {
                // DO NOTHING
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('ไม่สามารถทำรายการได้','E')", true);
            }
        }

        protected void btnLost_Click(object sender, EventArgs e)
        {
            var quStatus = quotationStatus.Value;
            if (quStatus == "FL" || quStatus == "PO")
            {
                SaveData("LO", "");
            }

            else
            {
                // DO NOTHING
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('ไม่สามารถทำรายการได้','E')", true);
            }
        }

        protected void btnPO_Click(object sender, EventArgs e)
        {
            var quStatus = quotationStatus.Value;
            if (quStatus == "FL")
            {
                SaveData("PO", "");
            }

            else
            {
                // DO NOTHING
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('ไม่สามารถทำรายการได้','E')", true);
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            var quStatus = quotationStatus.Value;
            SaveData("RE", "");
            /*if (quStatus == "DW")
            {
                SaveData("RE", "");
            }

            else
            {
                // DO NOTHING
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('ไม่สามารถทำรายการได้','E')", true);
            }*/
        }

        protected void cbbSubject_Callback(object sender, CallbackEventArgsBase e)
        {
            #region Load Config From Database
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {

                            new SqlParameter("@config_document", SqlDbType.Char,2) { Value = "QU" },
                            new SqlParameter("@config_type", SqlDbType.Char,1) { Value = cbbQuotationType.Value },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                        };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_config_list", arrParm.ToArray());
                conn.Close();
                var data = new List<ConfigDocument>();
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    //txtSubject.Value = Convert.ToString(dsResult.Tables[0].Rows[0]["config_description"]);
                    foreach (var row in dsResult.Tables[0].AsEnumerable())
                    {
                        data.Add(new ConfigDocument()
                        {
                            id = Convert.ToInt32(row["id"]),
                            document_description = Convert.ToString(row["config_description"])
                        });
                    }
                }
                cbbSubject.ValueField = "id";
                cbbSubject.TextField = "document_description";
                cbbSubject.DataSource = data;
                cbbSubject.DataBind();
            }
            #endregion Load Config From Database
        }

        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            var quStatus = quotationStatus.Value;
        }

        protected void CancelX_Click(object sender, EventArgs e)
        {

        }
    }
}