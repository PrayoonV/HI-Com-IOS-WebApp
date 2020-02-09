using Microsoft.ApplicationBlocks.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.XtraPivotGrid;
using System.Collections.Generic;

namespace HicomIOS.Report
{
    public partial class PivotGridViewer : System.Web.UI.Page
    {
        private DataSet dsPivot;
        protected void Page_Load(object sender, EventArgs e)
        {
            Pivot_Quotation_Summary_Product();
        }

        private void Pivot_Quotation_Summary_Product()
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@quotation_date_from", SqlDbType.VarChar,20) { Value = "" },
                            new SqlParameter("@quotation_date_to", SqlDbType.VarChar,20) { Value = "" }
                        };
                        conn.Open();
                        dsPivot = SqlHelper.ExecuteDataset(conn, "sp_report_quotation_summary_product", arrParm.ToArray());
                        ViewState["dsPivot"] = dsPivot;
                        conn.Close();
                    }
                    
                    pivotGridControl.RetrieveFields(PivotArea.FilterArea, false);

                    #region "Create PivotGrid Field"
                    
                    PivotGridField fieldQuotationNo = new PivotGridField("quotation_no", PivotArea.RowArea);
                    fieldQuotationNo.Caption = "Quotation No";
                    fieldQuotationNo.Visible = false;
                    
                    PivotGridField fieldQuotationDate_Month = new PivotGridField("quotation_date", PivotArea.ColumnArea);
                    fieldQuotationDate_Month.Caption = "Quotation Date (Month)";
                    fieldQuotationDate_Month.Visible = true;

                    PivotGridField fieldQuotationDate_Quarter = new PivotGridField("quotation_date", PivotArea.ColumnArea);
                    fieldQuotationDate_Quarter.Caption = "Quotation Date (Quarter)";
                    fieldQuotationDate_Quarter.Visible = false;
                    
                    PivotGridField fieldCustomerName = new PivotGridField("customer_name", PivotArea.RowArea);
                    fieldCustomerName.Caption = "Customer Name";
                    fieldCustomerName.Visible = true;
                    
                    PivotGridField fieldProjectName = new PivotGridField("project_name", PivotArea.RowArea);
                    fieldProjectName.Caption = "Project Name";
                    fieldProjectName.Visible = false;

                    PivotGridField fieldProvinceName = new PivotGridField("province_name", PivotArea.RowArea);
                    fieldProvinceName.Caption = "Province Name";
                    fieldProvinceName.Visible = true;
                    
                    PivotGridField fieldCustomerAddress = new PivotGridField("customer_address", PivotArea.RowArea);
                    fieldCustomerAddress.Caption = "Customer Address";
                    fieldCustomerAddress.Visible = false;
                    
                    PivotGridField fieldModelListAirCompressor = new PivotGridField("model_list_air_compressor", PivotArea.RowArea);
                    fieldModelListAirCompressor.Caption = "Air Compressor-Model";
                    fieldModelListAirCompressor.Visible = false;
                    
                    PivotGridField fieldModelListAirCompressorPressure = new PivotGridField("model_list_air_compressor_pressure", PivotArea.RowArea);
                    fieldModelListAirCompressorPressure.Caption = "Air Compressor-Pressure";
                    fieldModelListAirCompressorPressure.Visible = false;
                    
                    PivotGridField fieldModelListAirCompressorQty = new PivotGridField("model_list_air_compressor_qty", PivotArea.DataArea);
                    fieldModelListAirCompressorQty.Caption = "Air Compressor-Qty";
                    fieldModelListAirCompressorQty.Visible = true;
                    
                    PivotGridField fieldModelListAirDryer = new PivotGridField("model_list_air_dryer", PivotArea.RowArea);
                    fieldModelListAirDryer.Caption = "Air Dryer-Model";
                    fieldModelListAirDryer.Visible = false;
                    
                    PivotGridField fieldModelListAirDryerQty = new PivotGridField("model_list_air_dryer_qty", PivotArea.DataArea);
                    fieldModelListAirDryerQty.Caption = "Air Dryer-Qty";
                    fieldModelListAirDryerQty.Visible = true;
                    
                    PivotGridField fieldModelListLineFilter = new PivotGridField("model_list_line_filter", PivotArea.RowArea);
                    fieldModelListLineFilter.Caption = "Line Filter-Model";
                    fieldModelListLineFilter.Visible = false;
                    
                    PivotGridField fieldModelListLineFilterQty = new PivotGridField("model_list_line_filter_qty", PivotArea.DataArea);
                    fieldModelListLineFilterQty.Caption = "Line Filter-Qty";
                    fieldModelListLineFilterQty.Visible = true;
                    
                    PivotGridField fieldModelListMistFilter = new PivotGridField("model_list_mist_filter", PivotArea.RowArea);
                    fieldModelListMistFilter.Caption = "Mist Filter-Model";
                    fieldModelListMistFilter.Visible = false;
                    
                    PivotGridField fieldModelListMistFilterQty = new PivotGridField("model_list_mist_filter_qty", PivotArea.DataArea);
                    fieldModelListMistFilterQty.Caption = "Mist Filter-Qty";
                    fieldModelListMistFilterQty.Visible = true;
                    
                    PivotGridField fieldModelListAirTank = new PivotGridField("model_list_air_tank", PivotArea.RowArea);
                    fieldModelListAirTank.Caption = "Air Tank-Model";
                    fieldModelListAirTank.Visible = false;
                    
                    PivotGridField fieldModelListAirTankQty = new PivotGridField("model_list_air_tank_qty", PivotArea.DataArea);
                    fieldModelListAirTankQty.Caption = "Air Tank-Qty";
                    fieldModelListAirTankQty.Visible = true;
                    
                    PivotGridField fieldModelListOther = new PivotGridField("model_list_other", PivotArea.RowArea);
                    fieldModelListOther.Caption = "Other-Model";
                    fieldModelListOther.Visible = false;
                    
                    PivotGridField fieldModelListOtherQty = new PivotGridField("model_list_other_qty", PivotArea.DataArea);
                    fieldModelListOtherQty.Caption = "Other-Qty";
                    fieldModelListOtherQty.Visible = true;
                    
                    PivotGridField fieldSalesName = new PivotGridField("sales_name", PivotArea.RowArea);
                    fieldSalesName.Caption = "Sales Name";
                    fieldSalesName.Visible = false;
                    
                    PivotGridField fieldTotalAmount = new PivotGridField("total_amount", PivotArea.DataArea);
                    fieldTotalAmount.Caption = "Total Amount";
                    fieldTotalAmount.Visible = false;
                    fieldTotalAmount.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    fieldTotalAmount.CellFormat.FormatString = "n2";
                    
                    PivotGridField fieldTotalDiscount = new PivotGridField("total_discount", PivotArea.DataArea);
                    fieldTotalDiscount.Caption = "Total Discount";
                    fieldTotalDiscount.Visible = false;
                    fieldTotalDiscount.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    fieldTotalDiscount.CellFormat.FormatString = "n2";
                    
                    PivotGridField fieldGrandTotal = new PivotGridField("grand_total", PivotArea.DataArea);
                    fieldGrandTotal.Caption = "Net Amount";
                    fieldGrandTotal.Visible = true;
                    fieldGrandTotal.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    fieldGrandTotal.CellFormat.FormatString = "n2";
                   
                    PivotGridField fieldContactName = new PivotGridField("contact_name", PivotArea.RowArea);
                    fieldContactName.Caption = "Contact Name";
                    fieldContactName.Visible = false;
                   
                    PivotGridField fieldStatusName = new PivotGridField("status_name", PivotArea.FilterArea);
                    fieldStatusName.Caption = "Status Name";
                    fieldStatusName.Visible = true;

                    PivotGridField fieldCustomerTel = new PivotGridField("customer_tel", PivotArea.RowArea);
                    fieldCustomerTel.Caption = "Customer Tel";
                    fieldCustomerTel.Visible = false;
                    
                    PivotGridField fieldCustomerFax = new PivotGridField("customer_fax", PivotArea.RowArea);
                    fieldCustomerFax.Caption = "Customer Fax";
                    fieldCustomerFax.Visible = false;
                    
                    PivotGridField fieldCustomerEmail = new PivotGridField("customer_email", PivotArea.RowArea);
                    fieldCustomerEmail.Caption = "Customer Email";
                    fieldCustomerEmail.Visible = false;
                    
                    #endregion

                    #region " Add fields to Pivot Control"
                    // Add the fields to the control's field collection.         
                    pivotGridControl.Fields.Add(fieldQuotationNo);
                    pivotGridControl.Fields.Add(fieldQuotationDate_Month);
                    pivotGridControl.Fields.Add(fieldQuotationDate_Quarter);
                    pivotGridControl.Fields.Add(fieldCustomerName);
                    pivotGridControl.Fields.Add(fieldProjectName);
                    pivotGridControl.Fields.Add(fieldProvinceName);
                    pivotGridControl.Fields.Add(fieldCustomerAddress);
                    pivotGridControl.Fields.Add(fieldTotalAmount);
                    pivotGridControl.Fields.Add(fieldTotalDiscount);
                    pivotGridControl.Fields.Add(fieldGrandTotal);
                    pivotGridControl.Fields.Add(fieldModelListAirCompressor);
                    pivotGridControl.Fields.Add(fieldModelListAirCompressorPressure);
                    pivotGridControl.Fields.Add(fieldModelListAirCompressorQty);
                    pivotGridControl.Fields.Add(fieldModelListAirDryer);
                    pivotGridControl.Fields.Add(fieldModelListAirDryerQty);
                    pivotGridControl.Fields.Add(fieldModelListLineFilter);
                    pivotGridControl.Fields.Add(fieldModelListLineFilterQty);
                    pivotGridControl.Fields.Add(fieldModelListMistFilter);
                    pivotGridControl.Fields.Add(fieldModelListMistFilterQty);
                    pivotGridControl.Fields.Add(fieldModelListAirTank);
                    pivotGridControl.Fields.Add(fieldModelListAirTankQty);
                    pivotGridControl.Fields.Add(fieldModelListOther);
                    pivotGridControl.Fields.Add(fieldModelListOtherQty);
                    pivotGridControl.Fields.Add(fieldSalesName);
                    pivotGridControl.Fields.Add(fieldContactName);
                    pivotGridControl.Fields.Add(fieldStatusName);
                    pivotGridControl.Fields.Add(fieldCustomerTel);
                    pivotGridControl.Fields.Add(fieldCustomerFax);
                    pivotGridControl.Fields.Add(fieldCustomerEmail);
                    
                    //pivotGridControl.Fields.Add(fieldRemark);
                    #endregion

                    // Arrange the row fields within the Row Header Area.
                    fieldProvinceName.AreaIndex = 0;
                    fieldCustomerName.AreaIndex = 1;

                    // Arrange the row fields within the Column Header Area.
                    fieldGrandTotal.AreaIndex = 0;
                    fieldModelListAirCompressorQty.AreaIndex = 1;
                    fieldModelListAirDryerQty.AreaIndex = 2;
                    fieldModelListLineFilterQty.AreaIndex = 3;
                    fieldModelListMistFilterQty.AreaIndex = 4;
                    fieldModelListAirTankQty.AreaIndex = 5;
                    fieldModelListOtherQty.AreaIndex = 6;
                }
                else
                {
                    dsPivot = (DataSet)ViewState["dsPivot"];
                }
                pivotGridControl.DataSource = dsPivot;
                pivotGridControl.DataBind();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }

        private void Test_Pivot()
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        //List<SqlParameter> arrParm = new List<SqlParameter>
                        //{
                        //    new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID },
                        //    new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                        //    new SqlParameter("@customer_id", SqlDbType.Int) { Value = cbbCustomerID.Value },
                        //};
                        conn.Open();
                        dsPivot = SqlHelper.ExecuteDataset(conn, "sp_test_pivot");
                        ViewState["dsPivot"] = dsPivot;
                        conn.Close();

                        pivotGridControl.RetrieveFields(PivotArea.FilterArea, false);

                        // attention_name field.
                        PivotGridField fieldAttentionName = new PivotGridField("attention_name", PivotArea.RowArea);
                        fieldAttentionName.Caption = "ผู้ติดต่อ";

                        // project_name field.
                        PivotGridField fieldProjectName = new PivotGridField("project_name", PivotArea.RowArea);
                        fieldProjectName.Caption = "โครงการ";

                        // quotation_date field.
                        PivotGridField fieldQuotationDate = new PivotGridField("quotation_date", PivotArea.ColumnArea);
                        fieldQuotationDate.Caption = "Quotation Date";
                        fieldQuotationDate.GroupInterval = PivotGroupInterval.DateMonth;

                        // total_amount field.
                        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
                        PivotGridField fieldTotalAmount = new PivotGridField("total_amount", PivotArea.DataArea);
                        fieldTotalAmount.Caption = "Total Amount";
                        fieldTotalAmount.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        fieldTotalAmount.CellFormat.FormatString = "n2";

                        // vat_total field.
                        PivotGridField fieldTotalVAT = new PivotGridField("vat_total", PivotArea.FilterArea);
                        fieldTotalVAT.Caption = "VAT";
                        fieldTotalVAT.Visible = false;
                        fieldTotalVAT.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        fieldTotalVAT.CellFormat.FormatString = "n2";

                        // Add the fields to the control's field collection.         
                        pivotGridControl.Fields.Add(fieldAttentionName);
                        pivotGridControl.Fields.Add(fieldProjectName);
                        pivotGridControl.Fields.Add(fieldQuotationDate);
                        pivotGridControl.Fields.Add(fieldTotalAmount);
                        pivotGridControl.Fields.Add(fieldTotalVAT);

                        // Arrange the row fields within the Row Header Area.
                        fieldAttentionName.AreaIndex = 0;

                        // Arrange the column fields within the Column Header Area.
                        fieldQuotationDate.AreaIndex = 0;

                    }
                }
                else
                {
                    dsPivot = (DataSet)ViewState["dsPivot"];
                }
                pivotGridControl.DataSource = dsPivot;
                pivotGridControl.DataBind();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        
    }
}