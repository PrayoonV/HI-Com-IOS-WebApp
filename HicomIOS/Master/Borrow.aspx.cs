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
    public partial class Borrow : MasterDetailPage
    {
        public int dataId = 0;
        public override string PageName { get { return "Create Borrow"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }
        public override void OnFilterChanged()
        {

        }

        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            return result.OfType<FilterControlColumn>();
        }
        #region Members

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
            public int supplier_id { get; set; }
            public string supplier_titel { get; set; }

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

        }
        public class ProductDetail
        {
            public bool is_selected { get; set; }
            public int product_id { get; set; }
            public string product_no { get; set; }
            public string product_name { get; set; }
            public string item_type { get; set; }
            public string item_type_text { get; set; }
            public string item_unit { get; set; }
            public int qty { get; set; }
            public int selling_price { get; set; }
        }
        public class BorrowDetail
        {
            public int id { get; set; }
            public int sort_no { get; set; }
            public int item_id { get; set; }
            public string item_no { get; set; }
            public string item_name { get; set; }
            public string item_type { get; set; }
            public string item_type_text { get; set; }
            
            public string unit_code { get; set; }
            public int qty { get; set; }
            public int old_qty { get; set; }
            public decimal unit_price { get; set; }
            public decimal total_price { get; set; }
            public bool is_delete { get; set; }
        }
        public class MFGDetail
        {
            public bool is_selected { get; set; }
            public string mfg_no { get; set; }
        }
        public class BorrowDetailMFG
        {
            public int id { get; set; }
            public string borrow_no { get; set; }
            public int borrow_detail_id { get; set; }
            public int item_id { get; set; }
            public string mfg_no { get; set; }
            public string old_mfg_no { get; set; }
            public bool is_deleted { get; set; }

        }
        List<ProductDetail> productDetailList
        {
            get
            {
                if (Session["SESSION_PRODUCT_DETAIL_BORROW"] == null)
                    Session["SESSION_PRODUCT_DETAIL_BORROW"] = new List<ProductDetail>();
                return (List<ProductDetail>)Session["SESSION_PRODUCT_DETAIL_BORROW"];
            }
            set
            {
                Session["SESSION_PRODUCT_DETAIL_BORROW"] = value;
            }
        }
        List<QuotationDetail> quotationDetailList
        {
            get
            {
                if (Session["SESSION_QUOTATION_DETAIL_BORROW"] == null)
                    Session["SESSION_QUOTATION_DETAIL_BORROW"] = new List<QuotationDetail>();
                return (List<QuotationDetail>)Session["SESSION_QUOTATION_DETAIL_BORROW"];
            }
            set
            {
                Session["SESSION_QUOTATION_DETAIL_BORROW"] = value;
            }
        }

        List<BorrowDetail> borrowDetailList
        {
            get
            {
                if (Session["SESSION_BORROW_DETAIL_BORROW"] == null)
                    Session["SESSION_BORROW_DETAIL_BORROW"] = new List<BorrowDetail>();
                return (List<BorrowDetail>)Session["SESSION_BORROW_DETAIL_BORROW"];
            }
            set
            {
                Session["SESSION_BORROW_DETAIL_BORROW"] = value;
            }
        }
        List<BorrowDetail> selectedBorrowDetailList
        {
            get
            {
                if (Session["SESSION_SELECTED_BORROW_DETAIL_BORROW"] == null)
                    Session["SESSION_SELECTED_BORROW_DETAIL_BORROW"] = new List<BorrowDetail>();
                return (List<BorrowDetail>)Session["SESSION_SELECTED_BORROW_DETAIL_BORROW"];
            }
            set
            {
                Session["SESSION_SELECTED_BORROW_DETAIL_BORROW"] = value;
            }
        }
        List<MFGDetail> mfgDetailList
        {
            get
            {
                if (Session["SESSION_MFG_DETAIL_BORROW"] == null)
                    Session["SESSION_MFG_DETAIL_BORROW"] = new List<MFGDetail>();
                return (List<MFGDetail>)Session["SESSION_MFG_DETAIL_BORROW"];
            }
            set
            {
                Session["SESSION_MFG_DETAIL_BORROW"] = value;
            }
        }
        List<BorrowDetailMFG> borrowDetailMFGList
        {
            get
            {
                if (Session["SESSION_BORROW_DETAIL_MFG_BORROW"] == null)
                    Session["SESSION_BORROW_DETAIL_MFG_BORROW"] = new List<BorrowDetailMFG>();
                return (List<BorrowDetailMFG>)Session["SESSION_BORROW_DETAIL_MFG_BORROW"];
            }
            set
            {
                Session["SESSION_BORROW_DETAIL_MFG_BORROW"] = value;
            }
        }
        List<BorrowDetailMFG> selectedBorrowDetailMFGList
        {
            get
            {
                if (Session["SESSION_SELECTED_BORROW_DETAIL_MFG_BORROW"] == null)
                    Session["SESSION_SELECTED_BORROW_DETAIL_MFG_BORROW"] = new List<BorrowDetailMFG>();
                return (List<BorrowDetailMFG>)Session["SESSION_SELECTED_BORROW_DETAIL_MFG_BORROW"];
            }
            set
            {
                Session["SESSION_SELECTED_BORROW_DETAIL_MFG_BORROW"] = value;
            }
        }
        private DataSet dsBorrow = new DataSet();
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
                    BindGridViewBorrow();
                    BindGridViewProductDetail();
                    BindGridViewQuotationDetail();
                    BindGridMFGDetail();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ClearWorkingSession()
        {
            try
            {
                Session.Remove("SESSION_PRODUCT_DETAIL_BORROW");
                Session.Remove("SESSION_QUOTATION_DETAIL_BORROW");
                Session.Remove("SESSION_BORROW_DETAIL_BORROW");
                Session.Remove("SESSION_SELECTED_BORROW_DETAIL_BORROW");
                Session.Remove("SESSION_MFG_DETAIL_BORROW");
                Session.Remove("SESSION_SELECTED_BORROW_DETAIL_MFG_BORROW");
                Session.Remove("SESSION_BORROW_DETAIL_MFG_BORROW");

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
                    
                    SPlanetUtil.BindASPxComboBox(ref cbbQuotation, DataListUtil.DropdownStoreProcedureName.Quotation_Document); // ใช้อันเดียวกันได้
                }

                SPlanetUtil.BindASPxComboBox(ref cbbCustomer, DataListUtil.DropdownStoreProcedureName.Customer);
               
              
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                                {
                                    new SqlParameter("@quotation_no", SqlDbType.VarChar) { Value = "" },
                                    new SqlParameter("@item_type", SqlDbType.VarChar) { Value = "" },
                                };
                        conn.Open();
                        var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_supplier_qu", arrParm.ToArray());

                        conn.Close();
                        if (dsResult.Tables.Count > 0)
                        {
                            if (dsResult.Tables[0].Rows.Count > 0)
                            {
                                cbbSupplier.DataSource = dsResult;
                                cbbSupplier.TextField = "data_text";
                                cbbSupplier.ValueField = "data_value";
                                cbbSupplier.DataBind();
                            }
                        }
                    }
                
               

                txtBorrowDate.Value = DateTime.UtcNow.ToString("dd/MM/yyyy");
                txtPODate.Value = DateTime.UtcNow.ToString("dd/MM/yyyy");

                gridViewBorrow.SettingsBehavior.AllowFocusedRow = true;
                gridMFG.SettingsBehavior.AllowFocusedRow = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                        dsBorrow = SqlHelper.ExecuteDataset(conn, "sp_borrow_list_data", arrParm.ToArray());
                        ViewState["dsBorrow"] = dsBorrow;
                        conn.Close();
                    }
                    if (dsBorrow.Tables.Count > 0)
                    {
                        var header = (from t in dsBorrow.Tables[0].AsEnumerable() select t).FirstOrDefault();
                        if (header != null)
                        {
                            cbbCustomer.Value = Convert.IsDBNull(header["customer_id"]) ? string.Empty : Convert.ToString(header["customer_id"]);
                            cbbBorrowType.Value = Convert.IsDBNull(header["borrow_type"]) ? string.Empty : Convert.ToString(header["borrow_type"]);
                            if (cbbBorrowType.Value == "QU")
                            {
                                var quotationNo = Convert.IsDBNull(header["ref_doc_no"]) ? string.Empty : Convert.ToString(header["ref_doc_no"]);
                                var quotationId = Convert.IsDBNull(header["ref_doc_id"]) ? "0" : Convert.ToString(header["ref_doc_id"]);
                                cbbQuotation.Items.Insert(0, new ListEditItem { Text = quotationNo, Value = quotationId, Selected = true });
                            }
                            cbbQuotation.Value = Convert.IsDBNull(header["ref_doc_id"]) ? string.Empty : Convert.ToString(header["ref_doc_id"]);
                            cbbObjective.Value = Convert.IsDBNull(header["objective_type"]) ? string.Empty : Convert.ToString(header["objective_type"]);
                            txtPO.Value = Convert.IsDBNull(header["purchase_order_no"]) ? string.Empty : Convert.ToString(header["purchase_order_no"]);
                            txtPODate.Value = Convert.IsDBNull(header["purchase_order_date"]) ? string.Empty : Convert.ToString(Convert.ToDateTime(header["purchase_order_date"]).ToString("dd/MM/yyyy"));
                            lbCustomerFirstName.Value = Convert.IsDBNull(header["customer_name"]) ? string.Empty : Convert.ToString(header["customer_name"]);
                            lbCustomerNo.Value = Convert.IsDBNull(header["customer_code"]) ? string.Empty : Convert.ToString(header["customer_code"]);
                            lbQuotationDate.Value = Convert.IsDBNull(header["ref_doc_date"]) ? string.Empty : Convert.ToString(Convert.ToDateTime(header["ref_doc_date"]).ToString("dd/MM/yyyy"));
                            hdCustomerCode.Value = Convert.IsDBNull(header["customer_code"]) ? string.Empty : Convert.ToString(header["customer_code"]);
                            hdCustomerId.Value = Convert.IsDBNull(header["customer_id"]) ? string.Empty : Convert.ToString(header["customer_id"]);
                            //txtSupplierId.Value = Convert.IsDBNull(header["supplier_id"]) ? string.Empty : Convert.ToString(header["supplier_id"]);
                            //txtSupplier.Value = Convert.IsDBNull(header["supplier_name_tha"]) ? string.Empty : Convert.ToString(header["supplier_name_tha"]);

                            var supplier_name = Convert.IsDBNull(header["supplier_name_tha"]) ? string.Empty : Convert.ToString(header["supplier_name_tha"]);
                            var supplier_id = Convert.IsDBNull(header["supplier_id"]) ? string.Empty : Convert.ToString(header["supplier_id"]);
                            cbbSupplier.Items.Insert(0, new ListEditItem { Text = supplier_name, Value = supplier_id, Selected = true });
                            cbbSupplier.Value = Convert.IsDBNull(header["supplier_id"]) ? string.Empty : Convert.ToString(header["supplier_id"]);

                            txtBorrowDate.Value = Convert.IsDBNull(header["borrow_date"]) ? string.Empty : Convert.ToString(Convert.ToDateTime(header["borrow_date"]).ToString("dd/MM/yyyy"));
                            txtBorrowNo.Value = Convert.IsDBNull(header["borrow_no"]) ? string.Empty : Convert.ToString(header["borrow_no"]);
                            string productType = Convert.IsDBNull(header["product_type"]) ? string.Empty : Convert.ToString(header["product_type"]);

                            txtRemark.Value = Convert.IsDBNull(header["remark"]) ? string.Empty : Convert.ToString(header["remark"]);

                            if (cbbBorrowType.Value == "P")
                            {
                                dvProductCust.Attributes["style"] = "display:''";
                                dvQuotation.Attributes["style"] = "display:none";
                                dvQuotation2.Attributes["style"] = "display:none";
                                cbbProductType.Attributes["style"] = "display:''";

                                cbbProductType.Value = productType;
                            }
                            else
                            {
                                dvProductCust.Attributes["style"] = "display:none";
                                dvQuotation.Attributes["style"] = "display:''";
                                dvQuotation2.Attributes["style"] = "display:''";
                            }

                            //  Check column
                            if (productType == "P")
                            {
                                gridViewBorrow.Columns[1].Visible = true;
                            }
                            else
                            {
                                gridViewBorrow.Columns[1].Visible = false;
                            }

                            var status = Convert.IsDBNull(header["borrow_status"]) ? string.Empty : Convert.ToString(header["borrow_status"]);
                            if (status == "CF" || status == "RE")
                            {
                                btnDraft.Visible = false;
                                btnSave.Visible = false;

                                gridViewBorrow.Columns[0].Visible = false;
                                gridViewBorrow.Columns[1].Visible = false;
                            }
                            else
                            {
                                btnNew.Visible = false;
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

                        var detail = (from t in dsBorrow.Tables[1].AsEnumerable() select t).ToList();
                        if (detail != null)
                        {
                            borrowDetailList = new List<BorrowDetail>();
                            foreach (var row in detail)
                            {
                                borrowDetailList.Add(new BorrowDetail()
                                {
                                    id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                    is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                    item_name = Convert.IsDBNull(row["item_name"]) ? string.Empty : Convert.ToString(row["item_name"]),
                                    item_id = Convert.IsDBNull(row["item_id"]) ? 0 : Convert.ToInt32(row["item_id"]),
                                    item_no = Convert.IsDBNull(row["item_no"]) ? string.Empty : Convert.ToString(row["item_no"]),
                                    item_type = Convert.IsDBNull(row["item_type"]) ? string.Empty : Convert.ToString(row["item_type"]),
                                    item_type_text = Convert.IsDBNull(row["item_type_text"]) ? string.Empty : Convert.ToString(row["item_type_text"]),
                                    unit_code = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]),
                                    qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["qty"]),
                                    old_qty = Convert.IsDBNull(row["old_qty"]) ? 0 : Convert.ToInt32(row["old_qty"]),
                                    sort_no = Convert.IsDBNull(row["sort_no"]) ? 0 : Convert.ToInt32(row["sort_no"]),
                                    total_price = Convert.IsDBNull(row["total_price"]) ? 0 : Convert.ToDecimal(row["total_price"]),
                                    unit_price = Convert.IsDBNull(row["unit_price"]) ? 0 : Convert.ToDecimal(row["unit_price"]),
                                });
                            }
                        }
                        var mfgDetail = (from t in dsBorrow.Tables[2].AsEnumerable() select t).ToList();
                        if (mfgDetail != null)
                        {
                            borrowDetailMFGList = new List<BorrowDetailMFG>();
                            foreach (var row in mfgDetail)
                            {
                                borrowDetailMFGList.Add(new BorrowDetailMFG()
                                {
                                    id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                    is_deleted = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                    mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                    old_mfg_no = Convert.IsDBNull(row["old_mfg_no"]) ? string.Empty : Convert.ToString(row["old_mfg_no"]),
                                    borrow_detail_id = Convert.IsDBNull(row["borrow_detail_id"]) ? 0 : Convert.ToInt32(row["borrow_detail_id"]),
                                    borrow_no = Convert.IsDBNull(row["borrow_no"]) ? string.Empty : Convert.ToString(row["borrow_no"]),
                                    item_id = Convert.IsDBNull(row["item_id"]) ? 0 : Convert.ToInt32(row["item_id"]),

                                });
                            }
                        }
                    }

                    cbbBorrowType.Attributes["disabled"] = "disabled";

                    gridViewBorrow.DataSource = borrowDetailList;
                    gridViewBorrow.DataBind();
                }
                else
                {
                    dvProductCust.Attributes["style"] = "display:none";
                    dvQuotation.Attributes["style"] = "display:''";
                    dvQuotation2.Attributes["style"] = "display:''";
                    btnReportClient.Visible = false;
                    btnSave.Visible = false;
                    chkAllQuotationDetail.Checked = false;
                    chkAllProductDetail.Checked = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static string ValidateData(string borrow_type, string product_type)
        {
            string msg = string.Empty;
            var borrowMFGDetail = (List<BorrowDetailMFG>)HttpContext.Current.Session["SESSION_BORROW_DETAIL_MFG_BORROW"];
            var borrowDetail = (List<BorrowDetail>)HttpContext.Current.Session["SESSION_BORROW_DETAIL_BORROW"];

            if (borrowDetail == null || borrowDetail.Count == 0)
            {

                msg += "กรุณาเลือกรายการยืม อย่างน้อย 1 รายการ \n";

            }
            else
            {
                if (product_type == "P")
                {
                    foreach (var row in borrowDetail)
                    {
                        if (borrowMFGDetail != null)
                        {
                            if (borrowMFGDetail.Count > 0)
                            {
                                var mfgSelectedData = (from t in borrowMFGDetail where t.borrow_detail_id == row.id && t.is_deleted == false select t).ToList();
                                if (mfgSelectedData != null)
                                {
                                    if (row.qty != mfgSelectedData.Count)
                                    {
                                        msg += "สินค้า " + row.item_no + " จำนวน MFG (" + mfgSelectedData.Count + ") ไม่ตรงกับจำนวนที่รับทั้งหมด (" + row.qty + ")\n";
                                    }
                                }
                                else
                                {
                                    msg += "สินค้า " + row.item_no + " จำนวน MFG (" + mfgSelectedData.Count + ") ไม่ตรงกับจำนวนที่รับทั้งหมด (" + row.qty + ")\n";
                                }
                            }
                            else
                            {
                                msg += "กรุณาเลือก MFG ของแต่ละสินค้า";
                            }
                        }
                        else
                        {
                            msg += "กรุณาเลือก MFG ของแต่ละสินค้า";
                        }
                    }
                }
              
            }
            return msg;
        }

        private void SaveData(string status)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                int newID = DataListUtil.emptyEntryID;
                var borrowNo = string.Empty;
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {

                        if (dataId == 0)
                        {

                            #region Borrow Header
                            //Quotation Header
                            using (SqlCommand cmd = new SqlCommand("sp_borrow_header_add", conn, tran))
                            {
                                DateTime? ref_doc_date = cbbBorrowType.Value == "P" ? DateTime.MinValue : DateTime.ParseExact(lbQuotationDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@borrow_type", SqlDbType.VarChar, 2).Value = cbbBorrowType.Value;
                                cmd.Parameters.Add("@borrow_date", SqlDbType.DateTime).Value = DateTime.ParseExact(txtBorrowDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture); ; ;
                                cmd.Parameters.Add("@borrow_status", SqlDbType.VarChar, 2).Value = status;
                                if(cbbBorrowType.Value == "QU")
                                {
                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = cbbSupplier.Value;//txtSupplierId.Value;
                                }
                                else if (cbbBorrowType.Value == "P")
                                {
                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = cbbSupplier.Value == null ? DBNull.Value : cbbSupplier.Value;
                                }
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbBorrowType.Value == "P" ? cbbCustomer.Value : hdCustomerId.Value;
                                cmd.Parameters.Add("@objective_type", SqlDbType.VarChar, 1).Value = cbbObjective.Value;
                                cmd.Parameters.Add("@purchase_order_no", SqlDbType.VarChar, 20).Value = txtPO.Value;
                                cmd.Parameters.Add("@purchase_order_date", SqlDbType.Date).Value = DateTime.ParseExact(txtPODate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture); ;

                                cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = cbbBorrowType.Value == "P" ? 0 : cbbQuotation.Value; ;
                                cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = cbbBorrowType.Value == "P" ? string.Empty : cbbQuotation.Text;
                                if (cbbBorrowType.Value == "P")
                                {
                                    cmd.Parameters.Add("@ref_doc_date", SqlDbType.Date).Value = DBNull.Value;
                                }
                                else
                                {
                                    cmd.Parameters.Add("@ref_doc_date", SqlDbType.Date).Value = ref_doc_date;
                                }

                                cmd.Parameters.Add("@customer_code", SqlDbType.VarChar, 20).Value = cbbBorrowType.Value == "P" ? hdCustomerCode.Value : lbCustomerNo.Value;
                                cmd.Parameters.Add("@customer_name", SqlDbType.VarChar, 200).Value = cbbBorrowType.Value == "P" ? cbbCustomer.Text : lbCustomerFirstName.Value;
                                cmd.Parameters.Add("@supplier_name", SqlDbType.VarChar, 200).Value = cbbSupplier.Value == null ? string.Empty : cbbSupplier.Text;
                                cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 2).Value = hdProductType.Value;
                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = txtRemark.Value;

                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                newID = Convert.ToInt32(cmd.ExecuteScalar());

                            }
                            #endregion Borrow Header
                            List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = newID },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = "" }

                        };

                            var lastDataInsert = SqlHelper.ExecuteDataset(tran, "sp_borrow_header_list", arrParm.ToArray());


                            if (lastDataInsert != null)
                            {
                                borrowNo = (from t in lastDataInsert.Tables[0].AsEnumerable()
                                            select t.Field<string>("borrow_no")).FirstOrDefault();
                            }
                            #region Borrow Detail

                            var newDetailId = 0;
                            var oldDetailId = 0;

                            foreach (var row in borrowDetailList)
                            {
                                oldDetailId = row.id;
                                using (SqlCommand cmd = new SqlCommand("sp_borrow_detail_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                    cmd.Parameters.Add("@borrow_no", SqlDbType.VarChar, 20).Value = borrowNo;
                                    cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.item_id;
                                    cmd.Parameters.Add("@item_no", SqlDbType.VarChar, 50).Value = row.item_no;
                                    cmd.Parameters.Add("@item_name", SqlDbType.VarChar, 500).Value = row.item_name;
                                    cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 2).Value = row.item_type;
                                    cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 10).Value = row.unit_code;
                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                    cmd.Parameters.Add("@unit_price", SqlDbType.Decimal).Value = row.unit_price;
                                    cmd.Parameters.Add("@total_price", SqlDbType.Decimal).Value = row.total_price;

                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                    newDetailId = Convert.ToInt32(cmd.ExecuteScalar());

                                }

                                var childData = (from t in borrowDetailMFGList where t.borrow_detail_id == oldDetailId select t).ToList();
                                foreach (var rowDetail in childData)
                                {
                                    rowDetail.borrow_detail_id = newDetailId;// + countId;
                                }
                            }
                            //throw new Exception("xx");
                            #endregion

                            #region MFG Detail

                            foreach (var row in borrowDetailMFGList)
                            {
                                //throw new Exception("x");
                                using (SqlCommand cmd = new SqlCommand("sp_borrow_detail_mfg_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@borrow_no", SqlDbType.VarChar, 20).Value = borrowNo;
                                    cmd.Parameters.Add("@borrow_detail_id", SqlDbType.Int).Value = row.borrow_detail_id;
                                    cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.item_id;
                                    cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                    cmd.ExecuteNonQuery();

                                }
                            }
                            #endregion

                            #region Transection
                            if (status == "CF")
                            {
                                //if (status != "FL")
                                //{
                                foreach (var row in borrowDetailMFGList)
                                {

                                    using (SqlCommand cmd = new SqlCommand("sp_product_mfg_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;
                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
                                }

                                var transNo = string.Empty;
                                using (SqlCommand cmd = new SqlCommand("sp_gen_transection_no", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    transNo = Convert.ToString(cmd.ExecuteScalar());

                                }
                                foreach (var row in borrowDetailList)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                        cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                        cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "BORROW";
                                        cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "IN";
                                        cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                        cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "ยืมสินค้า";
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                        cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = borrowNo;
                                        cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = Convert.ToString(cbbSupplier.Value) == "" ? 0 : cbbSupplier.Value;
                                        cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToString(cbbCustomer.Value) == "" ? 0 : cbbCustomer.Value; 
                                        cmd.ExecuteNonQuery();

                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            newID = dataId;

                            #region Borrow Header
                            //Quotation Header
                            using (SqlCommand cmd = new SqlCommand("sp_borrow_header_edit", conn, tran))
                            {
                                DateTime? ref_doc_date = cbbBorrowType.Value == "P" ? DateTime.MinValue : DateTime.ParseExact(lbQuotationDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = dataId;
                                cmd.Parameters.Add("@borrow_type", SqlDbType.VarChar, 2).Value = cbbBorrowType.Value;
                                cmd.Parameters.Add("@borrow_no", SqlDbType.VarChar, 20).Value = txtBorrowNo.Value;
                                cmd.Parameters.Add("@borrow_date", SqlDbType.DateTime).Value = DateTime.ParseExact(txtBorrowDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture); ; ;
                                cmd.Parameters.Add("@borrow_status", SqlDbType.VarChar, 2).Value = status;
                                if (cbbBorrowType.Value == "QU")
                                {
                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = cbbSupplier.Value;// txtSupplierId.Value;
                                }
                                else if (cbbBorrowType.Value == "P")
                                {
                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = cbbSupplier.Value == null ? DBNull.Value : cbbSupplier.Value;
                                }
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbBorrowType.Value == "P" ? cbbCustomer.Value : hdCustomerId.Value;
                                cmd.Parameters.Add("@objective_type", SqlDbType.VarChar, 1).Value = cbbObjective.Value;
                                cmd.Parameters.Add("@purchase_order_no", SqlDbType.VarChar, 20).Value = txtPO.Value;
                                cmd.Parameters.Add("@purchase_order_date", SqlDbType.Date).Value = DateTime.ParseExact(txtPODate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture); ;

                                cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = cbbBorrowType.Value == "P" ? 0 : cbbQuotation.Value; ;
                                cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = cbbBorrowType.Value == "P" ? string.Empty : cbbQuotation.Text;
                                if (cbbBorrowType.Value == "P")
                                {
                                    cmd.Parameters.Add("@ref_doc_date", SqlDbType.Date).Value = DBNull.Value;
                                }
                                else
                                {
                                    cmd.Parameters.Add("@ref_doc_date", SqlDbType.Date).Value = ref_doc_date;
                                }

                                cmd.Parameters.Add("@customer_code", SqlDbType.VarChar, 20).Value = cbbBorrowType.Value == "P" ? hdCustomerCode.Value : lbCustomerNo.Value;
                                cmd.Parameters.Add("@customer_name", SqlDbType.VarChar, 200).Value = cbbBorrowType.Value == "P" ? cbbCustomer.Text : lbCustomerFirstName.Value;
                                cmd.Parameters.Add("@supplier_name", SqlDbType.VarChar, 200).Value = cbbSupplier.Value == null ? string.Empty : cbbSupplier.Text;
                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = txtRemark.Value;

                                cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                cmd.ExecuteNonQuery();//newID = Convert.ToInt32(cmd.ExecuteScalar());

                            }
                            #endregion Borrow Header

                            #region Borrow Detail

                            var newDetailId = 0;
                            var oldDetailId = 0;

                            foreach (var row in borrowDetailList)
                            {
                                if (row.id < 0)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_borrow_detail_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                        cmd.Parameters.Add("@borrow_no", SqlDbType.VarChar, 20).Value = txtBorrowNo.Value;
                                        cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.item_id;
                                        cmd.Parameters.Add("@item_no", SqlDbType.VarChar, 50).Value = row.item_no;
                                        cmd.Parameters.Add("@item_name", SqlDbType.VarChar, 500).Value = row.item_name;
                                        cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 2).Value = row.item_type;
                                        cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 10).Value = row.unit_code;
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                        cmd.Parameters.Add("@unit_price", SqlDbType.Float).Value = row.unit_price;
                                        cmd.Parameters.Add("@total_price", SqlDbType.Float).Value = row.total_price;

                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                        cmd.ExecuteNonQuery();

                                        var childData = (from t in borrowDetailMFGList where t.borrow_detail_id == oldDetailId select t).ToList();
                                        foreach (var rowDetail in childData)
                                        {
                                            rowDetail.borrow_detail_id = newDetailId;
                                        }

                                    }
                                }
                                else
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_borrow_detail_edit", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                        cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                        cmd.Parameters.Add("@borrow_no", SqlDbType.VarChar, 20).Value = txtBorrowNo.Value;
                                        cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.item_id;
                                        cmd.Parameters.Add("@item_no", SqlDbType.VarChar, 50).Value = row.item_no;
                                        cmd.Parameters.Add("@item_name", SqlDbType.VarChar, 500).Value = row.item_name;
                                        cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 2).Value = row.item_type;
                                        cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 10).Value = row.unit_code;
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                        cmd.Parameters.Add("@unit_price", SqlDbType.Float).Value = row.unit_price;
                                        cmd.Parameters.Add("@total_price", SqlDbType.Float).Value = row.total_price;
                                        cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = row.is_delete;
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                        cmd.ExecuteNonQuery();

                                    }
                                }
                            }

                            #endregion

                            #region Transection
                            // if (status != "FL")
                            // {
                            if (status == "CF")
                            {
                                var transNo = string.Empty;
                                using (SqlCommand cmd = new SqlCommand("sp_gen_transection_no", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    transNo = Convert.ToString(cmd.ExecuteScalar());

                                }
                                foreach (var row in borrowDetailList)
                                {
                                    if (row.item_type == "P")
                                    {
                                        var mfgList = (from t in borrowDetailMFGList where t.item_id == row.item_id && !t.is_deleted select t).ToList();
                                        foreach (var mfgRow in mfgList)
                                        {
                                            if (row.id < 0)
                                            {
                                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "BORROW";
                                                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "IN";
                                                    cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 1;
                                                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "ยืมสินค้า, MFG No. " + mfgRow.mfg_no;
                                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                    cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtBorrowNo.Value;
                                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = Convert.ToString(cbbSupplier.Value) == "" ? 0 : cbbSupplier.Value;
                                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToString(cbbCustomer.Value) == "" ? 0 : cbbCustomer.Value;
                                                    cmd.ExecuteNonQuery();

                                                }
                                            }
                                            else if (row.id > 0 && !row.is_delete)
                                            {
                                                var diffQty = row.qty - row.old_qty;
                                                if (row.qty != 0)
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                    {
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                        cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                        cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "BORROW";
                                                        cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "IN";//diffQty > 0 ? "IN" : "OUT";
                                                        cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 1;
                                                        cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "ยืมสินค้า, MFG No. " + mfgRow.mfg_no;//"แก้ไขจำนวน";
                                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                        cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                        cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtBorrowNo.Value;
                                                        cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = Convert.ToString(cbbSupplier.Value) == "" ? 0 : cbbSupplier.Value;
                                                        cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToString(cbbCustomer.Value) == "" ? 0 : cbbCustomer.Value;
                                                        cmd.ExecuteNonQuery();

                                                    }
                                                }
                                            }
                                            else if (row.id > 0 && row.is_delete)
                                            {
                                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "BORROW";
                                                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "OUT";
                                                    cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = -1;// คืนค่า
                                                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "คืนค่าจาก BR MFG No. " + mfgRow.mfg_no;//row.remark;
                                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                    cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtBorrowNo.Value;
                                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = cbbSupplier.Value;
                                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbCustomer.Value;
                                                    cmd.ExecuteNonQuery();

                                                }
                                            }
                                        }
                                    }
                                    else {
                                        if (row.id < 0)
                                        {
                                            using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "BORROW";
                                                cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "IN";
                                                cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                                cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "ยืมสินค้า";
                                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtBorrowNo.Value;
                                                cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = Convert.ToString(cbbSupplier.Value) == "" ? 0 : cbbSupplier.Value;
                                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToString(cbbCustomer.Value) == "" ? 0 : cbbCustomer.Value;
                                                cmd.ExecuteNonQuery();

                                            }
                                        }
                                        else if (row.id > 0 && !row.is_delete)
                                        {
                                            var diffQty = row.qty - row.old_qty;
                                            if (row.qty != 0)
                                            {
                                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "BORROW";
                                                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "IN";//diffQty > 0 ? "IN" : "OUT";
                                                    cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "ยืมสินค้า";//"แก้ไขจำนวน";
                                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                    cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtBorrowNo.Value;
                                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = Convert.ToString(cbbSupplier.Value) == "" ? 0 : cbbSupplier.Value;
                                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToString(cbbCustomer.Value) == "" ? 0 : cbbCustomer.Value;
                                                    cmd.ExecuteNonQuery();

                                                }
                                            }
                                        }
                                        else if (row.id > 0 && row.is_delete)
                                        {
                                            using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "BORROW";
                                                cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "OUT";
                                                cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty * -1;// คืนค่า
                                                cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "คืนค่าจากยืม";//row.remark;
                                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtBorrowNo.Value;
                                                cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = cbbSupplier.Value;
                                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbCustomer.Value;
                                                cmd.ExecuteNonQuery();

                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Detail MFG

                            foreach (var row in borrowDetailMFGList)
                            {
                                if (row.id < 0)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_borrow_detail_mfg_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@borrow_no", SqlDbType.VarChar, 20).Value = txtBorrowNo.Value;
                                        cmd.Parameters.Add("@borrow_detail_id", SqlDbType.Int).Value = row.borrow_detail_id;
                                        cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.item_id;
                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();
                                    }
                                    // if (status != "FL")
                                    //{


                                    /*using (SqlCommand cmd = new SqlCommand("sp_product_mfg_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;
                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }*/

                                    // }
                                }
                                else if (row.id > 0 && !row.is_deleted)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_borrow_detail_mfg_edit", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                        cmd.Parameters.Add("@borrow_no", SqlDbType.VarChar, 20).Value = txtBorrowNo.Value;
                                        cmd.Parameters.Add("@borrow_detail_id", SqlDbType.Int).Value = row.borrow_detail_id;
                                        cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.item_id;
                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
                                    //if (status != "FL")
                                    //{
                                    /*using (SqlCommand cmd = new SqlCommand("sp_product_mfg_edit_by_mfg ", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }*/
                                    // }
                                }
                                else
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_borrow_detail_mfg_delete", conn, tran))
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

                                if (status == "CF")
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_product_mfg_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;
                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
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
                            Response.Redirect("Borrow.aspx?dataId=" + newID);
                        }
                    }

                }
            }
        }
        protected void gridViewBorrow_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (hdProductType.Value == "P")
            {
                gridViewBorrow.Columns[1].Visible = true;
            }
            else
            {
                gridViewBorrow.Columns[1].Visible = false;
            }
            gridViewBorrow.DataSource = (from t in borrowDetailList where t.is_delete == false select t).ToList();
            gridViewBorrow.DataBind();
        }
        private void BindGridViewBorrow()
        {
            if (hdProductType.Value == "P")
            {
                gridViewBorrow.Columns[1].Visible = true;
            }
            else
            {
                gridViewBorrow.Columns[1].Visible = false;
            }
            gridViewBorrow.DataSource = (from t in borrowDetailList where t.is_delete == false select t).ToList();
            gridViewBorrow.DataBind();
        }

        protected void gridViewQuotationDetail_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
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
                                                   where  (t.product_no.ToUpper().Contains(searchText.ToUpper()) || t.quotation_description.ToUpper().Contains(searchText.ToUpper()))
                                                   select t).ToList();
                gridViewQuotationDetail.DataBind();
            }
            gridViewQuotationDetail.PageIndex = 0;
        }
        private void BindGridViewQuotationDetail()
        {
            string searchText = txtSearchText.Value;
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
        }

        protected void gridViewQuotationDetail_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            ASPxCheckBox checkBox = gridViewQuotationDetail.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkQuotationDetail") as ASPxCheckBox;
            if (checkBox != null)
            {
                if (e.DataColumn.FieldName == "is_selected")
                {
                    if (selectedBorrowDetailList != null)
                    {
                        var row = (from t in selectedBorrowDetailList where t.item_id == Convert.ToInt32(e.KeyValue) && !t.is_delete select t).FirstOrDefault();
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
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_quotation_list_data", arrParm.ToArray());
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
                        
                    }
                    //select cbb QU
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                                {
                                    new SqlParameter("@quotation_no", SqlDbType.VarChar) { Value = Convert.ToString(data["quotation_no"]) },
                                    new SqlParameter("@item_type", SqlDbType.VarChar) { Value = "" },
                                };
                        conn.Open();
                        var result = SqlHelper.ExecuteDataset(conn, "sp_dropdown_supplier_qu", arrParm.ToArray());

                        conn.Close();
                        if (result.Tables.Count > 0)
                        {
                            var rowdata = result.Tables[0].AsEnumerable().FirstOrDefault();
                            if (rowdata != null)
                            {
                                headerData.supplier_id = Convert.ToInt32(rowdata["data_value"]);
                                headerData.supplier_titel = Convert.ToString(rowdata["data_text"]);
                            }
                           
                        }
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

                    HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_BORROW"] = quotationDetailList;
                }

                return headerData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [WebMethod]
        public static string GetProductDetailData(int qu_id, int supplier_id, string product_type)
        {
            try
            {
                var borrowDetailData = (List<BorrowDetail>)HttpContext.Current.Session["SESSION_BORROW_DETAIL_BORROW"];
                var data = new List<ProductDetail>();
                var dsResult = new DataSet();
                if (qu_id == 0)
                {

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
                                    
                                    item_unit = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]),
                                    selling_price = 0,//Convert.IsDBNull(row["selling_price"]) ? 0 : Convert.ToInt32(row["selling_price"]),
                                });
                            }
                        }
                    }

                    HttpContext.Current.Session["SESSION_PRODUCT_DETAIL_BORROW"] = data;
                    HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_BORROW"] = borrowDetailData;
                }
                else
                {
                    try
                    {
                        var quotationDetailList = new List<QuotationDetail>(); //(List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_PR"];
                        using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                        {
                            //Create array of Parameters
                            List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = qu_id },
                        };
                            conn.Open();
                            dsResult = SqlHelper.ExecuteDataset(conn, "sp_quotation_list_data_br", arrParm.ToArray());
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
                                        unit_price = 0,//Convert.IsDBNull(row["unit_price"]) ? 0.0m : Convert.ToDecimal(row["unit_price"]),
                                        is_deleted = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                        sort_no = Convert.IsDBNull(row["sort_no"]) ? 1 : Convert.ToInt32(row["sort_no"]),
                                        parant_id = Convert.IsDBNull(row["parent_id"]) ? 0 : Convert.ToInt32(row["parent_id"]),
                                        quotation_line = Convert.IsDBNull(row["quotation_line"]) ? string.Empty : Convert.ToString(row["quotation_line"]),
                                        mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                        is_history_issue_mfg = Convert.IsDBNull(row["is_history_issue_mfg"]) ? false : Convert.ToBoolean(row["is_history_issue_mfg"]),
                                        product_type = Convert.IsDBNull(row["product_type"]) ? string.Empty : Convert.ToString(row["product_type"]),

                                    });
                                }
                            }
                            #endregion Quotation Detail

                            HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_BORROW"] = quotationDetailList;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return "BORROW_DATA";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [WebMethod]
        public static string AddSelectedProduct(string key, bool isSelected, string type, string product_type)
        {
            try
            {
                var productDetail = (List<ProductDetail>)HttpContext.Current.Session["SESSION_PRODUCT_DETAIL_BORROW"];
                var quotationDetail = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_BORROW"];
                var selectedBorrowData = (List<BorrowDetail>)HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_BORROW"];

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
                                if (selectedBorrowData != null) // ถ้ามี PRDetail อยุ่แล้ว
                                {
                                    var checkExist = (from t in selectedBorrowData where t.item_no == Convert.ToString(key) select t).FirstOrDefault();
                                    if (checkExist == null)
                                    {
                                        selectedBorrowData.Add(new BorrowDetail()
                                        {
                                            id = (selectedBorrowData.Count + 1) * -1,
                                            is_delete = false,
                                            item_name = selectedProductRow.product_name,
                                            item_id = selectedProductRow.product_id,
                                            qty = 1,
                                            item_type_text = product_type == "P"?"Product": "Spare Part",
                                            item_no = selectedProductRow.product_no,
                                            unit_code = selectedProductRow.item_unit,
                                            item_type = product_type,
                                            unit_price = 0,//selectedProductRow.selling_price,
                                            total_price = 0,//selectedProductRow.selling_price * 1,
                                            sort_no = selectedBorrowData.Count == 0 ? 1 : (from t in selectedBorrowData select t.sort_no).Max() + 1
                                        });
                                    }
                                    else // ไม่ต้องทำอะไร
                                    {
                                        checkExist.qty += 1;
                                        checkExist.total_price = checkExist.qty * checkExist.unit_price;
                                    }
                                }
                                else // ใหม่เอี่ยม
                                {
                                    selectedBorrowData = new List<BorrowDetail>();
                                    selectedBorrowData.Add(new BorrowDetail()
                                    {
                                        id = (selectedBorrowData.Count + 1) * -1,
                                        is_delete = false,
                                        item_name = selectedProductRow.product_name,
                                        item_id = selectedProductRow.product_id,
                                        qty = 1,
                                        item_no = selectedProductRow.product_no,
                                        unit_code = selectedProductRow.item_unit,
                                        item_type = product_type,
                                        item_type_text = product_type == "P" ? "Product" : "Spare Part",
                                        unit_price = 0,//selectedProductRow.selling_price,
                                        total_price = 0,//selectedProductRow.selling_price * 1,
                                        sort_no = selectedBorrowData.Count == 0 ? 1 : (from t in selectedBorrowData select t.sort_no).Max() + 1
                                    });
                                }
                            }
                            else
                            {
                                selectedProductRow.is_selected = false;
                                var checkExist = (from t in selectedBorrowData where t.item_no == Convert.ToString(key) select t).FirstOrDefault();
                                if (checkExist != null) // ต้องเข้า case นี้ เท่านั้น
                                {
                                    if (checkExist.id < 0)
                                    {
                                        selectedBorrowData.Remove(checkExist);
                                    }
                                    else
                                    {
                                        checkExist.is_delete = true;
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
                                if (selectedBorrowData != null) // ถ้ามี PRDetail อยุ่แล้ว
                                {
                                    var checkExist = (from t in selectedBorrowData where t.item_no == Convert.ToString(key) select t).FirstOrDefault();
                                    if (checkExist == null)
                                    {
                                        selectedBorrowData.Add(new BorrowDetail()
                                        {
                                            id = (selectedBorrowData.Count + 1) * -1,
                                            is_delete = false,
                                            item_name = selectedQuotationRow.quotation_description,
                                            item_id = selectedQuotationRow.product_id,
                                            qty = selectedQuotationRow.qty,
                                            item_type_text = product_type == "P" ? "Product" : "Spare Part",
                                            item_no = selectedQuotationRow.product_no,
                                            unit_code = selectedQuotationRow.unit_code,
                                            item_type = selectedQuotationRow.product_type,
                                            unit_price = 0,//selectedQuotationRow.unit_price,
                                            total_price = 0,//selectedQuotationRow.unit_price * selectedQuotationRow.qty,
                                            sort_no = selectedBorrowData.Count == 0 ? 1 : (from t in selectedBorrowData select t.sort_no).Max() + 1
                                        });
                                    }
                                    else // ไม่ต้องทำอะไร
                                    {
                                        checkExist.qty += 1;
                                        checkExist.total_price = checkExist.qty * checkExist.unit_price;
                                    }
                                }
                                else // ใหม่เอี่ยม
                                {
                                    selectedBorrowData = new List<BorrowDetail>();
                                    selectedBorrowData.Add(new BorrowDetail()
                                    {

                                        id = (selectedBorrowData.Count + 1) * -1,
                                        is_delete = false,
                                        item_name = selectedQuotationRow.quotation_description,
                                        item_id = selectedQuotationRow.product_id,
                                        qty = selectedQuotationRow.qty,
                                        item_type_text = product_type == "P" ? "Product" : "Spare Part",
                                        item_no = selectedQuotationRow.product_no,
                                        unit_code = selectedQuotationRow.unit_code,
                                        item_type = selectedQuotationRow.product_type,
                                        unit_price = 0,//selectedQuotationRow.unit_price,
                                        total_price = 0,//selectedQuotationRow.unit_price * selectedQuotationRow.qty,
                                        sort_no = selectedBorrowData.Count == 0 ? 1 : (from t in selectedBorrowData select t.sort_no).Max() + 1
                                    });
                                }
                            }
                            else
                            {
                                selectedQuotationRow.is_selected = false;
                                var checkExist = (from t in selectedBorrowData where t.item_no == Convert.ToString(key) select t).FirstOrDefault();
                                if (checkExist != null) // ต้องเข้า case นี้ เท่านั้น
                                {
                                    if (checkExist.id < 0)
                                    {
                                        selectedBorrowData.Remove(checkExist);
                                    }
                                    else
                                    {
                                        checkExist.is_delete = true;
                                    }
                                }
                                else
                                {

                                }

                            }
                        }
                    }
                }
                HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_BORROW"] = selectedBorrowData;
                HttpContext.Current.Session["SESSION_PRODUCT_DETAIL_BORROW"] = productDetail;
                HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_BORROW"] = quotationDetail;

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
                var selectedBorrowData = (List<BorrowDetail>)HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_BORROW"];
                var borrowDetailData = (List<BorrowDetail>)HttpContext.Current.Session["SESSION_BORROW_DETAIL_BORROW"];
                if (selectedBorrowData != null)
                {
                    borrowDetailData = new List<BorrowDetail>();
                    borrowDetailData = selectedBorrowData;
                }
                HttpContext.Current.Session["SESSION_BORROW_DETAIL_BORROW"] = borrowDetailData;
                HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_BORROW"] = null;

                return type;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        protected void gridViewProductDetail_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
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
        private void BindGridViewProductDetail()
        {
            string searchText = TextProduct.Value;
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
        }

        protected void gridViewProductDetail_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            ASPxCheckBox checkBox = gridViewProductDetail.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkProductDetail") as ASPxCheckBox;
            if (checkBox != null)
            {
                if (e.DataColumn.FieldName == "is_selected")
                {
                    if (selectedBorrowDetailList != null)
                    {
                        var row = (from t in selectedBorrowDetailList where t.item_no == Convert.ToString(e.KeyValue) && !t.is_delete select t).FirstOrDefault();
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
        public static string DeleteBorrowDetail(string id)
        {
            try
            {
                var borrowDetailData = (List<BorrowDetail>)HttpContext.Current.Session["SESSION_BORROW_DETAIL_BORROW"];
                if (borrowDetailData != null)
                {
                    var row = (from t in borrowDetailData where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (row != null)
                    {
                        if (row.id < 0)
                        {
                            borrowDetailData.Remove(row);
                        }
                        else
                        {
                            row.is_delete = true;
                        }
                    }
                }
                HttpContext.Current.Session["SESSION_BORROW_DETAIL_BORROW"] = borrowDetailData;

                return "SUCCESS";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [WebMethod]
        public static BorrowDetail EditBorrowDetail(string id)
        {
            try
            {
                var borrowDetailData = (List<BorrowDetail>)HttpContext.Current.Session["SESSION_BORROW_DETAIL_BORROW"];
                if (borrowDetailData != null)
                {
                    var row = (from t in borrowDetailData where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (row != null)
                    {
                        //row.display_receive_date = row.receive_date.HasValue ? row.receive_date.Value.ToString("dd/MM/yyyy") : string.Empty;
                        return row;
                    }

                }
                return new BorrowDetail();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [WebMethod]
        public static string SubmitBorrowDetail(string id, string qty, string unit_price)
        {
            try
            {
                var borrowDetailData = (List<BorrowDetail>)HttpContext.Current.Session["SESSION_BORROW_DETAIL_BORROW"];
                if (borrowDetailData != null)
                {
                    var row = (from t in borrowDetailData where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (row != null)
                    {

                        row.qty = Convert.ToInt32(qty);
                        row.unit_price = Convert.ToInt32(unit_price);
                        row.total_price = row.qty * row.unit_price;
                    }

                }
                HttpContext.Current.Session["SESSION_BORROW_DETAIL_BORROW"] = borrowDetailData;
                return "SUCCESS";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            SaveData("FL");
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            SaveData("CF");
        }

        [WebMethod]
        public static string ChangedData()
        {
            string returnData = string.Empty;
            //HttpContext.Current.Session.Remove("SESSION_BORROW_DETAIL_BORROW");
            //HttpContext.Current.Session.Remove("SESSION_PRODUCT_DETAIL_BORROW");
            //HttpContext.Current.Session.Remove("SESSION_QUOTATION_DETAIL_BORROW");
            HttpContext.Current.Session.Remove("SESSION_PRODUCT_DETAIL_BORROW");
            //Session.Remove("SESSION_QUOTATION_DETAIL_BORROW");
            HttpContext.Current.Session.Remove("SESSION_BORROW_DETAIL_BORROW");
            HttpContext.Current.Session.Remove("SESSION_SELECTED_BORROW_DETAIL_BORROW");
            HttpContext.Current.Session.Remove("SESSION_MFG_DETAIL_BORROW");
            HttpContext.Current.Session.Remove("SESSION_SELECTED_BORROW_DETAIL_MFG_BORROW");
            HttpContext.Current.Session.Remove("SESSION_BORROW_DETAIL_MFG_BORROW");
            return returnData;
        }

        #region MFG

        #endregion

        protected void gridMFG_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridMFG.DataSource = (from t in selectedBorrowDetailMFGList where !t.is_deleted select t).ToList();
            gridMFG.DataBind();
        }
        protected void BindGridMFGDetail()

        {
            gridMFG.DataSource = (from t in selectedBorrowDetailMFGList where !t.is_deleted select t).ToList();
            gridMFG.DataBind();
        }
        [WebMethod]
        public static BorrowDetail GetMFGDetail(int id, int borrowDetailId)
        {
            try
            {
                var dsResult = new DataSet();
                var borrowMFGDetailData = (List<BorrowDetailMFG>)HttpContext.Current.Session["SESSION_BORROW_DETAIL_MFG_BORROW"];
                if (borrowMFGDetailData != null && borrowMFGDetailData.Count != 0)
                {
                    var row = (from t in borrowMFGDetailData where t.item_id == Convert.ToInt32(id) select t).ToList();
                    if (row != null)
                    {
                        HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_MFG_BORROW"] = row;
                    }
                }
                //else
                //{
                //    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                //    {
                //        //Create array of Parameters
                //        List<SqlParameter> arrParm = new List<SqlParameter>
                //        {
                //            new SqlParameter("@borrow_detail_id", SqlDbType.Int) { Value =  Convert.ToInt32(borrowDetailId) },
                //            new SqlParameter("@item_id", SqlDbType.Int) { Value = Convert.ToInt32(id) },
                //        };
                //        conn.Open();
                //        dsResult = SqlHelper.ExecuteDataset(conn, "sp_borrow_detail_mfg_lish", arrParm.ToArray());
                //        conn.Close();
                //    }
                //    if (dsResult != null)
                //    {
                //        borrowMFGDetailData = new List<BorrowDetailMFG>();
                //        var row = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                //        foreach (var detail in row)
                //        {
                //            borrowMFGDetailData.Add(new BorrowDetailMFG()
                //            {
                //                id = Convert.ToInt32(detail["id"]),
                //                mfg_no = Convert.IsDBNull(detail["mfg_no"]) ? string.Empty : Convert.ToString(detail["mfg_no"]),
                //                borrow_no = Convert.IsDBNull(detail["borrow_no"]) ? string.Empty : Convert.ToString(detail["borrow_no"]),
                //                is_deleted = Convert.IsDBNull(detail["is_delete"]) ? false : Convert.ToBoolean(detail["is_delete"]),
                //                borrow_detail_id = Convert.IsDBNull(detail["borrow_detail_id"]) ? 0 : Convert.ToInt32(detail["borrow_detail_id"]),

                //            });
                //        }
                //    }
                //    HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_MFG_BORROW"] = borrowMFGDetailData;
                //}
                //////////////////////////////////////////////////

                var borrowDetailData = (List<BorrowDetail>)HttpContext.Current.Session["SESSION_BORROW_DETAIL_BORROW"];
                if (borrowDetailData != null)
                {
                    var row = (from t in borrowDetailData where t.item_id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (row != null)
                    {
                        return row;
                    }

                }
                //else
                //{
                //    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                //    {
                //        //Create array of Parameters
                //        List<SqlParameter> arrParm = new List<SqlParameter>
                //        {
                //            new SqlParameter("@borrow_detail_id", SqlDbType.Int) { Value =  Convert.ToInt32(borrowDetailId) },
                //            new SqlParameter("@item_id", SqlDbType.Int) { Value = Convert.ToInt32(id) },
                //        };
                //        conn.Open();
                //        dsResult = SqlHelper.ExecuteDataset(conn, "sp_borrow_detail_lish", arrParm.ToArray());
                //        conn.Close();
                //    }
                //    if (dsResult != null)
                //    {

                //        borrowDetailData = new List<BorrowDetail>();
                //        var row = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                //        foreach (var detail in row)
                //        {
                //            borrowDetailData.Add(new BorrowDetail()
                //            {
                //                item_id = Convert.ToInt32(detail["item_id"]),
                //                qty = Convert.IsDBNull(detail["qty"]) ? 0 : Convert.ToInt32(detail["qty"]),
                //                item_name = Convert.IsDBNull(detail["item_name"]) ? string.Empty : Convert.ToString(detail["item_name"]),

                //            });
                //        }
                //    }
                //    HttpContext.Current.Session["SESSION_BORROW_DETAIL_BORROW"] = borrowDetailData;

                //}

                return new BorrowDetail();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        [WebMethod]
        public static string AddMFG(string mfg_no, int product_id, int borrow_detail_id, int qty_product)
        {
            var returnData = string.Empty;

            var mfgProductList = (List<BorrowDetailMFG>)HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_MFG_BORROW"];
            var row = (from t in mfgProductList where t.is_deleted == false select t).ToList();

            if (row != null && row.Count != 0)
            {
                if (qty_product > row.Count)
                {
                    var checkExist = (from t in row
                                      where t.mfg_no == mfg_no
                                      select t).FirstOrDefault();
                    if (checkExist == null)
                    {

                        mfgProductList.Add(new BorrowDetailMFG()
                        {
                            id = (mfgProductList.Count + 1) * -1,
                            mfg_no = mfg_no,
                            item_id = product_id,
                            borrow_detail_id = borrow_detail_id
                        });
                    }
                }
                else
                {
                    returnData = "สินค้านี้สามารถเพิ่ม MFG ได้" + qty_product + "เท่านั้น";
                }
            }
            else
            {
                mfgProductList = new List<BorrowDetailMFG>();
                mfgProductList.Add(new BorrowDetailMFG()
                {
                    id = (mfgProductList.Count + 1) * -1,
                    mfg_no = mfg_no,
                    item_id = product_id,
                    borrow_detail_id = borrow_detail_id
                });

            }

            HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_MFG_BORROW"] = mfgProductList;


            return returnData;
        }
        [WebMethod]
        public static BorrowDetailMFG EditMFG(int id)
        {
            var mfgProductList = (List<BorrowDetailMFG>)HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_MFG_BORROW"];
            if (mfgProductList != null)
            {
                var row = (from t in mfgProductList where t.id == id select t).FirstOrDefault();
                if (row != null)
                {
                    return row;
                }
            }
            return new BorrowDetailMFG(); // ไม่มี Data
        }
        [WebMethod]
        public static string DeleteMFG(int id)
        {
            var returnData = string.Empty;
            var mfgProductList = (List<BorrowDetailMFG>)HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_MFG_BORROW"];
            if (mfgProductList != null)
            {
                var row = (from t in mfgProductList where t.id == id select t).FirstOrDefault();

                if (row != null)
                {
                    if (Convert.ToInt32(id) < 0)
                    {
                        mfgProductList.Remove(row);
                    }
                    else
                    {

                        row.is_deleted = true;
                    }
                }

            }
            HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_MFG_BORROW"] = mfgProductList;

            return returnData; // ไม่มี Data
        }
        [WebMethod]
        public static string SubmitEditMFG(int id, string mfg_no)
        {
            try
            {
                var returnData = "SUCCESS";
                var mfgProductList = (List<BorrowDetailMFG>)HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_MFG_BORROW"];
                if (mfgProductList != null)
                {
                    var checkExsit = (from t in mfgProductList where t.id != id && t.mfg_no == mfg_no select t).FirstOrDefault();
                    if (checkExsit == null)
                    {
                        var row = (from t in mfgProductList where t.id == id select t).FirstOrDefault();
                        if (row != null)
                        {
                            row.mfg_no = mfg_no;
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
        public static string SubmitMFGProduct(int id)
        {
            var returnData = string.Empty;
            try
            {
                var borrowDetailData = (List<BorrowDetail>)HttpContext.Current.Session["SESSION_BORROW_DETAIL_BORROW"];
                var selectedMFGList = (List<BorrowDetailMFG>)HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_MFG_BORROW"];
                var borrowMFGDetailData = (List<BorrowDetailMFG>)HttpContext.Current.Session["SESSION_BORROW_DETAIL_MFG_BORROW"];

                if (borrowDetailData != null)
                {
                    var qtyproduct = (from t in borrowDetailData where t.item_id == Convert.ToInt32(id) select t.qty).FirstOrDefault();
                    if (qtyproduct == (from t in selectedMFGList where !t.is_deleted select t).Count())
                    {
                        if (borrowMFGDetailData == null)
                        {
                            borrowMFGDetailData = new List<BorrowDetailMFG>();
                        }
                        if (selectedMFGList != null)
                        {
                            foreach (var row in selectedMFGList)
                            {
                                var checkExist = (from t in borrowMFGDetailData where t.id == row.id && t.borrow_detail_id == row.borrow_detail_id select t).FirstOrDefault();
                                if (checkExist != null)
                                {
                                    checkExist.borrow_detail_id = row.borrow_detail_id;
                                    checkExist.mfg_no = row.mfg_no;
                                    checkExist.is_deleted = row.is_deleted;
                                }
                                else
                                {
                                    borrowMFGDetailData.Add(row);
                                }
                            }
                        }
                        HttpContext.Current.Session["SESSION_BORROW_DETAIL_MFG_BORROW"] = borrowMFGDetailData;

                    }
                    else
                    {
                        returnData = "กรุณาเพิ่ม MFG ให้เท่ากับจำนวนสินค้า : " + qtyproduct;
                    }
                }
                return returnData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static string SelectAllQuDetail(bool selected, string borrowType, string product_type, string search)
        {
            var result = "";
            var productDetail = (List<ProductDetail>)HttpContext.Current.Session["SESSION_PRODUCT_DETAIL_BORROW"];
            var quotationDetail = (List<QuotationDetail>)HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_BORROW"];
            var selectedBorrowData = (List<BorrowDetail>)HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_BORROW"];
           
            if (borrowType == "P")
            {
                if(productDetail != null)
                {
                    var detailTemp = (from t in productDetail
                                      where t.product_no.ToUpper().Contains(search.ToUpper())
                                      select t).ToList();

                    foreach (var data in detailTemp)
                    {
                        if (selected)
                        {
                            data.is_selected = true;
                            if(selectedBorrowData != null && selectedBorrowData.Count > 0)
                            {
                                var rowExist = (from t in selectedBorrowData where t.item_no == data.product_no select t).FirstOrDefault();

                                if (rowExist != null) // กรณี Newmode
                                {
                                    rowExist.qty = data.qty;
                                }
                                else
                                {
                                    selectedBorrowData.Add(new BorrowDetail()
                                    {
                                        id = (selectedBorrowData.Count + 1) * -1,
                                        is_delete = false,
                                        item_name = data.product_name,
                                        item_id = data.product_id,
                                        qty = 1,
                                        item_no = data.product_no,
                                        unit_code = data.item_unit,
                                        item_type = product_type,
                                        unit_price = 0,//data.selling_price,
                                        total_price = 0,//data.selling_price * data.qty,
                                        sort_no = selectedBorrowData.Count == 0 ? 1 : (from t in selectedBorrowData select t.sort_no).Max() + 1
                                    });
                                }
                            }
                            else
                            {
                                if (selectedBorrowData == null)
                                {
                                    selectedBorrowData = new List<BorrowDetail>();
                                }
                                selectedBorrowData.Add(new BorrowDetail()
                                {
                                    id = (selectedBorrowData.Count + 1) * -1,
                                    is_delete = false,
                                    item_name = data.product_name,
                                    item_id = data.product_id,
                                    qty = 1,
                                    item_no = data.product_no,
                                    unit_code = data.item_unit,
                                    item_type = product_type,
                                    unit_price = 0,//data.selling_price,
                                    total_price = 0,//data.selling_price * data.qty,
                                    sort_no = selectedBorrowData.Count == 0 ? 1 : (from t in selectedBorrowData select t.sort_no).Max() + 1
                                });
                            }
                        }
                        else
                        {
                            var rowExist = (from t in selectedBorrowData where t.item_no == data.product_no select t).FirstOrDefault();
                            data.is_selected = false;
                            if (rowExist != null) // กรณี Newmode
                            {
                                if(rowExist.id < 0)
                                {
                                    selectedBorrowData.Remove(rowExist);
                                }
                                else
                                {
                                    rowExist.is_delete = true;
                                }
                            }
                        }
                    }
                }
                HttpContext.Current.Session["SESSION_PRODUCT_DETAIL_BORROW"] = productDetail;
                HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_BORROW"] = selectedBorrowData;
                result = "P";
            }
            else if(borrowType == "QU")
            {
                if (quotationDetail != null)
                {
                    var detailTemp = (from t in quotationDetail
                                      where t.product_no.ToUpper().Contains(search.ToUpper())
                                      select t).ToList();

                    foreach (var data in detailTemp)
                    {
                        if (selected)
                        {
                            data.is_selected = true;
                            if (selectedBorrowData != null && selectedBorrowData.Count > 0)
                            {
                                var rowExist = (from t in selectedBorrowData where t.item_no == data.product_no select t).FirstOrDefault();

                                if (rowExist != null) // กรณี Newmode
                                {
                                    rowExist.qty = data.qty;
                                }
                                else
                                {
                                    selectedBorrowData.Add(new BorrowDetail()
                                    {
                                        id = (selectedBorrowData.Count + 1) * -1,
                                        is_delete = false,
                                        item_name = data.quotation_description,
                                        item_id = data.product_id,
                                        qty = data.qty,
                                        item_no = data.product_no,
                                        unit_code = data.unit_code,
                                        item_type = data.product_type,
                                        unit_price = 0,//data.unit_price,
                                        total_price = 0,//data.unit_price * data.qty,
                                        sort_no = selectedBorrowData.Count == 0 ? 1 : (from t in selectedBorrowData select t.sort_no).Max() + 1
                                    });
                                }
                            }
                            else
                            {
                                if (selectedBorrowData == null)
                                {
                                    selectedBorrowData = new List<BorrowDetail>();
                                }
                                selectedBorrowData.Add(new BorrowDetail()
                                {
                                    id = (selectedBorrowData.Count + 1) * -1,
                                    is_delete = false,
                                    item_name = data.quotation_description,
                                    item_id = data.product_id,
                                    qty = data.qty,
                                    item_no = data.product_no,
                                    unit_code = data.unit_code,
                                    item_type = data.product_type,
                                    unit_price = 0,//data.unit_price,
                                    total_price = 0,//data.unit_price * data.qty,
                                    sort_no = selectedBorrowData.Count == 0 ? 1 : (from t in selectedBorrowData select t.sort_no).Max() + 1
                                });
                            }
                        }
                        else
                        {
                            var rowExist = (from t in selectedBorrowData where t.item_no == data.product_no select t).FirstOrDefault();
                            data.is_selected = false;
                            if (rowExist != null) // กรณี Newmode
                            {
                                if (rowExist.id < 0)
                                {
                                    selectedBorrowData.Remove(rowExist);
                                }
                                else
                                {
                                    rowExist.is_delete = true;
                                }
                            }
                        }
                    }
                }
                HttpContext.Current.Session["SESSION_QUOTATION_DETAIL_BORROW"] = quotationDetail;
                HttpContext.Current.Session["SESSION_SELECTED_BORROW_DETAIL_BORROW"] = selectedBorrowData;
                result = "QU";
            }
            return result; 
        }

        protected void cbbSupplier_Callback(object sender, CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');

            string type = cbbBorrowType.Value;
            string quotation_no = "";
            string product_type = "";
            if (type == "QU")
            {
                quotation_no = cbbQuotation.Text;
            }
            else
            {
                product_type = cbbProductType.Value;
            }

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                                {
                                    new SqlParameter("@quotation_no", SqlDbType.VarChar) { Value = quotation_no },
                                    new SqlParameter("@item_type", SqlDbType.VarChar) { Value = product_type },
                                };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_supplier_qu", arrParm.ToArray());

                conn.Close();
                if (dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        cbbSupplier.DataSource = dsResult;
                        cbbSupplier.TextField = "data_text";
                        cbbSupplier.ValueField = "data_value";
                        cbbSupplier.DataBind();

                        if (param.Length > 0)
                        {
                            cbbSupplier.Text = param[1].ToString();
                            cbbSupplier.Value = param[0].ToString();
                        }
                    }
                }
            }
        }
    }
}