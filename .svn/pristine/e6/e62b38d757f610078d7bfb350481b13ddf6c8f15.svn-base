using DevExpress.Web;
using HicomIOS.ClassUtil;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HicomIOS.Master
{
    public partial class StockCheckingList : MasterDetailPage
    {
        private DataSet dsResult = new DataSet();
        public override string PageName { get { return "Stock Checking List"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }
        public override void OnFilterChanged()
        {

        }

        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            return result.OfType<FilterControlColumn>();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                // Get Permission and if no permission, will redirect to another page.
                if (!Permission.GetPermission())
                    Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

                PrepareData();
            }
            else {
                dsResult = (DataSet)ViewState["VIEWSTATE_STOCKCHECKING_LIST"];
                BindDataGrid();
            }
        }
        protected void PrepareData()
        {

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                conn.Open();
                dsResult = SqlHelper.ExecuteDataset(conn, "sp_stock_movement_list");
                conn.Close();
                ViewState["VIEWSTATE_STOCKCHECKING_LIST"] = dsResult;
            }
            BindDataGrid();
        }
        private void BindDataGrid()
        {
            gridView.DataSource = dsResult;
            gridView.DataBind();
        }

        protected void gridView_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if ((e.Column.FieldName == "count_items" || e.Column.FieldName == "count_correct"
                    || e.Column.FieldName == "count_stock_in" || e.Column.FieldName == "count_stock_out") 
                    && (int)e.Value == 0)
                e.DisplayText = "-";
        }

        protected void gridView_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            ASPxGridView grid = (sender as ASPxGridView);
            if (e.RowType == GridViewRowType.Data)
            {
                if ((int)e.GetValue("count_stock_out") == (int)e.GetValue("count_items"))
                    e.Row.BackColor = Color.FromArgb(255, 193, 7); //#ffc107
                if ((int)e.GetValue("count_correct") == (int)e.GetValue("count_items"))
                    e.Row.BackColor = System.Drawing.Color.LightGreen;
            }
        }
    }
}