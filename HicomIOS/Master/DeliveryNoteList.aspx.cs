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

namespace HicomIOS
{
    public partial class DeliveryNoteList : MasterDetailPage
    {
        private DataSet dsResult;
        public class DeliveryNoteHeaderList
        {
            public string delivery_no { get; set; }
            public string delivery_status { get; set; }
        }
        public override string PageName { get { return "Delivery Note List"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
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
                    dsResult = (DataSet)Session["SESSION_DELIVERY_NOTE_LIST"];
                    BindGrid(false);
                }
            }
            catch(Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('"+ex.Message+"','E')", true);
            }
        }

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
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = "" },
                        };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_delivery_note_header_list",arrParm.ToArray());
                        conn.Close();
                        Session["SESSION_DELIVERY_NOTE_LIST"] = dsResult;
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
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('" + ex.Message + "','E')", true);
            }
        }

        
        [WebMethod]
        public static string DeleteDeliveryNote(string id)
        {
            string data = "success";

            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_delivery_note_header_delete", conn))
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
            return data;
        }

        [WebMethod]
        public static DeliveryNoteHeaderList GetDeliveryNoteData(string id)
        {
            var data = new DeliveryNoteHeaderList();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value =Convert.ToInt32(id) },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = "" }
                        };
                conn.Open();
                var dsDeliveryNoteData = SqlHelper.ExecuteDataset(conn, "sp_delivery_note_header_list", arrParm.ToArray());
                conn.Close();

                if (dsDeliveryNoteData.Tables.Count > 0)
                {
                    if (dsDeliveryNoteData.Tables[0].Rows.Count > 0)
                    {
                        var row = (from t in dsDeliveryNoteData.Tables[0].AsEnumerable() select t).FirstOrDefault();
                        if (row != null)
                        {
                            
                            data.delivery_no = Convert.IsDBNull(row["delivery_no"]) ? string.Empty : Convert.ToString(row["delivery_no"]);
                            data.delivery_status = Convert.IsDBNull(row["delivery_status"]) ? string.Empty : Convert.ToString(row["delivery_status"]);
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
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = e.Parameters.ToString() }
                        };
                    conn.Open();
                    dsResult = SqlHelper.ExecuteDataset(conn, "sp_delivery_note_header_list", arrParm.ToArray());
                    conn.Close();
                    Session["SESSION_DELIVERY_NOTE_LIST"] = dsResult;
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