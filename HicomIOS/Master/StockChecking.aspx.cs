using DevExpress.Web;
using HicomIOS.ClassUtil;
using Microsoft.ApplicationBlocks.Data;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;

namespace HicomIOS.Master
{
    public partial class StockChecking : MasterDetailPage
    {
        //private static Excel.Workbook MyBook = null;
        //private static Excel.Application MyApp = null;
        //private static Excel.Worksheet MySheet = null;


        private string dataNo = string.Empty;
        public override string PageName { get { return "Create Stock Checking"; } }
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
        public class ProductDetail
        {
            public bool is_selected { get; set; }
            public int product_id { get; set; }
            public string product_no { get; set; }
            public string product_name { get; set; }
            public string item_type { get; set; }
            public string item_unit { get; set; }
            public int qty { get; set; }
            public int quantity_reserve { get; set; }
        }

        public class StockCheckingDetail
        {
            public int id { get; set; }
            public int product_id { get; set; }
            public string product_no { get; set; }
            public string product_name { get; set; }
            public string unit_code { get; set; }
            public int before_qty { get; set; }
            public int quantity { get; set; }
            public int quantity_remain { get; set; }
            public int quantity_reserve { get; set; }
            public string remark { get; set; }
            public bool is_delete { get; set; }
            public string created_date { get; set; }
            public string transection_no { get; set; }
        }

