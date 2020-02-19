﻿using DevExpress.Web;
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
    public partial class AdjustProductSparePart : MasterDetailPage
    {
        public override string PageName { get { return "AdjustProductSparePart"; } }

        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }

        private static string guid = Guid.NewGuid().ToString();
        private const string session_item = "_SESSION_ADJUST_ITEM";
        private const string session_detail = "_SESSION_ADJUST_DETAIL";
        private const string session_selected_detail = "_SESSION_ADJUST_SELECTED_DETAIL";
        private const string session_selected_item = "_SESSION_ADJUST_SELECTED_ITEM";
        #region Members
        private int dataId = 0;
        public class AdjustProductDetail
        {
            public int id { get; set; }
            public int sort_no { get; set; }
            public string item_no { get; set; }
            public string item_name { get; set; }
            public string item_type { get; set; }
            public string adjust_type { get; set; }
            public int quantity { get; set; }
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
                BindGridView();
                BindGridSelectedItem();
            }
        }
        protected void PrepareData()
        {
            try
            {
                SPlanetUtil.BindASPxComboBox(ref cbbCustomer, DataListUtil.DropdownStoreProcedureName.Customer);
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
                if (dataId == 0)
                {
                    btnSave.Visible = false;
                }
                else
                {
                    var dsResult = new DataSet();
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = dataId },
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
                            txtObjective.Value = Convert.IsDBNull(header["adjust_objective"]) ? string.Empty : Convert.ToString(header["adjust_objective"]);
                            txtPO.Value = Convert.IsDBNull(header["po_no"]) ? string.Empty : Convert.ToString(header["po_no"]);
                            txtQuotationNo.Value = Convert.IsDBNull(header["quotation_no"]) ? string.Empty : Convert.ToString(header["quotation_no"]);
                            cbbCustomer.Value = Convert.IsDBNull(header["customer_id"]) ? string.Empty : Convert.ToString(header["customer_id"]);
                            dtAdjustDate.Value = Convert.IsDBNull(header["adjust_date"]) ? string.Empty : Convert.ToDateTime(header["adjust_date"]).ToString("dd/MM/yyyy");
                            hdDocStatus.Value = Convert.IsDBNull(header["doc_status"]) ? string.Empty : Convert.ToString(header["doc_status"]);
                        }

                        if (hdDocStatus.Value == "CF")
                        {
                            btnDraft.Visible = false;
                            btnProduct.Visible = false;
                            btnSparePart.Visible = false;
                            btnSave.Visible = false;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "Disabled Input", "disableInput()", true);
                        }
                        else
                        {

                        }

                        var detail = dsResult.Tables[1].AsEnumerable().ToList();
                        if (detail != null)
                        {
                            adjustDetailList = new List<AdjustProductDetail>();
                            foreach (var row in detail)
                            {
                                adjustDetailList.Add(new AdjustProductDetail()
                                {
                                    adjust_type = Convert.IsDBNull(row["adjust_type"]) ? string.Empty : Convert.ToString(row["adjust_type"]),
                                    id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                    is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                    item_name = Convert.IsDBNull(row["item_name"]) ? string.Empty : Convert.ToString(row["item_name"]),
                                    item_no = Convert.IsDBNull(row["item_no"]) ? string.Empty : Convert.ToString(row["item_no"]),
                                    item_type = Convert.IsDBNull(row["item_type"]) ? string.Empty : Convert.ToString(row["item_type"]),
                                    quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]),
                                    remark = Convert.IsDBNull(row["remark"]) ? string.Empty : Convert.ToString(row["remark"]),
                                    sort_no = Convert.IsDBNull(row["sort_no"]) ? 0 : Convert.ToInt32(row["sort_no"]),
                                    unit_code = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"])
                                });
                            }
                        }
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
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbCustomer.Value;
                                cmd.Parameters.Add("@customer_name", SqlDbType.VarChar, 200).Value = cbbCustomer.Text;
                                cmd.Parameters.Add("@adjust_date", SqlDbType.DateTime).Value = adjust_date;
                                cmd.Parameters.Add("@adjust_objective", SqlDbType.VarChar, 200).Value = txtObjective.Value;//FL = Follow , CF = Confirm
                                cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = txtQuotationNo.Value;
                                cmd.Parameters.Add("@po_no", SqlDbType.VarChar, 20).Value = txtPO.Value;
                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                newID = Convert.ToInt32(cmd.ExecuteScalar());

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

                                    using (SqlCommand cmd = new SqlCommand("sp_adjust_item_detail_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@header_id", SqlDbType.Int).Value = newID;
                                        cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                        cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = row.quantity;
                                        cmd.Parameters.Add("@item_no", SqlDbType.VarChar, 20).Value = row.item_no;
                                        cmd.Parameters.Add("@item_name", SqlDbType.VarChar, 200).Value = row.item_name;
                                        cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 2).Value = row.item_type;
                                        cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 10).Value = row.unit_code;
                                        cmd.Parameters.Add("@adjust_type", SqlDbType.VarChar, 5).Value = row.adjust_type;
                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = row.remark;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        newDetailId = Convert.ToInt32(cmd.ExecuteScalar());

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
                            using (SqlCommand cmd = new SqlCommand("sp_adjust_item_header_edit", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = dataId;
                                cmd.Parameters.Add("@doc_status", SqlDbType.VarChar, 20).Value = status;
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = cbbCustomer.Value;
                                cmd.Parameters.Add("@customer_name", SqlDbType.VarChar, 200).Value = cbbCustomer.Text;
                                cmd.Parameters.Add("@adjust_date", SqlDbType.DateTime).Value = adjust_date;
                                cmd.Parameters.Add("@adjust_objective", SqlDbType.VarChar, 200).Value = txtObjective.Value;//FL = Follow , CF = Confirm
                                cmd.Parameters.Add("@quotation_no", SqlDbType.VarChar, 20).Value = txtQuotationNo.Value;
                                cmd.Parameters.Add("@po_no", SqlDbType.VarChar, 20).Value = txtPO.Value;
                                cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                cmd.ExecuteNonQuery();

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
                                            cmd.Parameters.Add("@item_no", SqlDbType.VarChar, 20).Value = row.item_no;
                                            cmd.Parameters.Add("@item_name", SqlDbType.VarChar, 200).Value = row.item_name;
                                            cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = row.quantity;
                                            cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 2).Value = row.item_type;
                                            cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 10).Value = row.unit_code;
                                            cmd.Parameters.Add("@adjust_type", SqlDbType.VarChar, 5).Value = row.adjust_type;
                                            cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = row.remark;
                                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                            newDetailId = Convert.ToInt32(cmd.ExecuteScalar());

                                        }
                                    }
                                    else if (row.id > 0 && !row.is_delete)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_adjust_item_detail_edit", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                            cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                            // cmd.Parameters.Add("@item_no", SqlDbType.VarChar, 20).Value = row.item_no;
                                            // cmd.Parameters.Add("@item_name", SqlDbType.VarChar, 200).Value = row.item_name;
                                            // cmd.Parameters.Add("@unit_code", SqlDbType.VarChar, 10).Value = row.unit_code;
                                            cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = row.quantity;
                                            //cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 2).Value = row.item_type;
                                            cmd.Parameters.Add("@adjust_type", SqlDbType.VarChar, 5).Value = row.adjust_type;
                                            cmd.Parameters.Add("@remark", SqlDbType.VarChar, 200).Value = row.remark;
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
                                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

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

                            Response.Redirect("AdjustProductSparePart.aspx?dataId=" + newID);
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindGridView()
        {
            gridView.DataSource = adjustDetailList.Where(t => !t.is_delete).ToList();
            gridView.DataBind();
        }
        protected void gridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridView.DataSource = adjustDetailList.Where(t => !t.is_delete).ToList();
            gridView.DataBind();
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
            gridViewSelectedItem.DataSource = selectedItemDetailList;
            gridViewSelectedItem.DataBind();
        }
        [WebMethod]
        public static string PopupItemDetail(string item_type)
        {
            try
            {
                var selectedItemList = new List<AdjustProductDetail>();
                var adjustDetailList = (List<AdjustProductDetail>)HttpContext.Current.Session[guid + session_detail];
                if (adjustDetailList != null)
                {
                    selectedItemList.AddRange(adjustDetailList);
                }
                HttpContext.Current.Session[guid + session_selected_detail] = selectedItemList;

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
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_list", arrParm.ToArray());
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

                foreach (var row in selectedItemList)
                {
                    var checkExist = productList.Where(t => t.item_no == row.item_no && !row.is_delete).FirstOrDefault();
                    if (checkExist != null)
                    {
                        checkExist.is_selected = true;
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
                        selectedItemList.Add(new AdjustProductDetail()
                        {
                            adjust_type = null,
                            id = (selectedItemList.Count + 1) * -1,
                            is_delete = false,
                            item_name = item.item_name,
                            item_no = item.item_no,
                            item_type = item.item_type,
                            quantity = item.quantity,
                            unit_code = item.unit_code,
                            remark = string.Empty,
                            sort_no = (selectedItemList.Count + 1)

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
                var deletedItem = new List<AdjustProductDetail>();
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
                }

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
                if (rdoAdjust != null)
                {
                    if (e.DataColumn.FieldName == "adjust_type")
                    {
                        if (adjustDetailList != null)
                        {
                            var row = adjustDetailList.Where(t => t.id == Convert.ToInt32(e.KeyValue)).FirstOrDefault();
                            if (row != null)
                            {
                                rdoAdjust.Value = row.adjust_type;
                            }
                            if(hdDocStatus.Value == "CF")
                            {
                                rdoAdjust.Enabled = false;
                            }
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

            if (adjustDetailList != null)
            {
                var row = adjustDetailList.Where(t => t.id == id).FirstOrDefault();
                if (row != null)
                {
                    if (row.id < 0)
                    {
                        adjustDetailList.Remove(row);
                    }
                    else
                    {
                        row.is_delete = true;
                    }
                }
            }
            HttpContext.Current.Session[guid + session_detail] = adjustDetailList;
            return "QUANTITY";
        }

        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            SaveData("DR");
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            SaveData("CF");
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
                else
                {
                    foreach (var row in adjustDetailList)
                    {
                        if (row.quantity == 0)
                        {
                            msgError += "กรุณาใส่จำนวน สินค้า " + row.item_name + "</br>";
                        }
                        else if (string.IsNullOrEmpty(row.adjust_type))
                        {
                            msgError += "กรุณาระบุการ Adjust สินค้า " + row.item_name + "</br>";
                        }
                    }
                }
            }
            return msgError;
        }
    }
}