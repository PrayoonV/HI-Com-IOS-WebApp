﻿using System;
using System.Web.UI;
//Custom using namespace
using DevExpress.Web;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using HicomIOS.ClassUtil;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace HicomIOS.Master
{
    public partial class SparePart : MasterDetailPage
    {
        private DataSet dsResult;

        public class ServicePartListList
        {
            public int id { get; set; }
            public string part_no { get; set; }
            public string secondary_code { get; set; }
            public int shelf_id { get; set; }
            public string shelf_name { get; set; }
            public string part_name_tha { get; set; }
            public string part_name_eng { get; set; }
            public int cat_id { get; set; }
            public string cat_name_tha { get; set; }
            public int brand_id { get; set; }
            public int quantity { get; set; }
            public int quantity_reserve { get; set; }
            public int quantity_balance { get; set; }
            public int unit_id { get; set; }
            public string unit_code { get; set; }
            public decimal selling_price { get; set; }
            public decimal min_selling_price { get; set; }
            public string remark { get; set; }
            public bool is_enable { get; set; }
            public bool is_delete { get; set; }
            public string is_edit { get; set; }
            public int created_by { get; set; }
        }
        public class ServicePartSupplier
        {
            public int id { get; set; }
            public int part_id { get; set; }
            public string part_no { get; set; }
            public string part_name { get; set; }
            public string supplier_name { get; set; }
            public int supplier_id { get; set; }
            public bool is_delete { get; set; }
        }
        public class DropdownListClass
        {
            public int data_value { get; set; }
            public string data_text { get; set; }
        }
        public class SparePartShelf
        {
            public int id { get; set; }
            public int part_id { get; set; }
            public int shelf_id { get; set; }
            public string shelf_name { get; set; }
            public bool is_delete { get; set; }
        }

        List<ServicePartSupplier> servicePartListSupplierList
        {
            get
            {
                if (Session["SESSION_PART_SUPPLIER_SPAREPART"] == null)
                    Session["SESSION_PART_SUPPLIER_SPAREPART"] = new List<ServicePartSupplier>();
                return (List<ServicePartSupplier>)Session["SESSION_PART_SUPPLIER_SPAREPART"];
            }
            set
            {
                Session["SESSION_PART_SUPPLIER_SPAREPART"] = value;
            }
        }
        List<SparePartShelf> sparePartShelfList
        {
            get
            {
                if (Session["SESSION_PART_SHELF_SPAREPART"] == null)
                    Session["SESSION_PART_SHELF_SPAREPART"] = new List<SparePartShelf>();
                return (List<SparePartShelf>)Session["SESSION_PART_SHELF_SPAREPART"];
            }
            set
            {
                Session["SESSION_PART_SHELF_SPAREPART"] = value;
            }
        }
        List<DropdownListClass> supplierList
        {
            get
            {
                if (Session["SESSION_SUPPLIER_SPAREPART"] == null)
                    Session["SESSION_SUPPLIER_SPAREPART"] = new List<DropdownListClass>();
                return (List<DropdownListClass>)Session["SESSION_SUPPLIER_SPAREPART"];
            }
            set
            {
                Session["SESSION_SUPPLIER_SPAREPART"] = value;
            }
        }
        List<DropdownListClass> shelfList
        {
            get
            {
                if (Session["SESSION_SHELF_SPAREPART"] == null)
                    Session["SESSION_SHELF_SPAREPART"] = new List<DropdownListClass>();
                return (List<DropdownListClass>)Session["SESSION_SHELF_SPAREPART"];
            }
            set
            {
                Session["SESSION_SHELF_SPAREPART"] = value;
            }
        }
        List<ServicePartListList> sparePartList
        {
            get
            {
                if (Session["SESSION_SPARE_PART_LIST_SPAREPART"] == null)
                    Session["SESSION_SPARE_PART_LIST_SPAREPART"] = new List<ServicePartListList>();
                return (List<ServicePartListList>)Session["SESSION_SPARE_PART_LIST_SPAREPART"];
            }
            set
            {
                Session["SESSION_SPARE_PART_LIST_SPAREPART"] = value;
            }
        }
        public override string PageName { get { return "SparePart"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                //Bind Data into GridView
                if (!Page.IsPostBack)
                {
                    // Get Permission and if no permission, will redirect to another page.
                    if (!Permission.GetPermission())
                        Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

                    ClearWorkingSession();
                    PrepareData();
                    BindGrid(true);

                }
                else
                {
                    //dsResult = (DataSet)ViewState["dsResult"];
                    //dsResult = (DataSet)Session["SESSION_SPARE_PART_MASTER"];
                    BindGrid(false);
                    BindGridSupplier();
                    BindGridShelf();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region "Initial Property and Event"
        protected void PopupMenu_Load(object sender, EventArgs e)
        {
            (sender as ASPxPopupMenu).PopupElementID = gridView.ID;
        }
        #endregion

        #region "Inherited Event of Filter Control"
        public override void OnFilterChanged()
        {
            BindGrid();
        }
        public override void DeleteEntry(string id)
        {
            try
            {//Execute Delete Data Store Procedure
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                    {
                        new SqlParameter("@id", SqlDbType.Int) { Value = int.Parse(id) },
                        new SqlParameter("@update_by", SqlDbType.Int) { Value = Session[ConstantClass.SESSION_USER_ID] }
                    };
                    conn.Open();
                    SqlHelper.ExecuteNonQuery(conn, "sp_notification_delete", arrParm.ToArray());
                }
                BindGrid(true);
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        public override void RefreshEntry()     //Popup Menu Select Refresh Data
        {
            BindGrid(true);
        }
        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();

            return result.OfType<FilterControlColumn>();
        }
        #endregion
        protected void PrepareData()
        {
            try
            {
                // Load Combobox Product Category Data
                List<SqlParameter> arrParmSP = new List<SqlParameter>
                        {
                             new SqlParameter("@lang_id", SqlDbType.VarChar,3) { Value = "tha" },
                            new SqlParameter("@cat_type", SqlDbType.VarChar,2) { Value = "SP" }

                        };
                SPlanetUtil.BindASPxComboBox(ref cboCateID, DataListUtil.DropdownStoreProcedureName.SparePart_Category);
                cboCateID.Items.Insert(0, new ListEditItem("--โปรดเลือก--", "0"));

                // Load Combobox Product Unit Type Data
                SPlanetUtil.BindASPxComboBox(ref cboUnitType, DataListUtil.DropdownStoreProcedureName.Product_Unit);
                cboUnitType.Items.Insert(0, new ListEditItem("--โปรดเลือก--", "0"));

                // Load Combobox Supplier Data
                SPlanetUtil.BindASPxComboBox(ref cboSupplierSparepart, DataListUtil.DropdownStoreProcedureName.Supplier);
                cboSupplierSparepart.Items.Insert(0, new ListEditItem("--โปรดเลือก--", "0"));

                // Load Combobox Product Brand Data
                SPlanetUtil.BindASPxComboBox(ref cboBrand, DataListUtil.DropdownStoreProcedureName.Product_Brand);
                cboBrand.Items.Insert(0, new ListEditItem("--โปรดเลือก--", "0"));

                SPlanetUtil.BindASPxComboBox(ref cboShelf, DataListUtil.DropdownStoreProcedureName.Shelf_List);
                cboShelf.Items.Insert(0, new ListEditItem("--โปรดเลือก--", "0"));

                // Setting height gridView
                gridView.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen);
                gridView.SettingsBehavior.AllowFocusedRow = true;

                supplierList = new List<DropdownListClass>();

                var dsResult = new DataSet();
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 }
                        };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_supplier_list", arrParm.ToArray());
                    conn.Close();
                }
                if (dsResult.Tables.Count > 0)
                {
                    var data = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                    if (data != null)
                    {
                        foreach (var row in data)
                        {
                            supplierList.Add(new DropdownListClass()
                            {
                                data_value = Convert.ToInt32(row["id"]),
                                data_text = string.Format("{0}{1}", Convert.IsDBNull(row["supplier_code"]) ? string.Empty : Convert.ToString(row["supplier_code"] + " - "),
                                Convert.IsDBNull(row["supplier_name_tha"]) ? string.Empty : Convert.ToString(row["supplier_name_tha"]))

                            });
                        }
                    }
                }

                var dsResult2 = new DataSet();
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm2 = new List<SqlParameter>
                        {
                            new SqlParameter("@lang_id", SqlDbType.VarChar,3) { Value = "tha" },
                        };
                    conn.Open();
                    dsResult2 = SqlHelper.ExecuteDataset(conn, "sp_dropdown_shelf", arrParm2.ToArray());
                    conn.Close();
                }
                if (dsResult2.Tables.Count > 0)
                {
                    var data = (from t in dsResult2.Tables[0].AsEnumerable() select t).ToList();
                    if (data != null)
                    {
                        foreach (var row in data)
                        {
                            shelfList.Add(new DropdownListClass()
                            {
                                data_value = Convert.ToInt32(row["data_value"]),
                                data_text = Convert.IsDBNull(row["data_text"]) ? string.Empty : Convert.ToString(row["data_text"])

                            });
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
        public static void CleaSession()
        {
            HttpContext.Current.Session.Remove("SESSION_PART_SUPPLIER_SPAREPART");
            HttpContext.Current.Session.Remove("SESSION_SUPPLIER_SPAREPART");
            HttpContext.Current.Session.Remove("SESSION_SPARE_PART_LIST_SPAREPART");
            HttpContext.Current.Session.Remove("SESSION_SHELF_SPAREPART");
            HttpContext.Current.Session.Remove("SESSION_PART_SHELF_SPAREPART");
        }
        private void ClearWorkingSession()
        {
            Session.Remove("SESSION_PART_SUPPLIER_SPAREPART");
            Session.Remove("SESSION_SUPPLIER_SPAREPART");
            Session.Remove("SESSION_SPARE_PART_LIST_SPAREPART");
            Session.Remove("SESSION_SHELF_SPAREPART");
            Session.Remove("SESSION_PART_SHELF_SPAREPART");
        }
        protected void BindGrid(bool isForceRefreshData = false)
        {
            try
            {
                if (!Page.IsPostBack || isForceRefreshData)
                {
                    string search = "";
                    if (Session["SEARCH"] != null)
                    {
                        search = Session["SEARCH"].ToString();
                        txtSearchBoxData.Value = search;
                    }
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters

                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = search }
                        };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_list", arrParm.ToArray());
                    }
                    sparePartList = new List<ServicePartListList>();
                    /*int rowId = 0;
                    if (Session["ROW_ID"] != null)
                    {
                        rowId = Convert.ToInt32(Session["ROW_ID"].ToString());
                    }*/
                    foreach (var row in dsResult.Tables[0].AsEnumerable())
                    {
                        sparePartList.Add(new ServicePartListList()
                        {
                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                            part_no = Convert.IsDBNull(row["part_no"]) ? null : Convert.ToString(row["part_no"]),
                            secondary_code = Convert.IsDBNull(row["secondary_code"]) ? null : Convert.ToString(row["secondary_code"]),
                            cat_id = Convert.IsDBNull(row["cat_id"]) ? 0 : Convert.ToInt32(row["cat_id"]),
                            cat_name_tha = Convert.IsDBNull(row["cat_name_tha"]) ? "" : Convert.ToString(row["cat_name_tha"]),
                            shelf_id = Convert.IsDBNull(row["shelf_id"]) ? 0 : Convert.ToInt32(row["shelf_id"]),
                            shelf_name = Convert.IsDBNull(row["shelf_name"]) ? "" : Convert.ToString(row["shelf_name"]),
                            part_name_tha = Convert.IsDBNull(row["part_name_tha"]) ? null : Convert.ToString(row["part_name_tha"]),
                            part_name_eng = Convert.IsDBNull(row["part_name_eng"]) ? null : Convert.ToString(row["part_name_eng"]),
                            brand_id = Convert.IsDBNull(row["brand_id"]) ? 0 : Convert.ToInt32(row["brand_id"]),
                            unit_id = Convert.IsDBNull(row["unit_id"]) ? 0 : Convert.ToInt32(row["unit_id"]),
                            unit_code = Convert.IsDBNull(row["unit_code"]) ? null : Convert.ToString(row["unit_code"]),
                            selling_price = Convert.IsDBNull(row["selling_price"]) ? 0 : Convert.ToDecimal(row["selling_price"]),
                            min_selling_price = Convert.IsDBNull(row["min_selling_price"]) ? 0 : Convert.ToDecimal(row["min_selling_price"]),
                            quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]),
                            quantity_reserve = Convert.IsDBNull(row["quantity_reserve"]) ? 0 : Convert.ToInt32(row["quantity_reserve"]),
                            quantity_balance = Convert.IsDBNull(row["quantity_balance"]) ? 0 : Convert.ToInt32(row["quantity_balance"]),
                            is_enable = Convert.IsDBNull(row["is_enable"]) ? false : Convert.ToBoolean(row["is_enable"])
                        });

                        /*if (rowId == sparePartList[sparePartList.Count - 1].id)
                        {
                            int selectedRow = sparePartList.Count - 1;
                            int prevRow = Convert.ToInt32(Session["ROW"]);
                            int pageSize = gridView.SettingsPager.PageSize;
                            int pageIndex = (int) (selectedRow / pageSize);
                            int prevPageIndex = Convert.ToInt32(Session["PAGE"]);
                            if (prevRow == selectedRow)
                            {
                                Session["PAGE"] = prevPageIndex;
                                Session["ROW"] = prevPageIndex * pageSize;
                            }
                            else
                            {
                                Session["PAGE"] = pageIndex;
                                Session["ROW"] = selectedRow;
                            }
                        }*/
                    }
                }

                //Bind data into GridView
                Session["SESSION_SPARE_PART_MASTER"] = sparePartList;
                gridView.DataSource = sparePartList;
                gridView.FilterExpression = FilterBag.GetExpression(false);
                gridView.DataBind();

                //  Check page from session
                if (Session["ROW_ID"] != null)
                {
                    int row = Convert.ToInt32(Session["ROW"]);
                    gridView.FocusedRowIndex = row;
                }
                if (Session["PAGE"] != null)
                {
                    int page = Convert.ToInt32(Session["PAGE"]);
                    gridView.PageIndex = page;
                }
                if (!Page.IsPostBack && !string.IsNullOrEmpty(Session["COLUMN"].ToString()) && !string.IsNullOrEmpty(Session["ORDER"].ToString()))
                {
                    int order = Convert.ToInt32(Session["ORDER"]);
                    if (order == 1)
                    {
                        ((GridViewDataColumn)gridView.Columns[Session["COLUMN"].ToString()]).SortAscending();
                    }
                    else
                    {
                        ((GridViewDataColumn)gridView.Columns[Session["COLUMN"].ToString()]).SortDescending();
                    }
                }

                BindGridSupplier();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }

        [WebMethod]
        public static string NewData()
        {
            var returnData = string.Empty;
            HttpContext.Current.Session.Remove("SESSION_PART_SUPPLIER_SPAREPART");
            //HttpContext.Current.Session.Remove("SESSION_SUPPLIER_SPAREPART");
            HttpContext.Current.Session.Remove("SESSION_PART_SHELF_SPAREPART");
            //HttpContext.Current.Session.Remove("SESSION_SHELF_SPAREPART");
            return returnData;
        }

        [WebMethod]
        public static string ValidateData()
        {
            var returnMessage = string.Empty;
            var supplierSparePart = (List<ServicePartSupplier>)HttpContext.Current.Session["SESSION_PART_SUPPLIER_SPAREPART"];
            var sparePartShelfList = (List<SparePartShelf>)HttpContext.Current.Session["SESSION_PART_SHELF_SPAREPART"];

            if (supplierSparePart.Count == 0)
            {
                returnMessage += "- กรุณาเลือก Supplier อย่างน้อย 1 รายการ <br>";
            }

            if (sparePartShelfList.Count == 0)
            {
                returnMessage += "- กรุณาเลือก Shelf อย่างน้อย 1 รายการ <br>";
            }
            return returnMessage;
        }

        [WebMethod]
        public static string SaveSparePartData(ServicePartListList[] sparePartData)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                conn.Open();
                int newId = 0;
                var supplierSparePart = (List<ServicePartSupplier>)HttpContext.Current.Session["SESSION_PART_SUPPLIER_SPAREPART"];
                var sparePartShelfList = (List<SparePartShelf>)HttpContext.Current.Session["SESSION_PART_SHELF_SPAREPART"];
                var row = (from t in sparePartData select t).FirstOrDefault();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        if (row != null)
                        {
                            if (row.id == 0)
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_product_spare_part_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@item_no", SqlDbType.VarChar, 20).Value = "";
                                    cmd.Parameters.Add("@part_no", SqlDbType.VarChar, 20).Value = row.part_no;
                                    cmd.Parameters.Add("@secondary_code", SqlDbType.VarChar, 100).Value = row.secondary_code;
                                    cmd.Parameters.Add("@shelf_id", SqlDbType.Int).Value = row.shelf_id;
                                    cmd.Parameters.Add("@part_name_tha", SqlDbType.VarChar, 150).Value = row.part_name_tha;
                                    cmd.Parameters.Add("@part_name_eng", SqlDbType.VarChar, 150).Value = row.part_name_eng;
                                    cmd.Parameters.Add("@cat_id", SqlDbType.Int).Value = row.cat_id;
                                    cmd.Parameters.Add("@brand_id", SqlDbType.Int).Value = row.brand_id;
                                    //cmd.Parameters.Add("@description_tha", SqlDbType.VarChar, 150).Value = row.description_tha;
                                    //cmd.Parameters.Add("@description_eng", SqlDbType.VarChar, 150).Value = row.description_eng;
                                    //cmd.Parameters.Add("@stock_count_type", SqlDbType.VarChar, 150).Value = row.stock_count_type;
                                    cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = row.quantity;
                                    cmd.Parameters.Add("@quantity_reserve", SqlDbType.Int).Value = row.quantity_reserve;
                                    cmd.Parameters.Add("@quantity_balance", SqlDbType.Int).Value = row.quantity_balance;
                                    cmd.Parameters.Add("@unit_id", SqlDbType.Int).Value = row.unit_id;
                                    cmd.Parameters.Add("@selling_price", SqlDbType.Decimal).Value = row.selling_price;
                                    cmd.Parameters.Add("@min_selling_price", SqlDbType.Decimal).Value = row.min_selling_price;
                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 150).Value = row.remark == null ? string.Empty : row.remark;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                    newId = Convert.ToInt32(cmd.ExecuteScalar());

                                }
                                foreach (var rowSupplier in supplierSparePart)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_item_mapping_supplier_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = newId;
                                        cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = rowSupplier.supplier_id;
                                        cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 2).Value = "S";

                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                        cmd.ExecuteNonQuery();

                                    }
                                }
                                foreach (var rowShelf in sparePartShelfList)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_product_spare_part_shelf_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@part_id", SqlDbType.Int).Value = newId;
                                        cmd.Parameters.Add("@shelf_id", SqlDbType.Int).Value = rowShelf.shelf_id;
                                        cmd.Parameters.Add("@shelf_name", SqlDbType.VarChar, 20).Value = rowShelf.shelf_name;

                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                        cmd.ExecuteNonQuery();

                                    }
                                }
                            }

                            else
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_product_spare_part_edit", conn, tran))
                                {
                                    newId = row.id;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                    cmd.Parameters.Add("@item_no", SqlDbType.VarChar, 20).Value = "";
                                    cmd.Parameters.Add("@part_no", SqlDbType.VarChar, 20).Value = row.part_no;
                                    cmd.Parameters.Add("@secondary_code", SqlDbType.VarChar, 100).Value = row.secondary_code;
                                    cmd.Parameters.Add("@shelf_id", SqlDbType.Int).Value = row.shelf_id;
                                    cmd.Parameters.Add("@part_name_tha", SqlDbType.VarChar, 150).Value = row.part_name_tha;
                                    cmd.Parameters.Add("@part_name_eng", SqlDbType.VarChar, 150).Value = row.part_name_eng;
                                    cmd.Parameters.Add("@cat_id", SqlDbType.Int).Value = row.cat_id;
                                    cmd.Parameters.Add("@brand_id", SqlDbType.Int).Value = row.brand_id;
                                    //cmd.Parameters.Add("@description_tha", SqlDbType.VarChar, 150).Value = row.description_tha;
                                    //cmd.Parameters.Add("@description_eng", SqlDbType.VarChar, 150).Value = row.description_eng;
                                    //cmd.Parameters.Add("@stock_count_type", SqlDbType.VarChar, 150).Value = row.stock_count_type;
                                    cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = row.quantity;
                                    cmd.Parameters.Add("@quantity_reserve", SqlDbType.Int).Value = row.quantity_reserve;
                                    cmd.Parameters.Add("@quantity_balance", SqlDbType.Int).Value = row.quantity_balance;
                                    cmd.Parameters.Add("@unit_id", SqlDbType.Int).Value = row.unit_id;
                                    cmd.Parameters.Add("@selling_price", SqlDbType.Decimal).Value = row.selling_price;
                                    cmd.Parameters.Add("@min_selling_price", SqlDbType.Decimal).Value = row.min_selling_price;
                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 150).Value = row.remark == null ? string.Empty : row.remark;
                                    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                    cmd.ExecuteNonQuery();

                                }
                                foreach (var rowSupplier in supplierSparePart)
                                {
                                    if (rowSupplier.id < 0)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_item_mapping_supplier_add", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = newId;
                                            cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = rowSupplier.supplier_id;
                                            cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 2).Value = "S";

                                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                    else if (rowSupplier.is_delete)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_item_mapping_supplier_delete", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowSupplier.id;
                                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                }
                                foreach (var rowShelf in sparePartShelfList)
                                {
                                    if (rowShelf.id < 0)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_product_spare_part_shelf_add", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@part_id", SqlDbType.Int).Value = newId;
                                            cmd.Parameters.Add("@shelf_id", SqlDbType.Int).Value = rowShelf.shelf_id;
                                            cmd.Parameters.Add("@shelf_name", SqlDbType.VarChar, 20).Value = rowShelf.shelf_name;

                                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                    else
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_product_spare_part_shelf_edit", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowShelf.id;
                                            cmd.Parameters.Add("@part_id", SqlDbType.Int).Value = newId;
                                            cmd.Parameters.Add("@shelf_id", SqlDbType.Int).Value = rowShelf.shelf_id;
                                            cmd.Parameters.Add("@shelf_name", SqlDbType.VarChar, 20).Value = rowShelf.shelf_name;
                                            cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = rowShelf.is_delete;
                                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                }
                            }
                        }

                        //  Set new id
                        HttpContext.Current.Session["ROW_ID"] = Convert.ToString(newId);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        tran.Dispose();
                        conn.Close();
                        //throw ex;
                        return ex.Message.ToString();
                    }
                    finally
                    {
                        if (!conn.State.Equals(ConnectionState.Closed))
                        {

                            tran.Commit();
                            tran.Dispose();
                            conn.Close();
                        }
                    }
                }
            }
            return "success";
        }


        [WebMethod]
        public static ServicePartListList GetEditSparePartData(string id, int index)
        {
            //  Set active row
            HttpContext.Current.Session["ROW_ID"] = id;
            HttpContext.Current.Session["ROW"] = index;

            var sparePartData = new ServicePartListList();
            var dsDataSparePart = new DataSet();
            var supplierData = (List<DropdownListClass>)HttpContext.Current.Session["SESSION_SUPPLIER_SPAREPART"];
            var shelfData = (List<DropdownListClass>)HttpContext.Current.Session["SESSION_SHELF_SPAREPART"];
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {

                            new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(id) },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = "" }
                        };
                    conn.Open();
                    dsDataSparePart = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_list", arrParm.ToArray());
                    conn.Close();
                    if (dsDataSparePart.Tables.Count > 0)
                    {
                        var row = dsDataSparePart.Tables[0].Rows[0];

                        sparePartData.id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]);
                        sparePartData.part_no = Convert.IsDBNull(row["part_no"]) ? null : Convert.ToString(row["part_no"]);
                        sparePartData.secondary_code = Convert.IsDBNull(row["secondary_code"]) ? null : Convert.ToString(row["secondary_code"]);
                        sparePartData.cat_id = Convert.IsDBNull(row["cat_id"]) ? 0 : Convert.ToInt32(row["cat_id"]);
                        sparePartData.shelf_id = Convert.IsDBNull(row["shelf_id"]) ? 0 : Convert.ToInt32(row["shelf_id"]);
                        sparePartData.part_name_tha = Convert.IsDBNull(row["part_name_tha"]) ? null : Convert.ToString(row["part_name_tha"]);
                        sparePartData.part_name_eng = Convert.IsDBNull(row["part_name_eng"]) ? null : Convert.ToString(row["part_name_eng"]);
                        sparePartData.unit_code = Convert.IsDBNull(row["unit_code"]) ? null : Convert.ToString(row["unit_code"]);
                        //sparePartData.description_tha = Convert.IsDBNull(row["description_tha"]) ? null : Convert.ToString(row["description_tha"]);
                        //sparePartData.description_eng = Convert.IsDBNull(row["description_eng"]) ? null : Convert.ToString(row["description_eng"]);
                        sparePartData.brand_id = Convert.IsDBNull(row["brand_id"]) ? 0 : Convert.ToInt32(row["brand_id"]);
                        //sparePartData.stock_count_type = Convert.IsDBNull(row["stock_count_type"]) ? null : Convert.ToString(row["stock_count_type"]);
                        sparePartData.remark = Convert.ToString(row["remark"]);
                        sparePartData.unit_id = Convert.IsDBNull(row["unit_id"]) ? 0 : Convert.ToInt32(row["unit_id"]);
                        sparePartData.selling_price = Convert.IsDBNull(row["selling_price"]) ? 0 : Convert.ToDecimal(row["selling_price"]);
                        sparePartData.min_selling_price = Convert.IsDBNull(row["min_selling_price"]) ? 0 : Convert.ToDecimal(row["min_selling_price"]);
                        sparePartData.quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]);
                        sparePartData.quantity_reserve = Convert.IsDBNull(row["quantity_reserve"]) ? 0 : Convert.ToInt32(row["quantity_reserve"]);
                        sparePartData.quantity_balance = Convert.IsDBNull(row["quantity_balance"]) ? 0 : Convert.ToInt32(row["quantity_balance"]);
                        sparePartData.is_edit = Convert.IsDBNull(row["part_no"]) ? null : Convert.ToString(row["is_edit"]);
                    }

                    var supplierSparepart = new List<ServicePartSupplier>();
                    List<SqlParameter> arrParm2 = new List<SqlParameter>
                        {
                            new SqlParameter("@item_id", SqlDbType.Int) { Value = id },
                            new SqlParameter("@item_type", SqlDbType.VarChar,1) { Value = "S" }
                        };
                    conn.Open();
                    var dsResult = SqlHelper.ExecuteDataset(conn, "sp_item_mapping_supplier_list", arrParm2.ToArray());
                    conn.Close();
                    if (dsResult.Tables.Count > 0)
                    {
                        var data = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                        if (data != null)
                        {
                            foreach (var row in data)
                            {
                                supplierSparepart.Add(new ServicePartSupplier()
                                {
                                    id = Convert.ToInt32(row["id"]),
                                    is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                    part_id = Convert.ToInt32(row["part_id"]),
                                    part_name = Convert.IsDBNull("part_name") ? string.Empty : Convert.ToString(row["part_name"]),
                                    part_no = Convert.IsDBNull("part_no") ? string.Empty : Convert.ToString(row["part_no"]),
                                    supplier_id = Convert.ToInt32(row["supplier_id"]),
                                    supplier_name = Convert.IsDBNull("supplier_name") ? string.Empty : Convert.ToString(row["supplier_name"]),

                                });

                                var checkSupplier = (from t in supplierData where t.data_value == Convert.ToInt32(row["supplier_id"]) select t).FirstOrDefault();
                                if (checkSupplier != null)
                                {
                                    supplierData.Remove(checkSupplier);
                                }
                            }
                        }
                    }

                    var shelfSparepart = new List<SparePartShelf>();
                    List<SqlParameter> arrParm3 = new List<SqlParameter>
                        {
                            new SqlParameter("@part_id", SqlDbType.Int) { Value = id },
                        };
                    conn.Open();
                    var dsResult2 = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_shelf_list", arrParm3.ToArray());
                    conn.Close();
                    if (dsResult2.Tables.Count > 0)
                    {
                        var data = (from t in dsResult2.Tables[0].AsEnumerable() select t).ToList();
                        if (data != null)
                        {
                            foreach (var row in data)
                            {
                                shelfSparepart.Add(new SparePartShelf()
                                {
                                    id = Convert.ToInt32(row["id"]),
                                    is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                    part_id = Convert.ToInt32(row["part_id"]),
                                    shelf_name = Convert.IsDBNull("shelf_name") ? string.Empty : Convert.ToString(row["shelf_name"]),
                                    shelf_id = Convert.ToInt32(row["shelf_id"]),

                                });

                                var checkShelf = (from t in shelfData where t.data_value == Convert.ToInt32(row["shelf_id"]) select t).FirstOrDefault();
                                if (checkShelf != null)
                                {
                                    shelfData.Remove(checkShelf);
                                }
                            }
                        }
                    }
                    HttpContext.Current.Session["SESSION_SUPPLIER_SPAREPART"] = supplierData;
                    HttpContext.Current.Session["SESSION_PART_SUPPLIER_SPAREPART"] = supplierSparepart;
                    HttpContext.Current.Session["SESSION_SHELF_SPAREPART"] = shelfData;
                    HttpContext.Current.Session["SESSION_PART_SHELF_SPAREPART"] = shelfSparepart;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sparePartData;
        }

        [WebMethod]
        public static ServicePartListList CopySparePartData(string id)
        {
            var sparePartData = new ServicePartListList();
            var dsDataSparePart = new DataSet();
            var supplierData = (List<DropdownListClass>)HttpContext.Current.Session["SESSION_SUPPLIER_SPAREPART"];
            var shelfData = (List<DropdownListClass>)HttpContext.Current.Session["SESSION_SHELF_SPAREPART"];
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {

                            new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(id) },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = "" }
                        };
                    conn.Open();
                    dsDataSparePart = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_list", arrParm.ToArray());
                    conn.Close();
                    if (dsDataSparePart.Tables.Count > 0)
                    {
                        var row = dsDataSparePart.Tables[0].Rows[0];

                        sparePartData.id = -1;//Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]);
                        sparePartData.part_no = Convert.IsDBNull(row["part_no"]) ? null : Convert.ToString(row["part_no"]);
                        sparePartData.secondary_code = Convert.IsDBNull(row["secondary_code"]) ? null : Convert.ToString(row["secondary_code"]);
                        sparePartData.cat_id = Convert.IsDBNull(row["cat_id"]) ? 0 : Convert.ToInt32(row["cat_id"]);
                        sparePartData.shelf_id = Convert.IsDBNull(row["shelf_id"]) ? 0 : Convert.ToInt32(row["shelf_id"]);
                        sparePartData.part_name_tha = Convert.IsDBNull(row["part_name_tha"]) ? null : Convert.ToString(row["part_name_tha"]);
                        sparePartData.part_name_eng = Convert.IsDBNull(row["part_name_eng"]) ? null : Convert.ToString(row["part_name_eng"]);
                        sparePartData.unit_code = Convert.IsDBNull(row["unit_code"]) ? null : Convert.ToString(row["unit_code"]);
                        //sparePartData.description_tha = Convert.IsDBNull(row["description_tha"]) ? null : Convert.ToString(row["description_tha"]);
                        //sparePartData.description_eng = Convert.IsDBNull(row["description_eng"]) ? null : Convert.ToString(row["description_eng"]);
                        sparePartData.brand_id = Convert.IsDBNull(row["brand_id"]) ? 0 : Convert.ToInt32(row["brand_id"]);
                        //sparePartData.stock_count_type = Convert.IsDBNull(row["stock_count_type"]) ? null : Convert.ToString(row["stock_count_type"]);
                        sparePartData.unit_id = Convert.IsDBNull(row["unit_id"]) ? 0 : Convert.ToInt32(row["unit_id"]);
                        sparePartData.selling_price = Convert.IsDBNull(row["selling_price"]) ? 0 : Convert.ToDecimal(row["selling_price"]);
                        sparePartData.min_selling_price = Convert.IsDBNull(row["min_selling_price"]) ? 0 : Convert.ToDecimal(row["min_selling_price"]);
                        sparePartData.quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]);
                        sparePartData.quantity_reserve = Convert.IsDBNull(row["quantity_reserve"]) ? 0 : Convert.ToInt32(row["quantity_reserve"]);
                        sparePartData.quantity_balance = Convert.IsDBNull(row["quantity_balance"]) ? 0 : Convert.ToInt32(row["quantity_balance"]);

                        sparePartData.cat_name_tha = Convert.IsDBNull(row["cat_name_tha"]) ? null : Convert.ToString(row["cat_name_tha"]);
                        sparePartData.shelf_name = Convert.IsDBNull(row["shelf_name"]) ? null : Convert.ToString(row["shelf_name"]);
                    }

                    var supplierSparepart = new List<ServicePartSupplier>();
                    List<SqlParameter> arrParm2 = new List<SqlParameter>
                        {
                            new SqlParameter("@item_id", SqlDbType.Int) { Value = id },
                            new SqlParameter("@item_type", SqlDbType.VarChar,1) { Value = "S" }
                        };
                    conn.Open();
                    var dsResult = SqlHelper.ExecuteDataset(conn, "sp_item_mapping_supplier_list", arrParm2.ToArray());
                    conn.Close();
                    if (dsResult.Tables.Count > 0)
                    {
                        var data = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                        if (data != null)
                        {
                            foreach (var row in data)
                            {
                                supplierSparepart.Add(new ServicePartSupplier()
                                {
                                    id = Convert.ToInt32(row["id"]),
                                    is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                    part_id = -1,//Convert.ToInt32(row["part_id"]),
                                    part_name = Convert.IsDBNull("part_name") ? string.Empty : Convert.ToString(row["part_name"]),
                                    part_no = Convert.IsDBNull("part_no") ? string.Empty : Convert.ToString(row["part_no"]),
                                    supplier_id = Convert.ToInt32(row["supplier_id"]),
                                    supplier_name = Convert.IsDBNull("supplier_name") ? string.Empty : Convert.ToString(row["supplier_name"]),

                                });

                                var checkSupplier = (from t in supplierData where t.data_value == Convert.ToInt32(row["supplier_id"]) select t).FirstOrDefault();
                                if (checkSupplier != null)
                                {
                                    supplierData.Remove(checkSupplier);
                                }
                            }
                        }
                    }

                    var shelfSparepart = new List<SparePartShelf>();
                    List<SqlParameter> arrParm3 = new List<SqlParameter>
                        {
                            new SqlParameter("@part_id", SqlDbType.Int) { Value = id },
                        };
                    conn.Open();
                    var dsResult2 = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_shelf_list", arrParm3.ToArray());
                    conn.Close();
                    if (dsResult2.Tables.Count > 0)
                    {
                        var data = (from t in dsResult2.Tables[0].AsEnumerable() select t).ToList();
                        if (data != null)
                        {
                            foreach (var row in data)
                            {
                                shelfSparepart.Add(new SparePartShelf()
                                {
                                    id = Convert.ToInt32(row["id"]),
                                    is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                    part_id = -1,//Convert.ToInt32(row["part_id"]),
                                    shelf_name = Convert.IsDBNull("shelf_name") ? string.Empty : Convert.ToString(row["shelf_name"]),
                                    shelf_id = Convert.ToInt32(row["shelf_id"]),

                                });

                                var checkShelf = (from t in shelfData where t.data_value == Convert.ToInt32(row["shelf_id"]) select t).FirstOrDefault();
                                if (checkShelf != null)
                                {
                                    shelfData.Remove(checkShelf);
                                }
                            }
                        }
                    }
                    HttpContext.Current.Session["SESSION_SUPPLIER_SPAREPART"] = supplierData;
                    HttpContext.Current.Session["SESSION_PART_SUPPLIER_SPAREPART"] = supplierSparepart;
                    HttpContext.Current.Session["SESSION_SHELF_SPAREPART"] = shelfData;
                    HttpContext.Current.Session["SESSION_PART_SHELF_SPAREPART"] = shelfSparepart;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sparePartData;
        }
        private void BindGridSupplier()
        {
            try
            {
                gridSupplier.DataSource = (from t in servicePartListSupplierList where t.is_delete == false select t).ToList();
                gridSupplier.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static string SelectedSupplier(string id, string name)
        {
            var returnData = string.Empty;
            var supplierDataList = (List<DropdownListClass>)HttpContext.Current.Session["SESSION_SUPPLIER_SPAREPART"];
            var suplierPartListMapping = (List<ServicePartSupplier>)HttpContext.Current.Session["SESSION_PART_SUPPLIER_SPAREPART"];

            if (suplierPartListMapping != null)
            {
                var checkExist = (from t in suplierPartListMapping
                                  where t.supplier_id == Convert.ToInt32(id)
                                  select t).FirstOrDefault();
                if (checkExist == null)
                {
                    suplierPartListMapping.Add(new ServicePartSupplier()
                    {
                        id = (suplierPartListMapping.Count + 1) * -1,
                        supplier_id = Convert.ToInt32(id),
                        supplier_name = name
                    });
                    var removeRow = (from t in supplierDataList where t.data_value == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (removeRow != null)
                    {
                        supplierDataList.Remove(removeRow);
                    }
                }
            }
            else
            {
                suplierPartListMapping = new List<ServicePartSupplier>();
                suplierPartListMapping.Add(new ServicePartSupplier()
                {
                    id = (suplierPartListMapping.Count + 1) * -1,
                    supplier_id = Convert.ToInt32(id),
                    supplier_name = name
                });
                var removeRow = (from t in supplierDataList where t.data_value == Convert.ToInt32(id) select t).FirstOrDefault();
                if (removeRow != null)
                {
                    supplierDataList.Remove(removeRow);
                }
            }
            HttpContext.Current.Session["SESSION_SUPPLIER_SPAREPART"] = supplierDataList;
            HttpContext.Current.Session["SESSION_PART_SUPPLIER_SPAREPART"] = suplierPartListMapping;


            return returnData;
        }
        [WebMethod]
        public static string DeleteSupplier(string id)
        {
            var returnData = string.Empty;
            var supplierDataList = (List<DropdownListClass>)HttpContext.Current.Session["SESSION_SUPPLIER_SPAREPART"];
            var suplierPartListMapping = (List<ServicePartSupplier>)HttpContext.Current.Session["SESSION_PART_SUPPLIER_SPAREPART"];

            if (suplierPartListMapping != null)
            {
                var checkExist = (from t in suplierPartListMapping
                                  where t.id == Convert.ToInt32(id)
                                  select t).FirstOrDefault();
                if (checkExist != null)
                {
                    if (checkExist.id < 0)
                    {
                        suplierPartListMapping.Remove(checkExist);
                    }
                    else
                    {
                        checkExist.is_delete = true;
                    }
                    supplierDataList.Add(new DropdownListClass()
                    {
                        data_text = checkExist.supplier_name,
                        data_value = checkExist.supplier_id
                    });
                }
            }

            HttpContext.Current.Session["SESSION_SUPPLIER_SPAREPART"] = supplierDataList;
            HttpContext.Current.Session["SESSION_PART_SUPPLIER_SPAREPART"] = suplierPartListMapping;


            return returnData;
        }


        protected void gridSupplier_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridSupplier.DataSource = (from t in servicePartListSupplierList where t.is_delete == false select t).ToList();
            gridSupplier.DataBind();
        }

        protected void cboSupplierSparepart_Callback(object sender, CallbackEventArgsBase e)
        {
            try
            {
                cboSupplierSparepart.DataSource = supplierList;
                cboSupplierSparepart.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [WebMethod]
        public static string DeleteSparePart(string id)
        {
            string returnData = "error";
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    if (Convert.ToInt32(id) > 0)
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_product_spare_part_delete", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
                return returnData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void cboShelf_Callback(object sender, CallbackEventArgsBase e)
        {
            try
            {
                cboShelf.DataSource = shelfList;
                cboShelf.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gridViewShelf_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                gridViewShelf.DataSource = (from t in sparePartShelfList where t.is_delete == false select t).ToList();
                gridViewShelf.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindGridShelf()
        {
            try
            {
                gridViewShelf.DataSource = (from t in sparePartShelfList where t.is_delete == false select t).ToList();
                gridViewShelf.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [WebMethod]
        public static string SelectedShelf(string id, string name)
        {
            var returnData = string.Empty;
            var selfDataList = (List<DropdownListClass>)HttpContext.Current.Session["SESSION_SHELF_SPAREPART"];
            var selfSparePartMapping = (List<SparePartShelf>)HttpContext.Current.Session["SESSION_PART_SHELF_SPAREPART"];

            if (selfSparePartMapping != null)
            {
                var checkExist = (from t in selfSparePartMapping
                                  where t.shelf_id == Convert.ToInt32(id)
                                  select t).FirstOrDefault();
                if (checkExist == null)
                {
                    selfSparePartMapping.Add(new SparePartShelf()
                    {
                        id = (selfSparePartMapping.Count + 1) * -1,
                        shelf_id = Convert.ToInt32(id),
                        shelf_name = name
                    });
                    var removeRow = (from t in selfDataList where t.data_value == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (removeRow != null)
                    {
                        selfDataList.Remove(removeRow);
                    }
                }
            }
            else
            {
                selfSparePartMapping = new List<SparePartShelf>();
                selfSparePartMapping.Add(new SparePartShelf()
                {
                    id = (selfSparePartMapping.Count + 1) * -1,
                    shelf_id = Convert.ToInt32(id),
                    shelf_name = name
                });
                var removeRow = (from t in selfDataList where t.data_value == Convert.ToInt32(id) select t).FirstOrDefault();
                if (removeRow != null)
                {
                    selfDataList.Remove(removeRow);
                }
            }
            HttpContext.Current.Session["SESSION_SHELF_SPAREPART"] = selfDataList;
            HttpContext.Current.Session["SESSION_PART_SHELF_SPAREPART"] = selfSparePartMapping;


            return returnData;
        }

        [WebMethod]
        public static string DeleteShelf(string id)
        {
            var returnData = string.Empty;
            var shelfDataList = (List<DropdownListClass>)HttpContext.Current.Session["SESSION_SHELF_SPAREPART"];
            var shelfSparePartMapping = (List<SparePartShelf>)HttpContext.Current.Session["SESSION_PART_SHELF_SPAREPART"];

            if (shelfSparePartMapping != null)
            {
                var checkExist = (from t in shelfSparePartMapping
                                  where t.id == Convert.ToInt32(id)
                                  select t).FirstOrDefault();
                if (checkExist != null)
                {
                    if (checkExist.id < 0)
                    {
                        shelfSparePartMapping.Remove(checkExist);
                    }
                    else
                    {
                        checkExist.is_delete = true;
                    }
                    shelfDataList.Add(new DropdownListClass()
                    {
                        data_text = checkExist.shelf_name,
                        data_value = checkExist.id
                    });
                }
            }

            HttpContext.Current.Session["SESSION_SHELF_SPAREPART"] = shelfDataList;
            HttpContext.Current.Session["SESSION_PART_SHELF_SPAREPART"] = shelfSparePartMapping;


            return returnData;
        }

        protected void gridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var dsResult = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = e.Parameters.ToString() }

                        };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_list", arrParm.ToArray());
                }

                Session["SEARCH"] = e.Parameters.ToString();

                sparePartList = new List<ServicePartListList>();
                foreach (var row in dsResult.Tables[0].AsEnumerable())
                {
                    sparePartList.Add(new ServicePartListList()
                    {
                        id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                        part_no = Convert.IsDBNull(row["part_no"]) ? null : Convert.ToString(row["part_no"]),
                        secondary_code = Convert.IsDBNull(row["secondary_code"]) ? null : Convert.ToString(row["secondary_code"]),
                        cat_id = Convert.IsDBNull(row["cat_id"]) ? 0 : Convert.ToInt32(row["cat_id"]),
                        shelf_id = Convert.IsDBNull(row["shelf_id"]) ? 0 : Convert.ToInt32(row["shelf_id"]),
                        shelf_name = Convert.IsDBNull(row["shelf_name"]) ? "" : Convert.ToString(row["shelf_name"]),
                        cat_name_tha = Convert.IsDBNull(row["cat_name_tha"]) ? "" : Convert.ToString(row["cat_name_tha"]),
                        part_name_tha = Convert.IsDBNull(row["part_name_tha"]) ? null : Convert.ToString(row["part_name_tha"]),
                        part_name_eng = Convert.IsDBNull(row["part_name_eng"]) ? null : Convert.ToString(row["part_name_eng"]),
                        brand_id = Convert.IsDBNull(row["brand_id"]) ? 0 : Convert.ToInt32(row["brand_id"]),
                        unit_id = Convert.IsDBNull(row["unit_id"]) ? 0 : Convert.ToInt32(row["unit_id"]),
                        unit_code = Convert.IsDBNull(row["unit_code"]) ? null : Convert.ToString(row["unit_code"]),
                        selling_price = Convert.IsDBNull(row["selling_price"]) ? 0 : Convert.ToDecimal(row["selling_price"]),
                        min_selling_price = Convert.IsDBNull(row["min_selling_price"]) ? 0 : Convert.ToDecimal(row["min_selling_price"]),
                        quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]),
                        quantity_reserve = Convert.IsDBNull(row["quantity_reserve"]) ? 0 : Convert.ToInt32(row["quantity_reserve"]),
                        quantity_balance = Convert.IsDBNull(row["quantity_balance"]) ? 0 : Convert.ToInt32(row["quantity_balance"]),
                        is_enable = Convert.IsDBNull(row["is_enable"]) ? false : Convert.ToBoolean(row["is_enable"])
                    });
                }

                //Bind data into GridView
                Session["SESSION_SPARE_PART_MASTER"] = sparePartList;
                gridView.DataSource = sparePartList;
                gridView.FilterExpression = FilterBag.GetExpression(false);
                gridView.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void gridView_PageIndexChanged(object sender, EventArgs e)
        {
            int pageIndex = (sender as ASPxGridView).PageIndex;
            Session["PAGE"] = pageIndex;
        }

        protected void gridView_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            string column = e.Column.FieldName;
            int order = Convert.ToInt32(e.Column.SortOrder);

            Session["COLUMN"] = column;
            Session["ORDER"] = order;
        }
    }
}