        List<ProductDetail> productDetailList
        {
            get
            {
                if (Session["SESSION_PRODUCT_DETAIL_STOCK_CHECKING"] == null)
                    Session["SESSION_PRODUCT_DETAIL_STOCK_CHECKING"] = new List<ProductDetail>();
                return (List<ProductDetail>)Session["SESSION_PRODUCT_DETAIL_STOCK_CHECKING"];
            }
            set
            {
                Session["SESSION_PRODUCT_DETAIL_STOCK_CHECKING"] = value;
            }

        }
        List<StockCheckingDetail> stockCheckingDetailList
        {
            get
            {
                if (Session["SESSION_STOCK_CHECKING_DETAIL_STOCK_CHECKING"] == null)
                    Session["SESSION_STOCK_CHECKING_DETAIL_STOCK_CHECKING"] = new List<StockCheckingDetail>();
                return (List<StockCheckingDetail>)Session["SESSION_STOCK_CHECKING_DETAIL_STOCK_CHECKING"];
            }
            set
            {
                Session["SESSION_STOCK_CHECKING_DETAIL_STOCK_CHECKING"] = value;
            }

        }
        List<StockCheckingDetail> selectedStockCheckingDetailList
        {
            get
            {
                if (Session["SESSION_SELECTED_STOCK_CHECKING_DETAIL_STOCK_CHECKING"] == null)
                    Session["SESSION_SELECTED_STOCK_CHECKING_DETAIL_STOCK_CHECKING"] = new List<StockCheckingDetail>();
                return (List<StockCheckingDetail>)Session["SESSION_SELECTED_STOCK_CHECKING_DETAIL_STOCK_CHECKING"];
            }
            set
            {
                Session["SESSION_SELECTED_STOCK_CHECKING_DETAIL_STOCK_CHECKING"] = value;
            }

        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dataNo = Convert.ToString(Request.QueryString["dataNo"]);
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
                    BindGridViewProductDetail();
                    BindGridViewStockCheckDetail();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        protected void PrepareData()
        {
            try
            {
                //using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                //{
                //    using (SqlCommand cmd = new SqlCommand("sp_gen_transection_no", conn))
                //    {
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        conn.Open();
                //        txtStockCheckNo.Value = Convert.ToString(cmd.ExecuteScalar());
                //        conn.Close();
                //    }
                //}
                //txtStockCheckDate.Value = DateTime.UtcNow.ToString("dd/MM/yyyy");
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
                var dsResult = new DataSet();
                if (!string.IsNullOrEmpty(dataNo))
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                                {
                                    new SqlParameter("@transection_no", SqlDbType.VarChar,20) { Value = dataNo },
                                };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_stock_movement_by_transection_no", arrParm.ToArray());
                        conn.Close();
                    }
                    stockCheckingDetailList = new List<StockCheckingDetail>();
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        var dsTransection = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                        foreach (var row in dsTransection)
                        {
                            stockCheckingDetailList.Add(new StockCheckingDetail()
                            {
                                id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                is_delete = false,
                                product_id = Convert.IsDBNull(row["product_id"]) ? 0 : Convert.ToInt32(row["product_id"]),
                                product_name = Convert.IsDBNull(row["product_name"]) ? string.Empty : Convert.ToString(row["product_name"]),
                                product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                                before_qty = Convert.IsDBNull(row["before_qty"]) ? 0 : Convert.ToInt32(row["before_qty"]),
                                quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]),
                                quantity_remain = Convert.IsDBNull(row["quantity_remain"]) ? 0 : Convert.ToInt32(row["quantity_remain"]),
                                quantity_reserve = Convert.IsDBNull(row["quantity_reserve"]) ? 0 : Convert.ToInt32(row["quantity_reserve"]),
                                remark = Convert.IsDBNull(row["remark"]) ? string.Empty : Convert.ToString(row["remark"]),
                                unit_code = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]),
                                created_date = Convert.IsDBNull(row["created_date"]) ? string.Empty : Convert.ToDateTime(row["created_date"]).ToString("dd/MM/yyyy"),
                                transection_no = Convert.IsDBNull(row["transection_no"]) ? string.Empty : Convert.ToString(row["transection_no"]),
                            });
                        }
                    }
                    if (stockCheckingDetailList.Count > 0)
                    {
                        txtStockCheckDate.Value = stockCheckingDetailList[0].created_date;
                        txtStockCheckNo.Value = stockCheckingDetailList[0].transection_no;
                    }
                    gridViewStockCheckDetail.DataSource = stockCheckingDetailList;
                    gridViewStockCheckDetail.DataBind();
                    
                    legend_step1.InnerText = "รายละเอียด";
                    fieldset_step2.Visible = false;
                    fieldset_step3.Visible = false;
                    btnSave.Visible = false;
                    btnAddProduct.Visible = false;
                    btnExport.Visible = false;
                    btnImportExcel.Visible = false;
                    cbbProductType.Attributes["disabled"] = "disabled";

                    gridViewStockCheckDetail.Columns[0].Visible = false;
                }
                else
                {
                    gridViewStockCheckDetail.Columns[4].Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        protected void ClearWorkingSession()
        {
            Session.Remove("SESSION_PRODUCT_DETAIL_STOCK_CHECKING");
            Session.Remove("SESSION_STOCK_CHECKING_DETAIL_STOCK_CHECKING");
            Session.Remove("SESSION_SELECTED_STOCK_CHECKING_DETAIL_STOCK_CHECKING");
        }

        protected void gridViewStockCheckDetail_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            gridViewStockCheckDetail.DataSource = (from t in stockCheckingDetailList where t.is_delete == false select t).ToList(); ;
            gridViewStockCheckDetail.DataBind();
        }
        private void BindGridViewStockCheckDetail()
        {
            gridViewStockCheckDetail.DataSource = (from t in stockCheckingDetailList where t.is_delete == false select t).ToList(); ;
            gridViewStockCheckDetail.DataBind();
        }

        protected void gridViewProductDetail_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            gridViewProductDetail.DataSource = productDetailList;
            gridViewProductDetail.DataBind();
        }
        private void BindGridViewProductDetail()
        {
            gridViewProductDetail.DataSource = productDetailList;
            gridViewProductDetail.DataBind();
        }
        protected void gridViewProductDetail_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxCheckBox checkBox = gridViewProductDetail.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkProductDetail") as ASPxCheckBox;
                if (checkBox != null)
                {
                    if (e.DataColumn.FieldName == "is_selected")
                    {
                        if (productDetailList != null)
                        {
                            var row = (from t in productDetailList where t.product_no == Convert.ToString(e.KeyValue) select t).FirstOrDefault();
                            if (row != null)
                            {
                                checkBox.Checked = row.is_selected;
                            }
                        }
                        //checkBox.Checked = (row["is_selected"]) == DBNull.Value ? false : true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {

        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            SaveData();
        }
        [WebMethod]
        public static string GetProductDetail(string type)
        {
            var returnData = string.Empty;
            var data = new List<ProductDetail>();
            var dsProduct = new DataSet();
            if (type == "S")
            {

                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                                {
                                    new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                                    new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                                    
                                };
                    conn.Open();
                    dsProduct = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_list", arrParm.ToArray());
                    conn.Close();
                }
                if (dsProduct != null)
                {
                    //var detailRows = (from t in dsProduct.Tables[0].AsEnumerable() select t).ToList();
                    foreach (DataRow row in dsProduct.Tables[0].Rows)
                    {
                        data.Add(new ProductDetail()
                        {
                            is_selected = false,
                            item_type = "S",
                            item_unit = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]),
                            product_id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                            product_name = Convert.IsDBNull(row["part_name_tha"]) ? string.Empty : Convert.ToString(row["part_name_tha"]),
                            product_no = Convert.IsDBNull(row["part_no"]) ? string.Empty : Convert.ToString(row["part_no"]),
                            quantity_reserve = Convert.IsDBNull(row["quantity_reserve"]) ? 0 : Convert.ToInt32(row["quantity_reserve"]),
                            qty = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]),
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
                                    new SqlParameter("@search_name",SqlDbType.VarChar,200) {Value = "" },
                                    new SqlParameter("@product_no",SqlDbType.VarChar,50) {Value = "" },
                                    new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                                };
                    conn.Open();
                    dsProduct = SqlHelper.ExecuteDataset(conn, "sp_product_list", arrParm.ToArray());
                    conn.Close();
                }
                if (dsProduct != null)
                {
                    var detailRows = (from t in dsProduct.Tables[0].AsEnumerable() select t).ToList();
                    foreach (var row in detailRows)
                    {
                        data.Add(new ProductDetail()
                        {
                            is_selected = false,
                            item_type = "P",
                            item_unit = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]),
                            product_id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                            product_name = Convert.IsDBNull(row["product_name_tha"]) ? string.Empty : Convert.ToString(row["product_name_tha"]),
                            product_no = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]),
                            quantity_reserve = Convert.IsDBNull(row["quantity_reserve"]) ? 0 : Convert.ToInt32(row["quantity_reserve"]),
                            qty = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]),
                        });
                    }
                }

            }
            HttpContext.Current.Session["SESSION_PRODUCT_DETAIL_STOCK_CHECKING"] = data;
            return returnData;
        }

        [WebMethod]
        public static string AddSelectedProduct(string key, bool isSelected)
        {
            try
            {
                var productDetail = (List<ProductDetail>)HttpContext.Current.Session["SESSION_PRODUCT_DETAIL_STOCK_CHECKING"];
                var selectedStockDetail = (List<StockCheckingDetail>)HttpContext.Current.Session["SESSION_SELECTED_STOCK_CHECKING_DETAIL_STOCK_CHECKING"];


                if (productDetail != null) // ต้องหา Row เจอเท่านั้น ถึงจะทำต่อ
                {
                    var selectedProductRow = (from t in productDetail
                                              where t.product_no == Convert.ToString(key)
                                              select t).FirstOrDefault();
                    if (selectedProductRow != null)
                    {
                        if (isSelected)
                        {
                            selectedProductRow.is_selected = true;
                            if (selectedStockDetail != null) // ถ้ามี PRDetail อยุ่แล้ว
                            {
                                var checkExist = (from t in selectedStockDetail where t.product_no == Convert.ToString(key) select t).FirstOrDefault();
                                if (checkExist == null)
                                {
                                    selectedStockDetail.Add(new StockCheckingDetail()
                                    {
                                        id = (selectedStockDetail.Count + 1) * -1,
                                        is_delete = false,
                                        product_name = selectedProductRow.product_name,
                                        product_id = selectedProductRow.product_id,
                                        quantity = selectedProductRow.qty,
                                        product_no = selectedProductRow.product_no,
                                        unit_code = selectedProductRow.item_unit,
                                        before_qty = 0,
                                        quantity_remain = 0,
                                        quantity_reserve = selectedProductRow.quantity_reserve,
                                        remark = string.Empty
                                        //item_type = selectedProductRow.item_type
                                    });
                                }
                                else // ไม่ต้องทำอะไร
                                {

                                }
                            }
                            else // ใหม่เอี่ยม
                            {
                                selectedStockDetail = new List<StockCheckingDetail>();
                                selectedStockDetail.Add(new StockCheckingDetail()
                                {
                                    id = (selectedStockDetail.Count + 1) * -1,
                                    is_delete = false,
                                    product_name = selectedProductRow.product_name,
                                    product_id = selectedProductRow.product_id,
                                    quantity = selectedProductRow.qty,
                                    product_no = selectedProductRow.product_no,
                                    unit_code = selectedProductRow.item_unit,
                                    before_qty = 0,
                                    quantity_remain = 0,
                                    quantity_reserve = selectedProductRow.quantity_reserve,
                                    remark = string.Empty
                                });
                            }
                        }
                        else
                        {
                            selectedProductRow.is_selected = false;
                            var checkExist = (from t in selectedStockDetail where t.product_no == Convert.ToString(key) select t).FirstOrDefault();
                            if (checkExist != null) // ต้องเข้า case นี้ เท่านั้น
                            {
                                if (checkExist.id < 0)
                                {
                                    selectedStockDetail.Remove(checkExist);
                                }
                                else
                                {
                                    checkExist.is_delete = true;
                                }
                            }
                            else
                            {

                            }

                        }
                    }
                }


                HttpContext.Current.Session["SESSION_SELECTED_STOCK_CHECKING_DETAIL_STOCK_CHECKING"] = selectedStockDetail;
                HttpContext.Current.Session["SESSION_PRODUCT_DETAIL_STOCK_CHECKING"] = productDetail;

                return "SUCCESS";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [WebMethod]
        public static string SubmitProductDetail()
        {
            try
            {
                var selectedStockDetail = (List<StockCheckingDetail>)HttpContext.Current.Session["SESSION_SELECTED_STOCK_CHECKING_DETAIL_STOCK_CHECKING"];
                var stockDetailData = (List<StockCheckingDetail>)HttpContext.Current.Session["SESSION_STOCK_CHECKING_DETAIL_STOCK_CHECKING"];
                if (selectedStockDetail != null)
                {
                    stockDetailData = new List<StockCheckingDetail>();
                    stockDetailData = selectedStockDetail;
                }
                HttpContext.Current.Session["SESSION_STOCK_CHECKING_DETAIL_STOCK_CHECKING"] = stockDetailData;
                HttpContext.Current.Session["SESSION_SELECTED_STOCK_CHECKING_DETAIL_STOCK_CHECKING"] = null;

                return "SUCCESS";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [WebMethod]
        public static StockCheckingDetail GetStockDetailEdit(string id)
        {
            var data = new StockCheckingDetail();
            var stockDetailData = (List<StockCheckingDetail>)HttpContext.Current.Session["SESSION_STOCK_CHECKING_DETAIL_STOCK_CHECKING"];
            if (stockDetailData != null)
            {
                data = (from t in stockDetailData where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
            }

            return data;
        }

        [WebMethod]
        public static string SubmitStockDetailEdit(string id, string qty_remain, string remark)
        {
            var stockDetailData = (List<StockCheckingDetail>)HttpContext.Current.Session["SESSION_STOCK_CHECKING_DETAIL_STOCK_CHECKING"];
            if (stockDetailData != null)
            {
                var row = (from t in stockDetailData where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (row != null)
                {
                    row.quantity_remain = Convert.ToInt32(qty_remain);
                    row.remark = remark;
                }

            }

            return "SUCCESS";
        }

        
        [WebMethod]
        public static string ClearData()
        {
            string returnData = string.Empty;
            HttpContext.Current.Session.Remove("SESSION_STOCK_CHECKING_DETAIL_STOCK_CHECKING");
            HttpContext.Current.Session.Remove("SESSION_SELECTED_STOCK_CHECKING_DETAIL_STOCK_CHECKING");
            return returnData;
        }

        private void SaveData()
        {
            var transNo = string.Empty;

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_gen_transection_no", conn, tran))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            transNo = Convert.ToString(cmd.ExecuteScalar());

                        }
                        string stock_type = string.Empty;
                        string transection_type = string.Empty;
                        foreach (var row in stockCheckingDetailList)
                        {
                            var diffQty = row.quantity - row.quantity_remain;
                            if (diffQty == 0)
                                transection_type = "CORRECT";
                            else if (diffQty > 0)
                                transection_type = "STOCKOUT";
                            else
                                transection_type = "STOCKIN";

                            using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;
                                cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = cbbProductType.Value;
                                cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.product_id;

                                cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = transection_type;
                                cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "ADJ";//diffQty < 0 ? "IN" : "OUT";
                                cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.quantity_remain;
                                cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = row.quantity_remain;
                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = row.remark;
                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = DBNull.Value; // คำนวนจาก Store
                                cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = DBNull.Value;
                                cmd.ExecuteNonQuery();
                            }
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

                            Response.Redirect("StockChecking.aspx?dataNo=" + transNo);
                        }
                    }

                }
            }
        }

        #region Export Excel
        [WebMethod]
        public static string ExportExcel2(string type)
        {
            string strTemplatePath = "/Master/Template/";
            string baseUrl = string.Empty;
            String strExcelPath = string.Empty;
            string strProductTemplateFile = "Stock_Checking-Template_Product.xlsx";
            string strSparePartTemplateFile = "Stock_Checking-Template_Spare_Part.xlsx";
            int intStartRow = 2;
            DataSet dsResult;
            string strDownloadUrl = string.Empty;
            if (type == "P")
            {
                string excelTemplate = Path.Combine(HttpContext.Current.Server.MapPath(strTemplatePath), strProductTemplateFile);
                FileInfo templateFile = new FileInfo(excelTemplate);
                ExcelPackage excel = new ExcelPackage(templateFile);
                var templateSheet = excel.Workbook.Worksheets["Sheet1"];
                var workSheet = excel.Workbook.Worksheets.Copy("Sheet1", "Product");
                workSheet.DeleteRow(2, 1048576 - 2);
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
                    var i = 0;
                    foreach (DataRow row in dsResult.Tables[0].Rows)
                    {
                        i++;
                        templateSheet.SelectedRange["A2:XFA2"].Copy(workSheet.SelectedRange[intStartRow, 1]);
                        workSheet.Cells[intStartRow, 1].Value = i;
                        workSheet.Cells[intStartRow, 2].Value = Convert.IsDBNull(row["id"]) ? string.Empty : Convert.ToString(row["id"]);
                        workSheet.Cells[intStartRow, 3].Value = Convert.IsDBNull(row["product_no"]) ? string.Empty : Convert.ToString(row["product_no"]);
                        workSheet.Cells[intStartRow, 4].Value = Convert.IsDBNull(row["product_name_tha"]) ? string.Empty : Convert.ToString(row["product_name_tha"]);
                        workSheet.Cells[intStartRow, 5].Value = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]);
                        workSheet.Cells[intStartRow, 6].Value = Convert.IsDBNull(row["quantity"]) ? string.Empty : Convert.ToString(row["quantity"]);
                        workSheet.Cells[intStartRow, 7].Value = Convert.IsDBNull(row["quantity_reserve"]) ? string.Empty : Convert.ToString(row["quantity_reserve"]);
                        //workSheet.Cells[intStartRow, 8].Value = 0;
                        // แก้ไขเป็น ให้จำนวนเท่ากับ จำนวนปัจจุบันไปก่อน
                        workSheet.Cells[intStartRow, 8].Value = Convert.IsDBNull(row["quantity"]) ? string.Empty : Convert.ToString(row["quantity"]);
                        workSheet.Cells[intStartRow, 9].Value = string.Empty;

                        intStartRow++;
                    }
                }
                templateSheet = excel.Workbook.Worksheets["Sheet1"];
                excel.Workbook.Worksheets.Delete(templateSheet);
                //strDownloadUrl = baseUrl + strTemplatePath + strProductTemplateFile;
                string pathTemp = Path.GetTempFileName();
                excel.SaveAs(new FileInfo(pathTemp));

                strDownloadUrl = string.Format("LocalPath={0}&FileName={1}", pathTemp, "ProductStockChecking.xlsx");


            }
            else if (type == "S")
            {
                string excelTemplate = Path.Combine(HttpContext.Current.Server.MapPath(strTemplatePath), strSparePartTemplateFile);
                FileInfo templateFile = new FileInfo(excelTemplate);
                ExcelPackage excel = new ExcelPackage(templateFile);
                var templateSheet = excel.Workbook.Worksheets["Sheet1"];
                var workSheet = excel.Workbook.Worksheets.Copy("Sheet1", "SparePart");
                workSheet.DeleteRow(2, 1048576 - 2);
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                    {
                        new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                        new SqlParameter("@search_text", SqlDbType.VarChar) { Value = "" },
                    };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_list", arrParm.ToArray());
                    var i = 0;
                    foreach (DataRow row in dsResult.Tables[0].Rows)
                    {
                        i++;
                        templateSheet.SelectedRange["A2:XFA2"].Copy(workSheet.SelectedRange[intStartRow, 1]);
                        workSheet.Cells[intStartRow, 1].Value = i;
                        workSheet.Cells[intStartRow, 2].Value = Convert.IsDBNull(row["id"]) ? string.Empty : Convert.ToString(row["id"]);
                        workSheet.Cells[intStartRow, 3].Value = Convert.IsDBNull(row["part_no"]) ? string.Empty : Convert.ToString(row["part_no"]);
                        workSheet.Cells[intStartRow, 4].Value = Convert.IsDBNull(row["secondary_code"]) ? string.Empty : Convert.ToString(row["secondary_code"]);
                        workSheet.Cells[intStartRow, 5].Value = Convert.IsDBNull(row["maintenance_type"]) ? string.Empty : Convert.ToString(row["maintenance_type"]);
                        workSheet.Cells[intStartRow, 6].Value = Convert.IsDBNull(row["part_name_tha"]) ? string.Empty : Convert.ToString(row["part_name_tha"]);
                        workSheet.Cells[intStartRow, 7].Value = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"]);
                        workSheet.Cells[intStartRow, 8].Value = Convert.IsDBNull(row["quantity"]) ? string.Empty : Convert.ToString(row["quantity"]);
                        workSheet.Cells[intStartRow, 9].Value = Convert.IsDBNull(row["quantity_reserve"]) ? string.Empty : Convert.ToString(row["quantity_reserve"]);
                        workSheet.Cells[intStartRow, 10].Value = Convert.IsDBNull(row["quantity"]) ? string.Empty : Convert.ToString(row["quantity"]);
                        workSheet.Cells[intStartRow, 11].Value = string.Empty;

                        intStartRow++;
                    }
                }
                templateSheet = excel.Workbook.Worksheets["Sheet1"];
                excel.Workbook.Worksheets.Delete(templateSheet);
                //strDownloadUrl = baseUrl + strTemplatePath + strProductTemplateFile;
                string pathTemp = Path.GetTempFileName();
                excel.SaveAs(new FileInfo(pathTemp));

                strDownloadUrl = string.Format("LocalPath={0}&FileName={1}", pathTemp, "SparePartStockChecking.xlsx");
            }

            return strDownloadUrl;
        }
        #endregion

        #region Import Excel
        const string UploadDirectory = "~/Temp/";

        protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                string strExcelPath = string.Empty;
                if (e.UploadedFile.IsValid)
                {

                    //e.CallbackData = SavePostedFile(e.UploadedFile);
                    Guid guid = Guid.NewGuid();
                    e.CallbackData = String.Format("{0}_" + guid + ".xlsx", ConstantClass.SESSION_USER_ID);
                    string path = Server.MapPath("../Temp/") + e.CallbackData;
                    //string serverPath = e.CallbackData;
                    bool exists = System.IO.Directory.Exists(Server.MapPath("/Temp"));

                    if (!exists)
                        System.IO.Directory.CreateDirectory(Server.MapPath("/Temp"));

                    e.UploadedFile.SaveAs(path);


                    //strExcelPath = HttpContext.Current.Server.MapPath(path);
                    FileInfo file = new FileInfo(Path.Combine(path));

                    using (ExcelPackage package = new ExcelPackage(file))
                    {
                      

                        stockCheckingDetailList = new List<StockCheckingDetail>();
                        if (e.UploadedFile.FileName.ToLower().Contains("product"))
                        {
                            ExcelWorksheet workSheet = package.Workbook.Worksheets["Product"];
                            int totalRows = workSheet.Dimension.Rows;
                            for (var i = 2; i <= totalRows; i++)
                            {
                                stockCheckingDetailList.Add(new StockCheckingDetail()
                                {
                                    id = (stockCheckingDetailList.Count + 1) * -1,
                                    is_delete = false,
                                    product_id = workSheet.Cells[i, 2].Value == null ? 0 : Convert.ToInt32(workSheet.Cells[i, 2].Value),
                                    product_no = workSheet.Cells[i, 3].Value == null ? string.Empty : Convert.ToString(workSheet.Cells[i, 3].Value),
                                    product_name = workSheet.Cells[i, 4].Value == null ? string.Empty : Convert.ToString(workSheet.Cells[i, 4].Value),
                                    unit_code = workSheet.Cells[i, 5].Value == null ? string.Empty : Convert.ToString(workSheet.Cells[i, 5].Value),
                                    quantity = workSheet.Cells[i, 6].Value == null ? 0 : Convert.ToInt32(workSheet.Cells[i, 6].Value),
                                    quantity_reserve = workSheet.Cells[i, 7].Value == null ? 0 : Convert.ToInt32(workSheet.Cells[i, 7].Value),
                                    quantity_remain = workSheet.Cells[i, 8].Value == null ? 0 : Convert.ToInt32(workSheet.Cells[i, 8].Value),
                                    remark = workSheet.Cells[i, 9].Value == null ? string.Empty : Convert.ToString(workSheet.Cells[i, 9].Value),
                                });
                            }
                            e.CallbackData = "P";
                        }
                        else if (e.UploadedFile.FileName.ToLower().Contains("spare"))
                        {
                            ExcelWorksheet workSheet = package.Workbook.Worksheets["SparePart"];
                            int totalRows = workSheet.Dimension.Rows;
                            for (var i = 2; i <= totalRows; i++)
                            {

                                stockCheckingDetailList.Add(new StockCheckingDetail()
                                {
                                    id = (stockCheckingDetailList.Count + 1) * -1,
                                    is_delete = false,
                                    product_id = workSheet.Cells[i, 2].Value == null ? 0 : Convert.ToInt32(workSheet.Cells[i, 2].Value),
                                    product_no = workSheet.Cells[i, 3].Value == null ? string.Empty : Convert.ToString(workSheet.Cells[i, 3].Value),
                                    product_name = workSheet.Cells[i, 6].Value == null ? string.Empty : Convert.ToString(workSheet.Cells[i, 6].Value),
                                    unit_code = workSheet.Cells[i, 7].Value == null ? string.Empty : Convert.ToString(workSheet.Cells[i, 7].Value),
                                    quantity = workSheet.Cells[i, 8].Value == null ? 0 : Convert.ToInt32(workSheet.Cells[i, 8].Value),
                                    quantity_reserve = workSheet.Cells[i, 9].Value == null ? 0 : Convert.ToInt32(workSheet.Cells[i, 9].Value),
                                    quantity_remain = workSheet.Cells[i, 10].Value == null ? 0 : Convert.ToInt32(workSheet.Cells[i, 10].Value),
                                    remark = workSheet.Cells[i, 11].Value == null ? string.Empty : Convert.ToString(workSheet.Cells[i, 11].Value),
                                });
                            }
                            e.CallbackData = "S";
                        }
                        else
                        {
                            throw new Exception("ผิดพลาดในการ Import File");
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
    }
}