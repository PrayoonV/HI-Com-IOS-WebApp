﻿using DevExpress.Web;
using HicomIOS.ClassUtil;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HicomIOS.Master
{
    public partial class AnnualService : MasterDetailPage
    {
        public int dataId = 0;
        public override string PageName { get { return "Add Annual Service"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }
        public class CustomerMFGList
        {
            public string customer_code { get; set; }
            public string customer_name { get; set; }
            public string model { get; set; }
            public string mfg { get; set; }
            public string service_fee { get; set; }
            public string project { get; set; }
            public int customer_id { get; set; }
            public string unitwarraty { get; set; }
            public string airendwarraty { get; set; }
            public int customer_mfg_id { get; set; }
            public string startup_date { get; set; }

        }
        public class AnnualServiceList
        {
            public string starting_date { get; set; }
            public string contractstarting_date { get; set; }
            public string contractexpire_date { get; set; }
            public string settingdate { get; set; }
            public int service_year_id { get; set; }
            public int limit_of_type { get; set; }
            public int time_of_contract { get; set; }
            public string type_name { get; set; }
            public string expire_date { get; set; }
            public string setting_date { get; set; }
            public int type_of_contract_id { get; set; }
            public int customer_id { get; set; }
            public int service_master_id { get; set; }
            public int customer_mfg_id { get; set; }
            public bool is_deleted { get; set; }

        }
        public class AnnualServiceDetail
        {
            public string checking_date { get; set; }
            public string service_report_no { get; set; }
            public string service_date { get; set; }
            public int checking_status { get; set; }
            public string checking_remark { get; set; }
            public int id { get; set; }
            public int service_master_id { get; set; }
            public string status_name { get; set; }
            public bool is_deleted { get; set; }
            public int limit_type { get; set; }

        }
        public class AnnualServiceFile
        {
            public int service_master_id { get; set; }
            public int schedule_id { get; set; }
            public string description { get; set; }
            public string file_name { get; set; }
            public string running_no { get; set; }
            public int id { get; set; }
            public string file_name_upload { get; set; }
            public int type { get; set; }
            public string starting_date { get; set; }
            public string mfg_number { get; set; }
            public string model_number { get; set; }

            public bool is_deleted { get; set; }

        }
        public class AnnualServiceYear
        {
            public string setting_date { get; set; }
            public string contractstarting_date { get; set; }
            public string contractexpire_date { get; set; }
            public string starting_date { get; set; }
            public int service_time { get; set; }
            public decimal service_free { get; set; }
            public int service_year_id { get; set; }
            public string service_location { get; set; }
            public string service_remark { get; set; }
            public string expire_date { get; set; }
            public int customer_id { get; set; }
            public int customer_mfg_id { get; set; }
            public bool is_deleted { get; set; }
            public string service_type { get; set; }
            public int service_master_id { get; set; }
            public string type_of_contract { get; set; }
            public int type_of_contract_id { get; set; }

            public string type_of_contract_text { get; set; }
            public int time_of_contract { get; set; }
            public string type_name { get; set; }
            public string service_type_text { get; set; }

        }

        List<AnnualServiceList> annualServiceListSave
        {
            get
            {
                if (Session["SESSION_ANNUALSERVICE_SAVE"] == null)
                    Session["SESSION_ANNUALSERVICE_SAVE"] = new List<AnnualServiceList>();
                return (List<AnnualServiceList>)Session["SESSION_ANNUALSERVICE_SAVE"];
            }
            set
            {
                Session["SESSION_ANNUALSERVICE_SAVE"] = value;
            }
        }
        List<AnnualServiceYear> annualServiceYear
        {
            get
            {
                if (Session["SESSION_ANNUALSERVICE_YEAR"] == null)
                    Session["SESSION_ANNUALSERVICE_YEAR"] = new List<AnnualServiceYear>();
                return (List<AnnualServiceYear>)Session["SESSION_ANNUALSERVICE_YEAR"];
            }
            set
            {
                Session["SESSION_ANNUALSERVICE_FILE"] = value;
            }
        }
        List<AnnualServiceFile> annualServiceFile
        {
            get
            {
                if (Session["SESSION_ANNUALSERVICE_FILE"] == null)
                    Session["SESSION_ANNUALSERVICE_FILE"] = new List<AnnualServiceFile>();
                return (List<AnnualServiceFile>)Session["SESSION_ANNUALSERVICE_FILE"];
            }
            set
            {
                Session["SESSION_ANNUALSERVICE_FILE"] = value;
            }
        }
        List<CustomerMFGList> CustomerMFGData
        {
            get
            {
                if (Session["SESSION_ANNUALSERVICE_CUSTOMERMFG_DETAIL"] == null)
                    Session["SESSION_ANNUALSERVICE_CUSTOMERMFG_DETAIL"] = new List<CustomerMFGList>();
                return (List<CustomerMFGList>)Session["SESSION_ANNUALSERVICE_CUSTOMERMFG_DETAIL"];
            }
            set
            {
                Session["SESSION_ANNUALSERVICE_CUSTOMERMFG_DETAIL"] = value;
            }
        }
        List<AnnualServiceList> annualServiceList
        {
            get
            {
                if (Session["SESSION_ANNUALSERVICE_LIST"] == null)
                    Session["SESSION_ANNUALSERVICE_LIST"] = new List<AnnualServiceList>();
                return (List<AnnualServiceList>)Session["SESSION_ANNUALSERVICE_LIST"];
            }
            set
            {
                Session["SESSION_ANNUALSERVICE_LIST"] = value;
            }
        }
        List<AnnualServiceDetail> annualServiceDetail
        {
            get
            {
                if (Session["SESSION_ANNUALSERVICE_DETAIL"] == null)
                    Session["SESSION_ANNUALSERVICE_DETAIL"] = new List<AnnualServiceDetail>();
                return (List<AnnualServiceDetail>)Session["SESSION_ANNUALSERVICE_DETAIL"];
            }
            set
            {
                Session["SESSION_ANNUALSERVICE_DETAIL"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            dataId = Convert.ToInt32(Request.QueryString["dataId"]);
            if (!Page.IsPostBack)
            {
                // Get Permission and if no permission, will redirect to another page.
                if (!Permission.GetPermission())
                    Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

                ClearWorkingSession();
                PrepareData();
                LoadData();
            }
            else
            {
                BindGridViewCustomerMFG();
                BindGridFile();
                BindGridAnnualServiceYear();
                BindGridViewmasterList();
            }
        }
        protected void PrepareData()
        {
            try
            {
                //var dtSource = new DataTable();
                //dtSource.Columns.Add("data_value", typeof(string));
                //dtSource.Columns.Add("data_text", typeof(string));
                //dtSource.Rows.Add("P", "Product");
                //dtSource.Rows.Add("S", "SparePart");
                //cbbProductType.DataSource = dtSource;
                //cbbProductType.DataBind();

                SPlanetUtil.BindASPxComboBox(ref cbbCustomerMFG, DataListUtil.DropdownStoreProcedureName.Customer_MFG);
                cbbCustomerMFG.Items.Insert(0, new ListEditItem("ทั้งหมด", ""));
                cbbCustomerMFG.SelectedIndex = 0;

                SPlanetUtil.BindASPxComboBox(ref cboTypeContract, DataListUtil.DropdownStoreProcedureName.Type_Contract);

                cbbProject.Items.Insert(0, new ListEditItem("ทั้งหมด", ""));
                cbbProject.SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void LoadData()
        {
            try
            {
                if (dataId > 0)
                {
                    var data = new DataSet();
                    var customerMFGData = (List<CustomerMFGList>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_CUSTOMERMFG_DETAIL"];


                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = Convert.ToInt32(dataId) },
                        };
                        conn.Open();
                        data = SqlHelper.ExecuteDataset(conn, "sp_annual_service_customer_mfg_list", arrParm.ToArray());


                        if (data.Tables.Count > 0)
                        {
                            customerMFGData = new List<CustomerMFGList>();
                            var result = (from t in data.Tables[0].AsEnumerable() select t).ToList();
                            foreach (var row in result)
                            {
                                customerMFGData.Add(new CustomerMFGList()
                                {
                                    customer_id = Convert.IsDBNull(row["customer_id"]) ? 0 : Convert.ToInt32(row["customer_id"]),
                                    customer_code = Convert.IsDBNull(row["customer_code"]) ? "" : Convert.ToString(row["customer_code"]),
                                    customer_name = Convert.IsDBNull(row["company_name_tha"]) ? "" : Convert.ToString(row["company_name_tha"]),
                                    customer_mfg_id = Convert.IsDBNull(row["customer_mfg_id"]) ? 0 : Convert.ToInt32(row["customer_mfg_id"]),
                                    project = Convert.IsDBNull(row["project"]) ? string.Empty : Convert.ToString(row["project"]),
                                    service_fee = Convert.IsDBNull(row["service_fee"]) ? "" : Convert.ToString(row["service_fee"]),
                                    unitwarraty = Convert.IsDBNull(row["unitwarraty"]) ? "" : Convert.ToString(row["unitwarraty"]),
                                    airendwarraty = Convert.IsDBNull(row["airendwarraty"]) ? "" : Convert.ToString(row["airendwarraty"]),
                                    model = Convert.IsDBNull(row["model"]) ? string.Empty : Convert.ToString(row["model"]),
                                    mfg = Convert.IsDBNull(row["mfg"]) ? string.Empty : Convert.ToString(row["mfg"]),
                                    startup_date = Convert.IsDBNull(row["startup_date"]) ? string.Empty : Convert.ToString(row["startup_date"]),
                                });
                            }
                        }
                    }
                    cbbCustomerMFG.Value = dataId.ToString(); // value ต้องเป็น string น่าจะ อันนี้ไม่รุ้ว่ามันไม่ฉลาดพอ convert เอง
                    HttpContext.Current.Session["SESSION_ANNUALSERVICE_CUSTOMERMFG_DETAIL"] = customerMFGData;
                    BindGridViewCustomerMFG();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void ClearWorkingSession()
        {
            Session.Remove("SESSION_ANNUALSERVICE_CUSTOMERMFG_DETAIL");
            Session.Remove("SESSION_ANNUALSERVICE_DETAIL");
            Session.Remove("SESSION_ANNUALSERVICE_LIST");
            Session.Remove("SESSION_ANNUALSERVICE_FILE");
        }
        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            return result.OfType<FilterControlColumn>();
        }

        public override void OnFilterChanged()
        {
            //BindGrid();
        }

        protected void BindGridFile()
        {
            gridFile.DataSource = (from t in annualServiceFile where t.is_deleted == false select t).ToList();
            gridFile.DataBind();
        }

        [WebMethod]
        public static void GetServiceYearData(string customer_id, string customer_mfg_id)
        {
            var data = new DataSet();
            var annualServiceYear = (List<AnnualServiceYear>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_YEAR"];
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = Convert.ToInt32(customer_id) },
                            new SqlParameter("@customer_mfg_id", SqlDbType.Int) { Value = Convert.ToInt32(customer_mfg_id) },
                        };
                conn.Open();
                data = SqlHelper.ExecuteDataset(conn, "sp_annual_service_year_list", arrParm.ToArray());


                if (data.Tables.Count > 0)
                {
                    annualServiceYear = new List<AnnualServiceYear>();
                    var result = (from t in data.Tables[0].AsEnumerable() select t).ToList();
                    foreach (var row in result)
                    {
                        annualServiceYear.Add(new AnnualServiceYear()
                        {
                            type_of_contract_id = Convert.IsDBNull(row["contract_id"]) ? 0 : Convert.ToInt32(row["contract_id"]),
                            time_of_contract = Convert.IsDBNull(row["time_of_contract"]) ? 0 : Convert.ToInt32(row["time_of_contract"]),
                            service_master_id = Convert.IsDBNull(row["service_master_id"]) ? 0 : Convert.ToInt32(row["service_master_id"]),
                            customer_id = Convert.IsDBNull(row["customer_id"]) ? 0 : Convert.ToInt32(row["customer_id"]),
                            customer_mfg_id = Convert.IsDBNull(row["customer_mfg_id"]) ? 0 : Convert.ToInt32(row["customer_mfg_id"]),
                            service_year_id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                            service_type_text = Convert.IsDBNull(row["service_type"]) ? string.Empty : Convert.ToString(row["service_type"]),
                            service_type = Convert.IsDBNull(row["service_type"]) ? string.Empty : Convert.ToString(row["service_type"]),
                            type_name = Convert.IsDBNull(row["type_name"]) ? string.Empty : Convert.ToString(row["type_name"]),
                            service_time = Convert.IsDBNull(row["service_time"]) ? 0 : Convert.ToInt32(row["service_time"]),
                            service_free = Convert.IsDBNull(row["service_free"]) ? 0 : Convert.ToDecimal(row["service_free"]),
                            is_deleted = Convert.IsDBNull(row["is_delete"]) ? true : Convert.ToBoolean(row["is_delete"]),
                            service_location = Convert.IsDBNull(row["service_location"]) ? string.Empty : Convert.ToString(row["service_location"]),
                            service_remark = Convert.IsDBNull(row["service_remark"]) ? string.Empty : Convert.ToString(row["service_remark"]),
                            starting_date = Convert.IsDBNull(row["starting_date"]) ? string.Empty : Convert.ToString(row["starting_date"]),
                            setting_date = Convert.IsDBNull(row["setting_date"]) ? string.Empty : Convert.ToString(row["setting_date"]),
                            contractstarting_date = Convert.IsDBNull(row["contractstarting_date"]) ? string.Empty : Convert.ToString(row["contractstarting_date"]),
                            contractexpire_date= Convert.IsDBNull(row["contractexpire_date"]) ? string.Empty : Convert.ToString(row["contractexpire_date"]),
                            expire_date = Convert.IsDBNull(row["expire_date"]) ? string.Empty : Convert.ToString(row["expire_date"]),

                        });
                    }
                }
            }
            HttpContext.Current.Session["SESSION_ANNUALSERVICE_YEAR"] = annualServiceYear;
        }
        [WebMethod]
        public static void GetMFGData(string id, string project, string model, string mfg_no)
        {

            var data = new DataSet();
            var customerMFGData = (List<CustomerMFGList>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_CUSTOMERMFG_DETAIL"];


            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@customer_id", SqlDbType.Int) { Value = Convert.ToInt32(id) },
                            new SqlParameter("@project", SqlDbType.VarChar, 200) { Value = project },
                            new SqlParameter("@model", SqlDbType.VarChar, 200) { Value = model },
                            new SqlParameter("@mfg_no", SqlDbType.VarChar, 200) { Value = mfg_no },
                        };
                conn.Open();
                data = SqlHelper.ExecuteDataset(conn, "sp_annual_service_customer_mfg_list", arrParm.ToArray());


                if (data.Tables.Count > 0)
                {
                    customerMFGData = new List<CustomerMFGList>();
                    var result = (from t in data.Tables[0].AsEnumerable() select t).ToList();
                    foreach (var row in result)
                    {
                        customerMFGData.Add(new CustomerMFGList()
                        {
                            customer_id = Convert.IsDBNull(row["customer_id"]) ? 0 : Convert.ToInt32(row["customer_id"]),
                            customer_code = Convert.IsDBNull(row["customer_code"]) ? "" : Convert.ToString(row["customer_code"]),
                            customer_name = Convert.IsDBNull(row["company_name_tha"]) ? "" : Convert.ToString(row["company_name_tha"]),
                            customer_mfg_id = Convert.IsDBNull(row["customer_mfg_id"]) ? 0 : Convert.ToInt32(row["customer_mfg_id"]),
                            unitwarraty = Convert.IsDBNull(row["unitwarraty"]) ? "" : Convert.ToString(row["unitwarraty"]),
                            project = Convert.IsDBNull(row["project"]) ? string.Empty : Convert.ToString(row["project"]),
                            service_fee = Convert.IsDBNull(row["service_fee"]) ? "" : Convert.ToString(row["service_fee"]),
                            airendwarraty = Convert.IsDBNull(row["airendwarraty"]) ? "" : Convert.ToString(row["airendwarraty"]),
                            model = Convert.IsDBNull(row["model"]) ? string.Empty : Convert.ToString(row["model"]),
                            mfg = Convert.IsDBNull(row["mfg"]) ? string.Empty : Convert.ToString(row["mfg"]),
                            startup_date = Convert.IsDBNull(row["startup_date"]) ? string.Empty : Convert.ToString(row["startup_date"]),
                        });
                    }
                }
            }
            HttpContext.Current.Session["SESSION_ANNUALSERVICE_CUSTOMERMFG_DETAIL"] = customerMFGData;
        }
        private void BindGridViewCustomerMFG()
        {
            gridViewCustomerMFG.DataSource = (from t in CustomerMFGData select t).ToList();
            gridViewCustomerMFG.DataBind();
        }
        protected void gridViewCustomerMFG_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //gridViewCustomerMFG.DataSource = (from t in CustomerMFGData select t).ToList();
            //gridViewCustomerMFG.DataBind();
            BindGridViewCustomerMFG();
        }

        [WebMethod]
        public static AnnualServiceList GetDetailItem(string customer_id, string customer_mfg_id, string service_year_id)
        {
            var annualServiceList = new AnnualServiceList();
            var annualServiceDetail = new List<AnnualServiceDetail>();
            var annualServiceFile = new List<AnnualServiceFile>();
            var annualServiceYear = (List<AnnualServiceYear>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_YEAR"];
            try
            {
                if (annualServiceYear.Count > 0 && annualServiceYear != null)
                {
                    var checkExist = (from t in annualServiceYear
                                      where t.is_deleted == false && t.service_year_id == Convert.ToInt32(service_year_id)
                                      select t).FirstOrDefault();
                    if (checkExist != null)
                    {
                        //  Save obj to session
                        HttpContext.Current.Session["SESSION_ANNUALSERVICE_YEAR_SELECTED"] = checkExist;

                        annualServiceList.customer_id = checkExist.customer_id;
                        annualServiceList.time_of_contract = checkExist.time_of_contract;
                        annualServiceList.service_master_id = checkExist.service_master_id;

                        var datadb = new DataSet();

                        using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                        {
                            List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                                new SqlParameter("@customer_id", SqlDbType.Int) { Value = Convert.ToInt32(customer_id) },
                                new SqlParameter("@customer_mfg_id", SqlDbType.Int) { Value = Convert.ToInt32(customer_mfg_id) },
                                new SqlParameter("@service_year_id", SqlDbType.Int) { Value = Convert.ToInt32(service_year_id) > 0 ? Convert.ToInt32(service_year_id):0 },
                        };
                            conn.Open();
                            datadb = SqlHelper.ExecuteDataset(conn, "sp_service_master_list", arrParm.ToArray());
                            if (datadb.Tables[1].Rows.Count > 0 && datadb != null)
                            {
                                var dataDetail = (from t in datadb.Tables[1].AsEnumerable() select t).ToList();
                                if (dataDetail != null)
                                {

                                    foreach (var row in dataDetail)
                                    {
                                        annualServiceDetail.Add(new AnnualServiceDetail()
                                        {
                                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                            checking_status = Convert.IsDBNull(row["checking_status"]) ? 0 : Convert.ToInt32(row["checking_status"]),
                                            service_date = Convert.IsDBNull(row["service_date"]) ? string.Empty : Convert.ToDateTime(row["service_date"]).ToString("dd/MM/yyyy"),
                                            service_report_no = Convert.IsDBNull(row["service_report_no"]) ? string.Empty : row["service_report_no"].ToString(),
                                            status_name = Convert.ToBoolean(row["checking_status"]) == false ? "รอเข้าบริการ" : "ให้บริการเรียบร้อยแล้ว",
                                            service_master_id = annualServiceList.service_master_id,
                                            checking_date = Convert.IsDBNull(row["checking_date"]) ? string.Empty : Convert.ToDateTime(row["checking_date"]).ToString("dd/MM/yyyy"),
                                            checking_remark = Convert.IsDBNull(row["checking_remark"]) ? string.Empty : Convert.ToString(row["checking_remark"]),
                                            is_deleted = Convert.ToBoolean(row["is_delete"]),
                                        });
                                    }
                                }

                                /*var detailFile = (from t in datadb.Tables[2].AsEnumerable() select t).ToList();
                                if (detailFile != null)
                                {

                                    foreach (var row in detailFile)
                                    {
                                        annualServiceFile.Add(new AnnualServiceFile()
                                        {
                                            service_master_id = annualServiceList.service_master_id,
                                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                            description = Convert.IsDBNull(row["description"]) ? string.Empty : Convert.ToString(row["description"]),
                                            file_name = Convert.IsDBNull(row["file_name"]) ? string.Empty : Convert.ToString(row["file_name"]),
                                            type = Convert.IsDBNull(row["type"]) ? 0 : Convert.ToInt32(row["id"]),
                                            is_deleted = Convert.ToBoolean(row["is_delete"]),
                                        });
                                    }
                                }*/
                            }
                        }


                        /*for (int i = 0; i < 3; i++)
                        {
                            var running_no = "190203";
                            var description = "Repairing";
                            var file_name = "190203_Repairing";
                            if (i == 0)
                            {
                                running_no = "190203";
                                description = "Onsite service";
                                file_name = "190201_Service Report";
                            }
                            else if (i == 1)
                            {
                                running_no = "190202";
                                description = "Change part";
                                file_name = "190202_Part";
                            }
                            annualServiceFile.Add(new AnnualServiceFile()
                            {
                                service_master_id = annualServiceList.service_master_id,
                                id = i,
                                description = description,
                                file_name = file_name,
                                type = i + 1,
                                running_no = running_no,
                                is_deleted = false,
                            });
                        }*/
                    }
                }
                else
                {
                    var datadb = new DataSet();

                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                                new SqlParameter("@customer_id", SqlDbType.Int) { Value = Convert.ToInt32(customer_id) },
                                new SqlParameter("@customer_mfg_id", SqlDbType.Int) { Value = Convert.ToInt32(customer_mfg_id) },
                                new SqlParameter("@service_year_id", SqlDbType.Int) { Value = Convert.ToInt32(service_year_id) > 0 ? Convert.ToInt32(service_year_id):0 },
                        };
                        conn.Open();
                        datadb = SqlHelper.ExecuteDataset(conn, "sp_service_master_list", arrParm.ToArray());
                        if (datadb.Tables[0].Rows.Count > 0 && datadb != null)
                        {

                            var data = datadb.Tables[0].AsEnumerable().FirstOrDefault();
                            if (data != null)
                            {
                                annualServiceList.customer_id = Convert.IsDBNull(data["customer_id"]) ? 0 : Convert.ToInt32(data["customer_id"]);
                                annualServiceList.service_year_id = Convert.IsDBNull(data["service_year_id"]) ? 0 : Convert.ToInt32(data["service_year_id"]);
                                annualServiceList.type_of_contract_id = Convert.IsDBNull(data["type_of_contract_id"]) ? 0 : Convert.ToInt32(data["type_of_contract_id"]);
                                annualServiceList.type_name = Convert.IsDBNull(data["type_name"]) ? string.Empty : Convert.ToString(data["type_name"]);
                                annualServiceList.setting_date = Convert.IsDBNull(data["setting_date"]) ? string.Empty : Convert.ToDateTime(data["setting_date"]).ToString("dd/MM/yyyy");
                                annualServiceList.contractstarting_date = Convert.IsDBNull(data["contractstarting_date"]) ? string.Empty : Convert.ToDateTime(data["contractstarting_date"]).ToString("dd/MM/yyyy");
                                annualServiceList.contractexpire_date = Convert.IsDBNull(data["contractexpire_date"]) ? string.Empty : Convert.ToDateTime(data["contractexpire_date"]).ToString("dd/MM/yyyy");
                                annualServiceList.starting_date = Convert.IsDBNull(data["starting_date"]) ? string.Empty : Convert.ToDateTime(data["starting_date"]).ToString("dd/MM/yyyy");
                                annualServiceList.expire_date = Convert.IsDBNull(data["expire_date"]) ? string.Empty : Convert.ToDateTime(data["expire_date"]).ToString("dd/MM/yyyy");
                                annualServiceList.service_master_id = Convert.IsDBNull(data["id"]) ? 0 : Convert.ToInt32(data["id"]);
                                annualServiceList.customer_mfg_id = Convert.IsDBNull(data["customer_mfg_id"]) ? 0 : Convert.ToInt32(data["customer_mfg_id"]);
                                annualServiceList.time_of_contract = Convert.IsDBNull(data["time_of_contract"]) ? 0 : Convert.ToInt32(data["time_of_contract"]);

                                annualServiceList.is_deleted = Convert.ToBoolean(data["is_delete"]);

                            }

                            var dataDetail = (from t in datadb.Tables[1].AsEnumerable() select t).ToList();
                            if (dataDetail != null)
                            {

                                foreach (var row in dataDetail)
                                {
                                    annualServiceDetail.Add(new AnnualServiceDetail()
                                    {
                                        id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                        service_date = Convert.IsDBNull(row["service_date"]) ? string.Empty : Convert.ToDateTime(row["service_date"]).ToString("dd/MM/yyyy"),
                                        service_report_no = Convert.IsDBNull(row["service_report_no"]) ? string.Empty : row["service_report_no"].ToString(),
                                        checking_status = Convert.IsDBNull(row["checking_status"]) ? 0 : Convert.ToInt32(row["checking_status"]),
                                        status_name = Convert.ToBoolean(row["checking_status"]) == false ? "รอเข้าบริการ" : "ให้บริการเรียบร้อยแล้ว",
                                        service_master_id = annualServiceList.service_master_id,
                                        checking_date = Convert.IsDBNull(row["checking_date"]) ? string.Empty : Convert.ToDateTime(row["checking_date"]).ToString("dd/MM/yyyy"),
                                        checking_remark = Convert.IsDBNull(row["checking_remark"]) ? string.Empty : Convert.ToString(row["checking_remark"]),
                                        is_deleted = Convert.ToBoolean(row["is_delete"]),
                                    });
                                }
                            }

                            /*var detailFile = (from t in datadb.Tables[2].AsEnumerable() select t).ToList();
                            if (detailFile != null)
                            {

                                foreach (var row in detailFile)
                                {
                                    annualServiceFile.Add(new AnnualServiceFile()
                                    {
                                        service_master_id = annualServiceList.service_master_id,
                                        id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                        description = Convert.IsDBNull(row["description"]) ? string.Empty : Convert.ToString(row["description"]),
                                        file_name = Convert.IsDBNull(row["file_name"]) ? string.Empty : Convert.ToString(row["file_name"]),
                                        is_deleted = Convert.ToBoolean(row["is_delete"]),
                                    });
                                }
                            }*/

                        }
                    }
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }

            HttpContext.Current.Session["SESSION_ANNUALSERVICE_LIST"] = annualServiceList;
            HttpContext.Current.Session["SESSION_ANNUALSERVICE_DETAIL"] = annualServiceDetail;
            //HttpContext.Current.Session["SESSION_ANNUALSERVICE_FILE"] = annualServiceFile;
            return annualServiceList;
        }

        protected void GridViewmasterList_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridViewmasterList.DataSource = (from t in annualServiceDetail where t.is_deleted == false select t).ToList();
            GridViewmasterList.DataBind();
        }

        protected void BindGridViewmasterList()
        {
            GridViewmasterList.DataSource = (from t in annualServiceDetail where t.is_deleted == false select t).ToList();
            GridViewmasterList.DataBind();
        }

        protected void gridFile_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridFile.DataSource = (from t in annualServiceFile where t.is_deleted == false select t).ToList();
            gridFile.DataBind();
        }

        protected void gridFile_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "type")
            {
                ASPxLabel lblType = gridFile.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "lblType") as ASPxLabel;
                if (lblType != null)
                {
                    int type = Convert.ToInt32(e.GetValue("type"));
                    var typeDesc = "";
                    if (type == 1)
                    {
                        typeDesc = "Service";
                    }
                    else if (type == 2)
                    {
                        typeDesc = "Part Change";
                    }
                    else if (type == 3)
                    {
                        typeDesc = "Repairing";
                    }
                    else
                    {
                        typeDesc = "Test run/Contract";
                    }
                    lblType.Text = typeDesc;
                }
            }
        }

        [WebMethod]
        public static string EditserviceYear(string id, int service_time, string serviceFree, string remark, string setting_date, string starting_date, string expire_date, string service_type, string service_location
             ,string contractstarting_date, string contractexpire_date)
        {
            var annualServiceYear = (List<AnnualServiceYear>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_YEAR"];
            var idServiceYear = id == "" ? 0 : Convert.ToInt32(id);
            var dataServiceFree = serviceFree == "" ? 0 : Convert.ToDecimal(serviceFree);
            var dataRemark = remark == "" ? null : remark;
            var result = "";

            if (annualServiceYear.Count > 0)
            {
                //var checkExist = (from t in annualServiceYear
                //                  where t.service_year_id != idServiceYear && t.is_deleted == false && t.service_time == Convert.ToInt32(service_time)
                //                  select t).FirstOrDefault();
                
                //if (checkExist == null)
                //{
                    var row = (from t in annualServiceYear
                               where t.service_year_id == idServiceYear && t.is_deleted == false
                               select t).FirstOrDefault();

                    /*edit*/
                    row.service_time = service_time;
                    row.service_remark = dataRemark;
                    row.service_free = dataServiceFree;
                    row.service_type_text = service_type;
                    row.setting_date = setting_date;
                    row.starting_date = starting_date;
                    row.contractstarting_date = contractstarting_date;
                    row.contractexpire_date = contractexpire_date;
                    row.expire_date = expire_date;
                    row.service_type = service_type;
                    row.service_location = service_location;
                //}
                //else
                //{
                //    result = "error";
                //}

            }
            HttpContext.Current.Session["SESSION_ANNUALSERVICE_YEAR"] = annualServiceYear;


            return result;
        }

        [WebMethod]
        public static int GetLimitOfType(string id)
        {
            var data = new DataSet();
            var limit = 0;
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(id) },
                        };
                conn.Open();
                data = SqlHelper.ExecuteDataset(conn, "sp_annual_service_contract_limit", arrParm.FirstOrDefault());

                if (data.Tables.Count > 0)
                {


                    var result = data.Tables[0].AsEnumerable().FirstOrDefault();
                    limit = Convert.ToInt32(result["limit_of_type"]);
                }

            }
            return limit;
        }

        [WebMethod]
        public static int AddChecking(string id, string CheckingDate, string Remark, string service_master_id, string statusCheck, string limit_type, string service_date, string service_report_no)
        {
            var resule = 0;
            var annualServiceDetail = (List<AnnualServiceDetail>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_DETAIL"];
            var idSubmit = id == "" ? 0 : Convert.ToInt32(id);
            var master_id = id == "" ? 0 : Convert.ToInt32(service_master_id);
            var status = statusCheck == "" ? 0 : Convert.ToInt32(statusCheck);
            var limit = Convert.ToInt32(limit_type);
            if (annualServiceDetail != null)
            {
                var row = (from t in annualServiceDetail
                           where t.id == idSubmit && t.is_deleted == false
                           select t).FirstOrDefault();
                if (row == null)
                {
                    /*if (annualServiceDetail.Count >= limit)
                    {
                        resule = 1;
                    }
                    else
                    {*/
                        annualServiceDetail.Add(new AnnualServiceDetail()
                        {
                            id = (annualServiceDetail.Count + 1) * -1,
                            checking_date = CheckingDate,
                            service_date = service_date,
                            service_report_no = service_report_no,
                            checking_remark = Remark,
                            checking_status = status,
                            status_name = status == 0 ? "รอเข้าบริการ" : "ให้บริการเรียบร้อยแล้ว",
                            service_master_id = master_id,
                            limit_type = limit
                        });
                    //}


                }
                else
                { /*edit*/

                    row.checking_date = CheckingDate;
                    row.service_date = service_date;
                    row.service_report_no = service_report_no;
                    row.checking_remark = Remark;
                    row.checking_status = status;
                    row.status_name = status == 0 ? "รอเข้าบริการ" : "ให้บริการเรียบร้อยแล้ว";
                    row.service_master_id = master_id;
                    row.limit_type = limit;
                }

            }
            else
            {
                annualServiceDetail = new List<AnnualServiceDetail>();
                annualServiceDetail.Add(new AnnualServiceDetail()
                {
                    id = (annualServiceDetail.Count + 1) * -1,
                    checking_date = CheckingDate,
                    service_date = service_date,
                    service_report_no = service_report_no,
                    checking_remark = Remark,
                    checking_status = status,
                    status_name = status == 0 ? "รอเข้าบริการ" : "ให้บริการเรียบร้อยแล้ว",
                    service_master_id = master_id,
                    limit_type = limit,
                });


            }
            HttpContext.Current.Session["SESSION_ANNUALSERVICE_DETAIL"] = annualServiceDetail;
            return resule;
        }

        [WebMethod]
        public static int AutoAddChecking(String key, String limit_type)
        {
            var resule = 0;
            var annualServiceDetail = (List<AnnualServiceDetail>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_DETAIL"];
            if (annualServiceDetail == null)
            {
                annualServiceDetail = new List<AnnualServiceDetail>();
            }
            else
            {
                annualServiceDetail.Clear();
            }

            //  Auto add by its data
            AnnualServiceYear obj = (AnnualServiceYear)HttpContext.Current.Session["SESSION_ANNUALSERVICE_YEAR_SELECTED"];
            int month = (12 * Convert.ToInt16(obj.service_free)) / obj.time_of_contract;
            var limit = Convert.ToInt32(limit_type);

            DateTime starting_date = DateTime.ParseExact(obj.starting_date.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);

            for (int i = 0; i < obj.time_of_contract; i++)
            {
                var date = starting_date.AddMonths(i * month);
                var service_date = date.ToString("dd/MM/yyyy");
                annualServiceDetail.Add(new AnnualServiceDetail()
                {
                    id = (annualServiceDetail.Count + 1) * -1,
                    checking_date = service_date,
                    service_date = service_date,
                    service_report_no = "",
                    checking_remark = "",
                    checking_status = 0,
                    status_name = "รอเข้าบริการ",
                    service_master_id = Convert.ToInt32(key),
                    limit_type = limit
                });
            }

            HttpContext.Current.Session["SESSION_ANNUALSERVICE_DETAIL"] = annualServiceDetail;
            return resule;
        }

        [WebMethod]
        public static AnnualServiceDetail SelectEditChecking(int id)
        {
            var annualServiceDetail = (List<AnnualServiceDetail>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_DETAIL"];
            if (annualServiceDetail != null)
            {
                var row = (from t in annualServiceDetail where t.id == id && t.is_deleted == false select t).FirstOrDefault();
                if (row != null)
                {
                    return row;
                }
            }
            return new AnnualServiceDetail(); // ไม่มี Data
        }
        [WebMethod]
        public static AnnualServiceYear SelectServiceYear(int id, int service_master_id)
        {
            var annualServiceYear = (List<AnnualServiceYear>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_YEAR"];
            if (annualServiceYear != null)
            {
                var row = (from t in annualServiceYear where t.service_year_id == id && t.service_master_id == service_master_id && t.is_deleted == false select t).FirstOrDefault();
                if (row != null)
                {
                    return row;
                }
            }
            return new AnnualServiceYear(); // ไม่มี Data
        }

        [WebMethod]
        public static void GetFile(int schedule_id, int service_master_id)
        {
            var annualServiceFile = (List<AnnualServiceFile>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_FILE"];
            if (annualServiceFile != null)
            {
                annualServiceFile.Clear();
            }

            var dsResult = new DataSet();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                List<SqlParameter> arrParm = new List<SqlParameter>
                {
                    new SqlParameter("@service_master_id", SqlDbType.Int) { Value = service_master_id },
                    new SqlParameter("@schedule_id", SqlDbType.Int) { Value = schedule_id },
                };
                conn.Open();
                dsResult = SqlHelper.ExecuteDataset(conn, "sp_service_schedule_file_list", arrParm.ToArray());
                conn.Close();
            }

            if (dsResult != null)
            {
                var data = (from t in dsResult.Tables[0].AsEnumerable() select t).ToList();
                if (data != null)
                {
                    foreach (var row in data)
                    {
                        annualServiceFile.Add(new AnnualServiceFile()
                        {
                            service_master_id = Convert.IsDBNull(row["service_master_id"]) ? 0 : Convert.ToInt32(row["service_schedule_id"]),
                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                            description = Convert.IsDBNull(row["description"]) ? string.Empty : Convert.ToString(row["description"]),
                            file_name = Convert.IsDBNull(row["file_name"]) ? string.Empty : Convert.ToString(row["file_name"]),
                            running_no = Convert.IsDBNull(row["running_no"]) ? string.Empty : Convert.ToString(row["running_no"]),
                            schedule_id = Convert.IsDBNull(row["service_schedule_id"]) ? 0 : Convert.ToInt32(row["service_schedule_id"]),
                            type = Convert.IsDBNull(row["type"]) ? 0 : Convert.ToInt32(row["type"]),
                            starting_date = Convert.IsDBNull(row["starting_date"]) ? string.Empty : Convert.ToString(row["starting_date"]),
                            mfg_number = Convert.IsDBNull(row["mfg_number"]) ? string.Empty : Convert.ToString(row["mfg_number"]),
                            model_number = Convert.IsDBNull(row["model_number"]) ? string.Empty : Convert.ToString(row["model_number"]),
                            is_deleted = Convert.ToBoolean(row["is_delete"]),
                        });
                    }
                }
            }
            HttpContext.Current.Session["SESSION_ANNUALSERVICE_FILE"] = annualServiceFile;
        }

        protected void upload_FilePdfUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            var neme = e.UploadedFile;
            var file_name_upload = neme.FileName;
            //Guid guid = Guid.NewGuid();
            var fileName = file_name_upload;//String.Format("{0}_" + guid + ".pdf", ConstantClass.SESSION_USER_ID);
            string path = Page.MapPath("../Doc_pdf/") + fileName;
            string serverPath = e.CallbackData;
            bool exists = System.IO.Directory.Exists(Server.MapPath("/Doc_pdf"));

            if (!exists)
                System.IO.Directory.CreateDirectory(Server.MapPath("/Doc_pdf"));

            e.UploadedFile.SaveAs(path);

            if (annualServiceFile == null)
            {
                annualServiceFile = new List<AnnualServiceFile>();
            }
            int genId = (annualServiceFile.Count + 1) * -1;
            annualServiceFile.Add(new AnnualServiceFile()
            {
                // description = txtFileName.Value,
                file_name = fileName,
                id = genId,
                file_name_upload = file_name_upload
            });

            e.CallbackData = Convert.ToString(genId);
        }

        [WebMethod]
        public static string UpdateDescriptionFile(int id, string description, string running_no, int type, int service_master_id, int schedule_id,string starting_date , string mfg_number , string model_number)
        {
            var strReturn = string.Empty;
            var annualServiceFileList = (List<AnnualServiceFile>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_FILE"];
            if (annualServiceFileList != null)
            {
                var row = (from t in annualServiceFileList where t.id == id select t).FirstOrDefault();
                if (row != null)
                {
                    row.description = description;
                    row.running_no = running_no;
                    row.type = type;
                    row.starting_date = starting_date;
                    row.mfg_number = mfg_number;
                    row.model_number = model_number;
                    row.service_master_id = service_master_id;
                    row.schedule_id = schedule_id;
                }
            }
            HttpContext.Current.Session["SESSION_ANNUALSERVICE_FILE"] = annualServiceFileList;
            return strReturn;
        }

        //[WebMethod]
        //public static void SubmitMasterData(AnnualServiceList[] masterData)
        //{

        //    var dataMasterList = (from t in masterData select t).FirstOrDefault();

        //    if (dataMasterList != null)
        //    {
        //        var checkExist = (from t in dataMasterList
        //                          where t.service_time == Convert.ToInt32(service_time)
        //                          select t).FirstOrDefault();
        //        if (checkExist == null)
        //        {
        //            dataMasterList.Add(new dataMasterList()
        //            {
        //                service_year_id = (dataMasterList.Count + 1) * -1,
        //                service_time = Convert.ToInt32(service_time),
        //                service_free = Convert.ToInt32(service_free),
        //                service_remark = service_remark,
        //                starting_date = string.Empty,
        //                setting_date = string.Empty,
        //                expire_date = string.Empty,
        //                customer_id = Convert.ToInt32(customer_id),
        //                customer_mfg_id = Convert.ToInt32(customer_mfg_id)
        //            });
        //        }

        //    }
        //    else
        //    {
        //        dataMasterList.Add(new dataMasterList()
        //        {
        //            service_year_id = (dataMasterList.Count + 1) * -1,
        //            service_time = Convert.ToInt32(service_time),
        //            service_free = Convert.ToInt32(service_free),
        //            service_remark = service_remark,
        //            starting_date = string.Empty,
        //            setting_date = string.Empty,
        //            expire_date = string.Empty,
        //            customer_id = Convert.ToInt32(customer_id),
        //            customer_mfg_id = Convert.ToInt32(customer_mfg_id)
        //        });
        //    }
        //    HttpContext.Current.Session["SESSION_ANNUALSERVICE_SAVE"] = dataMasterList;

        //}
        [WebMethod]
        public static int SubmitServiceYear()
        {
            var mfg_id = 0;
            var strReturn = string.Empty;
            var annualServiceYear = (List<AnnualServiceYear>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_YEAR"];
            var dataAnnualServiceYear = (from t in annualServiceYear select t).ToList();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {

                    if (dataAnnualServiceYear.Count > 0)
                    {
                        try
                        {
                            foreach (var data in dataAnnualServiceYear)
                            {
                                var serviceType = data.service_type == "Free Service" ? 1 : data.service_type == "Contract" ? 2 : 3;
                                mfg_id = data.customer_mfg_id;
                                #region Delete service_year , Delete service_master
                                if (data.service_year_id > 0 && data.is_deleted == true) //delete service_year
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_annual_service_year_delete", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = data.service_year_id;
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();
                                    }

                                }
                                #endregion

                                #region Edit service_year ,(service_master Edit/Add)
                                if (data.service_year_id > 0 && data.is_deleted == false) //delete service_year
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_annual_service_year_edit", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = data.service_year_id;
                                        cmd.Parameters.Add("@service_time", SqlDbType.Int).Value = Convert.ToInt32(data.service_time);
                                        cmd.Parameters.Add("@service_free", SqlDbType.Decimal).Value = data.service_free;
                                        cmd.Parameters.Add("@service_remark", SqlDbType.VarChar, 500).Value = data.service_remark;
                                        cmd.Parameters.Add("@service_location", SqlDbType.VarChar, 500).Value = data.service_location;
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();
                                    }
                                    #region ServiceMaster_Edit
                                    if (data.service_master_id > 0)
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_annual_service_master_edit", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = data.service_master_id;
                                            cmd.Parameters.Add("@time_of_contract", SqlDbType.Int).Value = Convert.ToInt32(data.time_of_contract);
                                            cmd.Parameters.Add("@service_type", SqlDbType.Int).Value = Convert.ToInt32(serviceType);
                                            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToInt32(data.customer_id);
                                            cmd.Parameters.Add("@customer_mfg_id", SqlDbType.Int).Value = Convert.ToInt32(data.customer_mfg_id);
                                            cmd.Parameters.Add("@service_year_id", SqlDbType.Int).Value = Convert.ToInt32(data.service_year_id);
                                            cmd.Parameters.Add("@type_of_contract_id", SqlDbType.Int).Value = Convert.ToInt32(data.type_of_contract_id);
                                            if (!String.IsNullOrEmpty(data.setting_date))
                                            {
                                                cmd.Parameters.Add("@setting_date", SqlDbType.DateTime).Value = DateTime.ParseExact(data.setting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            }
                                            if (!String.IsNullOrEmpty(data.contractstarting_date))
                                            {
                                                cmd.Parameters.Add("@contractstarting_date", SqlDbType.Date).Value = DateTime.ParseExact(data.contractstarting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            }
                                            if (!String.IsNullOrEmpty(data.contractexpire_date))
                                            {
                                                cmd.Parameters.Add("@contractexpire_date", SqlDbType.Date).Value = DateTime.ParseExact(data.contractexpire_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            }
                                            if (!String.IsNullOrEmpty(data.starting_date))
                                            {
                                                cmd.Parameters.Add("@starting_date", SqlDbType.DateTime).Value = DateTime.ParseExact(data.starting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            }
                                            cmd.Parameters.Add("@expire_date", SqlDbType.DateTime).Value = DateTime.ParseExact(data.expire_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    #endregion

                                    #region ServiceMaster_Add
                                    else if (data.service_master_id < 0)
                                    {
                                        int newMasterId = DataListUtil.emptyEntryID;
                                        using (SqlCommand cmd = new SqlCommand("sp_annual_service_master_add", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@time_of_contract", SqlDbType.Int).Value = Convert.ToInt32(data.time_of_contract);
                                            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToInt32(data.customer_id);
                                            cmd.Parameters.Add("@customer_mfg_id", SqlDbType.Int).Value = Convert.ToInt32(data.customer_mfg_id);
                                            cmd.Parameters.Add("@service_type", SqlDbType.Int).Value = Convert.ToInt32(serviceType);
                                            cmd.Parameters.Add("@service_year_id", SqlDbType.Int).Value = Convert.ToInt32(data.service_year_id);
                                            cmd.Parameters.Add("@type_of_contract_id", SqlDbType.Int).Value = Convert.ToInt32(data.type_of_contract_id);
                                            cmd.Parameters.Add("@setting_date", SqlDbType.Date).Value = DateTime.ParseExact(data.setting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            cmd.Parameters.Add("@contractstarting_date", SqlDbType.Date).Value = DateTime.ParseExact(data.contractstarting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            cmd.Parameters.Add("@contractexpire_date", SqlDbType.Date).Value = DateTime.ParseExact(data.contractexpire_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            cmd.Parameters.Add("@starting_date", SqlDbType.Date).Value = DateTime.ParseExact(data.starting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            cmd.Parameters.Add("@expire_date", SqlDbType.Date).Value = DateTime.ParseExact(data.expire_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                            newMasterId = Convert.ToInt32(cmd.ExecuteScalar());
                                        }
                                    }

                                    #endregion
                                }
                                #endregion

                                #region Add Annual Service Year,Add Service Master
                                if (data.service_year_id < 0 && data.is_deleted == false)
                                {

                                    //ServiceYear Add
                                    #region ServiceYear_Add
                                    int newServiceYearId = DataListUtil.emptyEntryID;

                                    using (SqlCommand cmd = new SqlCommand("sp_annual_service_year_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToInt32(data.customer_id);
                                        cmd.Parameters.Add("@customer_mfg_id", SqlDbType.Int).Value = Convert.ToInt32(data.customer_mfg_id);
                                        cmd.Parameters.Add("@service_time", SqlDbType.Int).Value = Convert.ToInt32(data.service_time);
                                        cmd.Parameters.Add("@service_free", SqlDbType.Decimal).Value = data.service_free;

                                        cmd.Parameters.Add("@service_remark", SqlDbType.VarChar, 500).Value = data.service_remark;
                                        cmd.Parameters.Add("@service_location", SqlDbType.VarChar, 500).Value = data.service_location;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        newServiceYearId = Convert.ToInt32(cmd.ExecuteScalar());
                                    }
                                    #endregion

                                    #region ServiceMaster_Add
                                    int newID = DataListUtil.emptyEntryID;

                                    using (SqlCommand cmd = new SqlCommand("sp_annual_service_master_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@time_of_contract", SqlDbType.Int).Value = Convert.ToInt32(data.time_of_contract);
                                        cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToInt32(data.customer_id);
                                        cmd.Parameters.Add("@customer_mfg_id", SqlDbType.Int).Value = Convert.ToInt32(data.customer_mfg_id);
                                        cmd.Parameters.Add("@service_type", SqlDbType.Int).Value = Convert.ToInt32(serviceType);
                                        cmd.Parameters.Add("@service_year_id", SqlDbType.Int).Value = newServiceYearId;
                                        cmd.Parameters.Add("@type_of_contract_id", SqlDbType.Int).Value = Convert.ToInt32(data.type_of_contract_id);
                                        if (!String.IsNullOrEmpty(data.setting_date))
                                        {
                                            cmd.Parameters.Add("@setting_date", SqlDbType.Date).Value = DateTime.ParseExact(data.setting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        if (!String.IsNullOrEmpty(data.contractstarting_date))
                                        {
                                            cmd.Parameters.Add("@contractstarting_date", SqlDbType.Date).Value = DateTime.ParseExact(data.contractstarting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        if (!String.IsNullOrEmpty(data.contractexpire_date))
                                        {
                                            cmd.Parameters.Add("@contractexpire_date", SqlDbType.Date).Value = DateTime.ParseExact(data.contractexpire_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        if (!String.IsNullOrEmpty(data.starting_date))
                                        {
                                            cmd.Parameters.Add("@starting_date", SqlDbType.Date).Value = DateTime.ParseExact(data.starting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        if (!String.IsNullOrEmpty(data.expire_date))
                                        {
                                            cmd.Parameters.Add("@expire_date", SqlDbType.Date).Value = DateTime.ParseExact(data.expire_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        newID = Convert.ToInt32(cmd.ExecuteScalar());
                                    }
                                    #endregion

                                }
                                #endregion 
                            }

                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            tran.Dispose();
                            conn.Close();

                        }
                        finally
                        {
                            if (!conn.State.Equals(ConnectionState.Closed))
                            {
                                tran.Commit();
                                tran.Dispose();
                                conn.Close();
                            }
                        }

                    }
                    else
                    {
                        return mfg_id;
                    }
                }
            }

            return mfg_id;
        }

        [WebMethod]
        public static int SubmitData(AnnualServiceList[] masterData)
        {
            var complete = 0;
            var dataMasterList = (from t in masterData select t).FirstOrDefault();

            var annualServiceYear = (List<AnnualServiceYear>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_YEAR"];
            if (annualServiceYear != null)
            {
                var dataAnnualServiceYear = (from t in annualServiceYear where dataMasterList.service_year_id == t.service_year_id select t).FirstOrDefault();
                var serviceType = dataAnnualServiceYear.service_type == "Free Service" ? 1 : dataAnnualServiceYear.service_type == "Contract" ? 2 : 3;
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        if (dataAnnualServiceYear != null) //Check data AnnualServiceYear ว่ามีค่าไหม?
                        {
                            try
                            {
                                //  // start // //
                                if (dataAnnualServiceYear.service_year_id > 0 && dataAnnualServiceYear.is_deleted == false) ////mode Edit => have service_year_id
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_annual_service_year_edit", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = dataAnnualServiceYear.service_year_id;
                                        cmd.Parameters.Add("@service_time", SqlDbType.Int).Value = Convert.ToInt32(dataAnnualServiceYear.service_time);
                                        cmd.Parameters.Add("@service_free", SqlDbType.Decimal).Value = dataAnnualServiceYear.service_free;
                                        cmd.Parameters.Add("@service_location", SqlDbType.VarChar, 500).Value = dataAnnualServiceYear.service_location;
                                        cmd.Parameters.Add("@service_remark", SqlDbType.VarChar, 500).Value = dataAnnualServiceYear.service_remark;
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();
                                    }
                                    #region have service_year_id  

                                    //ServiceMaster Add 
                                    #region ServiceMaster_Add
                                    int newID = DataListUtil.emptyEntryID;

                                    using (SqlCommand cmd = new SqlCommand("sp_annual_service_master_edit", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = dataMasterList.service_master_id;
                                        cmd.Parameters.Add("@time_of_contract", SqlDbType.Int).Value = Convert.ToInt32(dataAnnualServiceYear.time_of_contract);
                                        cmd.Parameters.Add("@service_type", SqlDbType.Int).Value = Convert.ToInt32(serviceType);
                                        cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToInt32(dataMasterList.customer_id);
                                        cmd.Parameters.Add("@customer_mfg_id", SqlDbType.Int).Value = Convert.ToInt32(dataMasterList.customer_mfg_id);
                                        cmd.Parameters.Add("@service_year_id", SqlDbType.Int).Value = Convert.ToInt32(dataAnnualServiceYear.service_year_id);
                                        cmd.Parameters.Add("@type_of_contract_id", SqlDbType.Int).Value = Convert.ToInt32(dataAnnualServiceYear.type_of_contract_id);

                                        if (!String.IsNullOrEmpty(dataAnnualServiceYear.setting_date))
                                        {
                                            cmd.Parameters.Add("@setting_date", SqlDbType.DateTime).Value = DateTime.ParseExact(dataAnnualServiceYear.setting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        if (!String.IsNullOrEmpty(dataAnnualServiceYear.contractstarting_date))
                                        {
                                            cmd.Parameters.Add("@contractstarting_date", SqlDbType.Date).Value = DateTime.ParseExact(dataAnnualServiceYear.contractstarting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        if (!String.IsNullOrEmpty(dataAnnualServiceYear.contractexpire_date))
                                        {
                                            cmd.Parameters.Add("@contractexpire_date", SqlDbType.Date).Value = DateTime.ParseExact(dataAnnualServiceYear.contractexpire_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        if (!String.IsNullOrEmpty(dataAnnualServiceYear.starting_date))
                                        {
                                            cmd.Parameters.Add("@starting_date", SqlDbType.DateTime).Value = DateTime.ParseExact(dataAnnualServiceYear.starting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        cmd.Parameters.Add("@expire_date", SqlDbType.DateTime).Value = DateTime.ParseExact(dataAnnualServiceYear.expire_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();
                                    }
                                    #endregion

                                    //Shedule
                                    #region Shedule_Add
                                    var annualServiceDetail = (List<AnnualServiceDetail>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_DETAIL"];
                                    if (annualServiceDetail != null)
                                    {
                                        var row = (from t in annualServiceDetail select t).ToList();//where t.is_deleted == false select t
                                        if (row != null)
                                        {

                                            foreach (var data in row)
                                            {
                                                if (data.id > 0)
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("sp_annual_service_schedule_edit", conn, tran))
                                                    {
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = data.id;
                                                        cmd.Parameters.Add("@service_master_id", SqlDbType.Int).Value = dataMasterList.service_master_id;
                                                        cmd.Parameters.Add("@checking_date", SqlDbType.Date).Value = DateTime.ParseExact(data.@checking_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                        cmd.Parameters.Add("@service_date", SqlDbType.Date).Value = DateTime.ParseExact(data.@service_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                        cmd.Parameters.Add("@service_report_no", SqlDbType.VarChar, 200).Value = data.service_report_no;// DateTime.ParseExact(data.@service_report_no, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                        cmd.Parameters.Add("@checking_status", SqlDbType.Bit).Value = data.checking_status;
                                                        cmd.Parameters.Add("@checking_remark", SqlDbType.VarChar, 200).Value = data.checking_remark;
                                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                        cmd.ExecuteNonQuery();

                                                    }
                                                }
                                                else if (data.is_deleted)
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("sp_annual_service_schedule_delete", conn, tran))
                                                    {
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = data.id;
                                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                                else
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("sp_annual_service_schedule_add", conn, tran))
                                                    {
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add("@service_master_id", SqlDbType.Int).Value = dataMasterList.service_master_id;
                                                        cmd.Parameters.Add("@checking_date", SqlDbType.Date).Value = DateTime.ParseExact(data.@checking_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                        cmd.Parameters.Add("@checking_status", SqlDbType.Bit).Value = data.checking_status;
                                                        cmd.Parameters.Add("@checking_remark", SqlDbType.VarChar, 200).Value = data.checking_remark;
                                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    #endregion

                                    #endregion
                                }
                                else if (dataAnnualServiceYear.service_year_id > 0 && dataAnnualServiceYear.is_deleted == true)
                                {
                                    #region ServiceYear_delete
                                    using (SqlCommand cmd = new SqlCommand("sp_annual_service_year_delete", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = dataAnnualServiceYear.service_year_id;
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();
                                    }
                                    #endregion
                                }
                                else if (dataAnnualServiceYear.service_year_id < 0 && dataAnnualServiceYear.is_deleted == false) ////Mode ADD ALL => haven't service_year_id
                                {
                                    //ServiceYear Add
                                    #region ServiceYear_Add
                                    /*int newServiceYearId = DataListUtil.emptyEntryID;

                                    using (SqlCommand cmd = new SqlCommand("sp_annual_service_year_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToInt32(dataAnnualServiceYear.customer_id);
                                        cmd.Parameters.Add("@customer_mfg_id", SqlDbType.Int).Value = Convert.ToInt32(dataAnnualServiceYear.customer_mfg_id);
                                        cmd.Parameters.Add("@service_time", SqlDbType.Int).Value = Convert.ToInt32(dataAnnualServiceYear.service_time);
                                        cmd.Parameters.Add("@service_free", SqlDbType.Int).Value = Convert.ToInt32(dataAnnualServiceYear.service_free);
                                        cmd.Parameters.Add("@service_remark", SqlDbType.VarChar, 500).Value = dataAnnualServiceYear.service_remark;
                                        cmd.Parameters.Add("@service_location", SqlDbType.VarChar, 500).Value = dataAnnualServiceYear.service_location;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        newServiceYearId = Convert.ToInt32(cmd.ExecuteScalar());
                                    }*/
                                    #endregion

                                    //  Add other
                                    #region ServiceYear_Add
                                    int newID = DataListUtil.emptyEntryID;
                                    var row = (from t in annualServiceYear where t.service_year_id < 0 select t).ToList();
                                    if (row != null)
                                    {
                                        foreach (var data in row)
                                        {
                                            int newServiceYearId2 = DataListUtil.emptyEntryID;
                                            using (SqlCommand cmd = new SqlCommand("sp_annual_service_year_add", conn, tran))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToInt32(data.customer_id);
                                                cmd.Parameters.Add("@customer_mfg_id", SqlDbType.Int).Value = Convert.ToInt32(data.customer_mfg_id);
                                                cmd.Parameters.Add("@service_time", SqlDbType.Int).Value = Convert.ToInt32(data.service_time);
                                                cmd.Parameters.Add("@service_free", SqlDbType.Decimal).Value = data.service_free;
                                                cmd.Parameters.Add("@service_remark", SqlDbType.VarChar, 500).Value = data.service_remark;
                                                cmd.Parameters.Add("@service_location", SqlDbType.VarChar, 500).Value = data.service_location;
                                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                newServiceYearId2 = Convert.ToInt32(cmd.ExecuteScalar());
                                            }

                                            int newID2 = DataListUtil.emptyEntryID;
                                            using (SqlCommand cmd = new SqlCommand("sp_annual_service_master_add", conn, tran))
                                            {
                                                var serviceType2 = data.service_type == "Free Service" ? 1 : data.service_type == "Contract" ? 2 : 3;

                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add("@time_of_contract", SqlDbType.Int).Value = Convert.ToInt32(data.time_of_contract);
                                                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToInt32(data.customer_id);
                                                cmd.Parameters.Add("@customer_mfg_id", SqlDbType.Int).Value = Convert.ToInt32(data.customer_mfg_id);
                                                cmd.Parameters.Add("@service_type", SqlDbType.Int).Value = Convert.ToInt32(serviceType2);
                                                cmd.Parameters.Add("@service_year_id", SqlDbType.Int).Value = newServiceYearId2;
                                                cmd.Parameters.Add("@type_of_contract_id", SqlDbType.Int).Value = Convert.ToInt32(data.type_of_contract_id);

                                                if (!String.IsNullOrEmpty(data.setting_date))
                                                {
                                                    cmd.Parameters.Add("@setting_date", SqlDbType.DateTime).Value = DateTime.ParseExact(data.setting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                }
                                                if (!String.IsNullOrEmpty(data.contractstarting_date))
                                                {
                                                    cmd.Parameters.Add("@contractstarting_date", SqlDbType.Date).Value = DateTime.ParseExact(data.contractstarting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                }
                                                if (!String.IsNullOrEmpty(data.contractexpire_date))
                                                {
                                                    cmd.Parameters.Add("@contractexpire_date", SqlDbType.Date).Value = DateTime.ParseExact(data.contractexpire_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                }
                                                if (!String.IsNullOrEmpty(data.starting_date))
                                                {
                                                    cmd.Parameters.Add("@starting_date", SqlDbType.DateTime).Value = DateTime.ParseExact(data.starting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                }
                                                cmd.Parameters.Add("@expire_date", SqlDbType.DateTime).Value = DateTime.ParseExact(data.expire_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                                newID2 = Convert.ToInt32(cmd.ExecuteScalar());
                                            }

                                            if (data.service_year_id == dataMasterList.service_year_id)
                                            {
                                                newID = newID2;
                                            }
                                        }
                                    }


                                    #endregion

                                    //ServiceMaster
                                    #region ServiceMaster_Add
                                    /*int newID = DataListUtil.emptyEntryID;

                                    using (SqlCommand cmd = new SqlCommand("sp_annual_service_master_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@time_of_contract", SqlDbType.Int).Value = Convert.ToInt32(dataMasterList.time_of_contract);
                                        cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToInt32(dataMasterList.customer_id);
                                        cmd.Parameters.Add("@customer_mfg_id", SqlDbType.Int).Value = Convert.ToInt32(dataMasterList.customer_mfg_id);
                                        cmd.Parameters.Add("@service_type", SqlDbType.Int).Value = Convert.ToInt32(serviceType);
                                        cmd.Parameters.Add("@service_year_id", SqlDbType.Int).Value = newServiceYearId;
                                        cmd.Parameters.Add("@type_of_contract_id", SqlDbType.Int).Value = Convert.ToInt32(dataAnnualServiceYear.type_of_contract_id);

                                        if (!String.IsNullOrEmpty(dataAnnualServiceYear.setting_date))
                                        {
                                            cmd.Parameters.Add("@setting_date", SqlDbType.DateTime).Value = DateTime.ParseExact(dataAnnualServiceYear.setting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        if (!String.IsNullOrEmpty(dataAnnualServiceYear.starting_date))
                                        {
                                            cmd.Parameters.Add("@starting_date", SqlDbType.DateTime).Value = DateTime.ParseExact(dataAnnualServiceYear.starting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        cmd.Parameters.Add("@expire_date", SqlDbType.DateTime).Value = DateTime.ParseExact(dataAnnualServiceYear.expire_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        newID = Convert.ToInt32(cmd.ExecuteScalar());
                                    }*/
                                    #endregion
                                    //Shedule
                                    #region Shedule_Add
                                    var annualServiceDetail = (List<AnnualServiceDetail>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_DETAIL"];
                                    if (annualServiceDetail != null)
                                    {
                                        var list = (from t in annualServiceDetail where t.is_deleted == false select t).ToList();
                                        if (list != null)
                                        {

                                            foreach (var data in list)
                                            {
                                                using (SqlCommand cmd = new SqlCommand("sp_annual_service_schedule_add", conn, tran))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add("@service_master_id", SqlDbType.Int).Value = newID;
                                                    cmd.Parameters.Add("@checking_date", SqlDbType.Date).Value = DateTime.ParseExact(data.@checking_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                    cmd.Parameters.Add("@checking_status", SqlDbType.Bit).Value = data.checking_status;
                                                    cmd.Parameters.Add("@checking_remark", SqlDbType.VarChar, 200).Value = data.checking_remark;
                                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }

                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                tran.Dispose();
                                conn.Close();

                            }
                            finally
                            {
                                if (!conn.State.Equals(ConnectionState.Closed))
                                {
                                    tran.Commit();
                                    tran.Dispose();
                                    conn.Close();
                                }
                            }

                        }

                    }
                }
            }
            complete = dataMasterList.customer_id;

            return complete;
        }

        [WebMethod]
        public static int SubmitFile(AnnualServiceList[] masterData)
        {
            var complete = 0;
            var dataMasterList = (from t in masterData select t).FirstOrDefault();

            var annualServiceYear = (List<AnnualServiceYear>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_YEAR"];
            if (annualServiceYear != null)
            {
                var dataAnnualServiceYear = (from t in annualServiceYear where dataMasterList.service_year_id == t.service_year_id select t).FirstOrDefault();
                var serviceType = dataAnnualServiceYear.service_type == "Free Service" ? 1 : dataAnnualServiceYear.service_type == "Contract" ? 2 : 3;
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        if (dataAnnualServiceYear != null) //Check data AnnualServiceYear ว่ามีค่าไหม?
                        {
                            try
                            {
                                //  // start // //
                                if (dataAnnualServiceYear.service_year_id > 0 && dataAnnualServiceYear.is_deleted == false) ////mode Edit => have service_year_id
                                {
                                    //File PDF
                                    #region FilePDF
                                    var annualServiceFile = (List<AnnualServiceFile>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_FILE"];
                                    if (annualServiceFile != null)
                                    {
                                        var row = (from t in annualServiceFile select t).ToList();
                                        if (row != null)
                                        {

                                            foreach (var dataFile in row)
                                            {
                                                if (dataFile.id < 0 && dataFile.is_deleted == false)
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("sp_annual_service_attach_file_add", conn, tran))
                                                    {
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add("@service_master_id", SqlDbType.Int).Value = dataFile.service_master_id;
                                                        cmd.Parameters.Add("@schedule_id", SqlDbType.Int).Value = dataFile.schedule_id;
                                                        cmd.Parameters.Add("@description", SqlDbType.VarChar, 100).Value = dataFile.description;
                                                        cmd.Parameters.Add("@file_name", SqlDbType.VarChar, 100).Value = dataFile.file_name;
                                                        cmd.Parameters.Add("@running_no", SqlDbType.VarChar, 100).Value = dataFile.running_no;
                                                        cmd.Parameters.Add("@file_name_upload", SqlDbType.VarChar, 500).Value = dataFile.file_name_upload;
                                                        cmd.Parameters.Add("@type", SqlDbType.Int).Value = dataFile.type;
                                                        if (!String.IsNullOrEmpty(dataFile.starting_date))
                                                        {
                                                            cmd.Parameters.Add("@starting_date", SqlDbType.DateTime).Value = DateTime.ParseExact(dataFile.starting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                        }
                                                        cmd.Parameters.Add("@mfg_number", SqlDbType.VarChar,100).Value = dataFile.mfg_number;
                                                        cmd.Parameters.Add("@model_number", SqlDbType.VarChar,100).Value = dataFile.model_number;
                                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                                else
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("sp_annual_service_attach_file_edit", conn, tran))
                                                    {
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = dataFile.id;
                                                        cmd.Parameters.Add("@schedule_id", SqlDbType.Int).Value = dataFile.schedule_id;
                                                        cmd.Parameters.Add("@description", SqlDbType.VarChar, 100).Value = dataFile.description;
                                                        cmd.Parameters.Add("@running_no", SqlDbType.VarChar, 100).Value = dataFile.running_no;
                                                        cmd.Parameters.Add("@type", SqlDbType.Int).Value = dataFile.type;
                                                        if (!String.IsNullOrEmpty(dataFile.starting_date))
                                                        {
                                                            cmd.Parameters.Add("@starting_date", SqlDbType.DateTime).Value = DateTime.ParseExact(dataFile.starting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                        }
                                                        cmd.Parameters.Add("@mfg_number", SqlDbType.VarChar, 100).Value = dataFile.mfg_number;
                                                        cmd.Parameters.Add("@model_number", SqlDbType.VarChar, 100).Value = dataFile.model_number;
                                                        cmd.Parameters.Add("@is_delete", SqlDbType.Bit, 100).Value = dataFile.is_deleted;
                                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                        cmd.ExecuteNonQuery();

                                                    }
                                                }

                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else if (dataAnnualServiceYear.service_year_id < 0 && dataAnnualServiceYear.is_deleted == false) ////Mode ADD ALL => haven't service_year_id
                                {
                                    //File PDF
                                    #region FilePDF_Add
                                    var annualServiceFile = (List<AnnualServiceFile>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_FILE"];
                                    if (annualServiceFile != null)
                                    {
                                        var rowFile = (from t in annualServiceFile select t).ToList();
                                        if (rowFile != null)
                                        {
                                            foreach (var dataFile in rowFile)
                                            {
                                                using (SqlCommand cmd = new SqlCommand("sp_annual_service_attach_file_add", conn, tran))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add("@service_master_id", SqlDbType.Int).Value = dataFile.service_master_id;
                                                    cmd.Parameters.Add("@schedule_id", SqlDbType.Int).Value = dataFile.schedule_id;
                                                    cmd.Parameters.Add("@description", SqlDbType.VarChar, 100).Value = dataFile.description;
                                                    cmd.Parameters.Add("@file_name", SqlDbType.VarChar, 100).Value = dataFile.file_name;
                                                    cmd.Parameters.Add("@running_no", SqlDbType.VarChar, 100).Value = dataFile.running_no;
                                                    cmd.Parameters.Add("@file_name_upload", SqlDbType.VarChar, 500).Value = dataFile.file_name_upload;
                                                    cmd.Parameters.Add("@type", SqlDbType.Int).Value = dataFile.type;
                                                    if (!String.IsNullOrEmpty(dataFile.starting_date))
                                                    {
                                                        cmd.Parameters.Add("@starting_date", SqlDbType.DateTime).Value = DateTime.ParseExact(dataFile.starting_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                    }
                                                    cmd.Parameters.Add("@mfg_number", SqlDbType.VarChar, 100).Value = dataFile.mfg_number;
                                                    cmd.Parameters.Add("@model_number", SqlDbType.VarChar, 100).Value = dataFile.model_number;
                                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }

                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                tran.Dispose();
                                conn.Close();

                            }
                            finally
                            {
                                if (!conn.State.Equals(ConnectionState.Closed))
                                {
                                    tran.Commit();
                                    tran.Dispose();
                                    conn.Close();
                                }
                            }

                        }

                    }
                }
            }
            complete = dataMasterList.customer_id;

            return complete;
        }

        [WebMethod]
        public static void CleaSession()
        {
            HttpContext.Current.Session.Remove("SESSION_ANNUALSERVICE_CUSTOMERMFG_DETAIL");
            HttpContext.Current.Session.Remove("SESSION_ANNUALSERVICE_DETAIL");
            HttpContext.Current.Session.Remove("SESSION_ANNUALSERVICE_LIST");
            HttpContext.Current.Session.Remove("SESSION_ANNUALSERVICE_FILE");
            HttpContext.Current.Session.Remove("SESSION_ANNUALSERVICE_YEAR");
        }
        [WebMethod]
        public static void CleaSessionMaeter()
        {
            HttpContext.Current.Session.Remove("SESSION_ANNUALSERVICE_DETAIL");
            HttpContext.Current.Session.Remove("SESSION_ANNUALSERVICE_LIST");
            HttpContext.Current.Session.Remove("SESSION_ANNUALSERVICE_FILE");
        }

        [WebMethod]
        public static void DeleteMasterList(string id)
        {
            try
            {
                List<AnnualServiceDetail> dataMasterList = (List<AnnualServiceDetail>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_DETAIL"];

                if (dataMasterList != null)
                {
                    var selectedData = (from t in dataMasterList where t.id == Convert.ToInt32(id) && t.is_deleted == false select t).FirstOrDefault();
                    if (selectedData != null)
                    {
                        if (Convert.ToInt32(id) < 0)
                        {
                            dataMasterList.Remove(selectedData);
                        }
                        else
                        {
                            selectedData.is_deleted = true;
                        }
                    }
                }

                HttpContext.Current.Session["SESSION_ANNUALSERVICE_DETAIL"] = dataMasterList;


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [WebMethod]
        public static void DeleteServiceYear(string id)
        {
            try
            {
                List<AnnualServiceYear> dataServiceYear = (List<AnnualServiceYear>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_YEAR"];

                if (dataServiceYear != null)
                {
                    var selectedData = (from t in dataServiceYear where t.service_year_id == Convert.ToInt32(id) select t).FirstOrDefault();
                    if (selectedData != null)
                    {
                        if (Convert.ToInt32(id) < 0)
                        {
                            dataServiceYear.Remove(selectedData);
                        }
                        else
                        {
                            selectedData.is_deleted = true;
                        }
                    }
                }

                HttpContext.Current.Session["SESSION_ANNUALSERVICE_YEAR"] = dataServiceYear;


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [WebMethod]
        public static string AddServiceYear(string service_time, string service_type, string service_free, string type_of_contract, string type_of_contract_text, string setting_date,
            string starting_date, string expire_date, string service_remark, string customer_id, string customer_mfg_id, string service_location,string contractstarting_date , string contractexpire_date)
        {
            var limit = 0;
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(type_of_contract) },
                        };
                conn.Open();
                var datare = SqlHelper.ExecuteDataset(conn, "sp_annual_service_contract_limit", arrParm.FirstOrDefault());

                if (datare != null)
                {


                    var result = datare.Tables[0].AsEnumerable().FirstOrDefault();
                    limit = Convert.IsDBNull(result["limit_of_type"]) ? 0 : Convert.ToInt32(result["limit_of_type"]);
                }

            }
            var data = "success";
            var annualServiceYear = (List<AnnualServiceYear>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_YEAR"];
            if (annualServiceYear != null)
            {
                //var checkExist = (from t in annualServiceYear
                //                  where t.is_deleted == false && t.service_time == Convert.ToInt32(service_time)
                //                  select t).FirstOrDefault();
                //if (checkExist == null)
                //{
                    var de = Convert.ToInt32(Math.Floor(Convert.ToDouble(service_free)));
                    annualServiceYear.Add(new AnnualServiceYear()
                    {
                        service_year_id = (annualServiceYear.Count + 1) * -1,
                        service_master_id = (annualServiceYear.Count + 1) * -1,
                        service_time = Convert.ToInt32(service_time),
                        service_free = Convert.ToDecimal(service_free),
                        service_location = service_location,
                        service_remark = service_remark,
                        time_of_contract = de * limit,
                        service_type = service_type,
                        service_type_text = service_type,
                        type_of_contract = type_of_contract,
                        type_name = type_of_contract_text,
                        starting_date = starting_date,
                        setting_date = setting_date,
                        contractstarting_date  = contractstarting_date,
                        contractexpire_date = contractexpire_date,
                        expire_date = expire_date,
                        type_of_contract_id = Convert.ToInt32(type_of_contract),
                        customer_id = Convert.ToInt32(customer_id),
                        customer_mfg_id = Convert.ToInt32(customer_mfg_id)
                    });
                //}
                //else
                //{
                //    data = "erroe";

                //}

            }
            else
            {
                annualServiceYear.Add(new AnnualServiceYear()
                {
                    service_year_id = (annualServiceYear.Count + 1) * -1,
                    service_master_id = (annualServiceYear.Count + 1) * -1,
                    service_time = Convert.ToInt32(service_time),
                    service_free = Convert.ToDecimal(service_free),
                    service_location = service_location,
                    service_remark = service_remark,
                    service_type = service_type,
                    service_type_text = service_type,
                    type_of_contract_id = Convert.ToInt32(type_of_contract),
                    time_of_contract = Convert.ToInt32(service_free) * limit,
                    type_of_contract = type_of_contract,
                    type_name = type_of_contract_text,
                    starting_date = starting_date,
                    setting_date = setting_date,
                    contractstarting_date = contractstarting_date,
                    contractexpire_date = contractexpire_date,
                    expire_date = expire_date,
                    customer_id = Convert.ToInt32(customer_id),
                    customer_mfg_id = Convert.ToInt32(customer_mfg_id)
                });
            }
            HttpContext.Current.Session["SESSION_ANNUALSERVICE_YEAR"] = annualServiceYear;
            return data;
        }

        protected void GridAnnualServiceYear_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridAnnualServiceYear.DataSource = (from t in annualServiceYear where t.is_deleted == false select t).ToList();
            GridAnnualServiceYear.DataBind();
        }

        protected void BindGridAnnualServiceYear()
        {
            GridAnnualServiceYear.DataSource = (from t in annualServiceYear where t.is_deleted == false select t).ToList();
            GridAnnualServiceYear.DataBind();
        }

        [WebMethod]
        public static AnnualServiceFile SelectEditeditFile(int id)
        {
            var annualServiceFile = (List<AnnualServiceFile>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_FILE"];
            if (annualServiceFile != null)
            {
                var row = (from t in annualServiceFile where t.id == id && t.is_deleted == false select t).FirstOrDefault();
                if (row != null)
                {
                    //row.starting_date = string.IsNullOrEmpty(row.starting_date) ? string.Empty : Convert.ToDateTime(row.starting_date).ToString("dd/MM/yyyy");
                    return row;
                }
            }
            return new AnnualServiceFile(); // ไม่มี Data
        }
        [WebMethod]
        public static string SubmitEditeditFile(int id, string description, string running_no,string starting_date,string mfg_number , string model_number)
        {
            var data = "success";
            var annualServiceFile = (List<AnnualServiceFile>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_FILE"];
            if (annualServiceFile != null)
            {
                var row = (from t in annualServiceFile where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (row != null)
                {
                    row.description = description;
                    row.running_no = running_no;
                    row.starting_date = starting_date;
                    row.mfg_number = mfg_number;
                    row.model_number = model_number;
                }
            }
            HttpContext.Current.Session["SESSION_ANNUALSERVICE_FILE"] = annualServiceFile;
            return data;
        }
        [WebMethod]
        public static string DeleteFile(int id)
        {
            var data = "success";
            var annualServiceFile = (List<AnnualServiceFile>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_FILE"];
            if (annualServiceFile != null)
            {
                var row = (from t in annualServiceFile where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (row != null)
                {
                    row.is_deleted = true;
                }
            }
            HttpContext.Current.Session["SESSION_ANNUALSERVICE_FILE"] = annualServiceFile;
            return data;
        }

        protected void cbbProject_Callback(object sender, CallbackEventArgsBase e)
        {
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                {
                    new SqlParameter("@lang_id", SqlDbType.VarChar, 3) { Value = "tha" },
                    new SqlParameter("@customer_id", SqlDbType.Int) { Value = cbbCustomerMFG.Value },
                };
                conn.Open();
                var dsResult = SqlHelper.ExecuteDataset(conn, "sp_dropdown_annual_customer_project", arrParm.ToArray());

                conn.Close();
                if (dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        cbbProject.DataSource = dsResult;
                        cbbProject.TextField = "data_text";
                        cbbProject.ValueField = "data_value";
                        cbbProject.DataBind();

                        cbbProject.Items.Insert(0, new ListEditItem("ทั้งหมด", ""));
                        cbbProject.SelectedIndex = 0;
                    }
                }
            }
        }

        [WebMethod]
        public static string GetCustomerMfg(string customer_id, string customer_mfg_id)
        {
            var data = "";
            var customerMFGData = (List<CustomerMFGList>)HttpContext.Current.Session["SESSION_ANNUALSERVICE_CUSTOMERMFG_DETAIL"];
            if (customerMFGData != null)
            {
                var row = (from t in customerMFGData where t.customer_mfg_id == Convert.ToInt32(customer_mfg_id) select t).FirstOrDefault();
                if (row != null)
                {
                    data = "Unit Warranty : " + row.unitwarraty + ", Air-End Warranty : " + row.airendwarraty + ", Service Fee : " + row.service_fee;
                }
            }
            return data;
        }
    }
}


