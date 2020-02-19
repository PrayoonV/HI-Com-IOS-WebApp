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
    public partial class PurchaseRequest : MasterDetailPage
    {
        public int dataId = 0;
        public override string PageName { get { return "Purchase Request"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }
        public override void OnFilterChanged()
        {

        }

        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            return result.OfType<FilterControlColumn>();
        }
        #region  Member
        private DataSet dsPurchaseRequest = new DataSet();
        public class QuotationData
        {
            public string quotation_no { get; set; }
            public int quotation_id { get; set; }
            public string quotation_type { get; set; }
            public DateTime quotation_date { get; set; }
            public string display_quotation_date { get; set; }
            public int customer_id { get; set; }
            public string customer_name { get; set; }
            public string customer_code { get; set; }
            public string po_no { get; set; }
            public string po_date { get; set; }

        }
        public class QuotationDetail
        {
            public bool is_selected { get; set; }
            public int id { get; set; }
            public string quotation_no { get; set; }
            public string product_no { get; set; }
            public int product_id { get; set; }
            public string quotation_description { get; set; }
            public string quotation_line { get; set; }
            public string unit_code { get; set; }
            public int qty { get; set; }
            public int min_qty { get; set; }
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
            public int package_id { get; set; }
            public bool is_history_issue_mfg { get; set; }
            public string mfg_no { get; set; }
            public string product_type { get; set; }
            public int supplier_id { get; set; }
            public string supplier_name { get; set; }

        }
        public class ProductDetail
        {
            public bool is_selected { get; set; }
            public int product_id { get; set; }
            public string product_no { get; set; }
            public string product_name { get; set; }
            public string item_type { get; set; }
            public string item_unit { get; set; }
            public int qty { get; set; }
            public int qty_reserve { get; set; }
            public int supplier_id { get; set; }
            public string supplier_name { get; set; }
        }
        public class PurchaseRequestDetail
        {
            public int id { get; set; }
            public string purchase_request_no { get; set; }
            public string item_no { get; set; }
            public int item_id { get; set; }
            public int sort_no { get; set; }
            public string item_description { get; set; }
            public int qty { get; set; }
            public int old_qty { get; set; }
            public int receive_qty { get; set; }
            public int remain_qty { get; set; }
            public DateTime? receive_date { get; set; }
            public int receive_by { get; set; }
            public bool is_deleted { get; set; }
            public string item_unit { get; set; }
            public string item_type { get; set; }
            public string display_receive_date { get; set; }
            public string purchaseOrderNo { get; set; }
            public string receiveNo { get; set; }
            public int receive_qty_balance { get; set; }
            public bool is_receive_all { get; set; }
            public int supplier_id { get; set; }
            public string supplier_name { get; set; }
            public string history_log { get; set; }
        }
        public class MFGDetail
        {
            public bool is_selected { get; set; }
            public string mfg_no { get; set; }
        }
        public class PurchaseRequestDetailMFG
        {
            public int id { get; set; }
            public string pr_no { get; set; }
            public int pr_detail_id { get; set; }
            public int item_id { get; set; }
            public string mfg_no { get; set; }
            public string old_mfg_no { get; set; }
            public bool is_deleted { get; set; }
            public bool is_pr_receive { get; set; }
            public string receive_no { get; set; }
            public DateTime? receive_date { get; set; }
            public string display_receive_date { get; set; }
            public bool is_in_stock { get; set; }
        }
        List<PurchaseRequestDetail> purchaseRequestDetailList
        {
            get
            {
                if (Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"] == null)
                    Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"] = new List<PurchaseRequestDetail>();
                return (List<PurchaseRequestDetail>)Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"];
            }
            set
            {
                Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"] = value;
            }
        }
        List<PurchaseRequestDetail> selectedPurchaseRequestDetailList
        {
            get
            {
                if (Session["SESSION_SELECTED_PURCHASE_REQUEST_DETAIL_PR"] == null)
                    Session["SESSION_SELECTED_PURCHASE_REQUEST_DETAIL_PR"] = new List<PurchaseRequestDetail>();
                return (List<PurchaseRequestDetail>)Session["SESSION_SELECTED_PURCHASE_REQUEST_DETAIL_PR"];
            }
            set
            {
                Session["SESSION_SELECTED_PURCHASE_REQUEST_DETAIL_PR"] = value;
            }
        }
        List<ProductDetail> productDetailList
        {
            get
            {
                if (Session["SESSION_PRODUCT_DETAIL_PR"] == null)
                    Session["SESSION_PRODUCT_DETAIL_PR"] = new List<ProductDetail>();
                return (List<ProductDetail>)Session["SESSION_PRODUCT_DETAIL_PR"];
            }
            set
            {
                Session["SESSION_PRODUCT_DETAIL_PR"] = value;
            }
        }
        List<QuotationDetail> quotationDetailList
        {
            get
            {
                if (Session["SESSION_QUOTATION_DETAIL_PR"] == null)
                    Session["SESSION_QUOTATION_DETAIL_PR"] = new List<QuotationDetail>();
                return (List<QuotationDetail>)Session["SESSION_QUOTATION_DETAIL_PR"];
            }
            set
            {
                Session["SESSION_QUOTATION_DETAIL_PR"] = value;
            }
        }

        List<MFGDetail> mfgDetailList
        {
            get
            {
                if (Session["SESSION_MFG_DETAIL_PR"] == null)
                    Session["SESSION_MFG_DETAIL_PR"] = new List<MFGDetail>();
                return (List<MFGDetail>)Session["SESSION_MFG_DETAIL_PR"];
            }
            set
            {
                Session["SESSION_MFG_DETAIL_PR"] = value;
            }
        }
        List<PurchaseRequestDetailMFG> prDetailMFGList
        {
            get
            {
                if (Session["SESSION_PR_DETAIL_MFG_PR"] == null)
                    Session["SESSION_PR_DETAIL_MFG_PR"] = new List<PurchaseRequestDetailMFG>();
                return (List<PurchaseRequestDetailMFG>)Session["SESSION_PR_DETAIL_MFG_PR"];
            }
            set
            {
                Session["SESSION_PR_DETAIL_MFG_PR"] = value;
            }
        }
        List<PurchaseRequestDetailMFG> selectedPRDetailMFGList
        {
            get
            {
                if (Session["SESSION_SELECTED_PR_DETAIL_MFG_PR"] == null)
                    Session["SESSION_SELECTED_PR_DETAIL_MFG_PR"] = new List<PurchaseRequestDetailMFG>();
                return (List<PurchaseRequestDetailMFG>)Session["SESSION_SELECTED_PR_DETAIL_MFG_PR"];
            }
            set
            {
                Session["SESSION_SELECTED_PR_DETAIL_MFG_PR"] = value;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dataId = Convert.ToInt32(Request.QueryString["dataId"]);
                if (!IsPostBack)
                {
                    // Get Permission and if no permission, will redirect to another page.
                    if (!Permission.GetPermission())
                        Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

                    ClearWorkingSession();
                    PrepareData();
                    LoadData();
                }
                else
                {
                    BindGridViewQuotationDetail();
                    BindGridViewProductDetail();
                    BindGridMFGDetail();
                    BindGridViewPR();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void PrepareData()
        {
            try
            {
                if (dataId == 0)
                {
                    SPlanetUtil.BindASPxComboBox(ref cbbQuotation, DataListUtil.DropdownStoreProcedureName.Quotation_Document_PR); // ใช้อันเดียวกันได้

                    hdDocumentStatus.Value = "";
                }
                SPlanetUtil.BindASPxComboBox(ref cbbCustomer, DataListUtil.DropdownStoreProcedureName.Customer);
                SPlanetUtil.BindASPxComboBox(ref cbbSupplier, DataListUtil.DropdownStoreProcedureName.Supplier);

                cbbCustomer.Items.Insert(0, new ListEditItem("ไม่ระบุ", "0"));
                cbbCustomer.Value = "0";

                gridViewPR.SettingsBehavior.AllowFocusedRow = true;
                gridMFG.SettingsBehavior.AllowFocusedRow = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ClearWorkingSession()
        {
            Session.Remove("SESSION_PRODUCT_DETAIL_PR");
            Session.Remove("SESSION_PURCHASE_REQUEST_DETAIL_PR");
            Session.Remove("SESSION_QUOTATION_DETAIL_PR");
            Session.Remove("SESSION_SELECTED_PURCHASE_REQUEST_DETAIL_PR");
            Session.Remove("SESSION_SELECTED_PR_DETAIL_MFG_PR");
            Session.Remove("SESSION_PR_DETAIL_MFG_PR");
            Session.Remove("SESSION_MFG_DETAIL_PR");
        }

        protected void LoadData()
        {
            try
            {
                if (dataId > 0)
                {

                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = dataId },
                        };
                        conn.Open();
                        dsPurchaseRequest = SqlHelper.ExecuteDataset(conn, "sp_purchase_request_list_data", arrParm.ToArray());
                        ViewState["dsPurchaseRequest"] = dsPurchaseRequest;
                        conn.Close();
                    }


                    if (dsPurchaseRequest.Tables.Count > 0)
                    {
                        #region "if (dsPurchaseRequest.Tables.Count > 0)"

                        var header = (from t in dsPurchaseRequest.Tables[0].AsEnumerable() select t).FirstOrDefault();
                        if (header != null)
                        {


                            cbbCustomer.Value = Convert.IsDBNull(header["customer_id"]) ? string.Empty : Convert.ToString(header["customer_id"]) == "0" ? "" : Convert.ToString(header["customer_id"]);
                            cbbCustomer.Text = Convert.IsDBNull(header["customer_name"]) ? string.Empty : Convert.ToString(header["customer_name"]) == "0" ? "" : Convert.ToString(header["customer_name"]);
                            cbbPRType.Value = Convert.IsDBNull(header["purchase_request_type"]) ? string.Empty : Convert.ToString(header["purchase_request_type"]);
                            string status = Convert.IsDBNull(header["purchase_request_status"]) ? string.Empty : Convert.ToString(header["purchase_request_status"]);

                            if (cbbPRType.Value == "QU")
                            {
                                var quotationNo = Convert.IsDBNull(header["ref_doc_no"]) ? string.Empty : Convert.ToString(header["ref_doc_no"]);
                                var quotationId = Convert.IsDBNull(header["ref_doc_id"]) ? "0" : Convert.ToString(header["ref_doc_id"]);
                                cbbQuotation.Items.Insert(0, new ListEditItem { Text = quotationNo, Value = quotationId, Selected = true });
                            }
                            cbbQuotation.Value = Convert.IsDBNull(header["ref_doc_id"]) ? string.Empty : Convert.ToString(header["ref_doc_id"]);
                            cbbRequestFor.Value = Convert.IsDBNull(header["objective_type"]) ? string.Empty : Convert.ToString(header["objective_type"]);
                            txtDueDeliveryDate.Value = Convert.IsDBNull(header["due_date_delivery"]) ? string.Empty : Convert.ToString(Convert.ToDateTime(header["due_date_delivery"]).ToString("dd/MM/yyyy"));
                            txtPO.Value = Convert.IsDBNull(header["purchase_order_no"]) ? string.Empty : Convert.ToString(header["purchase_order_no"]);
                            txtPODate.Value = Convert.IsDBNull(header["purchase_order_date"]) ? string.Empty : Convert.ToString(Convert.ToDateTime(header["purchase_order_date"]).ToString("dd/MM/yyyy"));
                            txtPRDate.Value = Convert.IsDBNull(header["purchase_request_date"]) ? string.Empty : Convert.ToString(Convert.ToDateTime(header["purchase_request_date"]).ToString("dd/MM/yyyy"));
                            txtPRNo.Value = Convert.IsDBNull(header["purchase_request_no"]) ? string.Empty : Convert.ToString(header["purchase_request_no"]);
                            lbCustomerFirstName.Value = Convert.IsDBNull(header["customer_name"]) ? string.Empty : Convert.ToString(header["customer_name"]);
                            lbCustomerNo.Value = Convert.IsDBNull(header["customer_code"]) ? string.Empty : Convert.ToString(header["customer_code"]);
                            lbQuotationDate.Value = Convert.IsDBNull(header["ref_doc_date"]) ? string.Empty : Convert.ToString(Convert.ToDateTime(header["ref_doc_date"]).ToString("dd/MM/yyyy"));
                            hdCustomerCode.Value = Convert.IsDBNull(header["customer_code"]) ? string.Empty : Convert.ToString(header["customer_code"]);
                            hdCustomerId.Value = Convert.IsDBNull(header["customer_id"]) ? string.Empty : Convert.ToString(header["customer_id"]);
                            cbbSupplier.Value = Convert.IsDBNull(header["supplier_id"]) ? string.Empty : Convert.ToString(header["supplier_id"]);
                            hdDocumentStatus.Value = Convert.IsDBNull(header["purchase_request_status"]) ? string.Empty : Convert.ToString(header["purchase_request_status"]);
                            //hdProductType.Value = Convert.IsDBNull(header["ref_product_type"]) ? string.Empty : Convert.ToString(header["ref_product_type"]);
                            if (cbbPRType.Value == "P")
                            {
                                dvProductCust.Attributes["style"] = "display:''";
                                dvQuotation.Attributes["style"] = "display:none";
                                dvQuotation2.Attributes["style"] = "display:none";
                            }
                            else
                            {
                                dvProductCust.Attributes["style"] = "display:none";
                                dvQuotation.Attributes["style"] = "display:''";
                                dvQuotation2.Attributes["style"] = "display:''";
                            }

                            var dsQuotationData = new DataSet();
                            if (!Convert.IsDBNull(header["ref_doc_id"]))
                            {
                                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                                {
                                    //Create array of Parameters
                                    List<SqlParameter> arrParm = new List<SqlParameter>
                                {
                                    new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(header["ref_doc_id"]) },
                                };
                                    conn.Open();
                                    dsQuotationData = SqlHelper.ExecuteDataset(conn, "sp_quotation_list_data", arrParm.ToArray());
                                    conn.Close();
                                }
                                if (dsQuotationData.Tables.Count > 0)
                                {
                                    #region Quotation Detail 
                                    // Quotation Detail List
                                    foreach (var row in dsQuotationData.Tables[1].AsEnumerable())
                                    {
                                        quotationDetailList.Add(new QuotationDetail()
                                        {
                                            //delivery_note_no = Convert.ToString(row["delivery_note_no"]),

                                            discount_amount = Convert.IsDBNull(row["discount_amount"]) ? 0 : Convert.ToDecimal(row["discount_amount"]),
                                            discount_percentage = Convert.IsDBNull(row["discount_percentage"]) ? 0 : Convert.ToDecimal(row["discount_percentage"]),
                                            discount_type = Convert.IsDBNull(row["discount_type"]) ? string.Empty : Convert.ToString(row["discount_type"]),
                                            id = Convert.ToInt32(row["id"]),
                                            //issue_stock_no = Convert.ToString(row["issue_stock_no"]),
                                            //min_qty = Convert.IsDBNull(row["min_qty"]) ? 0 : Convert.ToInt32(row["min_qty"]),
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
                                            product_type = Convert.IsDBNull(row["product_type"]) ? string.Empty : Convert.ToString(row["product_type"]),

                                        });
                                    }
                                    #endregion Quotation Detail

                                }
                            }
                        }
                        var dataType = (from t in dsPurchaseRequest.Tables[1].AsEnumerable() select t).FirstOrDefault();

                        cbbProductType.Value = Convert.IsDBNull(dataType["item_type"]) ? string.Empty : Convert.ToString(dataType["item_type"]);
                        hdProductType.Value = Convert.IsDBNull(dataType["item_type"]) ? string.Empty : Convert.ToString(dataType["item_type"]);
                        var detail = (from t in dsPurchaseRequest.Tables[1].AsEnumerable() select t).ToList();

                        if (detail != null)
                        {
                            purchaseRequestDetailList = new List<PurchaseRequestDetail>();
                            foreach (var row in detail)
                            {
                                PurchaseRequestDetail obj = new PurchaseRequestDetail()
                                {
                                    id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                    sort_no = Convert.IsDBNull(row["sort_no"]) ? 0 : Convert.ToInt32(row["sort_no"]),
                                    is_deleted = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                    item_description = Convert.IsDBNull(row["item_description"]) ? string.Empty : Convert.ToString(row["item_description"]),
                                    item_id = Convert.IsDBNull(row["item_id"]) ? 0 : Convert.ToInt32(row["item_id"]),
                                    item_no = Convert.IsDBNull(row["item_no"]) ? string.Empty : Convert.ToString(row["item_no"]),
                                    item_type = Convert.IsDBNull(row["item_type"]) ? string.Empty : Convert.ToString(row["item_type"]),
                                    item_unit = Convert.IsDBNull(row["item_unit"]) ? string.Empty : Convert.ToString(row["item_unit"]),
                                    purchaseOrderNo = Convert.IsDBNull(row["ref_purchase_order_no_sup"]) ? string.Empty : Convert.ToString(row["ref_purchase_order_no_sup"]),
                                    receiveNo = Convert.IsDBNull(row["ref_receive_no"]) ? string.Empty : Convert.ToString(row["ref_receive_no"]),

                                    purchase_request_no = Convert.IsDBNull(row["purchase_request_no"]) ? string.Empty : Convert.ToString(row["purchase_request_no"]),
                                    qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["qty"]),
                                    old_qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["old_qty"]),
                                    receive_by = Convert.IsDBNull(row["receive_by"]) ? 0 : Convert.ToInt32(row["receive_by"]),
                                    receive_date = Convert.IsDBNull(row["receive_date"]) ? DateTime.Now : Convert.ToDateTime(row["receive_date"]),
                                    receive_qty = 0,//clear ทุกครั้งที่ Load//Convert.IsDBNull(row["receive_qty"]) ? 0 : Convert.ToInt32(row["receive_qty"]),
                                    display_receive_date = Convert.IsDBNull(row["receive_date"]) ? string.Empty : Convert.ToDateTime(row["receive_date"]).ToString("dd/MM/yyyy"),
                                    receive_qty_balance = Convert.IsDBNull(row["receive_qty_balance"]) ? 0 : Convert.ToInt32(row["receive_qty_balance"]),
                                    is_receive_all = Convert.IsDBNull(row["is_receive_all"]) ? false : Convert.ToBoolean(row["is_receive_all"]),
                                    supplier_id = Convert.IsDBNull(row["supplier_id"]) ? 0 : Convert.ToInt32(row["supplier_id"]),
                                    supplier_name = Convert.IsDBNull(row["supplier_name"]) ? string.Empty : Convert.ToString(row["supplier_name"]),

                                    history_log = Convert.IsDBNull(row["history_log"]) ? string.Empty : Convert.ToString(row["history_log"]),
                                };
                                //obj.receive_qty = obj.qty - obj.receive_qty_balance;

                                var receive_qty_temp = Convert.IsDBNull(row["receive_qty_temp"]) ? 0 : Convert.ToInt32(row["receive_qty_temp"]);
                                obj.receive_qty = receive_qty_temp == 0 ? 0 : receive_qty_temp;
                                obj.remain_qty = obj.qty - obj.receive_qty_balance;
                                purchaseRequestDetailList.Add(obj);
                            }
                        }
                        var mfgDetail = (from t in dsPurchaseRequest.Tables[2].AsEnumerable() select t).ToList();
                        if (mfgDetail != null)
                        {
                            prDetailMFGList = new List<PurchaseRequestDetailMFG>();
                            foreach (var row in mfgDetail)
                            {
                                prDetailMFGList.Add(new PurchaseRequestDetailMFG()
                                {
                                    id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                    is_deleted = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                    mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                    old_mfg_no = Convert.IsDBNull(row["old_mfg_no"]) ? string.Empty : Convert.ToString(row["old_mfg_no"]),
                                    pr_detail_id = Convert.IsDBNull(row["pr_detail_id"]) ? 0 : Convert.ToInt32(row["pr_detail_id"]),
                                    pr_no = Convert.IsDBNull(row["pr_no"]) ? string.Empty : Convert.ToString(row["pr_no"]),
                                    item_id = Convert.IsDBNull(row["item_id"]) ? 0 : Convert.ToInt32(row["item_id"]),
                                    receive_no = Convert.IsDBNull(row["pr_no"]) ? string.Empty : Convert.ToString(row["receive_no"]),
                                    is_in_stock = DBNull.Value.Equals(row["is_in_stock"]) ? false : Convert.ToBoolean(row["is_in_stock"]),
                                    display_receive_date = Convert.IsDBNull(row["receive_date"]) ? string.Empty : Convert.ToString(Convert.ToDateTime(row["receive_date"]).ToString("dd/MM/yyyy")),
                                    receive_date = Convert.IsDBNull(row["receive_date"]) ? DateTime.Now : Convert.ToDateTime(row["receive_date"])
                                });
                            }
                        }
                        #endregion
                    }
                    gridViewPR.DataSource = purchaseRequestDetailList;
                    gridViewPR.DataBind();

                    cbbSupplier_Callback(cbbSupplier, null);

                    // Set Grid Column "Receive and MFG" depend on Status
                    gridViewPR.Columns["item_Receive"].Visible = true;
                    if (cbbProductType.Value == "P")
                        gridViewPR.Columns["mfg_detail"].Visible = true;
                    else
                        gridViewPR.Columns["mfg_detail"].Visible = false;
                    if (hdDocumentStatus.Value == "CP")
                    {
                        btnDraft.Visible = false;
                        btnAddPRDetail.Visible = false;
                        btnSave.Visible = false;
                        gridViewPR.Columns["id"].Visible = false;
                        gridViewPR.Columns["item_Receive"].Visible = false;

                        txtPO.Attributes["disabled"] = "disabled";
                        txtPODate.Attributes["disabled"] = "disabled";
                        txtDueDeliveryDate.Attributes["disabled"] = "disabled";
                        cbbRequestFor.Attributes["disabled"] = "disabled";
                        cbbQuotation.Enabled = false;
                        cbbCustomer.Enabled = false;
                        cbbSupplier.Enabled = false;

                        btnNew.Visible = true;
                    }
                    else
                    {
                        //btnDraft.Visible = false;
                        btnAddPRDetail.Visible = true;

                        btnNew.Visible = false;
                    }
                    //********** End Set Grid Column *****************
                    cbbPRType.Attributes["disabled"] = "disabled";
                    cbbProductType.Attributes["disabled"] = "disabled";
                }
                else
                {
                    dvProductCust.Attributes["style"] = "display:none";
                    dvQuotation.Attributes["style"] = "display:''";
                    dvQuotation2.Attributes["style"] = "display:''";
                    gridViewPR.Columns["mfg_detail"].Visible = false;
                    gridViewPR.Columns["item_Receive"].Visible = false;
                    gridViewPR.Columns["receive_qty"].Visible = false;
                    btnReportClient.Visible = false;
                    btnSave.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gridViewQuotationDetail_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var splitStr = e.Parameters.ToString().Split('|');
            string searchText = string.Empty;
            if (splitStr.Length > 1)
            {
                searchText = splitStr[1];
            }
            if (string.IsNullOrEmpty(searchText))
            {
                gridViewQuotationDetail.DataSource = quotationDetailList;
                gridViewQuotationDetail.DataBind();
            }
            else
            {
                gridViewQuotationDetail.DataSource = (from t in quotationDetailList
                                                      where (t.product_no.ToUpper().Contains(searchText.ToUpper()) || t.quotation_description.ToUpper().Contains(searchText.ToUpper()))
                                                      select t).ToList();
                gridViewQuotationDetail.DataBind();
            }
            gridViewQuotationDetail.PageIndex = 0;
        }
        protected void BindGridViewQuotationDetail()
        {
            var searchText = txtSearchQuotation.Value;
            if (string.IsNullOrEmpty(searchText))
            {
                gridViewQuotationDetail.DataSource = quotationDetailList;
            }
            else
            {
                gridViewQuotationDetail.DataSource = (from t in quotationDetailList
                                                      where (t.product_no.ToUpper().Contains(searchText.ToUpper()) || t.quotation_description.ToUpper().Contains(searchText.ToUpper()))
                                                      select t).ToList();
                gridViewQuotationDetail.DataBind();
            }
        }
        [WebMethod]
        public static QuotationData GetQuotationData(string id)
        {

            try
            {
                var headerData = new QuotationData();
                var dsResult = new DataSet();
                var quotationDetailList = new List<QuotationDetail>(); //(List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_PR"];
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },
                        };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_quotation_list_data_pr", arrParm.ToArray());
                    conn.Close();
                }
                if (dsResult.Tables.Count > 0)
                {
                    #region Quotation Header
                    // Quotation Header
                    var data = dsResult.Tables[0].AsEnumerable().FirstOrDefault();
                    if (data != null)
                    {
                        headerData.display_quotation_date = Convert.ToString(Convert.ToDateTime(data["quotation_date"]).ToString("dd/MM/yyyy"));
                        headerData.quotation_no = Convert.IsDBNull(data["quotation_no"]) ? string.Empty : Convert.ToString(data["quotation_no"]);

                        headerData.customer_id = Convert.IsDBNull(data["customer_id"]) ? 0 : Convert.ToInt32(data["customer_id"]);
                        headerData.quotation_type = Convert.IsDBNull(data["quotation_type"]) ? string.Empty : Convert.ToString(data["quotation_type"]);
                        headerData.quotation_id = Convert.IsDBNull(data["id"]) ? 0 : Convert.ToInt32(data["id"]);

                        headerData.customer_code = Convert.IsDBNull(data["customer_code"]) ? string.Empty : Convert.ToString(data["customer_code"]);
                        headerData.customer_name = Convert.IsDBNull(data["customer_name"]) ? string.Empty : Convert.ToString(data["customer_name"]);

                        headerData.po_no = Convert.IsDBNull(data["ref_po_no"]) ? string.Empty : Convert.ToString(data["ref_po_no"]);
                        headerData.po_date = Convert.IsDBNull(data["ref_po_date"]) ? string.Empty : Convert.ToString(Convert.ToDateTime(data["ref_po_date"]).ToString("dd/MM/yyyy"));
                    }
                    #endregion Quotation Header

                    #region Quotation Detail 
                    // Quotation Detail List
                    foreach (var row in dsResult.Tables[1].AsEnumerable())
                    {
                        quotationDetailList.Add(new QuotationDetail()
                        {
                            //delivery_note_no = Convert.ToString(row["delivery_note_no"]),

                            discount_amount = Convert.IsDBNull(row["discount_amount"]) ? 0 : Convert.ToDecimal(row["discount_amount"]),
                            discount_percentage = Convert.IsDBNull(row["discount_percentage"]) ? 0 : Convert.ToDecimal(row["discount_percentage"]),
                            discount_type = Convert.IsDBNull(row["discount_type"]) ? string.Empty : Convert.ToString(row["discount_type"]),
                            id = Convert.ToInt32(row["id"]),
                            //issue_stock_no = Convert.ToString(row["issue_stock_no"]),
                            //min_qty = Convert.IsDBNull(row["min_qty"]) ? 0 : Convert.ToInt32(row["min_qty"]),
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
                            product_type = Convert.IsDBNull(row["product_type"]) ? string.Empty : Convert.ToString(row["product_type"]),

                        });
                    }
                    #endregion Quotation Detail

                    HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_PR"] = quotationDetailList;
                }

                HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"] = new List<PurchaseRequestDetail>();
                return headerData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void cbbSupplier_Callback(object sender, CallbackEventArgsBase e)
        {
            var param_quotation_no = "";
            var item_type = "";
            if (cbbPRType.Value == "P")
            {
                item_type = cbbProductType.Value;
            }
            else
            {
                param_quotation_no = (from t in quotationDetailList select t.quotation_no).FirstOrDefault();
            }
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                List<SqlParameter> arrParm = new List<SqlParameter>
                {
                    new SqlParameter("@quotation_no", SqlDbType.VarChar,20) { Value = param_quotation_no },
                    new SqlParameter("@item_type", SqlDbType.VarChar,1) { Value = item_type },
                };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_supplier_qu", arrParm.ToArray());
                conn.Close();
                cbbSupplier.DataSource = dsResult;
                cbbSupplier.TextField = "data_text";
                cbbSupplier.ValueField = "data_value";
                cbbSupplier.DataBind();
            }

        }

        private void Dropdrow()
        {
            /////////////////dropdrow Supplier

        }

        [WebMethod]
        public static string GetProductDetailData(int qu_id, int supplier_id, string product_type)
        {
            if (qu_id == 0)
            {
                try
                {

                    var prDetailData = new List<PurchaseRequestDetail>();//(List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"];

                    var data = new List<ProductDetail>();
                    var dsResult = new DataSet();
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@supplier_id", SqlDbType.Int) { Value = supplier_id },
                            new SqlParameter("@product_type", SqlDbType.VarChar,2) { Value = product_type },
                        };
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {

                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_union_spare_part_list", arrParm.ToArray());
                        conn.Close();
                    }
                    if (dsResult != null)
                    {
                        var productData = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                        if (productData != null)
                        {
                            foreach (var row in productData)
                            {
                                data.Add(new ProductDetail()
                                {
                                    is_selected = false,
                                    item_type = Convert.IsDBNull(row["item_type"]) ? string.Empty : Convert.ToString(row["item_type"]),
                                    product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                    product_name = Convert.IsDBNull(row["product_name"]) ? string.Empty : Convert.ToString(row["product_name"]),
                                    product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                    qty = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]),
                                    qty_reserve = Convert.IsDBNull(row["quantity_reserve"]) ? 0 : Convert.ToInt32(row["quantity_reserve"]),
                                    item_unit = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]),
                                    supplier_id = Convert.IsDBNull(row["supplier_id"]) ? 0 : Convert.ToInt32(row["supplier_id"]),
                                    supplier_name = Convert.IsDBNull(row["supplier_name"]) ? string.Empty : Convert.ToString(row["supplier_name"]),
                                });
                            }
                        }
                    }
                    HttpContext.Current.Session["SESSION_PRODUCT_DETAIL_PR"] = data;
                    HttpContext.Current.Session["SESSION_SELECTED_PURCHASE_REQUEST_DETAIL_PR"] = prDetailData;
                    return "PR_DATA";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    var dsResult = new DataSet();
                    var quotationDetailList = new List<QuotationDetail>(); //(List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_PR"];
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = qu_id },
                        };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_quotation_list_data_pr", arrParm.ToArray());
                        conn.Close();
                    }
                    if (dsResult.Tables.Count > 0)
                    {

                        #region Quotation Detail 
                        // Quotation Detail List
                        foreach (var row in dsResult.Tables[1].AsEnumerable())
                        {
                            if (supplier_id == Convert.ToInt32(row["supplier_id"]))
                            {
                                int product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]);
                                var selectedRow = (from t in quotationDetailList
                                                   where t.product_id == product_id
                                                   select t).FirstOrDefault();
                                if (selectedRow == null)
                                {
                                    quotationDetailList.Add(new QuotationDetail()
                                    {
                                        //delivery_note_no = Convert.ToString(row["delivery_note_no"]),

                                        discount_amount = Convert.IsDBNull(row["discount_amount"]) ? 0 : Convert.ToDecimal(row["discount_amount"]),
                                        discount_percentage = Convert.IsDBNull(row["discount_percentage"]) ? 0 : Convert.ToDecimal(row["discount_percentage"]),
                                        discount_type = Convert.IsDBNull(row["discount_type"]) ? string.Empty : Convert.ToString(row["discount_type"]),
                                        id = Convert.ToInt32(row["id"]),
                                        //issue_stock_no = Convert.ToString(row["issue_stock_no"]),
                                        //min_qty = Convert.IsDBNull(row["min_qty"]) ? 0 : Convert.ToInt32(row["min_qty"]),
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
                                        product_type = Convert.IsDBNull(row["product_type"]) ? string.Empty : Convert.ToString(row["product_type"]),
                                        supplier_id = Convert.IsDBNull(row["supplier_id"]) ? 0 : Convert.ToInt32(row["supplier_id"]),
                                        supplier_name = Convert.IsDBNull(row["supplier_name"]) ? string.Empty : Convert.ToString(row["supplier_name"]),
                                    });
                                }
                                else
                                {
                                    int qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["qty"]);
                                    selectedRow.qty += qty;
                                }
                            }
                        }
                        #endregion Quotation Detail

                        HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_PR"] = quotationDetailList;
                        return "PR_DATA";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return "ERR";
        }
        [WebMethod]
        public static string AddSelectedProduct(string key, bool isSelected, string type, string product_type)
        {
            try
            {
                var productDetail = (List<ProductDetail>)HttpContext.Current.Session["SESSION_PRODUCT_DETAIL_PR"];
                var quotationDetail = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_PR"];
                var selectedPRData = (List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_SELECTED_PURCHASE_REQUEST_DETAIL_PR"];
                var prDetailData = (List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"];
                var diffCount = prDetailData.Count;

                if (type == "P")
                {
                    if (productDetail != null) // ต้องหา Row เจอเท่านั้น ถึงจะทำต่อ
                    {
                        var selectedProductRow = (from t in productDetail
                                                  where t.product_no == Convert.ToString(key)
                                                  select t).FirstOrDefault();
                        if (selectedProductRow != null)
                        {
                            if (isSelected)
                            {
                                selectedProductRow.is_selected = true;
                                if (selectedPRData != null) // ถ้ามี PRDetail อยุ่แล้ว
                                {
                                    var checkExist = (from t in selectedPRData where t.item_no == Convert.ToString(key) select t).FirstOrDefault();
                                    if (checkExist == null)
                                    {
                                        selectedPRData.Add(new PurchaseRequestDetail()
                                        {
                                            id = (selectedPRData.Count + 1 + diffCount) * -1,
                                            sort_no = (selectedPRData.Count + 1 + diffCount),
                                            is_deleted = false,
                                            item_description = selectedProductRow.product_name,
                                            item_id = selectedProductRow.product_id,
                                            qty = 1,//(selectedProductRow.qty_reserve <= selectedProductRow.qty ? 1 : selectedProductRow.qty_reserve - selectedProductRow.qty),
                                            item_no = selectedProductRow.product_no,
                                            item_unit = selectedProductRow.item_unit,
                                            item_type = product_type,
                                            supplier_id = selectedProductRow.supplier_id,
                                            supplier_name = selectedProductRow.supplier_name,
                                        });
                                    }
                                    else // ไม่ต้องทำอะไร
                                    {

                                    }
                                }
                                else // ใหม่เอี่ยม
                                {
                                    selectedPRData = new List<PurchaseRequestDetail>();
                                    selectedPRData.Add(new PurchaseRequestDetail()
                                    {
                                        id = (selectedPRData.Count + 1 + diffCount) * -1,
                                        sort_no = (selectedPRData.Count + 1 + diffCount),
                                        is_deleted = false,
                                        item_description = selectedProductRow.product_name,
                                        item_id = selectedProductRow.product_id,
                                        qty = 1,//(selectedProductRow.qty_reserve <= selectedProductRow.qty ? 1 : selectedProductRow.qty_reserve - selectedProductRow.qty),
                                        item_no = selectedProductRow.product_no,
                                        item_unit = selectedProductRow.item_unit,
                                        item_type = product_type,
                                        supplier_id = selectedProductRow.supplier_id,
                                        supplier_name = selectedProductRow.supplier_name,
                                    });
                                }
                            }
                            else
                            {
                                selectedProductRow.is_selected = false;
                                var checkExist = (from t in selectedPRData where t.item_no == Convert.ToString(key) select t).FirstOrDefault();
                                if (checkExist != null) // ต้องเข้า case นี้ เท่านั้น
                                {
                                    if (checkExist.id < 0)
                                    {
                                        selectedPRData.Remove(checkExist);
                                    }
                                    else
                                    {
                                        checkExist.is_deleted = true;
                                    }
                                }
                                else
                                {

                                }

                            }
                        }
                    }
                }
                else if (type == "QU")
                {
                    if (quotationDetail != null) // ต้องหา Row เจอเท่านั้น ถึงจะทำต่อ
                    {
                        var selectedQuotationRow = (from t in quotationDetail
                                                    where t.product_no == Convert.ToString(key)
                                                    select t).FirstOrDefault();
                        if (selectedQuotationRow != null)
                        {
                            if (isSelected)
                            {
                                selectedQuotationRow.is_selected = true;
                                if (selectedPRData != null) // ถ้ามี PRDetail อยุ่แล้ว
                                {
                                    var checkExist = (from t in selectedPRData where t.item_no == Convert.ToString(key) select t).FirstOrDefault();
                                    if (checkExist == null)
                                    {
                                        selectedPRData.Add(new PurchaseRequestDetail()
                                        {
                                            id = (selectedPRData.Count + 1 + diffCount) * -1,
                                            sort_no = (selectedPRData.Count + 1 + diffCount),
                                            is_deleted = false,
                                            item_description = selectedQuotationRow.quotation_description,
                                            item_id = selectedQuotationRow.product_id,
                                            qty = selectedQuotationRow.qty,
                                            receive_qty = selectedQuotationRow.qty,
                                            item_no = selectedQuotationRow.product_no,
                                            item_unit = selectedQuotationRow.unit_code,
                                            item_type = selectedQuotationRow.product_type,
                                            supplier_id = selectedQuotationRow.supplier_id,
                                            supplier_name = selectedQuotationRow.supplier_name,
                                        });
                                    }
                                    else // ไม่ต้องทำอะไร
                                    {

                                    }
                                }
                                else // ใหม่เอี่ยม
                                {
                                    selectedPRData = new List<PurchaseRequestDetail>();
                                    selectedPRData.Add(new PurchaseRequestDetail()
                                    {
                                        id = (selectedPRData.Count + 1 + diffCount) * -1,
                                        sort_no = (selectedPRData.Count + 1 + diffCount),
                                        is_deleted = false,
                                        item_description = selectedQuotationRow.quotation_description,
                                        item_id = selectedQuotationRow.product_id,
                                        qty = selectedQuotationRow.qty,
                                        receive_qty = selectedQuotationRow.qty,
                                        item_no = selectedQuotationRow.product_no,
                                        item_unit = selectedQuotationRow.unit_code,
                                        item_type = selectedQuotationRow.product_type,
                                        supplier_id = selectedQuotationRow.supplier_id,
                                        supplier_name = selectedQuotationRow.supplier_name,
                                    });
                                }
                            }
                            else
                            {
                                selectedQuotationRow.is_selected = false;
                                var checkExist = (from t in selectedPRData where t.item_no == Convert.ToString(key) select t).FirstOrDefault();
                                if (checkExist != null) // ต้องเข้า case นี้ เท่านั้น
                                {
                                    if (checkExist.id < 0)
                                    {
                                        selectedPRData.Remove(checkExist);
                                    }
                                    else
                                    {
                                        checkExist.is_deleted = true;
                                    }
                                }
                                else
                                {

                                }

                            }
                        }
                    }
                }
                HttpContext.Current.Session["SESSION_SELECTED_PURCHASE_REQUEST_DETAIL_PR"] = selectedPRData;
                HttpContext.Current.Session["SESSION_PRODUCT_DETAIL_PR"] = productDetail;
                HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_PR"] = quotationDetail;

                return "SUCCESS";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        [WebMethod]
        public static string SubmitProductDetail(string type)
        {
            try
            {
                var selectedPRData = (List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_SELECTED_PURCHASE_REQUEST_DETAIL_PR"];
                var prDetailData = (List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"];
                if (selectedPRData != null)
                {
                    //prDetailData = new List<PurchaseRequestDetail>();
                    //prDetailData = selectedPRData;
                    prDetailData.AddRange(selectedPRData);
                }
                HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"] = prDetailData;
                HttpContext.Current.Session["SESSION_SELECTED_PURCHASE_REQUEST_DETAIL_PR"] = null;

                return "SUCCESS";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        protected void gridViewProductDetail_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var splitStr = e.Parameters.ToString().Split('|');
            string searchText = string.Empty;
            string issueType = string.Empty;
            if (splitStr.Length > 1)
            {
                searchText = splitStr[1];
            }
            if (string.IsNullOrEmpty(searchText))
            {
                gridViewProductDetail.DataSource = productDetailList;
                gridViewProductDetail.DataBind();
            }
            else
            {
                gridViewProductDetail.DataSource = (from t in productDetailList
                                                    where (t.product_no.ToUpper().Contains(searchText.ToUpper()) || t.product_name.ToUpper().Contains(searchText.ToUpper()))
                                                    select t).ToList();
                gridViewProductDetail.DataBind();
            }
            gridViewProductDetail.PageIndex = 0;
        }
        protected void BindGridViewProductDetail()
        {
            string searchText = txtSearchProduct.Value;
            if (string.IsNullOrEmpty(searchText))
            {
                gridViewProductDetail.DataSource = productDetailList;
            }
            else
            {
                gridViewProductDetail.DataSource = (from t in productDetailList
                                                    where (t.product_no.ToUpper().Contains(searchText.ToUpper()) || t.product_name.ToUpper().Contains(searchText.ToUpper()))
                                                    select t).ToList();
            }
            gridViewProductDetail.DataBind();
        }
        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            SaveData("FL");
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            SaveData("CF");
        }

        protected void gridViewPR_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.ToString().Contains("Search"))
            {

                var splitStr = e.Parameters.ToString().Split('|');
                string searchText = string.Empty;
                if (splitStr.Length > 0)
                {
                    searchText = splitStr[1];

                    gridViewPR.DataSource = purchaseRequestDetailList.Where(t => t.item_description.ToUpper().Contains(searchText.ToUpper())
                                                                                    || t.item_no.ToUpper().Contains(searchText.ToUpper())).OrderBy(t => t.sort_no).ToList();
                    gridViewPR.DataBind();
                }
            }
            else
            {
                gridViewPR.DataSource = (from t in purchaseRequestDetailList where t.is_deleted == false orderby t.sort_no ascending select t).ToList();
                gridViewPR.DataBind();
            }
        }
        protected void BindGridViewPR()
        {
            gridViewPR.DataSource = (from t in purchaseRequestDetailList where t.is_deleted == false select t).ToList();
            gridViewPR.DataBind();
        }
        [WebMethod]
        public static string DeletePRDetail(string id)
        {
            try
            {
                var prDetailData = (List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"];
                if (prDetailData != null)
                {
                    var row = (from t in prDetailData where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (row != null)
                    {
                        if (row.id < 0)
                        {
                            prDetailData.Remove(row);
                        }
                        else
                        {
                            row.is_deleted = true;
                        }
                    }
                }
                HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"] = prDetailData;

                return "SUCCESS";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [WebMethod]
        public static string DeleteMFG(string id)
        {
            try
            {
                var prDetailDataMFG = (List<PurchaseRequestDetailMFG>)HttpContext.Current.Session["SESSION_SELECTED_PR_DETAIL_MFG_PR"];
                if (prDetailDataMFG != null)
                {
                    var row = (from t in prDetailDataMFG where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (row != null)
                    {
                        //if (row.id < 0)
                        //{
                        //    prDetailDataMFG.Remove(row);
                        //}
                        //else
                        //{
                        //    row.is_deleted = true;
                        //}
                        row.is_in_stock = false;
                        row.is_deleted = true;
                    }
                }
                HttpContext.Current.Session["SESSION_SELECTED_PR_DETAIL_MFG_PR"] = prDetailDataMFG;

                return "SUCCESS";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [WebMethod]
        public static PurchaseRequestDetail EditPRDetail(string id)
        {
            try
            {
                var prDetailData = (List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"];
                if (prDetailData != null)
                {
                    var row = (from t in prDetailData where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (row != null)
                    {
                        row.display_receive_date = row.receive_date.HasValue ? row.receive_date.Value.ToString("dd/MM/yyyy") : string.Empty;
                        return row;
                    }

                }
                return new PurchaseRequestDetail();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [WebMethod]
        public static string SubmitPRDetail(string id, string qty, string receive_qty, string receive_date, string purchaseOrderNo, string receiveNo, Boolean isAllCheck)
        {
            try
            {

                var prDetailData = (List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"];
                if (prDetailData != null)
                {
                    var row = (from t in prDetailData where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (row != null)
                    {
                        DateTime? recieve_dt = null;
                        if (!string.IsNullOrEmpty(Convert.ToString(receive_date)))
                        {
                            recieve_dt = DateTime.ParseExact(Convert.ToString(receive_date).Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        }
                        row.qty = Convert.ToInt32(qty);
                        row.receive_qty = Convert.ToInt32(receive_qty);
                        row.receive_date = recieve_dt;
                        row.purchaseOrderNo = purchaseOrderNo == "" ? string.Empty : purchaseOrderNo;
                        row.receiveNo = receiveNo == "" ? string.Empty : receiveNo;
                    }

                    if (isAllCheck)
                    {
                        string[] list = receiveNo.Split(',');
                        var items = (from t in prDetailData where t.qty != t.receive_qty_balance select t).ToList();
                        foreach (var item in items)
                        {
                            DateTime? recieve_dt = null;
                            if (!string.IsNullOrEmpty(Convert.ToString(receive_date)))
                            {
                                recieve_dt = DateTime.ParseExact(Convert.ToString(receive_date).Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            }
                            item.receive_date = recieve_dt;
                            item.purchaseOrderNo = purchaseOrderNo == "" ? string.Empty : purchaseOrderNo;
                            //item.receiveNo = receiveNo == "" ? string.Empty : receiveNo;
                            foreach (var obj in list)
                            {
                                if (!item.receiveNo.Contains(obj.Trim()))
                                {
                                    if (item.receiveNo != "")
                                    {
                                        item.receiveNo += ",";
                                    }
                                    item.receiveNo += obj.Trim();
                                }
                            }
                        }
                    }
                }
                HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"] = prDetailData;
                return "SUCCESS";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        private void SaveData(string status)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                int newID = DataListUtil.emptyEntryID;
                var prNo = string.Empty;
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        if (dataId == 0)
                        {
                            #region PR Header
                            //Quotation Header
                            using (SqlCommand cmd = new SqlCommand("sp_purchase_request_header_add", conn, tran))
                            {
                                DateTime? ref_doc_date = null;
                                DateTime? po_date = null;
                                DateTime? due_date = null;
                                if (!string.IsNullOrEmpty(lbQuotationDate.Value))
                                {
                                    ref_doc_date = DateTime.ParseExact(lbQuotationDate.Value.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                }
                                if (!string.IsNullOrEmpty(txtPODate.Value))
                                {
                                    po_date = DateTime.ParseExact(txtPODate.Value.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                }
                                if (!string.IsNullOrEmpty(txtDueDeliveryDate.Value))
                                {
                                    due_date = DateTime.ParseExact(txtDueDeliveryDate.Value.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                }
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@purchase_request_type", SqlDbType.VarChar, 2).Value = cbbPRType.Value;
                                cmd.Parameters.Add("@purchase_request_status", SqlDbType.VarChar, 2).Value = status;
                                cmd.Parameters.Add("@purchase_order_no", SqlDbType.VarChar, 20).Value = txtPO.Value;
                                cmd.Parameters.Add("@purchase_order_date", SqlDbType.Date).Value = po_date;
                                cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = cbbSupplier.Value == null ? 0 : Convert.ToInt32(cbbSupplier.Value);
                                cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = cbbPRType.Value == "P" ? 0 : cbbQuotation.Value; ;
                                cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = cbbPRType.Value == "P" ? string.Empty : cbbQuotation.Text;
                                if (cbbPRType.Value == "P")
                                {
                                    cmd.Parameters.Add("@ref_doc_date", SqlDbType.Date).Value = DBNull.Value;
                                }
                                else
                                {
                                    cmd.Parameters.Add("@ref_doc_date", SqlDbType.Date).Value = ref_doc_date;
                                }
                                var customer_id = cbbPRType.Value == "P" ? cbbCustomer.Value ?? 0 : Convert.ToInt32(hdCustomerId.Value);
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customer_id;//cbbPRType.Value == "P" ? cbbCustomer.Value ?? 0 : Convert.ToInt32(hdCustomerId.Value);
                                cmd.Parameters.Add("@customer_code", SqlDbType.VarChar, 20).Value = cbbPRType.Value == "P" ? hdCustomerCode.Value : lbCustomerNo.Value;
                                cmd.Parameters.Add("@customer_name", SqlDbType.VarChar, 100).Value = cbbPRType.Value == "P" ? cbbCustomer.Text : lbCustomerFirstName.Value;
                                cmd.Parameters.Add("@request_for", SqlDbType.VarChar, 100).Value = string.Empty;//cbbPRType.Value == "P" ? cbbQuotation.Text : lbCustomerFirstName.Value;
                                cmd.Parameters.Add("@customer_po_no", SqlDbType.VarChar, 50).Value = string.Empty;//cbbPRType.Value == "P" ? cbbQuotation.Text : lbCustomerFirstName.Value;
                                cmd.Parameters.Add("@due_date_delivery", SqlDbType.Date).Value = due_date;
                                cmd.Parameters.Add("@objective_type", SqlDbType.VarChar, 1).Value = cbbRequestFor.Value;
                                cmd.Parameters.Add("@sales_order_no", SqlDbType.VarChar, 100).Value = string.Empty;
                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = string.Empty;
                                cmd.Parameters.Add("@request_by", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@purchase_order_by", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@audit_by", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@authorize_by", SqlDbType.Int).Value = 0;

                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                newID = Convert.ToInt32(cmd.ExecuteScalar());

                            }
                            #endregion PR Header
                            List<SqlParameter> arrParm = new List<SqlParameter>
                            {
                                new SqlParameter("@id", SqlDbType.Int) { Value = newID },
                                new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = "" }

                            };

                            var lastDataInsert = SqlHelper.ExecuteDataset(tran, "sp_purchase_request_header_list", arrParm.ToArray());


                            if (lastDataInsert != null)
                            {
                                prNo = (from t in lastDataInsert.Tables[0].AsEnumerable()
                                        select t.Field<string>("purchase_request_no")).FirstOrDefault();
                            }
                            #region PR Detail

                            var oldDetailId = 0;
                            var newDetailId = 0;
                            foreach (var row in purchaseRequestDetailList)
                            {
                                DateTime? receive_date = null;
                                if (row.receive_date != null)
                                {
                                    receive_date = row.receive_date;
                                }
                                oldDetailId = row.id;
                                using (SqlCommand cmd = new SqlCommand("sp_purchase_request_detail_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@purchase_request_no", SqlDbType.VarChar, 20).Value = prNo;
                                    cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                    cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.item_id;
                                    cmd.Parameters.Add("@item_no", SqlDbType.VarChar, 50).Value = row.item_no;
                                    cmd.Parameters.Add("@item_description", SqlDbType.VarChar, 200).Value = row.item_description;
                                    cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 20).Value = row.item_type;
                                    cmd.Parameters.Add("@item_unit", SqlDbType.VarChar, 20).Value = row.item_unit;
                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                    cmd.Parameters.Add("@receive_qty", SqlDbType.Int).Value = 0;// row.receive_qty;
                                    cmd.Parameters.Add("@receive_date", SqlDbType.DateTime).Value = receive_date;
                                    cmd.Parameters.Add("@receive_by", SqlDbType.Int).Value = row.receive_by;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                    cmd.Parameters.Add("@purchaseOrderNo", SqlDbType.VarChar, 50).Value = row.purchaseOrderNo ?? "";
                                    cmd.Parameters.Add("@receiveNo", SqlDbType.VarChar, 50).Value = row.receiveNo ?? "";

                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = row.supplier_id;

                                    newDetailId = Convert.ToInt32(cmd.ExecuteScalar());

                                    var childData = (from t in prDetailMFGList where t.pr_detail_id == oldDetailId select t).ToList();
                                    foreach (var rowDetail in childData)
                                    {
                                        rowDetail.pr_detail_id = newDetailId;// + countId;
                                    }

                                }
                            }

                            #endregion

                            #region MFG Detail

                            foreach (var row in prDetailMFGList)
                            {
                                //throw new Exception("x");
                                using (SqlCommand cmd = new SqlCommand("sp_purchase_request_detail_mfg_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@pr_no", SqlDbType.VarChar, 20).Value = prNo;
                                    cmd.Parameters.Add("@pr_detail_id", SqlDbType.Int).Value = row.pr_detail_id;
                                    cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.item_id;
                                    cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;

                                    cmd.Parameters.Add("@receive_no", SqlDbType.VarChar, 20).Value = row.receive_no;
                                    var receive_date = DateTime.ParseExact(row.display_receive_date.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                    cmd.Parameters.Add("@receive_date", SqlDbType.Date).Value = receive_date;

                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                    cmd.ExecuteNonQuery();

                                }
                            }
                            #endregion

                            #region Transection

                            if (status == "CF")
                            {
                                foreach (var row in prDetailMFGList)
                                {

                                    using (SqlCommand cmd = new SqlCommand("sp_product_mfg_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;
                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;
                                        cmd.Parameters.Add("@purchase_request_no", SqlDbType.VarChar, 20).Value = prNo;
                                        cmd.Parameters.Add("@receive_date", SqlDbType.DateTime).Value = row.receive_date;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            newID = dataId;

                            #region PR Header
                            //Quotation Header
                            using (SqlCommand cmd = new SqlCommand("sp_purchase_request_header_edit", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                DateTime? ref_doc_date = null;
                                DateTime? po_date = null;
                                DateTime? due_date = null;
                                if (!string.IsNullOrEmpty(lbQuotationDate.Value))
                                {
                                    ref_doc_date = DateTime.ParseExact(lbQuotationDate.Value.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                }
                                if (!string.IsNullOrEmpty(txtPODate.Value))
                                {
                                    po_date = DateTime.ParseExact(txtPODate.Value.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                }
                                if (!string.IsNullOrEmpty(txtDueDeliveryDate.Value))
                                {
                                    due_date = DateTime.ParseExact(txtDueDeliveryDate.Value.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                }

                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = dataId;
                                cmd.Parameters.Add("@purchase_request_no", SqlDbType.VarChar, 20).Value = txtPRNo.Value;
                                cmd.Parameters.Add("@purchase_request_type", SqlDbType.VarChar, 2).Value = cbbPRType.Value;

                                var headerStatus = status;
                                if (headerStatus == "FL")
                                {
                                    if (hdDocumentStatus.Value == "PT")
                                    {
                                        headerStatus = "PT";
                                    }
                                    else
                                    {
                                        var obj = (from t in purchaseRequestDetailList where t.purchaseOrderNo == "" && !t.is_deleted select t).FirstOrDefault();
                                        if (obj == null)
                                        {
                                            headerStatus = "PO";
                                        }
                                    }
                                }
                                cmd.Parameters.Add("@purchase_request_status", SqlDbType.VarChar, 2).Value = headerStatus;

                                cmd.Parameters.Add("@purchase_order_no", SqlDbType.VarChar, 20).Value = txtPO.Value;
                                cmd.Parameters.Add("@purchase_order_date", SqlDbType.Date).Value = po_date;
                                cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = cbbSupplier.Value == null ? 0 : Convert.ToInt32(cbbSupplier.Value);
                                cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = cbbPRType.Value == "P" ? 0 : Convert.ToInt32(cbbQuotation.Value);
                                cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = cbbPRType.Value == "P" ? string.Empty : cbbQuotation.Text;
                                if (cbbPRType.Value == "P")
                                {
                                    cmd.Parameters.Add("@ref_doc_date", SqlDbType.Date).Value = DBNull.Value;
                                }
                                else
                                {
                                    cmd.Parameters.Add("@ref_doc_date", SqlDbType.Date).Value = ref_doc_date;
                                }

                                var customer_id = hdCustomerId.Value;// cbbPRType.Value == "P" ? cbbCustomer.Value ?? 0 : Convert.ToInt32(hdCustomerId.Value);
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customer_id;//cbbPRType.Value == "P" ? cbbCustomer.Value ?? 0 : Convert.ToInt32(hdCustomerId.Value);
                                cmd.Parameters.Add("@customer_code", SqlDbType.VarChar, 20).Value = cbbPRType.Value == "P" ? hdCustomerCode.Value : lbCustomerNo.Value;
                                cmd.Parameters.Add("@customer_name", SqlDbType.VarChar, 100).Value = cbbPRType.Value == "P" ? cbbCustomer.Text : lbCustomerFirstName.Value;
                                cmd.Parameters.Add("@request_for", SqlDbType.VarChar, 100).Value = string.Empty;//cbbPRType.Value == "P" ? cbbQuotation.Text : lbCustomerFirstName.Value;
                                cmd.Parameters.Add("@customer_po_no", SqlDbType.VarChar, 50).Value = string.Empty;//cbbPRType.Value == "P" ? cbbQuotation.Text : lbCustomerFirstName.Value;
                                cmd.Parameters.Add("@due_date_delivery", SqlDbType.Date).Value = due_date;
                                cmd.Parameters.Add("@objective_type", SqlDbType.VarChar, 1).Value = cbbRequestFor.Value;
                                cmd.Parameters.Add("@sales_order_no", SqlDbType.VarChar, 100).Value = string.Empty;
                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = string.Empty;
                                cmd.Parameters.Add("@request_by", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@purchase_order_by", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@audit_by", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@authorize_by", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@is_cancel", SqlDbType.Bit).Value = 0;
                                cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = 0;

                                cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                cmd.ExecuteNonQuery();

                            }
                            #endregion PR Header

                            #region PR Detail

                            //var oldDetailId = 0;
                            var oldDetailId = 0;
                            var newDetailId = 0;
                            foreach (var row in purchaseRequestDetailList)
                            {
                                DateTime? receive_date = null;
                                if (row.receive_date != null)
                                {
                                    receive_date = row.receive_date;
                                }
                                if (row.id < 0)
                                {
                                    if (hdDocumentStatus.Value != "PT" && hdDocumentStatus.Value != "CF") // confirm กับ Partial เพิ่มสินค้าไม่ได้
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_purchase_request_detail_add", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@purchase_request_no", SqlDbType.VarChar, 20).Value = txtPRNo.Value;
                                            cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                            cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.item_id;
                                            cmd.Parameters.Add("@item_no", SqlDbType.VarChar, 50).Value = row.item_no;
                                            cmd.Parameters.Add("@item_description", SqlDbType.VarChar, 200).Value = row.item_description;
                                            cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 20).Value = row.item_type;
                                            cmd.Parameters.Add("@item_unit", SqlDbType.VarChar, 20).Value = row.item_unit;
                                            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                            cmd.Parameters.Add("@receive_qty", SqlDbType.Int).Value = 0;// row.receive_qty;
                                            cmd.Parameters.Add("@receive_date", SqlDbType.DateTime).Value = receive_date;
                                            cmd.Parameters.Add("@receive_by", SqlDbType.Int).Value = row.receive_by;

                                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                            cmd.Parameters.Add("@purchaseOrderNo", SqlDbType.VarChar, 50).Value = row.purchaseOrderNo;
                                            cmd.Parameters.Add("@receiveNo", SqlDbType.VarChar, 50).Value = row.receiveNo;

                                            cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = row.supplier_id;

                                            newDetailId = Convert.ToInt32(cmd.ExecuteScalar());

                                            var childData = (from t in prDetailMFGList where t.pr_detail_id == oldDetailId select t).ToList();
                                            foreach (var rowDetail in childData)
                                            {
                                                rowDetail.pr_detail_id = newDetailId;// + countId;
                                            }

                                            row.id = newDetailId;
                                        }

                                        //  Update after insert
                                        var flag = (status == "CF");
                                        using (SqlCommand cmd = new SqlCommand("sp_purchase_request_detail_edit", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                            cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                            cmd.Parameters.Add("@purchase_request_no", SqlDbType.VarChar, 20).Value = txtPRNo.Value;
                                            //cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.item_id;
                                            //cmd.Parameters.Add("@item_no", SqlDbType.VarChar, 20).Value = row.item_no;
                                            //cmd.Parameters.Add("@item_description", SqlDbType.VarChar, 200).Value = row.item_description;
                                            //cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 20).Value = row.item_type;
                                            //cmd.Parameters.Add("@item_unit", SqlDbType.VarChar, 20).Value = row.item_unit;
                                            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                            cmd.Parameters.Add("@receive_qty", SqlDbType.Int).Value = (flag ? row.receive_qty : 0);
                                            cmd.Parameters.Add("@receive_qty_temp", SqlDbType.Int).Value = (flag ? 0 : row.receive_qty);
                                            cmd.Parameters.Add("@receive_date", SqlDbType.DateTime).Value = receive_date;
                                            cmd.Parameters.Add("@receive_by", SqlDbType.Int).Value = row.receive_by;
                                            cmd.Parameters.Add("@is_deleted", SqlDbType.Bit).Value = row.is_deleted;

                                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                            cmd.Parameters.Add("@purchaseOrderNo", SqlDbType.VarChar, 50).Value = row.purchaseOrderNo;
                                            cmd.Parameters.Add("@receiveNo", SqlDbType.VarChar, 50).Value = row.receiveNo;
                                            cmd.Parameters.Add("@is_mobile", SqlDbType.Bit).Value = false;

                                            cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = row.supplier_id;

                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                                else
                                {
                                    var flag = (status == "CF");
                                    using (SqlCommand cmd = new SqlCommand("sp_purchase_request_detail_edit", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                        cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                        cmd.Parameters.Add("@purchase_request_no", SqlDbType.VarChar, 20).Value = txtPRNo.Value;
                                        //cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.item_id;
                                        //cmd.Parameters.Add("@item_no", SqlDbType.VarChar, 20).Value = row.item_no;
                                        //cmd.Parameters.Add("@item_description", SqlDbType.VarChar, 200).Value = row.item_description;
                                        //cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 20).Value = row.item_type;
                                        //cmd.Parameters.Add("@item_unit", SqlDbType.VarChar, 20).Value = row.item_unit;
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                        cmd.Parameters.Add("@receive_qty", SqlDbType.Int).Value = (flag ? row.receive_qty : 0);
                                        cmd.Parameters.Add("@receive_qty_temp", SqlDbType.Int).Value = (flag ? 0 : row.receive_qty);
                                        cmd.Parameters.Add("@receive_date", SqlDbType.DateTime).Value = receive_date;
                                        cmd.Parameters.Add("@receive_by", SqlDbType.Int).Value = row.receive_by;
                                        cmd.Parameters.Add("@is_deleted", SqlDbType.Bit).Value = row.is_deleted;

                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        cmd.Parameters.Add("@purchaseOrderNo", SqlDbType.VarChar, 50).Value = row.purchaseOrderNo;
                                        cmd.Parameters.Add("@receiveNo", SqlDbType.VarChar, 50).Value = row.receiveNo;
                                        cmd.Parameters.Add("@is_mobile", SqlDbType.Bit).Value = false;

                                        cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = row.supplier_id;

                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }

                            #endregion

                            #region Transection
                            if (status == "CF" || status == "PT")
                            {
                                //-------------- จาก requirement 19/04/2561 --------
                                var transNo = string.Empty;
                                using (SqlCommand cmd = new SqlCommand("sp_gen_transection_no", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    transNo = Convert.ToString(cmd.ExecuteScalar());
                                }

                                foreach (var row in purchaseRequestDetailList)//.Where(t => t.receive_qty_balance != 0))
                                {
                                    DateTime? receive_date = null;
                                    if (row.receive_date != null)
                                    {
                                        receive_date = row.receive_date;
                                    }
                                    if (row.item_type == "P")
                                    {
                                        #region Product 
                                        //var mfgList = (from t in prDetailMFGList where t.item_id == row.item_id && t.is_deleted && !t.is_in_stock select t).ToList();
                                        var mfgList = (from t in prDetailMFGList where t.item_id == row.item_id && !t.is_in_stock select t).ToList();
                                        foreach (var mfgRow in mfgList)
                                        {
                                            var mfg_receive_date = DateTime.ParseExact(mfgRow.display_receive_date.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                            if (row.id < 0)
                                            {
                                                if (row.receive_qty > 0)
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                    {
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                        cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                        cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "PR";
                                                        cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "IN";
                                                        cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 1;
                                                        cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "ใบแจ้งหนี้ " + mfgRow.receive_no + ", MFG No. " + mfgRow.mfg_no;
                                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                        cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                        cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtPRNo.Value;
                                                        cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = Convert.IsDBNull(cbbSupplier.Value) ? 0 : Convert.ToInt32(cbbSupplier.Value);
                                                        cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = hdCustomerId.Value;//cbbPRType.Value == "P" ? cbbCustomer.Value ?? 0 : Convert.ToInt32(hdCustomerId.Value);
                                                        cmd.Parameters.Add("@purchase_received_date", SqlDbType.DateTime).Value = mfg_receive_date;
                                                        cmd.ExecuteNonQuery();

                                                    }
                                                }
                                            }
                                            else if (row.id > 0 && !mfgRow.is_deleted) // ไม่มีการแก้ไขจำนวน มีแต่การเพิ่มเท่านั้น เพราะเกิดจากการรับ
                                            {
                                                //var diffQty = row.qty - row.old_qty;
                                                //if (diffQty != 0)
                                                //{
                                                if (row.receive_qty > 0)
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                    {
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                        cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                        cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "PR";
                                                        cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "IN";//row.receive_qty > 0 ? "IN" : "OUT";
                                                        cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 1;
                                                        cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "ใบแจ้งหนี้ " + mfgRow.receive_no + ", MFG No. " + mfgRow.mfg_no;
                                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                        cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                        cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtPRNo.Value;
                                                        cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = Convert.IsDBNull(cbbSupplier.Value) ? 0 : Convert.ToInt32(cbbSupplier.Value);
                                                        cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = hdCustomerId.Value;//cbbPRType.Value == "P" ? cbbCustomer.Value ?? 0 : Convert.ToInt32(hdCustomerId.Value);
                                                        cmd.Parameters.Add("@purchase_received_date", SqlDbType.DateTime).Value = mfg_receive_date;
                                                        cmd.ExecuteNonQuery();

                                                    }
                                                }
                                                //}
                                            }
                                            else if (row.id > 0 && mfgRow.is_deleted)
                                            {
                                                if (row.receive_qty < 0)
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                    {
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                        cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                        cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "PR";
                                                        cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "OUT";
                                                        cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = -1;// คืนค่า
                                                        cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "คืนค่าจาก PR MFG No. " + mfgRow.mfg_no;//row.remark;
                                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                        cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                        cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtPRNo.Value;
                                                        cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = hdCustomerId.Value;//Convert.IsDBNull(cbbSupplier.Value) ? 0 : Convert.ToInt32(cbbSupplier.Value);
                                                        cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbPRType.Value == "P" ? cbbCustomer.Value ?? 0 : Convert.ToInt32(hdCustomerId.Value);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Spare Part
                                        if (row.id < 0)
                                        {
                                            if (row.receive_qty > 0)
                                            {
                                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "PR";
                                                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "IN";
                                                    cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.receive_qty;
                                                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "ใบแจ้งหนี้ " + row.receiveNo;//"Purchase Request";
                                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                    cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtPRNo.Value;
                                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = Convert.IsDBNull(cbbSupplier.Value) ? 0 : Convert.ToInt32(cbbSupplier.Value);
                                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = hdCustomerId.Value;// cbbPRType.Value == "P" ? cbbCustomer.Value ?? 0 : Convert.ToInt32(hdCustomerId.Value);
                                                    cmd.Parameters.Add("@purchase_received_date", SqlDbType.DateTime).Value = receive_date;
                                                    cmd.ExecuteNonQuery();

                                                }
                                            }
                                        }
                                        else if (row.id > 0 && !row.is_deleted) // ไม่มีการแก้ไขจำนวน มีแต่การเพิ่มเท่านั้น เพราะเกิดจากการรับ
                                        {
                                            //var diffQty = row.qty - row.old_qty;
                                            //if (diffQty != 0)
                                            //{
                                            if (row.receive_qty > 0)
                                            {
                                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "PR";
                                                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "IN";//row.receive_qty > 0 ? "IN" : "OUT";
                                                    cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.receive_qty;
                                                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "ใบแจ้งหนี้ " + row.receiveNo;//"PURCHASE REQUEST";
                                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                    cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtPRNo.Value;
                                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = Convert.IsDBNull(cbbSupplier.Value) ? 0 : Convert.ToInt32(cbbSupplier.Value);
                                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = hdCustomerId.Value;// cbbPRType.Value == "P" ? cbbCustomer.Value ?? 0 : Convert.ToInt32(hdCustomerId.Value);
                                                    cmd.Parameters.Add("@purchase_received_date", SqlDbType.DateTime).Value = receive_date;
                                                    cmd.ExecuteNonQuery();

                                                }
                                            }
                                            //}
                                        }
                                        else if (row.id > 0 && row.is_deleted)
                                        {
                                            if (row.receive_qty > 0)
                                            {
                                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "PR";
                                                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "OUT";
                                                    cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.receive_qty * -1;// คืนค่า
                                                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "คืนค่าจาก PR";//row.remark;
                                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                    cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtPRNo.Value;
                                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = Convert.IsDBNull(cbbSupplier.Value) ? 0 : Convert.ToInt32(cbbSupplier.Value);
                                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = hdCustomerId.Value;// cbbPRType.Value == "P" ? cbbCustomer.Value ?? 0 : Convert.ToInt32(hdCustomerId.Value);
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                            #endregion

                            #region Detail MFG
                            var is_in_stock = (status == "CF" || status == "PT");
                            foreach (var row in prDetailMFGList)
                            {
                                var inner_in_stock = false;
                                if ((status == "CF" || status == "PT"))
                                {
                                    if (!row.is_in_stock)
                                    {
                                        inner_in_stock = is_in_stock;
                                    }
                                }
                                else if (status == "FL")
                                {
                                    inner_in_stock = is_in_stock = row.is_in_stock;
                                }

                                if (row.id < 0)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_purchase_request_detail_mfg_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@pr_no", SqlDbType.VarChar, 20).Value = txtPRNo.Value;
                                        cmd.Parameters.Add("@pr_detail_id", SqlDbType.Int).Value = row.pr_detail_id;
                                        cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.item_id;
                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;

                                        cmd.Parameters.Add("@receive_no", SqlDbType.VarChar, 20).Value = row.receive_no;
                                        DateTime? receive_date = null;
                                        if (row.display_receive_date != "")
                                        {
                                            receive_date = DateTime.ParseExact(row.display_receive_date.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                        }
                                        cmd.Parameters.Add("@receive_date", SqlDbType.Date).Value = receive_date;

                                        cmd.Parameters.Add("@is_in_stock", SqlDbType.Bit).Value = inner_in_stock;

                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();
                                    }

                                    var mfg_receive_date = DateTime.ParseExact(row.display_receive_date.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                    using (SqlCommand cmd = new SqlCommand("sp_product_mfg_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;
                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;
                                        cmd.Parameters.Add("@purchase_request_no", SqlDbType.VarChar, 20).Value = txtPRNo.Value;
                                        cmd.Parameters.Add("@receive_date", SqlDbType.DateTime).Value = mfg_receive_date;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }

                                }
                                else if (row.id > 0 && !row.is_deleted)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_purchase_request_detail_mfg_edit", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                        cmd.Parameters.Add("@pr_no", SqlDbType.VarChar, 20).Value = txtPRNo.Value;
                                        cmd.Parameters.Add("@pr_detail_id", SqlDbType.Int).Value = row.pr_detail_id;
                                        cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.item_id;
                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;

                                        cmd.Parameters.Add("@receive_no", SqlDbType.VarChar, 20).Value = row.receive_no;
                                        DateTime? receive_date = null;
                                        if (row.display_receive_date != "")
                                        {
                                            receive_date = DateTime.ParseExact(row.display_receive_date.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                        }
                                        cmd.Parameters.Add("@receive_date", SqlDbType.Date).Value = receive_date;

                                        cmd.Parameters.Add("@is_in_stock", SqlDbType.Bit).Value = is_in_stock;

                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
                                    //if (status != "FL")
                                    //{
                                    using (SqlCommand cmd = new SqlCommand("sp_product_mfg_edit_by_mfg ", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
                                    // }
                                }
                                else
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_purchase_request_detail_mfg_delete", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        cmd.ExecuteNonQuery();
                                    }
                                    // if (status != "FL")
                                    // {
                                    using (SqlCommand cmd = new SqlCommand("sp_product_mfg_delete_by_mfg ", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
                                    // }
                                }
                            }
                            #endregion
                        }

                        if (cbbPRType.Value == "P")
                        {
                            dvProductCust.Attributes["style"] = "display:''";
                            dvQuotation.Attributes["style"] = "display:none";
                            dvQuotation2.Attributes["style"] = "display:none";
                            //gridViewPR.Columns["mfg_detail"].VisibleIndex = 1;
                        }
                        else
                        {
                            dvProductCust.Attributes["style"] = "display:none";
                            dvQuotation.Attributes["style"] = "display:''";
                            dvQuotation2.Attributes["style"] = "display:''";
                            //gridViewPR.Columns["mfg_detail"].Visible = false;
                        }

                        MainMaster.ShowInfo(Page, this.GetType(), "บันทึกข้อมูลสำเร็จแล้ว!", MainMaster.AlertIconInfo.success, "");

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        tran.Dispose();
                        conn.Close();
                        //MainMaster.ShowInfo(Page, this.GetType(), ex.Message  , MainMaster.AlertIconInfo.error,"");
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('" + ex.Message + "','E')", true);
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('เกิดผิดพลาดในการบันทึกข้อมูล','E')", true);
                    }
                    finally
                    {
                        if (!conn.State.Equals(ConnectionState.Closed))
                        {
                            tran.Commit();
                            tran.Dispose();
                            conn.Close();
                            Response.Redirect("PurchaseRequest.aspx?dataId=" + newID);
                        }
                    }
                }
            }
        }
        protected void gridViewQuotationDetail_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            ASPxCheckBox checkBox = gridViewQuotationDetail.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkQuotationDetail") as ASPxCheckBox;
            if (checkBox != null)
            {
                if (e.DataColumn.FieldName == "is_selected")
                {
                    if (purchaseRequestDetailList != null)
                    {
                        var row = (from t in selectedPurchaseRequestDetailList where t.item_no == Convert.ToString(e.KeyValue) && !t.is_deleted select t).FirstOrDefault();
                        if (row != null)
                        {
                            checkBox.Checked = true;
                        }
                        else
                        {
                            checkBox.Checked = false;
                        }
                    }
                }
            }
        }

        protected void gridViewProductDetail_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            ASPxCheckBox checkBox = gridViewProductDetail.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkProductDetail") as ASPxCheckBox;
            if (checkBox != null)
            {
                if (e.DataColumn.FieldName == "is_selected")
                {
                    if (selectedPurchaseRequestDetailList != null)
                    {
                        var row = (from t in selectedPurchaseRequestDetailList where t.item_no == Convert.ToString(e.KeyValue) && !t.is_deleted select t).FirstOrDefault();
                        if (row != null)
                        {
                            checkBox.Checked = true;
                        }
                        else
                        {
                            checkBox.Checked = false;
                        }
                    }
                }
            }
        }

        [WebMethod]
        public static string ValidateData(string pr_type, string product_type)
        {
            string msg = string.Empty;
            var prMFGDetail = (List<PurchaseRequestDetailMFG>)HttpContext.Current.Session["SESSION_PR_DETAIL_MFG_PR"];
            var prDetail = (List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"];

            if (prDetail == null || prDetail.Count == 0)
            {

                msg += "กรุณาเลือกรายการยืม อย่างน้อย 1 รายการ \n";

            }
            else
            {
                if (product_type == "P")
                {
                    //  Check all pr type
                    foreach (var row in prDetail.Where(t => !t.is_deleted))
                    {
                        if (prMFGDetail != null)
                        {
                            if (prMFGDetail.Count > 0)
                            {
                                var mfgSelectedData = (from t in prMFGDetail where t.pr_detail_id == row.id && !t.is_deleted select t).ToList();
                                var all_receive = (row.receive_qty + row.receive_qty_balance);
                                if (mfgSelectedData != null)
                                {
                                    if (row.qty < all_receive)
                                    {
                                        msg += "สินค้า " + row.item_no + " จำนวน MFG (" + mfgSelectedData.Count + ") ไม่ตรงกับจำนวนที่รับทั้งหมด (" + row.qty + ")\n";
                                    }
                                    else if (all_receive != mfgSelectedData.Count)
                                    {
                                        msg += "สินค้า " + row.item_no + " จำนวน MFG (" + mfgSelectedData.Count + ") ไม่ตรงกับจำนวนที่รับทั้งหมด (" + all_receive + ")\n";
                                    }
                                }
                                else
                                {
                                    msg += "สินค้า " + row.item_no + " จำนวน MFG (" + mfgSelectedData.Count + ") ไม่ตรงกับจำนวนที่รับทั้งหมด (" + all_receive + ")\n";
                                }
                            }
                            else
                            {
                                if (product_type == "P")
                                {
                                    msg += "กรุณาเลือก MFG ของแต่ละสินค้า";
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (product_type == "P")
                            {
                                msg += "กรุณาเลือก MFG ของแต่ละสินค้า";
                                break;
                            }
                        }
                    }

                    //  Check ref no for product
                    if (msg == "")
                    {
                        foreach (var row in prDetail.Where(t => !t.is_deleted))
                        {
                            if (prMFGDetail != null)
                            {
                                if (prMFGDetail.Count > 0)
                                {
                                    var mfgSelectedData = (from t in prMFGDetail where t.pr_detail_id == row.id && !t.is_deleted select t).ToList();
                                    if (mfgSelectedData != null)
                                    {
                                        if (mfgSelectedData.Count > 0)
                                        {
                                            foreach (var row2 in mfgSelectedData.Where(t => !t.is_deleted))
                                            {
                                                if (row2.receive_no == "")
                                                {
                                                    if (msg != "")
                                                    {
                                                        msg += ", ";
                                                    }
                                                    msg += "" + row2.mfg_no + "";
                                                }
                                            }
                                            if (msg != "")
                                            {
                                                msg = "กรุณากรอกเลขที่เอกสารอ้างอิง สำหรับ MFG " + msg + "\n";
                                            }
                                        }
                                        else if (row.receive_qty != 0)
                                        {
                                            msg = "กรุณาเลือก MFG ของแต่ละสินค้า";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //  Check ref no
                if (msg == "")
                {
                    if (product_type != "P")
                    {
                        foreach (var row in prDetail.Where(t => !t.is_deleted))
                        {
                            if (row.receiveNo == "" && row.receive_qty > 0)
                            {
                                if (msg != "")
                                {
                                    msg += ", ";
                                }
                                msg += "" + row.item_no + "";
                            }
                        }
                        if (msg != "")
                        {
                            msg = "กรุณากรอกเลขที่เอกสารอ้างอิง สำหรับสินค้า " + msg + "\n";
                        }
                    }
                }
            }
            return msg;
        }
        [WebMethod]
        public static string ChangedSupplier()
        {
            string returnData = string.Empty;
            HttpContext.Current.Session.Remove("SESSION_PRODUCT_DETAIL_PR");
            HttpContext.Current.Session.Remove("SESSION_PURCHASE_REQUEST_DETAIL_PR");
            //Session.Remove("SESSION_QUOTATION_DETAIL_PR");
            HttpContext.Current.Session.Remove("SESSION_SELECTED_PURCHASE_REQUEST_DETAIL_PR");
            HttpContext.Current.Session.Remove("SESSION_SELECTED_PR_DETAIL_MFG_PR");
            HttpContext.Current.Session.Remove("SESSION_PR_DETAIL_MFG_PR");
            HttpContext.Current.Session.Remove("SESSION_MFG_DETAIL_PR");
            return returnData;
        }
        protected void gridMFG_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridMFG.DataSource = (from t in selectedPRDetailMFGList where !t.is_deleted select t).ToList();
            gridMFG.DataBind();
            if (hdDocumentStatus.Value == "CP")
            {
                gridMFG.Columns["id"].Visible = false;
            }

        }
        protected void BindGridMFGDetail()

        {
            gridMFG.DataSource = (from t in selectedPRDetailMFGList where !t.is_deleted select t).ToList();
            gridMFG.DataBind();
        }
        [WebMethod]
        public static PurchaseRequestDetail GetMFGDetail(int id)
        {
            try
            {
                var prDetailData = (List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"];
                var prMFGDetailData = (List<PurchaseRequestDetailMFG>)HttpContext.Current.Session["SESSION_PR_DETAIL_MFG_PR"];
                if (prMFGDetailData != null)
                {
                    var row = (from t in prMFGDetailData where t.item_id == Convert.ToInt32(id) select t).ToList();
                    if (row != null)
                    {
                        HttpContext.Current.Session["SESSION_SELECTED_PR_DETAIL_MFG_PR"] = row;
                    }
                }
                if (prDetailData != null)
                {
                    var row = (from t in prDetailData where t.item_id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (row != null)
                    {
                        return row;
                    }

                }

                return new PurchaseRequestDetail();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [WebMethod]
        public static string AddMFG(string mfg_no, int product_id, int pr_detail_id, string receive_no, string receive_date, int maxQty)
        {
            var returnData = string.Empty;

            var mfgProductList = (List<PurchaseRequestDetailMFG>)HttpContext.Current.Session["SESSION_SELECTED_PR_DETAIL_MFG_PR"];
            var list = (from t in mfgProductList
                        where !t.is_deleted
                        select t).ToList();
            if (list.Count == maxQty)
            {
                return "ไม่สามารถกรอก MFG เกินจำนวนสินค้า (" + maxQty + ") ได้";
            }

            if (mfgProductList != null)
            {
                var checkExist = (from t in mfgProductList
                                  where t.mfg_no == mfg_no
                                  select t).FirstOrDefault();
                if (checkExist == null)
                {
                    mfgProductList.Add(new PurchaseRequestDetailMFG()
                    {
                        id = (mfgProductList.Count + 1) * -1,
                        mfg_no = mfg_no,
                        item_id = product_id,
                        pr_detail_id = pr_detail_id,
                        receive_no = receive_no,
                        is_in_stock = false,
                        display_receive_date = receive_date
                    });

                }
            }
            else
            {
                mfgProductList = new List<PurchaseRequestDetailMFG>();
                mfgProductList.Add(new PurchaseRequestDetailMFG()
                {
                    id = (mfgProductList.Count + 1) * -1,
                    mfg_no = mfg_no,
                    item_id = product_id,
                    pr_detail_id = pr_detail_id,
                    receive_no = receive_no,
                    is_in_stock = false,
                    display_receive_date = receive_date
                });

            }
            HttpContext.Current.Session["SESSION_SELECTED_PR_DETAIL_MFG_PR"] = mfgProductList;


            return "";
        }
        [WebMethod]
        public static PurchaseRequestDetailMFG EditMFG(int id)
        {
            var mfgProductList = (List<PurchaseRequestDetailMFG>)HttpContext.Current.Session["SESSION_SELECTED_PR_DETAIL_MFG_PR"];
            if (mfgProductList != null)
            {
                var row = (from t in mfgProductList where t.id == id select t).FirstOrDefault();
                if (row != null)
                {
                    return row;
                }
            }
            return new PurchaseRequestDetailMFG(); // ไม่มี Data
        }
        [WebMethod]
        public static string ClearGridViewPR()
        {
            try
            {
                var returnData = "SUCCESS";
                HttpContext.Current.Session.Remove("SESSION_PURCHASE_REQUEST_DETAIL_PR");
                return returnData;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        [WebMethod]
        public static string SubmitEditMFG(int id, string mfg_no, string receive_no, string receive_date)
        {
            try
            {
                var returnData = "SUCCESS";
                var mfgProductList = (List<PurchaseRequestDetailMFG>)HttpContext.Current.Session["SESSION_SELECTED_PR_DETAIL_MFG_PR"];
                if (mfgProductList != null)
                {
                    var checkExsit = (from t in mfgProductList where t.id != id && t.mfg_no == mfg_no select t).FirstOrDefault();
                    if (checkExsit == null)
                    {
                        var row = (from t in mfgProductList where t.id == id select t).FirstOrDefault();
                        if (row != null)
                        {
                            row.mfg_no = mfg_no;
                            row.receive_no = receive_no;
                            row.display_receive_date = receive_date;
                        }
                    }
                    else
                    {
                        returnData = "MFG No : " + mfg_no + " ซ้ำ";
                    }
                }
                return returnData;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        [WebMethod]
        public static string SubmitMFGProduct(string prDetailId)
        {
            var returnData = string.Empty;
            try
            {
                var selectedMFGList = (List<PurchaseRequestDetailMFG>)HttpContext.Current.Session["SESSION_SELECTED_PR_DETAIL_MFG_PR"];
                var borrowMFGDetailData = (List<PurchaseRequestDetailMFG>)HttpContext.Current.Session["SESSION_PR_DETAIL_MFG_PR"];
                if (borrowMFGDetailData == null)
                {
                    borrowMFGDetailData = new List<PurchaseRequestDetailMFG>();
                }
                if (selectedMFGList != null)
                {
                    var count = 0;
                    foreach (var row in selectedMFGList)
                    {
                        var checkExist = (from t in borrowMFGDetailData where t.id == row.id && t.pr_detail_id == row.pr_detail_id select t).FirstOrDefault();
                        if (checkExist != null)
                        {
                            if (!row.is_deleted)
                            {
                                checkExist.pr_detail_id = row.pr_detail_id;
                                checkExist.mfg_no = row.mfg_no;
                                checkExist.is_deleted = row.is_deleted;
                                checkExist.receive_no = row.receive_no;
                                checkExist.is_in_stock = row.is_in_stock;
                                checkExist.display_receive_date = row.display_receive_date;

                                count++;
                            }
                            else
                            {
                                if (row.id < 0)
                                {
                                    borrowMFGDetailData.Remove(row);
                                }
                                else
                                {
                                    checkExist.is_deleted = row.is_deleted;
                                }
                            }
                        }
                        else
                        {
                            borrowMFGDetailData.Add(row);
                            count++;
                        }
                    }

                    var prDetailData = (List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"];
                    if (prDetailData != null)
                    {
                        var row = (from t in prDetailData where t.id == Convert.ToInt32(prDetailId) select t).FirstOrDefault();
                        if (row != null)
                        {
                            row.receive_qty = count - row.receive_qty_balance;
                        }
                    }

                    HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"] = prDetailData;
                }

                HttpContext.Current.Session["SESSION_PR_DETAIL_MFG_PR"] = borrowMFGDetailData;

                //  Update date number of mfg to product
                /*var purchaseRequestDetailList = (List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"];
                var checkExist2 = (from t in purchaseRequestDetailList where t.id == Convert.ToInt32(prDetailId) select t).FirstOrDefault();
                if (checkExist2 != null)
                {
                    checkExist2.receive_qty = borrowMFGDetailData.Count;
                }*/


                return returnData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static string SelectAllProductDetail(bool selected, string type, string product_type, string search)
        {
            var result = "";

            var productDetail = (List<ProductDetail>)HttpContext.Current.Session["SESSION_PRODUCT_DETAIL_PR"];
            var quotationDetail = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_PR"];
            var selectedPRData = (List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_SELECTED_PURCHASE_REQUEST_DETAIL_PR"];

            /*(from t in quotationDetailList
             where (t.product_no.ToUpper().Contains(searchText.ToUpper()) || t.quotation_description.ToUpper().Contains(searchText.ToUpper()))
             select t).ToList();*/


            if (type == "P")
            {
                if (productDetail != null)
                {

                    var detailTemp = (from t in productDetail
                                      where t.product_no.ToUpper().Contains(search)
                                      select t).ToList();

                    foreach (var data in detailTemp)
                    {
                        if (selected)
                        {
                            data.is_selected = true;
                            if (selectedPRData != null && selectedPRData.Count > 0)
                            {
                                var rowExist = (from t in quotationDetail
                                                where t.product_no == data.product_no
                                                select t).FirstOrDefault();

                                if (rowExist != null) // กรณี Newmode
                                {
                                    //rowExist.qty = data.qty;
                                }
                                else
                                {
                                    selectedPRData.Add(new PurchaseRequestDetail()
                                    {
                                        id = (selectedPRData.Count + 1) * -1,
                                        is_deleted = false,
                                        item_description = data.product_name,
                                        item_id = data.product_id,
                                        qty = (data.qty_reserve == data.qty ? 1 : data.qty_reserve - data.qty),
                                        item_no = data.product_no,
                                        item_unit = data.item_unit,
                                        item_type = product_type,
                                        supplier_id = data.supplier_id,
                                        supplier_name = data.supplier_name
                                    });
                                }
                            }
                            else
                            {
                                if (selectedPRData == null)
                                {
                                    selectedPRData = new List<PurchaseRequestDetail>();
                                }
                                selectedPRData.Add(new PurchaseRequestDetail()
                                {
                                    id = (selectedPRData.Count + 1) * -1,
                                    is_deleted = false,
                                    item_description = data.product_name,
                                    item_id = data.product_id,
                                    qty = (data.qty_reserve == data.qty ? 1 : data.qty_reserve - data.qty),
                                    item_no = data.product_no,
                                    item_unit = data.item_unit,
                                    item_type = product_type,
                                    supplier_id = data.supplier_id,
                                    supplier_name = data.supplier_name
                                });
                            }
                        }
                        else
                        {
                            var rowExist = (from t in selectedPRData where t.item_no == data.product_no select t).FirstOrDefault();
                            data.is_selected = false;
                            if (rowExist != null) // กรณี Newmode
                            {
                                if (rowExist.id < 0)
                                {
                                    selectedPRData.Remove(rowExist);
                                }
                                else
                                {
                                    rowExist.is_deleted = true;
                                }
                            }
                        }
                    }
                }
                HttpContext.Current.Session["SESSION_SELECTED_PURCHASE_REQUEST_DETAIL_PR"] = selectedPRData;
                HttpContext.Current.Session["SESSION_PRODUCT_DETAIL_PR"] = productDetail;
                HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_PR"] = quotationDetail;
                result = "P";
            }
            else if (type == "QU")
            {
                if (quotationDetail != null) // ต้องหา Row เจอเท่านั้น ถึงจะทำต่อ
                {
                    var detailTemp = (from t in quotationDetail
                                      where t.product_no.ToUpper().Contains(search)
                                      select t).ToList();
                    foreach (var data in detailTemp)
                    {
                        if (selected)
                        {
                            data.is_selected = true;
                            if (selectedPRData != null && selectedPRData.Count > 0)
                            {
                                var rowExist = (from t in selectedPRData where t.item_no == data.product_no select t).FirstOrDefault();

                                if (rowExist != null) // กรณี Newmode
                                {
                                    rowExist.qty = data.qty;
                                }
                                else
                                {
                                    selectedPRData.Add(new PurchaseRequestDetail()
                                    {
                                        id = (selectedPRData.Count + 1) * -1,
                                        is_deleted = false,
                                        item_description = data.quotation_description,
                                        item_id = data.product_id,
                                        qty = data.qty,
                                        item_no = data.product_no,
                                        item_unit = data.unit_code,
                                        item_type = data.product_type,
                                        supplier_id = data.supplier_id,
                                        supplier_name = data.supplier_name
                                    });
                                }
                            }
                            else
                            {
                                if (selectedPRData == null)
                                {
                                    selectedPRData = new List<PurchaseRequestDetail>();
                                }
                                selectedPRData.Add(new PurchaseRequestDetail()
                                {
                                    id = (selectedPRData.Count + 1) * -1,
                                    is_deleted = false,
                                    item_description = data.quotation_description,
                                    item_id = data.product_id,
                                    qty = data.qty,
                                    item_no = data.product_no,
                                    item_unit = data.unit_code,
                                    item_type = data.product_type,
                                    supplier_id = data.supplier_id,
                                    supplier_name = data.supplier_name
                                });
                            }
                        }
                        else
                        {
                            var rowExist = (from t in selectedPRData where t.item_no == data.product_no select t).FirstOrDefault();
                            data.is_selected = false;
                            if (rowExist != null) // กรณี Newmode
                            {
                                if (rowExist.id < 0)
                                {
                                    selectedPRData.Remove(rowExist);
                                }
                                else
                                {
                                    rowExist.is_deleted = true;
                                }
                            }
                        }
                    }
                }
                HttpContext.Current.Session["SESSION_SELECTED_PURCHASE_REQUEST_DETAIL_PR"] = selectedPRData;
                HttpContext.Current.Session["SESSION_PRODUCT_DETAIL_PR"] = productDetail;
                HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_PR"] = quotationDetail;
                result = "QU";
            }

            return result;
        }

        [WebMethod]
        public static string AutoAddQty()
        {
            var prDetailData = (List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"];
            if (prDetailData != null)
            {
                foreach (var item in prDetailData)
                {
                    if (item.purchaseOrderNo != "")
                    {
                        item.receive_qty = item.qty - item.receive_qty_balance;
                    }
                }
            }
            HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"] = prDetailData;
            return "";
        }

        [WebMethod]
        public static string AutoClearQty()
        {
            var prDetailData = (List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"];
            if (prDetailData != null)
            {
                foreach (var item in prDetailData)
                {
                    item.receive_qty = 0;
                }
            }
            HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"] = prDetailData;
            return "";
        }

        [WebMethod]
        public static void moveUpProductDetail(string id)
        {
            List<PurchaseRequestDetail> data = (List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"];
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
            HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"] = data;
        }
        [WebMethod]
        public static void moveDownProductDetail(string id)
        {
            List<PurchaseRequestDetail> data = (List<PurchaseRequestDetail>)HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"];
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
            HttpContext.Current.Session["SESSION_PURCHASE_REQUEST_DETAIL_PR"] = data;
        }

    }
}