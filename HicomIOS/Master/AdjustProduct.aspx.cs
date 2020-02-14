using DevExpress.Web;
using DevExpress.Web.Data;
using HicomIOS.ClassUtil;
using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
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
    public partial class AdjustProduct : MasterDetailPage
    {
        public override string PageName { get { return "Create Adjust Product"; } }

        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }

        private static string guid = Guid.NewGuid().ToString();
        private const string session_item = "_SESSION_ADJUST_ITEM";
        private const string session_detail = "_SESSION_ADJUST_DETAIL";
        private const string session_selected_detail = "_SESSION_ADJUST_SELECTED_DETAIL";
        private const string session_selected_item = "_SESSION_ADJUST_SELECTED_ITEM";
        #region Members
        private int dataId = 0, cloneId = 0;
        public class AdjustProductDetail
        {
            public int id { get; set; }
            public int sort_no { get; set; }
            public int item_id { get; set; }
            public string item_no { get; set; }
            public string item_name { get; set; }
            public string item_type { get; set; }
            public string adjust_type { get; set; }
            public int quantity { get; set; }
            public int max_quantity { get; set; }
            public string quantity_type { get; set; }
            public string unit_code { get; set; }
            public string remark { get; set; }
            public bool is_delete { get; set; }

        }

        public class ProductSparePartDetail
        {
            public bool is_selected { get; set; }
            public int item_id { get; set; }
            public string item_no { get; set; }
            public string item_name { get; set; }
            public string item_type { get; set; }
            public int quantity { get; set; }
            public string unit_code { get; set; }
        }

        public class ProductName
        {
            public string name_tha { get; set; }
            public string name_eng { get; set; }
            public int quantity_reserve { get; set; }
            public int quantity_balance { get; set; }
            public int quantity { get; set; }
        }

        public class QuotationData
        {
            public string quotation_subject { get; set; }
            public int customer_id { get; set; }
            public string project_name { get; set; }
            public string po_no { get; set; }
        }

        List<ProductSparePartDetail> itemDetailList
        {
            get
            {

                if (Session[guid + session_item] == null)
                    Session[guid + session_item] = new List<ProductSparePartDetail>();
                return (List<ProductSparePartDetail>)Session[guid + session_item];
            }
            set
            {
                Session[guid + session_item] = value;
            }
        }
        List<ProductSparePartDetail> selectedItemDetailList
        {
            get
            {

                if (Session[guid + session_item] == null)
                    Session[guid + session_item] = new List<ProductSparePartDetail>();
                return (List<ProductSparePartDetail>)Session[guid + session_item];
            }
            set
            {
                Session[guid + session_item] = value;
            }
        }
        List<AdjustProductDetail> adjustDetailList
        {
            get
            {

                if (Session[guid + session_detail] == null)
                    Session[guid + session_detail] = new List<AdjustProductDetail>();
                return (List<AdjustProductDetail>)Session[guid + session_detail];
            }
            set
            {
                Session[guid + session_detail] = value;
            }
        }
        List<AdjustProductDetail> selectedAdjustDetailList
        {

            get
            {

                if (Session[guid + session_selected_detail] == null)
                    Session[guid + session_selected_detail] = new List<AdjustProductDetail>();
                return (List<AdjustProductDetail>)Session[guid + session_selected_detail];
            }
            set
            {
                Session[guid + session_selected_detail] = value;
            }
        }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            dataId = Convert.ToInt32(Request.QueryString["dataId"]);
            cloneId = Convert.ToInt32(Request.QueryString["cloneId"]);
            hdDataId.Value = Convert.ToString(dataId);
            if (!IsPostBack)
            {
                ClearWorkingSession();
                PrepareData();
                LoadData();
            }
            else
            {
                BindGridView();
                BindGridSelectedItem();
            }
        }
        protected void PrepareData()
        {
            try
            {
                SPlanetUtil.BindASPxComboBox(ref cbbCustomer, DataListUtil.DropdownStoreProcedureName.Customer);
                cbbCustomer.Items.Insert(0, new ListEditItem("ไม่ระบุ", "0"));
                cbbCustomer.Value = "0";
                //SPlanetUtil.BindASPxComboBox(ref cbbProductNoID, DataListUtil.DropdownStoreProcedureName.Product_No);
                SPlanetUtil.BindASPxComboBox(ref cbbQuotation, DataListUtil.DropdownStoreProcedureName.Quotation_Document);
                cbbQuotation.Items.Insert(0, new ListEditItem("ไม่ระบุ", "0"));
                cbbQuotation.Value = "0";

                var dtSource = new DataTable();
                dtSource.Columns.Add("data_value", typeof(string));
                dtSource.Columns.Add("data_text", typeof(string));
                var serviceType = Session["DEPARTMENT_SERVICE_TYPE"].ToString();
                if (serviceType.Contains("P"))
                {
                    dtSource.Rows.Add("P", "Product");
                }
                if (serviceType.Contains("S"))
                {
                    dtSource.Rows.Add("S", "Spare Part");
                }
                cbbProductType.DataSource = dtSource;
                cbbProductType.DataBind();

                cbbProductType.Value = (serviceType.Contains("P") ? "P" : "S");

                gridView.SettingsBehavior.AllowFocusedRow = true;
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
                if (dataId == 0 && cloneId == 0)
                {
                    btnSave.Visible = false;
                    btnCancelAdj.Visible = false;
                }
                else
                {
                    if (dataId != 0)
                    {
                        btnAdjust.Visible = true;
                    }

                    var dsResult = new DataSet();
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = dataId == 0 ? cloneId : dataId },
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID },
                        };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_adjust_item_list_data", arrParm.ToArray());
                        conn.Close();
                    }
                    if (dsResult.Tables.Count > 0)
                    {
                        var header = dsResult.Tables[0].AsEnumerable().FirstOrDefault();
                        if (header != null)
                        {
                            txtSetNo.Value = Convert.IsDBNull(header["adjust_no"]) ? string.Empty : Convert.ToString(header["adjust_no"]);
                            cbbProductType.Value = Convert.IsDBNull(header["adjust_type"]) ? string.Empty : Convert.ToString(header["adjust_type"]);
                            dtAdjustDate.Value = Convert.IsDBNull(header["adjust_date"]) ? string.Empty : Convert.ToDateTime(header["adjust_date"]).ToString("dd/MM/yyyy");
                            txtStore.Value = Convert.IsDBNull(header["adjust_store"]) ? string.Empty : Convert.ToString(header["adjust_store"]);
                            txtSubject.Value = Convert.IsDBNull(header["adjust_subject"]) ? string.Empty : Convert.ToString(header["adjust_subject"]);
                            txtRemark2.Value = Convert.IsDBNull(header["adjust_remark"]) ? string.Empty : Convert.ToString(header["adjust_remark"]);
                            txtProject.Value = Convert.IsDBNull(header["adjust_project"]) ? string.Empty : Convert.ToString(header["adjust_project"]);

                            var quotation_no = Convert.IsDBNull(header["quotation_no"]) ? string.Empty : Convert.ToString(header["quotation_no"]);
                            var quotation_id = Convert.IsDBNull(header["quotation_id"]) ? string.Empty : Convert.ToString(header["quotation_id"]);
                            cbbQuotation.Items.Add(new ListEditItem(quotation_no, quotation_id));

                            cbbQuotation.Text = quotation_no;
                            cbbQuotation.Value = quotation_id;
                            cbbCustomer.Text = Convert.IsDBNull(header["customer_name"]) ? string.Empty : Convert.ToString(header["customer_name"]);
                            cbbCustomer.Value = Convert.IsDBNull(header["customer_id"]) ? string.Empty : Convert.ToString(header["customer_id"]);
                            hdDocStatus.Value = Convert.IsDBNull(header["doc_status"]) ? string.Empty : Convert.ToString(header["doc_status"]);
                            txtPO.Value = Convert.IsDBNull(header["po_no"]) ? string.Empty : Convert.ToString(header["po_no"]);
                        }

                        if (hdDocStatus.Value == "CF")
                        {
                            //btnDraft.Visible = false;
                            //btnProduct.Visible = false;
                            //btnSparePart.Visible = false;
                            //btnSave.Visible = false;
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "Disabled Input", "disableInput()", true);
                            btnCancelAdj.Visible = true;
                            btnAdjust.Visible = false;
                        }
                        else
                        {
                            btnCancelAdj.Visible = false;
                            btnAdjust.Visible = true;
                        }

                        var detail = dsResult.Tables[1].AsEnumerable().ToList();
                        if (detail != null)
                        {
                            adjustDetailList = new List<AdjustProductDetail>();
                            foreach (var row in detail)
                            {
                                var obj = new AdjustProductDetail()
                                {
                                    adjust_type = Convert.IsDBNull(row["adjust_type"]) ? string.Empty : Convert.ToString(row["adjust_type"]),
                                    id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                    is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                    item_name = Convert.IsDBNull(row["item_name"]) ? string.Empty : Convert.ToString(row["item_name"]),
                                    item_id = Convert.IsDBNull(row["item_id"]) ? 0 : Convert.ToInt32(row["item_id"]),
                                    item_no = Convert.IsDBNull(row["item_no"]) ? string.Empty : Convert.ToString(row["item_no"]),
                                    item_type = Convert.IsDBNull(row["item_type"]) ? string.Empty : Convert.ToString(row["item_type"]),
                                    quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]),
                                    max_quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]),
                                    remark = Convert.IsDBNull(row["remark"]) ? string.Empty : Convert.ToString(row["remark"]),
                                    sort_no = Convert.IsDBNull(row["sort_no"]) ? 0 : Convert.ToInt32(row["sort_no"]),
                                    unit_code = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"])
                                };
                                obj.quantity_type = (obj.quantity >= 0 ? "0" : "1");
                                obj.quantity = Math.Abs(obj.quantity);

                                adjustDetailList.Add(obj);
                            }
                        }

                        #region MFG Detail
                        var dataDetailMFG = dsResult.Tables[2].AsEnumerable().ToList();
                        if (dataDetailMFG != null)
                        {
                            issueDetailMFGList = new List<Issue.IssueDetailMFG>();
                            foreach (var row in dataDetailMFG)
                            {
                                issueDetailMFGList.Add(new Issue.IssueDetailMFG()
                                {
                                    id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                    issue_stock_no = Convert.IsDBNull(row["adjust_no"]) ? string.Empty : Convert.ToString(row["adjust_no"]),
                                    product_id = Convert.IsDBNull(row["item_id"]) ? 0 : Convert.ToInt32(row["item_id"]),
                                    issue_stock_detail_id = Convert.IsDBNull(row["adjust_detail_id"]) ? 0 : Convert.ToInt32(row["adjust_detail_id"]),
                                    mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                });
                            }
                        }
                        #endregion

                    }
                    gridView.DataSource = adjustDetailList;
                    gridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void SaveData(string status)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                conn.Open();
                int newID = DataListUtil.emptyEntryID;
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        if (dataId == 0) // Insert Mode
                        {

                            #region  Header
                            //Quotation Header
                            using (SqlCommand cmd = new SqlCommand("sp_adjust_item_header_add", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                DateTime? adjust_date = null;
                                if (!string.IsNullOrEmpty(dtAdjustDate.Value))
                                {
                                    adjust_date = DateTime.ParseExact(dtAdjustDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture); ;
                                }
                                cmd.Parameters.Add("@doc_status", SqlDbType.VarChar, 20).Value = status;
                                cmd.Parameters.Add("@adjust_type", SqlDbType.VarChar, 2).Value = cbbProductType.Value;
                                cmd.Parameters.Add("@adjust_date", SqlDbType.DateTime).Value = adjust_date;
                                cmd.Parameters.Add("@adjust_no", SqlDbType.VarChar, 200).Value = txtSetNo.Value;
                                cmd.Parameters.Add("@adjust_store", SqlDbType.VarChar, 10).Value = txtStore.Value;
                                cmd.Parameters.Add("@adjust_subject", SqlDbType.VarChar, 200).Value = txtSubject.Value;
                                cmd.Parameters.Add("@adjust_remark", SqlDbType.VarChar, 200).Value = txtRemark2.Value;
                                cmd.Parameters.Add("@quotation_id", SqlDbType.Int).Value = cbbQuotation.Value;
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbCustomer.Value;
                                cmd.Parameters.Add("@po_no", SqlDbType.VarChar, 200).Value = txtPO.Value;
                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                cmd.Parameters.Add("@adjust_project", SqlDbType.VarChar).Value = txtProject.Value;

                                newID = Convert.ToInt32(cmd.ExecuteScalar());

                            }
                            #endregion  Header

                            List<SqlParameter> arrParm = new List<SqlParameter>
                            {
                                new SqlParameter("@id", SqlDbType.Int) { Value = newID },
                                new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID },
                            };

                            var lastDataInsert = SqlHelper.ExecuteDataset(tran, "sp_adjust_item_list_data", arrParm.ToArray());

                            var adjustNo = "";
                            if (lastDataInsert != null)
                            {
                                adjustNo = (from t in lastDataInsert.Tables[0].AsEnumerable()
                                           select t.Field<string>("adjust_no")).FirstOrDefault();
                            }

                            #region  Detail
                            // Quotation Detail
                            var oldDetailId = 0;
                            var newDetailId = 0;
                            if (adjustDetailList != null)
                            {
                                foreach (var row in adjustDetailList)
                                {
                                    oldDetailId = row.id;

                                    using (SqlCommand cmd = new SqlCommand("sp_adjust_item_detail_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@header_id", SqlDbType.Int).Value = newID;
                                        cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                        cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = (row.quantity_type == "0" ? row.quantity : row.quantity * -1);
                                        cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.item_id;
                                        cmd.Parameters.Add("@item_no", SqlDbType.VarChar, 20).Value = row.item_no;
                                        cmd.Parameters.Add("@item_name", SqlDbType.VarChar, 200).Value = row.item_name;
                                        cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 2).Value = row.item_type;
                                        cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 10).Value = row.unit_code;
                                        cmd.Parameters.Add("@adjust_type", SqlDbType.VarChar, 5).Value = row.adjust_type;
                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = row.remark;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        newDetailId = Convert.ToInt32(cmd.ExecuteScalar());

                                    }

                                    var childData = (from t in issueDetailMFGList where t.issue_stock_detail_id == oldDetailId select t).ToList();
                                    foreach (var rowDetail in childData)
                                    {
                                        rowDetail.issue_stock_detail_id = newDetailId;
                                    }
                                }

                                foreach (var row in issueDetailMFGList)
                                {

                                    using (SqlCommand cmd = new SqlCommand("sp_adjust_detail_mfg_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@adjust_no", SqlDbType.VarChar, 20).Value = adjustNo;
                                        cmd.Parameters.Add("@adjust_detail_id", SqlDbType.Int).Value = row.issue_stock_detail_id;
                                        cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.product_id;
                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 100).Value = row.mfg_no.Trim();
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('Error','E')", true);
                            }
                            #endregion  Detail                           
                        }
                        else // Edit Mode
                        {
                            newID = dataId;

                            #region Header
                            //Quotation Header
                            DateTime? adjust_date = null;
                            if (!string.IsNullOrEmpty(dtAdjustDate.Value))
                            {
                                adjust_date = DateTime.ParseExact(dtAdjustDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture); ;
                            }
                            if (status == "CC")
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_adjust_item_header_edit", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = dataId;
                                    cmd.Parameters.Add("@doc_status", SqlDbType.VarChar, 20).Value = status;
                                    cmd.Parameters.Add("@adjust_type", SqlDbType.VarChar, 2).Value = cbbProductType.Value;
                                    cmd.Parameters.Add("@adjust_date", SqlDbType.DateTime).Value = adjust_date;
                                    cmd.Parameters.Add("@adjust_no", SqlDbType.VarChar, 200).Value = txtSetNo.Value;
                                    cmd.Parameters.Add("@adjust_store", SqlDbType.VarChar, 10).Value = txtStore.Value;
                                    cmd.Parameters.Add("@adjust_subject", SqlDbType.VarChar, 200).Value = txtSubject.Value;
                                    cmd.Parameters.Add("@adjust_remark", SqlDbType.VarChar, 200).Value = txtRemark2.Value;
                                    cmd.Parameters.Add("@quotation_id", SqlDbType.Int).Value = cbbQuotation.Value;
                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbCustomer.Value;
                                    cmd.Parameters.Add("@po_no", SqlDbType.VarChar, 200).Value = txtPO.Value;
                                    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                    cmd.Parameters.Add("@adjust_project", SqlDbType.VarChar).Value = txtProject.Value;

                                    cmd.ExecuteNonQuery();

                                }
                            }
                            else
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_adjust_item_header_edit", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = dataId;
                                    cmd.Parameters.Add("@doc_status", SqlDbType.VarChar, 20).Value = hdDocStatus.Value == "CF" ? "CF" : status;
                                    cmd.Parameters.Add("@adjust_type", SqlDbType.VarChar, 2).Value = cbbProductType.Value;
                                    cmd.Parameters.Add("@adjust_date", SqlDbType.DateTime).Value = adjust_date;
                                    cmd.Parameters.Add("@adjust_no", SqlDbType.VarChar, 200).Value = txtSetNo.Value;
                                    cmd.Parameters.Add("@adjust_store", SqlDbType.VarChar, 10).Value = txtStore.Value;
                                    cmd.Parameters.Add("@adjust_subject", SqlDbType.VarChar, 200).Value = txtSubject.Value;
                                    cmd.Parameters.Add("@adjust_remark", SqlDbType.VarChar, 200).Value = txtRemark2.Value;
                                    cmd.Parameters.Add("@quotation_id", SqlDbType.Int).Value = cbbQuotation.Value;
                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbCustomer.Value;
                                    cmd.Parameters.Add("@po_no", SqlDbType.VarChar, 200).Value = txtPO.Value;
                                    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                    cmd.Parameters.Add("@adjust_project", SqlDbType.VarChar).Value = txtProject.Value;

                                    cmd.ExecuteNonQuery();

                                }
                            }
                            
                            #endregion  Header


                            #region  Detail
                            // Quotation Detail
                            var oldDetailId = 0;
                            var newDetailId = 0;
                            if (adjustDetailList != null)
                            {
                                foreach (var row in adjustDetailList)
                                {
                                    oldDetailId = row.id;
                                    if (row.id < 0)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_adjust_item_detail_add", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@header_id", SqlDbType.Int).Value = newID;
                                            cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                            cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.item_id;
                                            cmd.Parameters.Add("@item_no", SqlDbType.VarChar, 20).Value = row.item_no;
                                            cmd.Parameters.Add("@item_name", SqlDbType.VarChar, 200).Value = row.item_name;
                                            cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = (row.quantity_type == "0" ? row.quantity : row.quantity * -1);
                                            cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 2).Value = row.item_type;
                                            cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 10).Value = row.unit_code;
                                            cmd.Parameters.Add("@adjust_type", SqlDbType.VarChar, 5).Value = row.adjust_type;
                                            cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = row.remark;
                                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                            newDetailId = Convert.ToInt32(cmd.ExecuteScalar());

                                            var childData = (from t in issueDetailMFGList where t.issue_stock_detail_id == oldDetailId select t).ToList();
                                            foreach (var rowDetail in childData)
                                            {
                                                rowDetail.issue_stock_detail_id = newDetailId;
                                            }
                                        }
                                    }
                                    else if (row.id > 0 && !row.is_delete)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_adjust_item_detail_edit", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                            cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                            // cmd.Parameters.Add("@item_id", SqlDbType.Int, 20).Value = row.item_id;
                                            // cmd.Parameters.Add("@item_name", SqlDbType.VarChar, 200).Value = row.item_name;
                                            // cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 10).Value = row.unit_code;
                                            cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = (row.quantity_type == "0" ? row.quantity : row.quantity * -1);
                                            //cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 2).Value = row.item_type;
                                            cmd.Parameters.Add("@adjust_type", SqlDbType.VarChar, 5).Value = row.adjust_type;
                                            cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = row.remark;
                                            cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = row.is_delete;
                                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                    else
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_adjust_item_detail_edit", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                            cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                            // cmd.Parameters.Add("@item_id", SqlDbType.Int, 20).Value = row.item_id;
                                            // cmd.Parameters.Add("@item_name", SqlDbType.VarChar, 200).Value = row.item_name;
                                            // cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 10).Value = row.unit_code;
                                            cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = (row.quantity_type == "0" ? row.quantity : row.quantity * -1);
                                            //cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 2).Value = row.item_type;
                                            cmd.Parameters.Add("@adjust_type", SqlDbType.VarChar, 5).Value = row.adjust_type;
                                            cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = row.remark;
                                            cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = row.is_delete;
                                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                            cmd.ExecuteNonQuery();

                                        }
                                    }

                                }

                                foreach (var row in issueDetailMFGList)
                                {
                                    if (row.id < 0)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_adjust_detail_mfg_add", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@adjust_no", SqlDbType.VarChar, 20).Value = txtSetNo.Value;
                                            cmd.Parameters.Add("@adjust_detail_id", SqlDbType.Int).Value = row.issue_stock_detail_id;
                                            cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.product_id;
                                            cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 100).Value = row.mfg_no.Trim();
                                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                    else if (row.id > 0 && !row.is_deleted)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_adjust_detail_mfg_edit", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                            cmd.Parameters.Add("@adjust_no", SqlDbType.VarChar, 20).Value = txtSetNo.Value;
                                            cmd.Parameters.Add("@adjust_detail_id", SqlDbType.Int).Value = row.issue_stock_detail_id;
                                            cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = row.product_id;
                                            cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 100).Value = row.mfg_no.Trim();
                                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_adjust_detail_mfg_delete", conn, tran))
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

                            //  Start adjust product
                            if (status == "CF")
                            {
                                var transNo = string.Empty;
                                using (SqlCommand cmd = new SqlCommand("sp_gen_transection_no", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    transNo = Convert.ToString(cmd.ExecuteScalar());
                                }
                                foreach (var row in adjustDetailList)//.Where(t => t.receive_qty_balance != 0))
                                {
                                    if (!row.is_delete)
                                    {
                                        if (row.item_type == "P")
                                        {
                                            var mfgList = (from t in issueDetailMFGList where t.product_id == row.item_id && !t.is_deleted select t).ToList();
                                            foreach (var mfgRow in mfgList)
                                            {
                                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "ADJ";
                                                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = (row.quantity_type == "0" ? "IN" : "OUT");
                                                    cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 1;
                                                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Adj product, MFG No. " + mfgRow.mfg_no;
                                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                    cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtSetNo.Value;// txtPRNo.Value;
                                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbCustomer.Value;
                                                    cmd.ExecuteNonQuery();
                                                }

                                                if (row.quantity_type == "0")
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("sp_product_mfg_add", conn, tran))
                                                    {
                                                        cmd.CommandType = CommandType.StoredProcedure;

                                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;
                                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = mfgRow.mfg_no;
                                                        cmd.Parameters.Add("@adjust_no", SqlDbType.VarChar, 20).Value = txtSetNo.Value;
                                                        cmd.Parameters.Add("@receive_date", SqlDbType.DateTime).Value = DateTime.Now;
                                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                                        cmd.ExecuteNonQuery();

                                                    }
                                                }
                                                else
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("sp_product_mfg_delete_by_mfg", conn, tran))
                                                    {
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = mfgRow.mfg_no;
                                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                        cmd.ExecuteNonQuery();

                                                    }
                                                }
                                            }
                                        }
                                        else {
                                            using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "ADJ";
                                                cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = (row.quantity_type == "0" ? "IN" : "OUT");
                                                cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                cmd.Parameters.Add("@qty", SqlDbType.Int).Value = Math.Abs(row.quantity);
                                                cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Adj product";
                                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtSetNo.Value;// txtPRNo.Value;
                                                cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbCustomer.Value;
                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                            }
                            else if (status == "CC")
                            {
                                var transNo = string.Empty;
                                using (SqlCommand cmd = new SqlCommand("sp_gen_transection_no", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    transNo = Convert.ToString(cmd.ExecuteScalar());
                                }

                                //  Set cancel flag for this ref doc
                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_cancel", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                    cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID;
                                    cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtSetNo.Value;// txtPRNo.Value;
                                    cmd.ExecuteNonQuery();
                                }

                                foreach (var row in adjustDetailList)//.Where(t => t.receive_qty_balance != 0))
                                {
                                    if (!row.is_delete)
                                    {
                                        if (row.item_type == "P")
                                        {
                                            var mfgList = (from t in issueDetailMFGList where t.product_id == row.item_id && !t.is_deleted select t).ToList();
                                            foreach (var mfgRow in mfgList)
                                            {
                                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "ADJ";
                                                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = (row.quantity_type == "0" ? "OUT" : "IN");   //  Swap IN, OUT
                                                    cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 1;
                                                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Return adj product, MFG No. " + mfgRow.mfg_no;
                                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                    cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtSetNo.Value;// txtPRNo.Value;
                                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbCustomer.Value;
                                                    cmd.Parameters.Add("@is_cancel", SqlDbType.Bit).Value = true;
                                                    cmd.ExecuteNonQuery();
                                                }

                                                if (row.quantity_type == "0")
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("sp_product_mfg_delete_by_mfg ", conn, tran))
                                                    {
                                                        cmd.CommandType = CommandType.StoredProcedure;

                                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = mfgRow.mfg_no;
                                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                        
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                                else
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("sp_product_mfg_edit_by_mfg ", conn, tran))
                                                    {
                                                        cmd.CommandType = CommandType.StoredProcedure;

                                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = mfgRow.mfg_no;
                                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "ADJ";
                                                cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = (row.quantity_type == "0" ? "OUT" : "IN");   //  Swap IN, OUT
                                                cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                cmd.Parameters.Add("@qty", SqlDbType.Int).Value = Math.Abs(row.quantity);
                                                cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Return adj product";
                                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtSetNo.Value;// txtPRNo.Value;
                                                cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbCustomer.Value;
                                                cmd.Parameters.Add("@is_cancel", SqlDbType.Bit).Value = true;
                                                cmd.ExecuteNonQuery();
                                            }
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
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('" + ex.Message + "','E')", true);
                        //throw ex;
                    }
                    finally
                    {
                        if (!conn.State.Equals(ConnectionState.Closed))
                        {

                            tran.Commit();
                            tran.Dispose();
                            conn.Close();

                            Response.Redirect("AdjustProduct.aspx?dataId=" + newID);
                        }
                    }
                }
            }
        }
        private void ClearWorkingSession()
        {
            try
            {
                Session.Remove(guid + session_item);
                Session.Remove(guid + session_detail);
                Session.Remove(guid + session_selected_detail);
                Session.Remove(guid + session_selected_item);
                Session.Remove("SESSION_MFG_DETAIL_ADJ");
                Session.Remove("SESSION_MFG_DETAIL_ADJ_TEMP");
                Session.Remove("SESSION_ISSUE_DETAIL_MFG_ADJ");
                Session.Remove("SESSION_SELECTED_ISSUE_DETAIL_MFG_ADJ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindGridView()
        {
            gridView.DataSource = adjustDetailList.Where(t => !t.is_delete).OrderBy(t => t.sort_no).ToList();
            gridView.DataBind();
        }
        protected void gridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridView.DataSource = adjustDetailList.Where(t => !t.is_delete).OrderBy(t => t.sort_no).ToList();
        }

        protected void gridViewSelectedItem_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.ToString().Contains("Search"))
            {
                var splitStr = e.Parameters.ToString().Split('|');
                string searchText = string.Empty;
                if (splitStr.Length > 0)
                {
                    searchText = splitStr[1];

                    gridViewSelectedItem.DataSource = selectedItemDetailList.Where(t => t.item_name.ToUpper().Contains(searchText.ToUpper())
                                                                                    || t.item_no.ToUpper().Contains(searchText.ToUpper())).ToList();
                    gridViewSelectedItem.DataBind();
                }
            }
            else
            {
                gridViewSelectedItem.DataSource = selectedItemDetailList;
                gridViewSelectedItem.DataBind();
            }
            gridViewSelectedItem.PageIndex = 0;
        }

        protected void gridViewSelectedItem_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            ASPxCheckBox checkBox = gridViewSelectedItem.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkBox") as ASPxCheckBox;
            if (checkBox != null)
            {
                if (e.DataColumn.FieldName == "is_selected")
                {
                    if (selectedItemDetailList != null)
                    {
                        var row = selectedItemDetailList.Where(t => t.item_no == e.KeyValue.ToString()).FirstOrDefault();
                        if (row != null)
                        {
                            checkBox.Checked = row.is_selected;
                        }
                    }
                }
            }
        }
        private void BindGridSelectedItem()
        {
            string searchText = txtSearchBoxData.Value;
            if (string.IsNullOrEmpty(searchText))
            {
                gridViewSelectedItem.DataSource = selectedItemDetailList;
                gridViewSelectedItem.DataBind();
            }
            else
            {
                gridViewSelectedItem.DataSource = selectedItemDetailList.Where(t => t.item_name.ToUpper().Contains(searchText.ToUpper())
                                                                                    || t.item_no.ToUpper().Contains(searchText.ToUpper())).ToList();
                gridViewSelectedItem.DataBind();
            }
        }
        [WebMethod]
        public static string PopupItemDetail(string item_type)
        {
            try
            {
                DataSet dsResult = new DataSet();
                var productList = new List<ProductSparePartDetail>();
                if (item_type == "P")
                {
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
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_list_popup", arrParm.ToArray());
                        conn.Close();
                    }

                    if (dsResult.Tables.Count > 0)
                    {
                        foreach (var row in dsResult.Tables[0].AsEnumerable())
                        {
                            productList.Add(new ProductSparePartDetail()
                            {
                                is_selected = false,
                                item_id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                item_name = Convert.IsDBNull(row["product_name_tha"]) ? string.Empty : Convert.ToString(row["product_name_tha"]),
                                item_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                item_type = "P",
                                quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]),
                                unit_code = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]).ToString(),

                            });
                        }
                    }
                }
                else if (item_type == "S")
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                             new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = string.Empty },
                        };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_list", arrParm.ToArray());
                        conn.Close();

                        if (dsResult != null)
                        {
                            foreach (var row in dsResult.Tables[0].AsEnumerable())
                            {
                                productList.Add(new ProductSparePartDetail()
                                {
                                    is_selected = false,
                                    item_id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                    item_no = Convert.IsDBNull(row["part_no"]) ? string.Empty : Convert.ToString(row["part_no"]),
                                    item_name = Convert.IsDBNull(row["part_name_tha"]) ? string.Empty : Convert.ToString(row["part_name_tha"]),
                                    unit_code = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]),
                                    quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]),
                                    item_type = "S"
                                });

                            }
                        }
                    }

                }

                HttpContext.Current.Session[guid + session_item] = productList;

                return "ITEMDETAIL";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [WebMethod]
        public static string SelectedItem(int key, bool isSelected)
        {
            var adjustDetailList = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_detail];
            var selectedItemList = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_selected_detail];
            var productList = (List<ProductSparePartDetail>)HttpContext.Current.Session[guid + session_item];
            if (productList != null)
            {
                var item = productList.Where(t => t.item_id == key).FirstOrDefault();
                if (item != null)
                {
                    if (selectedItemList == null)
                    {
                        selectedItemList = new List<AdjustProductDetail>();
                    }
                    if (isSelected)
                    {
                        var count = 0;
                        if (adjustDetailList != null)
                        {
                            count = adjustDetailList.Count;
                        }
                        count += selectedItemList.Count;

                        selectedItemList.Add(new AdjustProductDetail()
                        {
                            adjust_type = null,
                            id = (count + 1) * -1,
                            is_delete = false,
                            item_name = item.item_name,
                            item_id = item.item_id,
                            item_no = item.item_no,
                            item_type = item.item_type,
                            quantity = 0,//item.quantity,
                            max_quantity = item.quantity,
                            quantity_type = "0",
                            unit_code = item.unit_code,
                            remark = string.Empty,
                            sort_no = (count + 1),//(selectedItemList.Count + 1)

                        });
                        item.is_selected = true;
                    }
                    else
                    {
                        var checkExist = selectedItemList.Where(t => t.item_no == item.item_no).FirstOrDefault();
                        if (checkExist != null)
                        {
                            selectedItemList.Remove(checkExist);
                        }
                        item.is_selected = false;
                    }
                }
            }
            HttpContext.Current.Session[guid + session_selected_detail] = selectedItemList;
            HttpContext.Current.Session[guid + session_item] = productList;
            return "SELECETD";
        }

        [WebMethod]
        public static string SubmitSelectedItem()
        {
            var selectedItemList = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_selected_detail];
            var adjustDetailList = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_detail];

            if (selectedItemList != null)
            {

                if (adjustDetailList == null)
                {
                    adjustDetailList = new List<AdjustProductDetail>();
                }
                foreach (var row in selectedItemList)
                {
                    var checkExist = adjustDetailList.Where(t => t.item_no == row.item_no && !t.is_delete).FirstOrDefault();
                    if (checkExist != null)
                    {
                        //checkExist.quantity += 1;
                    }
                    else
                    {
                        adjustDetailList.Add(row);
                    }
                }
                /*var deletedItem = new List<AdjustProductDetail>();
                deletedItem.AddRange(adjustDetailList);
                foreach (var row in deletedItem)
                {
                    var checkDeleted = selectedItemList.Where(t => t.item_no == row.item_no).FirstOrDefault();
                    if (checkDeleted == null)
                    {
                        var itemDeleted = adjustDetailList.Where(t => t.id == row.id).FirstOrDefault();
                        if (itemDeleted.id < 0)
                        {
                            adjustDetailList.Remove(itemDeleted);
                        }
                        else
                        {
                            itemDeleted.is_delete = true;
                        }
                    }
                }*/

            }
            HttpContext.Current.Session[guid + session_selected_detail] = null;
            HttpContext.Current.Session[guid + session_detail] = adjustDetailList;
            return "SUBMIT";
        }

        [WebMethod]
        public static string CheckedAdjust(int key, string value)
        {
            var adjustDetailList = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_detail];

            if (adjustDetailList != null)
            {
                var row = adjustDetailList.Where(t => t.id == key).FirstOrDefault();
                if (row != null)
                {
                    row.adjust_type = value;
                }
            }
            HttpContext.Current.Session[guid + session_detail] = adjustDetailList;
            return "CHECKEDADJUST";
        }
        [WebMethod]
        public static string ChangeRemark(int key, string value)
        {
            var adjustDetailList = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_detail];

            if (adjustDetailList != null)
            {
                var row = adjustDetailList.Where(t => t.id == key).FirstOrDefault();
                if (row != null)
                {
                    row.remark = value;
                }
            }
            HttpContext.Current.Session[guid + session_detail] = adjustDetailList;
            return "CHECKEDADJUST";
        }
        [WebMethod]
        public static string ChangeQuantity(int key, int value)
        {
            var adjustDetailList = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_detail];

            if (adjustDetailList != null)
            {
                var row = adjustDetailList.Where(t => t.id == key).FirstOrDefault();
                if (row != null)
                {
                    row.quantity = value;
                }
            }
            HttpContext.Current.Session[guid + session_detail] = adjustDetailList;
            return "QUANTITY";
        }
        [WebMethod]
        public static string ChangeQuantityType(int key, string value)
        {
            var adjustDetailList = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_detail];

            if (adjustDetailList != null)
            {
                var row = adjustDetailList.Where(t => t.id == key).FirstOrDefault();
                if (row != null)
                {
                    row.quantity_type = value;

                    //  Clear temp
                    HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ_TEMP"] = null;
                }
            }
            HttpContext.Current.Session[guid + session_detail] = adjustDetailList;
            return "QUANTITY";
        }
        [WebMethod]
        public static string ChangeSortno(int key, int value)
        {
            var adjustDetailList = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_detail];

            if (adjustDetailList != null)
            {
                var row = adjustDetailList.Where(t => t.id == key).FirstOrDefault();
                if (row != null)
                {
                    row.sort_no = value;
                }
            }
            HttpContext.Current.Session[guid + session_detail] = adjustDetailList;
            return "QUANTITY";
        }

        protected void gridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxRadioButtonList rdoAdjust = gridView.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "rdoAdjust") as ASPxRadioButtonList;
                ASPxSpinEdit txtQuantity = gridView.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "txtQuantity") as ASPxSpinEdit;
                ASPxSpinEdit txtSortNo = gridView.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "txtSortNo") as ASPxSpinEdit;
                ASPxTextBox txtRemark = gridView.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "txtRemark") as ASPxTextBox;
                Control btnMFG = gridView.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "btnMFG");
                if (rdoAdjust != null)
                {
                    if (e.DataColumn.FieldName == "quantity")
                    {
                        if (adjustDetailList != null)
                        {
                            var row = adjustDetailList.Where(t => t.id == Convert.ToInt32(e.KeyValue)).FirstOrDefault();
                            if (row != null)
                            {
                                rdoAdjust.SelectedIndex = (row.quantity_type == "0" ? 0 : 1);
                            }
                            /*if (hdDocStatus.Value == "CF")
                            {
                                rdoAdjust.Enabled = false;
                            }*/
                        }
                    }
                }
                if (txtQuantity != null)
                {
                    if (e.DataColumn.FieldName == "quantity")
                    {
                        if (adjustDetailList != null)
                        {
                            var row = adjustDetailList.Where(t => t.id == Convert.ToInt32(e.KeyValue)).FirstOrDefault();
                            if (row != null)
                            {
                                txtQuantity.Value = row.quantity;
                                //txtQuantity.MaxValue = (row.quantity_type == "1" ? row.max_quantity : 0);
                            }
                        }
                    }
                }
                if (txtSortNo != null)
                {
                    if (e.DataColumn.FieldName == "sort_no")
                    {
                        if (adjustDetailList != null)
                        {
                            var row = adjustDetailList.Where(t => t.id == Convert.ToInt32(e.KeyValue)).FirstOrDefault();
                            if (row != null)
                            {
                                txtSortNo.Value = row.sort_no;
                            }
                        }
                    }
                }
                if (txtRemark != null)
                {
                    if (e.DataColumn.FieldName == "remark")
                    {
                        if (adjustDetailList != null)
                        {
                            var row = adjustDetailList.Where(t => t.id == Convert.ToInt32(e.KeyValue)).FirstOrDefault();
                            if (row != null)
                            {
                                txtRemark.Value = row.remark;
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
        public static string DeleteAdjustItem(int id)
        {
            var adjustDetailList = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_detail];
            var issueMFGDetail = (List<Issue.IssueDetailMFG>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ADJ"];
            if (adjustDetailList != null)
            {
                var row = adjustDetailList.Where(t => t.id == id).FirstOrDefault();
                if (row != null)
                {
                    if (row.id < 0)
                    {
                        adjustDetailList.Remove(row);
                        if (issueMFGDetail != null) // clear mfg
                        {
                            foreach (var item in (from t in issueMFGDetail where t.issue_stock_detail_id == Convert.ToInt32(id) select t).ToList())
                            {
                                issueMFGDetail.Remove(item);
                            }
                        }
                    }
                    else
                    {
                        row.is_delete = true;
                        if (issueMFGDetail != null) // clear mfg
                        {
                            foreach (var item in (from t in issueMFGDetail where t.issue_stock_detail_id == Convert.ToInt32(id) select t).ToList())
                            {
                                item.is_deleted = true;
                            }
                        }
                    }
                }
            }
            HttpContext.Current.Session[guid + session_detail] = adjustDetailList;
            HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ADJ"] = issueMFGDetail;
            return "QUANTITY";
        }

        [WebMethod]
        public static string CheckProduct()
        {
            string msgError = "";
            var adjustDetailList = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_detail];
            //  Check product
            var productNos = "";
            var partNos = "";
            foreach (var row in adjustDetailList)
            {
                if (row.item_type == "P")
                {
                    if (productNos != "")
                    {
                        productNos += ",";
                    }
                    productNos += row.item_no;

                    //Check mfg
                    var issueDetailMFGList = (List<Issue.IssueDetailMFG>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ADJ"];
                    var childData = (from t in issueDetailMFGList where t.issue_stock_detail_id == row.id select t).ToList();
                    if (childData.Count == 0)
                    {
                        msgError += "กรุณากรอก MFG No. ของ " + row.item_no + "\n";
                    }
                }
                else
                {
                    if (partNos != "")
                    {
                        partNos += ",";
                    }
                    partNos += row.item_no;
                }
            }

            if (msgError != "")
            {
                return msgError;
            }

            try
            {
                //  For product
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@item_type", SqlDbType.VarChar, 1) { Value = "P"},
                            new SqlParameter("@item_no", SqlDbType.VarChar, 100) { Value = productNos },
                        };
                    conn.Open();
                    var dsQuataionData = SqlHelper.ExecuteDataset(conn, "sp_adjust_product_check_qty", arrParm.ToArray());
                    conn.Close();

                    if (dsQuataionData.Tables.Count > 0)
                    {
                        if (dsQuataionData.Tables[0].Rows.Count > 0)
                        {
                            foreach (var row in dsQuataionData.Tables[0].AsEnumerable())
                            {
                                var qty = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]);
                                var item_no = Convert.IsDBNull(row["item_no"]) ? string.Empty : Convert.ToString(row["item_no"]);

                                var obj = adjustDetailList.Where(t => t.item_no == item_no).FirstOrDefault();
                                if (obj != null)
                                {
                                    if (obj.quantity_type == "1" && obj.quantity > qty)
                                    {
                                        if (msgError != "")
                                        {
                                            msgError += "\n";
                                        }
                                        msgError += (item_no + " จำนวนไม่เพียงพอ");
                                    }
                                }
                            }
                        }
                    }
                }

                //  For part
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@item_type", SqlDbType.VarChar, 1) { Value = "S"},
                            new SqlParameter("@item_no", SqlDbType.VarChar, 100) { Value = partNos },
                        };
                    conn.Open();
                    var dsQuataionData = SqlHelper.ExecuteDataset(conn, "sp_adjust_product_check_qty", arrParm.ToArray());
                    conn.Close();

                    if (dsQuataionData.Tables.Count > 0)
                    {
                        if (dsQuataionData.Tables[0].Rows.Count > 0)
                        {
                            foreach (var row in dsQuataionData.Tables[0].AsEnumerable())
                            {
                                var qty = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]);
                                var item_no = Convert.IsDBNull(row["item_no"]) ? string.Empty : Convert.ToString(row["item_no"]);

                                var obj = adjustDetailList.Where(t => t.item_no == item_no).FirstOrDefault();
                                if (obj != null)
                                {
                                    if (obj.quantity_type == "1" && obj.quantity > qty)
                                    {
                                        if (msgError != "")
                                        {
                                            msgError += "\n";
                                        }
                                        msgError += (item_no + " จำนวนไม่เพียงพอ");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return msgError;
        }

        [WebMethod]
        public static string StartAdjustProduct(int id)
        {
            var adjustDetailList = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_detail];

            return "QUANTITY " + id;
        }

        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            SaveData("DR");
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            SaveData("CF");
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            SaveData("CC");
        }
        
        [WebMethod]
        public static string ValidateData()
        {
            var msgError = string.Empty;
            var adjustDetailList = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_detail];
            if (adjustDetailList != null)
            {
                int countRows = adjustDetailList.Where(t => !t.is_delete).Count();
                if (countRows == 0)
                {
                    msgError = "กรุณาทำรายการอย่างน้อย 1 รายการ";
                }
                /*else
                {
                    foreach (var row in adjustDetailList)
                    {
                        if (row.quantity == 0)
                        {
                            msgError += "กรุณาใส่จำนวน สินค้า " + row.item_name + "\n";
                        }
                    }
                }*/
            }
            return msgError;
        }
        [WebMethod]
        public static ProductName GetProductName(string id)
        {
            var data = new ProductName();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = ""},
                            new SqlParameter("@product_no", SqlDbType.VarChar,50) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },
                        };
                    conn.Open();
                    var dsQuataionData = SqlHelper.ExecuteDataset(conn, "sp_product_list", arrParm.ToArray());
                    conn.Close();

                    if (dsQuataionData.Tables.Count > 0)
                    {
                        if (dsQuataionData.Tables[0].Rows.Count > 0)
                        {
                            var row = (from t in dsQuataionData.Tables[0].AsEnumerable() select t).FirstOrDefault();
                            if (row != null)
                            {
                                data.name_eng = Convert.IsDBNull(row["product_name_eng"]) ? string.Empty : Convert.ToString(row["product_name_eng"]);
                                data.name_tha = Convert.IsDBNull(row["product_name_tha"]) ? string.Empty : Convert.ToString(row["product_name_tha"]);
                                data.quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]);
                                data.quantity_reserve = Convert.IsDBNull(row["quantity_reserve"]) ? 0 : Convert.ToInt32(row["quantity_reserve"]);
                                data.quantity_balance = Convert.IsDBNull(row["quantity_balance"]) ? 0 : Convert.ToInt32(row["quantity_balance"]);


                            }
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

        [WebMethod]
        public static QuotationData GetQuotationData(string id)
        {
            var headerData = new QuotationData();
            try
            {
                var dsResult = new DataSet();
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },
                        };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_quotation_list_data_aj", arrParm.ToArray());
                    conn.Close();
                }
                if (dsResult.Tables.Count > 0)
                {
                    // Quotation Header
                    var data = dsResult.Tables[0].AsEnumerable().FirstOrDefault();
                    if (data != null)
                    {
                        var quotation_type = Convert.IsDBNull(data["quotation_type"]) ? "" : Convert.ToString(data["quotation_type"]);
                        var project_name = Convert.IsDBNull(data["project_name"]) ? "" : Convert.ToString(data["project_name"]);
                        var quotation_subject = "";
                        if (quotation_type != "P")
                        {
                            var model = Convert.IsDBNull(data["quotation_type"]) ? "" : Convert.ToString(data["quotation_type"]);
                            var mfg_no = Convert.IsDBNull(data["mfg"]) ? "" : Convert.ToString(data["mfg"]);
                            var machine = Convert.IsDBNull(data["machine"]) ? "" : Convert.ToString(data["machine"]);
                            var hour = Convert.IsDBNull(data["hour_amount"]) ? "" : Convert.ToString(data["hour_amount"]);
                            var pressure = Convert.IsDBNull(data["pressure"]) ? "" : Convert.ToString(data["pressure"]);
                            var location1 = Convert.IsDBNull(data["location1"]) ? "" : Convert.ToString(data["location1"]);
                            var location2 = Convert.IsDBNull(data["location2"]) ? "" : Convert.ToString(data["location2"]);     

                            if (model != "")
                            {
                                quotation_subject = "MODEL : " + model;
                            }
                            if (mfg_no != "")
                            {
                                if (quotation_subject != "")
                                {
                                    quotation_subject += ", ";
                                }
                                quotation_subject = "MFG No : " + mfg_no;
                            }
                            if (machine != "")
                            {
                                if (quotation_subject != "")
                                {
                                    quotation_subject += ", ";
                                }
                                quotation_subject = mfg_no;
                            }
                            if (pressure != "")
                            {
                                if (quotation_subject != "")
                                {
                                    quotation_subject += ", ";
                                }
                                quotation_subject = " Pressure : " + pressure;
                            }
                            if (location1 != "")
                            {
                                if (quotation_subject != "")
                                {
                                    quotation_subject += ", ";
                                }
                                quotation_subject = location1;
                            }
                            if (location2 != "")
                            {
                                if (quotation_subject != "")
                                {
                                    quotation_subject += ", ";
                                }
                                quotation_subject = location2;
                            }
                        }

                        headerData.customer_id = Convert.IsDBNull(data["customer_id"]) ? 0 : Convert.ToInt32(data["customer_id"]);
                        headerData.quotation_subject = quotation_subject;
                        headerData.project_name = project_name;
                        headerData.po_no = Convert.IsDBNull(data["po_no"]) ? "" : Convert.ToString(data["po_no"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return headerData;
        }

        [WebMethod]
        public static void MoveUpListModel(string id)
        {
            List<AdjustProductDetail> data = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_detail];
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
            HttpContext.Current.Session[guid + session_detail] = data;
        }
        [WebMethod]
        public static void MoveDownListModel(string id)
        {
            List<AdjustProductDetail> data = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_detail];
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
            HttpContext.Current.Session[guid + session_detail] = data;
        }

        List<Issue.IssueDetailMFG> issueDetailMFGList
        {
            get
            {
                if (Session["SESSION_ISSUE_DETAIL_MFG_ADJ"] == null)
                    Session["SESSION_ISSUE_DETAIL_MFG_ADJ"] = new List<Issue.IssueDetailMFG>();
                return (List<Issue.IssueDetailMFG>)Session["SESSION_ISSUE_DETAIL_MFG_ADJ"];
            }
            set
            {
                Session["SESSION_ISSUE_DETAIL_MFG_ADJ"] = value;
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
                if (Session["SESSION_MFG_DETAIL_ADJ_TEMP"] == null)
                    Session["SESSION_MFG_DETAIL_ADJ_TEMP"] = new List<MFGDetail>();
                return (List<MFGDetail>)Session["SESSION_MFG_DETAIL_ADJ_TEMP"];
            }
            set
            {
                Session["SESSION_MFG_DETAIL_ADJ_TEMP"] = value;
            }
        }
        List<Issue.IssueDetailMFG> selectedIssueDetailMFGList
        {
            get
            {
                if (Session["SESSION_SELECTED_ADJ_DETAIL_MFG"] == null)
                    Session["SESSION_SELECTED_ADJ_DETAIL_MFG"] = new List<Issue.IssueDetailMFG>();
                return (List<Issue.IssueDetailMFG>)Session["SESSION_SELECTED_ADJ_DETAIL_MFG"];
            }
            set
            {
                Session["SESSION_SELECTED_ADJ_DETAIL_MFG"] = value;
            }
        }

        public class MFGDetail
        {
            public int id { get; set; }
            public bool is_selected { get; set; }
            public string mfg_no { get; set; }
            public int item_id { get; set; }
            public int adj_detail_id { get; set; }
            public string pr_no { get; set; }
            public bool is_delete { get; set; }
        }

        protected void gridViewMFG_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridViewMFG.DataSource = (from t in mfgDetailListTemp select t).ToList(); ;
            gridViewMFG.DataBind();
        }

        protected void BindGridMFGDetail()
        {
            gridViewMFG.DataSource = (from t in mfgDetailListTemp select t).ToList();
            gridViewMFG.DataBind();
        }

        protected void gridViewMFG_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxCheckBox checkBox = gridViewMFG.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkProductMFG") as ASPxCheckBox;
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

                            //if (hdDocumentFlag.Value == "CF" || hdDocumentFlag.Value == "CP")
                            //{
                            //    checkBox.Enabled = false;
                            //}
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
        public static void GetMFGDetail(string id, string adjust_no)
        {
            var mfgDetailData = (List<MFGDetail>)HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ"];
            var issueMFGDetail = (List<Issue.IssueDetailMFG>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ADJ"];
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
            if (dsResult != null)//adjust_no
            {
                foreach (var row in (from t in dsResult.Tables[0].AsEnumerable()
                                     where t.Field<string>("issue_no") == "" || t.Field<string>("issue_no") == adjust_no
                                     select t).ToList())
                {
                    mfgDetailData.Add(new MFGDetail()
                    {
                        is_selected = false,
                        mfg_no = Convert.ToString(row["mfg_no"]),
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
                                mfg_no = row.mfg_no
                            });
                        }
                        else
                        {
                            checkExist.is_selected = true;
                        }
                    }
                    else
                    {
                        var checkExist = (from t in mfgDetailData where t.mfg_no == row.mfg_no select t).FirstOrDefault();
                        if (checkExist != null)
                        {
                            checkExist.is_selected = true;
                        }
                    }
                }
            }
            HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ"] = mfgDetailData;
            HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ_TEMP"] = mfgDetailData;
            HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ADJ"] = issueMFGDetail;
            HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_MFG_ADJ"] = issueMFGDetail;
        }

        [WebMethod]
        public static List<Issue.IssueDetailMFG> AddMFGProduct(string mfg_no, bool isSelected, string product_id, string adj_detail_id)
        {
            var data = (List<MFGDetail>)HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ_TEMP"];
            var selectedisssueMFGDetail = (List<Issue.IssueDetailMFG>)HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_MFG_ADJ"];

            if (data != null)
            {
                var selectedRow = (from t in data
                                   where t.mfg_no == mfg_no
                                   select t).FirstOrDefault();
                if (selectedRow != null)
                {
                    var borrowMFGDetailData = (List<Issue.IssueDetailMFG>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ADJ"];
                    var count = borrowMFGDetailData == null ? 0 : borrowMFGDetailData.Count;

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
                            selectedisssueMFGDetail.Add(new Issue.IssueDetailMFG()
                            {
                                id = (count + 1) * -1,
                                product_id = Convert.ToInt32(product_id),
                                mfg_no = mfg_no,
                                issue_stock_detail_id = Convert.ToInt32(adj_detail_id),
                                is_deleted = false,
                                qty = 1,
                                air_end_warranry = 1,
                                unit_warranty = 1
                            });
                        }
                    }
                    else // Add New เท่านั้น
                    {
                        selectedisssueMFGDetail = new List<Issue.IssueDetailMFG>();
                        selectedRow.is_selected = true;
                        selectedisssueMFGDetail.Add(new Issue.IssueDetailMFG()
                        {
                            id = (count + 1) * -1,
                            product_id = Convert.ToInt32(product_id),
                            mfg_no = mfg_no,
                            issue_stock_detail_id = Convert.ToInt32(adj_detail_id),
                            is_deleted = false,
                            qty = 1,
                            unit_warranty = 1,
                            air_end_warranry = 1
                        });
                    }
                }
            }
            HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ_TEMP"] = data;
            HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_MFG_ADJ"] = selectedisssueMFGDetail;
            return selectedisssueMFGDetail;

        }

        [WebMethod]
        public static string SubmitMFGProduct(int adj_detail_id)
        {
            string returnMessage = string.Empty;
            var issueMFGDetail = (List<Issue.IssueDetailMFG>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ADJ"];
            var selectedissueMFGDetail = (List<Issue.IssueDetailMFG>)HttpContext.Current.Session["SESSION_SELECTED_ISSUE_DETAIL_MFG_ADJ"];
            var adjustDetailList = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_detail];
            var mFGDetails = (List<MFGDetail>)HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ_TEMP"];
            var checkItem = (from t in adjustDetailList where t.id == adj_detail_id && !t.is_delete select t).FirstOrDefault();
            if (checkItem != null)
            {
                //if (checkItem.qty != selectedissueMFGDetail.Where(t => !t.is_deleted && t.issue_stock_detail_id == issue_detail_id).Count())
                if (checkItem.quantity != mFGDetails.Where(t => t.is_selected).Count())
                {
                    //returnMessage += "สินค้า " + checkItem.product_no + " กรุณาเลือก MFG ให้ตรงตามจำนวน ( Qty = " + checkItem.qty + ": MFG = " + selectedissueMFGDetail.Where(t => !t.is_deleted && t.issue_stock_detail_id == issue_detail_id).Count() + " )\n";
                    returnMessage += "สินค้า " + checkItem.item_no + " จำนวน MFG (" + mFGDetails.Where(t => t.is_selected).Count() + ") ไม่ตรงกับจำนวนที่รับทั้งหมด (" + checkItem.quantity + ")\n";
                }
            }
            if (string.IsNullOrEmpty(returnMessage))
            {
                if (selectedissueMFGDetail != null)
                {
                    for (int i = 0; i < selectedissueMFGDetail.Count; i++)
                    {
                        var detail = selectedissueMFGDetail[i];
                        if (issueMFGDetail == null)
                        {
                            issueMFGDetail = new List<Issue.IssueDetailMFG>();
                            issueMFGDetail.Add(detail);
                        }
                        else {
                            for (int j = 0; j < issueMFGDetail.Count; j++)
                            {
                                if (detail.id == issueMFGDetail[j].id)
                                {
                                    issueMFGDetail[j] = detail;
                                }
                                else
                                {
                                    issueMFGDetail.Add(detail);
                                    break;
                                }
                            }
                        }
                    }
                }
                HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ"] = mFGDetails;
                HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ADJ"] = issueMFGDetail;
                HttpContext.Current.Session["SESSION_SELECTED_ADJ_DETAIL_MFG_ADJ"] = null;
            }
            return returnMessage;
        }

        protected void gridMFG_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridMFG.DataSource = (from t in mfgDetailListTemp where !t.is_delete select t).ToList();
            gridMFG.DataBind();
            //if (hdDocumentStatus.Value == "CP")
            //{
            //    gridMFG.Columns["id"].Visible = false;
            //}

        }
        protected void BindGridMFGCustomDetail()

        {
            gridMFG.DataSource = (from t in mfgDetailListTemp where !t.is_delete select t).ToList();
            gridMFG.DataBind();
        }
        [WebMethod]
        public static AdjustProductDetail GetCustomMFGDetail(int id)
        {
            try
            {
                List<AdjustProductDetail> adjDetailData = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_detail];
                
                var adjMFGDetailData = (List<Issue.IssueDetailMFG>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ADJ"];
                if (adjMFGDetailData != null)
                {
                    List<MFGDetail> mfgDetailData = new List<MFGDetail>();
                    var row = (from t in adjMFGDetailData where t.product_id == Convert.ToInt32(id) select t).ToList();
                    foreach (var item in row)
                    {
                        mfgDetailData.Add(new MFGDetail()
                        {
                            id = item.id,
                            is_selected = true,
                            mfg_no = item.mfg_no,
                            adj_detail_id = item.issue_stock_detail_id
                        });
                    }

                    HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ_TEMP"] = mfgDetailData;
                }
                if (adjDetailData != null)
                {
                    var row = (from t in adjDetailData where t.item_id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (row != null)
                    {
                        return row;
                    }

                }

                return new AdjustProductDetail();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static string AddMFG(string mfg_no, int product_id, int adj_detail_id, int maxQty)
        {
            var returnData = string.Empty;

            var mfgProductList = (List<MFGDetail>)HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ_TEMP"];
            var list = (from t in mfgProductList
                        where !t.is_delete
                        select t).ToList();
            if (list.Count == maxQty)
            {
                return "ไม่สามารถกรอก MFG เกินจำนวนสินค้า (" + maxQty + ") ได้";
            }

            var borrowMFGDetailData = (List<Issue.IssueDetailMFG>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ADJ"];
            var count = borrowMFGDetailData == null ? 0 : borrowMFGDetailData.Count;
            if (mfgProductList != null)
            {
                var checkExist = (from t in mfgProductList
                                  where t.mfg_no == mfg_no
                                  select t).FirstOrDefault();
                if (checkExist == null)
                {
                    mfgProductList.Add(new MFGDetail()
                    {
                        id = (count + 1) * -1,
                        mfg_no = mfg_no,
                        item_id = product_id,
                        adj_detail_id = adj_detail_id,
                    });

                }
            }
            else
            {
                mfgProductList = new List<MFGDetail>();
                mfgProductList.Add(new MFGDetail()
                {
                    id = (count + 1) * -1,
                    mfg_no = mfg_no,
                    item_id = product_id,
                    adj_detail_id = adj_detail_id
                });

            }
            HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ_TEMP"] = mfgProductList;


            return "";
        }

        [WebMethod]
        public static MFGDetail EditMFG(int id)
        {
            var mfgProductList = (List<MFGDetail>)HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ_TEMP"];
            if (mfgProductList != null)
            {
                var row = (from t in mfgProductList where t.id == id select t).FirstOrDefault();
                if (row != null)
                {
                    return row;
                }
            }
            return new MFGDetail(); // ไม่มี Data
        }

        [WebMethod]
        public static string DeleteMFG(string id)
        {
            try
            {
                var prDetailDataMFG = (List<MFGDetail>)HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ_TEMP"];
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
                        row.is_delete = true;
                    }
                }
                HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ_TEMP"] = prDetailDataMFG;

                return "SUCCESS";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [WebMethod]
        public static string SubmitEditMFG(int id, string mfg_no)
        {
            try
            {
                var returnData = "SUCCESS";
                var mfgProductList = (List<MFGDetail>)HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ_TEMP"];
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
        public static string SubmitCustomMFGProduct(string adjDetailId)
        {
            var returnData = string.Empty;
            try
            {
                var selectedMFGList = (List<MFGDetail>)HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ_TEMP"];
                var borrowMFGDetailData = (List<Issue.IssueDetailMFG>)HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ADJ"];
                if (borrowMFGDetailData == null)
                {
                    borrowMFGDetailData = new List<Issue.IssueDetailMFG>();
                }
                if (selectedMFGList != null)
                {
                    var count = 0;
                    foreach (var row in selectedMFGList)
                    {
                        var checkExist = (from t in borrowMFGDetailData where t.id == row.id && t.issue_stock_detail_id == row.adj_detail_id select t).FirstOrDefault();
                        if (checkExist != null)
                        {
                            if (!row.is_delete)
                            {
                                checkExist.issue_stock_detail_id = row.adj_detail_id;
                                checkExist.mfg_no = row.mfg_no;

                                count++;
                            }
                            else
                            {
                                if (row.id < 0)
                                {
                                    borrowMFGDetailData.Remove(checkExist);
                                }
                                else
                                {
                                    checkExist.is_deleted = row.is_delete;
                                }
                            }
                        }
                        else
                        {
                            borrowMFGDetailData.Add(new Issue.IssueDetailMFG()
                            {
                                id = row.id,//(borrowMFGDetailData.Count + 1) * -1,
                                product_id = Convert.ToInt32(row.item_id),
                                mfg_no = row.mfg_no,
                                issue_stock_detail_id = Convert.ToInt32(row.adj_detail_id),
                                is_deleted = false,
                                qty = 1,
                                air_end_warranry = 1,
                                unit_warranty = 1
                            });
                            count++;
                        }
                    }
                }
                //HttpContext.Current.Session["SESSION_MFG_DETAIL_ADJ"] = mFGDetails;
                //HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ADJ"] = issueMFGDetail;
                HttpContext.Current.Session["SESSION_ISSUE_DETAIL_MFG_ADJ"] = borrowMFGDetailData;

                return returnData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}