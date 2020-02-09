using System;
using System.Collections.Generic;
using System.Linq;
//Custom using namespace
using DevExpress.Web;
using System.Collections;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using HicomIOS.ClassUtil;
using System.Globalization;

namespace HicomIOS
{
    public partial class _Default : MasterDetailPage
    {
        public class NotificationMessage
        {
            public int id { get; set; }
            public string description { get; set; }
            public string subject { get; set; }
            public string notice_type { get; set; }
            public decimal time { get; set; }
            public string customer_code { get; set; }
            public string customer_name { get; set; }
            public string display_notice_date { get; set; }
            public string reference_no { get; set; }
        }

        public class CheckApprove
        {
            public int user_id { get; set; } 
        }

        public class Dashboard
        {
            public List<DashboardSumDocumentClass> DashboardSumDocument { get; set; }
            public List<DashboardSumTotalGraphProductClass> DashboardSumTotalGraphProduct { get; set; }
            public List<DashboardSumTotalGraphSparePartClass> DashboardSumTotalGraphSparePart { get; set; }
            public List<DashboardSumTotalGraphAnnualClass> DashboardSumTotalGraphAnnual { get; set; }
        }

        public class DashboardSummaryGraphProduct
        {
            public List<DashboardSumTotalGraphProductClass> DashboardSumTotalGraphProduct { get; set; }
        }

        public class DashboardSummaryAnnualGraphProduct
        {
            public List<DashboardSumTotalGraphProductClass> DashboardSumTotalGraphProduct { get; set; }
        }
        public class DashboardSummaryGraphSparePart
        {
            public List<DashboardSumTotalGraphSparePartClass> DashboardSumTotalGraphSparePart { get; set; }
        }

        public class DashboardSummaryAnnualGraphSparePart
        {
            public List<DashboardSumTotalGraphSparePartClass> DashboardSumTotalGraphSparePart { get; set; }
        }
        public class DashboardSummaryGraphAnnual
        {
            public List<DashboardSumTotalGraphAnnualClass> DashboardSumTotalGraphAnnual { get; set; }
        }

        public class DashboardSummaryAnnualGraphAnnual
        {
            public List<DashboardSumTotalGraphAnnualClass> DashboardSumTotalGraphAnnual { get; set; }
        }

        public class DashboardSumDocumentClass
        {
            public int qu_sum { get; set; }
            public int so_sum { get; set; }
            public int is_sum { get; set; }
            public int pr_sum { get; set; }
            public int br_sum { get; set; }
            public int rt_sum { get; set; }
        }

        public class DashboardSumTotalGraphProductClass
        {
            public string dates { get; set; }
            public decimal total { get; set; }
        }

        public class DashboardSumTotalGraphSparePartClass
        {
            public string dates { get; set; }
            public decimal total { get; set; }
        }

        public class DashboardSumTotalGraphAnnualClass
        {
            public string dates { get; set; }
            public decimal total { get; set; }
        }

