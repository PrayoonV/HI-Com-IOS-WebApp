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

namespace HicomIOS.Master
{
    public partial class StockCard : MasterDetailPage
    {
        private static string guid = Guid.NewGuid().ToString();
        private const string session_item = "_SESSION_CARD_ITEM";
        private const string session_detail = "_SESSION_CARD_DETAIL";
        private const string session_selected_detail = "_SESSION_CARD_SELECTED_DETAIL";
        public class StockCardData
        {
            public object transaction_date { get; set; }
            public string datepickerFrom { get; set; }
            public string datepickerTo { get; set; }
            public string product_no { get; set; }
            public int product_id { get; set; }
            public string product_name { get; set; }
            public string product_type { get; set; }
            public int product_cat { get; set; }
            public int shelf { get; set; }
            public string secondary_code { get; set; }
            public string no_ { get; set; }
            public string name_ { get; set; }
            public string cat_name { get; set; }
            public string remark { get; set; }
            public int in_ { get; set; }
            public int out_ { get; set; }
            public int adj_ { get; set; }
            public int stock_balance { get; set; }
            public int supplier { get; set; }

        }
        public class ProductDetail
        {
            public int id { get; set; }
            public string item_no { get; set; }
            public string item_name { get; set; }
            public string item_type { get; set; }
            public bool is_selected { get; set; }
            public bool is_delete { get; set; }
        }
        List<StockCardData> stockCardData
        {
            get
            {
                if (Session["SESSION_STOCK_CARD"] == null)
                    Session["SESSION_STOCK_CARD"] = new List<StockCardData>();
                return (List<StockCardData>)Session["SESSION_STOCK_CARD"];
            }
            set
            {
                Session["SESSION_STOCK_CARD"] = value;
            }
        }
        List<ProductDetail> selectedItemDetailList
        {
            get
            {

                if (Session[guid + session_item] == null)
                    Session[guid + session_item] = new List<ProductDetail>();
                return (List<ProductDetail>)Session[guid + session_item];
            }
            set
            {
                Session[guid + session_item] = value;
            }
        }
        public override string PageName { get { return "Stock Card"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Get Permission and if no permission, will redirect to another page.
                if (!Permission.GetPermission())
                    Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

                PrepareData();
                ClearWoringSession();
            }
            else
            {
                BindDataGrid();
            }
        }
        private void BindDataGrid()
        {
            gridView.DataSource = (from t in stockCardData select t).ToList();
            gridView.DataBind();
        }
        protected void ClearWoringSession()
        {
            Session.Remove("SESSION_STOCK_CARD");
        }
        protected void PrepareData()
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    var dtSource = new DataTable();
                    dtSource.Columns.Add("data_value", typeof(string));
                    dtSource.Columns.Add("data_text", typeof(string));

                    dtSource.Rows.Add("SS", "กรุณาเลือก");

                    var serviceType = Session["DEPARTMENT_SERVICE_TYPE"].ToString();
                    if (serviceType.Contains("P"))
                    {
                        dtSource.Rows.Add("PP", "Product");
                    }
                    if (serviceType.Contains("S"))
                    {
                        dtSource.Rows.Add("SP", "Spare Part");
                    }
                    cbbProductType.DataSource = dtSource;
                    cbbProductType.DataBind();

                    cbbProductType.Value = "SS";// (serviceType.Contains("P") ? "PP" : "SP");
                }
                //SPlanetUtil.BindASPxComboBox(ref cbbProduct, DataListUtil.DropdownStoreProcedureName.Product);
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

