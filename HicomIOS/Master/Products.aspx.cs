using System;
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
using System.Globalization;

namespace HicomIOS.Master
{
    public partial class Products : MasterDetailPage
    {
        public class ProductList
        {
            public int id { get; set; }
            public string product_no { get; set; }
            public string serial_no { get; set; }
            public int cat_id { get; set; }
            public int brand_id { get; set; }
            public string product_model { get; set; }
            public string product_name_tha { get; set; }
            public string product_name_eng { get; set; }
            public string description_tha { get; set; }
            public string description_eng { get; set; }
            public string quotation_desc_line1 { get; set; }
            public string quotation_desc_line2 { get; set; }
            public string quotation_desc_line3 { get; set; }
            public string quotation_desc_line4 { get; set; }
            public string quotation_desc_line5 { get; set; }
            public string quotation_desc_line6 { get; set; }
            public string quotation_desc_line7 { get; set; }
            public string quotation_desc_line8 { get; set; }
            public string quotation_desc_line9 { get; set; }
            public string quotation_desc_line10 { get; set; }
            public int quantity { get; set; }
            public int quantity_reserve { get; set; }
            public int quantity_balance { get; set; }
            public int unit_id { get; set; }
            public decimal selling_price { get; set; }
            public decimal min_selling_price { get; set; }
            public string remark { get; set; }
            public bool is_enable { get; set; }
            public bool is_delete { get; set; }
            public string pressure { get; set; }
            public string power_supply { get; set; }
            public int phase { get; set; }
            public int hz { get; set; }
            public string unit_warranty { get; set; }
            public string air_end_warranty { get; set; }
            public string is_edit { get; set; }

        }
        public class SupplierDataList
        {
            public int data_value { get; set; }
            public string data_text { get; set; }
            public string data_active { get; set; }
        }
        public class ProductSupplier
        {
            public int id { get; set; }
            public int product_id { get; set; }
            public string product_no { get; set; }
            public string product_name { get; set; }
            public string supplier_name { get; set; }
            public int supplier_id { get; set; }
            public bool is_delete { get; set; }
            public string isactive { get; set; }
        }
        public class ProductMFG
        {
            public int id { get; set; }
            public bool is_delete { get; set; }
            public int product_id { get; set; }
            public string mfg_no { get; set; }
            public int issue_id { get; set; }
            public string issue_no { get; set; }
            public string display_issue_date { get; set; }
            public DateTime? receive_date { get; set; }
            public string display_receive_date { get; set; }
            public int purchase_request_id { get; set; }
            public string purchase_request_no { get; set; }
            public string customer_id { get; set; }
            public string model { get; set; }
            public string customer_mfg_id { get; set; }
        }
       
        List<SupplierDataList> supplierList
        {
            get
            {
                if (Session["SESSION_SUPPLIER_PRODUCT"] == null)
                    Session["SESSION_SUPPLIER_PRODUCT"] = new List<SupplierDataList>();
                return (List<SupplierDataList>)Session["SESSION_SUPPLIER_PRODUCT"];
            }
            set
            {
                Session["SESSION_SUPPLIER_PRODUCT"] = value;
            }
        }
        List<ProductSupplier> productSupplierList
        {
            get
            {
                if (Session["SESSION_PRODUCT_SUPPLIER_PRODUCT"] == null)
                    Session["SESSION_PRODUCT_SUPPLIER_PRODUCT"] = new List<ProductSupplier>();
                return (List<ProductSupplier>)Session["SESSION_PRODUCT_SUPPLIER_PRODUCT"];
            }
            set
            {
                Session["SESSION_PRODUCT_SUPPLIER_PRODUCT"] = value;
            }
        }

