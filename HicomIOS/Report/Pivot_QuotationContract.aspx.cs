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
    public partial class Pivot_QuotationContract : System.Web.UI.Page
    {
        private DataSet dsPivot;
        protected void Page_Load(object sender, EventArgs e)
        {
            Pivot_Quotation_Summary_Contract();
        }

        private void Pivot_Quotation_Summary_Contract()
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
                        dsPivot = SqlHelper.ExecuteDataset(conn, "sp_report_quotation_summary_service_contract", arrParm.ToArray());
                        ViewState["dsPivot"] = dsPivot;
                        conn.Close();
                    }

                    pivotGridControl.RetrieveFields(PivotArea.FilterArea, false);

                    #region "Create PivotGrid Field"

                    PivotGridField fieldContractNo = new PivotGridField("contract_no", PivotArea.RowArea);
                    fieldContractNo.Caption = "Contract No";
                    fieldContractNo.Visible = false;

                    PivotGridField fieldQuotationNo = new PivotGridField("quotation_no", PivotArea.RowArea);
                    fieldQuotationNo.Caption = "Quotation No";
                    fieldQuotationNo.Visible = false;
                    
                    PivotGridField fieldIssueDate_Month = new PivotGridField("issue_date", PivotArea.ColumnArea);
                    fieldIssueDate_Month.Caption = "Issue Date (Month)";
                    fieldIssueDate_Month.Visible = true;
                    fieldIssueDate_Month.GroupInterval = PivotGroupInterval.DateMonth;
                   
                    PivotGridField fieldIssueDate_Year = new PivotGridField("issue_date", PivotArea.ColumnArea);
                    fieldIssueDate_Year.Caption = "Issue Date (Quarter)";
                    fieldIssueDate_Year.Visible = false;
                    fieldIssueDate_Year.GroupInterval = PivotGroupInterval.DateQuarterYear;
                    
                    PivotGridField fieldCode = new PivotGridField("customer_code", PivotArea.ColumnArea);
                    fieldCode.Caption = "Code";
                    fieldCode.Visible = false;
                    
                    PivotGridField fieldCustomerName = new PivotGridField("customer_name", PivotArea.RowArea);
                    fieldCustomerName.Caption = "Customer Name";
                    fieldCustomerName.Visible = true;
                    
                    PivotGridField fieldProject = new PivotGridField("project_name", PivotArea.RowArea);
                    fieldProject.Caption = "Project";
                    fieldProject.Visible = true;
                   
                    PivotGridField fieldPrpduct = new PivotGridField("product", PivotArea.RowArea);
                    fieldPrpduct.Caption = "Product";
                    fieldPrpduct.Visible = false;

                    PivotGridField fieldContact = new PivotGridField("contact_name", PivotArea.FilterArea);
                    fieldContact.Caption = "Contact";
                    fieldContact.Visible = true;
                    
                    PivotGridField fieldQty = new PivotGridField("qty", PivotArea.RowArea);
                    fieldQty.Caption = "Qty";
                    fieldQty.Visible = false;

                    PivotGridField fieldUnit_Price = new PivotGridField("unit_price", PivotArea.RowArea);
                    fieldUnit_Price.Caption = "Unit Price";
                    fieldUnit_Price.Visible = false;

                    PivotGridField fieldDiscount1 = new PivotGridField("Discount1", PivotArea.RowArea);
                    fieldDiscount1.Caption = "Discount1";
                    fieldDiscount1.Visible = false;

                    PivotGridField fieldDiscount2 = new PivotGridField("Discount2", PivotArea.DataArea);
                    fieldDiscount2.Caption = "Discount2";
                    fieldDiscount2.Visible = true;

                    PivotGridField fieldamount = new PivotGridField("amount", PivotArea.RowArea);
                    fieldamount.Caption = "Total Amount";
                    fieldamount.Visible = false;

                    PivotGridField fieldTypeOfContract = new PivotGridField("type_of_contract", PivotArea.DataArea);
                    fieldTypeOfContract.Caption = "Type Of Contract";
                    fieldTypeOfContract.Visible = true;
                 
                    PivotGridField fieldStartiingDD = new PivotGridField("startiing_date_DD", PivotArea.RowArea);
                    fieldStartiingDD.Caption = "STARTIING DD";
                    fieldStartiingDD.Visible = false;

                    PivotGridField fieldStartiingMM = new PivotGridField("sstartiing_date_MM", PivotArea.RowArea);
                    fieldStartiingMM.Caption = "STARTIING MM";
                    fieldStartiingMM.Visible = false;

                    PivotGridField fieldStartiingYY = new PivotGridField("startiing_date_YY", PivotArea.DataArea);
                    fieldStartiingYY.Caption = "STARTIING YY";
                    fieldStartiingYY.Visible = true;
                  
                    PivotGridField fieldExpiredDateDD = new PivotGridField("expire_date_DD", PivotArea.DataArea);
                    fieldExpiredDateDD.Caption = "EXPIRE DD";
                    fieldExpiredDateDD.Visible = true;
                    
                    PivotGridField fieldExpiredDateMM = new PivotGridField("expire_date_MM", PivotArea.RowArea);
                    fieldExpiredDateMM.Caption = "EXPIRE MM";
                    fieldExpiredDateMM.Visible = false;

                    PivotGridField fieldExpiredDateYY = new PivotGridField("expire_date_YY", PivotArea.DataArea);
                    fieldExpiredDateYY.Caption = "EXPIRE YY";
                    fieldExpiredDateYY.Visible = true;
                    
                    PivotGridField fieldDetail_Status = new PivotGridField("status_name", PivotArea.DataArea);
                    fieldDetail_Status.Caption = "DETAIL_STATUS";
                    fieldDetail_Status.Visible = true;
                    
                    PivotGridField fielDetailDD = new PivotGridField("detail_date_DD", PivotArea.DataArea);
                    fielDetailDD.Caption = "Detail DD";
                    fielDetailDD.Visible = true;

                    PivotGridField fieldDetailMM = new PivotGridField("detail_date_MM", PivotArea.RowArea);
                    fieldDetailMM.Caption = "Date MM";
                    fieldDetailMM.Visible = false;

                    PivotGridField fieldDetailYY = new PivotGridField("detail_date_YY", PivotArea.DataArea);
                    fieldDetailYY.Caption = "Detail YY ";
                    fieldDetailYY.Visible = true;
                   
                    PivotGridField fieldPoNo = new PivotGridField("po_no", PivotArea.RowArea);
                    fieldPoNo.Caption = "Po No";
                    fieldPoNo.Visible = false;
 
                    PivotGridField fieldAmountPo = new PivotGridField("amount_no", PivotArea.DataArea);
                    fieldAmountPo.Caption = "Amount Po";
                    fieldAmountPo.Visible = true;
                    
                    PivotGridField fielInvoiceDD = new PivotGridField("inv_date_DD", PivotArea.DataArea);
                    fielInvoiceDD.Caption = "Invoice DD";
                    fielInvoiceDD.Visible = true;

                    PivotGridField fieldInvoiceMM = new PivotGridField("inv_date_MM", PivotArea.RowArea);
                    fieldInvoiceMM.Caption = "Invoice MM";
                    fieldInvoiceMM.Visible = false;

                    PivotGridField fieldInvoiceYY = new PivotGridField("inv_date_YY", PivotArea.DataArea);
                    fieldInvoiceYY.Caption = "Invoice YY ";
                    fieldInvoiceYY.Visible = true;

                    PivotGridField fieldInvNo  = new PivotGridField("inv_no", PivotArea.RowArea);
                    fieldInvNo.Caption = "Inv No";
                    fieldInvNo.Visible = false;
                   
                    PivotGridField fieldScheduleMM1 = new PivotGridField("schedule_from_MM", PivotArea.RowArea);
                    fieldScheduleMM1.Caption = "Schedu MM";
                    fieldScheduleMM1.Visible = false;

                    PivotGridField fieldScheduleYY1= new PivotGridField("schedule_from_YY", PivotArea.DataArea);
                    fieldScheduleYY1.Caption = "Schedu YY ";
                    fieldScheduleYY1.Visible = true;

                    PivotGridField fieldScheduleMM2 = new PivotGridField("schedule_to_MM", PivotArea.DataArea);
                    fieldScheduleMM2.Caption = "Schedu MM";
                    fieldScheduleMM2.Visible = false;

                    PivotGridField fieldScheduleYY2 = new PivotGridField("schedule_to_YY", PivotArea.DataArea);
                    fieldScheduleYY2.Caption = "dSchedule YY ";
                    fieldScheduleYY2.Visible = true;
                    
                    PivotGridField fieldNew_Contract_MM = new PivotGridField("new_contract_date_MM", PivotArea.DataArea);
                    fieldNew_Contract_MM.Caption = "New Contract MM";
                    fieldNew_Contract_MM.Visible = false;

                    PivotGridField fieldNew_Contract_YY = new PivotGridField("new_contract_date_YY", PivotArea.DataArea);
                    fieldNew_Contract_YY.Caption = "New Contract YY";
                    fieldNew_Contract_YY.Visible = false;

                    #endregion

                    #region " Add fields to Pivot Control"
                    // Add the fields to the control's field collection.         
                    pivotGridControl.Fields.Add(fieldContractNo);
                    pivotGridControl.Fields.Add(fieldQuotationNo);
                    pivotGridControl.Fields.Add(fieldIssueDate_Month);
                    pivotGridControl.Fields.Add(fieldIssueDate_Year);
                    pivotGridControl.Fields.Add(fieldCode);
                    pivotGridControl.Fields.Add(fieldCustomerName);
                    pivotGridControl.Fields.Add(fieldProject);
                    pivotGridControl.Fields.Add(fieldPrpduct);
                    pivotGridControl.Fields.Add(fieldContact);
                    pivotGridControl.Fields.Add(fieldQty);
                    pivotGridControl.Fields.Add(fieldUnit_Price);
                    pivotGridControl.Fields.Add(fieldDiscount1);
                    pivotGridControl.Fields.Add(fieldDiscount2);
                    pivotGridControl.Fields.Add(fieldamount);
                    pivotGridControl.Fields.Add(fieldTypeOfContract);
                    pivotGridControl.Fields.Add(fieldStartiingDD);
                    pivotGridControl.Fields.Add(fieldStartiingMM);
                    pivotGridControl.Fields.Add(fieldStartiingYY);
                    pivotGridControl.Fields.Add(fieldExpiredDateDD);
                    pivotGridControl.Fields.Add(fieldExpiredDateMM);
                    pivotGridControl.Fields.Add(fieldExpiredDateYY);
                    pivotGridControl.Fields.Add(fieldDetail_Status);
                    pivotGridControl.Fields.Add(fielDetailDD);
                    pivotGridControl.Fields.Add(fieldDetailMM);
                    pivotGridControl.Fields.Add(fieldDetailYY);
                    pivotGridControl.Fields.Add(fieldPoNo);
                    pivotGridControl.Fields.Add(fieldAmountPo);
                    pivotGridControl.Fields.Add(fielInvoiceDD);
                    pivotGridControl.Fields.Add(fieldInvoiceMM);
                    pivotGridControl.Fields.Add(fieldInvoiceYY);
                    pivotGridControl.Fields.Add(fieldInvNo);
                    pivotGridControl.Fields.Add(fieldScheduleMM1);
                    pivotGridControl.Fields.Add(fieldScheduleYY1);
                    pivotGridControl.Fields.Add(fieldScheduleMM2);
                    pivotGridControl.Fields.Add(fieldScheduleYY2);
                    pivotGridControl.Fields.Add(fieldNew_Contract_MM);
                    pivotGridControl.Fields.Add(fieldNew_Contract_YY);
                    #endregion

                    // Arrange the row fields within the Row Header Area. เลือกข้อมูลแถวที่แสดงตรงส่วนหัวของแถว
                    fieldContractNo.AreaIndex = 0;
                    fieldCustomerName.AreaIndex = 1;

                    // Arrange the row fields within the Column Header Area.เลือกข้อมูลคอลัมที่แสดงตรงส่วนหัวของแถว
                    fieldQuotationNo.AreaIndex = 0;
                     fieldCode.AreaIndex = 1;
                     fieldProject.AreaIndex = 2;
                     fieldIssueDate_Month.AreaIndex = 3;
                     fieldIssueDate_Year.AreaIndex = 4;
                     fieldPoNo.AreaIndex = 5;

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