        public override void OnFilterChanged()
        {
            //BindGrid();
        }
        [WebMethod]
        public static void ChangedData()
        {
            HttpContext.Current.Session.Remove("SESSION_STOCK_CARD");
        }
        [WebMethod]
        public static string GetViewGrid(StockCardData[] dataStockCard)
        {
            var result = "";
            var stockCardData = new List<StockCardData>();
            var data = (from t in dataStockCard select t).FirstOrDefault();
            if (data != null)
            {
                /*var adjustDetailList = (List<ProductDetail>)HttpContext.Current.Session[guid + session_detail];
                if (adjustDetailList != null)
                {
                    foreach (var row in adjustDetailList)
                    {
                        if (!data.product_no.Equals(""))
                        {
                            data.product_no += ",";
                        }
                        data.product_no += row.item_no;
                    }
                }*/
                try
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                             new SqlParameter("@product_name", SqlDbType.VarChar,200) { Value = data.product_name.Trim() },
                             new SqlParameter("@date_from", SqlDbType.VarChar,20) { Value = data.datepickerFrom },
                             new SqlParameter("@date_to", SqlDbType.VarChar,20) { Value = data.datepickerTo },
                             new SqlParameter("@product_type", SqlDbType.VarChar,3) { Value = data.product_type },
                             new SqlParameter("@shelf", SqlDbType.Int) { Value = data.shelf },
                             new SqlParameter("@secondary_code", SqlDbType.VarChar, 200) { Value = data.secondary_code.Trim() },
                             new SqlParameter("@product_cat", SqlDbType.Int) { Value = data.product_cat },
                             new SqlParameter("@product_no", SqlDbType.VarChar, 200) { Value = data.product_no.Trim() },
                             new SqlParameter("@supplier", SqlDbType.Int) { Value = data.supplier },
                        };
                        conn.Open();
                        var dsDatastockCard = SqlHelper.ExecuteDataset(conn, "sp_stock_card_report", arrParm.ToArray());

                        if (dsDatastockCard.Tables[0].Rows.Count > 0)
                        {
                            var productNo = "";
                            var actualBalance = 0;
                            foreach (var row in dsDatastockCard.Tables[0].AsEnumerable())
                            {
                                if (!productNo.Equals(Convert.ToString(row["no_"])))
                                {
                                    productNo = Convert.ToString(row["no_"]);

                                    //  Add previous balance
                                    var in_ = Convert.IsDBNull(row["in_"]) ? 0 : Convert.ToInt32(row["in_"]);
                                    var out_ = Convert.IsDBNull(row["out_"]) ? 0 : Convert.ToInt32(row["out_"]);
                                    var adj_ = Convert.IsDBNull(row["adj_"]) ? 0 : Convert.ToInt32(row["adj_"]);
                                    var balance = Convert.IsDBNull(row["stock_balance"]) ? 0 : Convert.ToInt32(row["stock_balance"]);
                                    var totalBalance = balance - in_ + out_ + adj_;
                                    actualBalance = totalBalance;
                                    stockCardData.Add(new StockCardData()
                                    {
                                        transaction_date = "",
                                        no_ = Convert.IsDBNull(row["no_"]) ? string.Empty : Convert.ToString(row["no_"]),
                                        name_ = Convert.IsDBNull(row["name_"]) ? string.Empty : Convert.ToString(row["name_"]),
                                        remark = "Previous Balance",
                                        in_ = 0,
                                        out_ = 0,
                                        adj_ = 0,
                                        cat_name = Convert.IsDBNull(row["cat_name_tha"]) ? string.Empty : Convert.ToString(row["cat_name_tha"]),
                                        stock_balance = actualBalance,
                                    });
                                }

                                var in_2 = Convert.IsDBNull(row["in_"]) ? 0 : Convert.ToInt32(row["in_"]);
                                var out_2 = Convert.IsDBNull(row["out_"]) ? 0 : Convert.ToInt32(row["out_"]);
                                var adj_2 = Convert.IsDBNull(row["adj_"]) ? 0 : Convert.ToInt32(row["adj_"]);
                                actualBalance += (in_2 - out_2 - adj_2);
                                stockCardData.Add(new StockCardData()
                                {
                                    transaction_date = "",
                                    no_ = Convert.ToString(row["transaction_date"]),
                                    name_ = Convert.IsDBNull(row["remark"]) ? string.Empty : Convert.ToString(row["remark"]),
                                    remark = Convert.IsDBNull(row["extra_remark"]) ? string.Empty : Convert.ToString(row["extra_remark"]),
                                    in_ = in_2,
                                    out_ = out_2,
                                    adj_ = adj_2,
                                    cat_name = "",
                                    stock_balance = actualBalance,
                                });
                            }
                        }
                    }

