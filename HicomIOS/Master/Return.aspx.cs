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
    public partial class Return : MasterDetailPage
    {
        private string strReturnType = string.Empty;
        public int dataId = 0;
        public override string PageName { get { return "Create Return" + (string.IsNullOrEmpty(Request.QueryString["type"]) ? "" : " " + Request.QueryString["type"].ToUpper()); } }
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

        private DataSet dsReturnData = new DataSet();
        public class ReturnRefDocData
        {
            public string issue_stock_no { get; set; }
            public string display_doc_date { get; set; }
            public string customer_code { get; set; }
            public string customer_name { get; set; }
            public int customer_id { get; set; }
            public string product_type { get; set; }

        }
        public class ReturnRefDocDetail
        {
            public bool is_selected { get; set; }
            public int id { get; set; }
            public int mfg_id { get; set; }
            public string issue_stock_no { get; set; }
            public string product_no { get; set; }
            public int product_id { get; set; }
            public string product_name_tha { get; set; }
            public int qty { get; set; }
            public int so_qty { get; set; }
            public string unit_tha { get; set; }
            public string remark { get; set; }
            public bool is_delete { get; set; }
            public string mfg_no { get; set; }
            public string product_type { get; set; }
        }
        public class ReturnDetail
        {
            public int id { get; set; }
            public string return_no { get; set; }
            public string product_no { get; set; }
            public int product_id { get; set; }
            public string product_name { get; set; }
            public string product_type { get; set; }
            public int qty { get; set; }
            public int old_qty { get; set; }
            public int ref_qty { get; set; }
            public string product_unit { get; set; }
            public bool is_delete { get; set; }
            public int ref_id { get; set; }
            public string mfg_no { get; set; }
            public string remark { get; set; }
        }

        List<ReturnRefDocDetail> refDocDetailList
        {
            get
            {
                if (Session["SESSION_REF_DOC_DETAIL_RETURN"] == null)
                    Session["SESSION_REF_DOC_DETAIL_RETURN"] = new List<ReturnRefDocDetail>();
                return (List<ReturnRefDocDetail>)Session["SESSION_REF_DOC_DETAIL_RETURN"];
            }
            set
            {
                Session["SESSION_REF_DOC_DETAIL_RETURN"] = value;
            }
        }
        List<ReturnDetail> returnDetailList
        {
            get
            {
                if (Session["SESSION_RETURN_DETAIL_RETURN"] == null)
                    Session["SESSION_RETURN_DETAIL_RETURN"] = new List<ReturnDetail>();
                return (List<ReturnDetail>)Session["SESSION_RETURN_DETAIL_RETURN"];
            }
            set
            {
                Session["SESSION_RETURN_DETAIL_RETURN"] = value;
            }
        }
        List<ReturnDetail> selectedReturnDetailList
        {
            get
            {
                if (Session["SESSION_SELECTED_RETURN_DETAIL_RETURN"] == null)
                    Session["SESSION_SELECTED_RETURN_DETAIL_RETURN"] = new List<ReturnDetail>();
                return (List<ReturnDetail>)Session["SESSION_SELECTED_RETURN_DETAIL_RETURN"];
            }
            set
            {
                Session["SESSION_SELECTED_RETURN_DETAIL_RETURN"] = value;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dataId = Convert.ToInt32(Request.QueryString["dataId"]);
                strReturnType = string.IsNullOrEmpty(Request.QueryString["type"]) ? "" : Request.QueryString["type"].ToUpper();
                if (!Page.IsPostBack)
                {
                    // Get Permission and if no permission, will redirect to another page.
                    //if (!Permission.GetPermission())
                        //Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

                    ClearWorkingSession();
                    PrepareData();
                    LoadData();
                }
                else
                {
                    BindGridviewIssue();
                    BindGridviewReturn();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Methods
        private void ClearWorkingSession()
        {
            Session.Remove("SESSION_REF_DOC_DETAIL_RETURN");
            Session.Remove("SESSION_RETURN_DETAIL_RETURN");
            Session.Remove("SESSION_SELECTED_RETURN_DETAIL_RETURN");
        }
        protected void PrepareData()
        {

            try
            {
                if (dataId == 0)
                {
                    // SPlanetUtil.BindASPxComboBox(ref cbbReturnDoc, DataListUtil.DropdownStoreProcedureName.Return_Issue);

                    if (strReturnType == "IS")
                    {
                        cbbReturnType.Items.Add(new ListItem("รับคืนสินค้า", "IS"));
                        cbbReturnType.Value = "IS";
                        SPlanetUtil.BindASPxComboBox(ref cbbReturnDoc, DataListUtil.DropdownStoreProcedureName.Return_Issue);
                        dvSupplier.Style["display"] = "none";
                    }
                    else if (strReturnType == "BR")
                    {
                        cbbReturnType.Items.Add(new ListItem("ส่งคืนสินค้า", "BR"));
                        cbbReturnType.Value = "BR";
                        SPlanetUtil.BindASPxComboBox(ref cbbReturnDoc, DataListUtil.DropdownStoreProcedureName.Return_Borrow);
                    }
                    else if (strReturnType == "PR")
                    {
                        cbbReturnType.Items.Add(new ListItem("คืนจากใบขอซื้อ", "PR"));
                        cbbReturnType.Value = "PR";
                        SPlanetUtil.BindASPxComboBox(ref cbbReturnDoc, DataListUtil.DropdownStoreProcedureName.Return_PR);
                    }
                    else
                    {
                        cbbReturnType.Disabled = true;
                    }


                    cbbCondition.Value = "G";
                }
                txtReturnDate.Value = DateTime.UtcNow.ToString("dd/MM/yyyy");

                gridViewReturn.SettingsBehavior.AllowFocusedRow = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void cbbReturnDoc_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "")
            {
                if (cbbReturnType.Value == "IS")
                {
                    SPlanetUtil.BindASPxComboBox(ref cbbReturnDoc, DataListUtil.DropdownStoreProcedureName.Return_Issue);
                }
                else if (cbbReturnType.Value == "BR")
                {
                    SPlanetUtil.BindASPxComboBox(ref cbbReturnDoc, DataListUtil.DropdownStoreProcedureName.Return_Borrow);
                }
                else if (cbbReturnType.Value == "PR")
                {
                    SPlanetUtil.BindASPxComboBox(ref cbbReturnDoc, DataListUtil.DropdownStoreProcedureName.Return_PR);
                }
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
                        dsReturnData = SqlHelper.ExecuteDataset(conn, "sp_return_list_data", arrParm.ToArray());
                        ViewState["dsReturnData"] = dsReturnData;
                        conn.Close();
                    }
                    if (dsReturnData.Tables.Count > 0)
                    {
                        #region Header
                        var header = (from t in dsReturnData.Tables[0].AsEnumerable() select t).FirstOrDefault();
                        if (header != null)
                        {
                            var return_type_text = string.Empty;
                            cbbReturnDoc.Items.Insert(0, new ListEditItem(Convert.ToString(header["ref_no"]), Convert.ToString(header["ref_no"])));
                            cbbCondition.Value = Convert.IsDBNull(header["item_condition"]) ? string.Empty : Convert.ToString(header["item_condition"]);
                            cbbReturnDoc.Value = Convert.IsDBNull(header["ref_no"]) ? string.Empty : Convert.ToString(header["ref_no"]);
                            return_type_text = Convert.IsDBNull(header["return_type"]) ? string.Empty : Convert.ToString(header["return_type"]);
                            //cbbReturnType.TEXT = Convert.IsDBNull(header["return_type_text"]) ? string.Empty : Convert.ToString(header["return_type_text"]);
                            lbCustomerFirstName.Value = Convert.IsDBNull(header["customer_name"]) ? string.Empty : Convert.ToString(header["customer_name"]);
                            lbCustomerNo.Value = Convert.IsDBNull(header["customer_code"]) ? string.Empty : Convert.ToString(header["customer_code"]);
                            lbIssueDate.Value = Convert.IsDBNull(header["ref_date"]) ? string.Empty : Convert.ToString(Convert.ToDateTime(header["ref_date"]).ToString("dd/MM/yyyy"));
                            txtReturnDate.Value = Convert.IsDBNull(header["return_date"]) ? string.Empty : Convert.ToString(Convert.ToDateTime(header["return_date"]).ToString("dd/MM/yyyy"));
                            txtReturnNo.Value = Convert.IsDBNull(header["return_no"]) ? string.Empty : Convert.ToString(header["return_no"]);
                            hdCustomerId.Value = Convert.IsDBNull(header["customer_id"]) ? string.Empty : Convert.ToString(header["customer_id"]);
                            TxtRemark.Value = Convert.IsDBNull(header["remark"]) ? string.Empty : Convert.ToString(header["remark"]);
                            
                            if (return_type_text == "IS")
                            {
                                cbbReturnType.Items.Add(new ListItem("รับคืนสินค้า", "IS"));
                                cbbReturnType.Value = "IS";
                                SPlanetUtil.BindASPxComboBox(ref cbbReturnDoc, DataListUtil.DropdownStoreProcedureName.Return_Issue);
                                dvSupplier.Style["display"] = "none";
                            }
                            else if (return_type_text == "BR")
                            {
                                cbbReturnType.Items.Add(new ListItem("ส่งคืนสินค้า", "BR"));
                                cbbReturnType.Value = "BR";
                                SPlanetUtil.BindASPxComboBox(ref cbbReturnDoc, DataListUtil.DropdownStoreProcedureName.Return_Borrow);
                                lbSupplierName.Value = Convert.IsDBNull(header["supplier_name"]) ? string.Empty : Convert.ToString(header["supplier_name"]); ;
                            }
                            else if (return_type_text == "PR")
                            {
                                cbbReturnType.Items.Add(new ListItem("คืนจากใบขอซื้อ", "PR"));
                                cbbReturnType.Value = "PR";
                                SPlanetUtil.BindASPxComboBox(ref cbbReturnDoc, DataListUtil.DropdownStoreProcedureName.Return_PR);
                            }

                            var return_status = Convert.IsDBNull(header["return_status"]) ? string.Empty : Convert.ToString(header["return_status"]);
                            if (return_status == "CF")
                            {
                                btnDraft.Visible = false;
                                btnSave.Visible = false;

                                gridViewReturn.Columns[0].Visible = false;
                            }
                            else
                            {
                                btnNew.Visible = false;
                            }
                        }

                        //returnType.Visible = false;
                        #endregion

                        #region Detail
                        var detail = (from t in dsReturnData.Tables[1].AsEnumerable() select t).ToList();
                        if (detail != null)
                        {
                            returnDetailList = new List<ReturnDetail>();
                            foreach (var row in detail)
                            {
                                returnDetailList.Add(new ReturnDetail()
                                {
                                    id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                    ref_id = Convert.IsDBNull(row["ref_id"]) ? 0 : Convert.ToInt32(row["ref_id"]),
                                    is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                    product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                    product_name = Convert.IsDBNull(row["product_name"]) ? string.Empty : Convert.ToString(row["product_name"]),
                                    product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                    product_unit = Convert.IsDBNull(row["product_unit"]) ? string.Empty : Convert.ToString(row["product_unit"]),
                                    qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["qty"]),
                                    ref_qty = Convert.IsDBNull(row["ref_qty"]) ? 0 : Convert.ToInt32(row["ref_qty"]),
                                    return_no = Convert.IsDBNull(row["return_no"]) ? string.Empty : Convert.ToString(row["return_no"]),
                                    mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                    product_type = Convert.IsDBNull(row["product_type"]) ? string.Empty : Convert.ToString(row["product_type"]),
                                    old_qty = Convert.IsDBNull(row["old_qty"]) ? 0 : Convert.ToInt32(row["old_qty"]),
                                    remark = Convert.IsDBNull(row["remark"]) ? string.Empty : Convert.ToString(row["remark"]),
                                });
                            }
                        }
                        #endregion

                        #region Issue Detail OR Borrow Detail
                        var dsResultDetail = new DataSet();
                        if (cbbReturnType.Value == "IS")
                        {
                            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                            {
                                //Create array of Parameters
                                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@issue_id", SqlDbType.Int) { Value = Convert.ToInt32(header["ref_id"]) },

                        };
                                conn.Open();
                                dsResultDetail = SqlHelper.ExecuteDataset(conn, "sp_issue_stock_detail_return_list", arrParm.ToArray());
                                conn.Close();
                            }
                            refDocDetailList = new List<ReturnRefDocDetail>();
                            if (dsResultDetail.Tables.Count > 0)
                            {
                                var detailData = (from t in dsResultDetail.Tables[0].AsEnumerable() select t).ToList();
                                if (detailData != null)
                                {
                                    foreach (var row in detailData)
                                    {
                                        refDocDetailList.Add(new ReturnRefDocDetail()
                                        {
                                            //product_name_tha
                                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                            qty = Convert.IsDBNull(row["qty_remain"]) ? 0 : Convert.ToInt32(row["qty_remain"]),
                                            product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                            unit_tha = Convert.IsDBNull(row["product_unit"]) ? string.Empty : Convert.ToString(row["product_unit"]),
                                            product_name_tha = Convert.IsDBNull(row["product_name_tha"]) ? string.Empty : Convert.ToString(row["product_name_tha"]),
                                            product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                            mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                            product_type = Convert.IsDBNull(row["product_type"]) ? string.Empty : Convert.ToString(row["product_type"]),
                                        });
                                    }
                                }

                            }
                        }
                        else if (cbbReturnType.Value == "BR")
                        {
                            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                            {
                                //Create array of Parameters
                                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@borrow_id", SqlDbType.Int) { Value = Convert.ToInt32(header["ref_id"]) },

                        };
                                conn.Open();
                                dsResultDetail = SqlHelper.ExecuteDataset(conn, "sp_borrow_detail_return_list", arrParm.ToArray());
                                conn.Close();
                            }
                            refDocDetailList = new List<ReturnRefDocDetail>();
                            if (dsResultDetail.Tables.Count > 0)
                            {
                                var detailData = (from t in dsResultDetail.Tables[0].AsEnumerable() select t).ToList();
                                if (detailData != null)
                                {
                                    foreach (var row in detailData)
                                    {
                                        refDocDetailList.Add(new ReturnRefDocDetail()
                                        {
                                            //product_name_tha
                                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                            qty = Convert.IsDBNull(row["qty_remain"]) ? 0 : Convert.ToInt32(row["qty_remain"]),
                                            product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                            unit_tha = Convert.IsDBNull(row["product_unit"]) ? string.Empty : Convert.ToString(row["product_unit"]),
                                            product_name_tha = Convert.IsDBNull(row["product_name_tha"]) ? string.Empty : Convert.ToString(row["product_name_tha"]),
                                            product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                            mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                            product_type = Convert.IsDBNull(row["product_type"]) ? string.Empty : Convert.ToString(row["product_type"]),
                                        });
                                    }
                                }

                            }
                        }
                        else if (cbbReturnType.Value == "PR")
                        {
                            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                            {
                                //Create array of Parameters
                                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@borrow_id", SqlDbType.Int) { Value = Convert.ToInt32(header["ref_id"]) },

                        };
                                conn.Open();
                                dsResultDetail = SqlHelper.ExecuteDataset(conn, "sp_purchase_request_detail_return_list", arrParm.ToArray());
                                conn.Close();
                            }
                            refDocDetailList = new List<ReturnRefDocDetail>();
                            if (dsResultDetail.Tables.Count > 0)
                            {
                                var detailData = (from t in dsResultDetail.Tables[0].AsEnumerable() select t).ToList();
                                if (detailData != null)
                                {
                                    foreach (var row in detailData)
                                    {
                                        refDocDetailList.Add(new ReturnRefDocDetail()
                                        {
                                            //product_name_tha
                                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                            qty = Convert.IsDBNull(row["qty_remain"]) ? 0 : Convert.ToInt32(row["qty_remain"]),
                                            product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                            unit_tha = Convert.IsDBNull(row["product_unit"]) ? string.Empty : Convert.ToString(row["product_unit"]),
                                            product_name_tha = Convert.IsDBNull(row["product_name_tha"]) ? string.Empty : Convert.ToString(row["product_name_tha"]),
                                            product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                            mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                            product_type = Convert.IsDBNull(row["product_type"]) ? string.Empty : Convert.ToString(row["product_type"]),
                                        });
                                    }
                                }

                            }
                        }
                        #endregion

                        if (returnDetailList.Count > 0)
                        {
                            if (returnDetailList[0].product_type == "P")
                            {
                                gridViewReturn.Columns["mfg_no"].Visible = true;
                            }
                            else
                            {
                                gridViewReturn.Columns["mfg_no"].Visible = false;
                            }
                        }

                        gridViewReturn.DataSource = returnDetailList;
                        gridViewReturn.DataBind();
                    }
                }
                else
                {
                    btnReportClient.Visible = false;
                    btnSave.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static string ValidateData()
        {
            string msg = string.Empty;
            var reDetail = (List<ReturnDetail>)HttpContext.Current.Session["SESSION_RETURN_DETAIL_RETURN"];
            if (reDetail == null || reDetail.Count == 0)
            {
                msg += "กรุณาเลือกรายการคืน อย่างน้อย 1 รายการ \n";
            }
            else
            {
                foreach (var row in reDetail)
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
            }
            return msg;
        }

        protected void SaveData(string status)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    int newID = DataListUtil.emptyEntryID;
                    var returnNo = string.Empty;
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {

                            if (dataId == 0) // new mode
                            {
                                #region Header
                                using (SqlCommand cmd = new SqlCommand("sp_return_header_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = hdCustomerId.Value;
                                    cmd.Parameters.Add("@customer_name", SqlDbType.VarChar, 150).Value = lbCustomerFirstName.Value;
                                    cmd.Parameters.Add("@return_status", SqlDbType.VarChar, 2).Value = status; //FL = Follow , CF = Confirm
                                    cmd.Parameters.Add("@return_date", SqlDbType.Date).Value = DateTime.ParseExact(txtReturnDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    cmd.Parameters.Add("@ref_no", SqlDbType.VarChar, 20).Value = cbbReturnDoc.Text;
                                    cmd.Parameters.Add("@return_type", SqlDbType.VarChar, 2).Value = cbbReturnType.Value; // BO = Borrow , IS = Issue
                                    cmd.Parameters.Add("@item_condition", SqlDbType.VarChar).Value = cbbCondition.Value;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                    cmd.Parameters.Add("@remark", SqlDbType.Text).Value = TxtRemark.Value;

                                    newID = Convert.ToInt32(cmd.ExecuteScalar());

                                }
                                #endregion

                                //Create array of Parameters

                                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = newID },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = "" },
                            new SqlParameter("@return_type", SqlDbType.VarChar, 200) { Value = strReturnType == "" ? "" :strReturnType }
                        };

                                var lastDataInsert = SqlHelper.ExecuteDataset(tran, "sp_return_header_list", arrParm.ToArray());


                                if (lastDataInsert != null)
                                {
                                    returnNo = (from t in lastDataInsert.Tables[0].AsEnumerable()
                                                select t.Field<string>("return_no")).FirstOrDefault();
                                }
                                var oldDetailId = 0;

                                #region Detail
                                foreach (var row in returnDetailList)
                                {
                                    oldDetailId = row.id;

                                    using (SqlCommand cmd = new SqlCommand("sp_return_detail_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@return_no", SqlDbType.VarChar, 20).Value = returnNo;
                                        cmd.Parameters.Add("@return_id", SqlDbType.Int).Value = newID;
                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                        cmd.Parameters.Add("@product_no", SqlDbType.VarChar, 20).Value = row.product_no;
                                        cmd.Parameters.Add("@product_name", SqlDbType.VarChar, 200).Value = row.product_name;
                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                        cmd.Parameters.Add("@product_unit", SqlDbType.VarChar, 5).Value = row.product_unit;
                                        cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 2).Value = row.product_type;
                                        cmd.Parameters.Add("@ref_qty", SqlDbType.Int).Value = row.ref_qty;
                                        cmd.Parameters.Add("@ref_id", SqlDbType.Int).Value = row.ref_id;
                                        cmd.Parameters.Add("@ref_type", SqlDbType.VarChar, 2).Value = cbbReturnType.Value;
                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 255).Value = row.remark;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        cmd.ExecuteNonQuery();

                                    }

                                }
                                // throw new Exception("x");
                                #endregion

                                int customer_id_stock_movement = 0;
                                if (cbbReturnType.Value == "IS")
                                {
                                    customer_id_stock_movement = Convert.ToInt32(hdCustomerId.Value);
                                    #region Issue Update Status

                                    using (SqlCommand cmd = new SqlCommand("sp_issue_update_status", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@ref_no", SqlDbType.VarChar, 20).Value = cbbReturnDoc.Text;
                                        cmd.Parameters.Add("@ref_type", SqlDbType.VarChar, 2).Value = "RE";

                                        cmd.ExecuteNonQuery();

                                    }

                                    #endregion
                                }
                                else if (cbbReturnType.Value == "BR")
                                {
                                    customer_id_stock_movement = 0;
                                    #region Borrow Update Status

                                    using (SqlCommand cmd = new SqlCommand("sp_borrow_update_status", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@ref_no", SqlDbType.VarChar, 20).Value = cbbReturnDoc.Text;
                                        cmd.Parameters.Add("@ref_type", SqlDbType.VarChar, 2).Value = "RE";

                                        cmd.ExecuteNonQuery();

                                    }

                                    #endregion
                                }
                                else if (cbbReturnType.Value == "PR")
                                {
                                    customer_id_stock_movement = 0;
                                    #region PR Update Status

                                    using (SqlCommand cmd = new SqlCommand("sp_purchase_request_update_status", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@ref_no", SqlDbType.VarChar, 20).Value = cbbReturnDoc.Text;
                                        cmd.Parameters.Add("@ref_type", SqlDbType.VarChar, 2).Value = "RE";

                                        cmd.ExecuteNonQuery();

                                    }

                                    #endregion
                                }

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
                                    foreach (var row in returnDetailList)
                                    {

                                        using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                            cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.product_type;
                                            cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;

                                            cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "RETURN";
                                            cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = cbbReturnType.Value == "IS" ? "IN" : "OUT";
                                            cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                            cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                            cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Return " + cbbReturnType.Value;
                                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                            cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                            cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = returnNo;
                                            cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customer_id_stock_movement;
                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                }
                                #endregion
                            }
                            else // edit mode
                            {
                                returnNo = Convert.ToString(txtReturnNo.Value);
                                newID = dataId;
                                #region Header
                                using (SqlCommand cmd = new SqlCommand("sp_return_header_edit", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = dataId;
                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = hdCustomerId.Value;
                                    cmd.Parameters.Add("@return_no", SqlDbType.VarChar, 20).Value = returnNo;
                                    cmd.Parameters.Add("@customer_name", SqlDbType.VarChar, 150).Value = lbCustomerFirstName.Value;
                                    cmd.Parameters.Add("@return_status", SqlDbType.VarChar, 2).Value = status; //FL = Follow , CF = Confirm
                                    cmd.Parameters.Add("@return_date", SqlDbType.Date).Value = DateTime.ParseExact(txtReturnDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    cmd.Parameters.Add("@ref_no", SqlDbType.VarChar, 20).Value = cbbReturnDoc.Text;
                                    //cmd.Parameters.Add("@return_type", SqlDbType.VarChar, 2).Value = cbbReturnType.Value; // BO = Borrow , IS = Issue
                                    cmd.Parameters.Add("@item_condition", SqlDbType.VarChar).Value = cbbCondition.Value;
                                    cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = false;
                                    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                    cmd.Parameters.Add("@remark", SqlDbType.Text).Value = TxtRemark.Value;

                                    cmd.ExecuteNonQuery();

                                }
                                #endregion

                                #region Transection
                                if (status == "CF")
                                {
                                    int customer_id_stock_movement = 0;
                                    if (cbbReturnType.Value == "IS")
                                    {
                                        customer_id_stock_movement = Convert.ToInt32(hdCustomerId.Value);
                                    }
                                    var transNo = string.Empty;
                                    using (SqlCommand cmd = new SqlCommand("sp_gen_transection_no", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        transNo = Convert.ToString(cmd.ExecuteScalar());

                                    }
                                    string stock_type = string.Empty;
                                    foreach (var row in returnDetailList)
                                    {
                                        if (row.id < 0)
                                        {
                                            using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.product_type;
                                                cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;

                                                cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "RETURN";
                                                cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = cbbReturnType.Value == "IS" ? "IN" : "OUT";
                                                cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                                cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Return " + cbbReturnType.Value;
                                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = returnNo;
                                                cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customer_id_stock_movement;
                                                cmd.ExecuteNonQuery();

                                            }
                                        }
                                        else if (row.id > 0 && !row.is_delete)
                                        {
                                            var diffQty = row.qty - row.old_qty;
                                            if (row.qty > 0)
                                            {
                                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.product_type;
                                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;

                                                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "RETURN";
                                                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = cbbReturnType.Value == "IS" ? "IN" : "OUT";//? (diffQty > 0 ? "IN" : "OUT") : (diffQty < 0 ? "IN" : "OUT");
                                                    cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Return " + cbbReturnType.Value;
                                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                    cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtReturnNo.Value;
                                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customer_id_stock_movement;
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

                                                cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "RETURN";
                                                cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = cbbReturnType.Value == "IS" ? "IN" : "OUT";
                                                cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                                cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Return " + cbbReturnType.Value;//"คืนค่าจากการลบ";
                                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = newID; // คำนวนจาก Store
                                                cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = txtReturnNo.Value;
                                                cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customer_id_stock_movement;
                                                cmd.ExecuteNonQuery();

                                            }

                                        }
                                    }
                                }
                                #endregion

                                var oldDetailId = 0;
                                #region Detail
                                foreach (var row in returnDetailList)
                                {
                                    oldDetailId = row.id;
                                    if (row.id < 0)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_return_detail_add", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@return_no", SqlDbType.VarChar, 20).Value = returnNo;
                                            cmd.Parameters.Add("@return_id", SqlDbType.Int).Value = newID;
                                            cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                            cmd.Parameters.Add("@product_no", SqlDbType.VarChar, 20).Value = row.product_no;
                                            cmd.Parameters.Add("@product_name", SqlDbType.VarChar, 200).Value = row.product_name;
                                            cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;
                                            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                            cmd.Parameters.Add("@product_unit", SqlDbType.VarChar, 5).Value = row.product_unit;
                                            cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 2).Value = row.product_type;
                                            cmd.Parameters.Add("@ref_qty", SqlDbType.Int).Value = row.ref_qty;
                                            cmd.Parameters.Add("@ref_id", SqlDbType.Int).Value = row.ref_id;
                                            cmd.Parameters.Add("@ref_type", SqlDbType.VarChar, 2).Value = cbbReturnType.Value;
                                            cmd.Parameters.Add("@remark", SqlDbType.VarChar, 255).Value = row.remark;
                                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                    else 
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_return_detail_edit", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                            cmd.Parameters.Add("@return_no", SqlDbType.VarChar, 20).Value = returnNo;
                                            cmd.Parameters.Add("@return_id", SqlDbType.Int).Value = newID;
                                            cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;
                                            cmd.Parameters.Add("@product_no", SqlDbType.VarChar, 20).Value = row.product_no;
                                            cmd.Parameters.Add("@product_name", SqlDbType.VarChar, 200).Value = row.product_name;
                                            cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;
                                            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                            cmd.Parameters.Add("@product_unit", SqlDbType.VarChar, 5).Value = row.product_unit;
                                            cmd.Parameters.Add("@ref_qty", SqlDbType.Int).Value = row.ref_qty;
                                            cmd.Parameters.Add("@ref_id", SqlDbType.Int).Value = row.ref_id;
                                            cmd.Parameters.Add("@ref_type", SqlDbType.VarChar, 2).Value = cbbReturnType.Value;
                                            cmd.Parameters.Add("@remark", SqlDbType.VarChar, 255).Value = row.remark;
                                            cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = row.is_delete;
                                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                            
                                            cmd.ExecuteNonQuery();

                                        }
                                    }

                                }
                                #endregion
                                #region Issue Update Status

                                if (cbbReturnType.Value == "IS")
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_issue_update_status", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@ref_no", SqlDbType.VarChar, 20).Value = cbbReturnDoc.Text;
                                        cmd.Parameters.Add("@ref_type", SqlDbType.VarChar, 2).Value = "RE";

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                                #endregion
                                else if (cbbReturnType.Value == "BR")
                                {
                                    #region Borrow Update Status

                                    using (SqlCommand cmd = new SqlCommand("sp_borrow_update_status", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@ref_no", SqlDbType.VarChar, 20).Value = cbbReturnDoc.Text;
                                        cmd.Parameters.Add("@ref_type", SqlDbType.VarChar, 2).Value = "RE";

                                        cmd.ExecuteNonQuery();

                                    }

                                    #endregion
                                }
                                else if (cbbReturnType.Value == "PR")
                                {
                                    //customer_id_stock_movement = 0;
                                    #region PR Update Status

                                    using (SqlCommand cmd = new SqlCommand("sp_purchase_request_update_status", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@ref_no", SqlDbType.VarChar, 20).Value = cbbReturnDoc.Text;
                                        cmd.Parameters.Add("@ref_type", SqlDbType.VarChar, 2).Value = "RE";

                                        cmd.ExecuteNonQuery();

                                    }

                                    #endregion
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            tran.Dispose();
                            conn.Close();
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('" + ex.Message + "','E')", true);
                        }
                        finally
                        {
                            if (!conn.State.Equals(ConnectionState.Closed))
                            {

                                tran.Commit();
                                tran.Dispose();
                                conn.Close();
                                Response.Redirect("Return.aspx?dataId=" + newID);
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

        #region Return
        protected void gridViewReturn_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (returnDetailList.Count > 0)
            {
                if (returnDetailList[0].product_type == "P")
                {
                    gridViewReturn.Columns["mfg_no"].Visible = true;
                }
                else
                {
                    gridViewReturn.Columns["mfg_no"].Visible = false;
                }
            }

            var param = e.Parameters;
            if (param == "clear")
            {
                gridViewReturn.DataSource = null;
                gridViewReturn.DataBind();
            }
            else
            {
                gridViewReturn.DataSource = (from t in returnDetailList
                                             where t.is_delete == false
                                             select t).ToList();
                gridViewReturn.DataBind();
            }

        }
        private void BindGridviewReturn()
        {
            gridViewReturn.DataSource = (from t in returnDetailList
                                         where t.is_delete == false
                                         select t).ToList();
            gridViewReturn.DataBind();
        }
        [WebMethod]
        public static ReturnDetail EditReturnDetail(string id)
        {
            var returnData = (List<ReturnDetail>)HttpContext.Current.Session["SESSION_RETURN_DETAIL_RETURN"];
            var selectedData = new ReturnDetail();
            if (returnData != null)
            {
                selectedData = (from t in returnData where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
            }
            return selectedData;
        }
        [WebMethod]
        public static List<ReturnDetail> DeleteReturnDetail(string id)
        {
            var returnData = (List<ReturnDetail>)HttpContext.Current.Session["SESSION_RETURN_DETAIL_RETURN"];
            var issueDetailData = (List<ReturnRefDocDetail>)HttpContext.Current.Session["SESSION_REF_DOC_DETAIL_RETURN"];

            if (returnData != null)
            {
                var rowDeleteReturn = (from t in returnData where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (issueDetailData != null)
                {
                    var rowIssueDetail = (from t in issueDetailData where t.id == rowDeleteReturn.ref_id select t).FirstOrDefault();
                    if (Convert.ToInt32(id) < 0)
                    {
                        if (rowIssueDetail != null)
                        {
                            rowIssueDetail.is_selected = false;
                        }
                        else
                        {
                            issueDetailData = new List<ReturnRefDocDetail>();
                            issueDetailData.Add(new ReturnRefDocDetail()
                            {
                                id = rowDeleteReturn.ref_id,
                                product_name_tha = rowDeleteReturn.product_name,
                                product_id = rowDeleteReturn.product_id,
                                product_no = rowDeleteReturn.product_no,
                                unit_tha = rowDeleteReturn.product_unit,
                                qty = rowDeleteReturn.qty,
                                mfg_no = rowDeleteReturn.mfg_no,
                                is_selected = false,
                                product_type = rowDeleteReturn.product_type
                            });
                        }
                        returnData.Remove(rowDeleteReturn);

                    }
                }
               
                else
                {
                    rowDeleteReturn.is_delete = true;
                    issueDetailData = new List<ReturnRefDocDetail>();
                    issueDetailData.Add(new ReturnRefDocDetail()
                    {
                        id = rowDeleteReturn.ref_id,
                        product_name_tha = rowDeleteReturn.product_name,
                        product_id = rowDeleteReturn.product_id,
                        product_no = rowDeleteReturn.product_no,
                        unit_tha = rowDeleteReturn.product_unit,
                        qty = rowDeleteReturn.qty,
                        mfg_no = rowDeleteReturn.mfg_no,
                        is_selected = false,
                        product_type = rowDeleteReturn.product_type
                    });
                    
                }
            }
            HttpContext.Current.Session["SESSION_REF_DOC_DETAIL_RETURN"] = issueDetailData;
            HttpContext.Current.Session["SESSION_RETURN_DETAIL_RETURN"] = returnData;

            return returnData;
        }
        [WebMethod]
        public static List<ReturnDetail> SubmitReturnDetail(string id, string qty, string remark)
        {
            var returnData = (List<ReturnDetail>)HttpContext.Current.Session["SESSION_RETURN_DETAIL_RETURN"];
            if (returnData != null)
            {
                var selectedRow = (from t in returnData where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (selectedRow != null)
                {
                    selectedRow.qty = Convert.ToInt32(qty);
                    selectedRow.remark = remark;
                }
            }

            HttpContext.Current.Session["SESSION_RETURN_DETAIL_RETURN"] = returnData;

            return returnData;
        }
        #endregion Return

        #region Event
        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            SaveData("FL");
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            SaveData("CF");
        }

        #endregion

        #region Issue
        [WebMethod]
        public static ReturnRefDocData GetRefDocData(string id, string type)
        {
            HttpContext.Current.Session.Remove("SESSION_REF_DOC_DETAIL_RETURN");
            HttpContext.Current.Session.Remove("SESSION_RETURN_DETAIL_RETURN");
            HttpContext.Current.Session.Remove("SESSION_SELECTED_RETURN_DETAIL_RETURN");
            var data = new ReturnRefDocData();
            var dsResult = new DataSet();
            var returnDetailList = new List<ReturnRefDocDetail>();

            if (type == "IS")
            {
                #region Header
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },

                        };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_issue_stock_list_data", arrParm.ToArray());
                    conn.Close();
                }
                if (dsResult.Tables.Count != 0)
                {

                    var headerData = (from t in dsResult.Tables[0].AsEnumerable() select t).FirstOrDefault();
                    if (headerData != null)
                    {

                        data.customer_name = Convert.IsDBNull(headerData["customer_name"]) ? string.Empty : Convert.ToString(headerData["customer_name"]);
                        data.display_doc_date = Convert.IsDBNull(headerData["issue_stock_date"]) ? string.Empty : Convert.ToDateTime(headerData["issue_stock_date"]).ToString("dd/MM/yyyy");
                        data.customer_code = Convert.IsDBNull(headerData["customer_code"]) ? string.Empty : Convert.ToString(headerData["customer_code"]);
                        data.customer_id = Convert.IsDBNull(headerData["customer_id"]) ? 0 : Convert.ToInt32(headerData["customer_id"]);
                        data.product_type = Convert.IsDBNull(headerData["quotation_type"]) ? string.Empty : Convert.ToString(headerData["quotation_type"]);
                    }

                }

                var dsResultDetail = new DataSet();
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@issue_id", SqlDbType.Int) { Value = id },

                        };
                    conn.Open();
                    dsResultDetail = SqlHelper.ExecuteDataset(conn, "sp_issue_stock_detail_return_list", arrParm.ToArray());
                    conn.Close();
                }
                #endregion
                #region Detail
                if (dsResultDetail.Tables.Count > 0)
                {
                    var detailData = (from t in dsResultDetail.Tables[0].AsEnumerable() select t).ToList();
                    if (detailData != null)
                    {
                        foreach (var row in detailData)
                        {
                            returnDetailList.Add(new ReturnRefDocDetail()
                            {
                                //product_name_tha
                                id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                qty = Convert.IsDBNull(row["qty_remain"]) ? 0 : Convert.ToInt32(row["qty_remain"]),
                                product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                unit_tha = Convert.IsDBNull(row["product_unit"]) ? string.Empty : Convert.ToString(row["product_unit"]),
                                product_name_tha = Convert.IsDBNull(row["product_name_tha"]) ? string.Empty : Convert.ToString(row["product_name_tha"]),
                                product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                product_type = Convert.IsDBNull(row["product_type"]) ? string.Empty : Convert.ToString(row["product_type"]),
                            });
                        }
                    }

                }
                #endregion
            }
            else if (type == "BR")
            {
                #region Header
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },

                        };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_borrow_list_data", arrParm.ToArray());
                    conn.Close();
                }
                if (dsResult.Tables.Count != 0)
                {

                    var headerData = (from t in dsResult.Tables[0].AsEnumerable() select t).FirstOrDefault();
                    if (headerData != null)
                    {

                        data.customer_name = Convert.IsDBNull(headerData["customer_name"]) ? string.Empty : Convert.ToString(headerData["customer_name"]);
                        data.display_doc_date = Convert.IsDBNull(headerData["borrow_date"]) ? string.Empty : Convert.ToDateTime(headerData["borrow_date"]).ToString("dd/MM/yyyy");
                        data.customer_code = Convert.IsDBNull(headerData["customer_code"]) ? string.Empty : Convert.ToString(headerData["customer_code"]);
                        data.customer_id = Convert.IsDBNull(headerData["customer_id"]) ? 0 : Convert.ToInt32(headerData["customer_id"]);
                        data.product_type = Convert.IsDBNull(headerData["product_type"]) ? string.Empty : Convert.ToString(headerData["product_type"]);
                    }

                }

                var dsResultDetail = new DataSet();
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@borrow_id", SqlDbType.Int) { Value = id },

                        };
                    conn.Open();
                    dsResultDetail = SqlHelper.ExecuteDataset(conn, "sp_borrow_detail_return_list", arrParm.ToArray());
                    conn.Close();
                }
                #endregion
                #region Detail
                if (dsResultDetail.Tables.Count > 0)
                {
                    var detailData = (from t in dsResultDetail.Tables[0].AsEnumerable() select t).ToList();
                    if (detailData != null)
                    {
                        foreach (var row in detailData)
                        {
                            returnDetailList.Add(new ReturnRefDocDetail()
                            {
                                //product_name_tha
                                id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                qty = Convert.IsDBNull(row["qty_remain"]) ? 0 : Convert.ToInt32(row["qty_remain"]),
                                product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                unit_tha = Convert.IsDBNull(row["product_unit"]) ? string.Empty : Convert.ToString(row["product_unit"]),
                                product_name_tha = Convert.IsDBNull(row["product_name_tha"]) ? string.Empty : Convert.ToString(row["product_name_tha"]),
                                product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                product_type = Convert.IsDBNull(row["product_type"]) ? string.Empty : Convert.ToString(row["product_type"]),
                            });
                        }
                    }

                }
                #endregion
            }
            else if (type == "PR")
            {
                #region Header
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },

                        };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_purchase_request_list_data", arrParm.ToArray());
                    conn.Close();
                }
                if (dsResult.Tables.Count != 0)
                {

                    var headerData = (from t in dsResult.Tables[0].AsEnumerable() select t).FirstOrDefault();
                    if (headerData != null)
                    {

                        data.customer_name = Convert.IsDBNull(headerData["customer_name"]) ? string.Empty : Convert.ToString(headerData["customer_name"]);
                        data.display_doc_date = Convert.IsDBNull(headerData["purchase_request_date"]) ? string.Empty : Convert.ToDateTime(headerData["purchase_request_date"]).ToString("dd/MM/yyyy");
                        data.customer_code = Convert.IsDBNull(headerData["customer_code"]) ? string.Empty : Convert.ToString(headerData["customer_code"]);
                        data.customer_id = Convert.IsDBNull(headerData["customer_id"]) ? 0 : Convert.ToInt32(headerData["customer_id"]);
                    }

                    var dataType = (from t in dsResult.Tables[1].AsEnumerable() select t).FirstOrDefault();
                    if (dataType != null)
                    {
                        data.product_type = Convert.IsDBNull(dataType["item_type"]) ? string.Empty : Convert.ToString(dataType["item_type"]);
                    }

                }

                var dsResultDetail = new DataSet();
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@purchase_request_id", SqlDbType.Int) { Value = id },

                        };
                    conn.Open();
                    dsResultDetail = SqlHelper.ExecuteDataset(conn, "sp_purchase_request_detail_return_list", arrParm.ToArray());
                    conn.Close();
                }
                #endregion
                #region Detail
                if (dsResultDetail.Tables.Count > 0)
                {
                    var detailData = (from t in dsResultDetail.Tables[0].AsEnumerable() select t).ToList();
                    if (detailData != null)
                    {
                        foreach (var row in detailData)
                        {
                            returnDetailList.Add(new ReturnRefDocDetail()
                            {
                                //product_name_tha
                                id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                qty = Convert.IsDBNull(row["qty_remain"]) ? 0 : Convert.ToInt32(row["qty_remain"]),
                                product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                unit_tha = Convert.IsDBNull(row["product_unit"]) ? string.Empty : Convert.ToString(row["product_unit"]),
                                product_name_tha = Convert.IsDBNull(row["product_name_tha"]) ? string.Empty : Convert.ToString(row["product_name_tha"]),
                                product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                product_type = Convert.IsDBNull(row["product_type"]) ? string.Empty : Convert.ToString(row["product_type"]),
                            });
                        }
                    }

                }
                #endregion
            }
            HttpContext.Current.Session["SESSION_REF_DOC_DETAIL_RETURN"] = returnDetailList;
            return data;

        }
        [WebMethod]
        public static List<ReturnDetail> GetIssueDetailData(string id)
        {
            // Clone Data จริง ที่ Selected Data ของ Return
            var data = (List<ReturnDetail>)HttpContext.Current.Session["SESSION_RETURN_DETAIL_RETURN"];
            if (data == null)
            {
                data = new List<ReturnDetail>();
            }

            HttpContext.Current.Session["SESSION_SELECTED_RETURN_DETAIL_RETURN"] = data;

            return data;
        }
        [WebMethod]
        public static List<ReturnRefDocDetail> AddReturnDetail(string id, bool isSelected)
        {
            var issueDetailData = (List<ReturnRefDocDetail>)HttpContext.Current.Session["SESSION_REF_DOC_DETAIL_RETURN"];
            var selectedReturnDetail = (List<ReturnDetail>)HttpContext.Current.Session["SESSION_SELECTED_RETURN_DETAIL_RETURN"];
            var selectedIssueDetailRow = (from t in issueDetailData where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
            if (selectedIssueDetailRow != null)
            {
                if (selectedReturnDetail != null)
                {
                    var rowReturnDetail = (from t in selectedReturnDetail where t.ref_id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (rowReturnDetail != null)
                    {

                        if (isSelected) // เกิดกรณีเดียว คือ EditMode เพราะ ถ้า NewMode จะถูก Remove Row ออกไป
                        {
                            selectedIssueDetailRow.is_selected = true;
                            rowReturnDetail.is_delete = false;
                        }
                        else // isSelected == false เกิดได้กรณีเดียวคือ เคยมีค่าอยู่แล้ว
                        {
                            selectedIssueDetailRow.is_selected = false;
                            if (rowReturnDetail.id < 0)
                            {
                                selectedReturnDetail.Remove(rowReturnDetail);
                            }
                            else
                            {
                                rowReturnDetail.is_delete = true;
                            }

                        }
                    }
                    else
                    {
                        selectedIssueDetailRow.is_selected = true;
                        // เกิดกรณีเดียว คือ isSelected == true
                        selectedReturnDetail.Add(new ReturnDetail()
                        {
                            id = (selectedReturnDetail.Count + 1) * -1,
                            ref_id = Convert.ToInt32(id),
                            is_delete = false,
                            product_id = selectedIssueDetailRow.product_id,
                            product_name = selectedIssueDetailRow.product_name_tha,
                            product_unit = selectedIssueDetailRow.unit_tha,
                            product_no = selectedIssueDetailRow.product_no,
                            qty = selectedIssueDetailRow.qty,
                            ref_qty = selectedIssueDetailRow.qty,
                            mfg_no = selectedIssueDetailRow.mfg_no,
                            product_type = selectedIssueDetailRow.product_type,
                        });
                    }
                }
                else
                {
                    selectedReturnDetail = new List<ReturnDetail>();
                    selectedIssueDetailRow.is_selected = true;
                    // เกิดกรณีเดียว คือ isSelected == true
                    selectedReturnDetail.Add(new ReturnDetail()
                    {
                        id = (selectedReturnDetail.Count + 1) * -1,
                        ref_id = Convert.ToInt32(id),
                        is_delete = false,
                        product_id = selectedIssueDetailRow.product_id,
                        product_name = selectedIssueDetailRow.product_name_tha,
                        product_unit = selectedIssueDetailRow.unit_tha,
                        product_no = selectedIssueDetailRow.product_no,
                        qty = selectedIssueDetailRow.qty,
                        ref_qty = selectedIssueDetailRow.qty,
                        mfg_no = selectedIssueDetailRow.mfg_no,
                        product_type = selectedIssueDetailRow.product_type
                    });
                }
            }

            HttpContext.Current.Session["SESSION_SELECTED_RETURN_DETAIL_RETURN"] = selectedReturnDetail;
            HttpContext.Current.Session["SESSION_REF_DOC_DETAIL_RETURN"] = issueDetailData;

            return issueDetailData;

        }
        [WebMethod]
        public static List<ReturnDetail> SubmitIssueDetail()
        {
            var returnData = (List<ReturnDetail>)HttpContext.Current.Session["SESSION_RETURN_DETAIL_RETURN"];
            var selectedReturnData = (List<ReturnDetail>)HttpContext.Current.Session["SESSION_SELECTED_RETURN_DETAIL_RETURN"];
            var issueDetailData = (List<ReturnRefDocDetail>)HttpContext.Current.Session["SESSION_REF_DOC_DETAIL_RETURN"];

            if (selectedReturnData != null)
            {
                foreach (var selectedRow in selectedReturnData)
                {
                    var existItem = (from t in returnData where t.id == selectedRow.id select t).FirstOrDefault();
                    if (existItem == null)
                    {
                        if (selectedRow.is_delete == false)
                        {
                            returnData.Add(selectedRow);
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

                            var rowIssue = (from t in issueDetailData where t.id == selectedRow.ref_id select t).FirstOrDefault();
                            if (rowIssue != null)
                            {
                                if (rowIssue.is_selected)
                                {
                                    issueDetailData.Remove(rowIssue);
                                }
                            }
                        }
                    }
                }

                HttpContext.Current.Session["SESSION_RETURN_DETAIL_RETURN"] = returnData;
                HttpContext.Current.Session["SESSION_SELECTED_RETURN_DETAIL_RETURN"] = null;
                HttpContext.Current.Session["SESSION_REF_DOC_DETAIL_RETURN"] = issueDetailData;
            }

            return returnData;
        }
        private void BindGridviewIssue()
        {
            gridViewRefDocDetail.DataSource = (from t in refDocDetailList
                                               where t.is_delete == false
                                               select t).ToList();
            gridViewRefDocDetail.DataBind();
        }
        #endregion Issue

        protected void gridViewRefDocDetail_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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

                gridViewRefDocDetail.DataSource = (from t in refDocDetailList
                                                   where t.is_delete == false
                                                   select t).ToList();
                //if (strReturnType == "IS")
                //{
                    //gridViewRefDocDetail.Columns[0].Visible = false;
                    if (hdProductType.Value == "P")
                    {
                        gridViewRefDocDetail.Columns[4].Visible = true;
                        gridViewRefDocDetail.Columns[2].Caption = "Product No";
                        gridViewRefDocDetail.Columns[3].Caption = "Product Name";
                    }
                    else
                    {
                        gridViewRefDocDetail.Columns[4].Visible = false;
                        gridViewRefDocDetail.Columns[2].Caption = "Part No";
                        gridViewRefDocDetail.Columns[3].Caption = "Part Name";
                    }
                //}
                gridViewRefDocDetail.DataBind();
            }
            else
            {
                gridViewRefDocDetail.DataSource = (from t in refDocDetailList
                                                   where t.is_delete == false && (t.product_no.ToUpper().Contains(searchText.ToUpper()) || t.product_name_tha.ToUpper().Contains(searchText.ToUpper()))
                                                   select t).ToList();
                if (strReturnType == "IS")
                {
                    //gridViewRefDocDetail.Columns[0].Visible = false;
                }
                gridViewRefDocDetail.DataBind();
            }
        }

        protected void gridViewRefDocDetail_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxCheckBox checkBox = gridViewRefDocDetail.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkIssueDetail") as ASPxCheckBox;
                if (checkBox != null)
                {
                    if (e.DataColumn.FieldName == "is_selected")
                    {
                        if (refDocDetailList != null)
                        {
                            var row = (from t in returnDetailList where t.ref_id == Convert.ToInt32(e.KeyValue) && t.is_delete == false select t).FirstOrDefault();
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [WebMethod]
        public static string SelectAllIssue(bool selected)
        {

            var data = (List<ReturnRefDocDetail>)HttpContext.Current.Session["SESSION_REF_DOC_DETAIL_RETURN"];
            var selectedReturnDetail = (List<ReturnDetail>)HttpContext.Current.Session["SESSION_SELECTED_RETURN_DETAIL_RETURN"];
            if (data != null)
            {
                foreach (var row in data)
                {
                    if (selected)
                    {
                        row.is_selected = true;
                        if (selectedReturnDetail != null && selectedReturnDetail.Count > 0)
                        {
                            var rowExist = (from t in selectedReturnDetail where t.ref_id == row.id select t).FirstOrDefault();

                            if (rowExist != null) // กรณี Newmode
                            {
                                rowExist.qty = row.qty;
                            }
                            else
                            {
                                selectedReturnDetail.Add(new ReturnDetail()
                                {
                                    id = (selectedReturnDetail.Count + 1) * -1,
                                    ref_id = Convert.ToInt32(row.id),
                                    is_delete = false,
                                    product_id = row.product_id,
                                    product_name = row.product_name_tha,
                                    product_unit = row.unit_tha,
                                    product_no = row.product_no,
                                    qty = row.qty,
                                    ref_qty = row.qty,
                                    mfg_no = row.mfg_no,
                                    product_type = row.product_type,

                                });
                            }

                        }
                        else
                        {
                            if (selectedReturnDetail == null)
                            {
                                selectedReturnDetail = new List<ReturnDetail>();
                            }
                            selectedReturnDetail.Add(new ReturnDetail()
                            {
                                id = (selectedReturnDetail.Count + 1) * -1,
                                ref_id = Convert.ToInt32(row.id),
                                is_delete = false,
                                product_id = row.product_id,
                                product_name = row.product_name_tha,
                                product_unit = row.unit_tha,
                                product_no = row.product_no,
                                qty = row.qty,
                                ref_qty = row.qty,
                                mfg_no = row.mfg_no,
                                product_type = row.product_type,

                            });
                        }
                    }
                    else
                    {
                        var rowExist = (from t in selectedReturnDetail where t.ref_id == row.id select t).FirstOrDefault();
                        row.is_selected = false;
                        if (rowExist != null)
                        {
                            if (rowExist.id < 0) // กรณี Newmode
                            {
                                selectedReturnDetail.Remove(rowExist);
                            }
                            else
                            {
                                rowExist.is_delete = true;
                            }
                        }
                    }
                }
            }
            HttpContext.Current.Session["SESSION_SELECTED_RETURN_DETAIL_RETURN"] = selectedReturnDetail;
            HttpContext.Current.Session["SESSION_REF_DOC_DETAIL_RETURN"] = data;

            return "SELECT ALL";
        }


    }
}