using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Drawing.Text;
using System.Web;

namespace HicomIOS.Report
{
    public partial class Sale_Order_Product : DevExpress.XtraReports.UI.XtraReport
    {
        public Sale_Order_Product()
        {
            InitializeComponent();
            
        }
        
        

        private void xrTableCell8_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void xrLabel30_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void xrLabel17_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string value = "";
            if (Convert.ToBoolean(this.DetailReport.GetCurrentColumnValue("is_inv_normal")))
            {
                value += "ปกติ";
            }
            if (Convert.ToBoolean(this.DetailReport.GetCurrentColumnValue("is_inv_customer_receive")))
            {
                if (!value.Equals(""))
                {
                    value += " | ";
                }
                value += "ลูกค้ามารับ";
            }
            if (Convert.ToBoolean(this.DetailReport.GetCurrentColumnValue("is_inv_thaipost")))
            {
                if (!value.Equals(""))
                {
                    value += " | ";
                }
                value += "ส่งไปรษณีย์ ATTN ";
                value += Convert.ToString(this.DetailReport.GetCurrentColumnValue("thaipost_attn"));
            }
            if (Convert.ToBoolean(this.DetailReport.GetCurrentColumnValue("is_inv_other")))
            {
                if (!value.Equals(""))
                {
                    value += " | ";
                }
                value += Convert.ToString(this.DetailReport.GetCurrentColumnValue("other_description"));
            }
            xrLabel17.Text = value;
        }
    }
}
