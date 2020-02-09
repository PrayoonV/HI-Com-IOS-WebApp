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
    public partial class SaleOrderList : MasterDetailPage
    {
        private DataSet dsResult;
        public class SaleOrderListHeaderList
        {
            public int id { get; set; }
            public string sale_order_no { get; set; }
            public string quotation_type { get; set; }
            public string sale_order_status { get; set; }
            public bool is_discount_by_item { get; set; }
            public string discount1_type { get; set; }
            public string discount2_type { get; set; }

        }
        public override string PageName { get { return "Sale Order List"; } }
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
        // ClientInstanceName that use in Script.js
        private string strClientInstanceName_gridView;
        private string strClientInstanceName_UserControlEditForm = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            // setting height gridView
            gridView.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen);

            //Bind Data into GridView
            if (!Page.IsPostBack)
            {
                // Get Permission and if no permission, will redirect to another page.
                if (!Permission.GetPermission())
                    Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

                BindGrid(true);
            }
            else
            {
                dsResult = (DataSet)Session["SESSION_SALE_ORDER_LIST"];
                BindGrid(false);
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
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID  },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = "" }
                        };

                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_sale_order_header_list", arrParm.ToArray());
                        conn.Close();
                        Session["SESSION_SALE_ORDER_LIST"] = dsResult;
                    }
                }

                //Bind data into GridView
                gridView.DataSource = dsResult;
                gridView.FilterExpression = FilterBag.GetExpression(false);
                gridView.DataBind();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }

        [WebMethod]
        public static string DeleteSaleOrder(string id)
        {
            string data = "success";
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {

                        using (SqlCommand cmd = new SqlCommand("sp_sale_order_delete", conn, tran))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(id);
                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


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
            return data;
        }

        [WebMethod]
        public static SaleOrderListHeaderList GeSaleOrderListData(string id)
        {
            var data = new SaleOrderListHeaderList();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },
                        };
                conn.Open();
                var dsQuataionData = SqlHelper.ExecuteDataset(conn, "sp_sale_order_list_data", arrParm.ToArray());
                conn.Close();

                if (dsQuataionData.Tables.Count > 0)
                {
                    if (dsQuataionData.Tables[0].Rows.Count > 0)
                    {
                        var row = (from t in dsQuataionData.Tables[0].AsEnumerable() select t).FirstOrDefault();
                        if (row != null)
                        {
                            data.id = Convert.ToInt32(id);
                            data.sale_order_no = Convert.IsDBNull(row["sale_order_no"]) ? string.Empty : Convert.ToString(row["sale_order_no"]);
                            data.quotation_type = Convert.IsDBNull(row["quotation_type"]) ? string.Empty : Convert.ToString(row["quotation_type"]);
                            data.is_discount_by_item = Convert.IsDBNull(row["is_discount_by_item"]) ? false : Convert.ToBoolean(row["is_discount_by_item"]);
                            data.discount1_type = Convert.IsDBNull(row["discount1_type"]) ? string.Empty : Convert.ToString(row["discount1_type"]);
                            data.discount2_type = Convert.IsDBNull(row["discount2_type"]) ? string.Empty : Convert.ToString(row["discount2_type"]);
                            data.sale_order_status = Convert.IsDBNull(row["sale_order_status"]) ? string.Empty : Convert.ToString(row["sale_order_status"]);

                        }
                    }
                }
            }

            return data;
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
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID  },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = e.Parameters.ToString() }
                        };

                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_sale_order_header_list", arrParm.ToArray());
                    conn.Close();
                    Session["SESSION_SALE_ORDER_LIST"] = dsResult;
                }

                //Bind data into GridView
                gridView.DataSource = dsResult;
                gridView.FilterExpression = FilterBag.GetExpression(false);
                gridView.DataBind();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
    }
}