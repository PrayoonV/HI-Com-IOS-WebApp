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
    public partial class ProductCategory : MasterDetailPage
    {

        public int parentId = 0;
        private DataSet dsResult;
        public override string PageName { get { return "ProductCategory"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }
        public class ProductCategoryDetail
        {
            public int id { get; set; }
            public string cat_type { get; set; }
            public string cat_name_tha { get; set; }
            public string cat_name_eng { get; set; }
            public int parent_id { get; set; }
            public string parent_name_eng { get; set; }
            public string parent_name_tha { get; set; }
            public int level_no { get; set; }
            public bool is_enable { get; set; }
            public bool is_delete { get; set; }
            public int created_by { get; set; }
            public int parent_root_id { get; set; }

            public DateTime created_date { get; set; }
            public int updated_by { get; set; }
            public DateTime updated_date { get; set; }
        }

        List<ProductCategoryDetail> productCatDetail
        {
            get
            {
                if (Session["SESSION_PRODUCT_CATEGORY_LIST"] == null)
                    Session["SESSION_PRODUCT_CATEGORY_LIST"] = new List<ProductCategoryDetail>();
                return (List<ProductCategoryDetail>)Session["SESSION_PRODUCT_CATEGORY_LIST"];
            }
            set
            {
                Session["SESSION_PRODUCT_CATEGORY_LIST"] = value;
            }
        }

        protected void PrepareData()
        {
            try
            {
                // Setting height gridView
                gridView.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen);
                gridView.SettingsBehavior.AllowFocusedRow = true;

                if (parentId == 0)
                {
                    group_breadcrumb.Visible = false;
                    var dtSource = new DataTable();
                    dtSource.Columns.Add("data_value", typeof(string));
                    dtSource.Columns.Add("data_text", typeof(string));

                    dtSource.Rows.Add("PP", "Product");
                    dtSource.Rows.Add("SP", "Spare Part");
                    cboCategoryType.DataSource = dtSource;
                    cboCategoryType.DataBind();

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!BasePage.SESSION_PERMISSION_SCREEN.is_create)
            {
                add_NewItem.Visible = false;
            }

           
            parentId = Convert.ToInt32(Request.QueryString["parentId"]);
            if (!Page.IsPostBack)
            {
                // Get Permission and if no permission, will redirect to another page.
                if (!Permission.GetPermission())
                    Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

                ClearWoringSession();
                PrepareData();
                BindGrid(true);

            }
            else
            {
                //dsResult = (DataSet)ViewState["dsResult"];
                dsResult = (DataSet)Session["SESSION_PRODUCT_CATEGORY_MASTER"];
                BindGrid(false);
            }
        }
        //override dataset partial  
        public override void OnFilterChanged()
        {
            // BindGrid();
        }
        protected void ClearWoringSession()
        {
            Session.Remove("SESSION_PRODUCT_CATEGORY_LIST");
        }

        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            return result.OfType<FilterControlColumn>();
        }


        protected void BindGrid(bool isForceRefreshData = false)

        {
            try
            {
                if (!Page.IsPostBack || isForceRefreshData)
                {
                    string search = " ";
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
                            new SqlParameter("@search_name",SqlDbType.VarChar) {Value = search },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@parent_id", SqlDbType.Int) { Value = parentId },
                            new SqlParameter("@level_no", SqlDbType.Int) { Value =  parentId > 0 ? 1 : 0 }

                        };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_category_list", arrParm.ToArray());
                        Session["SESSION_PRODUCT_CATEGORY_MASTER"] = dsResult;
                        if (dsResult != null)
                        {
                            /*int rowId = 0;
                            if (Session["ROW_ID"] != null)
                            {
                                rowId = Convert.ToInt32(Session["ROW_ID"].ToString());
                            }*/

                            var row = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                            foreach (var data in row)
                            {
                                productCatDetail.Add(new ProductCategoryDetail()
                                {
                                    id = Convert.ToInt32(data["id"]),
                                    cat_type = Convert.IsDBNull(data["cat_type"]) ? null : Convert.ToString(data["cat_type"]),
                                    cat_name_tha = Convert.IsDBNull(data["cat_name_tha"]) ? null : Convert.ToString(data["cat_name_tha"]),
                                    cat_name_eng = Convert.IsDBNull(data["cat_name_eng"]) ? null : Convert.ToString(data["cat_name_eng"]),
                                    parent_name_eng = Convert.IsDBNull(data["parent_name_eng"]) ? null : Convert.ToString(data["parent_name_eng"]),
                                    parent_name_tha = Convert.IsDBNull(data["parent_name_tha"]) ? null : Convert.ToString(data["parent_name_tha"]),
                                    parent_root_id = Convert.IsDBNull(data["parent_root_id"]) ? 0 : Convert.ToInt32(data["parent_root_id"]),
                                    parent_id = Convert.IsDBNull(data["parent_id"]) ? 0 : Convert.ToInt32(data["parent_id"]),
                                    level_no = Convert.IsDBNull(data["level_no"]) ? 0 : Convert.ToInt32(data["level_no"]),
                                    is_enable = Convert.IsDBNull(data["is_enable"]) ? false : Convert.ToBoolean(data["is_enable"]),

                                });

                                /*if (rowId == productCatDetail[productCatDetail.Count - 1].id)
                                {
                                    int selectedRow = productCatDetail.Count - 1;
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
                                }*/
                            }
                        }
                        if (parentId > 0)
                        {
                            var returnData = new { parent_root_id = 0, parent_name_tha = string.Empty };

                            var dataProductCate = productCatDetail;
                            if (dataProductCate != null)
                            {
                                returnData = (from t in dataProductCate
                                              where t.parent_id == Convert.ToInt32(parentId)
                                              select new
                                              {

                                                  parent_root_id = t.parent_root_id,
                                                  parent_name_tha = t.parent_name_tha
                                              }).FirstOrDefault();

                            }
                            if (returnData != null)
                            {
                                var parent_root_id = returnData.parent_root_id;
                                title.InnerText = returnData.parent_name_tha;
                                btn_backPage.Attributes.Add("onclick", "btnback(" + parent_root_id + ")");
                            }
                            else
                            {

                            }

                            group_breadcrumb.Visible = true;
                            add_NewItem.Visible = false;
                        }

                        HttpContext.Current.Session["SESSION_PRODUCT_CATEGORY_LIST"] = productCatDetail;
                    }
                }

                //Bind data into GridView
                gridView.DataSource = dsResult;
                //gridView.FilterExpression = FilterBag.GetExpression(false);
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
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }

        [WebMethod]
        public static int DeleteProductCategory(string id)
        {
            var parent_id = 0;
            var dataProductCate = (List<ProductCategoryDetail>)HttpContext.Current.Session["SESSION_PRODUCT_CATEGORY_LIST"];
            if (dataProductCate != null)
            {
                parent_id = (from t in dataProductCate where t.id == Convert.ToInt32(id) select t.parent_id).FirstOrDefault();
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_product_category_delete", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(id);
                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
            }
            return parent_id;
        }

        [WebMethod]
        public static string AddProductCategory(string cboCategoryType, string category_name_tha, string category_name_eng)
        {
            string data = "success";
            int newID = DataListUtil.emptyEntryID;
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("sp_product_category_add", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@cat_type", SqlDbType.VarChar, 2).Value = cboCategoryType;
                    cmd.Parameters.Add("@cat_name_tha", SqlDbType.VarChar, 100).Value = category_name_tha;
                    cmd.Parameters.Add("@cat_name_eng", SqlDbType.VarChar, 100).Value = category_name_eng;
                    cmd.Parameters.Add("@parent_id", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@level_no", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                    conn.Open();
                    newID = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();
                }

                //  Set new id
                HttpContext.Current.Session["ROW_ID"] = Convert.ToString(newID);
            }
            return data;
        }

        [WebMethod]
        public static List<string> FindChild(string id)
        {
            var data = new List<string>();
            var dsResult = (DataSet)HttpContext.Current.Session["SESSION_PRODUCT_CATEGORY_LIST"];
            var dsReturn = new DataSet();
            if (dsResult != null)
            {
                var row = (from t in dsResult.Tables[0].AsEnumerable()
                           where t.Field<int>("parent_id") == Convert.ToInt32(id)
                           select t).ToList();
                if (row != null)
                {
                    data.Add(Convert.ToString(row[0]["parent_name_tha"]));
                    data.Add(Convert.ToString(row[0]["level_no"]));
                }

            }


            return data;
        }

        protected void gridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //string param = e.Parameters.ToString();

            //gridView.DataSource = productCatDetail;
            //gridView.DataBind();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    Session["SEARCH"] = e.Parameters.ToString();

                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name",SqlDbType.VarChar,200) {Value = e.Parameters.ToString() },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@parent_id", SqlDbType.Int) { Value = parentId },
                            new SqlParameter("@level_no", SqlDbType.Int) { Value =  parentId > 0 ? 1 : 0 }

                        };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_category_list", arrParm.ToArray());
                    Session["SESSION_PRODUCT_CATEGORY_MASTER"] = dsResult;
                    if (dsResult != null)
                    {
                        var row = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                        foreach (var data in row)
                        {
                            productCatDetail.Add(new ProductCategoryDetail()
                            {
                                id = Convert.ToInt32(data["id"]),
                                cat_type = Convert.IsDBNull(data["cat_type"]) ? null : Convert.ToString(data["cat_type"]),
                                cat_name_tha = Convert.IsDBNull(data["cat_name_tha"]) ? null : Convert.ToString(data["cat_name_tha"]),
                                cat_name_eng = Convert.IsDBNull(data["cat_name_eng"]) ? null : Convert.ToString(data["cat_name_eng"]),
                                parent_name_eng = Convert.IsDBNull(data["parent_name_eng"]) ? null : Convert.ToString(data["parent_name_eng"]),
                                parent_name_tha = Convert.IsDBNull(data["parent_name_tha"]) ? null : Convert.ToString(data["parent_name_tha"]),
                                parent_root_id = Convert.IsDBNull(data["parent_root_id"]) ? 0 : Convert.ToInt32(data["parent_root_id"]),
                                parent_id = Convert.IsDBNull(data["parent_id"]) ? 0 : Convert.ToInt32(data["parent_id"]),
                                level_no = Convert.IsDBNull(data["level_no"]) ? 0 : Convert.ToInt32(data["level_no"]),
                                is_enable = Convert.IsDBNull(data["is_enable"]) ? false : Convert.ToBoolean(data["is_enable"]),

                            });
                        }
                    }
                    if (parentId > 0)
                    {
                        var returnData = new { parent_root_id = 0, parent_name_tha = string.Empty };

                        var dataProductCate = productCatDetail;
                        if (dataProductCate != null)
                        {
                            returnData = (from t in dataProductCate
                                          where t.parent_id == Convert.ToInt32(parentId)
                                          select new
                                          {

                                              parent_root_id = t.parent_root_id,
                                              parent_name_tha = t.parent_name_tha
                                          }).FirstOrDefault();

                        }
                        if (returnData != null)
                        {
                            var parent_root_id = returnData.parent_root_id;
                            title.InnerText = returnData.parent_name_tha;
                            btn_backPage.Attributes.Add("onclick", "btnback(" + parent_root_id + ")");
                        }
                        else
                        {

                        }

                        group_breadcrumb.Visible = true;
                        add_NewItem.Visible = false;
                    }

                    HttpContext.Current.Session["SESSION_PRODUCT_CATEGORY_LIST"] = productCatDetail;
                }


                //Bind data into GridView
                gridView.DataSource = dsResult;
                //gridView.FilterExpression = FilterBag.GetExpression(false);
                gridView.DataBind();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }

        [WebMethod]
        public static ProductCategoryDetail addItemChild(string id)
        {
            var Data = new { level_no = 0, parent_id = 0 };

            var dataProductCate = (List<ProductCategoryDetail>)HttpContext.Current.Session["SESSION_PRODUCT_CATEGORY_LIST"];


            if (dataProductCate != null)
            {
                Data = (from t in dataProductCate
                        where t.id == Convert.ToInt32(id)
                        select new
                        {

                            level_no = t.level_no,
                            parent_id = t.parent_id
                        }).FirstOrDefault();

            }
            //หาlevaelfrom SESSION_PRODUCT_CATEGORY_LIST

            var returnData = new ProductCategoryDetail();//ตั้งตัวแปรชนิดเดียวกับคลาสที่สร้าง ตามฟิวในตาราง
            var dsProductCategory = new DataSet(); //ตั้งตัวแปรเพื่อใช่เก็บข้อมูลจากtabel
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm = new List<SqlParameter>
                {
                    new SqlParameter("@search_name",SqlDbType.VarChar) {Value = " " },
                    new SqlParameter("@id", SqlDbType.Int) { Value = id },
                    new SqlParameter("@parent_id", SqlDbType.Int) { Value = Data.parent_id },
                    new SqlParameter("@level_no", SqlDbType.Int) { Value = Data.level_no}
                };
                    conn.Open();
                    dsProductCategory = SqlHelper.ExecuteDataset(conn, "sp_product_category_list", arrParm.ToArray());
                    conn.Close();

                }
                if (dsProductCategory.Tables.Count > 0) //เช็คว่าdata null?
                {
                    var row = dsProductCategory.Tables[0].Rows[0];//สร้างตัวแปรมาโดยเรียกระบุจากtabelแรก,colแรก ชี้ตำแหน่งลงไป
                    returnData.id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]);//returnData.id = ตัวแปรที่สร้างชี้ไปที่propetieที่สร้างตรงกับฟิวในตาราง, เอาข้อมูลจากtabelโดยเช็คค่าว่างของฟิว?ถ้าไม่ก็คอนเวิดฟิวเป็นint
                    returnData.cat_type = Convert.IsDBNull(row["cat_type"]) ? null : Convert.ToString(row["cat_type"]);
                    returnData.parent_id = Convert.IsDBNull(row["parent_id"]) ? 0 : Convert.ToInt32(row["parent_id"]);
                    returnData.parent_root_id = Convert.IsDBNull(row["parent_root_id"]) ? 0 : Convert.ToInt32(row["parent_root_id"]);
                    returnData.cat_name_tha = Convert.IsDBNull(row["cat_name_tha"]) ? null : Convert.ToString(row["cat_name_tha"]);
                    returnData.cat_name_eng = Convert.IsDBNull(row["cat_name_eng"]) ? null : Convert.ToString(row["cat_name_eng"]);
                    returnData.level_no = Convert.IsDBNull(row["level_no"]) ? 0 : Convert.ToInt32(row["level_no"]);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnData;
        }

        [WebMethod]
        public static int ConfirmAddParent(ProductCategoryDetail[] cateProductLevel1)
        {
            int newID = DataListUtil.emptyEntryID;//idเปล่า
            var parent_id = 0;
            var row = (from t in cateProductLevel1 select t).FirstOrDefault();
            if (row != null)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_product_category_add", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@cat_name_tha", SqlDbType.VarChar, 100).Value = row.cat_name_tha;
                            cmd.Parameters.Add("@cat_name_eng", SqlDbType.VarChar, 100).Value = row.cat_name_eng;
                            cmd.Parameters.Add("@cat_type", SqlDbType.VarChar, 2).Value = row.cat_type;
                            cmd.Parameters.Add("@parent_id", SqlDbType.Int).Value = row.parent_id;
                            cmd.Parameters.Add("@level_no", SqlDbType.Int).Value = row.level_no + 1;
                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                            conn.Open();
                            newID = Convert.ToInt32(cmd.ExecuteScalar());
                            conn.Close();
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                parent_id = row.parent_id;

            }

            return parent_id;
        }

        [WebMethod]
        public static int ProductCategoryView(string id)
        {
            var returnData = 0;
            var level = 0;

            var dataProductCate = (List<ProductCategoryDetail>)HttpContext.Current.Session["SESSION_PRODUCT_CATEGORY_LIST"];
            if (dataProductCate != null)
            {
                level = (from t in dataProductCate where t.id == Convert.ToInt32(id) select t.level_no).FirstOrDefault();

            }

            var productCatDetail = new List<ProductCategoryDetail>();
            var dsResult = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name",SqlDbType.VarChar) {Value = " " },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@parent_id", SqlDbType.Int) { Value = id },
                            new SqlParameter("@level_no", SqlDbType.Int) { Value = level+1 }
                        };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_product_category_list", arrParm.ToArray());

                    if (dsResult != null)
                    {
                        var row = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                        foreach (var data in row)
                        {
                            productCatDetail.Add(new ProductCategoryDetail()
                            {
                                id = Convert.ToInt32(data["id"]),
                                cat_type = Convert.IsDBNull(data["cat_type"]) ? null : Convert.ToString(data["cat_type"]),
                                cat_name_tha = Convert.IsDBNull(data["cat_name_tha"]) ? null : Convert.ToString(data["cat_name_tha"]),
                                cat_name_eng = Convert.IsDBNull(data["cat_name_eng"]) ? null : Convert.ToString(data["cat_name_eng"]),
                                parent_root_id = Convert.IsDBNull(data["parent_root_id"]) ? 0 : Convert.ToInt32(data["parent_root_id"]),
                                parent_id = Convert.IsDBNull(data["parent_id"]) ? 0 : Convert.ToInt32(data["parent_id"]),
                                level_no = Convert.IsDBNull(data["level_no"]) ? 0 : Convert.ToInt32(data["level_no"]),

                            });
                        }
                    }

                    HttpContext.Current.Session["SESSION_PRODUCT_CATEGORY_LIST"] = productCatDetail;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            returnData = productCatDetail.Count;
            return returnData;
        }

        [WebMethod]
        public static ProductCategoryDetail ProductCategoryEdit(string id, int index)
        {
            //  Set active row
            HttpContext.Current.Session["ROW_ID"] = id;
            HttpContext.Current.Session["ROW"] = index;

            var level = 0;
            var dataProductCate = (List<ProductCategoryDetail>)HttpContext.Current.Session["SESSION_PRODUCT_CATEGORY_LIST"];
            if (dataProductCate != null)
            {
                level = (from t in dataProductCate where t.id == Convert.ToInt32(id) select t.level_no).FirstOrDefault();
            }
            var callDataProductCate = new ProductCategoryDetail();
            var dsDataProductCat = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm = new List<SqlParameter>
                    {
                        new SqlParameter("@search_name",SqlDbType.VarChar,200) {Value = "" },
                        new SqlParameter("@id", SqlDbType.Int) { Value = id },
                        new SqlParameter("@parent_id", SqlDbType.Int) { Value = 0 },
                        new SqlParameter("@level_no", SqlDbType.Int) { Value = level}
                };
                    conn.Open();
                    dsDataProductCat = SqlHelper.ExecuteDataset(conn, "sp_product_category_list", arrParm.ToArray());
                    conn.Close();

                }
                if (dsDataProductCat.Tables.Count > 0)
                {
                    var row = dsDataProductCat.Tables[0].Rows[0];
                    callDataProductCate.id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]);//returnData.id = ตัวแปรที่สร้างชี้ไปที่propetieที่สร้างตรงกับฟิวในตาราง, เอาข้อมูลจากtabelโดยเช็คค่าว่างของฟิว?ถ้าไม่ก็คอนเวิดฟิวเป็นint
                    callDataProductCate.cat_type = Convert.IsDBNull(row["cat_type"]) ? null : Convert.ToString(row["cat_type"]);
                    callDataProductCate.parent_id = Convert.IsDBNull(row["parent_id"]) ? 0 : Convert.ToInt32(row["parent_id"]);
                    callDataProductCate.cat_name_tha = Convert.IsDBNull(row["cat_name_tha"]) ? null : Convert.ToString(row["cat_name_tha"]);
                    callDataProductCate.cat_name_eng = Convert.IsDBNull(row["cat_name_eng"]) ? null : Convert.ToString(row["cat_name_eng"]);
                    callDataProductCate.parent_root_id = Convert.IsDBNull(row["parent_root_id"]) ? 0 : Convert.ToInt32(row["parent_root_id"]);
                    callDataProductCate.level_no = Convert.IsDBNull(row["level_no"]) ? 0 : Convert.ToInt32(row["level_no"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return callDataProductCate;
        }

        [WebMethod]
        public static int ProductCategoryConfirmEdit(ProductCategoryDetail[] dataProductCategory)
        {
            var dataSuccess = 0;
            var dataRow = (from t in dataProductCategory select t).FirstOrDefault();
            if (dataRow != null)
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_product_category_edit", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = dataRow.id;
                        cmd.Parameters.Add("@cat_name_tha", SqlDbType.VarChar, 100).Value = dataRow.cat_name_tha;
                        cmd.Parameters.Add("@cat_name_eng", SqlDbType.VarChar, 100).Value = dataRow.cat_name_eng;
                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                dataSuccess = dataRow.parent_id;

                //  Set new id
                HttpContext.Current.Session["ROW_ID"] = Convert.ToString(dataRow.id);
            }

            return dataSuccess;
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