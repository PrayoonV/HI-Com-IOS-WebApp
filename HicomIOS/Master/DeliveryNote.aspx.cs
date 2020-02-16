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
    public partial class DeliveryNote : MasterDetailPage
    {
        private int dataId = 0;
        #region Members
        private DataSet dsDeliveryNoteData = new DataSet();
        public class DeliveryDetail
        {
            public int id { get; set; }
            public int issue_detail_id { get; set; }
            public String issue_stock_no { get; set; }
            public String product_no { get; set; }
            public int product_id { get; set; }
            public int issue_qty { get; set; }
            public int qty { get; set; }
            public String product_name_tha { get; set; }
            public decimal selling_price { get; set; }
            public bool is_deleted { get; set; }
            public bool is_selected { get; set; }
            public int sort_no { get; set; }
            public string unit_tha { get; set; }
            public string unit_code { get; set; }
            public string delivery_no { get; set; }
            public string remark { get; set; }
            public int qty_issue { get; set; }
            public string product_type { get; set; }
        }
        public class Issue
        {
            public int id { get; set; }
            public string unit_code { get; set; }
            public int customer_id { get; set; }
            public string company_name_tha { get; set; }
            public string attention_name { get; set; }
            public string address_bill_tha { get; set; }
            public string tel { get; set; }
            public string fax { get; set; }
            public string product_no { get; set; }
            public int product_id { get; set; }

            public string quotation_no { get; set; }
            public string project_name { get; set; }
            
            public string quotation_type { get; set; }
            public string issue_stock_no { get; set; }
            public string sales_order_no { get; set; }
            public string quotation_date { get; set; }
            public string sales_order_date { get; set; }
            public string issue_stock_date { get; set; }
            public bool is_deleted { get; set; }

            public string ref_po_no { get; set; }
            public string ref_po_date { get; set; }

        }
        public class IssueDETAIL
        {
            public bool is_selected { get; set; }
            public int id { get; set; }
            public int issue_qty { get; set; }
            public String product_no { get; set; }
            public int product_id { get; set; }
            public String issue_stock_no { get; set; }

            public String product_name_tha { get; set; }
            public decimal selling_price { get; set; }
            public String unit_tha { get; set; }
            public bool is_deleted { get; set; }
            public int sort_no { get; set; }
            public int qty { get; set; }
            public string product_type { get; set; }
        }
        List<IssueDETAIL> issueData   ///////
        {
            get
            {
                if (Session["SESSION_ISSUE_DETAIL_DELIVERY_NOTE"] == null)
                    Session["SESSION_ISSUE_DETAIL_DELIVERY_NOTE"] = new List<IssueDETAIL>();
                return (List<IssueDETAIL>)Session["SESSION_ISSUE_DETAIL_DELIVERY_NOTE"];
            }
            set
            {
                Session["SESSION_ISSUE_DETAIL_DELIVERY_NOTE"] = value;
            }
        }
        List<DeliveryDetail> deliveryDetailList ////
        {
            get
            {

                if (Session["SESSION_DELIVERY_DETAIL"] == null)
                    Session["SESSION_DELIVERY_DETAIL"] = new List<DeliveryDetail>();
                return (List<DeliveryDetail>)Session["SESSION_DELIVERY_DETAIL"];
            }
            set
            {
                Session["SESSION_DELIVERY_DETAIL"] = value;
            }
        }
        List<DeliveryDetail> deliverylist
        {
            get
            {
                if (Session["SESSION_SELECTED_DELIVERY"] == null)
                    Session["SESSION_SELECTED_DELIVERY"] = new List<DeliveryDetail>();
                return (List<DeliveryDetail>)Session["SESSION_SELECTED_DELIVERY"];
            }
            set
            {
                Session["SESSION_SELECTED_DELIVERY"] = value;
            }

        }
        Issue issueDataHeader
        {
            get
            {

                if (Session["SESSION_ISSUE_DATA_DELIVERY"] == null)
                    Session["SESSION_ISSUE_DATA_DELIVERY"] = new List<Issue>();
                return (Issue)Session["SESSION_ISSUE_DATA_DELIVERY"];
            }
            set
            {
                Session["SESSION_ISSUE_DATA_DELIVERY"] = value;
            }
        }
        private DataSet dsResult = new DataSet();
        #endregion

        public override string PageName { get { return "Create Delivery Note"; } }
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
                BindGridQuotationDetail();
            }
        }
        protected void PrepareData()
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    SPlanetUtil.BindASPxComboBox(ref cboIssueNo, DataListUtil.DropdownStoreProcedureName.Delivery_Note_Issue);
                }
                else
                {

                }

                gridViewDeliveryNote.SettingsBehavior.AllowFocusedRow = true;
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
                //cboIssueNo.Attributes.Add("disabled", "disabled");
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = dataId },
                        };
                    conn.Open();
                    dsDeliveryNoteData = SqlHelper.ExecuteDataset(conn, "sp_delivery_note_list_data", arrParm.ToArray());
                    ViewState["dsDeliveryNoteData"] = dsDeliveryNoteData;
                    conn.Close();
                }
                if (dsDeliveryNoteData.Tables.Count != 0)
                {
                    #region Header
                    var headerData = (from t in dsDeliveryNoteData.Tables[0].AsEnumerable() select t).FirstOrDefault();
                    if (headerData != null)
                    {
                        customer_id.Value = Convert.IsDBNull(headerData["customer_id"]) ? string.Empty : Convert.ToString(headerData["customer_id"]);
                        delivery_no.Value = Convert.IsDBNull(headerData["delivery_no"]) ? string.Empty : Convert.ToString(headerData["delivery_no"]);
                        cboIssueNo.Text = Convert.IsDBNull(headerData["issue_no"]) ? string.Empty : Convert.ToString(headerData["issue_no"]);
                        lbCustomerName.Value = Convert.IsDBNull(headerData["customer_name"]) ? string.Empty : Convert.ToString(headerData["customer_name"]);
                        lbAttentionName.Value = Convert.IsDBNull(headerData["attention_name"]) ? string.Empty : Convert.ToString(headerData["attention_name"]);
                        lbAddress.Value = Convert.IsDBNull(headerData["customer_address"]) ? string.Empty : Convert.ToString(headerData["customer_address"]);
                        lbtel.Value = Convert.IsDBNull(headerData["customer_tel"]) ? string.Empty : Convert.ToString(headerData["customer_tel"]);
                        lbQuotationNo.Value = Convert.IsDBNull(headerData["quotation_no"]) ? string.Empty : Convert.ToString(headerData["quotation_no"]);
                        hdQuotationType.Value = Convert.IsDBNull(headerData["quotation_type"]) ? string.Empty : Convert.ToString(headerData["quotation_type"]);
                        lbSalesOrderNo.Value = Convert.IsDBNull(headerData["saleorder_no"]) ? string.Empty : Convert.ToString(headerData["saleorder_no"]);
                        lbQuotationDate.Value = Convert.IsDBNull(headerData["quotation_date"]) ? string.Empty : Convert.ToDateTime(headerData["quotation_date"]).ToString("dd/MM/yyyy");
                        lbSalesOrderDate.Value = Convert.IsDBNull(headerData["saleorder_date"]) ? string.Empty : Convert.ToDateTime(headerData["saleorder_date"]).ToString("dd/MM/yyyy");
                        lbDeliveryDate.Value = Convert.IsDBNull(headerData["delivery_date"]) ? string.Empty : Convert.ToString(headerData["delivery_date"]);

                        lbPONo.Value = Convert.IsDBNull(headerData["ref_po_no"]) ? string.Empty : Convert.ToString(headerData["ref_po_no"]);
                        lbPODate.Value = Convert.IsDBNull(headerData["ref_po_date"]) ? string.Empty : Convert.ToDateTime(headerData["ref_po_date"]).ToString("dd/MM/yyyy");
                        lbProject.Value = Convert.IsDBNull(headerData["delivery_project"]) ? string.Empty : Convert.ToString(headerData["delivery_project"]);
                    }//
                    var status = Convert.ToString(headerData["delivery_status"]);
                    hdDocStatus.Value = status;
                    if (status == "CF")
                    {
                        btnDraft.Visible = false;
                        btnSave.InnerHtml = "<i class=\"fa fa-check-square-o\" aria-hidden=\"true\"></i>&nbsp;Complete";
                    }
                    else if (status == "CP")
                    {
                        btnNew.Visible = true;
                        btnSave.Visible = false;
                    }
                    else
                    {
                        btnNew.Visible = false;
                    }
                    #endregion

                    #region Detail
                    var detailData = (from t in dsDeliveryNoteData.Tables[1].AsEnumerable() select t).ToList();
                    if (detailData != null)
                    {
                        deliveryDetailList = new List<DeliveryDetail>();
                        foreach (var row in detailData)
                        {
                            deliveryDetailList.Add(new DeliveryDetail()
                            {
                                //product_name_tha
                                id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                delivery_no = Convert.IsDBNull(row["delivery_no"]) ? string.Empty : Convert.ToString(row["delivery_no"]),
                                issue_detail_id = Convert.IsDBNull(row["issue_detail_id"]) ? 0 : Convert.ToInt32(row["issue_detail_id"]),
                                qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["qty"]),
                                product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                unit_code = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]),
                                remark = Convert.IsDBNull(row["remark"]) ? string.Empty : Convert.ToString(row["remark"]),
                                product_name_tha = Convert.IsDBNull(row["product_name_tha"]) ? string.Empty : Convert.ToString(row["product_name_tha"]),
                                issue_qty = Convert.IsDBNull(row["issue_qty"]) ? 0 : Convert.ToInt32(row["issue_qty"]),
                                product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                            });
                        }
                    }

                    #endregion
                }

                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@issue_stock_no", SqlDbType.VarChar,20) { Value = cboIssueNo.Value },
                        };
                    conn.Open();

                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_issue_stock_detail_list", arrParm.ToArray());
                    conn.Close();
                }

                if (issueData == null || issueData.Count == 0)
                {
                    if (dsResult != null)
                    {
                        issueData = new List<IssueDETAIL>();
                        var row = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                        foreach (var detail in row)
                        {
                            issueData.Add(new IssueDETAIL()
                            {
                                id = Convert.ToInt32(detail["id"]),
                                is_selected = false,
                                issue_qty = Convert.IsDBNull(detail["qty_remain"]) ? 0 : Convert.ToInt32(detail["qty_remain"]),
                                issue_stock_no = Convert.IsDBNull(detail["issue_stock_no"]) ? string.Empty : Convert.ToString(detail["issue_stock_no"]),
                                product_name_tha = Convert.IsDBNull(detail["product_name_tha"]) ? string.Empty : Convert.ToString(detail["product_name_tha"]),
                                product_id = Convert.IsDBNull(detail["product_id"]) ? 0 : Convert.ToInt32(detail["product_id"]),
                                selling_price = Convert.IsDBNull(detail["selling_price"]) ? 0 : Convert.ToDecimal(detail["selling_price"]),
                                unit_tha = Convert.IsDBNull(detail["unit_tha"]) ? string.Empty : Convert.ToString(detail["unit_tha"]),
                                product_no = Convert.IsDBNull(detail["product_no"]) ? string.Empty : Convert.ToString(detail["product_no"]),
                                product_type = Convert.IsDBNull(detail["product_type"]) ? string.Empty : Convert.ToString(detail["product_type"]),

                            });
                        }

                    }
                }

                gridViewDeliveryNote.DataSource = deliveryDetailList;
                gridViewDeliveryNote.DataBind();
            }
            else
            {
                btnReportClient.Visible = false;
                btnSave.Visible = false;
            }

        }
        protected void ClearWorkingSession()
        {

            Session.Remove("SESSION_ISSUE_DETAIL_DELIVERY_NOTE");
            Session.Remove("SESSION_DELIVERY_DETAIL");
            Session.Remove("SESSION_SELECTED_DELIVERY");
            Session.Remove("SESSION_ISSUE_DATA_DELIVERY");
            Session.Remove("SESSION_DELIVERY_DETAIL");
        }
        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            return result.OfType<FilterControlColumn>();
        }

        protected void gridViewDeliveryNote_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridViewDeliveryNote.DataSource = (from t in deliveryDetailList where t.is_deleted == false select t).ToList();
            gridViewDeliveryNote.DataBind();
        }
        protected void BindGridSaleOrderDetail()
        {
            gridViewDeliveryNote.DataSource = (from t in deliveryDetailList where t.is_deleted == false select t).ToList();
            gridViewDeliveryNote.DataBind();
        }
        protected void gridViewDetailDeliveryNoteList_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxCheckBox checkBox = gridViewDetailDeliveryNoteList.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkIssueDetail") as ASPxCheckBox;
                if (checkBox != null)
                {
                    if (e.DataColumn.FieldName == "is_selected")
                    {
                        if (issueData != null)
                        {
                            var row = (from t in issueData where t.id == Convert.ToInt32(e.KeyValue) select t).FirstOrDefault();
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

        protected void gridViewDetailDeliveryNoteList_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
                gridViewDetailDeliveryNoteList.DataSource = issueData;
                gridViewDetailDeliveryNoteList.DataBind();
            }
            else
            {
                gridViewDetailDeliveryNoteList.DataSource = (from t in issueData
                                                   where  (t.product_no.ToUpper().Contains(searchText.ToUpper()) || t.product_name_tha.ToUpper().Contains(searchText.ToUpper()))
                                                   select t).ToList();
                gridViewDetailDeliveryNoteList.DataBind();
            }

        }
        protected void BindGridQuotationDetail()
        {
            gridViewDetailDeliveryNoteList.DataSource = (from t in issueData where t.is_deleted == false select t).ToList();
            gridViewDetailDeliveryNoteList.DataBind();
        }

        [WebMethod]
        public static List<DeliveryDetail> DeleteDeliveryNote(string id)
        {
            try
            {
                List<DeliveryDetail> datadelivery = (List<DeliveryDetail>)HttpContext.Current.Session["SESSION_DELIVERY_NOTE"];

                if (datadelivery != null)
                {
                    var selectedData = (from t in datadelivery where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (selectedData != null)
                    {
                        if (Convert.ToInt32(id) < 0)
                        {
                            datadelivery.Remove(selectedData);
                        }
                        else
                        {
                            selectedData.is_deleted = true;
                        }
                    }
                }

                HttpContext.Current.Session["SESSION_DELIVERY_NOTE"] = datadelivery;

                return datadelivery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static Issue GetIssueData(string id)
        {
            var dataSelected = new DataSet();
            var dataReturn = (Issue)HttpContext.Current.Session["SESSION_ISSUE_DATA_DELIVERY"];
            var IssueData = (List<IssueDETAIL>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_DELIVERY_NOTE"];
            var DeliveryDetail = (List<DeliveryDetail>)HttpContext.Current.Session["SESSION_DELIVERY_DETAIL"];
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(id) }
                        };
                conn.Open();
                dataSelected = SqlHelper.ExecuteDataset(conn, "sp_issue_stock_header_list", arrParm.ToArray());

                conn.Close();
            }
            IssueData = new List<IssueDETAIL>(); // clear detail ทุกครั้งที่เปลี่ยน header
            DeliveryDetail = new List<DeliveryDetail>(); // clear detail ทุกครั้งที่เปลี่ยน header
            dataReturn = new Issue();
            HttpContext.Current.Session["SESSION_DELIVERY_DETAIL"] = DeliveryDetail; // ยัดค่ากลับ Session
            HttpContext.Current.Session["SESSION_ISSUE_DETAIL_DELIVERY_NOTE"] = IssueData; // ยัดค่ากลับ Session

            if (dataSelected != null)
            {
                var row = (from t in dataSelected.Tables[0].AsEnumerable() select t).FirstOrDefault();

                if (row != null)
                {
                    dataReturn.customer_id = Convert.IsDBNull(row["customer_id"]) ? 0 : Convert.ToInt32(row["customer_id"]);

                    dataReturn.company_name_tha = Convert.IsDBNull(row["customer_name"]) ? string.Empty : Convert.ToString(row["customer_name"]);
                    dataReturn.attention_name = Convert.IsDBNull(row["attention_name"]) ? string.Empty : Convert.ToString(row["attention_name"]);
                    dataReturn.address_bill_tha = Convert.IsDBNull(row["address_bill_eng"]) ? string.Empty : Convert.ToString(row["address_bill_eng"]);
                    dataReturn.tel = Convert.IsDBNull(row["tel"]) ? string.Empty : Convert.ToString(row["tel"]);
                    dataReturn.fax = Convert.IsDBNull(row["fax"]) ? string.Empty : Convert.ToString(row["fax"]);
                    dataReturn.issue_stock_no = Convert.IsDBNull(row["issue_stock_no"]) ? string.Empty : Convert.ToString(row["issue_stock_no"]);
                    dataReturn.quotation_no = Convert.IsDBNull(row["quotation_no"]) ? string.Empty : Convert.ToString(row["quotation_no"]);
                    dataReturn.project_name = Convert.IsDBNull(row["project_name"]) ? string.Empty : Convert.ToString(row["project_name"]);
                    dataReturn.quotation_type = Convert.IsDBNull(row["quotation_type"]) ? string.Empty : Convert.ToString(row["quotation_type"]);
                    dataReturn.sales_order_no = Convert.IsDBNull(row["sale_order_no"]) ? string.Empty : Convert.ToString(row["sale_order_no"]);
                    dataReturn.quotation_date = Convert.IsDBNull(row["quotation_date"]) ? string.Empty : Convert.ToDateTime(row["quotation_date"]).ToString("dd/MM/yyyy");
                    dataReturn.sales_order_date = Convert.IsDBNull(row["sale_order_date"]) ? string.Empty : Convert.ToDateTime(row["sale_order_date"]).ToString("dd/MM/yyyy");
                    
                    dataReturn.issue_stock_date = Convert.IsDBNull(row["issue_stock_date"]) ? string.Empty : Convert.ToDateTime(row["issue_stock_date"]).ToString("dd/MM/yyyy");

                    dataReturn.ref_po_no = Convert.IsDBNull(row["ref_po_no"]) ? string.Empty : Convert.ToString(row["ref_po_no"]);
                    dataReturn.ref_po_date = Convert.IsDBNull(row["ref_po_date"]) ? string.Empty : Convert.ToDateTime(row["ref_po_date"]).ToString("dd/MM/yyyy");

                }
            }
            return dataReturn;
        }

        [WebMethod]
        public static List<IssueDETAIL> ViewIssueDetail(string id)
        {
            var dsResult = new DataSet();
            var issueData = (List<IssueDETAIL>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_DELIVERY_NOTE"];
            var deliveryDetail = (List<DeliveryDetail>)HttpContext.Current.Session["SESSION_DELIVERY_DETAIL"];

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@issue_stock_no", SqlDbType.VarChar,20) { Value = id },
                        };
                conn.Open();

                dsResult = SqlHelper.ExecuteDataset(conn, "sp_issue_stock_detail_list", arrParm.ToArray());
                conn.Close();
            }

            if (issueData == null || issueData.Count == 0)
            {
                if (dsResult != null)
                {
                    issueData = new List<IssueDETAIL>();
                    var row = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                    foreach (var detail in row)
                    {
                        issueData.Add(new IssueDETAIL()
                        {
                            id = Convert.ToInt32(detail["id"]),
                            is_selected = false,
                            issue_qty = Convert.IsDBNull(detail["qty_remain"]) ? 0 : Convert.ToInt32(detail["qty_remain"]),
                            issue_stock_no = Convert.IsDBNull(detail["issue_stock_no"]) ? string.Empty : Convert.ToString(detail["issue_stock_no"]),
                            product_name_tha = Convert.IsDBNull(detail["product_name_tha"]) ? string.Empty : Convert.ToString(detail["product_name_tha"]),
                            product_id = Convert.IsDBNull(detail["product_id"]) ? 0 : Convert.ToInt32(detail["product_id"]),
                            selling_price = Convert.IsDBNull(detail["selling_price"]) ? 0 : Convert.ToDecimal(detail["selling_price"]),
                            unit_tha = Convert.IsDBNull(detail["unit_tha"]) ? string.Empty : Convert.ToString(detail["unit_tha"]),
                            product_no = Convert.IsDBNull(detail["product_no"]) ? string.Empty : Convert.ToString(detail["product_no"]),
                            product_type = Convert.IsDBNull(detail["product_type"]) ? string.Empty : Convert.ToString(detail["product_type"]),

                        });
                    }

                }
            }
            HttpContext.Current.Session["SESSION_SELECTED_DELIVERY"] = deliveryDetail; // ย้ายดาต้าจริงเข้า Session จำลอง
            HttpContext.Current.Session["SESSION_ISSUE_DETAIL_DELIVERY_NOTE"] = issueData;
            return issueData;
        }

        [WebMethod]
        public static List<IssueDETAIL> AddIssueDetail(string id, bool isSelected)
        {
            var data = (List<IssueDETAIL>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_DELIVERY_NOTE"];
            var selectedDelivery = (List<DeliveryDetail>)HttpContext.Current.Session["SESSION_SELECTED_DELIVERY"];
            var selectedDeliveryDetailRow = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault(); // เลือก Data จาก Quotation Detail
            if (data != null)
            {
                if (selectedDelivery != null) // มี SaleOrder Detail
                {
                    var rowDeliveryDetail = (from t in selectedDelivery where t.issue_detail_id == Convert.ToInt32(id) select t).FirstOrDefault(); // เลือก Data จาก Sale ORder Detail
                        if (rowDeliveryDetail != null) // มีอยุ่ใน SaleOrder Detail
                        {
                            if (isSelected) // Add
                            {
                                selectedDeliveryDetailRow.is_selected = true;
                            rowDeliveryDetail.is_deleted = false;
                            //if (Convert.ToInt32(rowDeliveryDetail.id) < 0) // กรณี Newmode
                            //    {
                            //        selectedDelivery.Add(new DeliveryDetail()
                            //        {
                            //            id = (selectedDelivery.Count + 1) * -1,
                            //            issue_detail_id = selectedDeliveryDetailRow.id,
                            //            product_id = selectedDeliveryDetailRow.product_id,
                            //            unit_code = selectedDeliveryDetailRow.unit_tha,
                            //            issue_stock_no = rowDeliveryDetail.issue_stock_no,
                            //            qty = selectedDeliveryDetailRow.issue_qty,
                            //            issue_qty = selectedDeliveryDetailRow.issue_qty,
                            //            product_name_tha = selectedDeliveryDetailRow.product_name_tha,
                            //            sort_no = selectedDelivery.Count == 0 ? 1 : (from t in selectedDelivery select t.sort_no).Max() + 1,
                            //            product_no = selectedDeliveryDetailRow.product_no,
                            //            selling_price = selectedDeliveryDetailRow.selling_price,
                            //            product_type = selectedDeliveryDetailRow.product_type
                            
                            //        });
                            //    }
                            //    else if (Convert.ToInt32(rowDeliveryDetail.id) > 0) // กรณี EditMode
                            //    {
                            //        rowDeliveryDetail.is_deleted = false;
                            //        //selectedDelivery.Remove(rowDeliveryDetail);
                            //    }
                            }
                            else // Remove
                            {
                                selectedDeliveryDetailRow.is_selected = false;
                                if (Convert.ToInt32(rowDeliveryDetail.id) < 0) // กรณี Newmode
                                {
                                    selectedDelivery.Remove(rowDeliveryDetail); // ลบ SaleOrder Detail
                                }
                                else // กรณี EditMode
                                {
                                    rowDeliveryDetail.is_deleted = true; // เปลี่ยน Flag เป็นลบ แล้ว เพิ่มกลับไปยัง Quotation
                                   
                                }
                            }
                        }
                        else // Add SaleOrder Detail
                        {
                            if (isSelected) // มีแค่กรณี Selected เป็น True เท่านั้น
                            {
                                selectedDeliveryDetailRow.is_selected = true;

                                selectedDelivery.Add(new DeliveryDetail()
                                {
                                    id = (selectedDelivery.Count + 1) * -1,
                                    issue_detail_id = selectedDeliveryDetailRow.id,
                                    qty = selectedDeliveryDetailRow.issue_qty,
                                    product_id = selectedDeliveryDetailRow.product_id,
                                    issue_stock_no = selectedDeliveryDetailRow.issue_stock_no,
                                    issue_qty = selectedDeliveryDetailRow.issue_qty,
                                    product_name_tha = selectedDeliveryDetailRow.product_name_tha,
                                    sort_no = selectedDelivery.Count == 0 ? 1 : (from t in selectedDelivery select t.sort_no).Max() + 1,
                                    product_no = selectedDeliveryDetailRow.product_no,
                                    unit_code = selectedDeliveryDetailRow.unit_tha,
                                    selling_price = selectedDeliveryDetailRow.selling_price,
                                    product_type = selectedDeliveryDetailRow.product_type,
                                    is_deleted = false,
                                });
                            }
                        }
                    
                }
                else // Add SaleOrder Detail
                {
                    selectedDelivery = new List<DeliveryDetail>();
                    if (selectedDeliveryDetailRow != null)
                    {
                        if (isSelected) // มีแค่กรณี Selected เป็น True เท่านั้น
                        {
                            selectedDeliveryDetailRow.is_selected = true;
                            selectedDelivery.Add(new DeliveryDetail()
                            {
                                id = (selectedDelivery.Count + 1) * -1,
                                issue_detail_id = selectedDeliveryDetailRow.id,
                                product_id = selectedDeliveryDetailRow.product_id,
                                unit_code = selectedDeliveryDetailRow.unit_tha,
                                issue_qty = selectedDeliveryDetailRow.issue_qty,
                                issue_stock_no = selectedDeliveryDetailRow.issue_stock_no,
                                qty = selectedDeliveryDetailRow.issue_qty,
                                product_name_tha = selectedDeliveryDetailRow.product_name_tha,
                                sort_no = selectedDelivery.Count == 0 ? 1 : (from t in selectedDelivery select t.sort_no).Max() + 1,
                                product_no = selectedDeliveryDetailRow.product_no,
                                selling_price = selectedDeliveryDetailRow.selling_price,
                                product_type = selectedDeliveryDetailRow.product_type,
                                is_deleted = false,
                            });
                        }
                    }
                }
            }

            HttpContext.Current.Session["SESSION_ISSUE_DETAIL_DELIVERY_NOTE"] = data; // ยัดค่ากลับ Session QUotation Detail Data
            HttpContext.Current.Session["SESSION_SELECTED_DELIVERY"] = selectedDelivery; // ยัดค่ากลับ Sesssion Sale Order detail data
            return data;
        }

        [WebMethod]
        public static List<DeliveryDetail> SubmitIssueDetail()
        {
            var DeliveryDetail = (List<DeliveryDetail>)HttpContext.Current.Session["SESSION_DELIVERY_DETAIL"];
            var selectedDelivery = (List<DeliveryDetail>)HttpContext.Current.Session["SESSION_SELECTED_DELIVERY"];
            if (selectedDelivery != null)
            {
                foreach (var selectedRow in selectedDelivery)
                {
                    var existItem = (from t in DeliveryDetail where t.is_deleted != true && t.id == selectedRow.id select t).FirstOrDefault();
                    if (existItem == null)
                    {
                        if (selectedRow.is_deleted == false)
                        {
                            DeliveryDetail.Add(selectedRow);
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

                            var rowQuotation = (from t in selectedDelivery where t.id == selectedRow.id select t).FirstOrDefault();
                            if (rowQuotation != null)
                            {
                                if (rowQuotation.is_selected)
                                {
                                    selectedDelivery.Remove(rowQuotation);
                                }
                            }
                        }
                    }
                }
            }

            HttpContext.Current.Session["SESSION_DELIVERY_DETAIL"] = selectedDelivery;
            HttpContext.Current.Session["SESSION_SELECTED_DELIVERY"] = null;
            return selectedDelivery;
        }

        [WebMethod]
        public static List<DeliveryDetail> DeleteIssueDetail(string id)/////////Delete data =>grid detaildelivery
        {
            var DeliveryDetail = (List<DeliveryDetail>)HttpContext.Current.Session["SESSION_DELIVERY_DETAIL"];
            var IssueData = (List<IssueDETAIL>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_DELIVERY_NOTE"];
            var rowDeletedDelivery = (from t in DeliveryDetail where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
            var rowIssueDetail = (from t in IssueData where t.id == rowDeletedDelivery.issue_detail_id select t).FirstOrDefault();
            if (Convert.ToInt32(id) < 0)
            {
                DeliveryDetail.Remove(rowDeletedDelivery);
                rowIssueDetail.is_selected = false;
            }
            else
            {
                rowDeletedDelivery.is_deleted = true;
                IssueData.Add(new IssueDETAIL()
                {
                    id = rowDeletedDelivery.issue_detail_id,
                    is_selected = false,
                    issue_stock_no = rowDeletedDelivery.issue_stock_no,
                    issue_qty = rowDeletedDelivery.issue_qty,
                    product_name_tha = rowDeletedDelivery.product_name_tha,
                    product_no = rowDeletedDelivery.product_no,
                    unit_tha = rowDeletedDelivery.unit_tha,
                    sort_no = rowDeletedDelivery.sort_no,
                    product_type = rowDeletedDelivery.product_type

                });
            }
            HttpContext.Current.Session["SESSION_DELIVERY_DETAIL"] = DeliveryDetail;
            HttpContext.Current.Session["SESSION_ISSUE_DETAIL_DELIVERY_NOTE"] = IssueData;
            return DeliveryDetail;
        }

        [WebMethod]
        public static string SubmitSaveData()
        {
            string msg = string.Empty;
            msg += "success";
            return msg;
        }

        private void SaveData(string status)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                int newID = 0;
                var detailNo = string.Empty;

                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        DateTime? quotation_date = null;
                        DateTime? saleorder_date = null;
                        if (!string.IsNullOrEmpty(lbQuotationDate.Value))
                        {
                            quotation_date = DateTime.ParseExact(lbQuotationDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (!string.IsNullOrEmpty(lbSalesOrderDate.Value))
                        {
                            saleorder_date = DateTime.ParseExact(lbSalesOrderDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (dataId == 0) // Insert Mode
                        {

                            using (SqlCommand cmd = new SqlCommand("sp_delivery_note_header_add", conn,tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@issue_no", SqlDbType.VarChar, 20).Value = cboIssueNo.Text;
                                cmd.Parameters.Add("@delivery_status", SqlDbType.VarChar, 2).Value = status;
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customer_id.Value;
                                cmd.Parameters.Add("@customer_name", SqlDbType.VarChar, 200).Value = lbCustomerName.Value;
                                cmd.Parameters.Add("@attention_name", SqlDbType.VarChar, 200).Value = lbAttentionName.Value;
                                cmd.Parameters.Add("@customer_address", SqlDbType.VarChar, 200).Value = lbAddress.Value;
                                cmd.Parameters.Add("@customer_tel", SqlDbType.VarChar, 200).Value = lbtel.Value;
                                cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = lbQuotationNo.Value;
                                cmd.Parameters.Add("@quotation_type", SqlDbType.VarChar, 1).Value = hdQuotationType.Value;
                                cmd.Parameters.Add("@saleorder_no", SqlDbType.VarChar, 20).Value = lbSalesOrderNo.Value;
                                cmd.Parameters.Add("@quotation_date", SqlDbType.Date).Value = quotation_date;
                                cmd.Parameters.Add("@saleorder_date", SqlDbType.Date).Value = saleorder_date;
                                cmd.Parameters.Add("@delivery_date", SqlDbType.VarChar, 20).Value = lbDeliveryDate.Value == "" ? DateTime.UtcNow.ToString("dd/MM/yyyy") : lbDeliveryDate.Value;
                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                cmd.Parameters.Add("@delivery_project", SqlDbType.VarChar,200).Value = lbProject.Value;

                                newID = Convert.ToInt32(cmd.ExecuteScalar());
     
                            }

                            //Create array of Parameters
                            List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = newID },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = "" },
                        };
               
                            var lastDataInsert = SqlHelper.ExecuteDataset(tran, "sp_delivery_note_header_list", arrParm.ToArray());
                      

                            if (lastDataInsert != null)
                            {
                                detailNo = (from t in lastDataInsert.Tables[0].AsEnumerable()
                                            select t.Field<string>("delivery_no")).FirstOrDefault();
                            }

                            foreach (var row in deliveryDetailList)
                            {

                                using (SqlCommand cmd = new SqlCommand("sp_delivery_note_detail_add", conn,tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@delivery_no", SqlDbType.VarChar, 20).Value = detailNo;
                                    cmd.Parameters.Add("@issue_detail_id", SqlDbType.Int).Value = row.issue_detail_id;
                                    cmd.Parameters.Add("@issue_qty", SqlDbType.Int).Value = row.issue_qty; //////หาค่าqty_issueในสโตว์ไม่เจอ
                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = row.remark == "" ? string.Empty : row.remark;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                    cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 5).Value = row.unit_code;
                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 2).Value = row.product_type;
                                    cmd.Parameters.Add("@product_name_tha", SqlDbType.VarChar, 250).Value = row.product_name_tha;
                       
                                    cmd.ExecuteNonQuery();
                                 
                                }

                            }
                            #region Issue Update Status

                            /*using (SqlCommand cmd = new SqlCommand("sp_issue_update_status", conn,tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@ref_no", SqlDbType.VarChar, 20).Value = cboIssueNo.Text;
                                cmd.Parameters.Add("@ref_type", SqlDbType.VarChar, 2).Value = "DL";
                        
                                cmd.ExecuteNonQuery();
                           
                            }*/

                            #endregion
                        }
                        else  //Edit  Mode
                        {
                            newID = dataId;

                            if (status == "CF" && hdDocStatus.Value == "CF")
                            {
                                status = "CP";
                            }
                            else if (status == "FL")
                            {
                                status = hdDocStatus.Value;
                            }

                            using (SqlCommand cmd = new SqlCommand("sp_delivery_note_header_edit", conn,tran))
                            {

                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = dataId;
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customer_id.Value;
                                cmd.Parameters.Add("@delivery_status", SqlDbType.VarChar, 2).Value = status;
                                cmd.Parameters.Add("@delivery_no", SqlDbType.VarChar, 20).Value = delivery_no.Value;
                                cmd.Parameters.Add("@issue_no", SqlDbType.VarChar, 20).Value = cboIssueNo.Text;
                                cmd.Parameters.Add("@customer_name", SqlDbType.VarChar, 200).Value = lbCustomerName.Value;
                                cmd.Parameters.Add("@attention_name", SqlDbType.VarChar, 200).Value = lbAttentionName.Value;
                                cmd.Parameters.Add("@customer_address", SqlDbType.VarChar, 200).Value = lbAddress.Value;
                                cmd.Parameters.Add("@customer_tel", SqlDbType.VarChar, 200).Value = lbtel.Value;
                                cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = lbQuotationNo.Value;
                                cmd.Parameters.Add("@quotation_type", SqlDbType.VarChar, 1).Value = hdQuotationType.Value;
                                cmd.Parameters.Add("@saleorder_no", SqlDbType.VarChar, 20).Value = lbSalesOrderNo.Value;
                                cmd.Parameters.Add("@quotation_date", SqlDbType.Date).Value = quotation_date;
                                cmd.Parameters.Add("@saleorder_date", SqlDbType.Date).Value = saleorder_date;
                                cmd.Parameters.Add("@delivery_date", SqlDbType.VarChar,20).Value = lbDeliveryDate.Value == "" ? DateTime.UtcNow.ToString("dd/MM/yyyy") : lbDeliveryDate.Value ;
                                cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                cmd.Parameters.Add("@delivery_project", SqlDbType.VarChar, 200).Value = lbProject.Value;


                                cmd.ExecuteNonQuery();
                        
                            }

                            //Create array of Parameters
                            foreach (var row in deliveryDetailList)
                            {
                                if (row.id > 0 && row.is_deleted == false) // EDIT DETAIL EDIT MODE
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_delivery_note_detail_edit", conn,tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                        cmd.Parameters.Add("@issue_detail_id", SqlDbType.Int).Value = row.issue_detail_id;
                                        cmd.Parameters.Add("@issue_qty", SqlDbType.Int).Value = row.issue_qty;
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = row.remark == "" ? string.Empty : row.remark;
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                   
                                        cmd.ExecuteNonQuery();
                                     
                                    }
                                }
                                else if (row.id < 0)// ADD DETAIL EDIT MODE
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_delivery_note_detail_add", conn,tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@delivery_no", SqlDbType.VarChar, 20).Value = customer_id.Value;
                                        cmd.Parameters.Add("@issue_detail_id", SqlDbType.Int).Value = row.issue_detail_id;
                                        cmd.Parameters.Add("@issue_qty", SqlDbType.Int).Value = row.issue_qty;
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = row.remark == "" ? string.Empty: row.remark;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                        cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 5).Value = row.unit_code;
                                        cmd.Parameters.Add("@product_name_tha", SqlDbType.VarChar, 250).Value = row.product_name_tha;
                                        cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 2).Value = row.product_type;
                                        cmd.ExecuteNonQuery();
                                        
                                    }

                                }
                                else if (row.id > 0 && row.is_deleted == true) // DELETE DETAIL EDIT MODE
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_delivery_note_detail_delete", conn,tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                        cmd.Parameters.Add("@issue_detail_id", SqlDbType.Int).Value = row.issue_detail_id;
                                        cmd.Parameters.Add("@issue_qty", SqlDbType.Int).Value = row.issue_qty;
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 0;
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                       
                                        cmd.ExecuteNonQuery();
                                     
                                    }
                                }
                            }
                            #region Issue Update Status

                            if (status == "CF")
                            {
                                /*using (SqlCommand cmd = new SqlCommand("sp_issue_update_status", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@ref_no", SqlDbType.VarChar, 20).Value = cboIssueNo.Text;
                                    cmd.Parameters.Add("@ref_type", SqlDbType.VarChar, 2).Value = "DL";
                                    cmd.ExecuteNonQuery();
                                }*/
                            }

                            #endregion
                        }
                       
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        tran.Dispose();
                        conn.Close();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage(\"" + ex.Message + "\",'E')", true);
                        //throw ex;
                    }
                    finally
                    {
                        if (!conn.State.Equals(ConnectionState.Closed))
                        {

                            tran.Commit();
                            tran.Dispose();
                            conn.Close();
                            Response.Redirect("DeliveryNote.aspx?dataId=" + newID);
                        }
                    }
                }

            }
        }

        [WebMethod]
        public static DeliveryDetail EditDeliveryNote(string id)
        {
            var DeliveryNote = (List<DeliveryDetail>)HttpContext.Current.Session["SESSION_DELIVERY_DETAIL"];
            var SelectedDeliveryNote = new DeliveryDetail();
            if (DeliveryNote != null)
            {
                var row = (from t in DeliveryNote where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (row != null)
                {
                    SelectedDeliveryNote = row;
                }
            }

            return SelectedDeliveryNote;
        }

        [WebMethod]
        public static List<DeliveryDetail> DeleteDeliveryDetail(string id)
        {
            var DeliveryNoteDetail = (List<DeliveryDetail>)HttpContext.Current.Session["SESSION_DELIVERY_DETAIL"];
            var IssueData = (List<IssueDETAIL>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_DELIVERY_NOTE"];
            var rowDeletedDelivery = (from t in DeliveryNoteDetail where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
            var rowIssueDetail = (from t in IssueData where t.id == rowDeletedDelivery.issue_detail_id select t).FirstOrDefault();
            if (Convert.ToInt32(id) < 0)
            {
                DeliveryNoteDetail.Remove(rowDeletedDelivery);
                rowIssueDetail.is_selected = false;
            }
            else
            {
                rowDeletedDelivery.is_deleted = true;
                IssueData.Add(new IssueDETAIL()
                {
                    id = rowDeletedDelivery.issue_detail_id,
                    product_id = rowDeletedDelivery.product_id,
                    issue_stock_no = rowDeletedDelivery.issue_stock_no,
                    issue_qty = rowDeletedDelivery.qty,
                    qty = rowDeletedDelivery.qty,
                    product_name_tha = rowDeletedDelivery.product_name_tha,
                    product_no = rowDeletedDelivery.product_no,
                    unit_tha = rowDeletedDelivery.unit_tha,
                    product_type = rowDeletedDelivery.product_type


                });
            }
            HttpContext.Current.Session["SESSION_DELIVERY_DETAIL"] = DeliveryNoteDetail;
            HttpContext.Current.Session["SESSION_ISSUE_DETAIL_DELIVERY_NOTE"] = IssueData;
            return DeliveryNoteDetail;
        }
        [WebMethod]
        public static List<DeliveryDetail> SubmitEditDeliveryNote(string id, string qty, string remark)
        {
            var deliverNoteDetail = (List<DeliveryDetail>)HttpContext.Current.Session["SESSION_DELIVERY_DETAIL"];
            if (deliverNoteDetail != null)
            {
                var rowdeliverNoteDetail = (from d in deliverNoteDetail where d.id == Convert.ToInt32(id) select d).FirstOrDefault();
                if (rowdeliverNoteDetail != null)
                {
                    rowdeliverNoteDetail.qty = Convert.ToInt32(qty);
                    rowdeliverNoteDetail.remark = remark;
                }
            }
            HttpContext.Current.Session["SESSION_DELIVERY_DETAIL"] = deliverNoteDetail;
            return deliverNoteDetail;

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
        public static string SelectAllDetailDeliveryNoteList(bool selected)
        {
            var data = (List<IssueDETAIL>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_DELIVERY_NOTE"];
            var selectedDelivery = (List<DeliveryDetail>)HttpContext.Current.Session["SESSION_SELECTED_DELIVERY"];
            if (data != null)
            {
                
                foreach (var row in data)
                {
                    if (selected)
                    {
                        row.is_selected = true;
                        if (selectedDelivery != null && selectedDelivery.Count > 0)
                        {
                            var rowExist = (from t in selectedDelivery where t.issue_detail_id == row.id select t).FirstOrDefault();

                            if (rowExist != null) // กรณี Newmode
                            {
                                rowExist.qty = row.qty;
                                rowExist.is_deleted = false;
                            }
                            else
                            {
                                selectedDelivery.Add(new DeliveryDetail()
                                {
                                    id = (selectedDelivery.Count + 1) * -1,
                                    issue_detail_id = row.id,
                                    product_id = row.product_id,
                                    unit_code = row.unit_tha,
                                    issue_stock_no = row.issue_stock_no,
                                    qty = row.issue_qty,
                                    issue_qty = row.issue_qty,
                                    product_name_tha = row.product_name_tha,
                                    sort_no = selectedDelivery.Count == 0 ? 1 : (from t in selectedDelivery select t.sort_no).Max() + 1,
                                    product_no = row.product_no,
                                    selling_price = row.selling_price,
                                    is_deleted = false,
                                    product_type = row.product_type
                                });
                            }

                        }
                        else
                        {

                        if (selectedDelivery == null)
                            {
                                selectedDelivery = new List<DeliveryDetail>();
                            }
                            selectedDelivery.Add(new DeliveryDetail()
                            {
                                id = (selectedDelivery.Count + 1) * -1,
                                issue_detail_id = row.id,
                                is_deleted = false,
                                product_id = row.product_id,
                                unit_code = row.unit_tha,
                                issue_stock_no = row.issue_stock_no,
                                qty = row.issue_qty,
                                issue_qty = row.issue_qty,
                                product_name_tha = row.product_name_tha,
                                sort_no = selectedDelivery.Count == 0 ? 1 : (from t in selectedDelivery select t.sort_no).Max() + 1,
                                product_no = row.product_no,
                                selling_price = row.selling_price,
                                product_type = row.product_type
                            });
                        }
                    }
                    else
                    {
                        var rowExist = (from t in selectedDelivery where t.issue_detail_id == row.id select t).FirstOrDefault();
                        row.is_selected = false;
                        if (rowExist != null)
                        {
                            if (rowExist.id < 0) // กรณี Newmode
                            {
                                selectedDelivery.Remove(rowExist);
                            }
                            else
                            {
                                rowExist.is_deleted = true;
                            }
                        }
                    }
                }
            }
            HttpContext.Current.Session["SESSION_SELECTED_DELIVERY"] = selectedDelivery;
            HttpContext.Current.Session["SESSION_ISSUE_DETAIL_DELIVERY_NOTE"] = data;

            return "SELECT ALL";
        }
    }
}