        List<ProductMFG> productMFGList
        {
            get
            {
                if (Session["SESSION_PRODUCT_MFG_PRODUCT"] == null)
                    Session["SESSION_PRODUCT_MFG_PRODUCT"] = new List<ProductMFG>();
                return (List<ProductMFG>)Session["SESSION_PRODUCT_MFG_PRODUCT"];
            }
            set
            {
                Session["SESSION_PRODUCT_MFG_PRODUCT"] = value;
            }
        }
        private DataSet dsResult;
        public override string PageName { get { return "Products"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }
        public override long SelectedItemID
        {
            get
            {
                var ItemID = gridView.GetRowValues(gridView.FocusedRowIndex, gridView.KeyFieldName);
                return ItemID != null ? (int)ItemID : DataListUtil.emptyEntryID;
            }
            set
            {
                gridView.FocusedRowIndex = gridView.FindVisibleIndexByKeyValue(value);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Bind Data into GridView
            if (!Page.IsPostBack)
            {
                // Get Permission and if no permission, will redirect to another page.
                if (!Permission.GetPermission())
                    Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

                ClearWorkingSession();
                PrepareData();
                dsResult = null;
                BindGrid(true);
                //if (Session["row_id"] != null)
                //{
                //    SelectedItemID = Convert.ToInt64(Session["row_id"]);
                //    Session["row_id"] = null;
                //}
            }
            else
            {
                dsResult = (DataSet)Session["SESSION_PRODUCT_MASTER"];
                BindGrid(false);
                BindGridMFG();
                BindGridSupplier();
            }
        }

        protected void PrepareData()
        {
            try
            {
                // Load Combobox Product Category Data
                SPlanetUtil.BindASPxComboBox(ref cboCateID, DataListUtil.DropdownStoreProcedureName.Product_Category);
                cboCateID.Items.Insert(0, new ListEditItem("--โปรดเลือก--", "0"));

                // Load Combobox Product Unit Type Data
                SPlanetUtil.BindASPxComboBox(ref cboUnitType, DataListUtil.DropdownStoreProcedureName.Product_Unit);
                cboUnitType.Items.Insert(0, new ListEditItem("--โปรดเลือก--", "0"));

                // Load Combobox Product Brand Data
                SPlanetUtil.BindASPxComboBox(ref cboBrand, DataListUtil.DropdownStoreProcedureName.Product_Brand);
                cboBrand.Items.Insert(0, new ListEditItem("--โปรดเลือก--", "0"));

                // Load Combobox Supplier Data
                SPlanetUtil.BindASPxComboBox(ref cboSupplier, DataListUtil.DropdownStoreProcedureName.Supplier);
                cboSupplier.Items.Insert(0, new ListEditItem("--โปรดเลือก--", "0"));

                // Setting height gridView
                gridView.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen);
                gridView.SettingsBehavior.AllowFocusedRow = true;

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
                            supplierList.Add(new SupplierDataList()
                            {
                                data_value = Convert.ToInt32(row["id"]),
                                data_text = string.Format("{0}{1}", Convert.IsDBNull(row["supplier_code"]) ? string.Empty : Convert.ToString(row["supplier_code"] + " - "),
                                Convert.IsDBNull(row["supplier_name_tha"]) ? string.Empty : Convert.ToString(row["supplier_name_tha"]))

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
        private void ClearWorkingSession()
        {
            Session.Remove("SESSION_SUPPLIER_PRODUCT");
            Session.Remove("SESSION_PRODUCT_SUPPLIER_PRODUCT");
            Session.Remove("SESSION_PRODUCT_MFG_PRODUCT");
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

        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            return result.OfType<FilterControlColumn>();
        }
        #endregion

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
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = search },
                            new SqlParameter("@product_no", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                        };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_list_master", arrParm.ToArray());
                        /*int i = 0;
                        int rowId = 0;
                        if (Session["ROW_ID"] != null)
                        {
                            rowId = Convert.ToInt32(Session["ROW_ID"].ToString());
                        }
                        foreach (var row in dsResult.Tables[0].AsEnumerable())
                        {
                            if (rowId == Convert.ToInt32(row["id"]))
                            {
                                int selectedRow = i;
                                int prevRow = Convert.ToInt32(Session["ROW"]);
                                int pageSize = gridView.SettingsPager.PageSize;
                                int pageIndex = (int)(selectedRow / pageSize);
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
                            }
                            i++;
                        }*/
                        Session["SESSION_PRODUCT_MASTER"] = dsResult;
                    }
                }

                //Bind data into GridView
                gridView.DataSource = dsResult;
                gridView.DataBind();

                //BindGridSupplier();

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
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        [WebMethod]
        public static string SelectedSupplier(string id, string name)
        {
            var returnData = string.Empty;
            var supplierDataList = (List<SupplierDataList>)HttpContext.Current.Session["SESSION_SUPPLIER_PRODUCT"];
            var suplierProductMapping = (List<ProductSupplier>)HttpContext.Current.Session["SESSION_PRODUCT_SUPPLIER_PRODUCT"];

            if (suplierProductMapping != null)
            {
                var checkExist = (from t in suplierProductMapping
                                  where t.supplier_id == Convert.ToInt32(id)
                                  select t).FirstOrDefault();
                if (checkExist == null)
                {
                    suplierProductMapping.Add(new ProductSupplier()
                    {
                        id = (suplierProductMapping.Count + 1) * -1,
                        supplier_id = Convert.ToInt32(id),
                        supplier_name = name,
                         isactive  = supplierDataList.Where(a => a.data_value == Convert.ToInt32(id)).FirstOrDefault().data_active
                    });
                    /*var removeRow = (from t in supplierDataList where t.data_value == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (removeRow != null)
                    {
                        supplierDataList.Remove(removeRow);
                    }*/
                } else
                {
                    returnData = "error";
                }
            }
            else
            {
                suplierProductMapping = new List<ProductSupplier>();
                suplierProductMapping.Add(new ProductSupplier()
                {
                    id = (suplierProductMapping.Count + 1) * -1,
                    supplier_id = Convert.ToInt32(id),
                    supplier_name = name,
                    isactive = supplierDataList.Where(a => a.data_value == Convert.ToInt32(id)).FirstOrDefault().data_active
                });
                /*var removeRow = (from t in supplierDataList where t.data_value == Convert.ToInt32(id) select t).FirstOrDefault();
                if (removeRow != null)
                {
                    supplierDataList.Remove(removeRow);
                }*/
            }
            HttpContext.Current.Session["SESSION_SUPPLIER_PRODUCT"] = supplierDataList;
            HttpContext.Current.Session["SESSION_PRODUCT_SUPPLIER_PRODUCT"] = suplierProductMapping;


            return returnData;
        }
        [WebMethod]
        public static string DeleteSupplier(string id)
        {
            var returnData = string.Empty;
            var supplierDataList = (List<SupplierDataList>)HttpContext.Current.Session["SESSION_SUPPLIER_PRODUCT"];
            var suplierProducttMapping = (List<ProductSupplier>)HttpContext.Current.Session["SESSION_PRODUCT_SUPPLIER_PRODUCT"];

            if (suplierProducttMapping != null)
            {
                var checkExist = (from t in suplierProducttMapping
                                  where t.id == Convert.ToInt32(id)
                                  select t).FirstOrDefault();
                if (checkExist != null)
                {
                    if (checkExist.id < 0)
                    {
                        suplierProducttMapping.Remove(checkExist);
                    }
                    else
                    {
                        checkExist.is_delete = true;
                    }
                    supplierDataList.Add(new SupplierDataList()
                    {
                        data_text = checkExist.supplier_name,
                        data_value = checkExist.supplier_id
                    });
                }
            }

            HttpContext.Current.Session["SESSION_SUPPLIER_PRODUCT"] = supplierDataList;
            HttpContext.Current.Session["SESSION_PRODUCT_SUPPLIER_PRODUCT"] = suplierProducttMapping;


            return returnData;
        }
        protected void supplierGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            supplierGrid.DataSource = (from t in productSupplierList where t.is_delete == false select t).ToList();
            supplierGrid.DataBind();
        }
        private void BindGridSupplier()
        {
            supplierGrid.DataSource = (from t in productSupplierList where t.is_delete == false select t).ToList();
            supplierGrid.DataBind();
        }
        protected void cboSupplier_Callback(object sender, CallbackEventArgsBase e)
        {
            cboSupplier.DataSource = supplierList;
            cboSupplier.DataBind();
        }
        [WebMethod]
        public static string NewProductData()
        {
            try
            {
                var dsResult = new DataSet();
                var supplierList = new List<SupplierDataList>();
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
                            supplierList.Add(new SupplierDataList()
                            {
                                data_value = Convert.ToInt32(row["id"]),
                                data_text = string.Format("{0}{1}", Convert.IsDBNull(row["supplier_code"]) ? string.Empty : Convert.ToString(row["supplier_code"] + " - " ), 
                                Convert.IsDBNull(row["supplier_name_tha"]) ? string.Empty : Convert.ToString(row["supplier_name_tha"]))
                                ,data_active = Convert.IsDBNull(row["is_enable"]) ? string.Empty : Convert.ToString(row["is_enable"]) == "True" ? "Active" : "InActive"
                            });
                        }
                    }
                }
                HttpContext.Current.Session["SESSION_SUPPLIER_PRODUCT"] = supplierList;
                HttpContext.Current.Session.Remove("SESSION_PRODUCT_SUPPLIER_PRODUCT");
                HttpContext.Current.Session.Remove("SESSION_PRODUCT_MFG_PRODUCT");
                return "SUCCESS";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [WebMethod]
        public static ProductList GetEditProductData(string id, int index)
        {
            //  Set active row
            HttpContext.Current.Session["ROW_ID"] = id;
            HttpContext.Current.Session["ROW"] = index;

            var productData = new ProductList();
            var dsDataProduct = new DataSet();
            var supplierData = (List<SupplierDataList>)HttpContext.Current.Session["SESSION_SUPPLIER_PRODUCT"];

            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@product_no", SqlDbType.VarChar,50) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },
                        };
                    conn.Open();
                    dsDataProduct = SqlHelper.ExecuteDataset(conn, "sp_product_list_master", arrParm.ToArray());
                    conn.Close();

