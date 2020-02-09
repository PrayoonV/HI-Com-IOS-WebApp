using DevExpress.Web;
using HicomIOS.ClassUtil;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HicomIOS.Master
{
    public partial class CustomerBranch : MasterDetailPage
    {
        private DataSet dsResult;
        private DataTable dtProvince = new DataTable();
        private DataTable dtAmphur = new DataTable();
        private DataTable dtDistrict = new DataTable();

        public class CustomerBranchList
        {
            public int id { get; set; }
            public string branch_no { get; set; }
            public string branch_name { get; set; }
            public int customer_id { get; set; }
            public string address_bill_tha { get; set; }
            public string address_bill_eng { get; set; }
            public string address_install_tha { get; set; }
            public string address_install_eng { get; set; }
            public int geo_id { get; set; }
            public int province_id { get; set; }
            public int amphur_id { get; set; }
            public int district_id { get; set; }
            public string zipcode { get; set; }
            public bool is_enable { get; set; }
            public bool is_delete { get; set; }
            public int created_by { get; set; }
            public DateTime created_date { get; set; }
            public int updated_by { get; set; }
            public DateTime updated_date { get; set; }
            public List<ProvinceClass> provinceList { get; set; }
            public List<AmphurClass> amphurList { get; set; }
            public List<DistrictClass> districtList { get; set; }
        }

        public class ProvinceClass
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class AmphurClass
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class DistrictClass
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public override string PageName { get { return "Customer Branch"; } }
        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }
        public override long SelectedItemID
        {
            get
            {
                var ItemID = gridView.GetRowValues(gridView.FocusedRowIndex, gridView.KeyFieldName);
                return ItemID != null ? (int)ItemID : DataListUtil.emptyEntryID;
            }
            set
            {
                gridView.FocusedRowIndex = gridView.FindVisibleIndexByKeyValue(value);
            }
        }

        // ClientInstanceName that use in Script.js
        private string strClientInstanceName_gridView;
        private string strClientInstanceName_UserControlEditForm = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Load Combobox Customer Data
            SPlanetUtil.BindASPxComboBox(ref cboCustomerId, DataListUtil.DropdownStoreProcedureName.Customer);

            // Load Combobox Thailand Geo Data
            SPlanetUtil.BindASPxComboBox(ref cboGEO, DataListUtil.DropdownStoreProcedureName.Thailand_Geo);

            // Setting height gridView
            gridView.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen);

            //Bind Data into GridView
            if (!Page.IsPostBack)
            {
                // Get Permission and if no permission, will redirect to another page.
                if (!Permission.GetPermission())
                    Response.Redirect(ConstantClass.CONSTANT_NO_PERMISSION_PAGE);

                dsResult = null;
                BindGrid(true);
            }
            else
            {
                dsResult = (DataSet)ViewState["dsResult"];
                BindGrid(false);
            }
        }

        #region "Inherited Event of Filter Control"
        public override void OnFilterChanged()
        {
            BindGrid();
        }

        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            return result.OfType<FilterControlColumn>();
        }
        #endregion

        protected void BindGrid(bool isForceRefreshData = false)
        {
            try
            {
                if (!Page.IsPostBack || isForceRefreshData)
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 }
                        };
                        conn.Open();
                        dsResult = SqlHelper.ExecuteDataset(conn, "sp_customer_branch_list", arrParm.ToArray());
                        ViewState["dsResult"] = dsResult;
                    }
                }

                //Bind data into GridView
                gridView.DataSource = dsResult;
                gridView.FilterExpression = FilterBag.GetExpression(false);
                gridView.DataBind();
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }

        protected void cboProvince_Callback(object source, CallbackEventArgsBase e)
        {
            FillProvinceCombo(e.Parameter);
        }
        protected void cboAmphur_Callback(object source, CallbackEventArgsBase e)
        {
            FillAmphurCombo(e.Parameter);
        }
        protected void cboDistrict_Callback(object source, CallbackEventArgsBase e)
        {
            FillDistrictCombo(e.Parameter);
        }

        protected void FillProvinceCombo(string geoId)
        {
            try
            {
                dtProvince = new DataTable();
                dtProvince.Columns.Add("data_text", typeof(string));
                dtProvince.Columns.Add("data_value", typeof(int));
                dtProvince.Columns.Add("geo_id", typeof(int));

                if (!string.IsNullOrEmpty(geoId) && geoId != "0")
                {
                    var dtSource = new DataTable();
                    dtSource.Columns.Add("data_value", typeof(int));
                    dtSource.Columns.Add("data_text", typeof(string));
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        conn.Open();
                        using (DataSet dsResult = SqlHelper.ExecuteDataset(SPlanetUtil.GetConnectionString(), CommandType.StoredProcedure, DataListUtil.DropdownStoreProcedureName.Thailand_Province))
                        {
                            if (dsResult != null)
                            {
                                foreach (var row in dsResult.Tables[0].AsEnumerable())
                                {
                                    if (Convert.ToString(row["geo_id"]) == geoId)
                                    {
                                        dtProvince.Rows.Add(row[1], Convert.ToInt32(row[0]), Convert.ToInt32(row[2]));
                                    }
                                }
                            }

                        }
                        conn.Close();
                    }

                    var provinceFilter = (from t in dtProvince.AsEnumerable()
                                          where t.Field<int>("geo_id") == Convert.ToInt32(geoId)
                                          select t).ToList();
                    foreach (var row in provinceFilter)
                    {
                        dtSource.Rows.Add(row[1], row[0]);
                    }
                    cboProvince.DataSource = dtSource;
                    cboProvince.DataBind();
                }
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        protected void FillAmphurCombo(string provinceId)
        {
            try
            {
                dtAmphur = new DataTable();
                dtAmphur.Columns.Add("data_text", typeof(string));
                dtAmphur.Columns.Add("data_value", typeof(int));
                dtAmphur.Columns.Add("province_id", typeof(int));

                if (!string.IsNullOrEmpty(provinceId) && provinceId != "0")
                {
                    var dtSource = new DataTable();
                    dtSource.Columns.Add("data_value", typeof(int));
                    dtSource.Columns.Add("data_text", typeof(string));
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        conn.Open();
                        using (DataSet dsResult = SqlHelper.ExecuteDataset(SPlanetUtil.GetConnectionString(), CommandType.StoredProcedure, DataListUtil.DropdownStoreProcedureName.Thailand_Amphur))
                        {
                            if (dsResult != null)
                            {
                                foreach (var row in dsResult.Tables[0].AsEnumerable())
                                {
                                    if (Convert.ToString(row["province_id"]) == provinceId)
                                    {
                                        dtAmphur.Rows.Add(row[1], Convert.ToInt32(row[0]), Convert.ToInt32(row[2]));
                                    }
                                }
                            }

                        }
                        conn.Close();
                    }

                    var amphurFilter = (from t in dtAmphur.AsEnumerable()
                                        where t.Field<int>("province_id") == Convert.ToInt32(provinceId)
                                        select t).ToList();
                    foreach (var row in amphurFilter)
                    {
                        dtSource.Rows.Add(row[1], row[0]);
                    }
                    cboAmphur.DataSource = dtSource;
                    cboAmphur.DataBind();
                }
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }
        protected void FillDistrictCombo(string amphurId)
        {
            try
            {
                dtDistrict = new DataTable();
                dtDistrict.Columns.Add("data_text", typeof(string));
                dtDistrict.Columns.Add("data_value", typeof(int));
                dtDistrict.Columns.Add("amphur_id", typeof(int));

                if (!string.IsNullOrEmpty(amphurId) && amphurId != "0")
                {
                    var dtSource = new DataTable();
                    dtSource.Columns.Add("data_value", typeof(int));
                    dtSource.Columns.Add("data_text", typeof(string));

                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        conn.Open();
                        using (DataSet dsResult = SqlHelper.ExecuteDataset(SPlanetUtil.GetConnectionString(), CommandType.StoredProcedure, DataListUtil.DropdownStoreProcedureName.Thailand_District))
                        {
                            if (dsResult != null)
                            {
                                foreach (var row in dsResult.Tables[0].AsEnumerable())
                                {
                                    if (Convert.ToString(row["amphur_id"]) == amphurId)
                                    {
                                        dtDistrict.Rows.Add(row[1], Convert.ToInt32(row[0]), Convert.ToInt32(row[2]));
                                    }
                                }
                            }

                        }
                        conn.Close();
                    }

                    var districtFilter = (from t in dtDistrict.AsEnumerable()
                                          where t.Field<int>("amphur_id") == Convert.ToInt32(amphurId)
                                          select t).ToList();
                    foreach (var row in districtFilter)
                    {
                        dtSource.Rows.Add(row[1], row[0]);
                    }
                    cboDistrict.DataSource = dtSource;
                    cboDistrict.DataBind();
                }
            }
            catch (Exception ex)
            {
                string strErrorMsg = SPlanetUtil.LogErrorCollect(ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "myalert", "alert('" + strErrorMsg + "');", true);
            }
        }

        [WebMethod]
        public static string InsertCustomerBranch(CustomerBranchList[] customerBranchAddData)
        {
            var row = (from t in customerBranchAddData select t).FirstOrDefault();
            if (row != null)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_customer_branch_add", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@branch_no", SqlDbType.VarChar, 10).Value = Convert.ToString(row.branch_no);
                            cmd.Parameters.Add("@branch_name", SqlDbType.VarChar, 200).Value = Convert.ToString(row.branch_name);
                            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToInt32(row.customer_id);
                            cmd.Parameters.Add("@address_bill_tha", SqlDbType.VarChar, 200).Value = Convert.ToString(row.address_bill_tha);
                            cmd.Parameters.Add("@address_bill_eng", SqlDbType.VarChar, 200).Value = Convert.ToString(row.address_bill_eng);
                            cmd.Parameters.Add("@address_install_tha", SqlDbType.VarChar, 200).Value = Convert.ToString(row.address_install_tha);
                            cmd.Parameters.Add("@address_install_eng", SqlDbType.VarChar, 200).Value = Convert.ToString(row.address_install_eng);
                            cmd.Parameters.Add("@geo_id", SqlDbType.Int).Value = Convert.ToInt32(row.geo_id);
                            cmd.Parameters.Add("@province_id", SqlDbType.Int).Value = Convert.ToInt32(row.province_id);
                            cmd.Parameters.Add("@amphur_id", SqlDbType.Int).Value = Convert.ToInt32(row.amphur_id);
                            cmd.Parameters.Add("@district_id", SqlDbType.Int).Value = Convert.ToInt32(row.district_id);
                            cmd.Parameters.Add("@zipcode", SqlDbType.VarChar, 10).Value = Convert.ToString(row.zipcode);
                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return "success";
        }

        [WebMethod]
        public static CustomerBranchList GetEditCustomerBranchData(string id)
        {
            var customerBranchData = new CustomerBranchList();
            var dsDataCustomerBranch = new DataSet();
            var dsDataProvince = new DataSet();
            var dsDataAmphur = new DataSet();
            var dsDataDistrict = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = Convert.ToInt32(id) }
                        };
                    conn.Open();
                    dsDataCustomerBranch = SqlHelper.ExecuteDataset(conn, "sp_customer_branch_list", arrParm.ToArray());
                    conn.Close();

                    var geo_id = Convert.IsDBNull(dsDataCustomerBranch.Tables[0].Rows[0]["geo_id"]) ? 0 : Convert.ToInt32(dsDataCustomerBranch.Tables[0].Rows[0]["geo_id"]);
                    if (geo_id > 0)
                    {
                        List<SqlParameter> arrProvince = new List<SqlParameter>
                        {
                            new SqlParameter("@geo_id", SqlDbType.Int) { Value = Convert.ToInt32(geo_id) }
                        };
                        conn.Open();
                        dsDataProvince = SqlHelper.ExecuteDataset(conn, "sp_dropdrown_tb_thailand_2_province", arrProvince.ToArray());
                        conn.Close();
                    }


                    var province_id = Convert.IsDBNull(dsDataCustomerBranch.Tables[0].Rows[0]["province_id"]) ? 0 : Convert.ToInt32(dsDataCustomerBranch.Tables[0].Rows[0]["province_id"]);
                    if (province_id > 0)
                    {
                        List<SqlParameter> arrAmphur = new List<SqlParameter>
                        {
                            new SqlParameter("@province_id", SqlDbType.Int) { Value = Convert.ToInt32(province_id) }
                        };
                        conn.Open();
                        dsDataAmphur = SqlHelper.ExecuteDataset(conn, "sp_dropdrown_tb_thailand_3_amphur", arrAmphur.ToArray());
                        conn.Close();
                    }

                    var amphur_id = Convert.IsDBNull(dsDataCustomerBranch.Tables[0].Rows[0]["amphur_id"]) ? 0 : Convert.ToInt32(dsDataCustomerBranch.Tables[0].Rows[0]["amphur_id"]);
                    if (amphur_id > 0)
                    {
                        List<SqlParameter> arrDistrict = new List<SqlParameter>
                        {
                            new SqlParameter("@amphur_id", SqlDbType.Int) { Value = Convert.ToInt32(amphur_id) }
                        };
                        conn.Open();
                        dsDataDistrict = SqlHelper.ExecuteDataset(conn, "sp_dropdrown_tb_thailand_4_district", arrDistrict.ToArray());
                        conn.Close();
                    }
                }

                if (dsDataCustomerBranch.Tables.Count > 0)
                {
                    var row = dsDataCustomerBranch.Tables[0].Rows[0];

                    customerBranchData.id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]);
                    customerBranchData.branch_no = Convert.IsDBNull(row["branch_no"]) ? null : Convert.ToString(row["branch_no"]);
                    customerBranchData.branch_name = Convert.IsDBNull(row["branch_name"]) ? null : Convert.ToString(row["branch_name"]);
                    customerBranchData.customer_id = Convert.IsDBNull(row["customer_id"]) ? 0 : Convert.ToInt32(row["customer_id"]);
                    customerBranchData.address_bill_tha = Convert.IsDBNull(row["address_bill_tha"]) ? null : Convert.ToString(row["address_bill_tha"]);
                    customerBranchData.address_bill_eng = Convert.IsDBNull(row["address_bill_eng"]) ? null : Convert.ToString(row["address_bill_eng"]);
                    customerBranchData.address_install_tha = Convert.IsDBNull(row["address_install_tha"]) ? null : Convert.ToString(row["address_install_tha"]);
                    customerBranchData.address_install_eng = Convert.IsDBNull(row["address_install_eng"]) ? null : Convert.ToString(row["address_install_eng"]);
                    customerBranchData.geo_id = Convert.IsDBNull(row["geo_id"]) ? 0 : Convert.ToInt32(row["geo_id"]);
                    customerBranchData.province_id = Convert.IsDBNull(row["province_id"]) ? 0 : Convert.ToInt32(row["province_id"]);
                    customerBranchData.amphur_id = Convert.IsDBNull(row["amphur_id"]) ? 0 : Convert.ToInt32(row["amphur_id"]);
                    customerBranchData.district_id = Convert.IsDBNull(row["district_id"]) ? 0 : Convert.ToInt32(row["district_id"]);
                    customerBranchData.zipcode = Convert.IsDBNull(row["zipcode"]) ? null : Convert.ToString(row["zipcode"]);
                    
                }

                customerBranchData.provinceList = new List<ProvinceClass>();
                if (dsDataProvince.Tables.Count > 0)
                {
                    foreach (var rowProvince in dsDataProvince.Tables[0].AsEnumerable())
                    {
                        customerBranchData.provinceList.Add(new ProvinceClass()
                        {
                            id = Convert.IsDBNull(rowProvince["data_value"]) ? 0 : Convert.ToInt32(rowProvince["data_value"]),
                            name = Convert.IsDBNull(rowProvince["data_text"]) ? null : Convert.ToString(rowProvince["data_text"])
                        });
                    }
                }

                customerBranchData.amphurList = new List<AmphurClass>();
                if (dsDataAmphur.Tables.Count > 0)
                {
                    foreach (var rowAmphur in dsDataAmphur.Tables[0].AsEnumerable())
                    {
                        customerBranchData.amphurList.Add(new AmphurClass()
                        {
                            id = Convert.IsDBNull(rowAmphur["data_value"]) ? 0 : Convert.ToInt32(rowAmphur["data_value"]),
                            name = Convert.IsDBNull(rowAmphur["data_text"]) ? null : Convert.ToString(rowAmphur["data_text"])
                        });
                    }
                }

                customerBranchData.districtList = new List<DistrictClass>();
                if (dsDataDistrict.Tables.Count > 0)
                {
                    foreach (var rowDistrict in dsDataDistrict.Tables[0].AsEnumerable())
                    {
                        customerBranchData.districtList.Add(new DistrictClass()
                        {
                            id = Convert.IsDBNull(rowDistrict["data_value"]) ? 0 : Convert.ToInt32(rowDistrict["data_value"]),
                            name = Convert.IsDBNull(rowDistrict["data_text"]) ? null : Convert.ToString(rowDistrict["data_text"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return customerBranchData;
        }

        [WebMethod]
        public static string UpdateCustomerBranch(CustomerBranchList[] customerBranchUpdateData)
        {
            var row = (from t in customerBranchUpdateData select t).FirstOrDefault();
            if (row != null)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_customer_branch_edit", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(row.id);
                            cmd.Parameters.Add("@branch_no", SqlDbType.VarChar, 10).Value = Convert.ToString(row.branch_no);
                            cmd.Parameters.Add("@branch_name", SqlDbType.VarChar, 200).Value = Convert.ToString(row.branch_name);
                            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = Convert.ToInt32(row.customer_id);
                            cmd.Parameters.Add("@address_bill_tha", SqlDbType.VarChar, 200).Value = Convert.ToString(row.address_bill_tha);
                            cmd.Parameters.Add("@address_bill_eng", SqlDbType.VarChar, 200).Value = Convert.ToString(row.address_bill_eng);
                            cmd.Parameters.Add("@address_install_tha", SqlDbType.VarChar, 200).Value = Convert.ToString(row.address_install_tha);
                            cmd.Parameters.Add("@address_install_eng", SqlDbType.VarChar, 200).Value = Convert.ToString(row.address_install_eng);
                            cmd.Parameters.Add("@geo_id", SqlDbType.Int).Value = Convert.ToInt32(row.geo_id);
                            cmd.Parameters.Add("@province_id", SqlDbType.Int).Value = Convert.ToInt32(row.province_id);
                            cmd.Parameters.Add("@amphur_id", SqlDbType.Int).Value = Convert.ToInt32(row.amphur_id);
                            cmd.Parameters.Add("@district_id", SqlDbType.Int).Value = Convert.ToInt32(row.district_id);
                            cmd.Parameters.Add("@zipcode", SqlDbType.VarChar, 10).Value = Convert.ToString(row.zipcode);
                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return "success";
        }

        [WebMethod]
        public static string DeleteCustomerBranch(string id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_customer_branch_delete", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(id);
                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "success";
        }
    }
}