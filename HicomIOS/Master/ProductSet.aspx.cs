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
    public partial class ProductSet : MasterDetailPage
    {
        public class ProductSetList
        {
            public int id { get; set; }
            public int product_id { get; set; }
            public int product_set_id { get; set; }
            public string product_name { get; set; }
            public string product_no { get; set; }
            public int qty { get; set; }
            public int product_id_to { get; set; }
            public int qty_to { get; set; }
            public string productType { get; set; }
            public string unit_code { get; set; }
            public bool is_deleted { get; set; }
            public string product_type { get; set; }
            public int quantityProduct { get; set; }

        }
        public class ProductList
        {
            public int id { get; set; }
            public int quantity { get; set; }
            public string product_name { get; set; }
            public string product_no { get; set; }
            public string unit_code { get; set; }
            public bool is_selected { get; set; }
        }

        public class ProductName
        {
            public string name_tha { get; set; }
            public string name_eng { get; set; }
            public int quantity_reserve { get; set; }
            public int quantity_balance { get; set; }
            public int quantity { get; set; }
        }

        public class ProductData
        {
            public int id { get; set; }
            public string product_no { get; set; }
            public int quantityStock { get; set; }
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
            public string stock_count_type { get; set; }
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
        }
        List<ProductSetList> selectedProductListList
        {
            get
            {
                if (Session["SESSION_SELECTED_PRODUCT_LIST_PRODUCTSET"] == null)
                    Session["SESSION_SELECTED_PRODUCT_LIST_PRODUCTSET"] = new List<ProductSetList>();
                return (List<ProductSetList>)Session["SESSION_SELECTED_PRODUCT_LIST_PRODUCTSET"];
            }
            set
            {
                Session["SESSION_SELECTED_PRODUCT_LIST_PRODUCTSET"] = value;
            }
        }
        List<ProductSetList> productMappingList
        {
            get
            {
                if (Session["SESSION_PRODUCT_MAPPING_PRODUCTSET"] == null)
                    Session["SESSION_PRODUCT_MAPPING_PRODUCTSET"] = new List<ProductSetList>();
                return (List<ProductSetList>)Session["SESSION_PRODUCT_MAPPING_PRODUCTSET"];
            }
            set
            {
                Session["SESSION_PRODUCT_MAPPING_PRODUCTSET"] = value;
            }
        }

        List<ProductList> productList
        {
            get
            {
                if (Session["SESSION_PRODUCT_LIST_PRODUCTSET"] == null)
                    Session["SESSION_PRODUCT_LIST_PRODUCTSET"] = new List<ProductList>();
                return (List<ProductList>)Session["SESSION_PRODUCT_LIST_PRODUCTSET"];
            }
            set
            {
                Session["SESSION_PRODUCT_LIST_PRODUCTSET"] = value;
            }
        }
        List<ProductList> sparePartList
        {
            get
            {
                if (Session["SESSION_SPARE_PART_LIST_PRODUCTSET"] == null)
                    Session["SESSION_SPARE_PART_LIST_PRODUCTSET"] = new List<ProductList>();
                return (List<ProductList>)Session["SESSION_SPARE_PART_LIST_PRODUCTSET"];
            }
            set
            {
                Session["SESSION_SPARE_PART_LIST_PRODUCTSET"] = value;
            }
        }

        private DataSet dsResult = new DataSet();
        public override string PageName { get { return "ProductSet"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }

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
                BindGrid(true);
            }
            else
            {
                dsResult = (DataSet)ViewState["dsResult"];
                BindGrid(false);
                BindGridViewProduct();
                BindGridViewSelectProduct();
                BindGridViewSelectSparePart();
            }
        }

        protected void PrepareData()
        {
            SPlanetUtil.BindASPxComboBox(ref cbbProductNoID, DataListUtil.DropdownStoreProcedureName.Product_No);
            try
            {
                //Initial data into Dropdown List
                //SPlanetUtil.BindASPxComboBox(ref cboBrand, DataListUtil.DropdownStoreProcedureName.Product_Brand);
                //SPlanetUtil.BindASPxComboBox(ref cboUnitType, DataListUtil.DropdownStoreProcedureName.Product_Unit);
                //SPlanetUtil.BindASPxComboBox(ref cboCateID, DataListUtil.DropdownStoreProcedureName.Product_Category);

            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }

        private void ClearWorkingSession()
        {
            Session.Remove("SESSION_PRODUCT_LIST_PRODUCTSET");
            Session.Remove("SESSION_PRODUCT_MAPPING_PRODUCTSET");
            Session.Remove("SESSION_SELECTED_PRODUCT_LIST_PRODUCTSET");
        }
        #region "Inherited Event of Filter Control"
        public override void OnFilterChanged()
        {
            BindGrid();
        }
        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            //result.Add(new FilterControlTextColumn() { PropertyName = "company_name_tha", DisplayName = "Company Name" });
            //result.Add(new FilterControlTextColumn() { PropertyName = "customer_code", DisplayName = "Customer Code" });
            //result.Add(new FilterControlTextColumn() { PropertyName = "address_tha", DisplayName = "Address" });
            return result.OfType<FilterControlColumn>();
        }
        #endregion

        protected void BindGrid(bool isForceRefreshData = false)
        {
            try
            {
                if (!Page.IsPostBack || isForceRefreshData)
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            //new SqlParameter("@product_no", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                        };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_set_list", arrParm.ToArray());
                        ViewState["dsResult"] = dsResult;
                    }
                }

                var dataGrid = (from t in dsResult.Tables[0].AsEnumerable()
                                select new
                                {
                                    product_set_id = Convert.IsDBNull(t["product_set_id"]) ? 0 : Convert.ToInt32(t["product_set_id"]),
                                    product_name_tha = Convert.IsDBNull(t["product_name_tha"]) ? string.Empty : Convert.ToString(t["product_name_tha"]),
                                    product_no = Convert.IsDBNull(t["product_no"]) ? string.Empty : Convert.ToString(t["product_no"]),
                                    count_product_id =  Convert.IsDBNull(t["count_product_id"]) ? 0 : Convert.ToInt32(t["count_product_id"]),
                                    qty = Convert.IsDBNull(t["qty"]) ? 0 : Convert.ToInt32(t["qty"]),
                                    is_enable = t.Field<bool>("is_enable"),
                                }).Distinct().ToList();

                //Bind data into GridView
                gridViewProductSet.DataSource = dataGrid;
                gridViewProductSet.FilterExpression = FilterBag.GetExpression(false);
                gridViewProductSet.DataBind();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        [WebMethod]
        public static string LoadProductList()
        {
            var returnData = string.Empty;
            var dsResult = new DataSet();
            var productList = new List<ProductList>();
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
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    foreach (var row in dsResult.Tables[0].AsEnumerable())
                    {
                        productList.Add(new ProductList()
                        {
                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                            is_selected = false,
                            quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]),

                            product_name = Convert.IsDBNull(row["product_name_tha"]) ? string.Empty : Convert.ToString(row["product_name_tha"]),
                            product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                            unit_code = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]),
                        });
                    }
                }
            }
            List<ProductSetList> productMappingList = (List<ProductSetList>)HttpContext.Current.Session["SESSION_PRODUCT_MAPPING_PRODUCTSET"];

            HttpContext.Current.Session["SESSION_SELECTED_PRODUCT_LIST_PRODUCTSET"] = productMappingList;
            HttpContext.Current.Session["SESSION_PRODUCT_LIST_PRODUCTSET"] = productList;

            return returnData;
        }

        [WebMethod]
        public static string SelectedProduct(string id, bool isSelected, string product_type)
        {
            var returnData = string.Empty;
            List<ProductSetList> selectedProductList = (List<ProductSetList>)HttpContext.Current.Session["SESSION_SELECTED_PRODUCT_LIST_PRODUCTSET"];
            List<ProductList> productData = (List<ProductList>)HttpContext.Current.Session["SESSION_PRODUCT_LIST_PRODUCTSET"];
            List<ProductList> sparePartData = (List<ProductList>)HttpContext.Current.Session["SESSION_SPARE_PART_LIST_PRODUCTSET"];
            //if (data != null)
            //{

            var rowProduct = (from t in productData where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
            var rowSparePart = (from t in sparePartData where t.id == Convert.ToInt32(id) select t).FirstOrDefault();

            if (rowProduct != null || rowSparePart != null)
            {
                if (isSelected)
                {
                    if (product_type == "P")
                    {
                        rowProduct.is_selected = true;
                    }
                    else if (product_type == "S")
                    {
                        rowSparePart.is_selected = true;
                    }

                    if (selectedProductList.Count == 0)
                    {
                        selectedProductList = new List<ProductSetList>();
                    }

                    selectedProductList.Add(new ProductSetList
                    {
                        id = (selectedProductList.Count + 1) * -1,
                        product_id = Convert.ToInt32(id),
                        product_name = product_type == "P" ? rowProduct.product_name : rowSparePart.product_name,
                        product_no = product_type == "P" ? rowProduct.product_no : rowSparePart.product_no,
                        unit_code = product_type == "P" ? rowProduct.unit_code : rowSparePart.unit_code,
                        is_deleted = false,
                        qty = 1,
                        quantityProduct = product_type == "P" ? rowProduct.quantity : rowSparePart.quantity,
                        product_type = product_type
                    });

                }
                else
                {
                    if (product_type == "P")
                    {
                        rowProduct.is_selected = false;
                    }
                    else if (product_type == "S")
                    {
                        rowSparePart.is_selected = false;
                    }
                    var row = (from t in selectedProductList where t.product_id == Convert.ToInt32(id) && t.product_type == product_type select t).FirstOrDefault();
                    if (row != null)
                    {
                        if (row.id > 0)
                        {
                            row.is_deleted = true;
                        }
                        else
                        {
                            selectedProductList.Remove(row);
                        }

                    }

                }
            }

            HttpContext.Current.Session["SESSION_SELECTED_PRODUCT_LIST_PRODUCTSET"] = selectedProductList;
            return returnData;
        }
        [WebMethod]
        public static ProductSetList EditProduct(string id)
        {
            try
            {
                var productSetMapping = (List<ProductSetList>)HttpContext.Current.Session["SESSION_PRODUCT_MAPPING_PRODUCTSET"];
                var productRow = new ProductSetList();
                if (productSetMapping != null)
                {
                    var row = (from t in productSetMapping where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (row != null)
                    {
                        productRow = row;
                    }
                }
                return productRow;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static string SubmitEditProduct(string id, string qty)
        {
            try
            {
                var productSetMapping = (List<ProductSetList>)HttpContext.Current.Session["SESSION_PRODUCT_MAPPING_PRODUCTSET"];
                if (productSetMapping != null)
                {
                    var row = (from t in productSetMapping where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (row != null)
                    {
                        row.qty = Convert.ToInt32(qty);
                    }
                }
                HttpContext.Current.Session["SESSION_PRODUCT_MAPPING_PRODUCTSET"] = productSetMapping;

                return "SUCCESS";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static string SubmitSelectProduct()
        {
            try
            {
                var returnData = string.Empty;
                List<ProductSetList> selectedProductList = (List<ProductSetList>)HttpContext.Current.Session["SESSION_SELECTED_PRODUCT_LIST_PRODUCTSET"];
                List<ProductSetList> productMapingList = new List<ProductSetList>();

                if (selectedProductList != null)
                {
                    productMapingList = selectedProductList;
                }
                HttpContext.Current.Session["SESSION_PRODUCT_MAPPING_PRODUCTSET"] = productMapingList;
                HttpContext.Current.Session.Remove("SESSION_SELECTED_PRODUCT_LIST_PRODUCTSET");
                returnData = (from t in productMapingList where t.is_deleted == false select t).ToList().Count().ToString();
                return returnData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [WebMethod]
        public static string NewProductSetData()
        {

            HttpContext.Current.Session.Remove("SESSION_SELECTED_PRODUCT_LIST_PRODUCTSET");
            HttpContext.Current.Session.Remove("SESSION_PRODUCT_MAPPING_PRODUCTSET");
            return "NewMode";
        }
        [WebMethod]
        public static ProductData GetEditProductSetData(string id)
        {
            var productData = new ProductData();
            var dsDataProduct = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    var productSetList = new List<ProductSetList>();
                    List<SqlParameter> arrParm2 = new List<SqlParameter>
                        {
                            new SqlParameter("@product_set_id", SqlDbType.Int) { Value = id },
                        };
                    conn.Open();
                    var dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_set_detail_list", arrParm2.ToArray());
                    conn.Close();
                    if (dsResult.Tables.Count > 0)
                    {
                        var data = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                        if (data != null)
                        {
                            foreach (var row in data)
                            {
                                productSetList.Add(new ProductSetList()
                                {
                                    id = Convert.ToInt32(row["id"]),
                                    is_deleted = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                    product_id = Convert.ToInt32(row["product_id"]),
                                    product_name = Convert.IsDBNull("product_name") ? string.Empty : Convert.ToString(row["product_name"]),
                                    product_no = Convert.IsDBNull("product_no") ? string.Empty : Convert.ToString(row["product_no"]),
                                    unit_code = Convert.IsDBNull("unit_code") ? string.Empty : Convert.ToString(row["unit_code"]),
                                    qty = Convert.IsDBNull("qty") ? 0 : Convert.ToInt32(row["qty"]),
                                    product_type = Convert.IsDBNull(row["product_type"]) ? string.Empty : Convert.ToString(row["product_type"]),
                                    productType = Convert.IsDBNull(row["producttype"]) ? string.Empty : Convert.ToString(row["producttype"])
                                });
                            }
                        }
                    }

                    HttpContext.Current.Session["SESSION_PRODUCT_MAPPING_PRODUCTSET"] = productSetList;
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@product_no", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },
                        };
                    conn.Open();
                    dsDataProduct = SqlHelper.ExecuteDataset(conn, "sp_product_list", arrParm.ToArray());
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
                        //productData.stock_count_type = Convert.IsDBNull(row["stock_count_type"]) ? null : Convert.ToString(row["stock_count_type"]);
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
                        //productData.stock_count_type = Convert.IsDBNull(row["stock_count_type"]) ? null : Convert.ToString(row["stock_count_type"]);
                        productData.pressure = Convert.IsDBNull(row["pressure"]) ? null : Convert.ToString(row["pressure"]);
                        productData.power_supply = Convert.IsDBNull(row["power_supply"]) ? null : Convert.ToString(row["power_supply"]);
                        productData.phase = Convert.IsDBNull(row["phase"]) ? 0 : Convert.ToInt32(row["phase"]);
                        productData.hz = Convert.IsDBNull(row["hz"]) ? 0 : Convert.ToInt32(row["hz"]);
                        productData.unit_warranty = Convert.IsDBNull(row["unit_warranty"]) ? null : Convert.ToString(row["unit_warranty"]);
                        productData.air_end_warranty = Convert.IsDBNull(row["air_end_warranty"]) ? null : Convert.ToString(row["air_end_warranty"]);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return productData;
        }



        protected void gridViewSelectProduct_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            ASPxCheckBox checkBox = gridViewSelectProduct.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkBox") as ASPxCheckBox;
            if (checkBox != null)
            {
                if (e.DataColumn.FieldName == "is_selected")
                {
                    if (selectedProductListList != null)
                    {
                        var row = (from t in selectedProductListList
                                   where t.product_id == Convert.ToInt32(e.KeyValue) && t.product_type == "P" && !t.is_deleted
                                   select t).FirstOrDefault();
                        if (row != null)
                        {
                            checkBox.Checked = true;
                        }
                        else
                        {
                            checkBox.Checked = false;
                        }
                    }
                    //checkBox.Checked = (row["is_selected"]) == DBNull.Value ? false : true;
                }
            }
        }

        protected void gridViewProduct_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var splitStr = e.Parameters.ToString().Split('|');
            string searchText = string.Empty;
            string issueType = string.Empty;
            if (splitStr.Length > 1)
            {
                searchText = splitStr[2];

            }
            if (string.IsNullOrEmpty(searchText))
            {
                gridViewProduct.DataSource = (from t in productMappingList where t.is_deleted == false select t).ToList();
                gridViewProduct.DataBind();
            }
            else
            {

                gridViewProduct.DataSource = productMappingList.Where(t => (t.product_no.ToUpper().Contains(searchText.ToUpper())
                                                           || t.product_name.ToUpper().Contains(searchText.ToUpper())) && t.is_deleted == false).ToList();
                gridViewProduct.DataBind();


            }

          
        }
        private void BindGridViewProduct()
        {
            gridViewProduct.DataSource = (from t in productMappingList where t.is_deleted == false select t).ToList();
            gridViewProduct.DataBind();
        }

        protected void gridViewSelectProduct_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var splitStr = e.Parameters.ToString().Split('|');
            string searchText = string.Empty;
            string issueType = string.Empty;
            if (splitStr.Length > 1)
            {
                searchText = splitStr[2];

            }
            if (string.IsNullOrEmpty(searchText))
            {
                gridViewSelectProduct.DataSource = productList;
                gridViewSelectProduct.DataBind();
            }
            else
            {

                gridViewSelectProduct.DataSource = productList.Where(t => t.product_no.ToUpper().Contains(searchText.ToUpper())
                                                            || t.product_name.ToUpper().Contains(searchText.ToUpper())).ToList();
                gridViewSelectProduct.DataBind();
                
                
            }

          
        }
        private void BindGridViewSelectProduct()
        {
            gridViewSelectProduct.DataSource = productList;
            gridViewSelectProduct.DataBind();
        }
        [WebMethod]
        public static string ValidateData()
        {
            var returnMessage = string.Empty;
            var productMapping = (List<ProductSetList>)HttpContext.Current.Session["SESSION_PRODUCT_MAPPING_PRODUCTSET"];

            if (productMapping.Count == 0)
            {
                returnMessage += "- กรุณาเลือกสินค้าอะไหล่ อย่างน้อย 1 รายการ <br>";
            }

            return returnMessage;


        }

        [WebMethod]
        public static string SaveProductData(ProductData[] productData)
        {


            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                conn.Open();
                int newId = 0;
                var productMapping = (List<ProductSetList>)HttpContext.Current.Session["SESSION_PRODUCT_MAPPING_PRODUCTSET"];
                var row = (from t in productData select t).FirstOrDefault();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        if (row != null)
                        {
                            //if (row.id == 0)
                            //{
                            //using (SqlCommand cmd = new SqlCommand("sp_product_add", conn, tran))
                            //{
                            //    cmd.CommandType = CommandType.StoredProcedure;

                            //    cmd.Parameters.Add("@product_no", SqlDbType.VarChar, 50).Value = row.product_no;
                            //    cmd.Parameters.Add("@serial_no", SqlDbType.VarChar, 50).Value = Convert.IsDBNull(row.serial_no) ? "" : Convert.ToString(row.serial_no);
                            //    cmd.Parameters.Add("@cat_id", SqlDbType.Int).Value = Convert.IsDBNull(row.cat_id) ? 0 : Convert.ToInt32(row.cat_id);
                            //    cmd.Parameters.Add("@brand_id", SqlDbType.Int).Value = row.brand_id;
                            //    cmd.Parameters.Add("@product_model", SqlDbType.VarChar, 150).Value = row.product_model;
                            //    cmd.Parameters.Add("@product_name_tha", SqlDbType.VarChar, 150).Value = row.product_name_tha;
                            //    cmd.Parameters.Add("@product_name_eng", SqlDbType.VarChar, 150).Value = row.product_name_eng;
                            //    cmd.Parameters.Add("@description_tha", SqlDbType.VarChar, 300).Value = string.IsNullOrEmpty(row.description_tha) ? "" : Convert.ToString(row.description_tha);
                            //    cmd.Parameters.Add("@description_eng", SqlDbType.VarChar, 300).Value = string.IsNullOrEmpty(row.description_eng) ? "" : Convert.ToString(row.description_eng);
                            //    cmd.Parameters.Add("@quotation_desc_line1", SqlDbType.VarChar, 300).Value = string.IsNullOrEmpty(row.quotation_desc_line1) ? "" : Convert.ToString(row.quotation_desc_line1);
                            //    cmd.Parameters.Add("@quotation_desc_line2", SqlDbType.VarChar, 300).Value = string.IsNullOrEmpty(row.quotation_desc_line2) ? "" : Convert.ToString(row.quotation_desc_line2);
                            //    cmd.Parameters.Add("@quotation_desc_line3", SqlDbType.VarChar, 300).Value = string.IsNullOrEmpty(row.quotation_desc_line3) ? "" : Convert.ToString(row.quotation_desc_line3);
                            //    cmd.Parameters.Add("@quotation_desc_line4", SqlDbType.VarChar, 300).Value = string.IsNullOrEmpty(row.quotation_desc_line4) ? "" : Convert.ToString(row.quotation_desc_line4);
                            //    cmd.Parameters.Add("@quotation_desc_line5", SqlDbType.VarChar, 300).Value = string.IsNullOrEmpty(row.quotation_desc_line5) ? "" : Convert.ToString(row.quotation_desc_line5);
                            //    cmd.Parameters.Add("@quotation_desc_line6", SqlDbType.VarChar, 300).Value = string.IsNullOrEmpty(row.quotation_desc_line6) ? "" : Convert.ToString(row.quotation_desc_line6);
                            //    cmd.Parameters.Add("@quotation_desc_line7", SqlDbType.VarChar, 300).Value = string.IsNullOrEmpty(row.quotation_desc_line7) ? "" : Convert.ToString(row.quotation_desc_line7);
                            //    cmd.Parameters.Add("@quotation_desc_line8", SqlDbType.VarChar, 300).Value = string.IsNullOrEmpty(row.quotation_desc_line8) ? "" : Convert.ToString(row.quotation_desc_line8);
                            //    cmd.Parameters.Add("@quotation_desc_line9", SqlDbType.VarChar, 300).Value = string.IsNullOrEmpty(row.quotation_desc_line9) ? "" : Convert.ToString(row.quotation_desc_line9);
                            //    cmd.Parameters.Add("@quotation_desc_line10", SqlDbType.VarChar, 300).Value = string.IsNullOrEmpty(row.quotation_desc_line10) ? "" : Convert.ToString(row.quotation_desc_line10);
                            //    cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = row.quantity;
                            //    cmd.Parameters.Add("@quantity_reserve", SqlDbType.Int).Value = row.quantity_reserve;
                            //    cmd.Parameters.Add("@quantity_balance", SqlDbType.Int).Value = row.quantity_balance;
                            //    cmd.Parameters.Add("@unit_id", SqlDbType.Int).Value = row.unit_id;
                            //    cmd.Parameters.Add("@selling_price", SqlDbType.Decimal).Value = row.selling_price;
                            //    cmd.Parameters.Add("@min_selling_price", SqlDbType.Decimal).Value = row.min_selling_price;
                            //    cmd.Parameters.Add("@pressure", SqlDbType.VarChar, 10).Value = row.pressure;
                            //    cmd.Parameters.Add("@power_supply", SqlDbType.VarChar, 10).Value = row.power_supply;
                            //    cmd.Parameters.Add("@phase", SqlDbType.Int).Value = row.phase;
                            //    cmd.Parameters.Add("@hz", SqlDbType.Int).Value = row.hz;
                            //    cmd.Parameters.Add("@unit_warranty", SqlDbType.VarChar, 10).Value = row.unit_warranty;
                            //    cmd.Parameters.Add("@air_end_warranty", SqlDbType.VarChar, 15).Value = row.air_end_warranty;
                            //    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 300).Value = row.remark == null ? string.Empty : row.remark;
                            //    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                            //    newId = Convert.ToInt32(cmd.ExecuteScalar());
                            //}

                            foreach (var rowProduct in productMapping)
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_product_set_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@product_set_id", SqlDbType.Int).Value = row.id;
                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = rowProduct.product_id;
                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = rowProduct.qty;
                                    cmd.Parameters.Add("@product_id_to", SqlDbType.Int).Value = Convert.ToInt32(row.id);
                                    cmd.Parameters.Add("@qty_to", SqlDbType.Int).Value = (row.quantity);
                                    cmd.Parameters.Add("@product_set_type", SqlDbType.VarChar, 2).Value = "P";
                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 2).Value = rowProduct.product_type;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                    cmd.ExecuteNonQuery();
                                }
                            }
                            //}

                            //using (SqlCommand cmd = new SqlCommand("sp_master_product_set_edit", conn, tran))
                            //{
                            //    newId = row.id;
                            //    cmd.CommandType = CommandType.StoredProcedure;

                            //    cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                            //    cmd.Parameters.Add("@product_no", SqlDbType.VarChar, 50).Value = row.product_no;
                            //    cmd.Parameters.Add("@product_model", SqlDbType.VarChar, 150).Value = row.product_model;
                            //    cmd.Parameters.Add("@product_name_tha", SqlDbType.VarChar, 150).Value = row.product_name_tha;
                            //    cmd.Parameters.Add("@product_name_eng", SqlDbType.VarChar, 150).Value = row.product_name_eng;
                            //    cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = Convert.ToInt32(row.quantity) + Convert.ToInt32(row.quantityStock);
                            //    cmd.Parameters.Add("@quantity_reserve", SqlDbType.Int).Value = row.quantity_reserve;
                            //    cmd.Parameters.Add("@quantity_balance", SqlDbType.Int).Value = row.quantity_balance;
                            //    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 150).Value = "Product";
                            //    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                            //    cmd.ExecuteNonQuery();

                            //}
                            //foreach (var rowProduct in productMapping)
                            //{
                            //    using (SqlCommand cmd = new SqlCommand("sp_product_set_edit", conn, tran))
                            //    {
                            //        cmd.CommandType = CommandType.StoredProcedure;

                            //        cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowProduct.id;
                            //        cmd.Parameters.Add("@product_set_id", SqlDbType.Int).Value = newId;
                            //        cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = rowProduct.product_id;
                            //        cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 2).Value = rowProduct.product_type;
                            //        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = rowProduct.qty;
                            //        cmd.Parameters.Add("@is_enable", SqlDbType.Bit).Value = 1;
                            //        cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = rowProduct.is_deleted;
                            //        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                            //        cmd.ExecuteNonQuery();
                            //    }
                            //}

                            var transectionNo = SqlHelper.ExecuteDataset(tran, "sp_gen_transection_no");
                            var transection_no = string.Empty;
                            if (transectionNo != null)
                            {
                                transection_no = (from t in transectionNo.Tables[0].AsEnumerable()
                                                  select t.Field<string>("Column1")).FirstOrDefault();
                            }
                            //stam transection + tab1
                            using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transection_no;
                                cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = "P";
                                cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.id;
                                cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "SET+";
                                cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 1).Value = "A";
                                cmd.Parameters.Add("@checking_type", SqlDbType.VarChar, 1).Value = "M";
                                cmd.Parameters.Add("@qty", SqlDbType.Int).Value = Convert.ToInt32(row.quantity) + Convert.ToInt32(row.quantityStock);
                                cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = Convert.ToInt32(row.quantity) + Convert.ToInt32(row.quantity_balance);
                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Product";
                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = 0;

                                cmd.ExecuteNonQuery();
                            }
                            //stam transection - tab2
                            foreach (var rowProduct in productMapping)
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transection_no;
                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = rowProduct.product_type;
                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = rowProduct.product_id;
                                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "SET-";
                                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 1).Value = "A";
                                    cmd.Parameters.Add("@checking_type", SqlDbType.VarChar, 1).Value = "M";
                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = Convert.ToInt32(rowProduct.quantityProduct) - Convert.ToInt32(rowProduct.qty);
                                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = Convert.ToInt32(rowProduct.quantityProduct) - Convert.ToInt32(rowProduct.qty);
                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Product";
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = 0;

                                    cmd.ExecuteNonQuery();
                                }
                            }
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
                return "success";
            }
        }
        [WebMethod]
        public static string DeleteProduct(string id)
        {
            var returnData = string.Empty;
            var productMapping = (List<ProductSetList>)HttpContext.Current.Session["SESSION_PRODUCT_MAPPING_PRODUCTSET"];

            if (productMapping != null)
            {
                var checkExist = (from t in productMapping
                                  where t.id == Convert.ToInt32(id)
                                  select t).FirstOrDefault();
                if (checkExist != null)
                {
                    if (checkExist.id < 0)
                    {
                        productMapping.Remove(checkExist);
                    }
                    else
                    {
                        checkExist.is_deleted = true;
                    }

                }
            }

            HttpContext.Current.Session["SESSION_PRODUCT_MAPPING_PRODUCTSET"] = productMapping;


            return returnData;
        }

        [WebMethod]
        public static string DeleteProductSet(int id)
        {
            string returnData = "error";
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    if (id > 0)
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_product_set_delete", conn))
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

        protected void gridViewSelectSparePart_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var splitStr = e.Parameters.ToString().Split('|');
            string searchText = string.Empty;
            string issueType = string.Empty;
            if (splitStr.Length > 1)
            {
                searchText = splitStr[2];

            }
            if (string.IsNullOrEmpty(searchText))
            {
                gridViewSelectSparePart.DataSource = productList;
                gridViewSelectSparePart.DataBind();
            }
            else
            {

                gridViewSelectSparePart.DataSource = sparePartList.Where(t => t.product_no.ToUpper().Contains(searchText.ToUpper())
                                                            || t.product_name.ToUpper().Contains(searchText.ToUpper())).ToList();
                gridViewSelectSparePart.DataBind();


            }
            
        }
        private void BindGridViewSelectSparePart()
        {
            gridViewSelectSparePart.DataSource = sparePartList;
            gridViewSelectSparePart.DataBind();
        }

        protected void gridViewSelectSparePart_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            ASPxCheckBox checkBox = gridViewSelectSparePart.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkBox") as ASPxCheckBox;
            if (checkBox != null)
            {
                if (e.DataColumn.FieldName == "is_selected")
                {
                    if (selectedProductListList != null)
                    {
                        var row = (from t in selectedProductListList
                                   where t.product_id == Convert.ToInt32(e.KeyValue) && t.product_type == "S" && !t.is_deleted
                                   select t).FirstOrDefault();
                        if (row != null)
                        {
                            checkBox.Checked = true;
                        }
                        else
                        {
                            checkBox.Checked = false;
                        }
                    }
                    //checkBox.Checked = (row["is_selected"]) == DBNull.Value ? false : true;
                }
            }
        }
        [WebMethod]
        public static string LoadSparePartList()
        {
            var returnData = string.Empty;
            var dsResult = new DataSet();
            var sparePartList = new List<ProductList>();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" }
                        };
                conn.Open();
                dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_list", arrParm.ToArray());
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    foreach (var row in dsResult.Tables[0].AsEnumerable())
                    {
                        sparePartList.Add(new ProductList()
                        {
                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                            product_no = Convert.IsDBNull(row["part_no"]) ? null : Convert.ToString(row["part_no"]),
                            product_name = Convert.IsDBNull(row["part_name_tha"]) ? null : Convert.ToString(row["part_name_tha"]),
                            unit_code = Convert.IsDBNull(row["unit_code"]) ? null : Convert.ToString(row["unit_code"]),
                            is_selected = false
                        });
                    }
                }
            }
            List<ProductSetList> productMappingList = (List<ProductSetList>)HttpContext.Current.Session["SESSION_PRODUCT_MAPPING_PRODUCTSET"];

            HttpContext.Current.Session["SESSION_SELECTED_PRODUCT_LIST_PRODUCTSET"] = productMappingList;
            HttpContext.Current.Session["SESSION_SPARE_PART_LIST_PRODUCTSET"] = sparePartList;

            return returnData;
        }
        [WebMethod]
        public static ProductSetList GetSaleProductSetData(string set_id)
        {
            var data = new ProductSetList();
            var dsSaleProductSet = new DataSet();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@product_set_id", SqlDbType.Int) { Value = Convert.ToInt32(set_id) },
                        };
                conn.Open();
                dsSaleProductSet = SqlHelper.ExecuteDataset(conn, "sp_report_product_set", arrParm.ToArray());
                conn.Close();

                if (dsSaleProductSet.Tables.Count > 0)
                {
                    if (dsSaleProductSet.Tables[0].Rows.Count > 0)
                    {
                        var row = (from t in dsSaleProductSet.Tables[0].AsEnumerable() select t).FirstOrDefault();
                        if (row != null)
                        {

                            data.product_set_id = Convert.IsDBNull(row["product_set_id"]) ? 0 : Convert.ToInt32(row["product_set_id"]);


                        }
                    }
                }
            }

            return data;
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

        protected void gridViewProductSet_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var dsResult = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = e.Parameters.ToString() },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                        };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_set_list", arrParm.ToArray());
                    ViewState["dsResult"] = dsResult;
                }

                var dataGrid = (from t in dsResult.Tables[0].AsEnumerable()
                                select new
                                {
                                    product_set_id = t.Field<int>("product_set_id"),
                                    product_name_tha = t.Field<string>("product_name_tha"),
                                    product_no = t.Field<string>("product_no"),
                                    count_product_id = t.Field<int>("count_product_id"),
                                    is_enable = t.Field<bool>("is_enable"),
                                    qty = t.Field<int>("qty"),
                                }).Distinct().ToList();

                //Bind data into GridView
                gridViewProductSet.DataSource = dataGrid;
                gridViewProductSet.FilterExpression = FilterBag.GetExpression(false);
                gridViewProductSet.DataBind();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }

        [WebMethod]
        public static string EditEnable(string id, string is_enable)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_status_edit_product_set", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(id);
                        cmd.Parameters.Add("@is_enable", SqlDbType.Bit).Value = Convert.ToInt32(is_enable);
                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "success";
        }
    }
}