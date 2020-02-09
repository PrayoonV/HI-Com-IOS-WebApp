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
    public partial class AdjustProductSparePartList : MasterDetailPage
    {
        public override string PageName { get { return "Adjust Stock List"; } }
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
                // Get Permission and if no permission, will redirect to another page.
                if (!Permission.GetPermission())
                    Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

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
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID }
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
            //string returnData = "success";
            //var dsPurchaseRequest = new DataSet();
            //var purchaseRequestDetailList = new List<PurchaseRequestDetail>();
            //var prDetailMFGList = new List<PurchaseRequestDetailMFG>();
            //var customer_id = 0;
            //var supplier_id = 0;
            //using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            //{
            //    conn.Open();
            //    using (SqlTransaction tran = conn.BeginTransaction())
            //    {
            //        try
            //        {
            //            #region Load for Delete
            //            //Create array of Parameters
            //            var prNo = string.Empty;
            //            List<SqlParameter> arrParm = new List<SqlParameter>
            //            {
            //                new SqlParameter("@id", SqlDbType.Int) { Value = id },
            //            };

            //            dsPurchaseRequest = SqlHelper.ExecuteDataset(tran, "sp_purchase_request_list_data", arrParm.ToArray());


            //            if (dsPurchaseRequest.Tables.Count > 0)
            //            {
            //                var header = (from t in dsPurchaseRequest.Tables[0].AsEnumerable() select t).FirstOrDefault();
            //                if (header != null)
            //                {
            //                    prNo = Convert.IsDBNull(header["purchase_request_no"]) ? string.Empty : Convert.ToString(header["purchase_request_no"]);
            //                    customer_id = Convert.IsDBNull(header["customer_id"]) ? 0 : Convert.ToInt32(header["customer_id"]);
            //                    supplier_id = Convert.IsDBNull(header["supplier_id"]) ? 0 : Convert.ToInt32(header["supplier_id"]);
            //                }

            //                var detail = (from t in dsPurchaseRequest.Tables[1].AsEnumerable() select t).ToList();
            //                if (detail != null)
            //                {
            //                    purchaseRequestDetailList = new List<PurchaseRequestDetail>();
            //                    foreach (var row in detail)
            //                    {
            //                        purchaseRequestDetailList.Add(new PurchaseRequestDetail()
            //                        {
            //                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
            //                            is_deleted = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
            //                            item_description = Convert.IsDBNull(row["item_description"]) ? string.Empty : Convert.ToString(row["item_description"]),
            //                            item_id = Convert.IsDBNull(row["item_id"]) ? 0 : Convert.ToInt32(row["item_id"]),
            //                            item_no = Convert.IsDBNull(row["item_no"]) ? string.Empty : Convert.ToString(row["item_no"]),
            //                            item_type = Convert.IsDBNull(row["item_type"]) ? string.Empty : Convert.ToString(row["item_type"]),
            //                            item_unit = Convert.IsDBNull(row["item_unit"]) ? string.Empty : Convert.ToString(row["item_unit"]),
            //                            purchase_request_no = Convert.IsDBNull(row["purchase_request_no"]) ? string.Empty : Convert.ToString(row["purchase_request_no"]),
            //                            qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["qty"]),
            //                            old_qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["old_qty"]),
            //                            receive_by = Convert.IsDBNull(row["receive_by"]) ? 0 : Convert.ToInt32(row["receive_by"]),
            //                            receive_date = Convert.IsDBNull(row["receive_date"]) ? DateTime.Now : Convert.ToDateTime(row["receive_date"]),
            //                            receive_qty = Convert.IsDBNull(row["receive_qty"]) ? 0 : Convert.ToInt32(row["receive_qty"]),
            //                            display_receive_date = Convert.IsDBNull(row["receive_date"]) ? string.Empty : Convert.ToDateTime(row["receive_date"]).ToString("dd/MM/yyyy"),
            //                        });
            //                    }
            //                }
            //                var mfgDetail = (from t in dsPurchaseRequest.Tables[2].AsEnumerable() select t).ToList();
            //                if (mfgDetail != null)
            //                {
            //                    prDetailMFGList = new List<PurchaseRequestDetailMFG>();
            //                    foreach (var row in mfgDetail)
            //                    {
            //                        prDetailMFGList.Add(new PurchaseRequestDetailMFG()
            //                        {
            //                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
            //                            is_deleted = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
            //                            mfg_no = Convert.IsDBNull(row["mfg_no"]) ? string.Empty : Convert.ToString(row["mfg_no"]),
            //                            old_mfg_no = Convert.IsDBNull(row["old_mfg_no"]) ? string.Empty : Convert.ToString(row["old_mfg_no"]),
            //                            pr_detail_id = Convert.IsDBNull(row["pr_detail_id"]) ? 0 : Convert.ToInt32(row["pr_detail_id"]),
            //                            pr_no = Convert.IsDBNull(row["pr_no"]) ? string.Empty : Convert.ToString(row["pr_no"]),
            //                            item_id = Convert.IsDBNull(row["item_id"]) ? 0 : Convert.ToInt32(row["item_id"]),

            //                        });
            //                    }
            //                }
            //            }
            //            #endregion

            //            using (SqlCommand cmd = new SqlCommand("sp_purchase_request_header_delete", conn, tran))
            //            {
            //                cmd.CommandType = CommandType.StoredProcedure;
            //                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(id);
            //                cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


            //                cmd.ExecuteNonQuery();

            //            }

            //            #region Transection
            //            var transNo = string.Empty;
            //            using (SqlCommand cmd = new SqlCommand("sp_gen_transection_no", conn, tran))
            //            {
            //                cmd.CommandType = CommandType.StoredProcedure;

            //                transNo = Convert.ToString(cmd.ExecuteScalar());

            //            }
            //            string stock_type = string.Empty;

            //            foreach (var row in purchaseRequestDetailList)
            //            {

            //                using (SqlCommand cmd = new SqlCommand("sp_stock_movement_add", conn, tran))
            //                {
            //                    cmd.CommandType = CommandType.StoredProcedure;
            //                    cmd.Parameters.Add("@transection_no", SqlDbType.VarChar, 20).Value = transNo;

            //                    cmd.Parameters.Add("@product_type", SqlDbType.VarChar, 1).Value = row.item_type;
            //                    cmd.Parameters.Add("@product_id", SqlDbType.Int).Value = row.item_id;

            //                    cmd.Parameters.Add("@transection_type", SqlDbType.VarChar, 50).Value = "PR";
            //                    cmd.Parameters.Add("@movement_type", SqlDbType.VarChar, 3).Value = "OUT";
            //                    cmd.Parameters.Add("@checking_type", SqlDbType.NVarChar, 1).Value = "W";
            //                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty * -1;// คืนค่า
            //                    cmd.Parameters.Add("@stock_balance", SqlDbType.Int).Value = 0; // คำนวนจาก Store
            //                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = "คืนค่าจาก PR";//row.remark;
            //                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
            //                    cmd.Parameters.Add("@ref_doc_id", SqlDbType.Int).Value = id; // คำนวนจาก Store
            //                    cmd.Parameters.Add("@ref_doc_no", SqlDbType.VarChar, 20).Value = prNo;
            //                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = supplier_id;
            //                    cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customer_id;
            //                    cmd.ExecuteNonQuery();
            //                }
            //            }
            //            #endregion

            //            foreach (var row in prDetailMFGList)
            //            {

            //                using (SqlCommand cmd = new SqlCommand("sp_purchase_request_detail_mfg_delete", conn, tran))
            //                {
            //                    cmd.CommandType = CommandType.StoredProcedure;
            //                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
            //                    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
            //                    cmd.ExecuteNonQuery();
            //                }
            //                // if (status != "FL")
            //                // {
            //                using (SqlCommand cmd = new SqlCommand("sp_product_mfg_delete_by_mfg ", conn, tran))
            //                {
            //                    cmd.CommandType = CommandType.StoredProcedure;

            //                    cmd.Parameters.Add("@mfg_no", SqlDbType.VarChar, 20).Value = row.mfg_no;
            //                    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

            //                    cmd.ExecuteNonQuery();

            //                }

            //            }
            //        }

            //        catch (Exception ex)
            //        {
            //            tran.Rollback();
            //            tran.Dispose();
            //            conn.Close();
            //            //throw ex;
            //            return ex.Message.ToString();
            //        }
            //        finally
            //        {
            //            if (!conn.State.Equals(ConnectionState.Closed))
            //            {

            //                tran.Commit();
            //                tran.Dispose();
            //                conn.Close();
            //            }
            //        }

            //        return returnData;
            //    }
            //}
            return string.Empty;

        }
    }

}