                    HttpContext.Current.Session["SESSION_STOCK_CARD"] = stockCardData;
                    if (stockCardData.Count < 0)
                    {
                        result = "error";
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;

        }
        protected void gridStockCard_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridView.DataSource = (from t in stockCardData select t).ToList();
            gridView.DataBind();
        }

        protected void gridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            var t = stockCardData[e.VisibleIndex];
            if (t.remark.Equals("Previous Balance"))
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
            }
            else
            {
                e.Cell.BackColor = System.Drawing.Color.White;
            }
        }

        protected void cbbProductCat_Callback(object sender, CallbackEventArgsBase e)
        {
            var productType = e.Parameter.ToString();
            cbbProductCat_RetreiveDataIntoCombo(productType);
        }
        private void cbbProductCat_RetreiveDataIntoCombo(string productType)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@lang_id", SqlDbType.VarChar,3) { Value = "tha" },
                            new SqlParameter("@cat_type", SqlDbType.Int) { Value = productType },
                        };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_product_category_export", arrParm.ToArray());
                conn.Close();
                cbbProductCat.DataSource = dsResult;
                cbbProductCat.TextField = "data_text";
                cbbProductCat.ValueField = "data_value";
                cbbProductCat.DataBind();

                cbbProductCat.Items.Insert(0, new ListEditItem("All Category", "0"));
            }
        }

        //protected void cbbProduct_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    var productTtype = e.Parameter.ToString();
        //    try
        //    {
        //        if (productTtype == "PP")
        //        {
        //            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
        //            {
        //                List<SqlParameter> arrParm = new List<SqlParameter>
        //            {
        //                new SqlParameter("@lang_id", SqlDbType.VarChar,3) { Value = ""},
        //            };
        //                conn.Open();
        //                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_product_stock_card", arrParm.ToArray());
        //                conn.Close();
        //                if (dsResult.Tables.Count > 0)
        //                {
        //                    if (dsResult.Tables[0].Rows.Count > 0)
        //                    {
        //                        cbbProduct.DataSource = dsResult;
        //                        cbbProduct.TextField = "data_text";
        //                        cbbProduct.ValueField = "data_value";
        //                        //cbbProduct.SelectedIndex = "All";
        //                        cbbProduct.DataBind();
        //                    }
        //                }
        //            }
        //        }
        //        else if (productTtype == "SP")
        //        {
        //            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
        //            {
        //                List<SqlParameter> arrParm = new List<SqlParameter>
        //            {
        //                new SqlParameter("@lang_id", SqlDbType.VarChar,3) { Value = ""},
        //            };
        //                conn.Open();
        //                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_product_spare_part", arrParm.ToArray());
        //                conn.Close();
        //                if (dsResult.Tables.Count > 0)
        //                {
        //                    if (dsResult.Tables[0].Rows.Count > 0)
        //                    {
        //                        cbbProduct.DataSource = dsResult;
        //                        cbbProduct.TextField = "data_text";
        //                        cbbProduct.ValueField = "data_value";
        //                        cbbProduct.DataBind();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        protected void cbbProduct_Callback(object sender, CallbackEventArgsBase e)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                List<SqlParameter> arrParm = new List<SqlParameter>
                    {
                        new SqlParameter("@lang_id", SqlDbType.VarChar,3) { Value = ""},
                    };
                conn.Open();
                var storeName = cbbProductType.Value.ToString() == "PP" ? "sp_dropdown_product" : "sp_dropdown_product_spare_part";
                var dsResult = SqlHelper.ExecuteDataset(conn, storeName, arrParm.ToArray());
                conn.Close();
                cbbProduct.DataSource = dsResult;
                cbbProduct.TextField = "data_text";
                cbbProduct.ValueField = "data_value";
                cbbProduct.DataBind();

                var text = (cbbProductType.Value.ToString() == "PP" ? "All Product" : "All Part");
                cbbProduct.Items.Insert(0, new ListEditItem(text, "0"));
            }
        }
        
        protected void cbbSecondaryCode_Callback(object sender, CallbackEventArgsBase e)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                List<SqlParameter> arrParm = new List<SqlParameter>
                    {
                        new SqlParameter("@lang_id", SqlDbType.VarChar,3) { Value = ""},
                    };
                conn.Open();
                var storeName = cbbProductType.Value.ToString() == "PP" ? "sp_dropdown_product" : "sp_dropdown_product_spare_part";
                var dsResult = SqlHelper.ExecuteDataset(conn, storeName, arrParm.ToArray());
                conn.Close();
                cbbSecondaryCode.DataSource = dsResult;
                cbbSecondaryCode.TextField = "data_text";
                cbbSecondaryCode.ValueField = "data_value";
                cbbSecondaryCode.DataBind();

                cbbSecondaryCode.Items.Insert(0, new ListEditItem("All Secondary Code", "0"));
            }
        }

        protected void cbbShelf_Callback(object sender, CallbackEventArgsBase e)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                List<SqlParameter> arrParm = new List<SqlParameter>
                    {
                        new SqlParameter("@lang_id", SqlDbType.VarChar,3) { Value = ""},
                    };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_shelf", arrParm.ToArray());
                conn.Close();
                cbbShelf.DataSource = dsResult;
                cbbShelf.TextField = "data_text";
                cbbShelf.ValueField = "data_value";
                cbbShelf.DataBind();

                cbbShelf.Items.Insert(0, new ListEditItem("All Shelf", "0"));
            }
        }

        protected void cbbSupplier_Callback(object sender, CallbackEventArgsBase e)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                List<SqlParameter> arrParm = new List<SqlParameter>
                    {
                        new SqlParameter("@lang_id", SqlDbType.VarChar,3) { Value = ""},
                    };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_supplier", arrParm.ToArray());
                conn.Close();
                cbbSupplier.DataSource = dsResult;
                cbbSupplier.TextField = "data_text";
                cbbSupplier.ValueField = "data_value";
                cbbSupplier.DataBind();

                cbbSupplier.Items.Insert(0, new ListEditItem("All Supplier", "0"));
            }
        }

        [WebMethod]
        public static string PopupItemDetail(string item_type)
        {
            try
            {
                var selectedItemList = new List<ProductDetail>();
                var adjustDetailList = (List<ProductDetail>)HttpContext.Current.Session[guid + session_detail];
                if (adjustDetailList != null)
                {
                    selectedItemList.AddRange(adjustDetailList);
                }
                HttpContext.Current.Session[guid + session_selected_detail] = selectedItemList;

                DataSet dsResult = new DataSet();
                var productList = new List<ProductDetail>();
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
                            productList.Add(new ProductDetail()
                            {
                                is_selected = false,
                                id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                item_name = Convert.IsDBNull(row["product_name_tha"]) ? string.Empty : Convert.ToString(row["product_name_tha"]),
                                item_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                item_type = "P",

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
                                productList.Add(new ProductDetail()
                                {
                                    is_selected = false,
                                    id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                    item_no = Convert.IsDBNull(row["part_no"]) ? string.Empty : Convert.ToString(row["part_no"]),
                                    item_name = Convert.IsDBNull(row["part_name_tha"]) ? string.Empty : Convert.ToString(row["part_name_tha"]),
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
            var selectedItemList = (List<ProductDetail>)HttpContext.Current.Session[guid + session_selected_detail];
            var productList = (List<ProductDetail>)HttpContext.Current.Session[guid + session_item];
            if (productList != null)
            {
                var item = productList.Where(t => t.id == key).FirstOrDefault();
                if (item != null)
                {
                    if (selectedItemList == null)
                    {
                        selectedItemList = new List<ProductDetail>();
                    }
                    if (isSelected)
                    {
                        selectedItemList.Add(new ProductDetail()
                        {
                            id = (selectedItemList.Count + 1) * -1,
                            is_delete = false,
                            item_name = item.item_name,
                            item_no = item.item_no,
                            item_type = item.item_type

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
            var selectedItemList = (List<ProductDetail>)HttpContext.Current.Session[guid + session_selected_detail];
            var adjustDetailList = (List<ProductDetail>)HttpContext.Current.Session[guid + session_detail];

            if (selectedItemList != null)
            {

                if (adjustDetailList == null)
                {
                    adjustDetailList = new List<ProductDetail>();
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
                var deletedItem = new List<ProductDetail>();
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

            var result = "";
            foreach (var row in adjustDetailList)
            {
                if (result != "") {
                    result += ",";
                }
                result += row.item_no;
            }

            return result;
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

        protected void btnReportExcel_Click(object sender, EventArgs e)
        {
            gridViewExporter.WriteXlsxToResponse(DateTime.Now.ToString("yyyy-MM-dd") + "-Stock Card");
        }

    }

}

