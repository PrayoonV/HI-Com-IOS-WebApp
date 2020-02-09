using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Web;
using System.Drawing.Text;

namespace HicomIOS.Report
{
    public partial class Bill_Of_Loading : DevExpress.XtraReports.UI.XtraReport
    {
        public Bill_Of_Loading()
        {
            InitializeComponent();
            Bill_Of_Loading report = (Bill_Of_Loading)this;
            foreach (Band b in report.Bands)
            {
                foreach(XRControl con in b.Controls)
                {
                    con.Font = new Font(FontCollection.Families[0], con.Font.Size, con.Font.Style);
                }
            }
        }
        static PrivateFontCollection fontCollection;
        public static FontCollection FontCollection
        {
            get
            {
                if (fontCollection == null)
                {
                    fontCollection = new PrivateFontCollection();
                    fontCollection.AddFontFile(HttpContext.Current.Server.MapPath("~/Fonts/THSarabun.ttf"));
                }
                return fontCollection;
            }
        }



    }
}
