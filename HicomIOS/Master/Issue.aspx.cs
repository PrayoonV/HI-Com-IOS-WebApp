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
    public partial class Issue : MasterDetailPage
    {
        public int dataId = 0;
        #region Member
        public class SaleOrderData
        {
            public int id { get; set; }
            public string sales_order_no { get; set; }
            public string sales_order_date { get; set; }
            public string attention_name { get; set; }
            public string project_name { get; set; }
            public string quotation_subject { get; set; }
            public string quotation_type { get; set; }
            public string quotation_no { get; set; }
            public string customer_code { get; set; }
            public string company_name_tha { get; set; }
            public string address_bill_tha { get; set; }
            public string tel { get; set; }
            public string fax { get; set; }
            public int customer_id { get; set; }
        }
        public class SaleOrderDetail
        {
            public bool is_selected { get; set; }
            public int id { get; set; }
            public string sales_order_no { get; set; }
            public int product_id { get; set; }
            public string product_no { get; set; }
            public string saleorder_description { get; set; }
            public int qty { get; set; }
            public int so_qty { get; set; }
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
            public string quotation_type { get; set; }
            public string product_type { get; set; }
            public string quotation_no { get; set; }
        }
        public class IssueDetailMFG
        {
            public int id { get; set; }
            public string issue_stock_no { get; set; }
            public int issue_stock_detail_id { get; set; }
            public int product_id { get; set; }
            public string mfg_no { get; set; }
            public int qty { get; set; }
            public int unit_warranty { get; set; }
            public int air_end_warranry { get; set; }
            public decimal service_fee { get; set; }
            public bool is_deleted { get; set; }
            public string pr_no { get; set; }
            public string receive_no { get; set; }
        }
        public class IssueDetail
        {
            public int id { get; set; }
            public int issue_stock_id { get; set; }
            public string issue_stock_no { get; set; }
            public string product_no { get; set; }
            public int product_id { get; set; }
            public string product_name_tha { get; set; }
            public int qty { get; set; }
            public int so_qty { get; set; }
            public string unit_tha { get; set; }
            public string remark { get; set; }
            public bool is_delete { get; set; }
            public int sale_order_detail_id { get; set; }
            public string product_type { get; set; }
            public int old_qty { get; set; }
            public string quotation_no { get; set; }
        }
        public class MFGDetail
        {
            public bool is_selected { get; set; }
            public string mfg_no { get; set; }
            public int unit_warranty { get; set; }
            public int air_end_warranty { get; set; }
            public decimal service_fee { get; set; }
            public string pr_no { get; set; }
            public string receive_no { get; set; }
        }
        List<IssueDetail> issueDetailList
        {
            get
            {
                if (Session["SESSION_ISSUE_DETAIL_ISSUE"] == null)
                    Session["SESSION_ISSUE_DETAIL_ISSUE"] = new List<IssueDetail>();
                return (List<IssueDetail>)Session["SESSION_ISSUE_DETAIL_ISSUE"];
            }
            set
            {
                Session["SESSION_ISSUE_DETAIL_ISSUE"] = value;
            }
        }
        List<IssueDetail> selectedIssueDetailList
        {
            get
            {
                if (Session["SESSION_SELECTED_ISSUE_DETAIL_ISSUE"] == null)
                    Session["SESSION_SELECTED_ISSUE_DETAIL_ISSUE"] = new List<IssueDetail>();
                return (List<IssueDetail>)Session["SESSION_SELECTED_ISSUE_DETAIL_ISSUE"];
            }
            set
            {
                Session["SESSION_SELECTED_ISSUE_DETAIL_ISSUE"] = value;
            }
        }
        List<IssueDetailMFG> issueDetailMFGList
        {
            get
            {
                if (Session["SESSION_ISSUE_DETAIL_MFG_ISSUE"] == null)
                    Session["SESSION_ISSUE_DETAIL_MFG_ISSUE"] = new List<IssueDetailMFG>();
                return (List<IssueDetailMFG>)Session["SESSION_ISSUE_DETAIL_MFG_ISSUE"];
            }
            set
            {
                Session["SESSION_ISSUE_DETAIL_MFG_ISSUE"] = value;
            }
        }
        List<IssueDetailMFG> selectedIssueDetailMFGList
        {
            get
            {
                if (Session["SESSION_SELECTED_ISSUE_DETAIL_MFG_ISSUE"] == null)
                    Session["SESSION_SELECTED_ISSUE_DETAIL_MFG_ISSUE"] = new List<IssueDetailMFG>();
                return (List<IssueDetailMFG>)Session["SESSION_SELECTED_ISSUE_DETAIL_MFG_ISSUE"];
            }
            set
            {
                Session["SESSION_SELECTED_ISSUE_DETAIL_MFG_ISSUE"] = value;
            }
        }
        List<SaleOrderDetail> saleOrderDetailList
        {
            get
            {
                if (Session["SESSION_SALE_ORDER_DETAIL_ISSUE"] == null)
                    Session["SESSION_SALE_ORDER_DETAIL_ISSUE"] = new List<SaleOrderDetail>();
                return (List<SaleOrderDetail>)Session["SESSION_SALE_ORDER_DETAIL_ISSUE"];
            }
            set
            {
                Session["SESSION_SALE_ORDER_DETAIL_ISSUE"] = value;
            }
        }
        List<MFGDetail> mfgDetailList
        {
            get
            {
                if (Session["SESSION_MFG_DETAIL_ISSUE"] == null)
                    Session["SESSION_MFG_DETAIL_ISSUE"] = new List<MFGDetail>();
                return (List<MFGDetail>)Session["SESSION_MFG_DETAIL_ISSUE"];
            }
            set
            {
                Session["SESSION_MFG_DETAIL_ISSUE"] = value;
            }
        }
        List<MFGDetail> mfgDetailListTemp
        {
            get
            {
                if (Session["SESSION_MFG_DETAIL_ISSUE_TEMP"] == null)
                    Session["SESSION_MFG_DETAIL_ISSUE_TEMP"] = new List<MFGDetail>();
                return (List<MFGDetail>)Session["SESSION_MFG_DETAIL_ISSUE_TEMP"];
            }
            set
            {
                Session["SESSION_MFG_DETAIL_ISSUE_TEMP"] = value;
            }
        }

        private DataSet dsIssueData = new DataSet();
        #endregion
        protected void ClearWorkingSession()
        {
            Session.Remove("SESSION_ISSUE_DETAIL_ISSUE"); // Issue Detail Data
            Session.Remove("SESSION_SALE_ORDER_DETAIL_ISSUE"); // Sale Order For Select Data
            Session.Remove("SESSION_SELECTED_ISSUE_DETAIL_ISSUE"); // Selected Issue Detail
            Session.Remove("SESSION_ISSUE_DETAIL_MFG_ISSUE"); // Issue MFG Detail Data
            Session.Remove("SESSION_MFG_DETAIL_ISSUE"); // MFG Product Data
            Session.Remove("SESSION_SELECTED_ISSUE_DETAIL_MFG_ISSUE");  // Selected Issue MFG Detail Data
        }

        public override string PageName { get { return "Create Issue"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }
        public override void OnFilterChanged()
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            dataId = Convert.ToInt32(Request.QueryString["dataId"]);

            if (!Page.IsPostBack)
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
                BindGridIssueDetail();
                BindGridSaleOrderDetail();
                BindGridMFGDetail();
            }
        }

        protected void PrepareData()
        {
            try
            {
                SPlanetUtil.BindASPxComboBox(ref lbCustomerFirstName, DataListUtil.DropdownStoreProcedureName.Customer);
                if (dataId == 0)
                {
                    SPlanetUtil.BindASPxComboBox(ref cbbSaleOrder, DataListUtil.DropdownStoreProcedureName.Issue_Sale_Order);
                    selectedIssueDetailList = new List<IssueDetail>();
                    hdDocumentFlag.Value = string.Empty;
                }
                txtIssueDate.Value = DateTime.UtcNow.ToString("dd/MM/yyyy");
                cbbIssueFor.Items.Add(new ListItem { Value = "B", Text = "เบิกเพื่อยืม" });
                cbbIssueFor.Items.Add(new ListItem { Value = "S", Text = "เบิกขาย" });
                cbbIssueFor.Items.Add(new ListItem { Value = "R", Text = "เบิกเพื่อใช้ในงานซ่อมแซม" });
                cbbIssueFor.Items.Add(new ListItem { Value = "L", Text = "เบิกตัดชำรุด" });
                cbbIssueFor.Items.Add(new ListItem { Value = "O", Text = "เบิกอื่นๆ" });
                cbbIssueFor.Items.Add(new ListItem { Value = "W", Text = "รอระบุการเบิก" });
                //cbbIssueFor.Value = "S";

                gridViewIssue.SettingsBehavior.AllowFocusedRow = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void LoadData()
        {
            if (dataId != 0)
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(dataId) }
                        };
                    conn.Open();
                    dsIssueData = SqlHelper.ExecuteDataset(conn, "sp_issue_stock_list_data", arrParm.ToArray());
                    conn.Close();
                }
                if (dsIssueData.Tables.Count > 0)
                {
                    #region Header
                    var data = dsIssueData.Tables[0].AsEnumerable().FirstOrDefault();
                    if (data != null)
                    {
                        cbbIssueFor.Items.Clear();
                        cbbSaleOrder.ReadOnly = true;
                        cbbSaleOrder.Items.Insert(0, new ListEditItem(Convert.ToString(data["sale_order_no"]), Convert.ToString(data["sale_order_id"])));
                        txtIssueDate.Value = Convert.IsDBNull(data["issue_stock_date"]) ? string.Empty : Convert.ToString(Convert.ToDateTime(data["issue_stock_date"]).ToString("dd/MM/yyyy"));
                        txtIssueRemark.Value = Convert.IsDBNull(data["objective_other_detail"]) ? string.Empty : Convert.ToString(data["objective_other_detail"]);
                        txtRemark.Value = Convert.IsDBNull(data["remark"]) ? string.Empty : Convert.ToString(data["remark"]);
                        cbbSaleOrder.Value = Convert.IsDBNull(data["sale_order_id"]) ? string.Empty : Convert.ToString(data["sale_order_id"]);
                        //lbAddresss.Value = Convert.IsDBNull(data["customer_address"]) ? string.Empty : Convert.ToString(data["customer_address"]);
                        //lbAttention.Value = Convert.IsDBNull(data["attention_name"]) ? string.Empty : Convert.ToString(data["attention_name"]);
                        hdCustomerId.Value = Convert.IsDBNull(data["customer_id"]) ? string.Empty : Convert.ToString(data["customer_id"]);
                        lbCustomerFirstName.Value = Convert.IsDBNull(data["customer_id"]) ? string.Empty : Convert.ToString(data["customer_id"]);
                        lbCustomerFirstName.Text = Convert.IsDBNull(data["customer_name"]) ? string.Empty : Convert.ToString(data["customer_name"]);
                        lbCustomerID.Value = Convert.IsDBNull(data["customer_code"]) ? string.Empty : Convert.ToString(data["customer_code"]);
                        //lbFaxNumber.Value = Convert.IsDBNull(data["fax"]) ? string.Empty : Convert.ToString(data["fax"]);
                        lbProject.Value = Convert.IsDBNull(data["project"]) ? string.Empty : Convert.ToString(data["project"]);
                        lbSaleOrderDate.Value = Convert.IsDBNull(data["sale_order_date"]) ? string.Empty : Convert.ToString(Convert.ToDateTime(data["sale_order_date"]).ToString("dd/MM/yyyy"));
                        lbSubject.Value = Convert.IsDBNull(data["quotation_subject"]) ? string.Empty : Convert.ToString(data["quotation_subject"]);
                        //lbTelephoneNumber.Value = Convert.IsDBNull(data["attention_tel"]) ? string.Empty : Convert.ToString(data["attention_tel"]);

                        txtIssueNo.Value = Convert.IsDBNull(data["issue_stock_no"]) ? string.Empty : Convert.ToString(data["issue_stock_no"]);
                        hdCustomerId.Value = Convert.IsDBNull(data["customer_id"]) ? string.Empty : Convert.ToString(data["customer_id"]);
                        hdQuotationNo.Value = Convert.IsDBNull(data["quotation_no"]) ? string.Empty : Convert.ToString(data["quotation_no"]);
                        var issueType = Convert.IsDBNull(data["is_normal"]) ? string.Empty : (Convert.ToBoolean(data["is_normal"]) ? "1" : "0");
                        hdQuotationType.Value = Convert.IsDBNull(data["quotation_type"]) ? string.Empty : Convert.ToString(data["quotation_type"]);

                        cbbProductType.Value = Convert.IsDBNull(data["quotation_type"]) ? string.Empty : Convert.ToString(data["quotation_type"]);
                        var status = Convert.IsDBNull(data["issue_stock_status"]) ? string.Empty : Convert.ToString(data["issue_stock_status"]);
                        //cbbIssueType.Disabled = true;
                        //cbbIssueType.Attributes.Add("disabled", "true");
                        int create_by = Convert.IsDBNull(data["created_by"]) ? 0 : Convert.ToInt32(data["created_by"]);
                        cbbIssueType.Items.Clear();
                        cbbIssueType.Items.Add(new ListItem
                        {
                            Value = issueType,
                            Text = (Convert.ToBoolean(data["is_normal"]) ? "เบิกธรรมดา" : "เบิกลอย"),
                            Selected = true
                        });


                        cbbIssueType.Value = issueType;
                        if (status == "CF")
                        {
                            btnDraft.Visible = false;
                            btnSave.Visible = false;
                            btnSaleOrderDetail.Visible = false;
                            gridViewIssue.Columns[0].Visible = false;
                            Button3.Visible = false;
                            btnCancelItem.Visible = true;
                        }
                        else if (status == "CP" || status == "RE")
                        {
                            btnDraft.Visible = false;
                            btnSave.Visible = false;
                            btnSaleOrderDetail.Visible = false;
                            gridViewIssue.Columns[0].Visible = false;
                            Button3.Visible = false;
                            btnCancelItem.Visible = true;
                        }
                        else
                        {
                            btnNew.Visible = false;
                            btnCancelItem.Visible = false;
                        }
                        //if (hdQuotationType.Value == "P")
                        //{
                        //    dvProduct.Visible = true;
                        //}
                        //else
                        //{
                        //    dvProduct.Visible = false;
                        //}
                        if (cbbIssueType.Value == "1")
                        {
                            // cbbIssueFor.Items.Add(new ListItem { Value = "S", Text = "เบิกเพื่อจำหน่าย" });
                            cbbIssueFor.Items.Add(new ListItem { Value = "B", Text = "เบิกยืม" });
                            cbbIssueFor.Items.Add(new ListItem { Value = "S", Text = "เบิกขาย" });
                            cbbIssueFor.Items.Add(new ListItem { Value = "R", Text = "เบิกเพื่อใช้ในงานซ่อมแซม" });
                            cbbIssueFor.Items.Add(new ListItem { Value = "L", Text = "เบิกตัดชำรุด" });
                            cbbIssueFor.Items.Add(new ListItem { Value = "O", Text = "เบิกอื่นๆ" });
                            cbbIssueFor.Items.Add(new ListItem { Value = "W", Text = "รอระบุการเบิก" });
                            cbbIssueFor.Value = "S";
                            dvSaleOrder.Visible = true;
                            dvProduct.Visible = false;
                            dvProject2.Visible = true;
                        }
                        else
                        {
                            dvSaleOrder.Visible = false;
                            dvProduct.Visible = true;
                            dvProject2.Visible = false;
                            cbbIssueFor.Items.Add(new ListItem { Value = "B", Text = "เบิกยืม" });
                            cbbIssueFor.Items.Add(new ListItem { Value = "S", Text = "เบิกขาย" });
                            cbbIssueFor.Items.Add(new ListItem { Value = "R", Text = "เบิกเพื่อใช้ในงานซ่อมแซม" });
                            cbbIssueFor.Items.Add(new ListItem { Value = "L", Text = "เบิกตัดชำรุด" });
                            cbbIssueFor.Items.Add(new ListItem { Value = "O", Text = "เบิกอื่นๆ" });
                            cbbIssueFor.Items.Add(new ListItem { Value = "W", Text = "รอระบุการเบิก" });
                        }

                        cbbIssueFor.Value = Convert.IsDBNull(data["objective_type"]) ? string.Empty : Convert.ToString(data["objective_type"]);
                        //if (cbbIssueType.Value == "1")
                        //{
                            //gridViewIssue.Columns[1].Visible = true;
                            if (hdQuotationType.Value == "P")
                            {
                                gridViewIssue.Columns[1].Visible = true;
                            }
                            else
                            {
                                gridViewIssue.Columns[1].Visible = false;
                            }
                        /*}
                        else
                        {
                            gridViewIssue.Columns[1].Visible = false;
                        }*/

                        hdDocumentFlag.Value = status;

                        chkIssueDate.Checked = Convert.IsDBNull(data["is_issue_date"]) ? false : Convert.ToBoolean(data["is_issue_date"]);
                        if (chkIssueDate.Checked)
                        {
                            txtIssueDate.Value = string.Empty;
                            //txtIssueDate.Disabled = true;
                        }
                        else
                        {
                            //txtIssueDate.Disabled = false;
                        }

                        CheckPermission(create_by);

                    }
                    #endregion

                    #region Detail
                    var dataDetail = dsIssueData.Tables[1].AsEnumerable().ToList();
                    if (dataDetail != null)
                    {
                        issueDetailList = new List<IssueDetail>();
                        foreach (var row in dataDetail)
                        {
                            issueDetailList.Add(new IssueDetail()
                            {
                                id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                issue_stock_id = Convert.IsDBNull(row["issue_stock_id"]) ? 0 : Convert.ToInt32(row["issue_stock_id"]),
                                issue_stock_no = Convert.IsDBNull(row["issue_stock_no"]) ? string.Empty : Convert.ToString(row["issue_stock_no"]),
                                is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                product_name_tha = Convert.IsDBNull(row["product_name_tha"]) ? string.Empty : Convert.ToString(row["product_name_tha"]),
                                product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["qty"]),
                                remark = Convert.IsDBNull(row["remark"]) ? string.Empty : Convert.ToString(row["remark"]),
                                sale_order_detail_id = Convert.IsDBNull(row["sale_order_detail_id"]) ? 0 : Convert.ToInt32(row["sale_order_detail_id"]),
                                so_qty = Convert.IsDBNull(row["so_qty"]) ? 0 : Convert.ToInt32(row["so_qty"]),
                                unit_tha = Convert.IsDBNull(row["product_unit"]) ? string.Empty : Convert.ToString(row["product_unit"]),
                                old_qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["qty"]), // FOR EDIT
                                product_type = Convert.IsDBNull(row["product_type"]) ? string.Empty : Convert.ToString(row["product_type"]),
                                //quotation_no = Convert.IsDBNull(row["quotation_no"]) ? string.Empty : Convert.ToString(row["quotation_no"]),
                            });
                        }
                    }
                    #endregion

                    #region MFG Detail
                    var dataDetailMFG = dsIssueData.Tables[2].AsEnumerable().ToList();
                    if (dataDetailMFG != null)
                    {
                        issueDetailMFGList = new List<IssueDetailMFG>();
                        foreach (var row in dataDetailMFG)
                        {
                            var unit_warranty = Convert.IsDBNull(row["unitwarraty"]) ? 1 : Convert.ToInt32(row["unitwarraty"]);
                            var air_end_warranry = Convert.IsDBNull(row["airendwarraty"]) ? 1 : Convert.ToInt32(row["airendwarraty"]);

                            issueDetailMFGList.Add(new IssueDetailMFG()
                            {
                                id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                issue_stock_no = Convert.IsDBNull(row["issue_stock_no"]) ? string.Empty : Convert.ToString(row["issue_stock_no"]),
                                product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                qty = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                issue_stock_detail_id = Convert.IsDBNull(row["issue_stock_detail_id"]) ? 0 : Convert.ToInt32(row["issue_stock_detail_id"]),
                                is_deleted = Convert.IsDBNull(row["is_deleted"]) ? false : Convert.ToBoolean(row["is_deleted"]),
                                mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                service_fee = Convert.IsDBNull(row["service_fee"]) ? 0m : Convert.ToDecimal(row["service_fee"]),
                                unit_warranty = unit_warranty == 0 ? 1 : unit_warranty,
                                air_end_warranry = air_end_warranry == 0 ? 1 : air_end_warranry,

                            });
                        }
                    }
                    #endregion

                    #region Sale Order Data
                    var dsResult = new DataSet();
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@sale_order_id", SqlDbType.Int) { Value = cbbSaleOrder.Value },

                        };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_sale_order_detail_list", arrParm.ToArray());
                        conn.Close();
                    }

                    if (dsResult != null)
                    {
                        saleOrderDetailList = new List<SaleOrderDetail>();
                        var row = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                        foreach (var detail in row)
                        {
                            saleOrderDetailList.Add(new SaleOrderDetail()
                            {
                                id = Convert.ToInt32(detail["id"]),
                                product_id = Convert.IsDBNull(detail["product_id"]) ? 0 : Convert.ToInt32(detail["product_id"]),
                                saleorder_description = Convert.IsDBNull(detail["saleorder_description"]) ? string.Empty : Convert.ToString(detail["saleorder_description"]),
                                is_selected = false,
                                qty = Convert.IsDBNull(detail["qty_remain"]) ? 0 : Convert.ToInt32(detail["qty_remain"]),
                                so_qty = Convert.IsDBNull(detail["so_qty"]) ? 0 : Convert.ToInt32(detail["so_qty"]),
                                unit_price = Convert.IsDBNull(detail["unit_price"]) ? 0 : Convert.ToDecimal(detail["unit_price"]),
                                unit_code = Convert.IsDBNull(detail["unit_code"]) ? string.Empty : Convert.ToString(detail["unit_code"]),
                                total_amount = Convert.IsDBNull(detail["total_amount"]) ? 0 : Convert.ToDecimal(detail["total_amount"]),
                                sort_no = Convert.IsDBNull(detail["sort_no"]) ? 1 : Convert.ToInt32(detail["sort_no"]),
                                discount_amount = Convert.IsDBNull(detail["discount_amount"]) ? 0 : Convert.ToDecimal(detail["discount_amount"]),
                                discount_percentage = Convert.IsDBNull(detail["discount_percentage"]) ? 1 : Convert.ToDecimal(detail["discount_percentage"]),
                                discount_total = Convert.IsDBNull(detail["discount_total"]) ? 0 : Convert.ToDecimal(detail["discount_total"]),
                                discount_type = Convert.IsDBNull(detail["discount_type"]) ? string.Empty : Convert.ToString(detail["discount_type"]),
                                quotation_detail_id = Convert.IsDBNull(detail["quotation_detail_id"]) ? 0 : Convert.ToInt32(detail["quotation_detail_id"]),
                                product_no = Convert.IsDBNull(detail["product_no"]) ? string.Empty : Convert.ToString(detail["product_no"]),
                                quotation_type = Convert.IsDBNull(detail["quotation_type"]) ? string.Empty : Convert.ToString(detail["quotation_type"]),

                            });
                        }
                    }
                    #endregion

                    gridViewIssue.DataSource = issueDetailList;
                    gridViewIssue.DataBind();


                }
                cbbSaleOrder.Attributes["disabled"] = "disabled";


            }
            else
            {
                btnReportClient.Visible = false;
                btnSave.Visible = false;
                btnCancelItem.Visible = false;
                //dvProductType.Visible = false;
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
        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            return result.OfType<FilterControlColumn>();
        }

        #region Sale Order
        [WebMethod]
        public static SaleOrderData GetSaleOrderData(string id)
        {
            var dataSelected = new DataSet();
            var dataReturn = new SaleOrderData();
            var issueDetail = (List<IssueDetail>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_ISSUE"];
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID  },
                            new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(id) },
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" }
                        };
                conn.Open();
                dataSelected = SqlHelper.ExecuteDataset(conn, "sp_sale_order_header_list", arrParm.ToArray());
                conn.Close();
            }
            issueDetail = new List<IssueDetail>();
            if (dataSelected != null)
            {
                var row = (from t in dataSelected.Tables[0].AsEnumerable() select t).FirstOrDefault();

                if (row != null)
                {
                    dataReturn.id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]);
                    dataReturn.sales_order_no = Convert.IsDBNull(row["sale_order_no"]) ? string.Empty : Convert.ToString(row["sale_order_no"]);
                    dataReturn.sales_order_date = Convert.IsDBNull(row["sale_order_date"]) ? string.Empty : Convert.ToDateTime(row["sale_order_date"]).ToString("dd/MM/yyyy");
                    dataReturn.attention_name = Convert.IsDBNull(row["attention_name"]) ? string.Empty : Convert.ToString(row["attention_name"]);
                    dataReturn.project_name = Convert.IsDBNull(row["project_name"]) ? string.Empty : Convert.ToString(row["project_name"]);
                    dataReturn.quotation_subject = Convert.IsDBNull(row["quotation_subject"]) ? string.Empty : Convert.ToString(row["quotation_subject"]);
                    dataReturn.customer_code = Convert.IsDBNull(row["customer_code"]) ? string.Empty : Convert.ToString(row["customer_code"]);
                    dataReturn.fax = Convert.IsDBNull(row["fax"]) ? string.Empty : Convert.ToString(row["fax"]);
                    dataReturn.tel = Convert.IsDBNull(row["attention_tel"]) ? string.Empty : Convert.ToString(row["attention_tel"]);
                    dataReturn.quotation_type = Convert.IsDBNull(row["quotation_type"]) ? string.Empty : Convert.ToString(row["quotation_type"]);
                    dataReturn.customer_id = Convert.IsDBNull(row["customer_id"]) ? 0 : Convert.ToInt32(row["customer_id"]);
                    dataReturn.company_name_tha = Convert.IsDBNull(row["customer_name"]) ? string.Empty : Convert.ToString(row["customer_name"]);
                    dataReturn.address_bill_tha = Convert.IsDBNull(row["address_install_eng"]) ? string.Empty : Convert.ToString(row["address_install_eng"]);
                    dataReturn.quotation_no = Convert.IsDBNull(row["quotation_no"]) ? string.Empty : Convert.ToString(row["quotation_no"]);
                }
            }
            HttpContext.Current.Session["SESSION_ISSUE_DETAIL_ISSUE"] = issueDetail; // ย้ายดาต้าจริงเข้า Session จำลอง
            return dataReturn;
        }

        [WebMethod]
        public static List<SaleOrderDetail> GetSaleOrderDetailData(string id)
        {
            var saleOrderDetail = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL_ISSUE"];
            var issueDetail = (List<IssueDetail>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_ISSUE"];
            //var selectedIssueDetail = (List<IssueDetail>)HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_ISSUE"];
            var dsResult = new DataSet();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@sale_order_id", SqlDbType.Int) { Value = id },

                        };
                conn.Open();
                dsResult = SqlHelper.ExecuteDataset(conn, "sp_sale_order_detail_list", arrParm.ToArray());
                conn.Close();
            }
            //if (issueDetail == null || issueDetail.Count == 0) // โหลดใหม่เฉพาะ Issue Detail = 0 
            //{
                if (dsResult != null)
                {
                    saleOrderDetail = new List<SaleOrderDetail>();
                    var row = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                    foreach (var detail in row)
                    {
                        saleOrderDetail.Add(new SaleOrderDetail()
                        {
                            id = Convert.ToInt32(detail["id"]),
                            product_id = Convert.IsDBNull(detail["product_id"]) ? 0 : Convert.ToInt32(detail["product_id"]),
                            saleorder_description = Convert.IsDBNull(detail["saleorder_description"]) ? string.Empty : Convert.ToString(detail["saleorder_description"]),
                            is_selected = false,
                            qty = Convert.IsDBNull(detail["qty_remain"]) ? 0 : Convert.ToInt32(detail["qty_remain"]),
                            so_qty = Convert.IsDBNull(detail["so_qty"]) ? 0 : Convert.ToInt32(detail["so_qty"]),
                            unit_price = Convert.IsDBNull(detail["unit_price"]) ? 0 : Convert.ToDecimal(detail["unit_price"]),
                            unit_code = Convert.IsDBNull(detail["unit_code"]) ? string.Empty : Convert.ToString(detail["unit_code"]),
                            total_amount = Convert.IsDBNull(detail["total_amount"]) ? 0 : Convert.ToDecimal(detail["total_amount"]),
                            sort_no = Convert.IsDBNull(detail["sort_no"]) ? 1 : Convert.ToInt32(detail["sort_no"]),
                            discount_amount = Convert.IsDBNull(detail["discount_amount"]) ? 0 : Convert.ToDecimal(detail["discount_amount"]),
                            discount_percentage = Convert.IsDBNull(detail["discount_percentage"]) ? 1 : Convert.ToDecimal(detail["discount_percentage"]),
                            discount_total = Convert.IsDBNull(detail["discount_total"]) ? 0 : Convert.ToDecimal(detail["discount_total"]),
                            discount_type = Convert.IsDBNull(detail["discount_type"]) ? string.Empty : Convert.ToString(detail["discount_type"]),
                            quotation_detail_id = Convert.IsDBNull(detail["quotation_detail_id"]) ? 0 : Convert.ToInt32(detail["quotation_detail_id"]),
                            product_no = Convert.IsDBNull(detail["product_no"]) ? string.Empty : Convert.ToString(detail["product_no"]),
                            quotation_type = Convert.IsDBNull(detail["quotation_type"]) ? string.Empty : Convert.ToString(detail["quotation_type"]),
                            product_type = Convert.IsDBNull(detail["product_type"]) ? string.Empty : Convert.ToString(detail["product_type"]),
                            quotation_no = Convert.IsDBNull(detail["quotation_no"]) ? string.Empty : Convert.ToString(detail["quotation_no"]),
                        });
                    }

                }
            //}
            HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_ISSUE"] = issueDetail; // ย้ายดาต้าจริงเข้า Session จำลอง
            HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL_ISSUE"] = saleOrderDetail;
            return saleOrderDetail;
        }
        [WebMethod]
        public static string AddIssueDetail(string id, bool isSelected)
        {
            var data = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL_ISSUE"];
            var selectedIssue = (List<IssueDetail>)HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_ISSUE"];
            var selectedSaleOrderDetailRow = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault(); // เลือก Data จาก Quotation Detail
            if (data != null)
            {
                if (selectedIssue != null) // มี Issue Detail
                {
                    var rowIssueDetail = (from t in selectedIssue where t.sale_order_detail_id == Convert.ToInt32(id) select t).FirstOrDefault(); // เลือก Data จาก Sale ORder Detail

                    if (selectedSaleOrderDetailRow != null) // Sale Order ที่ Check มีค่าจริง
                    {
                        if (rowIssueDetail != null) // มีอยุ่ใน Issue Detail
                        {
                            if (isSelected) // Add
                            {
                                selectedSaleOrderDetailRow.is_selected = true;
                                if (Convert.ToInt32(rowIssueDetail.id) < 0) // กรณี Newmode
                                {
                                    selectedIssue.Add(new IssueDetail()
                                    {
                                        id = (selectedIssue.Count + 1) * -1,
                                        product_no = selectedSaleOrderDetailRow.product_no,
                                        product_id = selectedSaleOrderDetailRow.product_id,
                                        qty = selectedSaleOrderDetailRow.qty,
                                        is_delete = false,
                                        product_name_tha = selectedSaleOrderDetailRow.saleorder_description,
                                        sale_order_detail_id = Convert.ToInt32(id),
                                        remark = string.Empty,
                                        so_qty = selectedSaleOrderDetailRow.so_qty,
                                        unit_tha = selectedSaleOrderDetailRow.unit_code,
                                        product_type = selectedSaleOrderDetailRow.product_type
                                    });
                                }
                                else if (Convert.ToInt32(rowIssueDetail.id) > 0) // กรณี EditMode
                                {
                                    rowIssueDetail.is_delete = false;
                                }
                            }
                            else // Remove
                            {
                                selectedSaleOrderDetailRow.is_selected = false;
                                if (Convert.ToInt32(rowIssueDetail.id) < 0) // กรณี Newmode
                                {
                                    selectedIssue.Remove(rowIssueDetail); // ลบ Issue Detail
                                }
                                else if (Convert.ToInt32(rowIssueDetail.id) > 0) // กรณี EditMode
                                {
                                    rowIssueDetail.is_delete = false; // เปลี่ยน Flag เป็นลบ แล้ว เพิ่มกลับไปยัง Sale Order

                                    data.Add(new SaleOrderDetail()
                                    {
                                        id = rowIssueDetail.sale_order_detail_id,

                                        is_selected = false,
                                        product_id = rowIssueDetail.product_id,
                                        product_no = rowIssueDetail.product_no,
                                        qty = 1,//rowIssueDetail.so_qty,
                                        saleorder_description = rowIssueDetail.product_name_tha,
                                        unit_code = rowIssueDetail.unit_tha,
                                        product_type = rowIssueDetail.product_type

                                    });

                                }
                            }
                        }
                        else // Add Issue Detail
                        {
                            if (isSelected) // มีแค่กรณี Selected เป็น True เท่านั้น
                            {
                                selectedSaleOrderDetailRow.is_selected = true;
                                selectedIssue.Add(new IssueDetail()
                                {
                                    id = (selectedIssue.Count + 1) * -1,
                                    product_no = selectedSaleOrderDetailRow.product_no,
                                    product_id = selectedSaleOrderDetailRow.product_id,
                                    qty = (selectedSaleOrderDetailRow.quotation_detail_id == 0 ? 1 : selectedSaleOrderDetailRow.qty),
                                    is_delete = false,
                                    product_name_tha = selectedSaleOrderDetailRow.saleorder_description,
                                    sale_order_detail_id = Convert.ToInt32(id),
                                    remark = string.Empty,
                                    so_qty = selectedSaleOrderDetailRow.so_qty,
                                    unit_tha = selectedSaleOrderDetailRow.unit_code,
                                    product_type = selectedSaleOrderDetailRow.product_type
                                });
                            }
                        }
                    }
                }
                else // Add Issue Detail
                {
                    //selectedIssue = new List<IssueDetail>();
                    if (selectedIssue == null)
                    {
                        selectedIssue = new List<IssueDetail>();
                    }
                    if (selectedSaleOrderDetailRow != null)
                    {
                        if (isSelected) // มีแค่กรณี Selected เป็น True เท่านั้น
                        {
                            selectedSaleOrderDetailRow.is_selected = true;
                            selectedIssue.Add(new IssueDetail()
                            {
                                id = (selectedIssue.Count + 1) * -1,
                                product_no = selectedSaleOrderDetailRow.product_no,
                                product_id = selectedSaleOrderDetailRow.product_id,
                                qty = selectedSaleOrderDetailRow.qty,
                                is_delete = false,
                                product_name_tha = selectedSaleOrderDetailRow.saleorder_description,
                                sale_order_detail_id = Convert.ToInt32(id),
                                remark = string.Empty,
                                so_qty = selectedSaleOrderDetailRow.so_qty,
                                unit_tha = selectedSaleOrderDetailRow.unit_code,
                                product_type = selectedSaleOrderDetailRow.product_type
                            });
                        }
                    }
                }
            }

            HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL_ISSUE"] = data; // ยัดค่ากลับ Session SaleOrder Detail Data
            HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_ISSUE"] = selectedIssue; // ยัดค่ากลับ Session Seltected Issue Order detail data
            return "SUCCESS";
        }
        [WebMethod]
        public static string SubmitSaleOrderDetail()
        {
            var issueDetail = (List<IssueDetail>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_ISSUE"];
            var selectedIssue = (List<IssueDetail>)HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_ISSUE"];
            var saleOrderDetail = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL_ISSUE"];
            if (selectedIssue != null)
            {
                foreach (var selectedRow in selectedIssue)
                {
                    var existItem = (from t in issueDetail where t.product_id == selectedRow.product_id select t).FirstOrDefault();
                    if (existItem == null)
                    {
                        if (selectedRow.is_delete == false)
                        {
                            issueDetail.Add(selectedRow);
                        }
                        else
                        {
                            existItem.is_delete = true;
                        }
                    }
                    else
                    {
                        if (selectedRow.id < 0)
                        {
                            if (selectedRow.is_delete)
                            {
                                existItem.is_delete = true;
                            }
                            else
                            {
                                existItem.is_delete = false;
                            }
                        }
                        else
                        {
                            if (selectedRow.is_delete)
                            {
                                existItem.is_delete = true;
                            }
                            else
                            {
                                existItem.is_delete = false;
                            }

                            var rowSaleOrder = (from t in saleOrderDetail where t.id == selectedRow.sale_order_detail_id select t).FirstOrDefault();
                            if (rowSaleOrder != null)
                            {
                                if (rowSaleOrder.is_selected)
                                {
                                    saleOrderDetail.Remove(rowSaleOrder);
                                }
                            }
                        }
                    }
                }
            }

            HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL"] = issueDetail;
            HttpContext.Current.Session["SESSION_SELECTED_SALE_ORDER_DETAIL"] = null;
            HttpContext.Current.Session["SESSION_QUATATION_DETAIL_SALE_ORDER"] = saleOrderDetail;
            return "SUCCESS";
        }
        protected void gridViewSaleOrderDetail_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var splitStr = e.Parameters.ToString().Split('|');
            string searchText = string.Empty;
            string issueType = string.Empty;

            if (splitStr.Length > 1)
            {
                searchText = splitStr[2];
            }

            if (cbbIssueType.Value == "0")
            {
                gridViewSaleOrderDetail.Columns[6].Visible = false;
            }
            else
            {
                gridViewSaleOrderDetail.Columns[6].Visible = true;
            }

            if (string.IsNullOrEmpty(searchText))
            {
                gridViewSaleOrderDetail.DataSource = saleOrderDetailList;
                gridViewSaleOrderDetail.DataBind();
            }
            else
            {
                if (cbbProductType.Value == "S" && cbbIssueType.Value == "0")
                {
                    gridViewSaleOrderDetail.DataSource = saleOrderDetailList.Where(t => t.saleorder_description.ToUpper().Contains(searchText.ToUpper())
                                                            || t.product_no.ToUpper().Contains(searchText.ToUpper())
                                                            || t.quotation_no.ToUpper().Contains(searchText.Trim().ToUpper())).ToList();
                    gridViewSaleOrderDetail.DataBind();
                }
                else
                {
                    gridViewSaleOrderDetail.DataSource = saleOrderDetailList.Where(t => t.saleorder_description.ToUpper().Contains(searchText.ToUpper()) || t.product_no.ToUpper().Contains(searchText.ToUpper())).ToList();
                    gridViewSaleOrderDetail.DataBind();
                }
            }
            gridViewSaleOrderDetail.PageIndex = 0;
        }

        protected void gridViewSaleOrderDetail_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxCheckBox checkBox = gridViewSaleOrderDetail.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkSaleOrderDetail") as ASPxCheckBox;
                if (checkBox != null)
                {
                    if (e.DataColumn.FieldName == "is_selected")
                    {
                        if (saleOrderDetailList != null)
                        {
                            var row = (from t in saleOrderDetailList where t.id == Convert.ToInt32(e.KeyValue) select t).FirstOrDefault();
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
        #endregion

        #region Issue Detail
        [WebMethod]
        public static IssueDetail EditIssueDetail(string id)
        {
            var issueDetail = (List<IssueDetail>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_ISSUE"];
            var selectedIssue = new IssueDetail(); // new เสมอ เพราะเราต้องการ record ล่าสุดที่เลือก
            if (issueDetail != null)
            {
                var row = (from t in issueDetail where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (row != null)
                {
                    selectedIssue = row;
                }
            }
            // 
            return selectedIssue;
        }
        [WebMethod]
        public static List<IssueDetail> DeleteIssueDetail(string id)
        {
            var issueDetail = (List<IssueDetail>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_ISSUE"];
            var saleOrderData = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL_ISSUE"];
            var issueMFGDetail = (List<IssueDetailMFG>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ISSUE"];
            var rowDeletedIssue = (from t in issueDetail where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
            var rowSaleOrderDetail = (from t in saleOrderData where t.id == rowDeletedIssue.sale_order_detail_id select t).FirstOrDefault();
            if (Convert.ToInt32(id) < 0)
            {
                issueDetail.Remove(rowDeletedIssue);
                rowSaleOrderDetail.is_selected = false;
                if (issueMFGDetail != null) // clear mfg
                {
                    foreach (var row in (from t in issueMFGDetail where t.issue_stock_detail_id == Convert.ToInt32(id) select t).ToList())
                    {
                        issueMFGDetail.Remove(row);
                    }
                }
            }
            else
            {
                rowDeletedIssue.is_delete = true;
                saleOrderData.Add(new SaleOrderDetail()
                {
                    id = rowDeletedIssue.sale_order_detail_id,
                    is_selected = false,
                    product_id = rowDeletedIssue.product_id,
                    product_no = rowDeletedIssue.product_no,
                    qty = rowDeletedIssue.so_qty,
                    saleorder_description = rowDeletedIssue.product_name_tha,
                    unit_code = rowDeletedIssue.unit_tha,
                    product_type = rowDeletedIssue.product_type

                });
                if (issueMFGDetail != null) // clear mfg
                {
                    foreach (var row in (from t in issueMFGDetail where t.issue_stock_detail_id == Convert.ToInt32(id) select t).ToList())
                    {
                        row.is_deleted = true;
                    }
                }
            }
            HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ISSUE"] = issueMFGDetail;
            HttpContext.Current.Session["SESSION_ISSUE_DETAIL_ISSUE"] = issueDetail;
            HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL_ISSUE"] = saleOrderData;
            return issueDetail;
        }
        [WebMethod]
        public static List<IssueDetail> SubmitIssueDetail(string id, string qty, string remark)
        {
            List<IssueDetail> data = (List<IssueDetail>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_ISSUE"];

            if (data != null)
            {
                var row = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (row != null)
                {
                    row.qty = Convert.ToInt32(qty);
                    row.remark = remark;
                }
            }


            HttpContext.Current.Session["SESSION_ISSUE_DETAIL_ISSUE"] = data;
            HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_ISSUE"] = null;
            return data;
        }
        protected void gridViewIssue_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
               
                if (cbbIssueType.Value == "1")
                {
                    //gridViewIssue.Columns[1].Visible = true;
                    if (hdQuotationType.Value == "P")
                    {
                        gridViewIssue.Columns[1].Visible = true;
                    }
                    else
                    {
                        gridViewIssue.Columns[1].Visible = false;
                    }
                }
                else
                {
                    if (cbbProductType.Value == "P")
                    {
                        gridViewIssue.Columns[1].Visible = true;
                    }
                    else
                    {
                        gridViewIssue.Columns[1].Visible = false;
                    }
                }
                gridViewIssue.DataSource = (from t in issueDetailList where !t.is_delete select t).ToList();
                gridViewIssue.FilterExpression = FilterBag.GetExpression(false);
                gridViewIssue.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        protected void BindGridIssueDetail()
        {
            try
            {
                if (cbbIssueType.Value == "1")
                {
                    //gridViewIssue.Columns[1].Visible = true;
                    if (hdQuotationType.Value == "P")
                    {
                        gridViewIssue.Columns[1].Visible = true;
                    }
                    else
                    {
                        gridViewIssue.Columns[1].Visible = false;
                    }
                }
                else
                {
                    gridViewIssue.Columns[1].Visible = false;
                }
                gridViewIssue.DataSource = (from t in issueDetailList where !t.is_delete select t).ToList();
                gridViewIssue.FilterExpression = FilterBag.GetExpression(false);
                gridViewIssue.DataBind();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        protected void BindGridSaleOrderDetail()
        {
            try
            {
                string searchText = txtSearchText.Value;
                if (string.IsNullOrEmpty(searchText))
                {
                    gridViewSaleOrderDetail.DataSource = saleOrderDetailList;
                    gridViewSaleOrderDetail.DataBind();
                }
                else
                {
                    if (cbbProductType.Value == "S" && cbbIssueType.Value == "0")
                    {
                        gridViewSaleOrderDetail.DataSource = saleOrderDetailList.Where(t => t.saleorder_description.ToUpper().Contains(searchText.ToUpper())
                                                                || t.product_no.ToUpper().Contains(searchText.ToUpper())
                                                                || t.quotation_no.ToUpper().Contains(searchText.Trim().ToUpper())).ToList();
                        gridViewSaleOrderDetail.DataBind();
                    }
                    else
                    {
                        gridViewSaleOrderDetail.DataSource = saleOrderDetailList.Where(t => t.saleorder_description.ToUpper().Contains(searchText.ToUpper()) || t.product_no.ToUpper().Contains(searchText.ToUpper())).ToList();
                        gridViewSaleOrderDetail.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        #endregion

        #region Issue MFG
        [WebMethod]
        public static IssueDetail GetMFGDetail(string id, string issue_no)
        {
            var mfgDetailData = (List<MFGDetail>)HttpContext.Current.Session["SESSION_MFG_DETAIL_ISSUE"];
            var issueMFGDetail = (List<IssueDetailMFG>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ISSUE"];
            var issueDetail = (List<IssueDetail>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_ISSUE"];
            var dsResult = new DataSet();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@product_id", SqlDbType.Int) { Value = id },

                        };
                conn.Open();
                dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_mfg_list", arrParm.ToArray());
                conn.Close();
            }
            //if (mfgDetailData == null || mfgDetailData.Count == 0)
            //{
                mfgDetailData = new List<MFGDetail>();
                if (dsResult != null)
                {
                    foreach (var row in (from t in dsResult.Tables[0].AsEnumerable()
                                         where t.Field<string>("issue_no") == "" || t.Field<string>("issue_no") == issue_no
                                         select t).ToList())
                    {
                        mfgDetailData.Add(new MFGDetail()
                        {
                            is_selected = false,
                            mfg_no = Convert.ToString(row["mfg_no"]),
                            air_end_warranty = Convert.IsDBNull(row["airendwarraty"]) ? 0 : Convert.ToInt32(row["airendwarraty"]),
                            unit_warranty = Convert.IsDBNull(row["unitwarraty"]) ? 1 : Convert.ToInt32(row["unitwarraty"]),
                            service_fee = Convert.IsDBNull(row["service_fee"]) ? 0m : Convert.ToDecimal(row["service_fee"]),
                            pr_no = Convert.ToString(row["pr_no"]),
                            receive_no = Convert.ToString(row["receive_no"]),
                        });
                    }
                }
                if (issueMFGDetail != null)
                {
                    foreach (var row in (from t in issueMFGDetail
                                         where t.product_id == Convert.ToInt32(id) && !t.is_deleted
                                         select t).ToList())
                    {
                        if (row.id > 0)
                        {
                            var checkExist = (from t in mfgDetailData where t.mfg_no == row.mfg_no select t).FirstOrDefault();
                            if (checkExist == null)
                            {
                                mfgDetailData.Add(new MFGDetail()
                                {
                                    is_selected = true,
                                    mfg_no = row.mfg_no,
                                    air_end_warranty = 0,
                                    service_fee = row.service_fee,
                                    unit_warranty = 1,//row.unit_warranty
                                    pr_no = row.pr_no,
                                    receive_no = row.receive_no
                                });
                            }
                            else
                            {
                                checkExist.is_selected = true;
                                checkExist.air_end_warranty = 0; //row.air_end_warranry;
                                checkExist.service_fee = row.service_fee;
                                checkExist.unit_warranty = 1;///row.unit_warranty;
                            }
                        }
                        else
                        {
                            var checkExist = (from t in mfgDetailData where t.mfg_no == row.mfg_no select t).FirstOrDefault();
                            if (checkExist != null)
                            {
                                checkExist.is_selected = true;
                                checkExist.air_end_warranty = 0;//row.air_end_warranry;
                                checkExist.service_fee = row.service_fee;
                                checkExist.unit_warranty = 1;//row.unit_warranty;
                            }
                        }
                    }
                }
                HttpContext.Current.Session["SESSION_MFG_DETAIL_ISSUE"] = mfgDetailData;
            //}
            var productDetail = (from t in issueDetail
                                 where t.product_id == Convert.ToInt32(id) && !t.is_delete
                                 select t).FirstOrDefault();
            HttpContext.Current.Session["SESSION_MFG_DETAIL_ISSUE_TEMP"] = mfgDetailData;
            HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ISSUE"] = issueMFGDetail;
            HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_MFG_ISSUE"] = issueMFGDetail;
            return productDetail;
        }

        [WebMethod]
        public static List<IssueDetailMFG> AddMFGProduct(string mfg_no, bool isSelected, string product_id, string issue_detail_id)
        {
            var data = (List<MFGDetail>)HttpContext.Current.Session["SESSION_MFG_DETAIL_ISSUE_TEMP"];
            var selectedisssueMFGDetail = (List<IssueDetailMFG>)HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_MFG_ISSUE"];

            if (data != null)
            {
                var selectedRow = (from t in data
                                   where t.mfg_no == mfg_no
                                   select t).FirstOrDefault();
                if (selectedRow != null)
                {
                    if (selectedisssueMFGDetail != null) // List Issue MFG Detail มีค่า
                    {
                        var checkExist = (from t in selectedisssueMFGDetail where t.mfg_no == mfg_no select t).FirstOrDefault();
                        if (checkExist != null) // เจอว่าที่เลือกแล้วมีอยุ่ใน Dataset
                        {
                            if (isSelected) // checked
                            {
                                selectedRow.is_selected = true;
                                if (checkExist.id > 0)
                                {
                                    checkExist.is_deleted = false;
                                }
                            }
                            else
                            {
                                selectedRow.is_selected = false;
                                selectedRow.unit_warranty = 1;
                                selectedRow.air_end_warranty = 1;
                                selectedRow.service_fee = 0;

                                if (checkExist.id < 0)
                                {
                                    selectedisssueMFGDetail.Remove(checkExist);
                                }
                                else
                                {
                                    checkExist.is_deleted = true;
                                    /*checkExist.unit_warranty = 1;
                                    checkExist.air_end_warranry = 1;
                                    checkExist.service_fee = 0;*/
                                }
                            }
                        }
                        else // ไม่เจอ เพิ่มใหม่เท่านั้น
                        {
                            selectedRow.is_selected = true;
                            selectedisssueMFGDetail.Add(new IssueDetailMFG()
                            {
                                id = (selectedisssueMFGDetail.Count + 1) * -1,
                                product_id = Convert.ToInt32(product_id),
                                mfg_no = mfg_no,
                                issue_stock_detail_id = Convert.ToInt32(issue_detail_id),
                                is_deleted = false,
                                qty = 1,
                                air_end_warranry = 1,
                                unit_warranty = 1
                            });
                        }
                    }
                    else // Add New เท่านั้น
                    {
                        selectedisssueMFGDetail = new List<IssueDetailMFG>();
                        selectedRow.is_selected = true;
                        selectedisssueMFGDetail.Add(new IssueDetailMFG()
                        {
                            id = (selectedisssueMFGDetail.Count + 1) * -1,
                            product_id = Convert.ToInt32(product_id),
                            mfg_no = mfg_no,
                            issue_stock_detail_id = Convert.ToInt32(issue_detail_id),
                            is_deleted = false,
                            qty = 1,
                            unit_warranty = 1,
                            air_end_warranry = 1
                        });
                    }
                }
            }
            HttpContext.Current.Session["SESSION_MFG_DETAIL_ISSUE_TEMP"] = data;
            HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_MFG_ISSUE"] = selectedisssueMFGDetail;
            return selectedisssueMFGDetail;

        }

        [WebMethod]
        public static string TextChangedMFGProduct(string mfg_no, string value, string type)
        {
            var data = (List<MFGDetail>)HttpContext.Current.Session["SESSION_MFG_DETAIL_ISSUE"];
            var selectedisssueMFGDetail = (List<IssueDetailMFG>)HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_MFG_ISSUE"];
            if (data != null)
            {
                var selectedRow = (from t in data
                                   where t.mfg_no == mfg_no
                                   select t).FirstOrDefault();
                var rowSelected = (from t in selectedisssueMFGDetail where t.mfg_no == mfg_no select t).FirstOrDefault();
                if (selectedRow != null)
                {
                    if (selectedRow.is_selected)
                    {
                        if (type == "unit")
                        {
                            selectedRow.unit_warranty = Convert.ToInt32(value);
                        }
                        else if (type == "air")
                        {
                            selectedRow.air_end_warranty = Convert.ToInt32(value);
                        }
                        else if (type == "fee")
                        {
                            selectedRow.service_fee = Math.Round(Convert.ToDecimal(value), 2);
                        }

                        if (rowSelected != null)
                        {
                            rowSelected.unit_warranty = selectedRow.unit_warranty;
                            rowSelected.air_end_warranry = selectedRow.air_end_warranty;
                            rowSelected.service_fee = selectedRow.service_fee;
                        }
                    }
                    else
                    {
                        selectedRow.unit_warranty = 1;
                        selectedRow.air_end_warranty = 1;
                        selectedRow.service_fee = 0;
                        if (rowSelected != null)
                        {
                            rowSelected.unit_warranty = 1;
                            rowSelected.air_end_warranry = 1;
                            rowSelected.service_fee = 0;
                        }
                    }

                }
            }
            HttpContext.Current.Session["SESSION_MFG_DETAIL_ISSUE"] = data;
            HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_MFG_ISSUE"] = selectedisssueMFGDetail;
            return "SUCESS";
        }

        [WebMethod]
        public static string SubmitMFGProduct(int issue_detail_id)
        {
            string returnMessage = string.Empty;
            var issueMFGDetail = (List<IssueDetailMFG>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ISSUE"];
            var selectedissueMFGDetail = (List<IssueDetailMFG>)HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_MFG_ISSUE"];
            var issueDetail = (List<IssueDetail>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_ISSUE"];
            var mFGDetails = (List<MFGDetail>)HttpContext.Current.Session["SESSION_MFG_DETAIL_ISSUE_TEMP"];
            if (selectedissueMFGDetail != null)
            {
                foreach (var row in selectedissueMFGDetail.Where(t => !t.is_deleted && t.issue_stock_detail_id == issue_detail_id))
                {
                    if (row.unit_warranty == 0)
                    {
                        returnMessage += "กรุณากรอก Unit Warranty MFG : " + row.mfg_no + "<br>";
                    }
                    if (row.air_end_warranry == 0)
                    {
                        returnMessage += "กรุณากรอก Air End Warranty MFG : " + row.mfg_no + "<br>";
                    }
                }
            }
            var checkItem = (from t in issueDetail where t.id == issue_detail_id && !t.is_delete select t).FirstOrDefault();
            if (checkItem != null)
            {
                //if (checkItem.qty != selectedissueMFGDetail.Where(t => !t.is_deleted && t.issue_stock_detail_id == issue_detail_id).Count())
                if (checkItem.qty != mFGDetails.Where(t => t.is_selected).Count())
                {
                    //returnMessage += "สินค้า " + checkItem.product_no + " กรุณาเลือก MFG ให้ตรงตามจำนวน ( Qty = " + checkItem.qty + ": MFG = " + selectedissueMFGDetail.Where(t => !t.is_deleted && t.issue_stock_detail_id == issue_detail_id).Count() + " )\n";
                    returnMessage += "สินค้า " + checkItem.product_no + " จำนวน MFG (" + mFGDetails.Where(t => t.is_selected).Count() + ") ไม่ตรงกับจำนวนที่รับทั้งหมด (" + checkItem.qty + ")\n";
                }
            }
            if (string.IsNullOrEmpty(returnMessage))
            {
                if (selectedissueMFGDetail != null)
                {
                    issueMFGDetail = selectedissueMFGDetail;
                }

                HttpContext.Current.Session["SESSION_MFG_DETAIL_ISSUE"] = mFGDetails;
                HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ISSUE"] = issueMFGDetail;
                HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_MFG_ISSUE"] = null;
            }
            return returnMessage;
        }
        protected void gridViewIssueMFG_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridViewIssueMFG.DataSource = (from t in mfgDetailListTemp select t).ToList(); ;
            gridViewIssueMFG.DataBind();
        }

        protected void BindGridMFGDetail()
        {
            gridViewIssueMFG.DataSource = (from t in mfgDetailListTemp select t).ToList();
            gridViewIssueMFG.DataBind();
        }

        protected void gridViewIssueMFG_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxCheckBox checkBox = gridViewIssueMFG.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkProductMFG") as ASPxCheckBox;
                if (checkBox != null)
                {
                    if (e.DataColumn.FieldName == "is_selected")
                    {
                        if (mfgDetailListTemp != null)
                        {
                            var row = (from t in mfgDetailListTemp where t.mfg_no == Convert.ToString(e.KeyValue) select t).FirstOrDefault();
                            //var row = (from t in selectedIssueDetailMFGList where t.mfg_no == Convert.ToString(e.KeyValue) && !t.is_deleted select t).FirstOrDefault();
                            if (row != null)
                            {
                                if (row.is_selected)
                                {
                                    checkBox.Checked = true;
                                }
                                else
                                {
                                    checkBox.Checked = false;
                                }
                            }

                            if (hdDocumentFlag.Value == "CF" || hdDocumentFlag.Value == "CP")
                            {
                                checkBox.Enabled = false;
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
        #endregion 

        [WebMethod]
        public static string ValidateData(string type, string issue_type,bool chkIssue , string issue_date, string next_status, string status)
        {
            string msg = string.Empty;
            var issueMFGDetail = (List<IssueDetailMFG>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ISSUE"];
            var issueDetail = (List<IssueDetail>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_ISSUE"];

            if(!chkIssue)
            {
                if(string.IsNullOrEmpty(issue_date))
                {
                    msg += "กรุณาเลือกวันที่ \n";
                }
            }

            if ((issueDetail == null || issueDetail.Count == 0) && issue_type == "1")
            {

                msg += "กรุณาเลือกรายการ Issue อย่างน้อย 1 รายการ \n";

            }
            else
            {
                //  No need to select MFG
                /*if (issue_type == "1")
                {
                    foreach (var row in issueDetail.Where(t => !t.is_delete).ToList())
                    {
                        if (issueMFGDetail != null)
                        {
                            if (issueMFGDetail.Where(t => !t.is_deleted).ToList().Count > 0)
                            {
                                var mfgSelectedData = (from t in issueMFGDetail where t.issue_stock_detail_id == row.id && !t.is_deleted select t).ToList();
                                if (mfgSelectedData != null)
                                {
                                    if (row.qty != mfgSelectedData.Count)
                                    {
                                        msg += "" + row.product_no + " กรุณาเลือก MFG ให้ตรงตามจำนวน ( Qty = " + row.qty + ": MFG = " + mfgSelectedData.Count + " )\n";
                                    }
                                }
                                else
                                {
                                    msg += "" + row.product_no + " กรุณาเลือก MFG ให้ตรงตามจำนวน ( Qty = " + row.qty + ": MFG = " + mfgSelectedData.Count + " )\n";
                                }
                            }
                            else
                            {
                                if (type == "P")
                                {
                                    msg += "กรุณาเลือก MFG ของ " + row.product_no + "\n";
                                }
                            }
                        }
                        else
                        {
                            if (type == "P")
                            {
                                msg += "กรุณาเลือก MFG ของ " + row.product_no + "\n";
                            }
                        }
                    }
                }*/

                //  Check only type product
                if (type == "P")
                {
                    foreach (var row in issueDetail.Where(t => !t.is_delete).ToList())
                    {
                        if (issueMFGDetail != null)
                        {
                            if (issueMFGDetail.Where(t => !t.is_deleted).ToList().Count > 0)
                            {
                                var mfgSelectedData = (from t in issueMFGDetail where t.issue_stock_detail_id == row.id && !t.is_deleted select t).ToList();
                                if (mfgSelectedData != null)
                                {
                                    if (row.qty != mfgSelectedData.Count)
                                    {
                                        //msg += "" + row.product_no + " กรุณาเลือก MFG ให้ตรงตามจำนวน ( Qty = " + row.qty + ": MFG = " + mfgSelectedData.Count + " )\n";
                                        msg += "สินค้า " + row.product_no + " จำนวน MFG (" + mfgSelectedData.Count + ") ไม่ตรงกับจำนวนที่รับทั้งหมด (" + row.qty + ")\n";
                                    }
                                }
                                else
                                {
                                    //msg += "" + row.product_no + " กรุณาเลือก MFG ให้ตรงตามจำนวน ( Qty = " + row.qty + ": MFG = " + mfgSelectedData.Count + " )\n";
                                    msg += "สินค้า " + row.product_no + " จำนวน MFG (" + mfgSelectedData.Count + ") ไม่ตรงกับจำนวนที่รับทั้งหมด (" + row.qty + ")\n";
                                }
                            }
                            else
                            {
                                if (type == "P")
                                {
                                    msg += "กรุณาเลือก MFG ของ " + row.product_no + "\n";
                                }
                            }
                        }
                        else
                        {
                            if (type == "P")
                            {
                                msg += "กรุณาเลือก MFG ของ " + row.product_no + "\n";
                            }
                        }
                    }
                }
            }

            //  Check qty if next status is FL, CF
            if (msg != "")
            {
                return msg;
            }

            /*if (status == "CF")
            {
                return msg;
            }*/

            var issueDetailList = (List<IssueDetail>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_ISSUE"];
            foreach (var row in issueDetailList.Where(t => !t.is_delete).ToList())
            {
                if (row.product_type == "P")
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

                        if (dsResult != null)
                        {
                            var productRow = (from t in dsResult.Tables[0].AsEnumerable() select t).FirstOrDefault();
                            if (productRow != null)
                            {
                                var tempDiffQty = (row.qty - Convert.ToInt32(productRow["quantity_balance"]));
                                if (tempDiffQty > 0)
                                {
                                    msg += "" + row.product_no + " จำนวนไม่เพียงพอ \n";
                                }
                            }
                        }
                    }
                }
                else if (row.product_type == "S")
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


                        if (dsResult != null)
                        {
                            var productRow = (from t in dsResult.Tables[0].AsEnumerable() select t).FirstOrDefault();
                            if (productRow != null)
                            {
                                var tempDiffQty = (row.qty - Convert.ToInt32(productRow["quantity_balance"]));
                                if (tempDiffQty > 0)
                                {
                                    msg += "" + row.product_no + " จำนวนไม่เพียงพอ \n";
                                }

                            }
                        }
                    }
                }
            }

            return msg;
        }
        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            SaveData("FL");
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            SaveData("CF");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            SaveData("CC");
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

                        var issueNo = string.Empty;
                        if (dataId == 0) // INSERT MODE
                        {
                            #region Header
                            using (SqlCommand cmd = new SqlCommand("sp_issue_stock_header_add", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@issue_stock_status", SqlDbType.VarChar, 2).Value = status;//FL = Follow , CF = Confirm

                                cmd.Parameters.Add("@sale_order_no", SqlDbType.VarChar, 20).Value = cbbSaleOrder.Text;
                                cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = hdQuotationNo.Value;
                                cmd.Parameters.Add("@quotation_type", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(hdQuotationType.Value) ? cbbProductType.Value : hdQuotationType.Value;
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = string.IsNullOrEmpty(hdCustomerId.Value) ? 0 : Convert.ToInt32(hdCustomerId.Value);
                                cmd.Parameters.Add("@customer_name", SqlDbType.VarChar, 200).Value = lbCustomerFirstName.Text;
                                cmd.Parameters.Add("@objective_type", SqlDbType.VarChar, 1).Value = cbbIssueFor.Value;
                                cmd.Parameters.Add("@objective_other_detail", SqlDbType.VarChar, 100).Value = cbbIssueFor.Value == "5" ? txtIssueRemark.Value : string.Empty;
                                cmd.Parameters.Add("@request_by", SqlDbType.Int).Value = 1;
                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = txtRemark.Value;
                                cmd.Parameters.Add("@project", SqlDbType.VarChar, 200).Value = lbProject.Value;
                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                cmd.Parameters.Add("@is_normal", SqlDbType.Bit).Value = cbbIssueType.Value == "1" ? true : false;
                                cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 2).Value = Convert.ToString(cbbProductType.Value);
                                cmd.Parameters.Add("@is_issue_date", SqlDbType.Bit).Value = chkIssueDate.Checked ? true : false;


                                newID = Convert.ToInt32(cmd.ExecuteScalar());

                            }
                            #endregion

                            //Create array of Parameters

                            List<SqlParameter> arrParm = new List<SqlParameter>
                            {
                                new SqlParameter("@id", SqlDbType.Int) { Value = newID },
                            };

                            var lastDataInsert = SqlHelper.ExecuteDataset(tran, "sp_issue_stock_header_list", arrParm.ToArray());


                            if (lastDataInsert != null)
                            {
                                issueNo = (from t in lastDataInsert.Tables[0].AsEnumerable()
                                           select t.Field<string>("issue_stock_no")).FirstOrDefault();
                            }
                            var newDetailId = 0;
                            var oldDetailId = 0;

                            #region Detail
                            foreach (var row in issueDetailList)
                            {
                                oldDetailId = row.id;

                                using (SqlCommand cmd = new SqlCommand("sp_issue_stock_detail_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@issue_stock_id", SqlDbType.Int).Value = newID;
                                    cmd.Parameters.Add("@issue_stock_no", SqlDbType.VarChar, 20).Value = issueNo;
                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                    cmd.Parameters.Add("@product_name_tha", SqlDbType.VarChar, 200).Value = row.product_name_tha;
                                    cmd.Parameters.Add("@product_unit", SqlDbType.VarChar, 5).Value = row.unit_tha;
                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                    cmd.Parameters.Add("@so_qty", SqlDbType.Int).Value = row.so_qty;
                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = row.remark;
                                    cmd.Parameters.Add("@sale_order_detail_id", SqlDbType.Int).Value = (cbbIssueType.Value == "1" ? row.sale_order_detail_id : 0);
                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 2).Value = row.product_type;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                    newDetailId = Convert.ToInt32(cmd.ExecuteScalar());

                                }

                                var childData = (from t in issueDetailMFGList where t.issue_stock_detail_id == oldDetailId select t).ToList();
                                foreach (var rowDetail in childData)
                                {
                                    rowDetail.issue_stock_detail_id = newDetailId;
                                }
                            }
                            #endregion

                            #region Detail MFG
                            if ((cbbIssueType.Value == "1" && hdQuotationType.Value == "P") || (cbbIssueType.Value == "0" && hdQuotationType.Value == "P"))
                            {
                                foreach (var row in issueDetailMFGList)
                                {

                                    using (SqlCommand cmd = new SqlCommand("sp_issue_stock_detail_mfg_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@issue_stock_no", SqlDbType.VarChar, 20).Value = issueNo;
                                        cmd.Parameters.Add("@issue_stock_detail_id", SqlDbType.Int).Value = row.issue_stock_detail_id;
                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 100).Value = row.mfg_no.Trim();
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 1;// row.qty;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
                                    using (SqlCommand cmd = new SqlCommand("sp_customer_mfg_issue_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToInt32(hdCustomerId.Value);
                                        cmd.Parameters.Add("@model", SqlDbType.VarChar, 50).Value = string.Empty;
                                        cmd.Parameters.Add("@mfg", SqlDbType.VarChar, 100).Value = row.mfg_no.Trim();
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        cmd.Parameters.Add("@unitwarraty", SqlDbType.Int).Value = row.unit_warranty;// row.qty;
                                        cmd.Parameters.Add("@airendwarraty", SqlDbType.Int).Value = row.air_end_warranry;// row.qty;
                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;// row.qty;
                                        cmd.Parameters.Add("@service_fee", SqlDbType.Int).Value = row.service_fee;// row.qty;
                                        cmd.Parameters.Add("@project", SqlDbType.VarChar, 200).Value = lbSubject.Value;

                                        cmd.ExecuteNonQuery();

                                    }

                                }
                            }
                            #endregion

                            #region SaleOrder Update Status

                            /*using (SqlCommand cmd = new SqlCommand("sp_sale_order_update_status", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@sale_order_no", SqlDbType.VarChar, 20).Value = cbbSaleOrder.Text;

                                cmd.ExecuteNonQuery();

                            }*/

                            #endregion

                            #region Transection
                            if (status == "CF")
                            {
                                var transNo = string.Empty;
                                using (SqlCommand cmd = new SqlCommand("sp_gen_transection_no", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    transNo = Convert.ToString(cmd.ExecuteScalar());

                                }
                                string stock_type = string.Empty;
                                foreach (var row in issueDetailList)
                                {

                                    using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                        cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.product_type;
                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;

                                        cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(hdQuotationNo.Value) ? "ISSUE_O" : "ISSUE";
                                        cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "OUT";
                                        cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                        cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = row.remark;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                        cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = issueNo;
                                        cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = string.IsNullOrEmpty(hdCustomerId.Value) ? 0 : Convert.ToInt32(hdCustomerId.Value);
                                        cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                        cmd.ExecuteNonQuery();

                                    }
                                }
                            }
                            #endregion
                        }

                        else
                        {
                            newID = dataId;

                            #region Header
                            using (SqlCommand cmd = new SqlCommand("sp_issue_stock_header_edit", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = dataId;
                                cmd.Parameters.Add("@issue_stock_status", SqlDbType.VarChar, 2).Value = status;//FL = Follow , CF = Confirm
                                cmd.Parameters.Add("@sale_order_no", SqlDbType.VarChar, 20).Value = cbbSaleOrder.Text;
                                cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = hdQuotationNo.Value;
                                cmd.Parameters.Add("@quotation_type", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(hdQuotationType.Value) ? cbbProductType.Value : hdQuotationType.Value;
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = hdCustomerId.Value;
                                cmd.Parameters.Add("@objective_type", SqlDbType.VarChar, 1).Value = cbbIssueFor.Value;
                                cmd.Parameters.Add("@objective_other_detail", SqlDbType.VarChar, 100).Value = cbbIssueFor.Value == "5" ? txtIssueRemark.Value : string.Empty;
                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = txtRemark.Value;
                                cmd.Parameters.Add("@project", SqlDbType.VarChar, 200).Value = lbProject.Value;
                                cmd.Parameters.Add("@request_by", SqlDbType.Int).Value = 1;
                                cmd.Parameters.Add("@receive_by", SqlDbType.Int).Value = 1;
                                cmd.Parameters.Add("@audit_by", SqlDbType.Int).Value = 1;
                                cmd.Parameters.Add("@accounting_by", SqlDbType.Int).Value = 1;
                                cmd.Parameters.Add("@is_cancel", SqlDbType.Bit).Value = 0;
                                cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = 0;

                                cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                cmd.Parameters.Add("@is_issue_date", SqlDbType.Bit).Value = chkIssueDate.Checked ? true : false;
                                cmd.Parameters.Add("@issue_date", SqlDbType.DateTime).Value = !chkIssueDate.Checked ? DateTime.ParseExact(txtIssueDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.UtcNow;


                                cmd.ExecuteNonQuery();

                            }
                            #endregion

                            var newDetailId = 0;
                            var oldDetailId = 0;

                            #region Detail
                            foreach (var row in issueDetailList)
                            {
                                oldDetailId = row.id;

                                if (row.id < 0) // add
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_issue_stock_detail_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@issue_stock_id", SqlDbType.Int).Value = dataId;
                                        cmd.Parameters.Add("@issue_stock_no", SqlDbType.VarChar, 20).Value = txtIssueNo.Value;
                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                        cmd.Parameters.Add("@product_name_tha", SqlDbType.VarChar, 200).Value = row.product_name_tha;
                                        cmd.Parameters.Add("@product_unit", SqlDbType.VarChar, 5).Value = row.unit_tha;
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                        cmd.Parameters.Add("@so_qty", SqlDbType.Int).Value = row.so_qty;
                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = row.remark;
                                        cmd.Parameters.Add("@sale_order_detail_id", SqlDbType.Int).Value = (cbbIssueType.Value == "1" ? row.sale_order_detail_id : 0);
                                        cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 2).Value = row.product_type;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        newDetailId = Convert.ToInt32(cmd.ExecuteScalar());

                                    }

                                    var childData = (from t in issueDetailMFGList where t.issue_stock_detail_id == oldDetailId select t).ToList();
                                    foreach (var rowDetail in childData)
                                    {
                                        rowDetail.issue_stock_detail_id = newDetailId;
                                    }
                                }
                                else if (row.id > 0 && row.is_delete == false)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_issue_stock_detail_edit", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                        cmd.Parameters.Add("@issue_stock_id", SqlDbType.Int).Value = dataId;
                                        cmd.Parameters.Add("@issue_stock_no", SqlDbType.VarChar, 20).Value = txtIssueNo.Value;
                                        cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = row.is_delete;
                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                        cmd.Parameters.Add("@product_name_tha", SqlDbType.VarChar, 200).Value = row.product_name_tha;
                                        cmd.Parameters.Add("@product_unit", SqlDbType.VarChar, 5).Value = row.unit_tha;
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                        cmd.Parameters.Add("@so_qty", SqlDbType.Int).Value = row.so_qty;
                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = row.remark;
                                        cmd.Parameters.Add("@sale_order_detail_id", SqlDbType.Int).Value = (cbbIssueType.Value == "1" ? row.sale_order_detail_id : 0);
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                                else if (row.id > 0 && row.is_delete == true)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_issue_stock_detail_edit", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                        cmd.Parameters.Add("@issue_stock_id", SqlDbType.Int).Value = dataId;
                                        cmd.Parameters.Add("@issue_stock_no", SqlDbType.VarChar, 20).Value = txtIssueNo.Value;
                                        cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = row.is_delete;
                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                        cmd.Parameters.Add("@product_name_tha", SqlDbType.VarChar, 200).Value = row.product_name_tha;
                                        cmd.Parameters.Add("@product_unit", SqlDbType.VarChar, 5).Value = row.unit_tha;
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 0;
                                        cmd.Parameters.Add("@so_qty", SqlDbType.Int).Value = row.so_qty;
                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = row.remark;
                                        cmd.Parameters.Add("@sale_order_detail_id", SqlDbType.Int).Value = (cbbIssueType.Value == "1" ? row.sale_order_detail_id : 0);
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                            }
                            #endregion

                            #region Detail MFG
                            if ((cbbIssueType.Value == "1" && hdQuotationType.Value == "P") || (cbbIssueType.Value == "0" && hdQuotationType.Value == "P"))
                            {
                                foreach (var row in issueDetailMFGList)
                                {
                                    if (row.id < 0)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_issue_stock_detail_mfg_add", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@issue_stock_no", SqlDbType.VarChar, 20).Value = txtIssueNo.Value;
                                            cmd.Parameters.Add("@issue_stock_detail_id", SqlDbType.Int).Value = row.issue_stock_detail_id;
                                            cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                            cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 100).Value = row.mfg_no.Trim();
                                            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 1;//row.qty;
                                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                            cmd.ExecuteNonQuery();

                                        }

                                        using (SqlCommand cmd = new SqlCommand("sp_customer_mfg_issue_add", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToInt32(hdCustomerId.Value);
                                            cmd.Parameters.Add("@model", SqlDbType.VarChar, 50).Value = string.Empty;
                                            cmd.Parameters.Add("@mfg", SqlDbType.VarChar, 100).Value = row.mfg_no.Trim();
                                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                            cmd.Parameters.Add("@unitwarraty", SqlDbType.Int).Value = row.unit_warranty;// row.qty;
                                            cmd.Parameters.Add("@airendwarraty", SqlDbType.Int).Value = row.air_end_warranry;// row.qty;
                                            cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;// row.qty;
                                            cmd.Parameters.Add("@service_fee", SqlDbType.Int).Value = row.service_fee;// row.qty;
                                            cmd.Parameters.Add("@project", SqlDbType.VarChar, 200).Value = lbSubject.Value;

                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                    else if (row.id > 0 && !row.is_deleted)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_issue_stock_detail_mfg_edit", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                            cmd.Parameters.Add("@issue_stock_no", SqlDbType.VarChar, 20).Value = txtIssueNo.Value;
                                            cmd.Parameters.Add("@issue_stock_detail_id", SqlDbType.Int).Value = row.issue_stock_detail_id;
                                            cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                            cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 100).Value = row.mfg_no.Trim();
                                            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 1;//row.qty;
                                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                            cmd.Parameters.Add("@is_deleted", SqlDbType.Int).Value = row.is_deleted;

                                            cmd.ExecuteNonQuery();

                                        }
                                        using (SqlCommand cmd = new SqlCommand("sp_customer_mfg_issue_edit", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToInt32(hdCustomerId.Value);
                                            cmd.Parameters.Add("@model", SqlDbType.VarChar, 50).Value = string.Empty;
                                            cmd.Parameters.Add("@mfg", SqlDbType.VarChar, 100).Value = row.mfg_no.Trim();
                                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                            cmd.Parameters.Add("@unitwarraty", SqlDbType.Int).Value = row.unit_warranty;// row.qty;
                                            cmd.Parameters.Add("@airendwarraty", SqlDbType.Int).Value = row.air_end_warranry;// row.qty;
                                            cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;// row.qty;
                                            cmd.Parameters.Add("@service_fee", SqlDbType.Int).Value = row.service_fee;// row.qty;

                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                    else
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_issue_stock_detail_mfg_delete", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                            cmd.ExecuteNonQuery();
                                        }
                                        /*using (SqlCommand cmd = new SqlCommand("sp_customer_mfg_issue_delete", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@mfg", SqlDbType.VarChar, 100).Value = row.mfg_no;
                                            //cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                            cmd.ExecuteNonQuery();
                                        }*/
                                    }
                                }
                            }
                            #endregion

                            #region SaleOrder Update Status

                            /*using (SqlCommand cmd = new SqlCommand("sp_sale_order_update_status", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@sale_order_no", SqlDbType.VarChar, 20).Value = cbbSaleOrder.Text;

                                cmd.ExecuteNonQuery();

                            }*/

                            #endregion


                            #region Transection
                            if (status == "CF")
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_issue_stock_detail_clear_reserved", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    
                                    cmd.Parameters.Add("@issue_stock_id", SqlDbType.Int).Value = dataId;
                                    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                    cmd.ExecuteNonQuery();

                                }

                                var transNo = string.Empty;
                                using (SqlCommand cmd = new SqlCommand("sp_gen_transection_no", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    transNo = Convert.ToString(cmd.ExecuteScalar());

                                }
                                string stock_type = string.Empty;
                                var documentLastFlag = hdDocumentFlag.Value;
                                foreach (var row in issueDetailList)
                                {
                                    if (row.id < 0 || ((documentLastFlag != status) && !row.is_delete))
                                    {
                                        if ((cbbIssueType.Value == "1" && hdQuotationType.Value == "P") || (cbbIssueType.Value == "0" && hdQuotationType.Value == "P"))
                                        {
                                            var mfgList = (from t in issueDetailMFGList where t.issue_stock_detail_id == row.id && !t.is_deleted select t).ToList();
                                            foreach (var mfgRow in mfgList)
                                            {
                                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.product_type;
                                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;

                                                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(hdQuotationNo.Value) ? "ISSUE_O" : "ISSUE";
                                                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "OUT";
                                                    cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 1;//row.qty;
                                                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "MFG No. " + mfgRow.mfg_no;//row.remark;
                                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                    cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtIssueNo.Value;
                                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = string.IsNullOrEmpty(hdCustomerId.Value) ? 0 : Convert.ToInt32(hdCustomerId.Value);
                                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.product_type;
                                                cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;

                                                cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(hdQuotationNo.Value) ? "ISSUE_O" : "ISSUE";
                                                cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "OUT";
                                                cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                                cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = row.remark;
                                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtIssueNo.Value;
                                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = string.IsNullOrEmpty(hdCustomerId.Value) ? 0 : Convert.ToInt32(hdCustomerId.Value);
                                                cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                    else if (row.id > 0 && row.is_delete == false)
                                    {
                                        //var diffQty = row.qty - row.old_qty;
                                        //if (diffQty != 0)
                                        //{
                                        var diffQty = 1;
                                            if ((cbbIssueType.Value == "1" && hdQuotationType.Value == "P") || (cbbIssueType.Value == "0" && hdQuotationType.Value == "P"))
                                            {
                                                var mfgList = (from t in issueDetailMFGList where t.issue_stock_detail_id == row.id && !t.is_deleted select t).ToList();
                                                foreach (var mfgRow in mfgList)
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                    {
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                        cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.product_type;
                                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;

                                                        cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(hdQuotationNo.Value) ? "ISSUE_O" : "ISSUE";
                                                        cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = diffQty < 0 ? "IN" : "OUT";
                                                        cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 1;
                                                        cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "MFG No. " + mfgRow.mfg_no;//row.remark;
                                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                        cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                        cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtIssueNo.Value;
                                                        cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = string.IsNullOrEmpty(hdCustomerId.Value) ? 0 : Convert.ToInt32(hdCustomerId.Value);
                                                        cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                        cmd.ExecuteNonQuery();

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.product_type;
                                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;

                                                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(hdQuotationNo.Value) ? "ISSUE_O" : "ISSUE";
                                                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = diffQty < 0 ? "IN" : "OUT";
                                                    cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = diffQty;
                                                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "แก้ไขจำนวน";
                                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                    cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtIssueNo.Value;
                                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = string.IsNullOrEmpty(hdCustomerId.Value) ? 0 : Convert.ToInt32(hdCustomerId.Value);
                                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                    cmd.ExecuteNonQuery();

                                                }
                                            }
                                        //}
                                    }
                                    else if (row.id > 0 && row.is_delete == true)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                            cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.product_type;
                                            cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;

                                            cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(hdQuotationNo.Value) ? "ISSUE_O" : "ISSUE";
                                            cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "IN";
                                            cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty * -1;// คืนค่า
                                            cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                            cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "คืนค่าจาก ISSUE";//row.remark;
                                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                            cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                            cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtIssueNo.Value;
                                            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = string.IsNullOrEmpty(hdCustomerId.Value) ? 0 : Convert.ToInt32(hdCustomerId.Value);
                                            cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                }
                            }

                            if (status == "CC")
                            {
                                var transNo = string.Empty;
                                using (SqlCommand cmd = new SqlCommand("sp_gen_transection_no", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    transNo = Convert.ToString(cmd.ExecuteScalar());

                                }
                                string stock_type = string.Empty;
                                var documentLastFlag = hdDocumentFlag.Value;
                                foreach (var row in issueDetailList)
                                {
                                    if (row.id < 0 || ((documentLastFlag != status) && !row.is_delete))
                                    {
                                        if ((cbbIssueType.Value == "1" && hdQuotationType.Value == "P") || (cbbIssueType.Value == "0" && hdQuotationType.Value == "P"))
                                        {
                                            var mfgList = (from t in issueDetailMFGList where t.issue_stock_detail_id == row.id && !t.is_deleted select t).ToList();
                                            foreach (var mfgRow in mfgList)
                                            {
                                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.product_type;
                                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;

                                                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(hdQuotationNo.Value) ? "ISSUE_O" : "ISSUE";
                                                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "IN";
                                                    cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 1;//row.qty;
                                                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Cancel MFG No. " + mfgRow.mfg_no;//row.remark;
                                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                    cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtIssueNo.Value;
                                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = string.IsNullOrEmpty(hdCustomerId.Value) ? 0 : Convert.ToInt32(hdCustomerId.Value);
                                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.product_type;
                                                cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;

                                                cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(hdQuotationNo.Value) ? "ISSUE_O" : "ISSUE";
                                                cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "IN";
                                                cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                                cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Cancel " + row.remark;
                                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtIssueNo.Value;
                                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = string.IsNullOrEmpty(hdCustomerId.Value) ? 0 : Convert.ToInt32(hdCustomerId.Value);
                                                cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                    else if (row.id > 0 && row.is_delete == false)
                                    {
                                        //var diffQty = row.qty - row.old_qty;
                                        //if (diffQty != 0)
                                        //{
                                        var diffQty = 1;
                                        if ((cbbIssueType.Value == "1" && hdQuotationType.Value == "P") || (cbbIssueType.Value == "0" && hdQuotationType.Value == "P"))
                                        {
                                            var mfgList = (from t in issueDetailMFGList where t.issue_stock_detail_id == row.id && !t.is_deleted select t).ToList();
                                            foreach (var mfgRow in mfgList)
                                            {
                                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.product_type;
                                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;

                                                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(hdQuotationNo.Value) ? "ISSUE_O" : "ISSUE";
                                                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = diffQty < 0 ? "OUT" : "IN";  // Swap in - out
                                                    cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 1;
                                                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Cancel MFG No. " + mfgRow.mfg_no;//row.remark;
                                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                    cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtIssueNo.Value;
                                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = string.IsNullOrEmpty(hdCustomerId.Value) ? 0 : Convert.ToInt32(hdCustomerId.Value);
                                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                    cmd.ExecuteNonQuery();

                                                }
                                            }
                                        }
                                        else
                                        {
                                            using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.product_type;
                                                cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;

                                                cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(hdQuotationNo.Value) ? "ISSUE_O" : "ISSUE";
                                                cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = diffQty < 0 ? "OUT" : "IN";  //  Swap in-out
                                                cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                cmd.Parameters.Add("@qty", SqlDbType.Int).Value = diffQty;
                                                cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Cancel แก้ไขจำนวน";
                                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtIssueNo.Value;
                                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = string.IsNullOrEmpty(hdCustomerId.Value) ? 0 : Convert.ToInt32(hdCustomerId.Value);
                                                cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                cmd.ExecuteNonQuery();

                                            }
                                        }
                                        //}
                                    }
                                    else if (row.id > 0 && row.is_delete == true)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                            cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.product_type;
                                            cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;

                                            cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(hdQuotationNo.Value) ? "ISSUE_O" : "ISSUE";
                                            cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "IN";
                                            cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty * -1;// คืนค่า
                                            cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                            cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Cancel คืนค่าจาก ISSUE";//row.remark;
                                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                            cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                            cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtIssueNo.Value;
                                            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = string.IsNullOrEmpty(hdCustomerId.Value) ? 0 : Convert.ToInt32(hdCustomerId.Value);
                                            cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                            cmd.ExecuteNonQuery();

                                        }
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
                        string errorMsg = ex.Message.ToString();


                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('" + errorMsg + "','E')", true);
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "BindElement", "changedIssueType()");
                        //throw ex;
                    }
                    finally
                    {
                        if (!conn.State.Equals(ConnectionState.Closed))
                        {

                            tran.Commit();
                            tran.Dispose();
                            conn.Close();

                            Response.Redirect("Issue.aspx?dataId=" + newID);
                        }
                    }
                }
            }
        }

        [WebMethod]
        public static string GetProductDetailData(string product_type)
        {
            HttpContext.Current.Session.Remove("SESSION_SALE_ORDER_DETAIL_ISSUE");
            //HttpContext.Current.Session.Remove("SESSION_ISSUE_DETAIL_ISSUE");
            var issueDetail = (List<IssueDetail>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_ISSUE"];
            var saleOrderDetail = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL_ISSUE"]; // ใช้ดาตาเซตตัวเดียวกัน จะได้ไม่งง
            var dsResult = new DataSet();
            var returnData = string.Empty;
            if (product_type == "P")
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@product_no", SqlDbType.VarChar,50) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },

                        };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_list", arrParm.ToArray());
                    conn.Close();
                }

                if (dsResult != null)
                {
                    saleOrderDetail = new List<SaleOrderDetail>();
                    var row = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                    foreach (var detail in row)
                    {
                        saleOrderDetail.Add(new SaleOrderDetail()
                        {
                            id = Convert.ToInt32(detail["id"]),
                            product_id = Convert.IsDBNull(detail["id"]) ? 0 : Convert.ToInt32(detail["id"]),
                            saleorder_description = Convert.IsDBNull(detail["product_name_tha"]) ? string.Empty : Convert.ToString(detail["product_name_tha"]),
                            is_selected = false,
                            qty = Convert.IsDBNull(detail["quantity"]) ? 0 : Convert.ToInt32(detail["quantity"]),
                            so_qty = 0,//Convert.IsDBNull(detail["so_qty"]) ? 0 : Convert.ToInt32(detail["so_qty"]),
                            unit_price = Convert.IsDBNull(detail["selling_price"]) ? 0 : Convert.ToDecimal(detail["selling_price"]),
                            unit_code = Convert.IsDBNull(detail["unit_code"]) ? string.Empty : Convert.ToString(detail["unit_code"]),
                            total_amount = Convert.IsDBNull(detail["selling_price"]) ? 0 : Convert.ToDecimal(detail["selling_price"]),//Convert.IsDBNull(detail["total_amount"]) ? 0 : Convert.ToDecimal(detail["total_amount"]),
                            sort_no = saleOrderDetail.Count == 0 ? 1 : (from t in saleOrderDetail select t.sort_no).Min() + 1,
                            discount_amount = 0,//Convert.IsDBNull(detail["discount_amount"]) ? 0 : Convert.ToDecimal(detail["discount_amount"]),
                            discount_percentage = 0,//Convert.IsDBNull(detail["discount_percentage"]) ? 1 : Convert.ToDecimal(detail["discount_percentage"]),
                            discount_total = 0,//Convert.IsDBNull(detail["discount_total"]) ? 0 : Convert.ToDecimal(detail["discount_total"]),
                            discount_type = string.Empty,//Convert.IsDBNull(detail["discount_type"]) ? string.Empty : Convert.ToString(detail["discount_type"]),
                            quotation_detail_id = 0,//Convert.IsDBNull(detail["quotation_detail_id"]) ? 0 : Convert.ToInt32(detail["quotation_detail_id"]),
                            product_no = Convert.IsDBNull(detail["product_no"]) ? string.Empty : Convert.ToString(detail["product_no"]),
                            quotation_type = string.Empty,//Convert.IsDBNull(detail["quotation_type"]) ? string.Empty : Convert.ToString(detail["quotation_type"]),
                            product_type = product_type

                        });
                    }

                }

            }
            else
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@search_name" , SqlDbType.VarChar,200) {Value = string.Empty}

                        };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_list", arrParm.ToArray());
                    conn.Close();
                }

                if (dsResult != null)
                {
                    saleOrderDetail = new List<SaleOrderDetail>();
                    var row = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                    foreach (var detail in row)
                    {
                        saleOrderDetail.Add(new SaleOrderDetail()
                        {
                            id = Convert.ToInt32(detail["id"]),
                            product_id = Convert.IsDBNull(detail["id"]) ? 0 : Convert.ToInt32(detail["id"]),
                            saleorder_description = Convert.IsDBNull(detail["part_name_tha"]) ? string.Empty : Convert.ToString(detail["part_name_tha"]),
                            is_selected = false,
                            qty = 1,//Convert.IsDBNull(detail["qty_remain"]) ? 0 : Convert.ToInt32(detail["qty_remain"]),
                            so_qty = 0,//Convert.IsDBNull(detail["so_qty"]) ? 0 : Convert.ToInt32(detail["so_qty"]),
                            unit_price = Convert.IsDBNull(detail["selling_price"]) ? 0 : Convert.ToDecimal(detail["selling_price"]),
                            unit_code = Convert.IsDBNull(detail["unit_code"]) ? string.Empty : Convert.ToString(detail["unit_code"]),
                            total_amount = Convert.IsDBNull(detail["selling_price"]) ? 0 : Convert.ToDecimal(detail["selling_price"]),
                            sort_no = saleOrderDetail.Count == 0 ? 1 : (from t in saleOrderDetail select t.sort_no).Min() + 1,
                            discount_amount = 0,//Convert.IsDBNull(detail["discount_amount"]) ? 0 : Convert.ToDecimal(detail["discount_amount"]),
                            discount_percentage = 0,//Convert.IsDBNull(detail["discount_percentage"]) ? 1 : Convert.ToDecimal(detail["discount_percentage"]),
                            discount_total = 0,//Convert.IsDBNull(detail["discount_total"]) ? 0 : Convert.ToDecimal(detail["discount_total"]),
                            discount_type = "",//Convert.IsDBNull(detail["discount_type"]) ? string.Empty : Convert.ToString(detail["discount_type"]),
                            quotation_detail_id = 0,//Convert.IsDBNull(detail["quotation_detail_id"]) ? 0 : Convert.ToInt32(detail["quotation_detail_id"]),
                            product_no = Convert.IsDBNull(detail["part_no"]) ? string.Empty : Convert.ToString(detail["part_no"]),
                            quotation_type = string.Empty,//Convert.IsDBNull(detail["quotation_type"]) ? string.Empty : Convert.ToString(detail["quotation_type"]),
                            product_type = product_type,
                            quotation_no = ""//Convert.IsDBNull(detail["quotation_no"]) ? string.Empty : Convert.ToString(detail["quotation_no"]),


                        });
                    }

                }

            }
            foreach (var row in saleOrderDetail)
            {
                var checkSelected = issueDetail.Where(r => r.product_id == row.product_id).FirstOrDefault();
                if (checkSelected != null)
                {
                    row.is_selected = true;
                }
            }
            HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL_ISSUE"] = saleOrderDetail;
            HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_ISSUE"] = issueDetail;
            return returnData;
        }

        [WebMethod]
        public static string ClearSaleOrderDetail()
        {
            var returnData = string.Empty;
            HttpContext.Current.Session.Remove("SESSION_SALE_ORDER_DETAIL_ISSUE");
            HttpContext.Current.Session.Remove("SESSION_ISSUE_DETAIL_ISSUE");
            HttpContext.Current.Session.Remove("SESSION_ISSUE_DETAIL_MFG_ISSUE");
            HttpContext.Current.Session.Remove("SESSION_SELECTED_ISSUE_DETAIL_ISSUE");

            return returnData;
        }

        [WebMethod]
        public static string SelectAllSaleOrder(bool selected, string searchTextStr)
        {

            var data = (List<SaleOrderDetail>)HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL_ISSUE"];
            var selectedIssue = (List<IssueDetail>)HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_ISSUE"];
            string searchText = searchTextStr;
            if (data != null)
            {
                var dateWithSearch = data.Where(t => (t.saleorder_description.ToUpper().Contains(searchText.ToUpper())
                                                        || t.product_no.ToUpper().Contains(searchText.ToUpper())
                                                        ) || searchText == string.Empty).ToList();

                foreach (var row in dateWithSearch)
                {
                    if (selected)
                    {
                        var selectedRows = data.Where(t => t.id == row.id).FirstOrDefault();
                        selectedRows.is_selected = true;
                        if (selectedIssue != null && selectedIssue.Count > 0) // มี Issue Detail
                        {
                            var rowExist = (from t in selectedIssue where t.sale_order_detail_id == row.id && !t.is_delete select t).FirstOrDefault();

                            if (rowExist != null) // กรณี Newmode
                            {
                                rowExist.qty = row.qty;
                            }
                            else
                            {
                                selectedIssue.Add(new IssueDetail()
                                {
                                    id = (selectedIssue.Count + 1) * -1,
                                    product_no = row.product_no,
                                    product_id = row.product_id,
                                    qty = 1,//row.qty,
                                    is_delete = false,
                                    product_name_tha = row.saleorder_description,
                                    sale_order_detail_id = Convert.ToInt32(row.id),
                                    remark = string.Empty,
                                    so_qty = row.so_qty,
                                    unit_tha = row.unit_code,
                                    product_type = row.product_type

                                });
                            }

                        }
                        else
                        {
                            if (selectedIssue == null)
                            {
                                selectedIssue = new List<IssueDetail>();
                            }
                            selectedIssue.Add(new IssueDetail()
                            {
                                id = (selectedIssue.Count + 1) * -1,
                                product_no = row.product_no,
                                product_id = row.product_id,
                                qty = 1,//row.qty,
                                is_delete = false,
                                product_name_tha = row.saleorder_description,
                                sale_order_detail_id = Convert.ToInt32(row.id),
                                remark = string.Empty,
                                so_qty = row.so_qty,
                                unit_tha = row.unit_code,
                                product_type = row.product_type

                            });
                        }
                    }
                    else
                    {
                        var rowExist = (from t in selectedIssue where t.sale_order_detail_id == row.id && !t.is_delete select t).FirstOrDefault();
                        var selectedRows = data.Where(t => t.id == row.id).FirstOrDefault();
                        selectedRows.is_selected = false;
                        if (rowExist != null)
                        {
                            if (rowExist.id < 0) // กรณี Newmode
                            {
                                selectedIssue.Remove(rowExist);
                            }
                            else
                            {
                                rowExist.is_delete = true;
                            }
                        }
                    }
                }
            }
            HttpContext.Current.Session["SESSION_SALE_ORDER_DETAIL_ISSUE"] = data; // ยัดค่ากลับ Session SaleOrder Detail Data
            HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_ISSUE"] = selectedIssue; // ยัดค่ากลับ Session Seltected Issue Order detail data
            return "SELECT ALL";
        }
    }

}