                    if (dsDataProduct.Tables.Count > 0)
                    {
                        var row = dsDataProduct.Tables[0].Rows[0];

                        productData.id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]);
                        productData.product_no = Convert.IsDBNull(row["product_no"]) ? null : Convert.ToString(row["product_no"]);
                        productData.product_model = Convert.IsDBNull(row["product_model"]) ? null : Convert.ToString(row["product_model"]);
                        productData.description_tha = Convert.IsDBNull(row["description_tha"]) ? null : Convert.ToString(row["description_tha"]);
                        productData.description_eng = Convert.IsDBNull(row["description_eng"]) ? null : Convert.ToString(row["description_eng"]);
                        productData.brand_id = Convert.IsDBNull(row["brand_id"]) ? 0 : Convert.ToInt32(row["brand_id"]);

                        productData.unit_id = Convert.IsDBNull(row["unit_id"]) ? 0 : Convert.ToInt32(row["unit_id"]);
                        productData.selling_price = Convert.IsDBNull(row["selling_price"]) ? 0 : Convert.ToDecimal(row["selling_price"]);
                        productData.min_selling_price = Convert.IsDBNull(row["min_selling_price"]) ? 0 : Convert.ToDecimal(row["min_selling_price"]);
                        productData.quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]);
                        productData.quantity_reserve = Convert.IsDBNull(row["quantity_reserve"]) ? 0 : Convert.ToInt32(row["quantity_reserve"]);
                        productData.quantity_balance = Convert.IsDBNull(row["quantity_balance"]) ? 0 : Convert.ToInt32(row["quantity_balance"]);
                        productData.cat_id = Convert.IsDBNull(row["cat_id"]) ? 0 : Convert.ToInt32(row["cat_id"]);
                        productData.product_name_eng = Convert.IsDBNull(row["product_name_eng"]) ? null : Convert.ToString(row["product_name_eng"]);
                        productData.product_name_tha = Convert.IsDBNull(row["product_name_tha"]) ? null : Convert.ToString(row["product_name_tha"]);
                        productData.quotation_desc_line1 = Convert.IsDBNull(row["quotation_desc_line1"]) ? null : Convert.ToString(row["quotation_desc_line1"]);
                        productData.quotation_desc_line2 = Convert.IsDBNull(row["quotation_desc_line2"]) ? null : Convert.ToString(row["quotation_desc_line2"]);
                        productData.quotation_desc_line3 = Convert.IsDBNull(row["quotation_desc_line3"]) ? null : Convert.ToString(row["quotation_desc_line3"]);
                        productData.quotation_desc_line4 = Convert.IsDBNull(row["quotation_desc_line4"]) ? null : Convert.ToString(row["quotation_desc_line4"]);
                        productData.quotation_desc_line5 = Convert.IsDBNull(row["quotation_desc_line5"]) ? null : Convert.ToString(row["quotation_desc_line5"]);
                        productData.quotation_desc_line6 = Convert.IsDBNull(row["quotation_desc_line6"]) ? null : Convert.ToString(row["quotation_desc_line6"]);
                        productData.quotation_desc_line7 = Convert.IsDBNull(row["quotation_desc_line7"]) ? null : Convert.ToString(row["quotation_desc_line7"]);
                        productData.quotation_desc_line8 = Convert.IsDBNull(row["quotation_desc_line8"]) ? null : Convert.ToString(row["quotation_desc_line8"]);
                        productData.quotation_desc_line9 = Convert.IsDBNull(row["quotation_desc_line9"]) ? null : Convert.ToString(row["quotation_desc_line9"]);
                        productData.quotation_desc_line10 = Convert.IsDBNull(row["quotation_desc_line10"]) ? null : Convert.ToString(row["quotation_desc_line10"]);
                        productData.remark = Convert.IsDBNull(row["remark"]) ? null : Convert.ToString(row["remark"]);
                        productData.serial_no = Convert.IsDBNull(row["serial_no"]) ? null : Convert.ToString(row["serial_no"]);

                        productData.pressure = Convert.IsDBNull(row["pressure"]) ? null : Convert.ToString(row["pressure"]);
                        productData.power_supply = Convert.IsDBNull(row["power_supply"]) ? null : Convert.ToString(row["power_supply"]);
                        productData.phase = Convert.IsDBNull(row["phase"]) ? 0 : Convert.ToInt32(row["phase"]);
                        productData.hz = Convert.IsDBNull(row["hz"]) ? 0 : Convert.ToInt32(row["hz"]);
                        productData.unit_warranty = Convert.IsDBNull(row["unit_warranty"]) ? null : Convert.ToString(row["unit_warranty"]);
                        productData.air_end_warranty = Convert.IsDBNull(row["air_end_warranty"]) ? null : Convert.ToString(row["air_end_warranty"]);
                        productData.is_edit= Convert.IsDBNull(row["is_edit"]) ? null : Convert.ToString(row["is_edit"]);
                    }


                    List<SqlParameter> arrParm3 = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 }
                        };
                    conn.Open();
                    var dsSupplier = SqlHelper.ExecuteDataset(conn, "sp_supplier_list", arrParm3.ToArray());
                    conn.Close();

                    if (dsSupplier.Tables.Count > 0)
                    {
                        var data = (from t in dsSupplier.Tables[0].AsEnumerable() select t).ToList();
                        if (data != null)
                        {
                            if (supplierData == null)
                            {
                                supplierData.Clear();
                            } else
                            {
                                supplierData = new List<SupplierDataList>();
                            }
                            foreach (var row in data)
                            {
                                supplierData.Add(new SupplierDataList()
                                {
                                    data_value = Convert.ToInt32(row["id"]),
                                    data_text = string.Format("{0}{1}", Convert.IsDBNull(row["supplier_code"]) ? string.Empty : Convert.ToString(row["supplier_code"] + " - "),
                                Convert.IsDBNull(row["supplier_name_tha"]) ? string.Empty : Convert.ToString(row["supplier_name_tha"])),
                                 data_active = Convert.IsDBNull(row["is_enable"]) ? string.Empty : Convert.ToString(row["is_enable"]) == "True" ? "Active" : "InActive"
                                });
                            }
                        }
                    }

                    var productSupplier = new List<ProductSupplier>();
                    List<SqlParameter> arrParm2 = new List<SqlParameter>
                        {
                            new SqlParameter("@item_id", SqlDbType.Int) { Value = id },
                            new SqlParameter("@item_type", SqlDbType.VarChar,1) { Value = "P" }
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
                                productSupplier.Add(new ProductSupplier()
                                {
                                    id = Convert.ToInt32(row["id"]),
                                    is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                    product_id = Convert.ToInt32(row["product_id"]),
                                    product_name = Convert.IsDBNull("product_name") ? string.Empty : Convert.ToString(row["product_name"]),
                                    product_no = Convert.IsDBNull("product_no") ? string.Empty : Convert.ToString(row["product_no"]),
                                    supplier_id = Convert.ToInt32(row["supplier_id"]),
                                    supplier_name = Convert.IsDBNull("supplier_name") ? string.Empty : Convert.ToString(row["supplier_name"]),
                                    isactive = Convert.IsDBNull("isactive") ? string.Empty : Convert.ToString(row["isactive"])
                                });

                                /*var checkSupplier = (from t in supplierData where t.data_value == Convert.ToInt32(row["supplier_id"]) select t).FirstOrDefault();
                                if (checkSupplier != null)
                                {
                                    supplierData.Remove(checkSupplier);
                                }*/
                            }
                        }
                    }

                    var mfgProductList = new List<ProductMFG>();
                    List<SqlParameter> arrParm4 = new List<SqlParameter>
                        {
                            new SqlParameter("@product_id", SqlDbType.Int) { Value = id },
                        };
                    conn.Open();
                    var dsResult4 = SqlHelper.ExecuteDataset(conn, "sp_product_mfg_list", arrParm4.ToArray());
                    conn.Close();
                    if (dsResult4.Tables.Count > 0)
                    {
                        var data = (from t in dsResult4.Tables[0].AsEnumerable() select t).ToList();
                        if (data != null)
                        {
                            foreach (var row in data)
                            {
                                var item = (new ProductMFG()
                                {
                                    id = Convert.ToInt32(row["id"]),
                                    product_id = Convert.ToInt32(row["product_id"]),
                                    mfg_no = Convert.IsDBNull("mfg_no") ? string.Empty : Convert.ToString(row["mfg_no"]),
                                    purchase_request_id = Convert.IsDBNull("purchase_request_id") ? 0 : Convert.ToInt32(row["purchase_request_id"]),
                                    purchase_request_no = Convert.IsDBNull("purchase_request_no") ? string.Empty : Convert.ToString(row["purchase_request_no"]),
                                    display_receive_date = Convert.IsDBNull("display_receive_date") ? string.Empty : Convert.ToString(row["display_receive_date"]),
                                    issue_id = Convert.IsDBNull("issue_id") ? 0 : Convert.ToInt32(row["issue_id"]),
                                    issue_no = Convert.IsDBNull("issue_no") ? string.Empty : Convert.ToString(row["issue_no"]),
                                    display_issue_date = Convert.IsDBNull("display_issue_date") ? string.Empty : Convert.ToString(row["display_issue_date"]),
                                    customer_id = Convert.IsDBNull("customer_id") ? string.Empty : Convert.ToString(row["customer_id"]),
                                    model = Convert.IsDBNull("model") ? string.Empty : Convert.ToString(row["model"]),
                                    customer_mfg_id = Convert.IsDBNull("customer_mfg_id") ? string.Empty : Convert.ToString(row["customer_mfg_id"]),
                                });

                                if (!(row["receive_date"] is DBNull))
                                {
                                    item.receive_date = Convert.ToDateTime(row["receive_date"]);
                                }
                                mfgProductList.Add(item);
                            }
                        }
                    }

                    HttpContext.Current.Session["SESSION_PRODUCT_MFG_PRODUCT"] = mfgProductList;
                    HttpContext.Current.Session["SESSION_SUPPLIER_PRODUCT"] = supplierData;
                    HttpContext.Current.Session["SESSION_PRODUCT_SUPPLIER_PRODUCT"] = productSupplier;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return productData;
        }

        [WebMethod]
        public static ProductList CopyProductData(string id)
        {
            var productData = new ProductList();
            var dsDataProduct = new DataSet();
            var supplierData = (List<SupplierDataList>)HttpContext.Current.Session["SESSION_SUPPLIER_PRODUCT"];

            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@product_no", SqlDbType.VarChar,50) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },
                        };
                    conn.Open();
                    dsDataProduct = SqlHelper.ExecuteDataset(conn, "sp_product_list_master", arrParm.ToArray());
                    conn.Close();

                    if (dsDataProduct.Tables.Count > 0)
                    {
                        var row = dsDataProduct.Tables[0].Rows[0];

                        productData.id = -1;//Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]);
                        productData.product_no = Convert.IsDBNull(row["product_no"]) ? null : Convert.ToString(row["product_no"]);
                        productData.product_model = Convert.IsDBNull(row["product_model"]) ? null : Convert.ToString(row["product_model"]);
                        productData.description_tha = Convert.IsDBNull(row["description_tha"]) ? null : Convert.ToString(row["description_tha"]);
                        productData.description_eng = Convert.IsDBNull(row["description_eng"]) ? null : Convert.ToString(row["description_eng"]);
                        productData.brand_id = Convert.IsDBNull(row["brand_id"]) ? 0 : Convert.ToInt32(row["brand_id"]);

                        productData.unit_id = Convert.IsDBNull(row["unit_id"]) ? 0 : Convert.ToInt32(row["unit_id"]);
                        productData.selling_price = Convert.IsDBNull(row["selling_price"]) ? 0 : Convert.ToDecimal(row["selling_price"]);
                        productData.min_selling_price = Convert.IsDBNull(row["min_selling_price"]) ? 0 : Convert.ToDecimal(row["min_selling_price"]);
                        productData.quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]);
                        productData.quantity_reserve = Convert.IsDBNull(row["quantity_reserve"]) ? 0 : Convert.ToInt32(row["quantity_reserve"]);
                        productData.quantity_balance = Convert.IsDBNull(row["quantity_balance"]) ? 0 : Convert.ToInt32(row["quantity_balance"]);
                        productData.cat_id = Convert.IsDBNull(row["cat_id"]) ? 0 : Convert.ToInt32(row["cat_id"]);
                        productData.product_name_eng = Convert.IsDBNull(row["product_name_eng"]) ? null : Convert.ToString(row["product_name_eng"]);
                        productData.product_name_tha = Convert.IsDBNull(row["product_name_tha"]) ? null : Convert.ToString(row["product_name_tha"]);
                        productData.quotation_desc_line1 = Convert.IsDBNull(row["quotation_desc_line1"]) ? null : Convert.ToString(row["quotation_desc_line1"]);
                        productData.quotation_desc_line2 = Convert.IsDBNull(row["quotation_desc_line2"]) ? null : Convert.ToString(row["quotation_desc_line2"]);
                        productData.quotation_desc_line3 = Convert.IsDBNull(row["quotation_desc_line3"]) ? null : Convert.ToString(row["quotation_desc_line3"]);
                        productData.quotation_desc_line4 = Convert.IsDBNull(row["quotation_desc_line4"]) ? null : Convert.ToString(row["quotation_desc_line4"]);
                        productData.quotation_desc_line5 = Convert.IsDBNull(row["quotation_desc_line5"]) ? null : Convert.ToString(row["quotation_desc_line5"]);
                        productData.quotation_desc_line6 = Convert.IsDBNull(row["quotation_desc_line6"]) ? null : Convert.ToString(row["quotation_desc_line6"]);
                        productData.quotation_desc_line7 = Convert.IsDBNull(row["quotation_desc_line7"]) ? null : Convert.ToString(row["quotation_desc_line7"]);
                        productData.quotation_desc_line8 = Convert.IsDBNull(row["quotation_desc_line8"]) ? null : Convert.ToString(row["quotation_desc_line8"]);
                        productData.quotation_desc_line9 = Convert.IsDBNull(row["quotation_desc_line9"]) ? null : Convert.ToString(row["quotation_desc_line9"]);
                        productData.quotation_desc_line10 = Convert.IsDBNull(row["quotation_desc_line10"]) ? null : Convert.ToString(row["quotation_desc_line10"]);
                        productData.remark = Convert.IsDBNull(row["remark"]) ? null : Convert.ToString(row["remark"]);
                        productData.serial_no = Convert.IsDBNull(row["serial_no"]) ? null : Convert.ToString(row["serial_no"]);

                        productData.pressure = Convert.IsDBNull(row["pressure"]) ? null : Convert.ToString(row["pressure"]);
                        productData.power_supply = Convert.IsDBNull(row["power_supply"]) ? null : Convert.ToString(row["power_supply"]);
                        productData.phase = Convert.IsDBNull(row["phase"]) ? 0 : Convert.ToInt32(row["phase"]);
                        productData.hz = Convert.IsDBNull(row["hz"]) ? 0 : Convert.ToInt32(row["hz"]);
                        productData.unit_warranty = Convert.IsDBNull(row["unit_warranty"]) ? null : Convert.ToString(row["unit_warranty"]);
                        productData.air_end_warranty = Convert.IsDBNull(row["air_end_warranty"]) ? null : Convert.ToString(row["air_end_warranty"]);
                    }


                    List<SqlParameter> arrParm3 = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 }
                        };
                    conn.Open();
                    var dsSupplier = SqlHelper.ExecuteDataset(conn, "sp_supplier_list", arrParm3.ToArray());
                    conn.Close();

                    if (dsSupplier.Tables.Count > 0)
                    {
                        var data = (from t in dsSupplier.Tables[0].AsEnumerable() select t).ToList();
                        if (data != null)
                        {
                            foreach (var row in data)
                            {
                                supplierData.Add(new SupplierDataList()
                                {
                                    data_value = Convert.ToInt32(row["id"]),
                                    data_text = string.Format("{0}{1}", Convert.IsDBNull(row["supplier_code"]) ? string.Empty : Convert.ToString(row["supplier_code"] + " - "),
                                Convert.IsDBNull(row["supplier_name_tha"]) ? string.Empty : Convert.ToString(row["supplier_name_tha"]))

                                });
                            }
                        }
                    }

                    var productSupplier = new List<ProductSupplier>();
                    List<SqlParameter> arrParm2 = new List<SqlParameter>
                        {
                            new SqlParameter("@item_id", SqlDbType.Int) { Value = id },
                            new SqlParameter("@item_type", SqlDbType.VarChar,1) { Value = "P" }
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
                                productSupplier.Add(new ProductSupplier()
                                {
                                    id = Convert.ToInt32(row["id"]),
                                    is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                    product_id = -1,//Convert.ToInt32(row["product_id"]),
                                    product_name = Convert.IsDBNull("product_name") ? string.Empty : Convert.ToString(row["product_name"]),
                                    product_no = Convert.IsDBNull("product_no") ? string.Empty : Convert.ToString(row["product_no"]),
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

                    var mfgProductList = new List<ProductMFG>();
                    //List<SqlParameter> arrParm4 = new List<SqlParameter>
                    //    {
                    //        new SqlParameter("@product_id", SqlDbType.Int) { Value = id },
                    //    };
                    //conn.Open();
                    //var dsResult4 = SqlHelper.ExecuteDataset(conn, "sp_product_mfg_list", arrParm4.ToArray());
                    //conn.Close();
                    //if (dsResult4.Tables.Count > 0)
                    //{
                    //    var data = (from t in dsResult4.Tables[0].AsEnumerable() select t).ToList();
                    //    if (data != null)
                    //    {
                    //        foreach (var row in data)
                    //        {
                    //            mfgProductList.Add(new ProductMFG()
                    //            {
                    //                id = Convert.ToInt32(row["id"]),
                    //                product_id = -1,//Convert.ToInt32(row["product_id"]),
                    //                mfg_no = Convert.IsDBNull("mfg_no") ? string.Empty : Convert.ToString(row["mfg_no"]),
                    //                purchase_request_no = Convert.IsDBNull("purchase_request_no") ? string.Empty : Convert.ToString(row["purchase_request_no"]),
                    //                display_receive_date = Convert.IsDBNull("display_receive_date") ? string.Empty : Convert.ToString(row["display_receive_date"]),
                    //                issue_no = Convert.IsDBNull("issue_no") ? string.Empty : Convert.ToString(row["issue_no"]),
                    //                //receive_date = Convert.IsDBNull("receive_date") ? null : Convert.ToDateTime(row["receive_date"]),


                    //            });

                    //        }
                    //    }
                    //}

                    HttpContext.Current.Session["SESSION_PRODUCT_MFG_PRODUCT"] = mfgProductList;
                    HttpContext.Current.Session["SESSION_SUPPLIER_PRODUCT"] = supplierData;
                    HttpContext.Current.Session["SESSION_PRODUCT_SUPPLIER_PRODUCT"] = productSupplier;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return productData;
        }

        [WebMethod]
        public static string ValidateData()
        {
            var returnMessage = string.Empty;
            var supplierSparePart = (List<ProductSupplier>)HttpContext.Current.Session["SESSION_PRODUCT_SUPPLIER_PRODUCT"];
            //var sparePartShelfList = (List<SparePartShelf>)HttpContext.Current.Session["SESSION_PART_SHELF_SPAREPART"];

            if (supplierSparePart.Count == 0)
            {
                returnMessage += "- กรุณาเลือก Supplier อย่างน้อย 1 รายการ <br>";
            }

            //if (sparePartShelfList.Count == 0)
            //{
            //    returnMessage += "- กรุณาเลือก Shelf อย่างน้อย 1 รายการ <br>";
            //}
            return returnMessage;
        }

        [WebMethod]
        public static string SaveProductData(ProductList[] productData)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                var supplierProduct = (List<ProductSupplier>)HttpContext.Current.Session["SESSION_PRODUCT_SUPPLIER_PRODUCT"];
                var mfgProduct = (List<ProductMFG>)HttpContext.Current.Session["SESSION_PRODUCT_MFG_PRODUCT"];
                var row = (from t in productData select t).FirstOrDefault();
                conn.Open();
                int newId = DataListUtil.emptyEntryID;
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        if (row != null)
                        {

                            if (row.id == 0)
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_product_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@product_no", SqlDbType.VarChar, 50).Value = row.product_no;
                                    cmd.Parameters.Add("@serial_no", SqlDbType.VarChar, 50).Value = row.serial_no;
                                    cmd.Parameters.Add("@cat_id", SqlDbType.Int).Value = row.cat_id;
                                    cmd.Parameters.Add("@brand_id", SqlDbType.Int).Value = row.brand_id;
                                    cmd.Parameters.Add("@product_model", SqlDbType.VarChar, 150).Value = row.product_model;
                                    cmd.Parameters.Add("@product_name_tha", SqlDbType.VarChar, 150).Value = row.product_name_tha;
                                    cmd.Parameters.Add("@product_name_eng", SqlDbType.VarChar, 150).Value = row.product_name_eng;
                                    cmd.Parameters.Add("@description_tha", SqlDbType.VarChar, 300).Value = row.description_tha;
                                    cmd.Parameters.Add("@description_eng", SqlDbType.VarChar, 300).Value = row.description_eng;
                                    cmd.Parameters.Add("@quotation_desc_line1", SqlDbType.VarChar, 300).Value = row.quotation_desc_line1;
                                    cmd.Parameters.Add("@quotation_desc_line2", SqlDbType.VarChar, 300).Value = row.quotation_desc_line2;
                                    cmd.Parameters.Add("@quotation_desc_line3", SqlDbType.VarChar, 300).Value = row.quotation_desc_line3;
                                    cmd.Parameters.Add("@quotation_desc_line4", SqlDbType.VarChar, 300).Value = row.quotation_desc_line4;
                                    cmd.Parameters.Add("@quotation_desc_line5", SqlDbType.VarChar, 300).Value = row.quotation_desc_line5;
                                    cmd.Parameters.Add("@quotation_desc_line6", SqlDbType.VarChar, 300).Value = row.quotation_desc_line6;
                                    cmd.Parameters.Add("@quotation_desc_line7", SqlDbType.VarChar, 300).Value = row.quotation_desc_line7;
                                    cmd.Parameters.Add("@quotation_desc_line8", SqlDbType.VarChar, 300).Value = row.quotation_desc_line8;
                                    cmd.Parameters.Add("@quotation_desc_line9", SqlDbType.VarChar, 300).Value = row.quotation_desc_line9;
                                    cmd.Parameters.Add("@quotation_desc_line10", SqlDbType.VarChar, 300).Value = row.quotation_desc_line10;
                                    cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = row.quantity;
                                    cmd.Parameters.Add("@quantity_reserve", SqlDbType.Int).Value = row.quantity_reserve;
                                    cmd.Parameters.Add("@quantity_balance", SqlDbType.Int).Value = row.quantity_balance;
                                    cmd.Parameters.Add("@unit_id", SqlDbType.Int).Value = row.unit_id;
                                    cmd.Parameters.Add("@selling_price", SqlDbType.Decimal).Value = row.selling_price;
                                    cmd.Parameters.Add("@min_selling_price", SqlDbType.Decimal).Value = row.min_selling_price;
                                    cmd.Parameters.Add("@pressure", SqlDbType.VarChar, 10).Value = row.pressure;
                                    cmd.Parameters.Add("@power_supply", SqlDbType.VarChar, 10).Value = row.power_supply;
                                    cmd.Parameters.Add("@phase", SqlDbType.Int).Value = row.phase;
                                    cmd.Parameters.Add("@hz", SqlDbType.Int).Value = row.hz;
                                    cmd.Parameters.Add("@unit_warranty", SqlDbType.VarChar, 10).Value = row.unit_warranty;
                                    cmd.Parameters.Add("@air_end_warranty", SqlDbType.VarChar, 15).Value = row.air_end_warranty;
                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 300).Value = row.remark == null ? string.Empty : row.remark;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                    newId = Convert.ToInt32(cmd.ExecuteScalar());

                                }
                                foreach (var rowSupplier in supplierProduct)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_item_mapping_supplier_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = newId;
                                        cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = rowSupplier.supplier_id;
                                        cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 2).Value = "P";

                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                                //throw new Exception("ทดสอบ eer");
                                foreach (var rowMFG in mfgProduct)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_product_mfg_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = newId;
                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 50).Value = rowMFG.mfg_no;
                                        cmd.Parameters.Add("@receive_date", SqlDbType.DateTime).Value = rowMFG.receive_date;

                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                        cmd.ExecuteNonQuery();

                                    }
                                }
                            }
                            else
                            {
                                newId = row.id;
                                using (SqlCommand cmd = new SqlCommand("sp_product_edit", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                    cmd.Parameters.Add("@product_no", SqlDbType.VarChar, 50).Value = row.product_no;
                                    cmd.Parameters.Add("@serial_no", SqlDbType.VarChar, 50).Value = row.serial_no;
                                    cmd.Parameters.Add("@cat_id", SqlDbType.Int).Value = row.cat_id;
                                    cmd.Parameters.Add("@brand_id", SqlDbType.Int).Value = row.brand_id;
                                    cmd.Parameters.Add("@product_model", SqlDbType.VarChar, 150).Value = row.product_model;
                                    cmd.Parameters.Add("@product_name_tha", SqlDbType.VarChar, 150).Value = row.product_name_tha;
                                    cmd.Parameters.Add("@product_name_eng", SqlDbType.VarChar, 150).Value = row.product_name_eng;
                                    cmd.Parameters.Add("@description_tha", SqlDbType.VarChar, 300).Value = "";// row.description_tha;
                                    cmd.Parameters.Add("@description_eng", SqlDbType.VarChar, 300).Value = "";// row.description_eng;
                                    cmd.Parameters.Add("@quotation_desc_line1", SqlDbType.VarChar, 300).Value = row.quotation_desc_line1;
                                    cmd.Parameters.Add("@quotation_desc_line2", SqlDbType.VarChar, 300).Value = row.quotation_desc_line2;
                                    cmd.Parameters.Add("@quotation_desc_line3", SqlDbType.VarChar, 300).Value = row.quotation_desc_line3;
                                    cmd.Parameters.Add("@quotation_desc_line4", SqlDbType.VarChar, 300).Value = row.quotation_desc_line4;
                                    cmd.Parameters.Add("@quotation_desc_line5", SqlDbType.VarChar, 300).Value = row.quotation_desc_line5;
                                    cmd.Parameters.Add("@quotation_desc_line6", SqlDbType.VarChar, 300).Value = row.quotation_desc_line6;
                                    cmd.Parameters.Add("@quotation_desc_line7", SqlDbType.VarChar, 300).Value = row.quotation_desc_line7;
                                    cmd.Parameters.Add("@quotation_desc_line8", SqlDbType.VarChar, 300).Value = row.quotation_desc_line8;
                                    cmd.Parameters.Add("@quotation_desc_line9", SqlDbType.VarChar, 300).Value = row.quotation_desc_line9;
                                    cmd.Parameters.Add("@quotation_desc_line10", SqlDbType.VarChar, 300).Value = row.quotation_desc_line10;
                                    cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = row.quantity;
                                    cmd.Parameters.Add("@quantity_reserve", SqlDbType.Int).Value = row.quantity_reserve;
                                    cmd.Parameters.Add("@quantity_balance", SqlDbType.Int).Value = row.quantity_balance;
                                    cmd.Parameters.Add("@unit_id", SqlDbType.Int).Value = row.unit_id;
                                    cmd.Parameters.Add("@selling_price", SqlDbType.Decimal).Value = row.selling_price;
                                    cmd.Parameters.Add("@min_selling_price", SqlDbType.Decimal).Value = row.min_selling_price;
                                    cmd.Parameters.Add("@pressure", SqlDbType.VarChar, 10).Value = row.pressure;
                                    cmd.Parameters.Add("@power_supply", SqlDbType.VarChar, 10).Value = row.power_supply;
                                    cmd.Parameters.Add("@phase", SqlDbType.Int).Value = row.phase;
                                    cmd.Parameters.Add("@hz", SqlDbType.Int).Value = row.hz;
                                    cmd.Parameters.Add("@unit_warranty", SqlDbType.VarChar, 10).Value = row.unit_warranty;
                                    cmd.Parameters.Add("@air_end_warranty", SqlDbType.VarChar, 15).Value = row.air_end_warranty;
                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 300).Value = row.remark == null ? string.Empty : row.remark;
                                    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                    cmd.Parameters.Add("@is_enable", SqlDbType.Bit).Value = row.is_enable;
                                    cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = row.is_delete;


                                    cmd.ExecuteNonQuery();

                                }
                                //throw new Exception("ทดสอบ eer");
                                foreach (var rowSupplier in supplierProduct)
                                {
                                    if (rowSupplier.id < 0)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_item_mapping_supplier_add", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@item_id", SqlDbType.Int).Value = newId;
                                            cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = rowSupplier.supplier_id;
                                            cmd.Parameters.Add("@item_type", SqlDbType.VarChar, 2).Value = "P";

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

                                foreach (var rowMFG in mfgProduct)
                                {
                                    if (rowMFG.id < 0 && rowMFG.is_delete == false)
                                    {

                                        using (SqlCommand cmd = new SqlCommand("sp_product_mfg_add", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = newId;
                                            cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 50).Value = rowMFG.mfg_no;
                                            cmd.Parameters.Add("@receive_date", SqlDbType.DateTime).Value = rowMFG.receive_date;

                                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                            cmd.ExecuteNonQuery();

                                        }

                                    }
                                    else if (rowMFG.id > 0 && rowMFG.is_delete == true)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_product_mfg_delete", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowMFG.id;
                                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                    else
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_product_mfg_edit", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowMFG.id;
                                            cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = newId;
                                            cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 50).Value = rowMFG.mfg_no;
                                            cmd.Parameters.Add("@receive_date", SqlDbType.DateTime).Value = rowMFG.receive_date;
                                            cmd.Parameters.Add("@issue_no", SqlDbType.VarChar, 20).Value = rowMFG.issue_no;
                                            cmd.Parameters.Add("@purchase_request_no", SqlDbType.VarChar, 20).Value = rowMFG.purchase_request_no;

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
        public static string DeleteProduct(int id)
        {
            string returnData = "error";
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    if (id > 0)
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_product_delete", conn))
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

        protected void gridMFG_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridMFG.DataSource = (from t in productMFGList where t.is_delete == false select t).ToList();
            gridMFG.DataBind();
        }
        protected void BindGridMFG()
        {
            gridMFG.DataSource = (from t in productMFGList where t.is_delete == false select t).ToList();
            gridMFG.DataBind();
        }
        [WebMethod]
        public static string AddMFG(string mfg_no, string receive_date)
        {
            var returnData = string.Empty;

            var mfgProductList = (List<ProductMFG>)HttpContext.Current.Session["SESSION_PRODUCT_MFG_PRODUCT"];
            DateTime? recieve_dt = null;
            if (!string.IsNullOrEmpty(receive_date))
            {
                recieve_dt = DateTime.ParseExact(receive_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            if (mfgProductList != null)
            {
                var checkExist = (from t in mfgProductList
                                  where t.mfg_no == mfg_no
                                  select t).FirstOrDefault();
                if (checkExist == null)
                {
                    mfgProductList.Add(new ProductMFG()
                    {
                        id = (mfgProductList.Count + 1) * -1,
                        mfg_no = mfg_no,
                        display_receive_date = receive_date,
                        receive_date = recieve_dt
                    });

                }
                else
                {
                    if (checkExist.is_delete)
                    {
                        var checkExist2 = (from t in mfgProductList
                                           where t.mfg_no == mfg_no
                                           select t).LastOrDefault();
                        if (checkExist2.is_delete)
                        {
                            mfgProductList.Add(new ProductMFG()
                            {
                                id = (mfgProductList.Count + 1) * -1,
                                mfg_no = mfg_no,
                                display_receive_date = receive_date,
                                receive_date = recieve_dt
                            });
                        }
                        else
                        {
                            returnData = "-1";
                        }
                    }
                    else
                    {
                        returnData = "-1";
                    }
                }
            }
            else
            {
                mfgProductList = new List<ProductMFG>();
                mfgProductList.Add(new ProductMFG()
                {
                    id = (mfgProductList.Count + 1) * -1,
                    mfg_no = mfg_no,
                    display_receive_date = receive_date,
                    receive_date = recieve_dt
                });

            }
            HttpContext.Current.Session["SESSION_PRODUCT_MFG_PRODUCT"] = mfgProductList;


            return returnData;
        }
        [WebMethod]
        public static ProductMFG EditMFG(int id)
        {
            var mfgProductList = (List<ProductMFG>)HttpContext.Current.Session["SESSION_PRODUCT_MFG_PRODUCT"];
            if (mfgProductList != null)
            {
                var row = (from t in mfgProductList where t.id == id select t).FirstOrDefault();
                if (row != null)
                {
                    return row;
                }
            }
            return new ProductMFG(); // ไม่มี Data
        }
        [WebMethod]
        public static string SubmitEditMFG(int id, string mfg_no, string receive_date)
        {
            try
            {
                var mfgProductList = (List<ProductMFG>)HttpContext.Current.Session["SESSION_PRODUCT_MFG_PRODUCT"];
                if (mfgProductList != null)
                {
                    ProductMFG valid = new ProductMFG();
                    valid = mfgProductList.Where(a => a.id != id && a.mfg_no == mfg_no).FirstOrDefault();
                    if (valid != null)
                       throw new Exception( "MFG No : " + mfg_no + " ซ้ำในระบบ");

                    DateTime? recieve_dt = null;
                    if (!string.IsNullOrEmpty(receive_date))
                    {
                        recieve_dt = DateTime.ParseExact(receive_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    var row = (from t in mfgProductList where t.id == id select t).FirstOrDefault();
                    if (row != null)
                    {
                        row.mfg_no = mfg_no;
                        row.display_receive_date = receive_date;
                        row.receive_date = recieve_dt;
                    }
                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        [WebMethod]
        public static string DeleteMFG(string id)
        {
            var mfgProductList = (List<ProductMFG>)HttpContext.Current.Session["SESSION_PRODUCT_MFG_PRODUCT"];

            if (mfgProductList != null)
            {

                var row = (from t in mfgProductList where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (row != null)
                {

                    row.is_delete = true;
                }
            }
            return "SUCCESS";

        }
        protected void gridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var dsResult = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm = new List<SqlParameter>
                    {
                        new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = e.Parameters.ToString() },
                        new SqlParameter("@product_no", SqlDbType.VarChar,200) { Value = "" },
                        new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                    };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_list_master", arrParm.ToArray());
                    Session["SESSION_PRODUCT_MASTER"] = dsResult;
                    conn.Close();

                    Session["SEARCH"] = e.Parameters.ToString();
                }

                //Bind data into GridView
                gridView.DataSource = dsResult;
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

        [WebMethod]
        public static string SaveProductDataDummy(string product_id, int qty)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        var transNo = string.Empty;
                        using (SqlCommand cmd = new SqlCommand("sp_gen_transection_no", conn, tran))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            transNo = Convert.ToString(cmd.ExecuteScalar());
                        }
                        using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add_dummy", conn, tran))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                            cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = "P";
                            cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = product_id;

                            cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "PR";
                            cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "IN";
                            cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = qty;
                            cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                            cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Dummy master";
                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                            cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                            cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = "";
                            cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = 0;
                            cmd.Parameters.Add("@purchase_received_date", SqlDbType.DateTime).Value = new DateTime(2019, 1, 1);
                            cmd.ExecuteNonQuery();
                        }
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
    }
}