        public override string PageName { get { return "Default"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            //BasePage.SelectedTopMenu = "";
            Check_Approve();
        }
        protected void Page_Init(object sender, EventArgs e)
        {
          
        }

        protected void Check_Approve()
        { 
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        { 
                             new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID }

                        };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_check_approve", arrParm.ToArray());
                conn.Close();
                var data = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                if (data != null)
                {

                    ApproveContainer.Visible = false;

                    if (data.Count>0)
                    {
                        ApproveContainer.Visible = true;
                    }

                    ApproveTabQU.Visible =  false;
                    ApproveContentTabQU.Visible = false;
                    ApproveTabSO.Visible = false;
                    ApproveContentTabSO.Visible = false;
                    ApproveTabIS.Visible = false;
                    ApproveContentTabIS.Visible = false;
                    ApproveTabDN.Visible = false;
                    ApproveContentTabDN.Visible = false;
                    ApproveTabPR.Visible = false;
                    ApproveContentTabPR.Visible = false;
                    ApproveTabRE.Visible = false;
                    ApproveContentTabRE.Visible = false;
                    ApproveTabBR.Visible = false;
                    ApproveContentTabBR.Visible = false;

                    List<string> ApproveDoc = new List<string>();
                    String ApproveDocTypeQU = "";
                    String ApproveDocTypeSO = "";
                    String ApproveDocTypeIS = "";
                    String ApproveDocTypeDN = "";
                    String ApproveDocTypePR = "";
                    String ApproveDocTypeRE = "";
                    String ApproveDocTypeBR = "";

                    foreach (var row in data)
                    {
                        ApproveDoc.Add(Convert.ToString(row["approve_document"]));
                        if (Convert.ToString(row["approve_document"]) == Convert.ToString("QU"))
                        {
                            ApproveTabQU.Visible = true;
                            ApproveContentTabQU.Visible = true;
                            if (!ApproveDocTypeQU.Equals(""))
                            {
                                ApproveDocTypeQU += ",";
                            }
                            ApproveDocTypeQU += Convert.ToString(row["approve_doc_type"]);

                        }
                        if (Convert.ToString(row["approve_document"]) == Convert.ToString("SO"))
                        {
                            ApproveTabSO.Visible = true;
                            ApproveContentTabSO.Visible = true;
                            if (!ApproveDocTypeSO.Equals(""))
                            {
                                ApproveDocTypeSO += ",";
                            }
                            ApproveDocTypeSO += Convert.ToString(row["approve_doc_type"]);

                        }
                        if (Convert.ToString(row["approve_document"]) == Convert.ToString("IS"))
                        {
                            ApproveTabIS.Visible = true;
                            ApproveContentTabIS.Visible = true;
                            if (!ApproveDocTypeIS.Equals(""))
                            {
                                ApproveDocTypeIS += ",";
                            }
                            ApproveDocTypeIS += Convert.ToString(row["approve_doc_type"]);

                        }

                        if (Convert.ToString(row["approve_document"]) == Convert.ToString("DN"))
                        {
                            ApproveTabDN.Visible = true;
                            ApproveContentTabDN.Visible = true;
                            if (!ApproveDocTypeDN.Equals(""))
                            {
                                ApproveDocTypeDN += ",";
                            }
                            ApproveDocTypeDN += Convert.ToString(row["approve_doc_type"]);

                        }

                        if (Convert.ToString(row["approve_document"]) == Convert.ToString("PR"))
                        {
                            ApproveTabPR.Visible = true;
                            ApproveContentTabPR.Visible = true;
                            if (!ApproveDocTypePR.Equals(""))
                            {
                                ApproveDocTypePR += ",";
                            }
                            ApproveDocTypePR += Convert.ToString(row["approve_doc_type"]);

                        }

                        if (Convert.ToString(row["approve_document"]) == Convert.ToString("RE"))
                        {
                            ApproveTabRE.Visible = true;
                            ApproveContentTabRE.Visible = true;
                            if (!ApproveDocTypeRE.Equals(""))
                            {
                                ApproveDocTypeRE += ",";
                            }
                            ApproveDocTypeRE += Convert.ToString(row["approve_doc_type"]);

                        }

                        if (Convert.ToString(row["approve_document"]) == Convert.ToString("BR"))
                        {
                            ApproveTabBR.Visible = true;
                            ApproveContentTabBR.Visible = true;
                            if (!ApproveDocTypeBR.Equals(""))
                            {
                                ApproveDocTypeBR += ",";
                            }
                            ApproveDocTypeBR += Convert.ToString(row["approve_doc_type"]);

                        }


                    } //END FOREACH 


                    string[] arrApproveDoc = ApproveDoc.ToArray();  
                    if (Array.IndexOf(arrApproveDoc, "QU") > -1)
                    {
                        List<SqlParameter> arrParmQU = new List<SqlParameter>
                        {
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID) },
                            new SqlParameter("@approve_type", SqlDbType.VarChar, 100) { Value = ApproveDocTypeQU  }
                        };

                        conn.Open();
                        var dsResultQU = SqlHelper.ExecuteDataset(conn, "sp_approve_quotation_list", arrParmQU.ToArray());
                        conn.Close(); 
                        gridViewQU.DataSource = dsResultQU;
                        gridViewQU.FilterExpression = FilterBag.GetExpression(false);
                        gridViewQU.DataBind();
                    }

                    if (Array.IndexOf(arrApproveDoc, "SO") > -1)
                    {
                        List<SqlParameter> arrParmSO = new List<SqlParameter>
                        {
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID) },
                            new SqlParameter("@approve_type", SqlDbType.VarChar, 100) { Value = ApproveDocTypeSO  }
                        };

