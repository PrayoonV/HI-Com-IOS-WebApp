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
    public partial class SaleOrder : MasterDetailPage
    {
        DateTime dateTime = DateTime.UtcNow.Date;

        public int dataId = 0;

        #region Members
        public class QuotationData
        {
            public int id { get; set; }
            public string quotation_no { get; set; }
            public string quotation_type { get; set; }
            public DateTime quotation_date { get; set; }
            public string quotation_status { get; set; }
            public int customer_id { get; set; }
            public string customer_code { get; set; }
            public string customer_name { get; set; }
            public int sale_id { get; set; }
            public string sale_name { get; set; }
            public string fax { get; set; }
            public string tel { get; set; }
            public string attention_name { get; set; }
            public string quotation_subject { get; set; }
            public string project_name { get; set; }
            public string customer_address { get; set; }
            public string customer_tel { get; set; }
            public string customer_fax { get; set; }
            public string display_quotation_date { get; set; }
            public bool is_discount_by_item { get; set; }
            public string discount1_type { get; set; }
            public decimal discount1_percentage { get; set; }
            public decimal discount1_amount { get; set; }
            public decimal discount1_total { get; set; }
            public string discount2_type { get; set; }
            public decimal discount2_percentage { get; set; }
            public decimal discount2_amount { get; set; }
            public decimal discount2_total { get; set; }
            public decimal total_amount { get; set; }
            public decimal grand_total { get; set; }
            public bool is_vat { get; set; }
            public decimal vat_total { get; set; }
            public bool is_product_description { get; set; }
            public string discount_item_type { get; set; }

        }
        public class SaleOrderDetail
        {
            public int id { get; set; }
            public string sales_order_no { get; set; }
            public int product_id { get; set; }
            public string product_no { get; set; }
            public string saleorder_description { get; set; }
            public int qty { get; set; }
            public int qu_qty { get; set; }
            public decimal unit_price { get; set; }
            public string unit_code { get; set; }
            public string discount_type { get; set; }
            public decimal discount_percentage { get; set; }
            public decimal discount_amount { get; set; }
            public decimal discount_total { get; set; }
            public decimal total_amount { get; set; }
            public int parent_id { get; set; }
            public int sort_no { get; set; }
            public int quotation_detail_id { get; set; }
            public bool is_deleted { get; set; }
            public string product_type { get; set; }
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
            public int qu_qty { get; set; }
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
            public string product_type { get; set; }

        }
        public class ProductDetailValue
        {
            public decimal total { get; set; }
            public decimal discount_total { get; set; }
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
        }
        public class PODetail
        {
            public int id { get; set; }
            public string ref_po_no { get; set; }
            //public string po_description { get; set; }
            public string ref_po_date { get; set; }
            public bool is_deleted { get; set; }
            public string display_po_date { get; set; }
            public string invoice_no { get; set; }
            public string invoice_date { get; set; }
            public string display_invoice_date { get; set; }
            public int payment_type { get; set; }
            public string display_payment_type { get; set; }
            public DateTime? cheque_date { get; set; }
            public string display_cheque_date { get; set; }
            public int percent_price { get; set; }
            public decimal amount { get; set; }
            public DateTime? bill_date { get; set; }
            public string display_bill_date { get; set; }
            public DateTime? delivery_date { get; set; }
            public string display_delivery_date { get; set; }
            public string temp_delivery_no { get; set; }
            public int credit_day { get; set; }
            public int period_no { get; set; }
            public decimal period_amount { get; set; }
            public decimal diff_deposit { get; set; }
        }
        public class TaxDetail
        {
            public int id { get; set; }
            public string tax_no { get; set; }
            public string tax_description { get; set; }
            public DateTime tax_date { get; set; }
            public bool is_deleted { get; set; }
            public string display_tax_date { get; set; }
        }
        List<QuotationDetail> quotationData
        {
            get
            {

                if (Session["SESSION_QUATATION_DETAIL_SALE_ORDER"] == null)
                    Session["SESSION_QUATATION_DETAIL_SALE_ORDER"] = new List<QuotationDetail>();
                return (List<QuotationDetail>)Session["SESSION_QUATATION_DETAIL_SALE_ORDER"];
            }
            set
            {
                Session["SESSION_QUATATION_DETAIL_SALE_ORDER"] = value;
            }
        }
        List<QuotationDetail> selectedQuotationDetailList
        {
            get
            {

                if (Session["SESSION_SELECTED_QUATATION_DETAIL_SALE_ORDER"] == null)
                    Session["SESSION_SELECTED_QUATATION_DETAIL_SALE_ORDER"] = new List<QuotationDetail>();
                return (List<QuotationDetail>)Session["SESSION_SELECTED_QUATATION_DETAIL_SALE_ORDER"];
            }
            set
            {
                Session["SESSION_SELECTED_QUATATION_DETAIL_SALE_ORDER"] = value;
            }
        }
        List<SaleOrderDetail> saleOrderDetailList
        {
            get
            {

                if (Session["SESSION_SALE_ORDER_DETAIL"] == null)
                    Session["SESSION_SALE_ORDER_DETAIL"] = new List<SaleOrderDetail>();
                return (List<SaleOrderDetail>)Session["SESSION_SALE_ORDER_DETAIL"];
            }
            set
            {
                Session["SESSION_SALE_ORDER_DETAIL"] = value;
            }
        }
        List<SaleOrderDetail> selectedSaleOrderDetailList
        {
            get
            {

                if (Session["SESSION_SELECTED_SALE_ORDER_DETAIL"] == null)
                    Session["SESSION_SELECTED_SALE_ORDER_DETAIL"] = new List<SaleOrderDetail>();
                return (List<SaleOrderDetail>)Session["SESSION_SELECTED_SALE_ORDER_DETAIL"];
            }
            set
            {
                Session["SESSION_SELECTED_SALE_ORDER_DETAIL"] = value;
            }
        }
        List<PODetail> poDetailList
        {
            get
            {

                if (Session["SESSION_PO_DETAIL_SALE_ORDER"] == null)
                    Session["SESSION_PO_DETAIL_SALE_ORDER"] = new List<PODetail>();
                return (List<PODetail>)Session["SESSION_PO_DETAIL_SALE_ORDER"];
            }
            set
            {
                Session["SESSION_SELECTED_SALE_ORDER_DETAIL"] = value;
            }
        }
        PODetail selectedPODetailList
        {
            get
            {

                if (Session["SESSION_SELECTED_PO_DETAIL_SALE_ORDER"] == null)
                    Session["SESSION_SELECTED_PO_DETAIL_SALE_ORDER"] = new PODetail();
                return (PODetail)Session["SESSION_SELECTED_PO_DETAIL_SALE_ORDER"];
            }
            set
            {
                Session["SESSION_SELECTED_PO_DETAIL_SALE_ORDER"] = value;
            }
        }
        List<TaxDetail> taxDetailList
        {
            get
            {

                if (Session["SESSION_TAX_DETAIL_SALE_ORDER"] == null)
                    Session["SESSION_TAX_DETAIL_SALE_ORDER"] = new List<TaxDetail>();
                return (List<TaxDetail>)Session["SESSION_TAX_DETAIL_SALE_ORDER"];
            }
            set
            {
                Session["SESSION_TAX_DETAIL_SALE_ORDER"] = value;
            }
        }
        TaxDetail selectedTaxDetailList
        {
            get
            {

                if (Session["SESSION_SELECTED_TAX_DETAIL_SALE_ORDER"] == null)
                    Session["SESSION_SELECTED_TAX_DETAIL_SALE_ORDER"] = new TaxDetail();
                return (TaxDetail)Session["SESSION_SELECTED_TAX_DETAIL_SALE_ORDER"];
            }
            set
            {
                Session["SESSION_SELECTED_TAX_DETAIL_SALE_ORDER"] = value;
            }
        }
        List<Notification> notificationList
        {
            get
            {

                if (Session["SESSION_NOTIFICATION_SALE_ORDER"] == null)
                    Session["SESSION_NOTIFICATION_SALE_ORDER"] = new List<Notification>();
                return (List<Notification>)Session["SESSION_NOTIFICATION_SALE_ORDER"];
            }
            set
            {
                Session["SESSION_NOTIFICATION_SALE_ORDER"] = value;
            }
        }
        Notification selectedNotificationList
        {
            get
            {

                if (Session["SESSION_SELECTED_NOTIFICATION_SALE_ORDER"] == null)
                    Session["SESSION_SELECTED_NOTIFICATION_SALE_ORDER"] = new Notification();
                return (Notification)Session["SESSION_SELECTED_NOTIFICATION_SALE_ORDER"];
            }
            set
            {
                Session["SESSION_SELECTED_NOTIFICATION_SALE_ORDER"] = value;
            }
        }
        QuotationData quotationDataHeader
        {
            get
            {

                if (Session["SESSION_QUOTATION_DATA_SALE_ORDER"] == null)
                    Session["SESSION_QUOTATION_DATA_SALE_ORDER"] = new List<QuotationData>();
                return (QuotationData)Session["SESSION_QUOTATION_DATA_SALE_ORDER"];
            }
            set
            {
                Session["SESSION_QUOTATION_DATA_SALE_ORDER"] = value;
            }
        }

        private DataSet dsSaleOrderData = new DataSet();
        #endregion
        public override string PageName { get { return "Create Sale Order"; } }

        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }

        public override void OnFilterChanged()
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            dataId = Convert.ToInt32(Request.QueryString["dataId"]);
            try
            {
                var dsIssueHeaderListOnData = new DataSet();
                if (!IsPostBack)
                {
                    // Get Permission and if no permission, will redirect to another page.
                    if (!Permission.GetPermission())
                        Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

                    /*using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        conn.Open();
                        dsIssueHeaderListOnData = SqlHelper.ExecuteDataset(conn, "sp_dropdown_issue_header_list_on");
                        conn.Close();
                    }

                    if (dsIssueHeaderListOnData != null)
                    {
                        foreach (var row in dsIssueHeaderListOnData.Tables[0].AsEnumerable())
                        {
                            cboRefIssueNo.Items.Add(new ListItem(row["data_text"].ToString(), row["data_text"].ToString()));
                        }
                    }*/

                    ClearWorkingSession();
                    PrepareData();
                    LoadData();

                }
                else
                {
                    BindGridSaleOrderDetail();
                    BindGridQuotationDetail();
                    BindGridNoticeDetail();
                    BindGridPODetail();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ClearWorkingSession()
        {
            Session.Remove("SESSION_SALE_ORDER_DETAIL");
            Session.Remove("SESSION_SELECTED_QUATATION_DETAIL_SALE_ORDER");
            Session.Remove("SESSION_QUATATION_DETAIL_SALE_ORDER");
            Session.Remove("SESSION_QUOTATION_DATA_SALE_ORDER");
            Session.Remove("SESSION_NOTIFICATION_SALE_ORDER");
            Session.Remove("SESSION_SELECTED_NOTIFICATION_SALE_ORDER");
            Session.Remove("SESSION_PO_DETAIL_SALE_ORDER");
            Session.Remove("SESSION_SELECTED_PO_DETAIL_SALE_ORDER");
            Session.Remove("SESSION_TAX_DETAIL_SALE_ORDER");
            Session.Remove("SESSION_SELECTED_TAX_DETAIL_SALE_ORDER");
        }

        private DataSet dsResult = new DataSet();

        protected void CheckPermission(int created_by)
        {
            try
            {
                /*if (created_by != Convert.ToInt32(ConstantClass.SESSION_USER_ID))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Disabled Input", "disableInput()", true);
                }*/
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
                    //SPlanetUtil.BindASPxComboBox(ref cbbQuotation, DataListUtil.DropdownStoreProcedureName.Sale_Order_Quotation);
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {

                        conn.Open();
                        var dsQuotationDoc = SqlHelper.ExecuteDataset(conn, DataListUtil.DropdownStoreProcedureName.Sale_Order_Quotation);
                        ViewState["dsQuotationDoc"] = dsQuotationDoc;
                        var serviceType = ConstantClass.SESSION_DEPARTMENT_SERVICE_TYPE;
                        conn.Close();
                        if (dsQuotationDoc.Tables.Count > 0)
                        {
                            if (serviceType.Contains("A"))
                            {
                                cbbQuotation.DataSource = (from t in dsQuotationDoc.Tables[0].AsEnumerable()
                                                           select new
                                                           {
                                                               data_text = t.Field<string>("data_text"),
                                                               data_value = t.Field<int>("data_value").ToString()
                                                           }).Distinct().ToList();
                            }
                            else {
                                cbbQuotation.DataSource = (from t in dsQuotationDoc.Tables[0].AsEnumerable()
                                                           where t.Field<string>("quotation_status") == "PO"
                                                           select new
                                                           {
                                                               data_text = t.Field<string>("data_text"),
                                                               data_value = t.Field<int>("data_value").ToString()
                                                           }).Distinct().ToList();
                            }
                            cbbQuotation.DataBind();
                        }

                    }

                    btnReportClient.Visible = false;
                }

                if (cbDiscountByItem.Checked)
                {
                    if (cbbDiscountByItem.Value == "P")
                    {
                        gridViewDetailSaleOrder.Columns[5].Visible = false;
                        gridViewDetailSaleOrder.Columns[6].Visible = true;
                    }
                    else if (cbbDiscountByItem.Value == "A")
                    {
                        gridViewDetailSaleOrder.Columns[5].Visible = true;
                        gridViewDetailSaleOrder.Columns[6].Visible = false;
                    }
                }
                else
                {
                    gridViewDetailSaleOrder.Columns[5].Visible = false;
                    gridViewDetailSaleOrder.Columns[6].Visible = false;
                }

                gridViewDetailSaleOrder.SettingsBehavior.AllowFocusedRow = true;

                txtDateQuatationNo.Value = dateTime.ToString("dd/MM/yyyy");
                //txtDateShipping.Value = dateTime.ToString("dd/MM/yyyy");
                //txtDateBill.Value = dateTime.ToString("dd/MM/yyyy");

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
                    dsSaleOrderData = SqlHelper.ExecuteDataset(conn, "sp_sale_order_list_data", arrParm.ToArray());
                    ViewState["dsSaleOrderData"] = dsSaleOrderData;
                    conn.Close();
                }
                if (dsSaleOrderData.Tables.Count > 0)
                {
                    #region SaleOrder Header
                    var data = dsSaleOrderData.Tables[0].AsEnumerable().FirstOrDefault();
                    if (data != null)
                    {

                        quotationDataHeader = new QuotationData();
                        quotationDataHeader.is_vat = Convert.ToBoolean(data["is_vat"]);
                        quotationDataHeader.vat_total = Convert.ToDecimal(data["vat_total"]);
                        quotationDataHeader.grand_total = Convert.ToDecimal(data["grand_total"]);
                        quotationDataHeader.total_amount = Convert.ToDecimal(data["total_amount"]);
                        quotationDataHeader.discount1_total = Convert.ToDecimal(data["discount1_total"]);
                        quotationDataHeader.discount1_type = Convert.ToString(data["discount1_type"]);
                        quotationDataHeader.discount1_percentage = Convert.ToDecimal(data["discount1_percentage"]);
                        quotationDataHeader.discount1_amount = Convert.ToDecimal(data["discount1_amount"]);
                        quotationDataHeader.discount2_amount = Convert.ToDecimal(data["discount2_amount"]);
                        quotationDataHeader.discount2_percentage = Convert.ToDecimal(data["discount2_percentage"]);
                        quotationDataHeader.discount2_type = Convert.ToString(data["discount2_type"]);
                        quotationDataHeader.discount2_total = Convert.ToDecimal(data["discount2_total"]);

                        cbbQuotation.ReadOnly = true;
                        cbbQuotation.Items.Insert(0, new ListEditItem(Convert.ToString(data["quotation_no"]), Convert.ToString(data["quotation_id"])));
                        txtSaleOrder.Value = Convert.IsDBNull(data["sale_order_no"]) ? string.Empty : Convert.ToString(data["sale_order_no"]);
                        rdoRemarkBillDelivery.Checked = Convert.IsDBNull(data["remark_id"]) ? false :
                                                   (Convert.ToString(data["remark_id"]) == "1" ? true : false);
                        rdoRemarkSubmitted.Checked = Convert.IsDBNull(data["remark_id"]) ? false :
                                                        (Convert.ToString(data["remark_id"]) == "2" ? true : false);

                        rdoRemarkOthers.Checked = Convert.IsDBNull(data["remark_id"]) ? false :
                                                        (Convert.ToString(data["remark_id"]) == "3" ? true : false);
                        if (rdoRemarkOthers.Checked)
                        {
                            txtRemarkOther.Attributes.Remove("disabled");
                        }
                        txtRemarkOther.Value = Convert.IsDBNull(data["remark_other"]) ? string.Empty : Convert.ToString(data["remark_other"]);

                        //////////////////////////////////////////////////////////////////
                        cbTypeSendTaxNormal.Checked = Convert.IsDBNull(data["is_inv_normal"]) ? false : Convert.ToBoolean(data["is_inv_normal"]);
                        //////////////////////////////////////////////////////////////////
                        cbTypeSendTaxCustomer.Checked = Convert.IsDBNull(data["is_inv_customer_receive"]) ? false : Convert.ToBoolean(data["is_inv_customer_receive"]);
                        ///////////////////////////////////////////////////////////////////
                        cbTypeSendTaxPost.Checked = Convert.IsDBNull(data["is_inv_thaipost"]) ? false : Convert.ToBoolean(data["is_inv_thaipost"]);

                        if (cbTypeSendTaxPost.Checked)
                        {
                            txtTypeSendAttention.Attributes.Remove("disabled");
                        }
                        txtTypeSendAttention.Value = Convert.IsDBNull(data["thaipost_attn"]) ? string.Empty : Convert.ToString(data["thaipost_attn"]);
                        ////////////////////////////////////////////////////////////////////
                        cbTypeSendTaxOther.Checked = Convert.IsDBNull(data["is_inv_other"]) ? false : Convert.ToBoolean(data["is_inv_other"]);
                        if (cbTypeSendTaxOther.Checked)
                        {
                            txtTypeSendOther.Attributes.Remove("disabled");
                        }
                        txtTypeSendOther.Value = Convert.IsDBNull(data["other_description"]) ? string.Empty : Convert.ToString(data["other_description"]);

                        cbbQuotation.Value = Convert.IsDBNull(data["quotation_id"]) ? string.Empty : Convert.ToString(data["quotation_id"]);
                        txtAttentionName.Value = Convert.IsDBNull(data["attention_name"]) ? string.Empty : Convert.ToString(data["attention_name"]);

                        txtGrandTotal.Value = Convert.IsDBNull(data["grand_total"]) ? string.Empty : String.Format("{0:0,0.00}", Convert.ToDouble(data["grand_total"]) - Convert.ToDouble(data["vat_total"]));
                        txtProject.Value = Convert.IsDBNull(data["project_name"]) ? string.Empty : Convert.ToString(data["project_name"]);
                        txtSubject.Value = Convert.IsDBNull(data["quotation_subject"]) ? string.Empty : Convert.ToString(data["quotation_subject"]);

                        txtDateQuatationNo.Value = Convert.IsDBNull(data["quotation_date"]) ? string.Empty : Convert.ToString(Convert.ToDateTime(data["quotation_date"]).ToString("dd/MM/yyyy"));

                        txtCustomerName.Value = Convert.IsDBNull(data["company_name_tha"]) ? string.Empty : Convert.ToString(data["company_name_tha"]);

                        cbDiscountByItem.Checked = Convert.IsDBNull(data["is_discount_by_item"]) ? false : Convert.ToBoolean(data["is_discount_by_item"]);
                        cbDiscountBottomBill1.Checked = string.IsNullOrEmpty(Convert.ToString(data["discount1_type"])) ? false :
                                                                                            (Convert.ToString(data["discount1_type"]) == "N" ? false : true);
                        cbDiscountBottomBill2.Checked = string.IsNullOrEmpty(Convert.ToString(data["discount2_type"])) ? false :
                                                                                            (Convert.ToString(data["discount2_type"]) == "N" ? false : true);

                        cbbDiscountBottomBill1.Value = Convert.IsDBNull(data["discount1_type"]) ? string.Empty : Convert.ToString(data["discount1_type"]);
                        cbbDiscountBottomBill2.Value = Convert.IsDBNull(data["discount2_type"]) ? string.Empty : Convert.ToString(data["discount2_type"]);

                        cbShowVat.Checked = Convert.IsDBNull(data["is_vat"]) ? false : Convert.ToBoolean(data["is_vat"]);
                        rdoDescription.Checked = Convert.IsDBNull(data["is_product_description"]) ? false : !Convert.ToBoolean(data["is_product_description"]);
                        rdoQuotationLine.Checked = Convert.IsDBNull(data["is_product_description"]) ? false : Convert.ToBoolean(data["is_product_description"]);

                        hdQuotationType.Value = Convert.IsDBNull(data["quotation_type"]) ? string.Empty : Convert.ToString(data["quotation_type"]);
                        hdQuotationItemDiscount.Value = cbDiscountByItem.Checked ? "true" : "false";

                        txtTotal.Value = Convert.IsDBNull(data["total_amount"]) ? string.Empty : String.Format("{0:0,0.00}", Convert.ToDouble(data["total_amount"]));
                        txtRemark.Value = Convert.IsDBNull(data["remark"]) ? string.Empty : Convert.ToString(data["remark"]);

                        if (Convert.ToString(data["quotation_type"]) != "A")
                        {
                            btnService.Visible = false;
                        }
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
                        txtSumDiscount1.Value = Convert.IsDBNull(data["discount1_total"]) ? "0" : String.Format("{0:0,0.00}", Convert.ToDouble(data["discount1_total"]));
                        txtSumDiscount2.Value = Convert.IsDBNull(data["discount2_total"]) ? "0" : String.Format("{0:0,0.00}", Convert.ToDouble(data["discount2_total"]));

                        hdCustomerId.Value = Convert.IsDBNull(data["customer_id"]) ? string.Empty : Convert.ToString(data["customer_id"]);

                        var status = Convert.IsDBNull(data["sale_order_status"]) ? string.Empty : Convert.ToString(data["sale_order_status"]);
                        hdSaleOrderStatus.Value = status;
                        
                        if (status == "FL")
                        {
                            btnCancel.Visible = false;
                            btnNew.Visible = false;
                        }
                        if (status == "CF")
                        {
                            btnSave.Visible = true;
                            btnSave.InnerHtml = "<i class=\"fa fa-check-square-o\" aria-hidden=\"true\"></i>&nbsp;Complete";
                        }
                        if (status == "CF" || status == "CP")
                        {
                            detailQuotation.Visible = false;
                        }

                        if (status == "CP")
                        {
                            btnDraft.Visible = false;
                            btnSave.Visible = false;
                            btnCancel.Visible = false;
                        }
                        btnReportClient.Visible = true;

                        cbRefIssueNo.Checked = Convert.IsDBNull(data["is_ref_issue_no"]) ? false : Convert.ToBoolean(data["is_ref_issue_no"]);
                        if (cbRefIssueNo.Checked)
                        {
                            //cboRefIssueNo.Attributes.Remove("disabled");
                            //cboRefIssueNo.Value = Convert.ToString(data["ref_issue_stock_no"]);
                            var issueStockNo = Convert.ToString(data["ref_issue_stock_no"]);
                            cboRefIssueNo.Items.Add(new ListItem(issueStockNo, issueStockNo));
                            cboRefIssueNo.Value = issueStockNo;

                            refIssueNo.Style["display"] = "block";
                        }

                        var created_by = Convert.IsDBNull(data["created_by"]) ? 0 : Convert.ToInt32(data["created_by"]);
                        CheckPermission(created_by);
                    }
                    #endregion

                    #region SaleOrder Detail
                    var detail = dsSaleOrderData.Tables[1].AsEnumerable().ToList();
                    saleOrderDetailList = new List<SaleOrderDetail>();
                    foreach (var row in detail)
                    {
                        saleOrderDetailList.Add(new SaleOrderDetail()
                        {
                            discount_amount = Convert.IsDBNull(row["discount_amount"]) ? 0.0m : Convert.ToDecimal(row["discount_amount"]),
                            discount_percentage = Convert.IsDBNull(row["discount_percentage"]) ? 0.0m : Convert.ToDecimal(row["discount_percentage"]),
                            discount_total = Convert.IsDBNull(row["discount_total"]) ? 0.0m : Convert.ToDecimal(row["discount_total"]),
                            discount_type = Convert.IsDBNull(row["discount_type"]) ? string.Empty : Convert.ToString(row["discount_type"]),
                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                            is_deleted = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                            parent_id = Convert.IsDBNull(row["parent_id"]) ? 0 : Convert.ToInt32(row["parent_id"]),
                            product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                            product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                            qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["qty"]),
                            quotation_detail_id = Convert.IsDBNull(row["quotation_detail_id"]) ? 0 : Convert.ToInt32(row["quotation_detail_id"]),
                            qu_qty = Convert.IsDBNull(row["qu_qty"]) ? 0 : Convert.ToInt32(row["qu_qty"]),
                            saleorder_description = Convert.IsDBNull(row["saleorder_description"]) ? string.Empty : Convert.ToString(row["saleorder_description"]),
                            sales_order_no = Convert.IsDBNull(row["sale_order_no"]) ? string.Empty : Convert.ToString(row["sale_order_no"]),
                            sort_no = Convert.IsDBNull(row["sort_no"]) ? 1 : Convert.ToInt32(row["sort_no"]),
                            total_amount = Convert.IsDBNull(row["total_amount"]) ? 0.0m : Convert.ToDecimal(row["total_amount"]),
                            unit_code = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]),
                            unit_price = Convert.IsDBNull(row["unit_price"]) ? 0.0m : Convert.ToDecimal(row["unit_price"]),
                        });
                    }
                    #endregion

                    #region SaleOrder Notification
                    // Quotation Notification
                    foreach (var row in dsSaleOrderData.Tables[2].AsEnumerable())
                    {
                        notificationList.Add(new Notification()
                        {
                            description = Convert.IsDBNull(row["description"]) ? string.Empty : Convert.ToString(row["description"]),
                            display_notice_date = Convert.IsDBNull(row["notice_date"]) ? string.Empty : Convert.ToDateTime(row["notice_date"]).ToString("dd/MM/yyyy HH:mm"),//Convert.ToString(row["display_notice_date"]),
                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                            is_deleted = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                            notice_date = Convert.IsDBNull(row["notice_date"]) ? DateTime.MinValue : Convert.ToDateTime(row["notice_date"]),
                            notice_type = Convert.IsDBNull(row["description"]) ? string.Empty : Convert.ToString(row["description"]),
                            reference_id = Convert.IsDBNull(row["reference_id"]) ? 0 : Convert.ToInt32(row["reference_id"]),
                            reference_no = Convert.IsDBNull(row["reference_no"]) ? string.Empty : Convert.ToString(row["reference_no"]),
                            subject = Convert.IsDBNull(row["subject"]) ? string.Empty : Convert.ToString(row["subject"]),
                            topic = Convert.IsDBNull(row["topic"]) ? string.Empty : Convert.ToString(row["topic"])
                        });
                    }
                    #endregion SaleOrder Notification

                    #region SaleOrder Payment
                    // Quotation Notification
                    foreach (var row in dsSaleOrderData.Tables[3].AsEnumerable())
                    {
                        poDetailList.Add(new PODetail()
                        {
                            amount = Convert.IsDBNull(row["amount"]) ? 0.0m : Convert.ToDecimal(row["amount"]),
                            display_invoice_date = Convert.IsDBNull(row["inv_date"]) ? string.Empty : Convert.ToDateTime(row["inv_date"]).ToString("dd/MM/yyyy"),//Convert.ToString(row["display_notice_date"]),
                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                            //is_deleted = Convert.IsDBNull(row["is_deleted"]) ? false : Convert.ToBoolean(row["is_deleted"]),
                            cheque_date = Convert.IsDBNull(row["cheque_date"]) ? DateTime.MinValue : Convert.ToDateTime(row["cheque_date"]),
                            display_cheque_date = Convert.IsDBNull(row["cheque_date"]) ? string.Empty : Convert.ToDateTime(row["cheque_date"]).ToString("dd/MM/yyyy"),//Convert.ToString(row["display_notice_date"]),
                            display_payment_type = Convert.IsDBNull(row["display_payment_type"]) ? string.Empty : Convert.ToString(row["display_payment_type"]),
                            display_po_date = Convert.IsDBNull(row["ref_po_date"]) ? string.Empty : Convert.ToDateTime(row["ref_po_date"]).ToString("dd/MM/yyyy"),
                            invoice_date = Convert.IsDBNull(row["inv_date"]) ? string.Empty : Convert.ToDateTime(row["inv_date"]).ToString("dd/MM/yyyy"),
                            invoice_no = Convert.IsDBNull(row["inv_no"]) ? string.Empty : Convert.ToString(row["inv_no"]),
                            payment_type = Convert.IsDBNull(row["payment_type"]) ? 0 : Convert.ToInt32(row["payment_type"]),
                            percent_price = Convert.IsDBNull(row["percent_price"]) ? 0 : Convert.ToInt32(row["percent_price"]),
                            ref_po_date = Convert.IsDBNull(row["ref_po_date"]) ? string.Empty : Convert.ToDateTime(row["ref_po_date"]).ToString("dd/MM/yyyy"),
                            ref_po_no = Convert.IsDBNull(row["ref_po_no"]) ? string.Empty : Convert.ToString(row["ref_po_no"]),
                            //bill_date = Convert.IsDBNull(row["bill_date"]) ? DateTime.MinValue : Convert.ToDateTime(row["bill_date"]),
                            delivery_date = Convert.IsDBNull(row["delivery_date"]) ? DateTime.MinValue : Convert.ToDateTime(row["delivery_date"]),
                            //display_bill_date = Convert.IsDBNull(row["bill_date"]) ? string.Empty : Convert.ToDateTime(row["bill_date"]).ToString("dd/MM/yyyy"),
                            display_delivery_date = Convert.IsDBNull(row["delivery_date"]) ? string.Empty : Convert.ToDateTime(row["delivery_date"]).ToString("dd/MM/yyyy"),
                            credit_day = Convert.IsDBNull(row["credit_day"]) ? 0 : Convert.ToInt32(row["credit_day"]),
                            period_no = Convert.IsDBNull(row["period_no"]) ? 0 : Convert.ToInt32(row["period_no"]),
                            period_amount = Convert.IsDBNull(row["period_amount"]) ? 0 : Convert.ToDecimal(row["period_amount"]),
                            diff_deposit = Convert.IsDBNull(row["diff_deposit"]) ? 0 : Convert.ToDecimal(row["diff_deposit"]),
                            temp_delivery_no = Convert.IsDBNull(row["temp_delivery_no"]) ? string.Empty : Convert.ToString(row["temp_delivery_no"]),

                        });
                    }
                    #endregion SaleOrder Payment

                    #region Quotation Detail Data
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@quotation_no", SqlDbType.VarChar,20) { Value = cbbQuotation.Text },
                            new SqlParameter("@service_type", SqlDbType.VarChar, 100) { Value = ConstantClass.SESSION_DEPARTMENT_SERVICE_TYPE },

                        };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_quotation_detail_list_so", arrParm.ToArray());
                        conn.Close();
                    }
                    if (quotationData == null || quotationData.Count == 0) // โหลดใหม่เฉพาะ Quotation Detail = 0 
                    {
                        if (dsResult != null)
                        {
                            quotationData = new List<QuotationDetail>();
                            var row = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                            foreach (var detailRow in row)
                            {
                                quotationData.Add(new QuotationDetail()
                                {
                                    id = Convert.ToInt32(detailRow["id"]),
                                    product_id = Convert.IsDBNull(detailRow["product_id"]) ? 0 : Convert.ToInt32(detailRow["product_id"]),
                                    quotation_no = Convert.IsDBNull(detailRow["quotation_no"]) ? string.Empty : Convert.ToString(detailRow["quotation_no"]),
                                    quotation_description = Convert.IsDBNull(detailRow["quotation_description"]) ? string.Empty : Convert.ToString(detailRow["quotation_description"]),
                                    is_selected = false,
                                    qty = Convert.IsDBNull(detailRow["qty"]) ? 0 : Convert.ToInt32(detailRow["qty"]),
                                    qu_qty = Convert.IsDBNull(detailRow["qty_remain"]) ? 0 : Convert.ToInt32(detailRow["qty_remain"]),
                                    unit_price = Convert.IsDBNull(detailRow["unit_price"]) ? 0 : Convert.ToDecimal(detailRow["unit_price"]),
                                    unit_code = Convert.IsDBNull(detailRow["unit_code"]) ? string.Empty : Convert.ToString(detailRow["unit_code"]),
                                    total_amount = Convert.IsDBNull(detailRow["total_amount"]) ? 0 : Convert.ToDecimal(detailRow["total_amount"]),
                                    sort_no = Convert.IsDBNull(detailRow["sort_no"]) ? 1 : Convert.ToInt32(detailRow["sort_no"]),
                                    discount_amount = Convert.IsDBNull(detailRow["discount_amount"]) ? 0 : Convert.ToDecimal(detailRow["discount_amount"]),
                                    discount_percentage = Convert.IsDBNull(detailRow["discount_percentage"]) ? 1 : Convert.ToDecimal(detailRow["discount_percentage"]),
                                    discount_total = Convert.IsDBNull(detailRow["discount_total"]) ? 0 : Convert.ToDecimal(detailRow["discount_total"]),
                                    discount_type = Convert.IsDBNull(detailRow["discount_type"]) ? string.Empty : Convert.ToString(detailRow["discount_type"]),
                                    //min_qty = Convert.IsDBNull(detailRow["min_qty"]) ? 0 : Convert.ToInt32(detailRow["min_qty"]),
                                    min_unit_price = Convert.IsDBNull(detailRow["min_selling_price"]) ? 0 : Convert.ToDecimal(detailRow["min_selling_price"]),
                                    parant_id = Convert.IsDBNull(detailRow["parent_id"]) ? 0 : Convert.ToInt32(detailRow["parent_id"]),
                                    product_no = Convert.IsDBNull(detailRow["product_no"]) ? string.Empty : Convert.ToString(detailRow["product_no"]),
                                    quotation_line = Convert.IsDBNull(detailRow["quotation_line"]) ? string.Empty : Convert.ToString(detailRow["quotation_line"]),

                                });
                            }

                        }
                    }
                    #endregion

                    gridViewDetailSaleOrder.DataSource = saleOrderDetailList;
                    gridViewDetailSaleOrder.DataBind();

                    gridViewNotice.DataSource = notificationList;
                    gridViewNotice.DataBind();

                    gridViewPO.DataSource = poDetailList;
                    gridViewPO.DataBind();

                    gridViewDetailList.DataSource = quotationData;
                    gridViewDetailList.DataBind();

                    cbbQuotation.Attributes.Add("disabled", "disabled");
                    cbbQuotation.Attributes.Add("readonly", "readonly");

                    foreach (var row in saleOrderDetailList)
                    {
                        cbbDiscountByItem.Value = row.discount_type;
                        break;
                    }

                    if (cbDiscountByItem.Checked)
                    {
                        if (cbbDiscountByItem.Value == "P")
                        {
                            gridViewDetailSaleOrder.Columns[5].Visible = false;
                            gridViewDetailSaleOrder.Columns[6].Visible = true;
                        }
                        else if (cbbDiscountByItem.Value == "A")
                        {
                            gridViewDetailSaleOrder.Columns[5].Visible = true;
                            gridViewDetailSaleOrder.Columns[6].Visible = false;
                        }
                    }
                    else
                    {
                        gridViewDetailSaleOrder.Columns[5].Visible = false;
                        gridViewDetailSaleOrder.Columns[6].Visible = false;
                    }
                }

            }
            else
            {
                btnService.Visible = false;
                btnReportClient.Visible = false;
                btnSave.Visible = false;
                btnCancel.Visible = false;
            }
        }

        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            return result.OfType<FilterControlColumn>();
        }

        #region Quotation Detail
        [WebMethod]
        public static List<QuotationDetail> AddSaleOrderDetail(string id, bool isSelected)
        {
            var data = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUATATION_DETAIL_SALE_ORDER"];
            // var saleOrderDetail = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL"];
            var selectedSaleOrder = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SELECTED_SALE_ORDER_DETAIL"];
            var selectedQuotationDetailRow = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault(); // เลือก Data จาก Quotation Detail
            if (data != null)
            {
                if (selectedSaleOrder != null) // มี SaleOrder Detail
                {
                    var rowSaleOrderDetail = (from t in selectedSaleOrder where t.quotation_detail_id == Convert.ToInt32(id) select t).FirstOrDefault(); // เลือก Data จาก Sale ORder Detail

                    if (selectedQuotationDetailRow != null)
                    {
                        if (rowSaleOrderDetail != null) // มีอยุ่ใน SaleOrder Detail
                        {
                            if (isSelected) // Add
                            {
                                selectedQuotationDetailRow.is_selected = true;
                                if (Convert.ToInt32(rowSaleOrderDetail.id) < 0) // กรณี Newmode
                                {
                                    selectedSaleOrder.Add(new SaleOrderDetail()
                                    {
                                        id = (selectedSaleOrder.Count + 1) * -1,
                                        discount_amount = selectedQuotationDetailRow.discount_amount,
                                        discount_percentage = selectedQuotationDetailRow.discount_percentage,
                                        discount_total = selectedQuotationDetailRow.discount_total,
                                        discount_type = selectedQuotationDetailRow.discount_type,
                                        parent_id = selectedQuotationDetailRow.parant_id,
                                        product_id = selectedQuotationDetailRow.product_id,
                                        product_no = selectedQuotationDetailRow.product_no,
                                        qty = selectedQuotationDetailRow.qty,
                                        qu_qty = selectedQuotationDetailRow.qu_qty,
                                        quotation_detail_id = Convert.ToInt32(id),
                                        saleorder_description = selectedQuotationDetailRow.quotation_description,
                                        sort_no = selectedSaleOrder.Count == 0 ? 1 : (from t in selectedSaleOrder select t.sort_no).Max() + 1,
                                        total_amount = selectedQuotationDetailRow.total_amount,
                                        unit_code = selectedQuotationDetailRow.unit_code,
                                        unit_price = selectedQuotationDetailRow.unit_price,
                                        product_type = selectedQuotationDetailRow.product_type,
                                    });
                                }
                                else if (Convert.ToInt32(rowSaleOrderDetail.id) > 0) // กรณี EditMode
                                {
                                    rowSaleOrderDetail.is_deleted = false;
                                }
                            }
                            else // Remove
                            {
                                selectedQuotationDetailRow.is_selected = false;
                                if (Convert.ToInt32(rowSaleOrderDetail.id) < 0) // กรณี Newmode
                                {
                                    selectedSaleOrder.Remove(rowSaleOrderDetail); // ลบ SaleOrder Detail
                                }
                                else if (Convert.ToInt32(rowSaleOrderDetail.id) > 0) // กรณี EditMode
                                {
                                    rowSaleOrderDetail.is_deleted = false; // เปลี่ยน Flag เป็นลบ แล้ว เพิ่มกลับไปยัง Quotation

                                    data.Add(new QuotationDetail()
                                    {
                                        id = rowSaleOrderDetail.quotation_detail_id,
                                        discount_amount = rowSaleOrderDetail.discount_amount,
                                        discount_percentage = rowSaleOrderDetail.discount_percentage,
                                        discount_total = rowSaleOrderDetail.discount_total,
                                        discount_type = rowSaleOrderDetail.discount_type,
                                        is_selected = false,
                                        parant_id = rowSaleOrderDetail.parent_id,
                                        product_id = rowSaleOrderDetail.product_id,
                                        product_no = rowSaleOrderDetail.product_no,
                                        qty = rowSaleOrderDetail.qty,
                                        qu_qty = rowSaleOrderDetail.qu_qty,
                                        quotation_description = rowSaleOrderDetail.saleorder_description,
                                        quotation_line = rowSaleOrderDetail.saleorder_description,
                                        total_amount = rowSaleOrderDetail.total_amount,
                                        unit_code = rowSaleOrderDetail.unit_code,
                                        unit_price = rowSaleOrderDetail.unit_price,
                                        product_type = rowSaleOrderDetail.product_type,
                                    });

                                }
                            }
                        }
                        else // Add SaleOrder Detail
                        {
                            if (isSelected) // มีแค่กรณี Selected เป็น True เท่านั้น
                            {
                                selectedQuotationDetailRow.is_selected = true;
                                selectedSaleOrder.Add(new SaleOrderDetail()
                                {
                                    id = (selectedSaleOrder.Count + 1) * -1,
                                    discount_amount = selectedQuotationDetailRow.discount_amount,
                                    discount_percentage = selectedQuotationDetailRow.discount_percentage,
                                    discount_total = selectedQuotationDetailRow.discount_total,
                                    discount_type = selectedQuotationDetailRow.discount_type,
                                    parent_id = selectedQuotationDetailRow.parant_id,
                                    product_id = selectedQuotationDetailRow.product_id,
                                    product_no = selectedQuotationDetailRow.product_no,
                                    qty = selectedQuotationDetailRow.qty,
                                    qu_qty = selectedQuotationDetailRow.qu_qty,
                                    quotation_detail_id = Convert.ToInt32(id),
                                    saleorder_description = selectedQuotationDetailRow.quotation_description,
                                    sort_no = selectedSaleOrder.Count == 0 ? 1 : (from t in selectedSaleOrder select t.sort_no).Max() + 1,
                                    total_amount = selectedQuotationDetailRow.total_amount,
                                    unit_code = selectedQuotationDetailRow.unit_code,
                                    unit_price = selectedQuotationDetailRow.unit_price,
                                    product_type = selectedQuotationDetailRow.product_type,
                                });
                            }
                        }
                    }
                }
                else // Add SaleOrder Detail
                {
                    selectedSaleOrder = new List<SaleOrderDetail>();
                    if (selectedQuotationDetailRow != null)
                    {
                        if (isSelected) // มีแค่กรณี Selected เป็น True เท่านั้น
                        {
                            selectedQuotationDetailRow.is_selected = true;
                            selectedSaleOrder.Add(new SaleOrderDetail()
                            {
                                id = (selectedSaleOrder.Count + 1) * -1,
                                discount_amount = selectedQuotationDetailRow.discount_amount,
                                discount_percentage = selectedQuotationDetailRow.discount_percentage,
                                discount_total = selectedQuotationDetailRow.discount_total,
                                discount_type = selectedQuotationDetailRow.discount_type,
                                parent_id = selectedQuotationDetailRow.parant_id,
                                product_id = selectedQuotationDetailRow.product_id,
                                product_no = selectedQuotationDetailRow.product_no,
                                qty = selectedQuotationDetailRow.qty,
                                qu_qty = selectedQuotationDetailRow.qu_qty,
                                quotation_detail_id = Convert.ToInt32(id),
                                saleorder_description = selectedQuotationDetailRow.quotation_description,
                                sort_no = selectedSaleOrder.Count == 0 ? 1 : (from t in selectedSaleOrder select t.sort_no).Max() + 1,
                                total_amount = selectedQuotationDetailRow.total_amount,
                                unit_code = selectedQuotationDetailRow.unit_code,
                                unit_price = selectedQuotationDetailRow.unit_price,
                                product_type = selectedQuotationDetailRow.product_type,
                            });
                        }
                    }
                }
            }

            HttpContext.Current.Session["SESSION_QUATATION_DETAIL_SALE_ORDER"] = data; // ยัดค่ากลับ Session QUotation Detail Data
            HttpContext.Current.Session["SESSION_SELECTED_SALE_ORDER_DETAIL"] = selectedSaleOrder; // ยัดค่ากลับ Sesssion Sale Order detail data
            return data;
        }
        [WebMethod]
        public static QuotationData SubmitQuotationDetail()
        {
            var quotationSaleOrder = (QuotationData)HttpContext.Current.Session["SESSION_QUOTATION_DATA_SALE_ORDER"];
            var saleOrderDetail = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL"];
            var selectedSaleOrder = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SELECTED_SALE_ORDER_DETAIL"];
            var quotationData = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUATATION_DETAIL_SALE_ORDER"];
            var saleOrder_amount = 0.0m;
            if (selectedSaleOrder != null)
            {
                foreach (var selectedRow in selectedSaleOrder)
                {
                    var existItem = (from t in saleOrderDetail where t.id == selectedRow.id select t).FirstOrDefault();
                    if (existItem == null)
                    {
                        if (selectedRow.is_deleted == false)
                        {
                            saleOrderDetail.Add(selectedRow);
                        }
                        else
                        {
                            existItem.is_deleted = true;
                        }
                    }
                    else
                    {
                        if (selectedRow.id < 0)
                        {
                            if (selectedRow.is_deleted)
                            {
                                existItem.is_deleted = true;
                            }
                            else
                            {
                                existItem.is_deleted = false;
                            }
                            saleOrder_amount += selectedRow.total_amount;
                        }
                        else
                        {
                            if (selectedRow.is_deleted)
                            {
                                existItem.is_deleted = true;
                            }
                            else
                            {
                                existItem.is_deleted = false;
                            }

                            var rowQuotation = (from t in quotationData where t.id == selectedRow.quotation_detail_id select t).FirstOrDefault();
                            if (rowQuotation != null)
                            {
                                if (rowQuotation.is_selected)
                                {
                                    quotationData.Remove(rowQuotation);
                                }

                            }
                            saleOrder_amount += selectedRow.total_amount;
                        }
                    }
                }

                /////// Cal Discount///

                if (quotationSaleOrder != null)
                {
                    var discount1 = 0.0m;
                    var discount2 = 0.0m;
                    if (quotationSaleOrder.discount1_type != "")
                    {
                        //discount1 = quotationSaleOrder.discount1_percentage == 0.0m ? quotationSaleOrder.discount1_amount : (quotationSaleOrder.discount1_percentage * saleOrder_amount) / 100;
                        discount1 = quotationSaleOrder.discount1_total == 0.0m ? 0.0m : (saleOrder_amount * 100.0m / quotationSaleOrder.total_amount) * (quotationSaleOrder.discount1_total / 100.0m);
                    }
                    if (quotationSaleOrder.discount2_type != "")
                    {
                        //discount2 = quotationSaleOrder.discount2_percentage == 0.0m ? quotationSaleOrder.discount2_amount : (quotationSaleOrder.discount2_percentage * saleOrder_amount) / 100;
                        discount2 = quotationSaleOrder.discount2_total == 0.0m ? 0.0m : (saleOrder_amount * 100.0m / quotationSaleOrder.total_amount) * (quotationSaleOrder.discount2_total / 100.0m);
                    }

                    quotationSaleOrder.total_amount = saleOrder_amount;
                    quotationSaleOrder.discount1_total = Math.Round(discount1, 2, MidpointRounding.AwayFromZero);
                    quotationSaleOrder.discount2_total = Math.Round(discount2, 2, MidpointRounding.AwayFromZero);
                    var grandTotalNoVat = (saleOrder_amount - discount1) - discount2;
                    quotationSaleOrder.vat_total = quotationSaleOrder.is_vat ? Math.Round(grandTotalNoVat * ConstantClass.VAT / 100, 2, MidpointRounding.AwayFromZero) : 0.0m;
                    quotationSaleOrder.grand_total = grandTotalNoVat + quotationSaleOrder.vat_total;
                }
            }
            HttpContext.Current.Session["SESSION_QUOTATION_DATA_SALE_ORDER"] = quotationSaleOrder;
            HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL"] = selectedSaleOrder;
            HttpContext.Current.Session["SESSION_SELECTED_SALE_ORDER_DETAIL"] = null;
            HttpContext.Current.Session["SESSION_QUATATION_DETAIL_SALE_ORDER"] = quotationData;
            return quotationSaleOrder;
        }
        [WebMethod]
        public static List<QuotationDetail> GetQuotationDetailData(string id)
        {
            var dsResult = new DataSet();
            var quotationData = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUATATION_DETAIL_SALE_ORDER"];
            var saleOrderDetail = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL"];

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@quotation_no", SqlDbType.VarChar,20) { Value = id },
                            new SqlParameter("@service_type", SqlDbType.VarChar, 100) { Value = ConstantClass.SESSION_DEPARTMENT_SERVICE_TYPE },

                        };
                conn.Open();
                dsResult = SqlHelper.ExecuteDataset(conn, "sp_quotation_detail_list_so", arrParm.ToArray());
                conn.Close();
            }
            if (quotationData == null || quotationData.Count == 0) // โหลดใหม่เฉพาะ Quotation Detail = 0 
            {
                if (dsResult != null)
                {
                    quotationData = new List<QuotationDetail>();
                    var row = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                    foreach (var detail in row)
                    {
                        quotationData.Add(new QuotationDetail()
                        {
                            id = Convert.ToInt32(detail["id"]),
                            product_id = Convert.IsDBNull(detail["product_id"]) ? 0 : Convert.ToInt32(detail["product_id"]),
                            quotation_no = Convert.IsDBNull(detail["quotation_no"]) ? string.Empty : Convert.ToString(detail["quotation_no"]),
                            quotation_description = Convert.IsDBNull(detail["quotation_description"]) ? string.Empty : Convert.ToString(detail["quotation_description"]),
                            is_selected = false,
                            qty = Convert.IsDBNull(detail["qty"]) ? 0 : Convert.ToInt32(detail["qty"]),
                            qu_qty = Convert.IsDBNull(detail["qty_remain"]) ? 0 : Convert.ToInt32(detail["qty_remain"]),
                            unit_price = Convert.IsDBNull(detail["unit_price"]) ? 0 : Convert.ToDecimal(detail["unit_price"]),
                            unit_code = Convert.IsDBNull(detail["unit_code"]) ? string.Empty : Convert.ToString(detail["unit_code"]),
                            total_amount = Convert.IsDBNull(detail["total_amount"]) ? 0 : Convert.ToDecimal(detail["total_amount"]),
                            sort_no = Convert.IsDBNull(detail["sort_no"]) ? 1 : Convert.ToInt32(detail["sort_no"]),
                            discount_amount = Convert.IsDBNull(detail["discount_amount"]) ? 0 : Convert.ToDecimal(detail["discount_amount"]),
                            discount_percentage = Convert.IsDBNull(detail["discount_percentage"]) ? 1 : Convert.ToDecimal(detail["discount_percentage"]),
                            discount_total = Convert.IsDBNull(detail["discount_total"]) ? 0 : Convert.ToDecimal(detail["discount_total"]),
                            discount_type = Convert.IsDBNull(detail["discount_type"]) ? string.Empty : Convert.ToString(detail["discount_type"]),
                            //min_qty = Convert.IsDBNull(detail["min_qty"]) ? 0 : Convert.ToInt32(detail["min_qty"]),
                            min_unit_price = Convert.IsDBNull(detail["min_selling_price"]) ? 0 : Convert.ToDecimal(detail["min_selling_price"]),
                            parant_id = Convert.IsDBNull(detail["parent_id"]) ? 0 : Convert.ToInt32(detail["parent_id"]),
                            product_no = Convert.IsDBNull(detail["product_no"]) ? string.Empty : Convert.ToString(detail["product_no"]),
                            quotation_line = Convert.IsDBNull(detail["quotation_line"]) ? string.Empty : Convert.ToString(detail["quotation_line"]),
                            product_type = Convert.IsDBNull(detail["product_type"]) ? string.Empty : Convert.ToString(detail["product_type"]),

                        });
                    }

                }
            }
            HttpContext.Current.Session["SESSION_SELECTED_SALE_ORDER_DETAIL"] = saleOrderDetail; // ย้ายดาต้าจริงเข้า Session จำลอง
            HttpContext.Current.Session["SESSION_QUATATION_DETAIL_SALE_ORDER"] = quotationData;
            return quotationData;
        }
        [WebMethod]
        public static QuotationData GetQuotationData(string id)
        {
            var dataSelected = new DataSet();
            var dataReturn = (QuotationData)HttpContext.Current.Session["SESSION_QUOTATION_DATA_SALE_ORDER"];
            var quotationData = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUATATION_DETAIL_SALE_ORDER"];
            var saleOrderDetail = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL"];
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(id) },
                            new SqlParameter("@search_name", SqlDbType.Int) { Value = "" },
                            new SqlParameter("@service_type", SqlDbType.VarChar, 100) { Value = ConstantClass.SESSION_DEPARTMENT_SERVICE_TYPE },
                        };
                conn.Open();
                dataSelected = SqlHelper.ExecuteDataset(conn, "sp_quotation_header_list", arrParm.ToArray());
                conn.Close();
            }
            quotationData = new List<QuotationDetail>(); // clear detail ทุกครั้งที่เปลี่ยน header
            saleOrderDetail = new List<SaleOrderDetail>(); // clear detail ทุกครั้งที่เปลี่ยน header
            dataReturn = new QuotationData();
            HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL"] = saleOrderDetail; // ยัดค่ากลับ Session
            HttpContext.Current.Session["SESSION_QUATATION_DETAIL_SALE_ORDER"] = quotationData; // ยัดค่ากลับ Session
            if (dataSelected != null)
            {
                var row = (from t in dataSelected.Tables[0].AsEnumerable() select t).FirstOrDefault();

                if (row != null)
                {
                    dataReturn.attention_name = Convert.IsDBNull(row["attention_name"]) ? string.Empty : Convert.ToString(row["attention_name"]);
                    dataReturn.customer_address = Convert.IsDBNull(row["address_bill_eng"]) ? string.Empty : Convert.ToString(row["address_bill_eng"]);
                    dataReturn.customer_fax = Convert.IsDBNull(row["fax"]) ? string.Empty : Convert.ToString(row["fax"]);
                    dataReturn.customer_id = Convert.IsDBNull(row["customer_id"]) ? 0 : Convert.ToInt32(row["customer_id"]);
                    dataReturn.customer_name = Convert.IsDBNull(row["company_name_tha"]) ? string.Empty : Convert.ToString(row["company_name_tha"]);
                    dataReturn.customer_tel = Convert.IsDBNull(row["tel"]) ? string.Empty : Convert.ToString(row["tel"]);
                    dataReturn.id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]);
                    dataReturn.project_name = Convert.IsDBNull(row["project_name"]) ? string.Empty : Convert.ToString(row["project_name"]);
                    dataReturn.quotation_date = Convert.IsDBNull(row["quotation_date"]) ? DateTime.MinValue : Convert.ToDateTime(row["quotation_date"]);
                    dataReturn.quotation_no = Convert.IsDBNull(row["quotation_no"]) ? string.Empty : Convert.ToString(row["quotation_no"]);
                    dataReturn.quotation_status = Convert.IsDBNull(row["quotation_status"]) ? string.Empty : Convert.ToString(row["quotation_status"]);
                    dataReturn.quotation_subject = Convert.IsDBNull(row["quotation_subject"]) ? string.Empty : Convert.ToString(row["quotation_subject"]);
                    dataReturn.quotation_type = Convert.IsDBNull(row["quotation_type"]) ? string.Empty : Convert.ToString(row["quotation_type"]);
                    dataReturn.sale_id = Convert.IsDBNull(row["sales_id"]) ? 0 : Convert.ToInt32(row["sales_id"]);
                    dataReturn.sale_name = string.Empty;//Convert.IsDBNull(row["address_bill_tha"]) ? string.Empty : Convert.ToString(row["address_bill_tha"]);
                    dataReturn.customer_code = Convert.IsDBNull(row["customer_code"]) ? string.Empty : Convert.ToString(row["customer_code"]);
                    dataReturn.display_quotation_date = Convert.IsDBNull(row["quotation_date"]) ? string.Empty : Convert.ToDateTime(row["quotation_date"]).ToString("dd/MM/yyyy");


                    dataReturn.is_discount_by_item = Convert.IsDBNull(row["is_discount_by_item"]) ? false : Convert.ToBoolean(row["is_discount_by_item"]);
                    dataReturn.discount1_type = Convert.IsDBNull(row["discount1_type"]) ? string.Empty : Convert.ToString(row["discount1_type"]);
                    dataReturn.discount1_percentage = Convert.IsDBNull(row["discount1_percentage"]) ? 0.0m : Convert.ToDecimal(row["discount1_percentage"]);
                    dataReturn.discount1_amount = Convert.IsDBNull(row["discount1_amount"]) ? 0.0m : Convert.ToDecimal(row["discount1_amount"]);
                    dataReturn.discount1_total = Convert.IsDBNull(row["discount1_total"]) ? 0.0m : Convert.ToDecimal(row["discount1_total"]);
                    dataReturn.discount2_type = Convert.IsDBNull(row["discount2_type"]) ? string.Empty : Convert.ToString(row["discount2_type"]);
                    dataReturn.discount2_percentage = Convert.IsDBNull(row["discount2_percentage"]) ? 0.0m : Convert.ToDecimal(row["discount2_percentage"]);
                    dataReturn.discount2_amount = Convert.IsDBNull(row["discount2_amount"]) ? 0.0m : Convert.ToDecimal(row["discount2_amount"]);
                    dataReturn.discount2_total = Convert.IsDBNull(row["discount2_total"]) ? 0.0m : Convert.ToDecimal(row["discount2_total"]);
                    dataReturn.total_amount = Convert.IsDBNull(row["total_amount"]) ? 0.0m : Convert.ToDecimal(row["total_amount"]);
                    dataReturn.grand_total = Convert.IsDBNull(row["grand_total"]) ? 0.0m : Convert.ToDecimal(row["grand_total"]);
                    dataReturn.is_vat = Convert.IsDBNull(row["is_vat"]) ? false : Convert.ToBoolean(row["is_vat"]);
                    dataReturn.vat_total = Convert.IsDBNull(row["vat_total"]) ? 0.0m : Convert.ToDecimal(row["vat_total"]);
                    dataReturn.is_product_description = Convert.IsDBNull(row["is_product_description"]) ? false : Convert.ToBoolean(row["is_product_description"]);
                    dataReturn.discount_item_type = Convert.IsDBNull(row["discount_item_type"]) ? string.Empty : Convert.ToString(row["discount_item_type"]);

                }
            }
            HttpContext.Current.Session["SESSION_QUOTATION_DATA_SALE_ORDER"] = dataReturn;
            return dataReturn;
        }

        protected void gridViewDetailList_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxCheckBox checkBox = gridViewDetailList.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkQuotationDetail") as ASPxCheckBox;
                if (checkBox != null)
                {
                    if (e.DataColumn.FieldName == "is_selected")
                    {
                        if (quotationData != null)
                        {
                            var row = (from t in quotationData where t.id == Convert.ToInt32(e.KeyValue) select t).FirstOrDefault();
                            if (row != null)
                            {
                                checkBox.Checked = row.is_selected;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gridViewDetailList_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var splitStr = e.Parameters.ToString().Split('|');
            string searchText = string.Empty;
            if (splitStr.Length > 1)
            {
                searchText = splitStr[1];
            }

            if (string.IsNullOrEmpty(searchText))
            {
                gridViewDetailList.DataSource = quotationData;//(from t in quotationData select t).ToList();
                gridViewDetailList.DataBind();
            }
            else
            {
                gridViewDetailList.DataSource = quotationData.Where(t => t.quotation_description.ToUpper().Contains(searchText.ToUpper()) || t.product_no.ToUpper().Contains(searchText.ToUpper())).ToList();
                gridViewDetailList.DataBind();
            }
            gridViewDetailList.PageIndex = 0;
        }

        protected void BindGridQuotationDetail()
        {
            gridViewDetailList.DataSource = (from t in quotationData where t.is_deleted == false select t).ToList();
            gridViewDetailList.DataBind();
        }
        protected void BindGridNoticeDetail()
        {
            try
            {
                gridViewNotice.DataSource = (from t in notificationList where t.is_deleted == false && t.topic == "SO" select t).ToList();
                gridViewNotice.FilterExpression = FilterBag.GetExpression(false);
                gridViewNotice.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void BindGridPODetail()
        {
            gridViewPO.DataSource = (from t in poDetailList
                                     where t.is_deleted == false
                                     select t).ToList();
            gridViewPO.DataBind();
        }
        [WebMethod]
        public static string SelectAllQuotation(bool selected)
        {
            var data = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUATATION_DETAIL_SALE_ORDER"];
            // var saleOrderDetail = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL"];
            var selectedSaleOrder = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SELECTED_SALE_ORDER_DETAIL"];
            if (data != null)
            {
                foreach (var row in data)
                {
                    if (selected)
                    {
                        row.is_selected = true;
                        if (selectedSaleOrder.Count > 0) // มี SaleOrder Detail
                        {
                            var rowExist = (from t in selectedSaleOrder where t.quotation_detail_id == row.id && !t.is_deleted select t).FirstOrDefault();

                            if (rowExist != null) // กรณี Newmode
                            {
                                rowExist.qty = row.qty;
                            }
                            else
                            {
                                selectedSaleOrder.Add(new SaleOrderDetail()
                                {
                                    id = (selectedSaleOrder.Count + 1) * -1,
                                    discount_amount = row.discount_amount,
                                    discount_percentage = row.discount_percentage,
                                    discount_total = row.discount_total,
                                    discount_type = row.discount_type,
                                    parent_id = row.parant_id,
                                    product_id = row.product_id,
                                    product_no = row.product_no,
                                    qty = row.qty,
                                    qu_qty = row.qu_qty,
                                    quotation_detail_id = Convert.ToInt32(row.id),
                                    saleorder_description = row.quotation_description,
                                    sort_no = selectedSaleOrder.Count == 0 ? 1 : (from t in selectedSaleOrder select t.sort_no).Max() + 1,
                                    total_amount = row.total_amount,
                                    unit_code = row.unit_code,
                                    unit_price = row.unit_price,
                                    product_type = row.product_type
                                });
                            }

                        }
                        else
                        {
                            selectedSaleOrder.Add(new SaleOrderDetail()
                            {
                                id = (selectedSaleOrder.Count + 1) * -1,
                                discount_amount = row.discount_amount,
                                discount_percentage = row.discount_percentage,
                                discount_total = row.discount_total,
                                discount_type = row.discount_type,
                                parent_id = row.parant_id,
                                product_id = row.product_id,
                                product_no = row.product_no,
                                qty = row.qty,
                                qu_qty = row.qu_qty,
                                quotation_detail_id = Convert.ToInt32(row.id),
                                saleorder_description = row.quotation_description,
                                sort_no = selectedSaleOrder.Count == 0 ? 1 : (from t in selectedSaleOrder select t.sort_no).Max() + 1,
                                total_amount = row.total_amount,
                                unit_code = row.unit_code,
                                unit_price = row.unit_price,
                                product_type = row.product_type
                            });
                        }
                    }
                    else
                    {
                        var rowExist = (from t in selectedSaleOrder where t.quotation_detail_id == row.id && !t.is_deleted select t).FirstOrDefault();
                        row.is_selected = false;
                        if (rowExist != null)
                        {
                            if (rowExist.id < 0) // กรณี Newmode
                            {
                                selectedSaleOrder.Remove(rowExist);
                            }
                            else
                            {
                                rowExist.is_deleted = true;
                            }
                        }
                    }
                }
            }
            HttpContext.Current.Session["SESSION_QUATATION_DETAIL_SALE_ORDER"] = data; // ยัดค่ากลับ Session QUotation Detail Data
            HttpContext.Current.Session["SESSION_SELECTED_SALE_ORDER_DETAIL"] = selectedSaleOrder; // ยัดค่ากลับ Sesssion Sale Order detail data
            return "SELECT ALL";
        }

        #endregion

        #region Sale Order Detail
        [WebMethod]
        public static SaleOrderDetail EditSaleOrderDetail(string id)
        {
            var saleOrderDetail = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL"];
            var selectedSaleOrder = new SaleOrderDetail(); // new เสมอ เพราะเราต้องการ record ล่าสุดที่เลือก
            if (saleOrderDetail != null)
            {
                var row = (from t in saleOrderDetail where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (row != null)
                {
                    selectedSaleOrder = row;
                }
            }
            // 
            return selectedSaleOrder;
        }
        [WebMethod]
        public static List<SaleOrderDetail> DeleteSaleOrderDetail(string id)
        {
            var saleOrderDetail = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL"];
            var quotationData = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUATATION_DETAIL_SALE_ORDER"];
            var rowDeletedSaleOrder = (from t in saleOrderDetail where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
            var rowQuotationDetail = (from t in quotationData where t.id == rowDeletedSaleOrder.quotation_detail_id select t).FirstOrDefault();
            if (Convert.ToInt32(id) < 0)
            {
                saleOrderDetail.Remove(rowDeletedSaleOrder);
                rowQuotationDetail.is_selected = false;
            }
            else
            {
                rowDeletedSaleOrder.is_deleted = true;
                quotationData.Add(new QuotationDetail()
                {
                    id = rowDeletedSaleOrder.quotation_detail_id,
                    qty = rowDeletedSaleOrder.qty,
                    qu_qty = rowDeletedSaleOrder.qu_qty,
                    product_id = rowDeletedSaleOrder.product_id,
                    product_no = rowDeletedSaleOrder.product_no,
                    quotation_description = rowDeletedSaleOrder.saleorder_description,
                    unit_code = rowDeletedSaleOrder.unit_code,
                    sort_no = rowDeletedSaleOrder.sort_no,
                    unit_price = rowDeletedSaleOrder.unit_price,
                    total_amount = rowDeletedSaleOrder.total_amount,
                    discount_amount = rowDeletedSaleOrder.discount_amount,
                    discount_percentage = rowDeletedSaleOrder.discount_percentage,
                    discount_total = rowDeletedSaleOrder.discount_total,
                    discount_type = rowDeletedSaleOrder.discount_type,
                    product_type = rowDeletedSaleOrder.product_type,

                });
            }
            HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL"] = saleOrderDetail;
            HttpContext.Current.Session["SESSION_QUATATION_DETAIL_SALE_ORDER"] = quotationData;

            return saleOrderDetail;
        }
        [WebMethod]
        public static QuotationData SubmitSaleOrderDetail(string id, string qty, string unit_price, string amount, string percent, string discountType)
        {

            List<SaleOrderDetail> data = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL"];

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

                }
            }
            HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL"] = data;
            HttpContext.Current.Session["SESSION_SELECTED_SALE_ORDER_DETAIL"] = null;

            /////////////// /////// Cal Discount/////////////////////////

            var saleOrder_amount = 0.0m;

            var quotationSaleOrder = (QuotationData)HttpContext.Current.Session["SESSION_QUOTATION_DATA_SALE_ORDER"];
            var saleOrderDetail = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL"];

            if (saleOrderDetail != null)
            {
                foreach (var dataSaleOrderDetail in saleOrderDetail)
                {
                    saleOrder_amount += dataSaleOrderDetail.total_amount;
                }

                if (quotationSaleOrder != null)
                {
                    var discount1 = 0.0m;
                    var discount2 = 0.0m;
                    if (quotationSaleOrder.discount1_type != "")
                    {
                        discount1 = quotationSaleOrder.discount1_percentage == 0.0m ? quotationSaleOrder.discount1_amount : (quotationSaleOrder.discount1_percentage * saleOrder_amount) / 100;
                    }
                    if (quotationSaleOrder.discount2_type != "")
                    {
                        discount2 = quotationSaleOrder.discount2_percentage == 0.0m ? quotationSaleOrder.discount2_amount : (quotationSaleOrder.discount2_percentage * saleOrder_amount) / 100;
                    }

                    quotationSaleOrder.total_amount = saleOrder_amount;
                    quotationSaleOrder.discount1_total = Math.Round(discount1, 2, MidpointRounding.AwayFromZero);
                    quotationSaleOrder.discount2_total = Math.Round(discount2, 2, MidpointRounding.AwayFromZero);
                    var grandTotalNoVat = (saleOrder_amount - discount1) - discount2;
                    quotationSaleOrder.vat_total = quotationSaleOrder.is_vat ? Math.Round(grandTotalNoVat * ConstantClass.VAT / 100, 2, MidpointRounding.AwayFromZero) : 0.0m;
                    quotationSaleOrder.grand_total = grandTotalNoVat + quotationSaleOrder.vat_total;
                }
            }
            HttpContext.Current.Session["SESSION_QUOTATION_DATA_SALE_ORDER"] = quotationSaleOrder;


            return quotationSaleOrder;
        }
        [WebMethod]
        public static ProductDetailValue CalcurateDiscount(string discount_type)
        {
            List<SaleOrderDetail> data = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL"];
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
                    }
                    else if (discount_type == "A")
                    {
                        returnData.discount_total += row.discount_amount;
                    }
                    else
                    {
                        returnData.discount_total = 0;
                    }
                }
            }
            return returnData;
        }
        protected void gridViewDetailSaleOrder_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (cbDiscountByItem.Checked)
            {
                if (cbbDiscountByItem.Value == "P")
                {
                    gridViewDetailSaleOrder.Columns[5].Visible = false;
                    gridViewDetailSaleOrder.Columns[6].Visible = true;
                }
                else if (cbbDiscountByItem.Value == "A")
                {
                    gridViewDetailSaleOrder.Columns[5].Visible = true;
                    gridViewDetailSaleOrder.Columns[6].Visible = false;
                }
            }
            else
            {
                gridViewDetailSaleOrder.Columns[5].Visible = false;
                gridViewDetailSaleOrder.Columns[6].Visible = false;
            }
            gridViewDetailSaleOrder.DataSource = (from t in saleOrderDetailList where t.is_deleted == false select t).ToList();
            gridViewDetailSaleOrder.DataBind();


        }
        protected void BindGridSaleOrderDetail()
        {
            if (cbDiscountByItem.Checked)
            {
                if (cbbDiscountByItem.Value == "P")
                {
                    gridViewDetailSaleOrder.Columns[5].Visible = false;
                    gridViewDetailSaleOrder.Columns[6].Visible = true;
                }
                else if (cbbDiscountByItem.Value == "A")
                {
                    gridViewDetailSaleOrder.Columns[5].Visible = true;
                    gridViewDetailSaleOrder.Columns[6].Visible = false;
                }
            }
            else
            {
                gridViewDetailSaleOrder.Columns[5].Visible = false;
                gridViewDetailSaleOrder.Columns[6].Visible = false;
            }
            gridViewDetailSaleOrder.DataSource = (from t in saleOrderDetailList where t.is_deleted == false select t).ToList();
            gridViewDetailSaleOrder.DataBind();
        }
        #endregion

        #region PO Detail
        [WebMethod]
        public static List<PODetail> AddPO(string id, string po_no, string po_date, string payment_type
                                            , string payment_val, string invoice_no, string invoice_date, string temp_no_delivery, string delivery_date, string credit_day, string period_no, string period_amount, string diff_deposit)
        {
            try
            {
                List<PODetail> dataPO = (List<PODetail>)HttpContext.Current.Session["SESSION_PO_DETAIL_SALE_ORDER"];

                //
                DateTime? po_dt = null;
                if (!string.IsNullOrEmpty(po_date))
                {
                    po_dt = DateTime.ParseExact(po_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                DateTime? invoice_dt = null;
                if (!string.IsNullOrEmpty(invoice_date))
                {
                    invoice_dt = DateTime.ParseExact(invoice_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                DateTime? payment_dt = null;
                if (payment_type == "2")
                {
                    if (!string.IsNullOrEmpty(payment_val))
                    {
                        payment_dt = DateTime.ParseExact(payment_val, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }

                    payment_val = "100";
                }
                //DateTime? bill_dt = null;
                DateTime? delivery_dt = null;
                //if (!string.IsNullOrEmpty(bill_date))
                //{
                //    bill_dt = DateTime.ParseExact(bill_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //}
                if (!string.IsNullOrEmpty(delivery_date))
                {
                    delivery_dt = DateTime.ParseExact(delivery_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                List<string> displayPaymentType = new List<string>();
                displayPaymentType.Add("เงินสด");
                displayPaymentType.Add("รับเช็คล่วงหน้า ณ วันที่ส่งของ");
                displayPaymentType.Add("เครดิต");
                displayPaymentType.Add("มัดจำ ก่อนส่งให้ลูกค้า");
                displayPaymentType.Add("ส่วนต่างมัดจำ นับจากวันที่ส่งของ");
                displayPaymentType.Add("แบ่งชำระ");
                //DateTime dateTime = DateTime.Parse(po_date);
                //var dateee = Convert.ToDateTime(po_date);
                if (dataPO == null) // Insert mode only
                {
                    dataPO = new List<PODetail>();

                    dataPO.Add(new PODetail
                    {
                        id = (dataPO.Count + 1) * -1,
                        ref_po_no = po_no,
                        ref_po_date = po_dt != null ? po_dt.Value.ToString("dd/MM/yyyy") : "",//dateTime,
                        is_deleted = false,
                        display_po_date = po_dt != null ? po_dt.Value.ToString("dd/MM/yyyy") : "",
                        payment_type = Convert.ToInt32(payment_type),
                        invoice_no = invoice_no,
                        invoice_date = invoice_dt != null ? invoice_dt.Value.ToString("dd/MM/yyyy") : "",
                        display_payment_type = displayPaymentType[Convert.ToInt32(payment_type) - 1],
                        cheque_date = payment_dt != null ? payment_dt.Value : payment_dt,
                        display_cheque_date = payment_dt != null ? payment_dt.Value.ToString("dd/MM/yyyy") : "",
                        display_invoice_date = invoice_dt != null ? invoice_dt.Value.ToString("dd/MM/yyyy") : "",
                        percent_price = payment_val == "" ? 100 : Convert.ToInt32(payment_val),
                        //bill_date = bill_dt,
                        //display_bill_date = bill_dt != null ? bill_dt.Value.ToString("dd/MM/yyyy") : string.Empty,
                        delivery_date = delivery_dt,
                        display_delivery_date = delivery_dt != null ? delivery_dt.Value.ToString("dd/MM/yyyy") : string.Empty,
                        temp_delivery_no = temp_no_delivery,
                        credit_day = credit_day != "" ? Convert.ToInt32(credit_day) : 0,
                        period_no = period_no != "" ? Convert.ToInt32(period_no) : 0,
                        period_amount = period_amount != "" ? Convert.ToDecimal(period_amount) : 0,
                        diff_deposit = diff_deposit != "" ? Convert.ToDecimal(diff_deposit) : 0

                    });

                }
                else
                {
                    if (Convert.ToInt32(id) == 0) // Insert mode when id == 0
                    {
                        dataPO.Add(new PODetail
                        {
                            id = (dataPO.Count + 1) * -1,
                            ref_po_no = po_no,

                            ref_po_date = po_dt != null ? po_dt.Value.ToString("dd/MM/yyyy") : "",
                            is_deleted = false,
                            display_po_date = po_dt != null ? po_dt.Value.ToString("dd/MM/yyyy") : "",
                            payment_type = payment_type != "" ? Convert.ToInt32(payment_type) : 0,
                            invoice_no = invoice_no,
                            invoice_date = invoice_dt != null ? invoice_dt.Value.ToString("dd/MM/yyyy") : "",
                            display_payment_type = displayPaymentType[Convert.ToInt32(payment_type) - 1],
                            display_invoice_date = invoice_dt != null ? invoice_dt.Value.ToString("dd/MM/yyyy") : "",
                            percent_price = payment_val == "" ? 100 : Convert.ToInt32(payment_val),
                            cheque_date = payment_dt != null ? payment_dt.Value : payment_dt,
                            display_cheque_date = payment_dt != null ? payment_dt.Value.ToString("dd/MM/yyyy") : "",
                            //bill_date = bill_dt,
                            //display_bill_date = bill_dt != null ? bill_dt.Value.ToString("dd/MM/yyyy") : string.Empty,
                            delivery_date = delivery_dt,
                            display_delivery_date = delivery_dt != null ? delivery_dt.Value.ToString("dd/MM/yyyy") : string.Empty,
                            temp_delivery_no = temp_no_delivery,
                            credit_day = credit_day != "" ? Convert.ToInt32(credit_day) : 0,
                            period_no = period_no != "" ? Convert.ToInt32(period_no) : 0,
                            period_amount = period_amount != "" ? Convert.ToDecimal(period_amount) : 0,
                            diff_deposit = diff_deposit != "" ? Convert.ToDecimal(diff_deposit) : 0
                        });
                    }
                    else
                    {
                        var row = (from t in dataPO where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                        row.ref_po_no = po_no;

                        row.ref_po_date = po_dt != null ? po_dt.Value.ToString("dd/MM/yyyy") : "";//dateTime,
                        row.is_deleted = false;
                        row.display_po_date = po_dt != null ? po_dt.Value.ToString("dd/MM/yyyy") : "";
                        row.payment_type = payment_type != "" ? Convert.ToInt32(payment_type) : 0;
                        row.invoice_no = invoice_no;
                        row.invoice_date = invoice_dt != null ? invoice_dt.Value.ToString("dd/MM/yyyy") : "";
                        row.display_payment_type = displayPaymentType[Convert.ToInt32(payment_type) - 1];
                        row.percent_price = payment_val == "" ? 100 : Convert.ToInt32(payment_val);
                        row.display_invoice_date = invoice_dt != null ? invoice_dt.Value.ToString("dd/MM/yyyy") : "";
                        row.cheque_date = payment_dt != null ? payment_dt.Value : DateTime.MinValue;
                        row.display_cheque_date = payment_dt != null ? payment_dt.Value.ToString("dd/MM/yyyy") : "";
                        //row.bill_date = bill_dt;
                        //row.display_bill_date = bill_dt != null ? bill_dt.Value.ToString("dd/MM/yyyy") : string.Empty;
                        row.delivery_date = delivery_dt;
                        row.display_delivery_date = delivery_dt != null ? delivery_dt.Value.ToString("dd/MM/yyyy") : string.Empty;
                        row.temp_delivery_no = temp_no_delivery;
                        row.credit_day = credit_day != "" ? Convert.ToInt32(credit_day) : 0;
                        row.period_no = period_no != "" ? Convert.ToInt32(period_no) : 0;
                        row.period_amount = period_amount != "" ? Convert.ToDecimal(period_amount) : 0;
                        row.diff_deposit = diff_deposit != "" ? Convert.ToDecimal(diff_deposit) : 0;
                    }

                }


                HttpContext.Current.Session["SESSION_PO_DETAIL_SALE_ORDER"] = dataPO;

                return (from t in dataPO where t.is_deleted == false select t).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static PODetail ShowEditPO(string id)
        {
            try
            {
                List<PODetail> dataPO = (List<PODetail>)HttpContext.Current.Session["SESSION_PO_DETAIL_SALE_ORDER"];
                PODetail dataEditPO = new PODetail();

                if (dataPO != null)
                {
                    var selectedData = (from t in dataPO where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (selectedData != null)
                    {
                        dataEditPO = selectedData;
                    }
                }

                HttpContext.Current.Session["SESSION_SELECTED_PO_DETAIL_SALE_ORDER"] = dataEditPO;

                return dataEditPO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static List<PODetail> DeletePODetail(string id)
        {
            try
            {
                List<PODetail> dataPO = (List<PODetail>)HttpContext.Current.Session["SESSION_PO_DETAIL_SALE_ORDER"];

                if (dataPO != null)
                {
                    var selectedData = (from t in dataPO where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (selectedData != null)
                    {
                        if (Convert.ToInt32(id) < 0)
                        {
                            dataPO.Remove(selectedData);
                        }
                        else
                        {
                            selectedData.is_deleted = true;
                        }
                    }
                }

                HttpContext.Current.Session["SESSION_PO_DETAIL_SALE_ORDER"] = dataPO;

                return dataPO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gridViewPO_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridViewPO.DataSource = (from t in poDetailList
                                     where t.is_deleted == false
                                     select t).ToList();
            gridViewPO.DataBind();
        }
        protected void gridViewPO_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxHyperLink btnEdit = gridViewPO.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "btnEdit") as ASPxHyperLink;
                if (btnEdit != null)
                {
                    if (e.DataColumn.FieldName == "id")
                    {
                        if (quotationData != null)
                        {
                            var row = (from t in poDetailList where t.id == Convert.ToInt32(e.KeyValue) select t).FirstOrDefault();
                            if (row != null)
                            {
                                if (row.id > 0)
                                {
                                    //btnEdit.Enabled = false;
                                }
                            }
                        }
                    }
                }
                ASPxHyperLink btnDelete = gridViewPO.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "btnDelete") as ASPxHyperLink;
                if (btnDelete != null)
                {
                    if (e.DataColumn.FieldName == "id")
                    {
                        if (quotationData != null)
                        {
                            var row = (from t in poDetailList where t.id == Convert.ToInt32(e.KeyValue) select t).FirstOrDefault();
                            if (row != null)
                            {
                                if (row.id > 0)
                                {
                                    btnDelete.Enabled = false;
                                }
                            }
                        }
                    }
                }
                Label lblDetail = gridViewPO.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "lblDetail") as Label;
                if (lblDetail != null)
                {
                    if (e.DataColumn.FieldName == "percent_price")
                    {
                        if (quotationData != null)
                        {
                            var row = (from t in poDetailList where t.id == Convert.ToInt32(e.KeyValue) select t).FirstOrDefault();
                            if (row != null)
                            {
                                string text = "";
                                List<string> displayPaymentType = new List<string>();
                                displayPaymentType.Add("เงินสด");
                                displayPaymentType.Add("รับเช็คล่วงหน้า ณ วันที่ส่งของ");
                                displayPaymentType.Add("เครดิต");
                                displayPaymentType.Add("มัดจำ ก่อนส่งให้ลูกค้า");
                                displayPaymentType.Add("ส่วนต่างมัดจำ นับจากวันที่ส่งของ");
                                displayPaymentType.Add("แบ่งชำระ");
                                if (row.payment_type == 1 || row.payment_type == 2)
                                {
                                    text = displayPaymentType[Convert.ToInt32(row.payment_type) - 1];
                                }
                                else if (row.payment_type == 3)
                                {
                                    text = String.Format("{0} {1} วัน", displayPaymentType[Convert.ToInt32(row.payment_type) - 1], row.credit_day);
                                }
                                else if (row.payment_type == 4)
                                {
                                    text = String.Format("มัดจำ {0}%, ส่วนต่าง {1}%", row.percent_price, row.diff_deposit);
                                }
                                else if (row.payment_type == 6)
                                {
                                    text = String.Format("จำนวนงวด {0}/{1}, งวดละ {2} บาท", e.VisibleIndex + 1, row.period_no, row.period_amount);
                                }
                                lblDetail.Text = text;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static string CheckDataPayment()
        {
            string status = "PENDING";
            var sum_percent = 0;
            List<PODetail> dataPO = (List<PODetail>)HttpContext.Current.Session["SESSION_PO_DETAIL_SALE_ORDER"];
            if (dataPO != null)
            {

                foreach (var row in dataPO)
                {
                    if (row.is_deleted == false)
                    {
                        if (row.payment_type == 1 || row.payment_type == 2 || row.payment_type == 3)
                        {
                            status = "FINISH";
                        }
                        else if (row.payment_type != 6)
                        {
                            sum_percent += row.percent_price;
                        }
                    }

                }
                if (sum_percent == 100)
                {
                    status = "FINISH";
                }
            }
            return status;
        }

        #endregion PO Detail

        #region Tax Detail
        protected void gridViewTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridViewTax.DataSource = (from t in taxDetailList
                                      where t.is_deleted == false
                                      select t).ToList();
            gridViewTax.DataBind();
        }
        #endregion Tax Detail

        #region Order History
        protected void gridViewOrderHistory_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

        }
        #endregion

        #region Notification
        protected void gridViewNotice_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                gridViewNotice.DataSource = (from t in notificationList where t.is_deleted == false && t.topic.Trim() == "SO" select t).ToList();
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
                List<Notification> dataNotification = (List<Notification>)HttpContext.Current.Session["SESSION_NOTIFICATION_SALE_ORDER"];

                //
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
                        topic = "SO",
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
                            topic = "SO",
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
                        row.topic = "SO";
                        row.notice_date = dateee;//dateTime,
                        row.is_deleted = false;
                        row.display_notice_date = dateee.ToString("dd/MM/yyyy HH:mm");
                    }

                }


                HttpContext.Current.Session["SESSION_NOTIFICATION_SALE_ORDER"] = dataNotification;

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
                List<Notification> dataNotification = (List<Notification>)HttpContext.Current.Session["SESSION_NOTIFICATION_SALE_ORDER"];
                Notification dataEditNotification = new Notification();

                if (dataNotification != null)
                {
                    var selectedData = (from t in dataNotification where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (selectedData != null)
                    {
                        dataEditNotification = selectedData;
                    }
                }

                HttpContext.Current.Session["SESSION_SELECTED_NOTIFICATION_SALE_ORDER"] = dataEditNotification;

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
                List<Notification> dataNotification = (List<Notification>)HttpContext.Current.Session["SESSION_NOTIFICATION_SALE_ORDER"];

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

                HttpContext.Current.Session["SESSION_NOTIFICATION_SALE_ORDER"] = dataNotification;

                return dataNotification;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion

        #region Validate Data
        [WebMethod]
        public static string ValidateData(string type, string status, string issue_no)
        {
            string msg = string.Empty;
            var saleOrderDetail = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL"];
            if (saleOrderDetail != null)
            {
                if (issue_no == "")
                {
                    if (type == "P")
                    {
                        foreach (var row in saleOrderDetail)
                        {
                            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                            {
                                //Create array of Parameters
                                List<SqlParameter> arrParm = new List<SqlParameter>
                            {
                                new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                                new SqlParameter("@product_no", SqlDbType.VarChar,50) { Value = "" },
                                new SqlParameter("@id", SqlDbType.Int) { Value = row.product_id }
                            };
                                conn.Open();
                                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_list", arrParm.ToArray());
                                conn.Close();

                                List<SqlParameter> arrParmQty = new List<SqlParameter>
                            {
                                new SqlParameter("@id", SqlDbType.Int) { Value = row.id },
                                new SqlParameter("@qu_detail_id", SqlDbType.Int) { Value = row.quotation_detail_id }
                            };
                                conn.Open();
                                var dsQty = SqlHelper.ExecuteDataset(conn, "sp_sale_order_check_qty", arrParmQty.ToArray());
                                conn.Close();

                                if (dsResult != null)
                                {
                                    var productRow = (from t in dsResult.Tables[0].AsEnumerable() select t).FirstOrDefault();
                                    if (productRow != null)
                                    {
                                        var tempReserveQuantity = row.qty;// row.qty + Convert.ToInt32(productRow["quantity_reserve"]);
                                        if (dsQty.Tables[0].Rows.Count != 0) // edit mode
                                        {
                                            var old_qty = (from t in dsQty.Tables[0].AsEnumerable() select t).FirstOrDefault();
                                            // หาค่าต่าง แล้วเอาไปบวกกับ reserve เก่า
                                            var tempDiffQty = row.qty;// (row.qty - Convert.ToInt32(old_qty["qty_so"])) + Convert.ToInt32(productRow["quantity_reserve"]);
                                            if (tempDiffQty > Convert.ToInt32(productRow["quantity_balance"]))
                                            {
                                                msg += "" + row.product_no + " จำนวนไม่เพียงพอ \n";
                                            }
                                        }
                                        else // new mode
                                        {
                                            if (tempReserveQuantity > Convert.ToInt32(productRow["quantity_balance"]))
                                            {
                                                msg += "" + row.product_no + " จำนวนไม่เพียงพอ \n";
                                            }
                                        }

                                    }
                                }
                            }

                        }
                    }
                    else if (type == "S")
                    {
                        foreach (var row in saleOrderDetail)
                        {
                            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                            {
                                //Create array of Parameters
                                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = row.product_id },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = "" },

                        };
                                conn.Open();
                                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_list", arrParm.ToArray());
                                conn.Close();

                                List<SqlParameter> arrParmQty = new List<SqlParameter>
                        {

                            new SqlParameter("@id", SqlDbType.Int) { Value = row.id },
                            new SqlParameter("@qu_detail_id", SqlDbType.Int) { Value = row.quotation_detail_id },

                        };
                                conn.Open();
                                var dsQty = SqlHelper.ExecuteDataset(conn, "sp_sale_order_check_qty", arrParmQty.ToArray());
                                conn.Close();


                                if (dsResult != null)
                                {
                                    var productRow = (from t in dsResult.Tables[0].AsEnumerable() select t).FirstOrDefault();
                                    if (productRow != null)
                                    {
                                        var tempReserveQuantity = row.qty;//row.qty + Convert.ToInt32(productRow["quantity_reserve"]);
                                        if (dsQty.Tables[0].Rows.Count > 0) // edit mode
                                        {

                                            var old_qty = (from t in dsQty.Tables[0].AsEnumerable() select t).FirstOrDefault();
                                            // หาค่าต่าง แล้วเอาไปบวกกับ reserve เก่า
                                            var tempDiffQty = row.qty;//(row.qty - Convert.ToInt32(old_qty["qty_so"])) + +Convert.ToInt32(productRow["quantity_reserve"]);
                                            if (tempDiffQty > Convert.ToInt32(productRow["quantity_balance"]))
                                            {
                                                msg += "" + row.product_no + " จำนวนไม่เพียงพอ \n";
                                            }
                                        }
                                        else // new mode
                                        {
                                            if (tempReserveQuantity > Convert.ToInt32(productRow["quantity_balance"]))
                                            {
                                                msg += "" + row.product_no + " จำนวนไม่เพียงพอ \n";
                                            }
                                        }

                                    }
                                }
                            }

                        }
                    }

                    if (saleOrderDetail.Count == 0)
                    {
                        msg += "กรุณาเลือกรายการทำ Sale Order อย่างน้อย 1 รายการ \n";
                    }
                }
            }
            else
            {
                msg += "กรุณาเลือกรายการทำ Sale Order อย่างน้อย 1 รายการ \n";
            }

            //  Check paymanet
            List<PODetail> dataPO = (List<PODetail>)HttpContext.Current.Session["SESSION_PO_DETAIL_SALE_ORDER"];
            if (dataPO.Count == 0)
            {
                msg += "กรุณาเลือกเงือนไขการชำระเงิน\n";
            }
            else {//if (status == "FL")
                foreach (var item in dataPO)
                {
                    if (item.ref_po_no == "" || item.ref_po_date == "")
                    {
                        msg += "กรุณากรอกรายละเอียด PO\n";
                        break;
                    }

                    if (status == "CF")
                    {
                        if (item.invoice_no == "" || item.invoice_date == "")
                        {
                            msg += "กรุณากรอกรายละเอียด Invoice\n";
                            break;
                        }
                    }
                }
            }

            return msg;
        }
        #endregion
        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            SaveData("FL");
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            SaveData("CF");
        }

        private void SaveData(string status)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                int newID = DataListUtil.emptyEntryID;
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        #region Prepare SaveDATA

                        var saleOrderNo = string.Empty;
                        var total = 0.0m;
                        var totalWithDiscount = 0.0m;
                        var grandTotal = 0.0m;
                        var total_amount = 0.0m;

                        var discountBottom1 = (string.IsNullOrEmpty(txtDiscountBottomBill1.Value) ? 0 : Convert.ToDecimal(txtDiscountBottomBill1.Value));
                        var discountBottom2 = (string.IsNullOrEmpty(txtDiscountBottomBill2.Value) ? 0 : Convert.ToDecimal(txtDiscountBottomBill2.Value));
                        var totalVat = 0.0m;
                        var is_payment = false;
                        var payment_date = DateTime.Now;
                        var sum_payment_percent = 0;
                        // #region Re-Calcurate
                        //foreach (var row in saleOrderDetailList)
                        //{
                        //    total += row.unit_price * row.qty;
                        //    totalWithDiscount += row.total_amount;

                        //}

                        //if (cbDiscountBottomBill1.Checked && cbbDiscountBottomBill1.Value == "P")
                        //{
                        //    sumDiscount1 = Math.Round(((totalWithDiscount * discountBottom1) / 100), 2);
                        //    grandTotal = Math.Round((totalWithDiscount - Math.Round(((totalWithDiscount * discountBottom1) / 100), 2)), 2);
                        //}
                        //else if (cbDiscountBottomBill1.Checked && cbbDiscountBottomBill1.Value == "A")
                        //{
                        //    sumDiscount1 = Math.Round((discountBottom1), 2);
                        //    grandTotal = Math.Round((totalWithDiscount - discountBottom1), 2);
                        //}
                        //else
                        //{
                        //    grandTotal = Math.Round((totalWithDiscount), 2);
                        //}
                        //if (cbDiscountBottomBill2.Checked && cbbDiscountBottomBill2.Value == "P")
                        //{
                        //    sumDiscount2 = Math.Round((((totalWithDiscount - sumDiscount1) * discountBottom2) / 100), 2);
                        //    grandTotal = Math.Round((grandTotal - Math.Round(Math.Round(((totalWithDiscount - sumDiscount2) * discountBottom2), 2) / 100, 2)), 2);
                        //}
                        //else if (cbDiscountBottomBill2.Checked && cbbDiscountBottomBill2.Value == "A")
                        //{
                        //    sumDiscount2 = Math.Round((discountBottom2), 2);
                        //    grandTotal = Math.Round((grandTotal - discountBottom2), 2);
                        //}
                        //else
                        //{
                        //    //grandTotal = totalWithDiscount;  
                        //}

                        var quotationSaleOrder = (QuotationData)HttpContext.Current.Session["SESSION_QUOTATION_DATA_SALE_ORDER"];
                        if (quotationSaleOrder != null)
                        {
                            if (cbShowVat.Checked)
                            {
                                grandTotal = Math.Round((quotationSaleOrder.grand_total), 2);
                                totalVat = Math.Round((quotationSaleOrder.vat_total), 2);
                                total_amount = Math.Round((quotationSaleOrder.total_amount), 2);
                            }
                            else
                            {
                                total_amount = Math.Round((quotationSaleOrder.total_amount), 2);
                                grandTotal = Math.Round((quotationSaleOrder.grand_total), 2);
                                totalVat = 0;
                            }
                        }
                        #endregion

                        #region PaymentStatus
                        foreach (var row in poDetailList)
                        {
                            if (row.payment_type == 1 || row.payment_type == 2 || row.payment_type == 3)
                            {
                                is_payment = true;
                                payment_date = DateTime.Now;
                            }
                            else
                            {
                                sum_payment_percent += row.percent_price;
                            }
                        }
                        if (sum_payment_percent == 100)
                        {
                            is_payment = true;
                        }

                        #endregion

                        if (dataId == 0) // Insert Mode
                        {
                            #region Sale Order Header
                            //Quotation Header
                            using (SqlCommand cmd = new SqlCommand("sp_sale_order_header_add", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@sale_order_status", SqlDbType.VarChar, 2).Value = status;//FL = Follow , CF = Confirm

                                cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = cbbQuotation.Text;
                                cmd.Parameters.Add("@quotation_type", SqlDbType.VarChar, 2).Value = hdQuotationType.Value;
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = hdCustomerId.Value;
                                cmd.Parameters.Add("@customer_name", SqlDbType.VarChar, 200).Value = txtCustomerName.Value;
                                cmd.Parameters.Add("@attention_name", SqlDbType.VarChar, 200).Value = txtAttentionName.Value;
                                cmd.Parameters.Add("@quotation_subject", SqlDbType.VarChar, 200).Value = txtSubject.Value;
                                cmd.Parameters.Add("@project_name", SqlDbType.VarChar, 200).Value = txtProject.Value;

                                cmd.Parameters.Add("@is_discount_by_item", SqlDbType.Bit).Value = cbDiscountByItem.Checked;
                                cmd.Parameters.Add("@discount1_type", SqlDbType.VarChar, 1).Value = cbDiscountBottomBill1.Checked ? cbbDiscountBottomBill1.Value : string.Empty;
                                cmd.Parameters.Add("@discount1_percentage", SqlDbType.Decimal).Value = string.IsNullOrEmpty(txtDiscountBottomBill1.Value) ? 0
                                                                                                       : (cbbDiscountBottomBill1.Value == "P" ? Convert.ToDecimal(txtDiscountBottomBill1.Value) : 0);
                                cmd.Parameters.Add("@discount1_amount", SqlDbType.Decimal).Value = string.IsNullOrEmpty(txtDiscountBottomBill1.Value) ? 0
                                                                                                       : (cbbDiscountBottomBill1.Value == "A" ? Convert.ToDecimal(txtDiscountBottomBill1.Value) : 0);
                                cmd.Parameters.Add("@discount1_total", SqlDbType.Decimal).Value = quotationSaleOrder.discount1_total;
                                cmd.Parameters.Add("@discount2_type", SqlDbType.VarChar, 1).Value = cbDiscountBottomBill2.Checked ? cbbDiscountBottomBill2.Value : string.Empty;
                                cmd.Parameters.Add("@discount2_percentage", SqlDbType.Decimal).Value = string.IsNullOrEmpty(txtDiscountBottomBill2.Value) ? 0
                                                                                                       : (cbbDiscountBottomBill2.Value == "P" ? Convert.ToDecimal(txtDiscountBottomBill2.Value) : 0);
                                cmd.Parameters.Add("@discount2_amount", SqlDbType.Decimal).Value = string.IsNullOrEmpty(txtDiscountBottomBill1.Value) ? 0
                                                                                                       : (cbbDiscountBottomBill2.Value == "A" ? Convert.ToDecimal(txtDiscountBottomBill2.Value) : 0);
                                cmd.Parameters.Add("@discount2_total", SqlDbType.Decimal).Value = quotationSaleOrder.discount2_total;
                                cmd.Parameters.Add("@total_amount", SqlDbType.Decimal).Value = total_amount;
                                cmd.Parameters.Add("@grand_total", SqlDbType.Decimal).Value = grandTotal;
                                cmd.Parameters.Add("@is_vat", SqlDbType.Bit).Value = cbShowVat.Checked;
                                cmd.Parameters.Add("@vat_total", SqlDbType.Decimal).Value = totalVat;

                                cmd.Parameters.Add("@remark_id", SqlDbType.Int).Value = rdoRemarkBillDelivery.Checked ? 1 : (rdoRemarkSubmitted.Checked ? 2 : (rdoRemarkOthers.Checked ? 3 : 0));
                                cmd.Parameters.Add("@remark_other", SqlDbType.VarChar, 100).Value = rdoRemarkOthers.Checked ? txtRemarkOther.Value : string.Empty;
                                cmd.Parameters.Add("@is_inv_normal", SqlDbType.Bit).Value = cbTypeSendTaxNormal.Checked ? true : false;
                                cmd.Parameters.Add("@is_inv_customer_receive", SqlDbType.Bit).Value = cbTypeSendTaxCustomer.Checked ? true : false;

                                cmd.Parameters.Add("@is_inv_thaipost", SqlDbType.Bit).Value = cbTypeSendTaxPost.Checked ? true : false;
                                cmd.Parameters.Add("@thaipost_attn", SqlDbType.VarChar, 200).Value = cbTypeSendTaxPost.Checked ? txtTypeSendAttention.Value : string.Empty;

                                cmd.Parameters.Add("@is_inv_other", SqlDbType.Bit).Value = cbTypeSendTaxOther.Checked ? true : false;
                                cmd.Parameters.Add("@other_description", SqlDbType.VarChar, 200).Value = cbTypeSendTaxOther.Checked ? txtTypeSendOther.Value : string.Empty;

                                cmd.Parameters.Add("@is_product_description", SqlDbType.Bit).Value = rdoDescription.Checked ? false : true;
                                cmd.Parameters.Add("@is_payment", SqlDbType.Bit).Value = is_payment;// rdoDescription.Checked ? false : true;
                                cmd.Parameters.Add("@payment_date", SqlDbType.DateTime).Value = DBNull.Value;// rdoDescription.Checked ? false : true;

                                cmd.Parameters.Add("@is_ref_issue_no", SqlDbType.Bit).Value = cbRefIssueNo.Checked ? true : false;
                                cmd.Parameters.Add("@ref_issue_stock_no", SqlDbType.VarChar, 20).Value = cboRefIssueNo.Value;

                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = txtRemark.Value;
                                newID = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            #endregion Quatation Header

                            //Create array of Parameters

                            List<SqlParameter> arrParm = new List<SqlParameter>
                            {
                                new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID  },
                                new SqlParameter("@id", SqlDbType.Int) { Value = newID },
                                new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = "" }
                            };

                            var lastDataInsert = SqlHelper.ExecuteDataset(tran, "sp_sale_order_header_list", arrParm.ToArray());


                            if (lastDataInsert != null)
                            {
                                saleOrderNo = (from t in lastDataInsert.Tables[0].AsEnumerable()
                                               select t.Field<string>("sale_order_no")).FirstOrDefault();
                            }

                            #region SaleOrder Detail
                            // Quotation Detail
                            var newDetailId = 0;
                            var oldDetailId = 0;
                            foreach (var row in saleOrderDetailList)
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
                                using (SqlCommand cmd = new SqlCommand("sp_sale_order_detail_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@sale_order_no", SqlDbType.VarChar, 20).Value = saleOrderNo;
                                    cmd.Parameters.Add("@quotation_detail_id", SqlDbType.Int).Value = row.quotation_detail_id;
                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                    cmd.Parameters.Add("@product_no", SqlDbType.VarChar, 50).Value = row.product_no;
                                    cmd.Parameters.Add("@saleorder_description", SqlDbType.VarChar, 200).Value = row.saleorder_description;
                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                    cmd.Parameters.Add("@qu_qty", SqlDbType.Int).Value = row.qu_qty;
                                    cmd.Parameters.Add("@unit_price", SqlDbType.Decimal).Value = row.unit_price;
                                    cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 200).Value = row.unit_code;
                                    cmd.Parameters.Add("@discount_type", SqlDbType.VarChar, 1).Value = cbbDiscountByItem.Value;
                                    cmd.Parameters.Add("@discount_percentage", SqlDbType.Decimal).Value = row.discount_percentage;
                                    cmd.Parameters.Add("@discount_amount", SqlDbType.Decimal).Value = row.discount_amount;
                                    cmd.Parameters.Add("@discount_total", SqlDbType.Decimal).Value = row.discount_total;
                                    cmd.Parameters.Add("@total_amount", SqlDbType.Decimal).Value = row.total_amount;
                                    cmd.Parameters.Add("@parent_id", SqlDbType.Int).Value = 0;
                                    cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = string.Empty;//row.remark;
                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 2).Value = row.product_type;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                    newDetailId = Convert.ToInt32(cmd.ExecuteScalar());

                                }

                            }
                            #endregion SaleOrder Detail

                            #region SaleOrder Remark


                            //using (SqlCommand cmd = new SqlCommand("sp_quotation_remark_add", conn))
                            //{
                            //    cmd.CommandType = CommandType.StoredProcedure;

                            //    cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = quotationNo;
                            //    cmd.Parameters.Add("@quotation_description", SqlDbType.VarChar, 200).Value = listRemark[i];
                            //    cmd.Parameters.Add("@sortno", SqlDbType.VarChar, 50).Value = i + 1;
                            //    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                            //    conn.Open();
                            //    cmd.ExecuteNonQuery();
                            //    conn.Close();
                            //}
                            //i++;

                            #endregion SaleOrder Remark

                            #region SaleOrder Notification
                            // Notification
                            foreach (var row in (from t in notificationList
                                                 where t.is_deleted == false
                                                 select t).ToList())
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_notification_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@subject", SqlDbType.VarChar, 100).Value = row.subject;
                                    cmd.Parameters.Add("@description", SqlDbType.VarChar, 200).Value = row.description;
                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = hdCustomerId.Value;
                                    cmd.Parameters.Add("@topic", SqlDbType.Char, 3).Value = "SO";
                                    cmd.Parameters.Add("@reference_id", SqlDbType.Int).Value = newID;
                                    cmd.Parameters.Add("@reference_no", SqlDbType.VarChar, 100).Value = saleOrderNo;
                                    cmd.Parameters.Add("@notice_type", SqlDbType.VarChar, 3).Value = row.notice_type;
                                    cmd.Parameters.Add("@notice_date", SqlDbType.DateTime).Value = row.notice_date;
                                    cmd.Parameters.Add("@is_read", SqlDbType.Bit).Value = 0;
                                    cmd.Parameters.Add("@is_enable", SqlDbType.Bit).Value = 1;
                                    cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = 0;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                    cmd.ExecuteNonQuery();

                                }
                            }
                            #endregion SaleOrder Notification

                            #region SaleOrder Payment

                            foreach (var row in poDetailList)
                            {
                                DateTime? cheque_date = null;
                                DateTime? delivery_date = null;
                                DateTime? bill_date = null;
                                DateTime? invoice_date = null;
                                if (row.cheque_date != null && row.cheque_date != DateTime.MinValue)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(row.cheque_date)))
                                    {
                                        cheque_date = row.cheque_date;
                                    }
                                }
                                if (row.delivery_date != null && row.delivery_date != DateTime.MinValue)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(row.delivery_date)))
                                    {
                                        delivery_date = row.delivery_date;
                                    }

                                }
                                //if (row.bill_date.Value != DateTime.MinValue)
                                //{
                                //    if (!string.IsNullOrEmpty(Convert.ToString(row.bill_date)))
                                //    {
                                //        bill_date = row.bill_date;
                                //    }

                                //}
                                DateTime? intvoiceDate = null;
                                if (!string.IsNullOrEmpty(row.invoice_date))
                                {
                                    intvoiceDate = DateTime.ParseExact(row.invoice_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }

                                DateTime? refPoDate = null;
                                if (!string.IsNullOrEmpty(row.ref_po_date))
                                {
                                    refPoDate = DateTime.ParseExact(row.ref_po_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }

                                if (row.invoice_date != null && intvoiceDate != DateTime.MinValue)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(row.invoice_date)))
                                    {
                                        invoice_date = intvoiceDate;
                                    }

                                }
                                //oldDetailId = row.id;
                                if (row.payment_type != 4 && row.payment_type != 5)
                                {
                                    row.percent_price = 100;
                                }
                                row.amount = (grandTotal * row.percent_price) / 100;
                                try
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_sale_order_payment_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@sale_order_no", SqlDbType.VarChar, 20).Value = Convert.ToString(saleOrderNo);
                                        cmd.Parameters.Add("@percent_price", SqlDbType.Int).Value = Convert.ToInt32(row.percent_price);
                                        cmd.Parameters.Add("@amount", SqlDbType.Decimal).Value = Convert.ToDecimal(row.amount);
                                        cmd.Parameters.Add("@payment_type", SqlDbType.Int).Value = Convert.ToInt32(row.payment_type);
                                        cmd.Parameters.Add("@cheque_date", SqlDbType.DateTime).Value = cheque_date;
                                        cmd.Parameters.Add("@ref_po_no", SqlDbType.VarChar, 20).Value = row.ref_po_no;
                                        cmd.Parameters.Add("@ref_po_date", SqlDbType.DateTime).Value = refPoDate;
                                        cmd.Parameters.Add("@inv_no", SqlDbType.VarChar, 200).Value = row.invoice_no;
                                        cmd.Parameters.Add("@inv_date", SqlDbType.DateTime).Value = invoice_date;
                                        //cmd.Parameters.Add("@bill_date", SqlDbType.DateTime).Value = bill_date;
                                        cmd.Parameters.Add("@delivery_date", SqlDbType.DateTime).Value = delivery_date;
                                        cmd.Parameters.Add("@temp_delivery_no", SqlDbType.VarChar, 20).Value = row.temp_delivery_no;
                                        cmd.Parameters.Add("@credit_day", SqlDbType.Int).Value = row.credit_day;
                                        cmd.Parameters.Add("@period_no", SqlDbType.Int).Value = Convert.ToInt32(row.period_no);
                                        cmd.Parameters.Add("@period_amount", SqlDbType.Decimal).Value = Convert.ToDecimal(row.period_amount);
                                        cmd.Parameters.Add("@diff_deposit", SqlDbType.Decimal).Value = Convert.ToDecimal(row.diff_deposit);
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }

                            }
                            #endregion

                            #region Quotation Update Status

                            /*using (SqlCommand cmd = new SqlCommand("sp_quotation_update_status", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = cbbQuotation.Text;

                                cmd.ExecuteNonQuery();
                            }*/

                            #endregion
                        }
                        else
                        {
                            newID = dataId;

                            #region Sale Order Header
                            //Quotation Header
                            using (SqlCommand cmd = new SqlCommand("sp_sale_order_edit", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = dataId;
                                
                                //  On payment, po exists => confirm, invoice exists => complete
                                if (status != "FL")
                                {
                                    foreach (var row in poDetailList)
                                    {
                                        if (row.invoice_no != "" && row.invoice_date != "")
                                        {
                                            status = "CP";
                                        }
                                    }
                                }
                                else if (status == "FL")
                                {
                                    if (hdSaleOrderStatus.Value == "CF")
                                    {
                                        status = "CF";
                                    }
                                }
                                cmd.Parameters.Add("@sale_order_status", SqlDbType.VarChar, 2).Value = status;//FL = Follow , CF = Confirm

                                //cmd.Parameters.Add("@open_bill_date", SqlDbType.Date).Value = DateTime.ParseExact(txtDateBill.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);//Convert.ToDateTime(txtDateBill.Value);
                                //cmd.Parameters.Add("@delivery_date", SqlDbType.Date).Value = DateTime.ParseExact(txtDateShipping.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);//txtDateShipping.Value;

                                cmd.Parameters.Add("@is_discount_by_item", SqlDbType.Bit).Value = cbDiscountByItem.Checked;
                                cmd.Parameters.Add("@discount1_type", SqlDbType.VarChar, 1).Value = cbDiscountBottomBill1.Checked ? cbbDiscountBottomBill1.Value : string.Empty;
                                cmd.Parameters.Add("@discount1_percentage", SqlDbType.Decimal).Value = string.IsNullOrEmpty(txtDiscountBottomBill1.Value) ? 0
                                                                                                       : (cbbDiscountBottomBill1.Value == "P" ? Convert.ToDecimal(txtDiscountBottomBill1.Value) : 0);
                                cmd.Parameters.Add("@discount1_amount", SqlDbType.Decimal).Value = string.IsNullOrEmpty(txtDiscountBottomBill1.Value) ? 0
                                                                                                       : (cbbDiscountBottomBill1.Value == "A" ? Convert.ToDecimal(txtDiscountBottomBill1.Value) : 0);
                                cmd.Parameters.Add("@discount1_total", SqlDbType.Decimal).Value = quotationSaleOrder.discount1_total;
                                cmd.Parameters.Add("@discount2_type", SqlDbType.VarChar, 1).Value = cbDiscountBottomBill2.Checked ? cbbDiscountBottomBill2.Value : string.Empty;
                                cmd.Parameters.Add("@discount2_percentage", SqlDbType.Decimal).Value = string.IsNullOrEmpty(txtDiscountBottomBill2.Value) ? 0
                                                                                                       : (cbbDiscountBottomBill2.Value == "P" ? Convert.ToDecimal(txtDiscountBottomBill2.Value) : 0);
                                cmd.Parameters.Add("@discount2_amount", SqlDbType.Decimal).Value = string.IsNullOrEmpty(txtDiscountBottomBill1.Value) ? 0
                                                                                                       : (cbbDiscountBottomBill2.Value == "A" ? Convert.ToDecimal(txtDiscountBottomBill2.Value) : 0);
                                cmd.Parameters.Add("@discount2_total", SqlDbType.Decimal).Value = quotationSaleOrder.discount2_total;
                                cmd.Parameters.Add("@total_amount", SqlDbType.Decimal).Value = total_amount;
                                cmd.Parameters.Add("@grand_total", SqlDbType.Decimal).Value = grandTotal;
                                cmd.Parameters.Add("@is_vat", SqlDbType.Bit).Value = cbShowVat.Checked;
                                cmd.Parameters.Add("@vat_total", SqlDbType.Decimal).Value = totalVat;
                                cmd.Parameters.Add("@remark_id", SqlDbType.Int).Value = rdoRemarkBillDelivery.Checked ? 1 : (rdoRemarkSubmitted.Checked ? 2 : (rdoRemarkOthers.Checked ? 3 : 0));
                                cmd.Parameters.Add("@remark_other", SqlDbType.VarChar, 100).Value = rdoRemarkOthers.Checked ? txtRemarkOther.Value : string.Empty;
                                cmd.Parameters.Add("@is_inv_normal", SqlDbType.Bit).Value = cbTypeSendTaxNormal.Checked ? true : false;
                                cmd.Parameters.Add("@is_inv_customer_receive", SqlDbType.Bit).Value = cbTypeSendTaxCustomer.Checked ? 1 : 0;
                                cmd.Parameters.Add("@is_inv_thaipost", SqlDbType.Bit).Value = cbTypeSendTaxPost.Checked ? true : false;
                                cmd.Parameters.Add("@thaipost_attn", SqlDbType.VarChar, 200).Value = cbTypeSendTaxPost.Checked ? txtTypeSendAttention.Value : string.Empty;
                                cmd.Parameters.Add("@is_inv_other", SqlDbType.Bit).Value = cbTypeSendTaxOther.Checked ? true : false;
                                cmd.Parameters.Add("@other_description", SqlDbType.VarChar, 200).Value = cbTypeSendTaxOther.Checked ? txtTypeSendOther.Value : string.Empty;

                                cmd.Parameters.Add("@is_payment", SqlDbType.Bit).Value = is_payment;// rdoDescription.Checked ? false : true;
                                cmd.Parameters.Add("@payment_date", SqlDbType.DateTime).Value = DBNull.Value;// rdoDescription.Checked ? false : true;
                                cmd.Parameters.Add("@is_ref_issue_no", SqlDbType.Bit).Value = cbRefIssueNo.Checked ? true : false;
                                cmd.Parameters.Add("@ref_issue_stock_no", SqlDbType.VarChar, 20).Value = cboRefIssueNo.Value;
                                cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = txtRemark.Value;

                                cmd.ExecuteNonQuery();

                            }
                            #endregion Quatation Header

                            #region SaleOrder Detail
                            // Quotation Detail
                            var oldDetailId = 0;
                            foreach (var row in saleOrderDetailList)
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
                                if (row.id > 0 && row.is_deleted == false) // EDIT DETAIL EDIT MODE
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_sale_order_detail_edit", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                        cmd.Parameters.Add("@sale_order_no", SqlDbType.VarChar, 20).Value = txtSaleOrder.Value;
                                        cmd.Parameters.Add("@quotation_detail_id", SqlDbType.Int).Value = row.quotation_detail_id;
                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                        cmd.Parameters.Add("@product_no", SqlDbType.VarChar, 50).Value = row.product_no;
                                        cmd.Parameters.Add("@saleorder_description", SqlDbType.VarChar, 200).Value = row.saleorder_description;
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                        cmd.Parameters.Add("@qu_qty", SqlDbType.Int).Value = row.qu_qty;
                                        cmd.Parameters.Add("@unit_price", SqlDbType.Decimal).Value = row.unit_price;
                                        cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 200).Value = row.unit_code;
                                        cmd.Parameters.Add("@discount_type", SqlDbType.VarChar, 1).Value = cbbDiscountByItem.Value;
                                        cmd.Parameters.Add("@discount_percentage", SqlDbType.Decimal).Value = row.discount_percentage;
                                        cmd.Parameters.Add("@discount_amount", SqlDbType.Decimal).Value = row.discount_amount;
                                        cmd.Parameters.Add("@discount_total", SqlDbType.Decimal).Value = row.discount_total;
                                        cmd.Parameters.Add("@total_amount", SqlDbType.Decimal).Value = row.total_amount;
                                        cmd.Parameters.Add("@parent_id", SqlDbType.Int).Value = 0;
                                        cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = string.Empty;//row.remark;
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                                else if (row.id < 0)  // ADD DETAIL EDIT MODE
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_sale_order_detail_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@sale_order_no", SqlDbType.VarChar, 20).Value = txtSaleOrder.Value;
                                        cmd.Parameters.Add("@quotation_detail_id", SqlDbType.Int).Value = row.quotation_detail_id;
                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                        cmd.Parameters.Add("@product_no", SqlDbType.VarChar, 50).Value = row.product_no;
                                        cmd.Parameters.Add("@saleorder_description", SqlDbType.VarChar, 200).Value = row.saleorder_description;
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                        cmd.Parameters.Add("@qu_qty", SqlDbType.Int).Value = row.qu_qty;
                                        cmd.Parameters.Add("@unit_price", SqlDbType.Decimal).Value = row.unit_price;
                                        cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 200).Value = row.unit_code;
                                        cmd.Parameters.Add("@discount_type", SqlDbType.VarChar, 1).Value = cbbDiscountByItem.Value;
                                        cmd.Parameters.Add("@discount_percentage", SqlDbType.Decimal).Value = row.discount_percentage;
                                        cmd.Parameters.Add("@discount_amount", SqlDbType.Decimal).Value = row.discount_amount;
                                        cmd.Parameters.Add("@discount_total", SqlDbType.Decimal).Value = row.discount_total;
                                        cmd.Parameters.Add("@total_amount", SqlDbType.Decimal).Value = row.total_amount;
                                        cmd.Parameters.Add("@parent_id", SqlDbType.Int).Value = 0;
                                        cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = string.Empty;//row.remark;
                                        cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 2).Value = row.product_type;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                                else if (row.id > 0 && row.is_deleted == true) // DELETE DETAIL EDIT MODE
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_sale_order_detail_delete", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                        cmd.Parameters.Add("@quotation_detail_id", SqlDbType.Int).Value = row.quotation_detail_id;
                                        cmd.Parameters.Add("@sale_order_no", SqlDbType.VarChar, 20).Value = txtSaleOrder.Value;
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                            }
                            #endregion SaleOrder Detail

                            #region Quotation Notification
                            // Notification
                            using (SqlCommand cmd = new SqlCommand("sp_sale_order_notification_delete_edit", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@sale_order_no", SqlDbType.VarChar, 20).Value = txtSaleOrder.Value;

                                cmd.ExecuteNonQuery();

                            }


                            foreach (var row in (from t in notificationList
                                                 where t.topic.Trim() == "SO" && t.is_deleted == false
                                                 select t).ToList())
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_notification_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@subject", SqlDbType.VarChar, 100).Value = row.subject;
                                    cmd.Parameters.Add("@description", SqlDbType.VarChar, 200).Value = row.description;
                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = hdCustomerId.Value;
                                    cmd.Parameters.Add("@topic", SqlDbType.NVarChar, 3).Value = row.topic;
                                    cmd.Parameters.Add("@reference_id", SqlDbType.Int).Value = dataId;
                                    cmd.Parameters.Add("@reference_no", SqlDbType.VarChar, 100).Value = txtSaleOrder.Value;
                                    cmd.Parameters.Add("@notice_type", SqlDbType.VarChar, 3).Value = row.notice_type;
                                    cmd.Parameters.Add("@notice_date", SqlDbType.DateTime).Value = row.notice_date;
                                    cmd.Parameters.Add("@is_read", SqlDbType.Bit).Value = 0;
                                    cmd.Parameters.Add("@is_enable", SqlDbType.Bit).Value = 1;
                                    cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = 0;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                    cmd.ExecuteNonQuery();

                                }
                            }
                            #endregion Quotation Notification

                            #region SaleOrder Payment

                            foreach (var row in poDetailList)
                            {
                                DateTime? cheque_date = null;
                                DateTime? delivery_date = null;
                                DateTime? bill_date = null;
                                DateTime? invoice_date = null;

                                if (row.cheque_date != DateTime.MinValue)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(row.cheque_date)))
                                    {
                                        cheque_date = row.cheque_date;
                                    }
                                }

                                if (row.delivery_date != DateTime.MinValue)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(row.delivery_date)))
                                    {
                                        delivery_date = row.delivery_date;
                                    }

                                }
                                //if (row.bill_date.Value != DateTime.MinValue)
                                //{
                                //    if (!string.IsNullOrEmpty(Convert.ToString(row.bill_date)))
                                //    {
                                //        bill_date = row.bill_date;
                                //    }

                                //}

                                if (!string.IsNullOrEmpty(Convert.ToString(row.invoice_date)))
                                {
                                    if (DateTime.ParseExact(row.invoice_date, "dd/MM/yyyy", CultureInfo.InvariantCulture) != DateTime.MinValue)
                                    {
                                        invoice_date = DateTime.ParseExact(row.invoice_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    }

                                }

                                DateTime? refPoDate = null;
                                if (!string.IsNullOrEmpty(Convert.ToString(row.ref_po_date)))
                                {
                                    if (DateTime.ParseExact(row.ref_po_date, "dd/MM/yyyy", CultureInfo.InvariantCulture) != DateTime.MinValue)
                                    {
                                        refPoDate = DateTime.ParseExact(row.ref_po_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    }

                                }

                                //oldDetailId = row.id;
                                if (row.payment_type != 4 && row.payment_type != 5)
                                {
                                    row.percent_price = 100;
                                }

                                row.amount = (grandTotal * row.percent_price) / 100;
                                if (row.id < 0)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_sale_order_payment_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@sale_order_no", SqlDbType.VarChar, 20).Value = txtSaleOrder.Value;
                                        cmd.Parameters.Add("@percent_price", SqlDbType.Int).Value = row.percent_price;
                                        cmd.Parameters.Add("@amount", SqlDbType.Decimal).Value = row.amount;
                                        cmd.Parameters.Add("@payment_type", SqlDbType.Int).Value = row.payment_type;
                                        cmd.Parameters.Add("@cheque_date", SqlDbType.DateTime).Value = cheque_date;
                                        cmd.Parameters.Add("@ref_po_no", SqlDbType.VarChar, 20).Value = row.ref_po_no;
                                        cmd.Parameters.Add("@ref_po_date", SqlDbType.DateTime).Value = refPoDate;
                                        //cmd.Parameters.Add("@bill_date", SqlDbType.DateTime).Value = bill_date;
                                        cmd.Parameters.Add("@delivery_date", SqlDbType.DateTime).Value = delivery_date;
                                        cmd.Parameters.Add("@inv_no", SqlDbType.VarChar, 200).Value = row.invoice_no;
                                        cmd.Parameters.Add("@inv_date", SqlDbType.DateTime).Value = invoice_date;
                                        cmd.Parameters.Add("@temp_delivery_no", SqlDbType.VarChar, 20).Value = row.temp_delivery_no;
                                        cmd.Parameters.Add("@credit_day", SqlDbType.Int).Value = row.credit_day;
                                        cmd.Parameters.Add("@period_no", SqlDbType.Int).Value = Convert.ToInt32(row.period_no);
                                        cmd.Parameters.Add("@period_amount", SqlDbType.Decimal).Value = Convert.ToDecimal(row.period_amount);
                                        cmd.Parameters.Add("@diff_deposit", SqlDbType.Decimal).Value = Convert.ToDecimal(row.diff_deposit);
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                                else
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_sale_order_payment_edit", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@id", SqlDbType.VarChar, 20).Value = row.id;
                                        cmd.Parameters.Add("@sale_order_no", SqlDbType.VarChar, 20).Value = txtSaleOrder.Value;
                                        cmd.Parameters.Add("@percent_price", SqlDbType.Int).Value = row.percent_price;
                                        cmd.Parameters.Add("@amount", SqlDbType.Decimal).Value = row.amount;
                                        cmd.Parameters.Add("@payment_type", SqlDbType.Int).Value = row.payment_type;
                                        cmd.Parameters.Add("@cheque_date", SqlDbType.DateTime).Value = cheque_date;
                                        cmd.Parameters.Add("@ref_po_no", SqlDbType.VarChar, 20).Value = row.ref_po_no;
                                        cmd.Parameters.Add("@ref_po_date", SqlDbType.DateTime).Value = refPoDate;
                                        //cmd.Parameters.Add("@bill_date", SqlDbType.DateTime).Value = bill_date;
                                        cmd.Parameters.Add("@delivery_date", SqlDbType.DateTime).Value = delivery_date;
                                        cmd.Parameters.Add("@inv_no", SqlDbType.VarChar, 200).Value = row.invoice_no;
                                        cmd.Parameters.Add("@inv_date", SqlDbType.DateTime).Value = invoice_date;
                                        cmd.Parameters.Add("@period_no", SqlDbType.Int).Value = Convert.ToInt32(row.period_no);
                                        cmd.Parameters.Add("@period_amount", SqlDbType.Decimal).Value = Convert.ToDecimal(row.period_amount);
                                        cmd.Parameters.Add("@diff_deposit", SqlDbType.Decimal).Value = Convert.ToDecimal(row.diff_deposit);
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = row.is_deleted;
                                        cmd.Parameters.Add("@temp_delivery_no", SqlDbType.VarChar, 20).Value = row.temp_delivery_no;
                                        cmd.Parameters.Add("@credit_day", SqlDbType.Int).Value = row.credit_day;

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                            }
                            #endregion

                            #region Quotation Update Status
                            if (status == "CP")
                            {
                                /*using (SqlCommand cmd = new SqlCommand("sp_quotation_update_status", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = cbbQuotation.Text;

                                    cmd.ExecuteNonQuery();
                                }*/

                                using (SqlCommand cmd = new SqlCommand("sp_issue_update_status", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@ref_no", SqlDbType.VarChar, 20).Value = cboRefIssueNo.Value;
                                    cmd.Parameters.Add("@ref_type", SqlDbType.VarChar, 2).Value = "SO";

                                    cmd.ExecuteNonQuery();

                                }
                            }
                            #endregion

                        }

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        tran.Dispose();
                        conn.Close();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('เกิดผิดพลาดในการบันทึกข้อมูล','E')", true);
                    }
                    finally
                    {
                        if (!conn.State.Equals(ConnectionState.Closed))
                        {

                            tran.Commit();
                            tran.Dispose();
                            conn.Close();
                            Response.Redirect("SaleOrder.aspx?dataId=" + newID);
                        }
                    }
                }
            }
        }
        [WebMethod]
        public static string CheckCustomerMFG(string id)
        {
            var data = string.Empty;
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {

                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value =  Convert.ToInt32(id)},
                        };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_mfg_list", arrParm.FirstOrDefault());
                conn.Close();

                if (dsResult.Tables.Count > 0)
                {
                    var row = dsResult.Tables[0].AsEnumerable().FirstOrDefault();
                    if (row != null)
                    {
                        data = "success";
                    }

                }
            }
            return data;
        }
        [WebMethod]
        public static string CancelSaleOrderDetail(string saleOrderNo)
        {
            var data = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    if (saleOrderNo != null)
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_sale_order_edit_cancel", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@sale_order_no", SqlDbType.VarChar, 20).Value = saleOrderNo;
                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
