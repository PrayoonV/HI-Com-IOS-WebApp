using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HicomIOS.Master
{
    public partial class StockService : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckStockPending();
        }

        private void CheckStockPending()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                        };
                    conn.Open();
                    var dsDatastockCard = SqlHelper.ExecuteDataset(conn, "sp_check_stock_pending", arrParm.ToArray());
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            DateTime today = DateTime.Now;
            lblStatus.Text = "Checking stock done : " + today.ToString("dd/MM/yyyy HH:mm");
        }

    }
}