                        conn.Open();
                        var dsResultSO = SqlHelper.ExecuteDataset(conn, "sp_approve_sale_order_list", arrParmSO.ToArray());
                        conn.Close();

                        gridViewSO.DataSource = dsResultSO;
                        gridViewSO.FilterExpression = FilterBag.GetExpression(false);
                        gridViewSO.DataBind();
                    }


                    if (Array.IndexOf(arrApproveDoc, "IS") > -1)
                    {
                        List<SqlParameter> arrParmIS = new List<SqlParameter>
                        {
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID) },
                            new SqlParameter("@approve_type", SqlDbType.VarChar, 100) { Value = ApproveDocTypeIS  }
                        };

                        conn.Open();
                        var dsResultIS = SqlHelper.ExecuteDataset(conn, "sp_approve_issue_list", arrParmIS.ToArray());
                        conn.Close();

                        gridViewIS.DataSource = dsResultIS;
                        gridViewIS.FilterExpression = FilterBag.GetExpression(false);
                        gridViewIS.DataBind();
                    }

                    if (Array.IndexOf(arrApproveDoc, "DN") > -1)
                    {
                        List<SqlParameter> arrParmDN = new List<SqlParameter>
                        {
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID) },
                            new SqlParameter("@approve_type", SqlDbType.VarChar, 100) { Value = ApproveDocTypeDN  }
                        };

                        conn.Open();
                        var dsResultDN = SqlHelper.ExecuteDataset(conn, "sp_approve_delivery_note_list", arrParmDN.ToArray());
                        conn.Close();

                        gridViewDN.DataSource = dsResultDN;
                        gridViewDN.FilterExpression = FilterBag.GetExpression(false);
                        gridViewDN.DataBind();
                    }

                    if (Array.IndexOf(arrApproveDoc, "PR") > -1)
                    {
                        List<SqlParameter> arrParmPR = new List<SqlParameter>
                        {
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID) },
                            new SqlParameter("@approve_type", SqlDbType.VarChar, 100) { Value = ApproveDocTypePR  }
                        };

                        conn.Open();
                        var dsResultPR = SqlHelper.ExecuteDataset(conn, "sp_approve_purchase_request_list", arrParmPR.ToArray());
                        conn.Close();

                        gridViewPR.DataSource = dsResultPR;
                        gridViewPR.FilterExpression = FilterBag.GetExpression(false);
                        gridViewPR.DataBind();
                    }

                    if (Array.IndexOf(arrApproveDoc, "RE") > -1)
                    {
                        List<SqlParameter> arrParmRE = new List<SqlParameter>
                        {
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID) },
                            new SqlParameter("@approve_type", SqlDbType.VarChar, 100) { Value = ApproveDocTypeRE  }
                        };

                        conn.Open();
                        var dsResultRE = SqlHelper.ExecuteDataset(conn, "sp_approve_return_list", arrParmRE.ToArray());
                        conn.Close();

                        //gridViewRE.DataSource = dsResultRE;
                        //gridViewRE.FilterExpression = FilterBag.GetExpression(false);
                        //gridViewRE.DataBind();
                    }

                    if (Array.IndexOf(arrApproveDoc, "BR") > -1)
                    {
                        List<SqlParameter> arrParmBR = new List<SqlParameter>
                        {
                            new SqlParameter("@user_id", SqlDbType.Int) { Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID) },
                            new SqlParameter("@approve_type", SqlDbType.VarChar, 100) { Value = ApproveDocTypeBR  }
                        };

                        conn.Open();
                        var dsResultBR = SqlHelper.ExecuteDataset(conn, "sp_approve_borrow_list", arrParmBR.ToArray());
                        conn.Close();

                        //gridViewBR.DataSource = dsResultBR;
                        //gridViewBR.FilterExpression = FilterBag.GetExpression(false);
                        //gridViewBR.DataBind();
                    } 

                }

            }
        }

        protected void gridViewQU_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        { 
        }

        protected void gridViewSO_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        { 
        }

        protected void gridViewIS_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        { 
        }

        protected void gridViewDN_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
        } 

        protected void gridViewPR_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
        }
         
        protected void gridViewRE_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
        }

        protected void gridViewBR_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
        } 


        [WebMethod]
        public static string ChangeStatus(string refer_id, string refer_name , string refer_type)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParmQU = new List<SqlParameter>
                            {
                                new SqlParameter("@user_id", SqlDbType.Int) { Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID)},
                                new SqlParameter("@refer_id", SqlDbType.Int) { Value = Convert.ToInt32(refer_id) },
                                new SqlParameter("@refer_name", SqlDbType.VarChar,150) { Value = Convert.ToString(refer_name) } ,
                                new SqlParameter("@refer_type", SqlDbType.VarChar,150) { Value = Convert.ToString(refer_type) } 
                               // new SqlParameter("@customer_id", SqlDbType.Int) { Value = 0 },
                            };

                    conn.Open();
                    var insertLogApprove= SqlHelper.ExecuteDataset(conn, "sp_approve_log_add", arrParmQU.ToArray());
                    conn.Close();

                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "success";
        }


        #region "Inherited Event of Filter Control"
        public override void OnFilterChanged()
        {
            //Code Here
        }
        public override void SaveEditFormChanges(string parameters)
        {
            //Code Here
        }
        public override void DeleteEntry(string employeeID)
        {
            //Code Here
        }

        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            //Code Here
            return result.OfType<FilterControlColumn>();
        }
        #endregion

        [WebMethod]
        public static void SetSelectedTopMenu(string code)
        {
            try
            {
                BasePage.SelectedTopMenu = code;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static void SetHeightScreen(string height)
        {
            try
            {
                BasePage.SizeScreen = height;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [WebMethod]
        public static List<NotificationMessage> GetNotification()
        {
            var returnData = new List<NotificationMessage>();
            if (string.IsNullOrEmpty(ConstantClass.SESSION_USER_ID))
            {
                throw new Exception("NO SESSION ID");
            }
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                             new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                             new SqlParameter("@customer_id", SqlDbType.Int) { Value = 0 },
                             new SqlParameter("@user_id", SqlDbType.Int) { Value = ConstantClass.SESSION_USER_ID }

                        };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_notification_list", arrParm.ToArray());

                if (dsResult != null)
                {
                    var row = (from t in dsResult.Tables[0].AsEnumerable() where t.Field<Boolean>("is_read") == false select t).ToList();
                    foreach (var data in row)
                    {
                        var noticeDate = Convert.IsDBNull(data["notice_date"]) ? DateTime.MinValue : Convert.ToDateTime(data["notice_date"]);
                        var diffDate = (noticeDate - DateTime.Now).TotalSeconds;
                        returnData.Add(new NotificationMessage()
                        {
                            id = Convert.IsDBNull(data["id"]) ? 0 : Convert.ToInt32(data["id"]),
                            customer_code = Convert.IsDBNull(data["customer_code"]) ? null : Convert.ToString(data["customer_code"]),
                            customer_name = Convert.IsDBNull(data["company_name_tha"]) ? null : Convert.ToString(data["company_name_tha"]),
                            subject = Convert.IsDBNull(data["subject"]) ? null : Convert.ToString(data["subject"]),
                            description = Convert.IsDBNull(data["description"]) ? null : Convert.ToString(data["description"]),
                            time = Convert.ToDecimal(diffDate) * 1000,
                            display_notice_date = Convert.IsDBNull(data["notice_date"]) ? string.Empty : Convert.ToDateTime(data["notice_date"]).ToString("dd/MM/yyyy HH:mm"),
                            notice_type = Convert.IsDBNull(data["notice_type"]) ? null : Convert.ToString(data["notice_type"]),
                            reference_no = Convert.IsDBNull(data["reference_no"]) ? null : Convert.ToString(data["reference_no"]),
                        });

                    }
                }

            }
            return returnData;
        }
        [WebMethod]
        public static string ReadNotification(int id)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("sp_notification_update_read", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                    conn.Open();
                    cmd.ExecuteNonQuery(); ;
                    conn.Close();
                }
            }
            return "READ";
        }

        [WebMethod]
        public static Dashboard GetDashboardData(string date_start, string last_day_in_month)
        {
            var dashboardData = new Dashboard();
            var dsDataDashboardSumDocument = new DataSet();
            var dsDataDashboardSumTotalGraphProduct = new DataSet();
            var dsDataDashboardSumTotalGraphSparePart = new DataSet();
            var dsDataDashboardSumTotalGraphAnnual = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@user_group", SqlDbType.Int) { Value = Convert.ToInt32(ConstantClass.SESSION_USER_GROUP_ID) }
                        };
                    conn.Open();
                    dsDataDashboardSumDocument = SqlHelper.ExecuteDataset(conn, "sp_dashboard_sum_document_list", arrParm.ToArray());
                    conn.Close();

                    List<SqlParameter> arrGraphProduct = new List<SqlParameter>
                        {
                            new SqlParameter("@date_start", SqlDbType.VarChar, 50) { Value = date_start },
                            new SqlParameter("@lastDayInMonth", SqlDbType.Int) { Value = Convert.ToInt32(last_day_in_month) },
                            new SqlParameter("@quotation_type", SqlDbType.VarChar, 1) { Value = "P" }
                        };
                    conn.Open();
                    dsDataDashboardSumTotalGraphProduct = SqlHelper.ExecuteDataset(conn, "sp_dashboard_sum_total_graph", arrGraphProduct.ToArray());
                    conn.Close();

                    List<SqlParameter> arrGraphSpare = new List<SqlParameter>
                        {
                            new SqlParameter("@date_start", SqlDbType.VarChar, 50) { Value = date_start },
                            new SqlParameter("@lastDayInMonth", SqlDbType.Int) { Value = Convert.ToInt32(last_day_in_month) },
                            new SqlParameter("@quotation_type", SqlDbType.VarChar, 1) { Value = "S" }
                        };
                    conn.Open();
                    dsDataDashboardSumTotalGraphSparePart = SqlHelper.ExecuteDataset(conn, "sp_dashboard_sum_total_graph", arrGraphSpare.ToArray());
                    conn.Close();

                    List<SqlParameter> arrGraphAnnual = new List<SqlParameter>
                        {
                            new SqlParameter("@date_start", SqlDbType.VarChar, 50) { Value = date_start },
                            new SqlParameter("@lastDayInMonth", SqlDbType.Int) { Value = Convert.ToInt32(last_day_in_month) },
                            new SqlParameter("@quotation_type", SqlDbType.VarChar, 1) { Value = "A" }
                        };
                    conn.Open();
                    dsDataDashboardSumTotalGraphAnnual = SqlHelper.ExecuteDataset(conn, "sp_dashboard_sum_total_graph", arrGraphAnnual.ToArray());
                    conn.Close();
                }

                dashboardData.DashboardSumDocument = new List<DashboardSumDocumentClass>();
                if (dsDataDashboardSumDocument.Tables.Count > 0)
                {
                    dashboardData.DashboardSumDocument.Add(new DashboardSumDocumentClass()
                    {
                        qu_sum = Convert.IsDBNull(dsDataDashboardSumDocument.Tables[0].Rows[0]["qu_sum"]) ? 0 : Convert.ToInt32(dsDataDashboardSumDocument.Tables[0].Rows[0]["qu_sum"]),
                        so_sum = Convert.IsDBNull(dsDataDashboardSumDocument.Tables[1].Rows[0]["so_sum"]) ? 0 : Convert.ToInt32(dsDataDashboardSumDocument.Tables[1].Rows[0]["so_sum"]),
                        is_sum = Convert.IsDBNull(dsDataDashboardSumDocument.Tables[2].Rows[0]["is_sum"]) ? 0 : Convert.ToInt32(dsDataDashboardSumDocument.Tables[2].Rows[0]["is_sum"]),
                        pr_sum = Convert.IsDBNull(dsDataDashboardSumDocument.Tables[3].Rows[0]["pr_sum"]) ? 0 : Convert.ToInt32(dsDataDashboardSumDocument.Tables[3].Rows[0]["pr_sum"]),
                        br_sum = Convert.IsDBNull(dsDataDashboardSumDocument.Tables[4].Rows[0]["br_sum"]) ? 0 : Convert.ToInt32(dsDataDashboardSumDocument.Tables[4].Rows[0]["br_sum"]),
                        rt_sum = Convert.IsDBNull(dsDataDashboardSumDocument.Tables[5].Rows[0]["rt_sum"]) ? 0 : Convert.ToInt32(dsDataDashboardSumDocument.Tables[5].Rows[0]["rt_sum"])
                    });
                }

                dashboardData.DashboardSumTotalGraphProduct = new List<DashboardSumTotalGraphProductClass>();
                if (dsDataDashboardSumTotalGraphProduct.Tables.Count > 0)
                {
                    foreach (var rowGraphProduct in dsDataDashboardSumTotalGraphProduct.Tables[0].AsEnumerable())
                    {
                        dashboardData.DashboardSumTotalGraphProduct.Add(new DashboardSumTotalGraphProductClass()
                        {
                            dates = Convert.IsDBNull(rowGraphProduct["dates"]) ? null : Convert.ToDateTime(rowGraphProduct["dates"]).ToString("dd/MM/yyyy"),
                            total = Convert.IsDBNull(rowGraphProduct["total"]) ? 0 : Convert.ToDecimal(rowGraphProduct["total"])
                        });
                    }
                }

                dashboardData.DashboardSumTotalGraphSparePart = new List<DashboardSumTotalGraphSparePartClass>();
                if (dsDataDashboardSumTotalGraphSparePart.Tables.Count > 0)
                {
                    foreach (var rowGraphSparePart in dsDataDashboardSumTotalGraphSparePart.Tables[0].AsEnumerable())
                    {
                        dashboardData.DashboardSumTotalGraphSparePart.Add(new DashboardSumTotalGraphSparePartClass()
                        {
                            dates = Convert.IsDBNull(rowGraphSparePart["dates"]) ? null : Convert.ToDateTime(rowGraphSparePart["dates"]).ToString("dd/MM/yyyy"),
                            total = Convert.IsDBNull(rowGraphSparePart["total"]) ? 0 : Convert.ToDecimal(rowGraphSparePart["total"])
                        });
                    }
                }

                dashboardData.DashboardSumTotalGraphAnnual = new List<DashboardSumTotalGraphAnnualClass>();
                if (dsDataDashboardSumTotalGraphAnnual.Tables.Count > 0)
                {
                    foreach (var rowGraphAnnual in dsDataDashboardSumTotalGraphAnnual.Tables[0].AsEnumerable())
                    {
                        dashboardData.DashboardSumTotalGraphAnnual.Add(new DashboardSumTotalGraphAnnualClass()
                        {
                            dates = Convert.IsDBNull(rowGraphAnnual["dates"]) ? null : Convert.ToDateTime(rowGraphAnnual["dates"]).ToString("dd/MM/yyyy"),
                            total = Convert.IsDBNull(rowGraphAnnual["total"]) ? 0 : Convert.ToDecimal(rowGraphAnnual["total"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dashboardData;
        }

        [WebMethod]
        public static DashboardSummaryGraphProduct GetSummaryProduct(string dateStart, string amountDay)
        {
            var dashboardSumProductData = new DashboardSummaryGraphProduct();
            var dsDataDashboardSumProduct = new DataSet();
            try
            {

                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrGraphProduct = new List<SqlParameter>
                        {
                            new SqlParameter("@date_start", SqlDbType.VarChar, 50) { Value = dateStart },
                            new SqlParameter("@lastDayInMonth", SqlDbType.Int) { Value = Convert.ToInt32(amountDay) },
                            new SqlParameter("@quotation_type", SqlDbType.VarChar, 1) { Value = "P" }
                        };
                    conn.Open();
                    dsDataDashboardSumProduct = SqlHelper.ExecuteDataset(conn, "sp_dashboard_sum_total_graph", arrGraphProduct.ToArray());
                    conn.Close();
                }

                dashboardSumProductData.DashboardSumTotalGraphProduct = new List<DashboardSumTotalGraphProductClass>();
                if (dsDataDashboardSumProduct.Tables.Count > 0)
                {
                    foreach (var rowGraphProduct in dsDataDashboardSumProduct.Tables[0].AsEnumerable())
                    {
                        dashboardSumProductData.DashboardSumTotalGraphProduct.Add(new DashboardSumTotalGraphProductClass()
                        {
                            dates = Convert.IsDBNull(rowGraphProduct["dates"]) ? null : Convert.ToDateTime(rowGraphProduct["dates"]).ToString("dd/MM/yyyy"),
                            total = Convert.IsDBNull(rowGraphProduct["total"]) ? 0 : Convert.ToDecimal(rowGraphProduct["total"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dashboardSumProductData;
        }

        [WebMethod]
        public static DashboardSummaryAnnualGraphProduct GetSummaryAnnualProduct(string thisYear)
        {
            var dashboardSummaryAnnualProductData = new DashboardSummaryAnnualGraphProduct();
            var dsDataDashboardSummaryAnnualProduct = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrGraphAnnualProduct = new List<SqlParameter>
                        {
                            new SqlParameter("@years", SqlDbType.Int) { Value = Convert.ToInt32(thisYear) },
                            new SqlParameter("@quotation_type", SqlDbType.VarChar, 1) { Value = "P" }
                        };
                    conn.Open();
                    dsDataDashboardSummaryAnnualProduct = SqlHelper.ExecuteDataset(conn, "sp_dashboard_summary_annual_graph", arrGraphAnnualProduct.ToArray());
                    conn.Close();
                }

                dashboardSummaryAnnualProductData.DashboardSumTotalGraphProduct = new List<DashboardSumTotalGraphProductClass>();
                if (dsDataDashboardSummaryAnnualProduct.Tables.Count > 0)
                {
                    foreach (var rowGraphAnnualProduct in dsDataDashboardSummaryAnnualProduct.Tables[0].AsEnumerable())
                    {
                        dashboardSummaryAnnualProductData.DashboardSumTotalGraphProduct.Add(new DashboardSumTotalGraphProductClass()
                        {
                            dates = Convert.IsDBNull(rowGraphAnnualProduct["dates"]) ? null : Convert.ToString(rowGraphAnnualProduct["dates"]),
                            total = Convert.IsDBNull(rowGraphAnnualProduct["total"]) ? 0 : Convert.ToDecimal(rowGraphAnnualProduct["total"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dashboardSummaryAnnualProductData;
        }

        [WebMethod]
        public static DashboardSummaryGraphSparePart GetSummarySparePart(string dateStart, string amountDay)
        {
            var dashboardSummarySparePartData = new DashboardSummaryGraphSparePart();
            var dsDataDashboardSummarySparePart = new DataSet();
            try
            {

                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrGraphProduct = new List<SqlParameter>
                        {
                            new SqlParameter("@date_start", SqlDbType.VarChar, 50) { Value = dateStart },
                            new SqlParameter("@lastDayInMonth", SqlDbType.Int) { Value = Convert.ToInt32(amountDay) },
                            new SqlParameter("@quotation_type", SqlDbType.VarChar, 1) { Value = "S" }
                        };
                    conn.Open();
                    dsDataDashboardSummarySparePart = SqlHelper.ExecuteDataset(conn, "sp_dashboard_sum_total_graph", arrGraphProduct.ToArray());
                    conn.Close();
                }

                dashboardSummarySparePartData.DashboardSumTotalGraphSparePart = new List<DashboardSumTotalGraphSparePartClass>();
                if (dsDataDashboardSummarySparePart.Tables.Count > 0)
                {
                    foreach (var rowGraphProduct in dsDataDashboardSummarySparePart.Tables[0].AsEnumerable())
                    {
                        dashboardSummarySparePartData.DashboardSumTotalGraphSparePart.Add(new DashboardSumTotalGraphSparePartClass()
                        {
                            dates = Convert.IsDBNull(rowGraphProduct["dates"]) ? null : Convert.ToDateTime(rowGraphProduct["dates"]).ToString("dd/MM/yyyy"),
                            total = Convert.IsDBNull(rowGraphProduct["total"]) ? 0 : Convert.ToDecimal(rowGraphProduct["total"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dashboardSummarySparePartData;
        }

        [WebMethod]
        public static DashboardSummaryAnnualGraphSparePart GetSummaryAnnualSparePart(string thisYear)
        {
            var dashboardSummaryAnnualSparePartData = new DashboardSummaryAnnualGraphSparePart();
            var dsDataDashboardSummaryAnnualSparePart = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrGraphAnnualProduct = new List<SqlParameter>
                        {
                            new SqlParameter("@years", SqlDbType.Int) { Value = Convert.ToInt32(thisYear) },
                            new SqlParameter("@quotation_type", SqlDbType.VarChar, 1) { Value = "S" }
                        };
                    conn.Open();
                    dsDataDashboardSummaryAnnualSparePart = SqlHelper.ExecuteDataset(conn, "sp_dashboard_summary_annual_graph", arrGraphAnnualProduct.ToArray());
                    conn.Close();
                }

                dashboardSummaryAnnualSparePartData.DashboardSumTotalGraphSparePart = new List<DashboardSumTotalGraphSparePartClass>();
                if (dsDataDashboardSummaryAnnualSparePart.Tables.Count > 0)
                {
                    foreach (var rowGraphAnnualProduct in dsDataDashboardSummaryAnnualSparePart.Tables[0].AsEnumerable())
                    {
                        dashboardSummaryAnnualSparePartData.DashboardSumTotalGraphSparePart.Add(new DashboardSumTotalGraphSparePartClass()
                        {
                            dates = Convert.IsDBNull(rowGraphAnnualProduct["dates"]) ? null : Convert.ToString(rowGraphAnnualProduct["dates"]),
                            total = Convert.IsDBNull(rowGraphAnnualProduct["total"]) ? 0 : Convert.ToDecimal(rowGraphAnnualProduct["total"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dashboardSummaryAnnualSparePartData;
        }

        [WebMethod]
        public static DashboardSummaryGraphAnnual GetSummaryAnnual(string dateStart, string amountDay)
        {
            var dashboardSummaryAnnualData = new DashboardSummaryGraphAnnual();
            var dsDataDashboardSummaryAnnual = new DataSet();
            try
            {

                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrGraphProduct = new List<SqlParameter>
                        {
                            new SqlParameter("@date_start", SqlDbType.VarChar, 50) { Value = dateStart },
                            new SqlParameter("@lastDayInMonth", SqlDbType.Int) { Value = Convert.ToInt32(amountDay) },
                            new SqlParameter("@quotation_type", SqlDbType.VarChar, 1) { Value = "A" }
                        };
                    conn.Open();
                    dsDataDashboardSummaryAnnual = SqlHelper.ExecuteDataset(conn, "sp_dashboard_sum_total_graph", arrGraphProduct.ToArray());
                    conn.Close();
                }

                dashboardSummaryAnnualData.DashboardSumTotalGraphAnnual = new List<DashboardSumTotalGraphAnnualClass>();
                if (dsDataDashboardSummaryAnnual.Tables.Count > 0)
                {
                    foreach (var rowGraphProduct in dsDataDashboardSummaryAnnual.Tables[0].AsEnumerable())
                    {
                        dashboardSummaryAnnualData.DashboardSumTotalGraphAnnual.Add(new DashboardSumTotalGraphAnnualClass()
                        {
                            dates = Convert.IsDBNull(rowGraphProduct["dates"]) ? null : Convert.ToDateTime(rowGraphProduct["dates"]).ToString("dd/MM/yyyy"),
                            total = Convert.IsDBNull(rowGraphProduct["total"]) ? 0 : Convert.ToDecimal(rowGraphProduct["total"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dashboardSummaryAnnualData;
        }

        [WebMethod]
        public static DashboardSummaryAnnualGraphAnnual GetSummaryAnnualAN(string thisYear)
        {
            var dashboardSummaryAnnualData = new DashboardSummaryAnnualGraphAnnual();
            var dsDataDashboardSummaryAnnual = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrGraphAnnualProduct = new List<SqlParameter>
                        {
                            new SqlParameter("@years", SqlDbType.Int) { Value = Convert.ToInt32(thisYear) },
                            new SqlParameter("@quotation_type", SqlDbType.VarChar, 1) { Value = "A" }
                        };
                    conn.Open();
                    dsDataDashboardSummaryAnnual = SqlHelper.ExecuteDataset(conn, "sp_dashboard_summary_annual_graph", arrGraphAnnualProduct.ToArray());
                    conn.Close();
                }

                dashboardSummaryAnnualData.DashboardSumTotalGraphAnnual = new List<DashboardSumTotalGraphAnnualClass>();
                if (dsDataDashboardSummaryAnnual.Tables.Count > 0)
                {
                    foreach (var rowGraphAnnualProduct in dsDataDashboardSummaryAnnual.Tables[0].AsEnumerable())
                    {
                        dashboardSummaryAnnualData.DashboardSumTotalGraphAnnual.Add(new DashboardSumTotalGraphAnnualClass()
                        {
                            dates = Convert.IsDBNull(rowGraphAnnualProduct["dates"]) ? null : Convert.ToString(rowGraphAnnualProduct["dates"]),
                            total = Convert.IsDBNull(rowGraphAnnualProduct["total"]) ? 0 : Convert.ToDecimal(rowGraphAnnualProduct["total"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dashboardSummaryAnnualData;
        }
    }
}