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
    public partial class SparePartSet : MasterDetailPage
    {
        public class ProductList
        {
            public int id { get; set; }
            public string product_name { get; set; }
            public string product_no { get; set; }
            public string unit_code { get; set; }
            public bool is_selected { get; set; }
            public int quantity { get; set; }

        }
        public class PastName
        {
            public string name_tha { get; set; }
            public string name_eng { get; set; }
            public int quantity_reserve { get; set; }
            public int quantity_balance { get; set; }
            public int quantity { get; set; }


        }

        public class ProductSetList
        {
            public int id { get; set; }
            public int product_id { get; set; }
            public string part_name { get; set; }
            public string part_no { get; set; }
            public int product_set_id { get; set; }
            public int qty { get; set; }
            public string unit_code { get; set; }
            public bool is_deleted { get; set; }
            public string product_type { get; set; }
            public int quantitySparePart { get; set; }

        }
        public class ServicePartListList
        {
            public int id { get; set; }
            public string part_no { get; set; }
            public string secondary_code { get; set; }
            public int shelf_id { get; set; }
            public string part_name_tha { get; set; }
            public string part_name_eng { get; set; }
            public int cat_id { get; set; }
            public int brand_id { get; set; }
            public int quantity { get; set; }
            public int quantityStock { get; set; }

            public int quantity_reserve { get; set; }
            public int quantity_balance { get; set; }
            public int unit_id { get; set; }
            public string unit_code { get; set; }
            public decimal selling_price { get; set; }
            public decimal min_selling_price { get; set; }
            public string remark { get; set; }
            public bool is_enable { get; set; }
            public bool is_delete { get; set; }
            public int created_by { get; set; }
        }
        List<ProductList> sparePartList
        {
            get
            {
                if (Session["SESSION_SPARE_PART_LIST_SPARE_PART_SET"] == null)
                    Session["SESSION_SPARE_PART_LIST_SPARE_PART_SET"] = new List<ProductList>();
                return (List<ProductList>)Session["SESSION_SPARE_PART_LIST_SPARE_PART_SET"];
            }
            set
            {
                Session["SESSION_SPARE_PART_LIST_SPARE_PART_SET"] = value;
            }
        }
        List<ProductSetList> selectedSparePartList
        {
            get
            {
                if (Session["SESSION_SELECTED_PART_LIST_SPARE_PART_SET"] == null)
                    Session["SESSION_SELECTED_PART_LIST_SPARE_PART_SET"] = new List<ProductSetList>();
                return (List<ProductSetList>)Session["SESSION_SELECTED_PART_LIST_SPARE_PART_SET"];
            }
            set
            {
                Session["SESSION_SELECTED_PART_LIST_SPARE_PART_SET"] = value;
            }
        }
        List<ProductSetList> sparePartMappingList
        {
            get
            {
                if (Session["SESSION_PART_MAPPING_SPARE_PART_SET"] == null)
                    Session["SESSION_PART_MAPPING_SPARE_PART_SET"] = new List<ProductSetList>();
                return (List<ProductSetList>)Session["SESSION_PART_MAPPING_SPARE_PART_SET"];
            }
            set
            {
                Session["SESSION_PART_MAPPING_SPARE_PART_SET"] = value;
            }
        }
        private DataSet dsResult = new DataSet();
        public override string PageName { get { return "Part Set"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
                BindGridViewSparePart();
                BindGridViewSelectSparePart();

            }
        }
        protected void ClearWorkingSession()
        {

            Session.Remove("SESSION_SELECTED_PART_LIST_SPARE_PART_SET");
            Session.Remove("SESSION_PART_MAPPING_SPARE_PART_SET");
        }
        protected void PrepareData()
        {
            // Load Combobox Product Category Data
            //SPlanetUtil.BindASPxComboBox(ref cboCateID, DataListUtil.DropdownStoreProcedureName.Product_Category);

            //// Load Combobox Product Unit Type Data
            //SPlanetUtil.BindASPxComboBox(ref cboUnitType, DataListUtil.DropdownStoreProcedureName.Product_Unit);

            // Load Combobox Product Brand Data
            //SPlanetUtil.BindASPxComboBox(ref cboBrand, DataListUtil.DropdownStoreProcedureName.Product_Brand);
            SPlanetUtil.BindASPxComboBox(ref cbbPartNoID, DataListUtil.DropdownStoreProcedureName.Part_No);
        }
        [WebMethod]
        public static string NewProductSetData()
        {

            HttpContext.Current.Session.Remove("SESSION_SELECTED_PART_LIST_SPARE_PART_SET");
            HttpContext.Current.Session.Remove("SESSION_PART_MAPPING_SPARE_PART_SET");
            return "NewMode";
        }

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
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" }
                        };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_spare_part_set_list", arrParm.ToArray());
                        ViewState["dsResult"] = dsResult;
                    }
                }

                var dataGrid = (from t in dsResult.Tables[0].AsEnumerable()
                                select new
                                {
                                    spare_part_set_id = Convert.IsDBNull(t["product_set_id"]) ? 0 : Convert.ToInt32(t["product_set_id"]),
                                    spare_part_set_name = Convert.IsDBNull(t["part_name_tha"]) ? string.Empty : Convert.ToString(t["part_name_tha"]),
                                    spare_part_set_no = Convert.IsDBNull(t["part_no"]) ? string.Empty : Convert.ToString(t["part_no"]),
                                    count_product_id =  Convert.IsDBNull(t["count_product_id"]) ? 0 : Convert.ToInt32(t["count_product_id"]),
                                    qty = Convert.IsDBNull(t["qty"]) ? 0 : Convert.ToInt32(t["qty"]),
                                    is_enable = t.Field<bool>("is_enable"),
                                   
                                }).Distinct().ToList();

                //Bind data into GridView
                gridViewSparePartSet.DataSource = dataGrid;
                gridViewSparePartSet.FilterExpression = FilterBag.GetExpression(false);
                gridViewSparePartSet.DataBind();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
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
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = "" }
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
                            quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]),
                            is_selected = false
                        });
                    }
                }
            }
            List<ProductSetList> productMappingList = (List<ProductSetList>)HttpContext.Current.Session["SESSION_PART_MAPPING_SPARE_PART_SET"];

            HttpContext.Current.Session["SESSION_SELECTED_PART_LIST_SPARE_PART_SET"] = productMappingList;
            HttpContext.Current.Session["SESSION_SPARE_PART_LIST_SPARE_PART_SET"] = sparePartList;

            return returnData;
        }

        [WebMethod]
        public static ServicePartListList GetEditSparePartData(string id)
        {

            var sparePartData = new ServicePartListList();
            var dsDataSparePart = new DataSet();
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
                        sparePartData.brand_id = Convert.IsDBNull(row["brand_id"]) ? 0 : Convert.ToInt32(row["brand_id"]);
                        sparePartData.unit_id = Convert.IsDBNull(row["unit_id"]) ? 0 : Convert.ToInt32(row["unit_id"]);
                        sparePartData.selling_price = Convert.IsDBNull(row["selling_price"]) ? 0 : Convert.ToDecimal(row["selling_price"]);
                        sparePartData.min_selling_price = Convert.IsDBNull(row["min_selling_price"]) ? 0 : Convert.ToDecimal(row["min_selling_price"]);
                        sparePartData.quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]);
                        sparePartData.quantity_reserve = Convert.IsDBNull(row["quantity_reserve"]) ? 0 : Convert.ToInt32(row["quantity_reserve"]);
                        sparePartData.quantity_balance = Convert.IsDBNull(row["quantity_balance"]) ? 0 : Convert.ToInt32(row["quantity_balance"]);
                    }

                    var sparePartMapping = new List<ProductSetList>();
                    List<SqlParameter> arrParm2 = new List<SqlParameter>
                        {
                            new SqlParameter("@product_set_id", SqlDbType.Int) { Value = id },
                        };
                    conn.Open();
                    var dsResult = SqlHelper.ExecuteDataset(conn, "sp_spare_part_set_detail_list", arrParm2.ToArray());
                    conn.Close();
                    if (dsResult.Tables.Count > 0)
                    {
                        var data = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                        if (data != null)
                        {
                            foreach (var row in data)
                            {
                                sparePartMapping.Add(new ProductSetList()
                                {
                                    id = Convert.ToInt32(row["id"]),
                                    is_deleted = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                    product_id = Convert.ToInt32(row["product_id"]),
                                    part_name = Convert.IsDBNull("part_name_tha") ? string.Empty : Convert.ToString(row["part_name_tha"]),
                                    part_no = Convert.IsDBNull("part_no") ? string.Empty : Convert.ToString(row["part_no"]),
                                    unit_code = Convert.IsDBNull("unit_code") ? string.Empty : Convert.ToString(row["unit_code"]),
                                    qty = Convert.IsDBNull("qty") ? 0 : Convert.ToInt32(row["qty"]),
                                    product_type = Convert.IsDBNull(row["product_type"]) ? string.Empty : Convert.ToString(row["product_type"])
                                });

                            }
                        }
                    }


                    HttpContext.Current.Session["SESSION_PART_MAPPING_SPARE_PART_SET"] = sparePartMapping;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sparePartData;
        }

        [WebMethod]
        public static string SelectedProduct(string id, bool isSelected, string product_type)
        {
            var returnData = string.Empty;
            List<ProductSetList> selectedProductList = (List<ProductSetList>)HttpContext.Current.Session["SESSION_SELECTED_PART_LIST_SPARE_PART_SET"];
            List<ProductList> sparePartData = (List<ProductList>)HttpContext.Current.Session["SESSION_SPARE_PART_LIST_SPARE_PART_SET"];
            //if (data != null)
            //{

            var rowSparePart = (from t in sparePartData where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
            if (rowSparePart != null)
            {
                if (isSelected)
                {

                    rowSparePart.is_selected = true;

                    if (selectedProductList.Count == 0)
                    {
                        selectedProductList = new List<ProductSetList>();
                    }

                    selectedProductList.Add(new ProductSetList
                    {
                        id = (selectedProductList.Count + 1) * -1,
                        product_id = Convert.ToInt32(id),
                        part_name = rowSparePart.product_name,
                        part_no = rowSparePart.product_no,
                        unit_code = rowSparePart.unit_code,
                        is_deleted = false,
                        qty = 1,
                        quantitySparePart = rowSparePart.quantity,
                        product_type = product_type
                    });

                }


                else
                {

                    rowSparePart.is_selected = false;

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

            HttpContext.Current.Session["SESSION_SELECTED_PART_LIST_SPARE_PART_SET"] = selectedProductList;
            return returnData;
        }

        [WebMethod]
        public static string SubmitSelectProduct()
        {
            try
            {
                var returnData = string.Empty;
                List<ProductSetList> selectedProductList = (List<ProductSetList>)HttpContext.Current.Session["SESSION_SELECTED_PART_LIST_SPARE_PART_SET"];
                List<ProductSetList> productMapingList = new List<ProductSetList>();

                if (selectedProductList != null)
                {
                    productMapingList = selectedProductList;
                }
                HttpContext.Current.Session["SESSION_PART_MAPPING_SPARE_PART_SET"] = productMapingList;
                HttpContext.Current.Session.Remove("SESSION_SELECTED_PART_LIST_SPARE_PART_SET");
                returnData = (from t in productMapingList where t.is_deleted == false select t).ToList().Count().ToString();
                return returnData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static ProductSetList EditProduct(string id)
        {
            try
            {
                var productSetMapping = (List<ProductSetList>)HttpContext.Current.Session["SESSION_PART_MAPPING_SPARE_PART_SET"];
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
                var productSetMapping = (List<ProductSetList>)HttpContext.Current.Session["SESSION_PART_MAPPING_SPARE_PART_SET"];
                if (productSetMapping != null)
                {
                    var row = (from t in productSetMapping where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (row != null)
                    {
                        row.qty = Convert.ToInt32(qty);
                    }
                }
                HttpContext.Current.Session["SESSION_PART_MAPPING_SPARE_PART_SET"] = productSetMapping;

                return "SUCCESS";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gridViewSparePart_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridViewSparePart.DataSource = (from t in sparePartMappingList where !t.is_deleted select t).ToList();
            gridViewSparePart.DataBind();
        }
        protected void BindGridViewSparePart()
        {
            gridViewSparePart.DataSource = (from t in sparePartMappingList where !t.is_deleted select t).ToList();
            gridViewSparePart.DataBind();
        }
        protected void gridViewSelectSparePart_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            ASPxCheckBox checkBox = gridViewSelectSparePart.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkBox") as ASPxCheckBox;
            if (checkBox != null)
            {
                if (e.DataColumn.FieldName == "is_selected")
                {
                    if (selectedSparePartList != null)
                    {
                        var row = (from t in selectedSparePartList
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

        protected void gridViewSelectSparePart_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridViewSelectSparePart.DataSource = sparePartList;
            gridViewSelectSparePart.DataBind();
        }
        protected void BindGridViewSelectSparePart()
        {
            gridViewSelectSparePart.DataSource = sparePartList;
            gridViewSelectSparePart.DataBind();
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
        [WebMethod]
        public static PastName GetSparePartName(string id)
        {
            var data = new PastName();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = "" }
                        };
                    conn.Open();
                    var dsQuataionData = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_list", arrParm.ToArray());
                    conn.Close();

                    if (dsQuataionData.Tables.Count > 0)
                    {
                        if (dsQuataionData.Tables[0].Rows.Count > 0)
                        {
                            var row = (from t in dsQuataionData.Tables[0].AsEnumerable() select t).FirstOrDefault();
                            if (row != null)
                            {
                                data.name_eng = Convert.IsDBNull(row["part_name_eng"]) ? string.Empty : Convert.ToString(row["part_name_eng"]);
                                data.name_tha = Convert.IsDBNull(row["part_name_tha"]) ? string.Empty : Convert.ToString(row["part_name_tha"]);
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
        public static string DeleteProduct(string id)
        {
            var returnData = string.Empty;
            var productMapping = (List<ProductSetList>)HttpContext.Current.Session["SESSION_PART_MAPPING_SPARE_PART_SET"];

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

            HttpContext.Current.Session["SESSION_PART_MAPPING_SPARE_PART_SET"] = productMapping;


            return returnData;
        }

        [WebMethod]
        public static string ValidateData()
        {
            var returnMessage = string.Empty;
            var sparePartMapping = (List<ProductSetList>)HttpContext.Current.Session["SESSION_PART_MAPPING_SPARE_PART_SET"];

            if (sparePartMapping.Count == 0)
            {
                returnMessage += "- กรุณาเลือกสินค้าอะไหล่ อย่างน้อย 1 รายการ <br>";
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
                var sparePartMapping = (List<ProductSetList>)HttpContext.Current.Session["SESSION_PART_MAPPING_SPARE_PART_SET"];
                var row = (from t in sparePartData select t).FirstOrDefault();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        if (row != null)
                        {

                            /*using (SqlCommand cmd = new SqlCommand("sp_master_spare_part_set_edit", conn, tran))
                            {
                                newId = row.id;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                cmd.Parameters.Add("@part_no", SqlDbType.VarChar, 20).Value = row.part_no;
                                cmd.Parameters.Add("@secondary_code", SqlDbType.VarChar, 100).Value = row.secondary_code;
                                cmd.Parameters.Add("@part_name_tha", SqlDbType.VarChar, 150).Value = row.part_name_tha;
                                cmd.Parameters.Add("@part_name_eng", SqlDbType.VarChar, 150).Value = row.part_name_eng;
                                cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = Convert.ToInt32(row.quantity) + Convert.ToInt32(row.quantityStock);
                                cmd.Parameters.Add("@quantity_reserve", SqlDbType.Int).Value = row.quantity_reserve;
                                cmd.Parameters.Add("@quantity_balance", SqlDbType.Int).Value = row.quantity_balance;
                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 150).Value = "SparePart";
                                cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                cmd.ExecuteNonQuery();


                            }*/

                            foreach (var rowSparePart in sparePartMapping)
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_product_set_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    //cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowSparePart.id;
                                    cmd.Parameters.Add("@product_set_id", SqlDbType.Int).Value = row.id;
                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = rowSparePart.product_id;
                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = rowSparePart.qty;
                                    cmd.Parameters.Add("@product_id_to", SqlDbType.Int).Value = Convert.ToInt32(row.id);
                                    cmd.Parameters.Add("@qty_to", SqlDbType.Int).Value = (row.quantity);
                                    cmd.Parameters.Add("@product_set_type", SqlDbType.VarChar, 2).Value = "S";
                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 2).Value = "S";
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                    cmd.ExecuteNonQuery();
                                }
                            }

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
                                cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = "S";
                                cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.id;
                                cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "SET+";
                                cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 1).Value = "A";
                                cmd.Parameters.Add("@checking_type", SqlDbType.VarChar, 1).Value = "M";
                                cmd.Parameters.Add("@qty", SqlDbType.Int).Value = Convert.ToInt32(row.quantity) + Convert.ToInt32(row.quantityStock);
                                cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = Convert.ToInt32(row.quantity) + Convert.ToInt32(row.quantity_balance);
                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "SparePart";
                                cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                cmd.ExecuteNonQuery();
                            }
                            //stam transection - tab12
                            foreach (var rowSparePart in sparePartMapping)
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transection_no;
                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = "S";
                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = rowSparePart.product_id;
                                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "SET-";
                                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 1).Value = "A";
                                    cmd.Parameters.Add("@checking_type", SqlDbType.VarChar, 1).Value = "M";
                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = Convert.ToInt32(rowSparePart.quantitySparePart) - Convert.ToInt32(rowSparePart.qty);
                                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = Convert.ToInt32(rowSparePart.quantitySparePart) - Convert.ToInt32(rowSparePart.qty);
                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "SparePart";
                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = 0;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

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
            }
            return "success";
        }

        [WebMethod]
        public static ProductSetList SelectedPrintSparePartSet(string id)
        {
            var data = new ProductSetList();
            var dsSaleProductSet = new DataSet();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@product_set_id", SqlDbType.Int) { Value = Convert.ToInt32(id) },
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

        protected void gridViewSparePartSet_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = e.Parameters.ToString() }
                        };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_spare_part_set_list", arrParm.ToArray());
                    ViewState["dsResult"] = dsResult;
                }

                var dataGrid = (from t in dsResult.Tables[0].AsEnumerable()
                                select new
                                {
                                    spare_part_set_id = t.Field<int>("product_set_id"),
                                    spare_part_set_name = t.Field<string>("part_name_tha"),
                                    spare_part_set_no = t.Field<string>("part_no"),
                                    count_product_id = Convert.IsDBNull(t["count_product_id"]) ? 0 : Convert.ToInt32(t["count_product_id"]),
                                    is_enable = t.Field<bool>("is_enable"),
                                    qty = t.Field<int>("qty"),
                                }).Distinct().ToList();

                //Bind data into GridView
                gridViewSparePartSet.DataSource = dataGrid;
                gridViewSparePartSet.FilterExpression = FilterBag.GetExpression(false);
                gridViewSparePartSet.DataBind();
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