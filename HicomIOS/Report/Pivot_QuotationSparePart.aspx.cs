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
    public partial class Pivot_QuotationSparePart : System.Web.UI.Page
    {
        private DataSet dsPivot;
        protected void Page_Load(object sender, EventArgs e)
        {
            Pivot_Quotation_Summary_Spare_Part();
        }

        private void Pivot_Quotation_Summary_Spare_Part()
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
                        dsPivot = SqlHelper.ExecuteDataset(conn, "sp_report_quotation_summary_spare_part", arrParm.ToArray());
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
                    //fieldQuotationDate_Month.GroupInterval = PivotGroupInterval.DateMonth;
                 
                    PivotGridField fieldQuotationDate_Quarter = new PivotGridField("quotation_date", PivotArea.ColumnArea);
                    fieldQuotationDate_Quarter.Caption = "Quotation Date (Quarter)";
                    fieldQuotationDate_Quarter.Visible = false;
                    //fieldQuotationDate_Quarter.GroupInterval = PivotGroupInterval.DateQuarterYear;
                  
                    PivotGridField fieldCustomerName = new PivotGridField("customer_name", PivotArea.RowArea);
                    fieldCustomerName.Caption = "Customer Name";
                    fieldCustomerName.Visible = true;
                   
                    PivotGridField fieldH = new PivotGridField("h", PivotArea.RowArea);
                    fieldH.Caption = "H";
                    fieldH.Visible = true;
                 
                    PivotGridField fieldModel = new PivotGridField("model", PivotArea.RowArea);
                    fieldModel.Caption = "MODEL";
                    fieldModel.Visible = false;
                   
                    PivotGridField fieldMFG = new PivotGridField("mfg", PivotArea.FilterArea);
                    fieldMFG.Caption = "MFG NO";
                    fieldMFG.Visible = true;
                   
                    PivotGridField fieldMachine = new PivotGridField("machine", PivotArea.RowArea);
                    fieldMachine.Caption = "MACHIN NO";
                    fieldMachine.Visible = false;
                    
                    PivotGridField fieldPressure = new PivotGridField("pressure", PivotArea.RowArea);
                    fieldPressure.Caption = "PRESSURE";
                    fieldPressure.Visible = false;
                    
                    PivotGridField fieldDetailOfPart = new PivotGridField("detail_of_part", PivotArea.DataArea);
                    fieldDetailOfPart.Caption = "DETAIL OF PART";
                    fieldDetailOfPart.Visible = true;
                   
                    PivotGridField fieldDiscount1 = new PivotGridField("Discount1", PivotArea.RowArea);
                    fieldDiscount1.Caption = "DISCOUNT1";
                    fieldDiscount1.Visible = false;
                   
                    PivotGridField fieldDiscount2 = new PivotGridField("Discount2", PivotArea.DataArea);
                    fieldDiscount2.Caption = "Discount2";
                    fieldDiscount2.Visible = true;
                    
                    PivotGridField fieldamount = new PivotGridField("amount", PivotArea.RowArea);
                    fieldamount.Caption = "AMOUNT";
                    fieldamount.Visible = false;
                    
                    PivotGridField fieldContactName = new PivotGridField("contact_name", PivotArea.DataArea);
                    fieldContactName.Caption = "CONTACT";
                    fieldContactName.Visible = true;
                   
                    PivotGridField fieldQuStatus = new PivotGridField("qu_status", PivotArea.RowArea);
                    fieldQuStatus.Caption = "DETAIL STATUS";
                    fieldQuStatus.Visible = false;
                   
                    PivotGridField fieldStatusDateDD = new PivotGridField("status_date_DD", PivotArea.DataArea);
                    fieldStatusDateDD.Caption = "DETAIL DD";
                    fieldStatusDateDD.Visible = true;

                    PivotGridField fieldStatusDateMM = new PivotGridField("status_date_MM", PivotArea.RowArea);
                    fieldStatusDateMM.Caption = "DETAIL MM";
                    fieldStatusDateMM.Visible = false;
                    
                    PivotGridField fieldStatusDateYY = new PivotGridField("status_date_YY", PivotArea.DataArea);
                    fieldStatusDateYY.Caption = "DETAIL YY";
                    fieldStatusDateYY.Visible = true;
                   
                    PivotGridField fieldPoNo = new PivotGridField("po_no", PivotArea.RowArea);
                    fieldPoNo.Caption = "PO NO";
                    fieldPoNo.Visible = false;
                    
                    PivotGridField fieldAmountNo = new PivotGridField("amount_no", PivotArea.DataArea);
                    fieldAmountNo.Caption = "AMOUNT NO";
                    fieldAmountNo.Visible = true;
                    
                    PivotGridField fieldSalesOrderDate = new PivotGridField("sales_order_date", PivotArea.RowArea);
                    fieldSalesOrderDate.Caption = "SALE ORDER DATE";
                    fieldSalesOrderDate.Visible = false;
                    
                    PivotGridField fieldDeliveryDate = new PivotGridField("delivery_date", PivotArea.DataArea);
                    fieldDeliveryDate.Caption = "DELIVERY DATE";
                    fieldDeliveryDate.Visible = false;
                    
                    PivotGridField fieldInvNo = new PivotGridField("inv_no", PivotArea.DataArea);
                    fieldInvNo.Caption = "INV NO";
                    fieldInvNo.Visible = false;
                    
                    PivotGridField fieldAllStatus = new PivotGridField("all_status", PivotArea.DataArea);
                    fieldAllStatus.Caption = "STATUS";
                    fieldAllStatus.Visible = true;
                 
                    #endregion

                    #region " Add fields to Pivot Control"
                    // Add the fields to the control's field collection.         
                    pivotGridControl.Fields.Add(fieldQuotationNo);
                    pivotGridControl.Fields.Add(fieldQuotationDate_Month);
                    pivotGridControl.Fields.Add(fieldQuotationDate_Quarter);
                    pivotGridControl.Fields.Add(fieldCustomerName);
                    pivotGridControl.Fields.Add(fieldH);
                    pivotGridControl.Fields.Add(fieldModel);
                    pivotGridControl.Fields.Add(fieldMFG);
                    pivotGridControl.Fields.Add(fieldMachine);
                    pivotGridControl.Fields.Add(fieldPressure);
                    pivotGridControl.Fields.Add(fieldDetailOfPart);
                    pivotGridControl.Fields.Add(fieldDiscount1);
                    pivotGridControl.Fields.Add(fieldDiscount2);
                    pivotGridControl.Fields.Add(fieldamount);
                    pivotGridControl.Fields.Add(fieldContactName);
                    pivotGridControl.Fields.Add(fieldQuStatus);
                    pivotGridControl.Fields.Add(fieldStatusDateDD);
                    pivotGridControl.Fields.Add(fieldStatusDateMM);
                    pivotGridControl.Fields.Add(fieldStatusDateYY);
                    pivotGridControl.Fields.Add(fieldPoNo);
                    pivotGridControl.Fields.Add(fieldAmountNo);
                    pivotGridControl.Fields.Add(fieldSalesOrderDate);
                    pivotGridControl.Fields.Add(fieldDeliveryDate);
                    pivotGridControl.Fields.Add(fieldInvNo);
                    pivotGridControl.Fields.Add(fieldAllStatus);
                    #endregion

                    // Arrange the row fields within the Row Header Area. เลือกข้อมูลแถวที่แสดงตรงส่วนหัวของแถว
                    fieldQuotationNo.AreaIndex = 0;
                    fieldCustomerName.AreaIndex = 1;

                    // Arrange the row fields within the Column Header Area.เลือกข้อมูลคอลัมที่แสดงตรงส่วนหัวของแถว
                    fieldDetailOfPart.AreaIndex = 0;
                    fieldamount.AreaIndex = 1;
                    fieldQuStatus.AreaIndex = 2;
                    fieldStatusDateDD.AreaIndex = 3;
                    fieldStatusDateMM.AreaIndex = 4;
                    fieldStatusDateYY.AreaIndex = 5;
                    fieldPoNo.AreaIndex = 6;

                    
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