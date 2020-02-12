namespace HicomIOS.Report
{
    partial class Quotation_Approve_Qu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Quotation_Approve_Qu));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
            this.formattingRule3 = new DevExpress.XtraReports.UI.FormattingRule();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.formattingRule2 = new DevExpress.XtraReports.UI.FormattingRule();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox3 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaption3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData3_Odd = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaptionBackground3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.parameterApprove_Qu = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox2,
            this.xrPictureBox1,
            this.xrLine4,
            this.xrLine3,
            this.xrLine2,
            this.xrLabel3,
            this.xrLabel4,
            this.xrLabel5,
            this.xrLabel2,
            this.xrLabel12,
            this.xrLabel8,
            this.xrLabel9,
            this.xrLabel11,
            this.xrLabel7,
            this.xrPictureBox3});
            this.Detail.HeightF = 187.5262F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("ImageUrl", null, "sp_report_approve_list.sign_2")});
            this.xrPictureBox2.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter;
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(217.7874F, 0F);
            this.xrPictureBox2.LockedInUserDesigner = true;
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(210.3085F, 64.58334F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("ImageUrl", null, "sp_report_approve_list.sign_1")});
            this.xrPictureBox1.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter;
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPictureBox1.LockedInUserDesigner = true;
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(210.3085F, 64.58334F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrLine4
            // 
            this.xrLine4.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrLine4.ForeColor = System.Drawing.Color.Black;
            this.xrLine4.FormattingRules.Add(this.formattingRule3);
            this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(458.5678F, 68.83213F);
            this.xrLine4.Name = "xrLine4";
            this.xrLine4.SizeF = new System.Drawing.SizeF(170.5988F, 2F);
            this.xrLine4.StylePriority.UseBorderDashStyle = false;
            this.xrLine4.StylePriority.UseForeColor = false;
            this.xrLine4.Visible = false;
            // 
            // formattingRule3
            // 
            this.formattingRule3.Condition = " [name_app3] != \' \'";
            this.formattingRule3.DataMember = "sp_report_approve_list";
            this.formattingRule3.Formatting.Visible = DevExpress.Utils.DefaultBoolean.True;
            this.formattingRule3.Name = "formattingRule3";
            // 
            // xrLine3
            // 
            this.xrLine3.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrLine3.ForeColor = System.Drawing.Color.Black;
            this.xrLine3.FormattingRules.Add(this.formattingRule2);
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(245.7823F, 69.91871F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(172.5519F, 2F);
            this.xrLine3.StylePriority.UseBorderDashStyle = false;
            this.xrLine3.StylePriority.UseForeColor = false;
            this.xrLine3.Visible = false;
            // 
            // formattingRule2
            // 
            this.formattingRule2.Condition = " [name_app2] != \' \'";
            this.formattingRule2.DataMember = "sp_report_approve_list";
            this.formattingRule2.Formatting.Visible = DevExpress.Utils.DefaultBoolean.True;
            this.formattingRule2.Name = "formattingRule2";
            // 
            // xrLine2
            // 
            this.xrLine2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrLine2.ForeColor = System.Drawing.Color.Black;
            this.xrLine2.FormattingRules.Add(this.formattingRule1);
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(19.68352F, 68.83213F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(169.2967F, 2F);
            this.xrLine2.StylePriority.UseBorderDashStyle = false;
            this.xrLine2.StylePriority.UseForeColor = false;
            this.xrLine2.Visible = false;
            // 
            // formattingRule1
            // 
            this.formattingRule1.Condition = " [name_app1] != \' \'";
            this.formattingRule1.DataMember = "sp_report_approve_list";
            this.formattingRule1.Formatting.Visible = DevExpress.Utils.DefaultBoolean.True;
            this.formattingRule1.Name = "formattingRule1";
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_report_approve_list.name_app1")});
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 70.83212F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(216.6927F, 23.00001F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_report_approve_list.home_phone_app1")});
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 116.8321F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(216.6927F, 23F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel5
            // 
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_report_approve_list.name_app2")});
            this.xrLabel5.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(217.7874F, 71.91871F);
            this.xrLabel5.LockedInUserDesigner = true;
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(219.2967F, 23.00001F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_report_approve_list.POS_THA_APP1")});
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 93.83211F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(216.6927F, 23.00002F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel12
            // 
            this.xrLabel12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_report_approve_list.home_phone_app3")});
            this.xrLabel12.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(437.8005F, 117.9187F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(202.1995F, 23.00002F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel8
            // 
            this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_report_approve_list.home_phone_app2")});
            this.xrLabel8.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(217.7874F, 117.9187F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(219.2967F, 23F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel9
            // 
            this.xrLabel9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_report_approve_list.name_app3")});
            this.xrLabel9.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(437.8005F, 70.83212F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(202.1994F, 23.00001F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel11
            // 
            this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_report_approve_list.POS_THA_APP1_3")});
            this.xrLabel11.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(437.8005F, 94.91869F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(202.1995F, 23.00002F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel7
            // 
            this.xrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_report_approve_list.POS_THA_APP1_1")});
            this.xrLabel7.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(217.7874F, 94.91869F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(219.2967F, 23F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrPictureBox3
            // 
            this.xrPictureBox3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("ImageUrl", null, "sp_report_approve_list.sign_3")});
            this.xrPictureBox3.LocationFloat = new DevExpress.Utils.PointFloat(437.8005F, 0F);
            this.xrPictureBox3.Name = "xrPictureBox3";
            this.xrPictureBox3.SizeF = new System.Drawing.SizeF(202.1993F, 64.58335F);
            this.xrPictureBox3.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "HicomDBConnectionString";
            this.sqlDataSource1.Name = "sqlDataSource1";
            storedProcQuery1.Name = "sp_report_approve_list";
            queryParameter1.Name = "@document_no";
            queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("[Parameters.parameterApprove_Qu]", typeof(string));
            storedProcQuery1.Parameters.Add(queryParameter1);
            storedProcQuery1.StoredProcName = "sp_report_approve_list";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // Title
            // 
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.BorderColor = System.Drawing.Color.Black;
            this.Title.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Title.BorderWidth = 1F;
            this.Title.Font = new System.Drawing.Font("Tahoma", 14F);
            this.Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.Title.Name = "Title";
            // 
            // DetailCaption3
            // 
            this.DetailCaption3.BackColor = System.Drawing.Color.Transparent;
            this.DetailCaption3.BorderColor = System.Drawing.Color.Transparent;
            this.DetailCaption3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.DetailCaption3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.DetailCaption3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.DetailCaption3.Name = "DetailCaption3";
            this.DetailCaption3.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailCaption3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData3
            // 
            this.DetailData3.Font = new System.Drawing.Font("Tahoma", 8F);
            this.DetailData3.ForeColor = System.Drawing.Color.Black;
            this.DetailData3.Name = "DetailData3";
            this.DetailData3.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData3_Odd
            // 
            this.DetailData3_Odd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.DetailData3_Odd.BorderColor = System.Drawing.Color.Transparent;
            this.DetailData3_Odd.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.DetailData3_Odd.BorderWidth = 1F;
            this.DetailData3_Odd.Font = new System.Drawing.Font("Tahoma", 8F);
            this.DetailData3_Odd.ForeColor = System.Drawing.Color.Black;
            this.DetailData3_Odd.Name = "DetailData3_Odd";
            this.DetailData3_Odd.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData3_Odd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailCaptionBackground3
            // 
            this.DetailCaptionBackground3.BackColor = System.Drawing.Color.Transparent;
            this.DetailCaptionBackground3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
            this.DetailCaptionBackground3.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.DetailCaptionBackground3.BorderWidth = 2F;
            this.DetailCaptionBackground3.Name = "DetailCaptionBackground3";
            // 
            // PageInfo
            // 
            this.PageInfo.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.PageInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.PageInfo.Name = "PageInfo";
            this.PageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            // 
            // parameterApprove_Qu
            // 
            this.parameterApprove_Qu.Description = "parameterApprove_Qu";
            this.parameterApprove_Qu.Name = "parameterApprove_Qu";
            // 
            // Quotation_Approve_Qu
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "sp_report_approve_list";
            this.DataSource = this.sqlDataSource1;
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRule1,
            this.formattingRule2,
            this.formattingRule3});
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 150;
            this.PageWidth = 650;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.parameterApprove_Qu});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.DetailCaption3,
            this.DetailData3,
            this.DetailData3_Odd,
            this.DetailCaptionBackground3,
            this.PageInfo});
            this.Version = "17.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.UI.XRControlStyle Title;
        private DevExpress.XtraReports.UI.XRControlStyle DetailCaption3;
        private DevExpress.XtraReports.UI.XRControlStyle DetailData3;
        private DevExpress.XtraReports.UI.XRControlStyle DetailData3_Odd;
        private DevExpress.XtraReports.UI.XRControlStyle DetailCaptionBackground3;
        private DevExpress.XtraReports.UI.XRControlStyle PageInfo;
        private DevExpress.XtraReports.UI.XRLine xrLine4;
        private DevExpress.XtraReports.UI.XRLine xrLine3;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.Parameters.Parameter parameterApprove_Qu;
        private DevExpress.XtraReports.UI.FormattingRule formattingRule1;
        private DevExpress.XtraReports.UI.FormattingRule formattingRule3;
        private DevExpress.XtraReports.UI.FormattingRule formattingRule2;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox3;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox2;
    }
}
