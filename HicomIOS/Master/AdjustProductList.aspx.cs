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
    public partial class AdjustProductList : MasterDetailPage
    {
        public override string PageName { get { return "Adjust Product List"; } }
        DataSet dsResult = new DataSet();
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }
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
        protected void Page_Load(object sender, EventArgs e)
        {
            gridView.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen);

            //Bind Data into GridView
            if (!Page.IsPostBack)
            {
                BindGrid(true);
            }
            else
            {
                dsResult = (DataSet)Session["SESSION_ADJUST_ITEM_LIST"];
                BindGrid(false);
            }
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
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID },
                            new SqlParameter("@service_type", SqlDbType.VarChar, 100) { Value = ConstantClass.SESSION_DEPARTMENT_SERVICE_TYPE },
                        };

                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_adjust_item_header_list", arrParm.ToArray());
                        conn.Close();
                        Session["SESSION_ADJUST_ITEM_LIST"] = dsResult;
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
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID}
                        };

                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_adjust_item_header_list", arrParm.ToArray());
                    conn.Close();
                    Session["SESSION_ADJUST_ITEM_LIST"] = dsResult;
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
        public static string DeleteItem(string id)
        {
            string returnData = "success";

            var dsResult = new DataSet();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID },
                        };
                dsResult = SqlHelper.ExecuteDataset(conn, "sp_adjust_item_list_data", arrParm.ToArray());
                conn.Close();
            }

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                conn.Open();

                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        if (dsResult.Tables.Count > 0)
                        {
                            var header = dsResult.Tables[0].AsEnumerable().FirstOrDefault();
                            var adjust_no = Convert.IsDBNull(header["adjust_no"]) ? string.Empty : Convert.ToString(header["adjust_no"]);
                            var status = Convert.IsDBNull(header["doc_status"]) ? string.Empty : Convert.ToString(header["doc_status"]);
                            var customer_id = Convert.IsDBNull(header["customer_id"]) ? string.Empty : Convert.ToString(header["customer_id"]);

                            var detail = dsResult.Tables[1].AsEnumerable().ToList();
                            var adjustDetailList = new List<AdjustProduct.AdjustProductDetail>();
                            if (detail != null)
                            {
                                foreach (var row in detail)
                                {
                                    var obj = new AdjustProduct.AdjustProductDetail()
                                    {
                                        adjust_type = Convert.IsDBNull(row["adjust_type"]) ? string.Empty : Convert.ToString(row["adjust_type"]),
                                        id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                        is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                        item_name = Convert.IsDBNull(row["item_name"]) ? string.Empty : Convert.ToString(row["item_name"]),
                                        item_id = Convert.IsDBNull(row["item_id"]) ? 0 : Convert.ToInt32(row["item_id"]),
                                        item_no = Convert.IsDBNull(row["item_no"]) ? string.Empty : Convert.ToString(row["item_no"]),
                                        item_type = Convert.IsDBNull(row["item_type"]) ? string.Empty : Convert.ToString(row["item_type"]),
                                        quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]),
                                        max_quantity = Convert.IsDBNull(row["quantity"]) ? 0 : Convert.ToInt32(row["quantity"]),
                                        remark = Convert.IsDBNull(row["remark"]) ? string.Empty : Convert.ToString(row["remark"]),
                                        sort_no = Convert.IsDBNull(row["sort_no"]) ? 0 : Convert.ToInt32(row["sort_no"]),
                                        unit_code = Convert.IsDBNull(row["unit_code"]) ? string.Empty : Convert.ToString(row["unit_code"])
                                    };
                                    obj.quantity_type = (obj.quantity >= 0 ? "0" : "1");
                                    obj.quantity = Math.Abs(obj.quantity);

                                    adjustDetailList.Add(obj);
                                }
                            }

                            #region MFG Detail
                            var dataDetailMFG = dsResult.Tables[2].AsEnumerable().ToList();
                            var issueDetailMFGList = new List<Issue.IssueDetailMFG>();
                            if (dataDetailMFG != null)
                            {
                                foreach (var row in dataDetailMFG)
                                {
                                    issueDetailMFGList.Add(new Issue.IssueDetailMFG()
                                    {
                                        id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                        issue_stock_no = Convert.IsDBNull(row["adjust_no"]) ? string.Empty : Convert.ToString(row["adjust_no"]),
                                        product_id = Convert.IsDBNull(row["item_id"]) ? 0 : Convert.ToInt32(row["item_id"]),
                                        issue_stock_detail_id = Convert.IsDBNull(row["adjust_detail_id"]) ? 0 : Convert.ToInt32(row["adjust_detail_id"]),
                                        mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
                                    });
                                }
                            }
                            #endregion

                            using (SqlCommand cmd = new SqlCommand("sp_adjust_item_header_delete", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                                cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                cmd.ExecuteNonQuery();
                            }

                            if (status == "CF")
                            {
                                var transNo = string.Empty;
                                using (SqlCommand cmd = new SqlCommand("sp_gen_transection_no", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    transNo = Convert.ToString(cmd.ExecuteScalar());
                                }
                                foreach (var row in adjustDetailList)//.Where(t => t.receive_qty_balance != 0))
                                {
                                    if (!row.is_delete)
                                    {
                                        if (row.item_type == "P")
                                        {
                                            var mfgList = (from t in issueDetailMFGList where t.product_id == row.item_id && !t.is_deleted select t).ToList();
                                            foreach (var mfgRow in mfgList)
                                            {
                                                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "ADJ";
                                                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = (row.quantity_type == "0" ? "OUT" : "IN");   //  Swap IN, OUT
                                                    cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 1;
                                                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Return adjust product MFG No : " + mfgRow.mfg_no;
                                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                    cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = id; // คำนวนจาก Store
                                                    cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = adjust_no;// txtPRNo.Value;
                                                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customer_id;
                                                    cmd.ExecuteNonQuery();
                                                }

                                                if (row.quantity_type == "0")
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("sp_product_mfg_delete_by_mfg ", conn, tran))
                                                    {
                                                        cmd.CommandType = CommandType.StoredProcedure;

                                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = mfgRow.mfg_no;
                                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                                        cmd.ExecuteNonQuery();

                                                    }
                                                }
                                                else
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("sp_product_mfg_edit_by_mfg", conn, tran))
                                                    {
                                                        cmd.CommandType = CommandType.StoredProcedure;

                                                        cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = mfgRow.mfg_no;
                                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                                        cmd.ExecuteNonQuery();

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

                                                cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
                                                cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

                                                cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "ADJ";
                                                cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = (row.quantity_type == "0" ? "OUT" : "IN");   //  Swap IN, OUT
                                                cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
                                                cmd.Parameters.Add("@qty", SqlDbType.Int).Value = Math.Abs(row.quantity);
                                                cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
                                                cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "Return adjust product";
                                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = id; // คำนวนจาก Store
                                                cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = adjust_no;// txtPRNo.Value;
                                                cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = 0;
                                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customer_id;
                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        tran.Dispose();
                        conn.Close();

                        returnData = ex.Message;
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

            return returnData;

        }
    }
}