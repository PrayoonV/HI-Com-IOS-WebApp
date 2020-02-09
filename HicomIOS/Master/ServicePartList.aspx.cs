﻿using DevExpress.Web;
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
    public partial class ServicePartList : MasterDetailPage
    {
        private int dataId = 0;
        public override string PageName { get { return "Maintenance Part"; } }

        public override FilterBag FilterBag { get { return SPlanetUtil.ObjectFilter; } }

        public override void OnFilterChanged()
        {

        }
        public override IEnumerable<FilterControlColumn> GetFilterColumns()
        {
            var result = new ArrayList();
            return result.OfType<FilterControlColumn>();
        }
        #region Members
        public class ModelServiceHeader
        {
            public int id { get; set; }
            public bool is_enable { get; set; }
            public string model_name { get; set; }
            public string description_tha { get; set; }
            public int count_package { get; set; }
            public int count_partlist { get; set; }
        }

        /// <summary>
        /// สำหรับ เลือก PartList จาก Table tb_service_part_list
        /// </summary>
        public class ServicePartListItem
        {
            public bool is_selected { get; set; }
            public int id { get; set; }
            public string item_no { get; set; }
            public string part_no { get; set; }
            public string part_name_tha { get; set; }
            public string description_tha { get; set; }
            public int qty { get; set; }
            public int qty_balance { get; set; }
            public int qty_reserve { get; set; }
            public int selling_price { get; set; }
            public int sort_no { get; set; }
        }

        /// <summary>
        /// บันทึกลง tb_service_part_maintenance
        /// </summary>
        public class ServicePartMaintenance
        {
            public int id { get; set; }
            public int part_id { get; set; }
            public int sort_no { get; set; }
            public string type { get; set; }
            public int item { get; set; }
            public string no { get; set; }
            public string part_no { get; set; }
            public string part_name { get; set; }
            public int model_id { get; set; }
            public int qty { get; set; }
            public int selling_price { get; set; }
            public int total_price { get; set; }
            public bool is_delete { get; set; }

        }

        /// <summary>
        ///  สำหรับ Table tb_service_package
        /// </summary>
        public class ServicePackage
        {
            public int id { get; set; }
            public int model_id { get; set; }
            public int count_year { get; set; }
            public string item_of_part { get; set; }
            public string running_hours { get; set; }
            public int total_price { get; set; }
            public int service_charge { get; set; }
            public string overhaul_service { get; set; }
            public string overhaul_servicedes { get; set; }
            public string overhaul_rate { get; set; }
            public bool is_deleted { get; set; }
            public int count_part_list { get; set; }
            public bool is_enable { get; set; }
        }
        public class PackageData
        {
            public int ModelId { get; set; }
            public int PackageId { get; set; }
            public int PackageCountYear { get; set; }
            public string PackageItemOfPart { get; set; }
            public string PackageRunningHours { get; set; }
            public int PackageServiceChange { get; set; }
            public string Packageoverhaul_service { get; set; }
            public string Packageoverhaul_servicedes { get; set; }
            public string Packageoverhaul_rate { get; set; }
        }

        /// <summary>
        /// บันทึกลง tb_service_package_part_mapping
        /// </summary>
        public class ServicePackagePartMapping
        {
            public int id { get; set; }
            public int package_id { get; set; }
            public int part_id { get; set; }
            public string part_name { get; set; }
            public string part_no { get; set; }
            public int selling_price { get; set; }
            public string item_no { get; set; }
            public bool is_delete { get; set; }
            public bool is_enable { get; set; }

            public int sort_no { get; set; }
        }

        public class ServicePackageDescription
        {
            public int id { get; set; }
            public int maintanance_package_id { get; set; }
            public int maintanance_description_id { get; set; }
            public string description { get; set; }
        }

        public class ModelRemark
        {
            public int id { get; set; }
            public string remark { get; set; }
            public int model_id { get; set; }
            public bool is_delete { get; set; }
        }
        List<ServicePartListItem> servicePartListItemList
        {
            get
            {
                if (Session["SESSION_SERVICE_PART_ITEM_SERVICEPARTLIST"] == null)
                    Session["SESSION_SERVICE_PART_ITEM_SERVICEPARTLIST"] = new List<ServicePartListItem>();
                return (List<ServicePartListItem>)Session["SESSION_SERVICE_PART_ITEM_SERVICEPARTLIST"];
            }
            set
            {
                Session["SESSION_SERVICE_PART_ITEM_SERVICEPARTLIST"] = value;
            }
        }
        List<ServicePartMaintenance> servicePartListMaintenanceList
        {
            get
            {
                if (Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"] == null)
                    Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"] = new List<ServicePartMaintenance>();
                return (List<ServicePartMaintenance>)Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"];
            }
            set
            {
                Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"] = value;
            }
        }
        List<ServicePartMaintenance> selectedServicePartListMaintenanceList
        {
            get
            {
                if (Session["SESSION_SELECTED_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"] == null)
                    Session["SESSION_SELECTED_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"] = new List<ServicePartMaintenance>();
                return (List<ServicePartMaintenance>)Session["SESSION_SELECTED_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"];
            }
            set
            {
                Session["SESSION_SELECTED_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"] = value;
            }
        }
        List<ModelServiceHeader> modelHeaderList
        {
            get
            {
                if (Session["SESSION_MODEL_LIST_SERVICEPARTLIST"] == null)
                    Session["SESSION_MODEL_LIST_SERVICEPARTLIST"] = new List<ModelServiceHeader>();
                return (List<ModelServiceHeader>)Session["SESSION_MODEL_LIST_SERVICEPARTLIST"];
            }
            set
            {
                Session["SESSION_MODEL_LIST_SERVICEPARTLIST"] = value;
            }
        }
        List<ServicePackage> servicePackageList
        {
            get
            {
                if (Session["SESSION_SERVICE_PACKAGE_SERVICEPARTLIST"] == null)
                    Session["SESSION_SERVICE_PACKAGE_SERVICEPARTLIST"] = new List<ServicePackage>();
                return (List<ServicePackage>)Session["SESSION_SERVICE_PACKAGE_SERVICEPARTLIST"];
            }
            set
            {
                Session["SESSION_SERVICE_PACKAGE_SERVICEPARTLIST"] = value;
            }
        }
        List<ServicePartListItem> servicePartListPackageSelectList
        {
            get
            {
                if (Session["SESSION_SERVICE_PART_PACKAGE_SERVICEPARTLIST"] == null)
                    Session["SESSION_SERVICE_PART_PACKAGE_SERVICEPARTLIST"] = new List<ServicePartListItem>();
                return (List<ServicePartListItem>)Session["SESSION_SERVICE_PART_PACKAGE_SERVICEPARTLIST"];
            }
            set
            {
                Session["SESSION_SERVICE_PART_PACKAGE_SERVICEPARTLIST"] = value;
            }
        }
        List<ServicePackagePartMapping> servicePartPackageMappingList
        {
            get
            {
                if (Session["SESSION_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"] == null)
                    Session["SESSION_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"] = new List<ServicePackagePartMapping>();
                return (List<ServicePackagePartMapping>)Session["SESSION_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"];
            }
            set
            {
                Session["SESSION_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"] = value;
            }
        }
        List<ServicePackagePartMapping> selectedServicePartPackageMappingList
        {
            get
            {
                if (Session["SESSION_SELECTED_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"] == null)
                    Session["SESSION_SELECTED_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"] = new List<ServicePackagePartMapping>();
                return (List<ServicePackagePartMapping>)Session["SESSION_SELECTED_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"];
            }
            set
            {
                Session["SESSION_SELECTED_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"] = value;
            }
        }
        List<ServicePackageDescription> selectedServicePackageDescriptionList
        {
            get
            {
                if (Session["SESSION_SELECTED_SERVICE_DESCRIPTION_PACKAGE_"] == null)
                    Session["SESSION_SELECTED_SERVICE_DESCRIPTION_PACKAGE_"] = new List<ServicePackageDescription>();
                return (List<ServicePackageDescription>)Session["SESSION_SELECTED_SERVICE_DESCRIPTION_PACKAGE_"];
            }
            set
            {
                Session["SESSION_SELECTED_SERVICE_DESCRIPTION_PACKAGE_"] = value;
            }
        }
        List<ModelRemark> modelRemarkList
        {
            get
            {
                if (Session["SESSION_MODEL_REMARK_SERVICEPARTLIST"] == null)
                    Session["SESSION_MODEL_REMARK_SERVICEPARTLIST"] = new List<ModelRemark>();
                return (List<ModelRemark>)Session["SESSION_MODEL_REMARK_SERVICEPARTLIST"];
            }
            set
            {
                Session["SESSION_MODEL_REMARK_SERVICEPARTLIST"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // Setting height gridView
            gridViewModel.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen);

            try
            {
                if (!IsPostBack)
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
                    BindGridViewModel();
                    BindGridViewPartListModel();
                    BindGridViewPartListSelected();
                    BindGridViewPackage();
                    BindGridViewPartListPackage();
                    BindGridViewPartListPackageSelect();
                    BindGridViewDescriptionListPackage();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void PrepareData()
        {

            // Setting height gridView
            gridViewModel.Settings.VerticalScrollableHeight = Convert.ToInt32(BasePage.SizeScreen);
            gridViewModel.SettingsBehavior.AllowFocusedRow = true;

            try
            {
                modelHeaderList = new List<ModelServiceHeader>();

                #region Model List
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                        new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                        };
                    conn.Open();
                    var dsModel = SqlHelper.ExecuteDataset(conn, "sp_maintenance_model_list", arrParm.ToArray());
                    conn.Close();
                    if (dsModel != null)
                    {
                        /*int rowId = 0;
                        if (Session["ROW_ID"] != null)
                        {
                            rowId = Convert.ToInt32(Session["ROW_ID"].ToString());
                        }*/
                        foreach (var row in dsModel.Tables[0].AsEnumerable())
                        {
                            modelHeaderList.Add(new ModelServiceHeader()
                            {
                                count_package = Convert.IsDBNull(row["count_package"]) ? 0 : Convert.ToInt32(row["count_package"]),
                                count_partlist = Convert.IsDBNull(row["count_partlist"]) ? 0 : Convert.ToInt32(row["count_partlist"]),
                                description_tha = Convert.IsDBNull(row["description_tha"]) ? string.Empty : Convert.ToString(row["description_tha"]),
                                id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                is_enable = Convert.IsDBNull(row["is_enable"]) ? false : Convert.ToBoolean(row["is_enable"]),
                                model_name = Convert.IsDBNull(row["model_name"]) ? string.Empty : Convert.ToString(row["model_name"]),
                            });

                            /*if (rowId == modelHeaderList[modelHeaderList.Count - 1].id)
                            {
                                int selectedRow = modelHeaderList.Count - 1;
                                int prevRow = Convert.ToInt32(Session["ROW"]);
                                int pageSize = gridViewModel.SettingsPager.PageSize;
                                int pageIndex = (int)(selectedRow / pageSize);
                                int prevPageIndex = Convert.ToInt32(Session["PAGE"]);
                                if (prevRow == selectedRow)
                                {
                                    Session["PAGE"] = prevPageIndex;
                                    Session["ROW"] = prevPageIndex * pageSize;
                                }
                                else
                                {
                                    Session["PAGE"] = pageIndex;
                                    Session["ROW"] = selectedRow;
                                }
                            }*/
                        }
                    }
                    //ViewState["VIEWSTATE_MODEL_HEADER"] = modelHeaderList;
                }
                #endregion

                #region PartList
                /*using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {
                    //Create array of Parameters
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = "" }
                        };
                    conn.Open();
                    var dsServicePartList = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_list", arrParm.ToArray());
                    conn.Close();
                    if (dsServicePartList != null)
                    {
                        servicePartListItemList = new List<ServicePartListItem>();
                        foreach (var detailRow in dsServicePartList.Tables[0].AsEnumerable())
                        {
                            servicePartListItemList.Add(new ServicePartListItem()
                            {
                                id = Convert.IsDBNull(detailRow["id"]) ? 0 : Convert.ToInt32(detailRow["id"]),
                                //item_no = Convert.IsDBNull(detailRow["item_no"]) ? string.Empty : Convert.ToString(detailRow["item_no"]),
                                part_no = Convert.IsDBNull(detailRow["part_no"]) ? string.Empty : Convert.ToString(detailRow["part_no"]),
                                part_name_tha = Convert.IsDBNull(detailRow["part_name_tha"]) ? string.Empty : Convert.ToString(detailRow["part_name_tha"]),
                                //description_tha = Convert.IsDBNull(detailRow["description_tha"]) ? string.Empty : Convert.ToString(detailRow["description_tha"]),
                                selling_price = Convert.IsDBNull(detailRow["selling_price"]) ? 0 : Convert.ToInt32(detailRow["selling_price"]),
                                is_selected = false,
                                qty = Convert.IsDBNull(detailRow["quantity"]) ? 0 : Convert.ToInt32(detailRow["quantity"]),
                                qty_balance = Convert.IsDBNull(detailRow["quantity_balance"]) ? 0 : Convert.ToInt32(detailRow["quantity_balance"]),
                                qty_reserve = Convert.IsDBNull(detailRow["quantity_reserve"]) ? 0 : Convert.ToInt32(detailRow["quantity_reserve"])
                            });

                        }
                    }
                }*/
                #endregion

                gridViewModel.DataSource = modelHeaderList;
                gridViewModel.DataBind();

                //  Check page from session
                if (Session["ROW_ID"] != null)
                {
                    int row = Convert.ToInt32(Session["ROW"]);
                    gridViewModel.FocusedRowIndex = row;
                }
                if (Session["PAGE"] != null)
                {
                    int page = Convert.ToInt32(Session["PAGE"]);
                    gridViewModel.PageIndex = page;
                }
                if (!Page.IsPostBack && !string.IsNullOrEmpty(Session["COLUMN"].ToString()) && !string.IsNullOrEmpty(Session["ORDER"].ToString()))
                {
                    int order = Convert.ToInt32(Session["ORDER"]);
                    if (order == 1)
                    {
                        ((GridViewDataColumn)gridViewModel.Columns[Session["COLUMN"].ToString()]).SortAscending();
                    }
                    else
                    {
                        ((GridViewDataColumn)gridViewModel.Columns[Session["COLUMN"].ToString()]).SortDescending();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void ClearWorkingSession()
        {
            Session.Remove("SESSION_SERVICE_PART_ITEM_SERVICEPARTLIST");
            Session.Remove("SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST");
            Session.Remove("SESSION_SELECTED_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST");
            Session.Remove("SESSION_MODEL_HEADER_MODEL");
            Session.Remove("SESSION_SERVICE_PACKAGE_SERVICEPARTLIST");
            Session.Remove("SESSION_SERVICE_PART_PACKAGE_SERVICEPARTLIST");
            Session.Remove("SESSION_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST");
            Session.Remove("SESSION_SELECTED_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST");
            Session.Remove("SESSION_MODEL_LIST_SERVICEPARTLIST");
            Session.Remove("SESSION_MODEL_REMARK_SERVICEPARTLIST");
            Session.Remove("SESSION_SELECTED_SERVICE_DESCRIPTION_PACKAGE_");
        }
        protected void LoadData()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Model
        protected void gridViewPartListModel_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var splitStr = e.Parameters.ToString().Split('|');
            string searchText = string.Empty;

            if (splitStr.Length > 1)
            {
                searchText = splitStr[1];
            }
            if (string.IsNullOrEmpty(searchText))
            {
                gridViewPartListModel.DataSource = (from t in servicePartListMaintenanceList
                                                    where t.is_delete == false
                                                    select t).ToList();
                gridViewPartListModel.DataBind();
            }
            else
            {
                gridViewPartListModel.DataSource = (from t in servicePartListMaintenanceList
                                                    where (t.no.ToUpper().Contains(searchText.ToUpper()) || t.part_no.ToUpper().Contains(searchText.ToUpper()) || t.part_name.ToUpper().Contains(searchText.ToUpper()))
                                                    where t.is_delete == false
                                                    select t).ToList();
                gridViewPartListModel.DataBind();
            }



        }

        private void BindGridViewPartListModel()
        {
            gridViewPartListModel.DataSource = (from t in servicePartListMaintenanceList
                                                where t.is_delete == false
                                                select t).ToList();
            gridViewPartListModel.DataBind();
        }

        protected void gridViewPartListSelected_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var splitStr = e.Parameters.ToString().Split('|');
            string searchText = string.Empty;

            if (splitStr.Length > 1)
            {
                searchText = splitStr[1];
            }

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@search_name", SqlDbType.VarChar, 200) { Value = searchText }
                        };
                conn.Open();
                var dsServicePartList = SqlHelper.ExecuteDataset(conn, "sp_product_spare_part_list", arrParm.ToArray());
                conn.Close();
                if (dsServicePartList != null)
                {
                    servicePartListItemList = new List<ServicePartListItem>();
                    foreach (var detailRow in dsServicePartList.Tables[0].AsEnumerable())
                    {
                        servicePartListItemList.Add(new ServicePartListItem()
                        {
                            id = Convert.IsDBNull(detailRow["id"]) ? 0 : Convert.ToInt32(detailRow["id"]),
                            //item_no = Convert.IsDBNull(detailRow["item_no"]) ? string.Empty : Convert.ToString(detailRow["item_no"]),
                            part_no = Convert.IsDBNull(detailRow["part_no"]) ? string.Empty : Convert.ToString(detailRow["part_no"]),
                            part_name_tha = Convert.IsDBNull(detailRow["part_name_tha"]) ? string.Empty : Convert.ToString(detailRow["part_name_tha"]),
                            //description_tha = Convert.IsDBNull(detailRow["description_tha"]) ? string.Empty : Convert.ToString(detailRow["description_tha"]),
                            selling_price = Convert.IsDBNull(detailRow["selling_price"]) ? 0 : Convert.ToInt32(detailRow["selling_price"]),
                            is_selected = false,
                            qty = Convert.IsDBNull(detailRow["quantity"]) ? 0 : Convert.ToInt32(detailRow["quantity"]),
                            qty_balance = Convert.IsDBNull(detailRow["quantity_balance"]) ? 0 : Convert.ToInt32(detailRow["quantity_balance"]),
                            qty_reserve = Convert.IsDBNull(detailRow["quantity_reserve"]) ? 0 : Convert.ToInt32(detailRow["quantity_reserve"])
                        });

                    }
                }
            }

            gridViewPartListSelected.DataSource = servicePartListItemList;
            gridViewPartListSelected.DataBind();

        }
        private void BindGridViewPartListSelected()
        {
            gridViewPartListSelected.DataSource = servicePartListItemList;
            gridViewPartListSelected.DataBind();
        }
        protected void gridViewModel_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            modelHeaderList = new List<ModelServiceHeader>();
            try
            {
                using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                {

                    //Create array of Parameters 
                    List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value =  e.Parameters.ToString() },
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                        };
                    conn.Open();
                    var dsModel = SqlHelper.ExecuteDataset(conn, "sp_maintenance_model_list", arrParm.ToArray());
                    conn.Close();
                    if (dsModel != null)
                    {
                        foreach (var row in dsModel.Tables[0].AsEnumerable())
                        {
                            modelHeaderList.Add(new ModelServiceHeader()
                            {
                                count_package = Convert.IsDBNull(row["count_package"]) ? 0 : Convert.ToInt32(row["count_package"]),
                                count_partlist = Convert.IsDBNull(row["count_partlist"]) ? 0 : Convert.ToInt32(row["count_partlist"]),
                                description_tha = Convert.IsDBNull(row["description_tha"]) ? string.Empty : Convert.ToString(row["description_tha"]),
                                id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                is_enable = Convert.IsDBNull(row["is_enable"]) ? false : Convert.ToBoolean(row["is_enable"]),
                                model_name = Convert.IsDBNull(row["model_name"]) ? string.Empty : Convert.ToString(row["model_name"]),
                            });
                        }
                    }

                }
                gridViewModel.DataSource = modelHeaderList;
                gridViewModel.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void BindGridViewModel()
        {
            gridViewModel.DataSource = modelHeaderList;
            gridViewModel.DataBind();
        }
        [WebMethod]
        public static string NewModel()
        {
            var returnData = string.Empty;
            HttpContext.Current.Session.Remove("SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST");
            HttpContext.Current.Session.Remove("SESSION_SELECTED_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST");
            HttpContext.Current.Session.Remove("SESSION_MODEL_REMARK_SERVICEPARTLIST");

            var partListData = (List<ServicePartListItem>)HttpContext.Current.Session["SESSION_SERVICE_PART_ITEM_SERVICEPARTLIST"];
            if (partListData != null)
            {
                foreach (var row in partListData)
                {
                    row.is_selected = false;
                }
            }


            return returnData;
        }

        [WebMethod]
        public static string SelectAll(bool selected)
        {

            var partListItemData = (List<ServicePartListItem>)HttpContext.Current.Session["SESSION_SERVICE_PART_ITEM_SERVICEPARTLIST"];
            var selectedPartListModelDetail = (List<ServicePartMaintenance>)HttpContext.Current.Session["SESSION_SELECTED_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"];

            if (partListItemData != null)
            {
                foreach (var row in partListItemData)
                {
                    if (selected)
                    {
                        row.is_selected = true;
                        if (selectedPartListModelDetail.Count > 0)
                        {
                            var rowExist = (from t in selectedPartListModelDetail where t.id == row.id select t).FirstOrDefault();

                            if (rowExist != null) // กรณี Newmode
                            {
                                rowExist.qty = row.qty;
                            }
                            else
                            {
                                selectedPartListModelDetail.Add(new ServicePartMaintenance()
                                {
                                    id = (selectedPartListModelDetail.Count + 1) * -1,
                                    item = selectedPartListModelDetail.Count == 0 ? 1 :
                                            (from t in selectedPartListModelDetail select t.item).Max() + 1,
                                    model_id = -1,
                                    part_id = row.id,
                                    part_name = row.part_name_tha,
                                    part_no = row.part_no,
                                    qty = row.qty,
                                    selling_price = row.selling_price,
                                    sort_no = selectedPartListModelDetail.Count == 0 ? 1 :
                                            (from t in selectedPartListModelDetail select t.sort_no).Max() + 1,
                                    total_price = (1 * row.selling_price),
                                    is_delete = false,
                                    type = string.Empty,
                                    no = string.Empty
                                });
                            }

                        }
                        else
                        {
                            selectedPartListModelDetail.Add(new ServicePartMaintenance()
                            {
                                id = (selectedPartListModelDetail.Count + 1) * -1,
                                item = selectedPartListModelDetail.Count == 0 ? 1 :
                                        (from t in selectedPartListModelDetail select t.item).Max() + 1,
                                model_id = -1,
                                part_id = row.id,
                                part_name = row.part_name_tha,
                                part_no = row.part_no,
                                qty = row.qty,
                                selling_price = row.selling_price,
                                sort_no = selectedPartListModelDetail.Count == 0 ? 1 :
                                        (from t in selectedPartListModelDetail select t.sort_no).Max() + 1,
                                total_price = (1 * row.selling_price),
                                is_delete = false,
                                type = string.Empty,
                                no = string.Empty
                            });
                        }

                    }
                    else
                    {
                        var rowExist = (from t in selectedPartListModelDetail where t.part_id == row.id select t).FirstOrDefault();
                        row.is_selected = false;
                        if (rowExist != null)
                        {
                            if (rowExist.id < 0) // กรณี Newmode
                            {
                                selectedPartListModelDetail.Remove(rowExist);
                            }
                            else
                            {
                                rowExist.is_delete = true;
                            }
                        }
                    }
                }
            }
            HttpContext.Current.Session["SESSION_SERVICE_PART_ITEM_SERVICEPARTLIST"] = partListItemData; // ยัดค่ากลับ Session QUotation Detail Data
            HttpContext.Current.Session["SESSION_SELECTED_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"] = selectedPartListModelDetail; // ยัดค่ากลับ Sesssion Sale Order detail data
            return "SELECT ALL";

        }

        [WebMethod]
        public static string SelectAllPartListPackage(bool selected)
        {
            var selectedPartListPackageDetail = (List<ServicePackagePartMapping>)HttpContext.Current.Session["SESSION_SELECTED_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"];
            var partListItemData = (List<ServicePartListItem>)HttpContext.Current.Session["SESSION_SERVICE_PART_PACKAGE_SERVICEPARTLIST"];
            if (partListItemData != null)
            {
                foreach (var row in partListItemData)
                {
                    if (selected)
                    {
                        row.is_selected = true;
                        if (selectedPartListPackageDetail.Count > 0)
                        {
                            var checkExist = (from t in selectedPartListPackageDetail
                                              where t.part_id == Convert.ToInt32(row.id)
                                              select t).FirstOrDefault();
                            if (checkExist == null)
                            {
                                selectedPartListPackageDetail.Add(new ServicePackagePartMapping()
                                {
                                    id = (selectedPartListPackageDetail.Count + 1) * -1,

                                    part_id = row.id,
                                    part_name = row.part_name_tha,
                                    part_no = row.part_no,
                                    is_delete = false,
                                    selling_price = row.selling_price,
                                    item_no = row.item_no,
                                    sort_no = selectedPartListPackageDetail.Count == 0 ? 1 :
                                                (from t in selectedPartListPackageDetail select t.sort_no).Max() + 1

                                });
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            selectedPartListPackageDetail.Add(new ServicePackagePartMapping()
                            {
                                id = (selectedPartListPackageDetail.Count + 1) * -1,

                                part_id = row.id,
                                part_name = row.part_name_tha,
                                part_no = row.part_no,
                                is_delete = false,
                                selling_price = row.selling_price,
                                item_no = row.item_no,
                                sort_no = selectedPartListPackageDetail.Count == 0 ? 1 :
                                                (from t in selectedPartListPackageDetail select t.sort_no).Max() + 1

                            });
                        }
                    }
                    else
                    {
                        var rowExist = (from t in selectedPartListPackageDetail where t.part_id == row.id select t).FirstOrDefault();
                        row.is_selected = false;
                        if (rowExist != null)
                        {
                            if (rowExist.id < 0) // กรณี Newmode
                            {
                                selectedPartListPackageDetail.Remove(rowExist);
                            }
                            else
                            {
                                rowExist.is_delete = true;
                            }
                        }
                    }
                }
            }
            HttpContext.Current.Session["SESSION_SELECTED_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"] = selectedPartListPackageDetail;
            HttpContext.Current.Session["SESSION_SERVICE_PART_PACKAGE_SERVICEPARTLIST"] = partListItemData;

            return "SUCCESS";
        }

        [WebMethod]
        public static ModelServiceHeader GetModelDataEdit(string id, int index)
        {
            //  Set active row
            HttpContext.Current.Session["ROW_ID"] = id;
            HttpContext.Current.Session["ROW"] = index;

            var modelHeaderList = new ModelServiceHeader();
            var partList = new List<ServicePartMaintenance>();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                    new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },
                        };
                conn.Open();
                var dsModel = SqlHelper.ExecuteDataset(conn, "sp_maintenance_model_list", arrParm.ToArray());
                conn.Close();
                if (dsModel != null)
                {
                    var row = (from t in dsModel.Tables[0].AsEnumerable() select t).FirstOrDefault();
                    if (row != null)
                    {
                        modelHeaderList.count_package = Convert.IsDBNull(row["count_package"]) ? 0 : Convert.ToInt32(row["count_package"]);
                        modelHeaderList.count_package = Convert.IsDBNull(row["count_partlist"]) ? 0 : Convert.ToInt32(row["count_partlist"]);

                        modelHeaderList.description_tha = Convert.IsDBNull(row["description_tha"]) ? string.Empty : Convert.ToString(row["description_tha"]);
                        modelHeaderList.id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]);
                        modelHeaderList.model_name = Convert.IsDBNull(row["model_name"]) ? string.Empty : Convert.ToString(row["model_name"]);
                    }
                }

                //Create array of Parameters
                List<SqlParameter> arrParm2 = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@model_id", SqlDbType.Int) { Value = id },
                        };
                conn.Open();
                var dsPartList = SqlHelper.ExecuteDataset(conn, "sp_maintanance_part_list", arrParm2.ToArray());
                conn.Close();
                if (dsPartList != null)
                {
                    var dataPartList = (from t in dsPartList.Tables[0].AsEnumerable() select t).ToList();

                    foreach (var row in dataPartList)
                    {
                        partList.Add(new ServicePartMaintenance()
                        {
                            is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                            item = Convert.IsDBNull(row["item"]) ? 1 : Convert.ToInt32(row["item"]),
                            no = Convert.IsDBNull(row["no"]) ? string.Empty : Convert.ToString(row["no"]),
                            type = Convert.IsDBNull(row["type"]) ? string.Empty : Convert.ToString(row["type"]),
                            part_name = Convert.IsDBNull(row["part_name_tha"]) ? string.Empty : Convert.ToString(row["part_name_tha"]),
                            qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["qty"]),
                            part_no = Convert.IsDBNull(row["part_no"]) ? string.Empty : Convert.ToString(row["part_no"]),
                            part_id = Convert.IsDBNull(row["part_id"]) ? 0 : Convert.ToInt32(row["part_id"]),
                            model_id = Convert.IsDBNull(row["maintenance_model_id"]) ? 0 : Convert.ToInt32(row["maintenance_model_id"]),
                            selling_price = Convert.IsDBNull(row["selling_price"]) ? 0 : Convert.ToInt32(row["selling_price"]),
                            total_price = Convert.IsDBNull(row["total_price"]) ? 0 : Convert.ToInt32(row["total_price"]),
                            sort_no = Convert.IsDBNull(row["sort_no"]) ? 0 : Convert.ToInt32(row["sort_no"])
                        });
                    }

                }
                var modelRemarkList = new List<ModelRemark>();
                List<SqlParameter> arrParm3 = new List<SqlParameter>
                        {
                            new SqlParameter("@model_id", SqlDbType.Int) { Value = id },
                        };
                conn.Open();
                var dsModelRemark = SqlHelper.ExecuteDataset(conn, "sp_maintenance_model_remark_list", arrParm3.ToArray());
                conn.Close();
                if (dsModel != null)
                {
                    var dataRemark = (from t in dsModelRemark.Tables[0].AsEnumerable() select t).ToList();
                    if (dataRemark != null)
                    {
                        foreach (var row in dataRemark)
                        {
                            modelRemarkList.Add(new ModelRemark()
                            {
                                id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                model_id = Convert.IsDBNull(row["model_id"]) ? 0 : Convert.ToInt32(row["model_id"]),
                                remark = Convert.IsDBNull(row["remark"]) ? string.Empty : Convert.ToString(row["remark"])
                            });
                        }
                    }
                }
                HttpContext.Current.Session["SESSION_MODEL_REMARK_SERVICEPARTLIST"] = modelRemarkList;
                HttpContext.Current.Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"] = partList;
            }


            return modelHeaderList;
        }

        [WebMethod]
        public static string GetPartListModel()
        {
            var returnData = string.Empty;
            var selectedPartListModelDetail = (List<ServicePartMaintenance>)HttpContext.Current.Session["SESSION_SELECTED_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"];
            var partListModelDetail = (List<ServicePartMaintenance>)HttpContext.Current.Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"];

            if (partListModelDetail != null)
            {
                selectedPartListModelDetail = partListModelDetail;
            }
            HttpContext.Current.Session["SESSION_SELECTED_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"] = selectedPartListModelDetail;

            return returnData;

        }

        [WebMethod]
        public static string SelectPartListModel(string id, bool isSelected)
        {
            var returnData = string.Empty;
            var partListItemData = (List<ServicePartListItem>)HttpContext.Current.Session["SESSION_SERVICE_PART_ITEM_SERVICEPARTLIST"];
            var selectedPartListModelDetail = (List<ServicePartMaintenance>)HttpContext.Current.Session["SESSION_SELECTED_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"];
            if (partListItemData != null)
            {
                var row = (from t in partListItemData
                           where t.id == Convert.ToInt32(id)
                           select t).FirstOrDefault();
                if (row != null)
                {
                    if (selectedPartListModelDetail == null)
                    {
                        selectedPartListModelDetail = new List<ServicePartMaintenance>();
                    }
                    if (isSelected)
                    {
                        row.is_selected = true;
                        var checkExist = (from t in selectedPartListModelDetail
                                          where t.part_id == Convert.ToInt32(id)
                                          select t).FirstOrDefault();
                        if (checkExist == null)
                        {
                            selectedPartListModelDetail.Add(new ServicePartMaintenance()
                            {
                                id = (selectedPartListModelDetail.Count + 1) * -1,
                                item = selectedPartListModelDetail.Count == 0 ? 1 :
                                            (from t in selectedPartListModelDetail select t.item).Max() + 1,
                                model_id = -1,
                                part_id = row.id,
                                part_name = row.part_name_tha,
                                part_no = row.part_no,
                                qty = 1,//row.qty,
                                selling_price = row.selling_price,
                                sort_no = selectedPartListModelDetail.Count == 0 ? 1 :
                                            (from t in selectedPartListModelDetail select t.sort_no).Max() + 1,
                                total_price = (1 * row.selling_price),
                                is_delete = false,
                                type = string.Empty,
                                no = string.Empty
                            });
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        row.is_selected = false;
                        var checkExist = (from t in selectedPartListModelDetail
                                          where t.part_id == Convert.ToInt32(id)
                                          select t).FirstOrDefault();
                        if (checkExist != null)
                        {
                            if (checkExist.id < 0)
                            {
                                selectedPartListModelDetail.Remove(checkExist);
                            }
                            else
                            {
                                checkExist.is_delete = true;
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
            HttpContext.Current.Session["SESSION_SERVICE_PART_ITEM_SERVICEPARTLIST"] = partListItemData;
            HttpContext.Current.Session["SESSION_SELECTED_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"] = selectedPartListModelDetail;
            return returnData;
        }
        [WebMethod]
        public static string SubmitPartListModel()
        {
            var returnData = string.Empty;
            var selectedPartListModelDetail = (List<ServicePartMaintenance>)HttpContext.Current.Session["SESSION_SELECTED_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"];
            var partListModelDetail = new List<ServicePartMaintenance>(); //(List<ServicePartMaintenance>)HttpContext.Current.Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"];
            if (selectedPartListModelDetail != null)
            {
                partListModelDetail = selectedPartListModelDetail;
            }

            HttpContext.Current.Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"] = partListModelDetail;

            return returnData;
        }

        [WebMethod]
        public static ServicePartMaintenance GetPartListModelDataEdit(string id)
        {
            var returnData = new ServicePartMaintenance();
            var partListModel = (List<ServicePartMaintenance>)HttpContext.Current.Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"];
            if (partListModel != null)
            {
                returnData = (from t in partListModel
                              where t.id == Convert.ToInt32(id)
                              select t).FirstOrDefault();
            }

            return returnData;
        }
        [WebMethod]
        public static string SubmitPartListModelDataEdit(string id, string item, string no, string qty, string type)
        {
            var returnData = string.Empty;
            var partListModel = (List<ServicePartMaintenance>)HttpContext.Current.Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"];

            if (partListModel != null)
            {
                var row = (from t in partListModel
                           where t.id == Convert.ToInt32(id)
                           select t).FirstOrDefault();
                if (row != null)
                {
                    row.no = no;
                    row.item = Convert.ToInt32(item);
                    row.qty = Convert.ToInt32(qty);
                    row.type = type;
                    row.total_price = Convert.ToInt32(qty) * row.selling_price;
                }

            }
            HttpContext.Current.Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"] = partListModel;
            return returnData;
        }

        protected void gridViewPartListSelected_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxCheckBox checkBox = gridViewPartListSelected.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkBox") as ASPxCheckBox;
                if (checkBox != null)
                {
                    if (e.DataColumn.FieldName == "is_selected")
                    {
                        if (servicePartListMaintenanceList != null)
                        {
                            var row = (from t in servicePartListMaintenanceList where t.part_id == Convert.ToInt32(e.KeyValue) select t).FirstOrDefault();
                            if (row != null)
                            {
                                checkBox.Checked = true;
                            }
                            else
                            {
                                checkBox.Checked = false;
                            }
                        }
                        //checkBox.Checked = (row["is_selected"]) == DBNull.Value ? false : true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveDataModel()
        {

            int newID = dataId;
            dataId = Convert.ToInt32(hdModelId.Value);
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {

                        if (dataId == 0)
                        {
                            #region Model Header
                            //Quotation Header
                            using (SqlCommand cmd = new SqlCommand("sp_maintenance_model_add", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@model_name", SqlDbType.VarChar, 150).Value = txtModelName.Value;
                                cmd.Parameters.Add("@description_tha", SqlDbType.VarChar, 150).Value = txtModelDescription.Value;
                                cmd.Parameters.Add("@description_eng", SqlDbType.VarChar, 150).Value = txtModelDescription.Value;
                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                newID = Convert.ToInt32(cmd.ExecuteScalar());

                            }
                            #endregion Model Header

                            //Create array of Parameters
                            #region Model PartList
                            // Quotation Detail
                            var oldDetailId = 0;
                            foreach (var row in servicePartListMaintenanceList)
                            {
                                oldDetailId = row.id;

                                using (SqlCommand cmd = new SqlCommand("sp_maintanance_part_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@part_no", SqlDbType.VarChar, 50).Value = row.part_no;
                                    cmd.Parameters.Add("@part_id", SqlDbType.Int).Value = row.part_id;
                                    cmd.Parameters.Add("@maintenance_model_id", SqlDbType.Int).Value = newID;
                                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                    cmd.Parameters.Add("@selling_price", SqlDbType.Int).Value = row.selling_price;
                                    cmd.Parameters.Add("@total_price", SqlDbType.Int).Value = row.total_price;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                    cmd.Parameters.Add("@type", SqlDbType.VarChar, 10).Value = row.type;
                                    cmd.Parameters.Add("@item", SqlDbType.Int).Value = row.item;
                                    cmd.Parameters.Add("@no", SqlDbType.VarChar, 10).Value = row.no;
                                    cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;

                                    cmd.ExecuteNonQuery();

                                }
                            }

                            #endregion Model PartList

                            #region Remark
                            foreach (var row in modelRemarkList)
                            {

                                using (SqlCommand cmd = new SqlCommand("sp_maintenance_model_remark_add", conn, tran))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@model_id", SqlDbType.Int).Value = newID;
                                    cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = row.remark;
                                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                    cmd.ExecuteNonQuery();

                                }
                            }
                            #endregion Remark
                        }
                        else
                        {
                            #region Model Header
                            //Quotation Header
                            using (SqlCommand cmd = new SqlCommand("sp_maintenance_model_edit", conn, tran))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = dataId;
                                cmd.Parameters.Add("@model_name", SqlDbType.VarChar, 150).Value = txtModelName.Value;
                                cmd.Parameters.Add("@description_tha", SqlDbType.VarChar, 150).Value = txtModelDescription.Value;
                                cmd.Parameters.Add("@description_eng", SqlDbType.VarChar, 150).Value = txtModelDescription.Value;
                                cmd.Parameters.Add("@is_enable", SqlDbType.Bit).Value = 1;
                                //cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = 0;
                                cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                cmd.ExecuteNonQuery();

                            }
                            #endregion Model Header

                            //Create array of Parameters
                            #region Model PartList
                            // Quotation Detail
                            var oldDetailId = 0;
                            foreach (var row in servicePartListMaintenanceList)
                            {
                                oldDetailId = row.id;
                                if (row.id < 0)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_maintanance_part_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@part_id", SqlDbType.Int).Value = row.part_id;
                                        cmd.Parameters.Add("@part_no", SqlDbType.VarChar, 50).Value = row.part_no;
                                        cmd.Parameters.Add("@maintenance_model_id", SqlDbType.Int).Value = dataId;
                                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                        cmd.Parameters.Add("@selling_price", SqlDbType.Int).Value = row.selling_price;
                                        cmd.Parameters.Add("@total_price", SqlDbType.Int).Value = row.total_price;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        cmd.Parameters.Add("@type", SqlDbType.VarChar, 10).Value = row.type;
                                        cmd.Parameters.Add("@item", SqlDbType.Int).Value = row.item;
                                        cmd.Parameters.Add("@no", SqlDbType.VarChar, 10).Value = row.no;
                                        cmd.Parameters.Add("@sort_no", SqlDbType.VarChar, 10).Value = row.sort_no;

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                                else
                                {
                                    if (!row.is_delete)
                                    {
                                        try
                                        {
                                            using (SqlCommand cmd = new SqlCommand("sp_maintanance_part_edit", conn, tran))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;

                                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                                cmd.Parameters.Add("@part_id", SqlDbType.Int).Value = row.part_id;
                                                cmd.Parameters.Add("@part_no", SqlDbType.VarChar, 50).Value = row.part_no;
                                                cmd.Parameters.Add("@maintenance_model_id", SqlDbType.Int).Value = dataId;
                                                cmd.Parameters.Add("@qty", SqlDbType.Int).Value = row.qty;
                                                cmd.Parameters.Add("@selling_price", SqlDbType.Int).Value = row.selling_price;
                                                cmd.Parameters.Add("@total_price", SqlDbType.Int).Value = row.total_price;
                                                cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                                //cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = row.is_delete;
                                                cmd.Parameters.Add("@type", SqlDbType.VarChar, 10).Value = row.type;
                                                cmd.Parameters.Add("@item", SqlDbType.Int).Value = row.item;
                                                cmd.Parameters.Add("@no", SqlDbType.VarChar, 10).Value = row.no;
                                                cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;

                                                cmd.ExecuteNonQuery();

                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new Exception(ex.Message);
                                        }
                                    }
                                    else
                                    {
                                        using (SqlCommand cmd = new SqlCommand("sp_maintanance_part_delete", conn, tran))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;

                                            cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                }
                            }

                            #endregion Model PartList

                            #region Remark
                            foreach (var row in modelRemarkList)
                            {
                                if (row.id < 0)
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_maintenance_model_remark_add", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add("@model_id", SqlDbType.Int).Value = dataId;
                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = row.remark;
                                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                                else
                                {
                                    using (SqlCommand cmd = new SqlCommand("sp_maintenance_model_remark_edit", conn, tran))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                        cmd.Parameters.Add("@model_id", SqlDbType.Int).Value = dataId;
                                        cmd.Parameters.Add("@remark", SqlDbType.VarChar, 500).Value = row.remark;
                                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                        cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = row.is_delete;

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                            }
                            #endregion Remark
                        }

                        //  Set new id
                        HttpContext.Current.Session["ROW_ID"] = Convert.ToString(newID);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        tran.Dispose();
                        conn.Close();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", "alertMessage('เกิดข้อผิดพลาดบางอย่าง ( กรุณาตรวจสอบ item number ) ','E')", true);
                        //throw ex;
                    }
                    finally
                    {
                        if (!conn.State.Equals(ConnectionState.Closed))
                        {

                            tran.Commit();
                            tran.Dispose();
                            conn.Close();

                            Response.Redirect("ServicePartList.aspx");
                        }
                    }
                }
            }
        }

        protected void btnSaveModel_Click(object sender, EventArgs e)
        {
            SaveDataModel();
        }

        [WebMethod]
        public static string DeleteModel(string id)
        {
            var returnData = "";
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("sp_maintanance_model_delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                returnData = "success";
            }
            return returnData;
        }
        [WebMethod]
        public static string DeletePartListModel(string id)
        {
            var returnData = string.Empty;
            var partListModel = (List<ServicePartMaintenance>)HttpContext.Current.Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"];
            if (partListModel != null)
            {
                var row = (from t in partListModel
                           where t.id == Convert.ToInt32(id)
                           select t).FirstOrDefault();
                if (row != null)
                {
                    if (row.id > 0)
                    {
                        row.is_delete = true;
                    }
                    else
                    {
                        partListModel.Remove(row);
                    }
                }
            }
            HttpContext.Current.Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"] = partListModel;
            return returnData;
        }
        #endregion

        #region Package
        [WebMethod]
        public static string NewPackage()
        {
            var returnData = string.Empty;
            HttpContext.Current.Session.Remove("SESSION_SERVICE_PACKAGE_SERVICEPARTLIST");
            HttpContext.Current.Session.Remove("SESSION_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST");
            HttpContext.Current.Session.Remove("SESSION_SELECTED_SERVICE_DESCRIPTION_PACKAGE_");
            return returnData;
        }

        [WebMethod]
        public static string ViewPackageList(string model_id)
        {
            var returnData = string.Empty;
            var data = new List<ServicePackage>();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@model_id", SqlDbType.Int) { Value = model_id },
                            new SqlParameter("@package_id", SqlDbType.Int) { Value = 0 },
                        };
                conn.Open();
                var dsPartList = SqlHelper.ExecuteDataset(conn, "sp_maintenance_package_list", arrParm.ToArray());
                conn.Close();
                if (dsPartList != null)
                {
                    var dataPartList = (from t in dsPartList.Tables[0].AsEnumerable() select t).ToList();

                    foreach (var row in dataPartList)
                    {
                        data.Add(new ServicePackage()
                        {
                            count_year = Convert.IsDBNull(row["count_year"]) ? 0 : Convert.ToInt32(row["count_year"]),
                            id = Convert.IsDBNull(row["package_id"]) ? 0 : Convert.ToInt32(row["package_id"]),
                            item_of_part = Convert.IsDBNull(row["item_of_part"]) ? string.Empty : Convert.ToString(row["item_of_part"]),
                            running_hours = Convert.IsDBNull(row["running_hours"]) ? string.Empty : Convert.ToString(row["running_hours"]),
                            service_charge = Convert.IsDBNull(row["service_charge"]) ? 0 : Convert.ToInt32(row["service_charge"]),
                            overhaul_service = Convert.IsDBNull(row["overhaul_service"]) ? string.Empty : Convert.ToString(row["overhaul_service"]),
                            overhaul_servicedes = Convert.IsDBNull(row["overhaul_servicedes"]) ? string.Empty : Convert.ToString(row["overhaul_servicedes"]),
                            overhaul_rate = Convert.IsDBNull(row["overhaul_rate"]) ? string.Empty : Convert.ToString(row["overhaul_rate"]),
                            model_id = Convert.IsDBNull(row["model_id"]) ? 0 : Convert.ToInt32(row["model_id"]),
                            is_enable = Convert.IsDBNull(row["is_enable"]) ? false : Convert.ToBoolean(row["is_enable"]),
                            total_price = Convert.IsDBNull(row["total_price"]) ? 0 : Convert.ToInt32(row["total_price"]),
                            count_part_list = Convert.IsDBNull(row["count_part_list"]) ? 0 : Convert.ToInt32(row["count_part_list"]),
                        });
                    }


                }
                /// NAME
                //Create array of Parameters
                List<SqlParameter> arrParm2 = new List<SqlParameter>
                        {
                    new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value ="" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = model_id },
                        };
                conn.Open();
                var dsModel = SqlHelper.ExecuteDataset(conn, "sp_maintenance_model_list", arrParm2.ToArray());
                conn.Close();
                if (dsModel != null)
                {
                    var row = (from t in dsModel.Tables[0].AsEnumerable() select t).FirstOrDefault();
                    if (row != null)
                    {
                        returnData = Convert.IsDBNull(row["model_name"]) ? string.Empty : Convert.ToString(row["model_name"]);
                    }
                }
            }
            HttpContext.Current.Session["SESSION_SERVICE_PACKAGE_SERVICEPARTLIST"] = data;
            return returnData;
        }

        [WebMethod]
        public static string ViewPartListPackage(string model_id)
        {
            var returnData = string.Empty;
            var data = new List<ServicePartListItem>();

            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@model_id", SqlDbType.Int) { Value = model_id },
                        };
                conn.Open();
                var dsPartList = SqlHelper.ExecuteDataset(conn, "sp_maintanance_part_list", arrParm.ToArray());
                conn.Close();
                if (dsPartList != null)
                {
                    var dataPartList = (from t in dsPartList.Tables[0].AsEnumerable() select t).ToList();

                    foreach (var row in dataPartList)
                    {
                        data.Add(new ServicePartListItem()
                        {
                            //description_tha = Convert.IsDBNull(row["description_tha"]) ? string.Empty : Convert.ToString(row["description_tha"]),
                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                            item_no = Convert.IsDBNull(row["item"]) ? string.Empty : Convert.ToString(row["item"]),
                            part_name_tha = Convert.IsDBNull(row["part_name_tha"]) ? string.Empty : Convert.ToString(row["part_name_tha"]),
                            qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["qty"]),
                            part_no = Convert.IsDBNull(row["part_no"]) ? string.Empty : Convert.ToString(row["part_no"]),
                            qty_balance = Convert.IsDBNull(row["quantity_balance"]) ? 0 : Convert.ToInt32(row["quantity_balance"]),
                            qty_reserve = Convert.IsDBNull(row["quantity_reserve"]) ? 0 : Convert.ToInt32(row["quantity_reserve"]),
                            selling_price = Convert.IsDBNull(row["selling_price"]) ? 0 : Convert.ToInt32(row["selling_price"]),
                            is_selected = false,

                        });
                    }

                }
            }

            HttpContext.Current.Session["SESSION_SERVICE_PART_PACKAGE_SERVICEPARTLIST"] = data;
            var packagePartListMappingData = (List<ServicePackagePartMapping>)HttpContext.Current.Session["SESSION_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"];
            HttpContext.Current.Session["SESSION_SELECTED_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"] = packagePartListMappingData;
            return returnData;
        }

        [WebMethod]
        public static string SelectPartListPackage(string id, bool isSelected)
        {
            var returnData = string.Empty;
            var selectedPartListPackageDetail = (List<ServicePackagePartMapping>)HttpContext.Current.Session["SESSION_SELECTED_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"];
            var partListItemData = (List<ServicePartListItem>)HttpContext.Current.Session["SESSION_SERVICE_PART_PACKAGE_SERVICEPARTLIST"];
            if (partListItemData != null)
            {
                var row = (from t in partListItemData
                           where t.id == Convert.ToInt32(id)
                           select t).FirstOrDefault();
                if (row != null)
                {
                    if (selectedPartListPackageDetail == null)
                    {
                        selectedPartListPackageDetail = new List<ServicePackagePartMapping>();
                    }
                    if (isSelected)
                    {
                        row.is_selected = true;
                        var checkExist = (from t in selectedPartListPackageDetail
                                          where t.part_id == Convert.ToInt32(id)
                                          select t).FirstOrDefault();
                        if (checkExist == null)
                        {
                            selectedPartListPackageDetail.Add(new ServicePackagePartMapping()
                            {
                                id = (selectedPartListPackageDetail.Count + 1) * -1,

                                part_id = row.id,
                                part_name = row.part_name_tha,
                                part_no = row.part_no,
                                is_delete = false,
                                selling_price = row.selling_price,
                                item_no = row.item_no,
                                sort_no = selectedPartListPackageDetail.Count == 0 ? 1 :
                                            (from t in selectedPartListPackageDetail select t.sort_no).Max() + 1

                            });
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        row.is_selected = false;
                        var checkExist = (from t in selectedPartListPackageDetail
                                          where t.part_id == Convert.ToInt32(id)
                                          select t).FirstOrDefault();
                        if (checkExist != null)
                        {
                            if (checkExist.id < 0)
                            {
                                selectedPartListPackageDetail.Remove(checkExist);
                            }
                            else
                            {
                                checkExist.is_delete = true;
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
            HttpContext.Current.Session["SESSION_SELECTED_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"] = selectedPartListPackageDetail;
            HttpContext.Current.Session["SESSION_SERVICE_PART_PACKAGE_SERVICEPARTLIST"] = partListItemData;

            return returnData;
        }

        [WebMethod]
        public static ServicePackage GetPackageDataEdit(string id)
        {
            var returnData = new ServicePackage();
            var partListData = new List<ServicePackagePartMapping>();
            var DescListData = new List<ServicePackageDescription>();
            var packageListData = (List<ServicePackage>)HttpContext.Current.Session["SESSION_SERVICE_PACKAGE_SERVICEPARTLIST"];
            if (packageListData != null)
            {
                var row = (from t in packageListData
                           where t.id == Convert.ToInt32(id)
                           select t).FirstOrDefault();
                if (row != null)
                {
                    returnData = row;
                }
            }
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@package_id", SqlDbType.Int) { Value = id },
                        };
                conn.Open();
                var dsPartList = SqlHelper.ExecuteDataset(conn, "sp_maintanance_package_part_mapping_list", arrParm.ToArray());
                conn.Close();
                if (dsPartList != null)
                {
                    var dataPartList = (from t in dsPartList.Tables[0].AsEnumerable() select t).ToList();

                    foreach (var row in dataPartList)
                    {
                        partListData.Add(new ServicePackagePartMapping()
                        {
                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                            item_no = Convert.IsDBNull(row["item"]) ? string.Empty : Convert.ToString(row["item"]),
                            part_name = Convert.IsDBNull(row["part_name_tha"]) ? string.Empty : Convert.ToString(row["part_name_tha"]),
                            part_no = Convert.IsDBNull(row["part_no"]) ? string.Empty : Convert.ToString(row["part_no"]),
                            selling_price = Convert.IsDBNull(row["selling_price"]) ? 0 : Convert.ToInt32(row["selling_price"]),
                            part_id = Convert.IsDBNull(row["maintanance_part_id"]) ? 0 : Convert.ToInt32(row["maintanance_part_id"]),
                            sort_no = Convert.IsDBNull(row["sort_no"]) ? 1 : Convert.ToInt32(row["sort_no"]),
                        });
                    }

                }
                HttpContext.Current.Session["SESSION_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"] = partListData;

                conn.Open();
                var dsDescriptList = SqlHelper.ExecuteDataset(conn, "sp_maintanance_package_description_list", arrParm.ToArray());
                conn.Close();
                if (dsDescriptList != null)
                {
                    var dataDescList = (from t in dsDescriptList.Tables[0].AsEnumerable() select t).ToList();

                    foreach (var row in dataDescList)
                    {
                        DescListData.Add(new ServicePackageDescription()
                        {
                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                            description = Convert.IsDBNull(row["description"]) ? string.Empty : Convert.ToString(row["description"])
                        });
                    }

                }
                HttpContext.Current.Session["SESSION_SELECTED_SERVICE_DESCRIPTION_PACKAGE_"] = DescListData;
            }
            return returnData;
        }

        [WebMethod]
        public static string DeletePackageItem(string id)
        {
            var returnData = string.Empty;
            var packageListData = (List<ServicePackage>)HttpContext.Current.Session["SESSION_SERVICE_PACKAGE_SERVICEPARTLIST"];
            if (packageListData != null)
            {
                var row = (from t in packageListData
                           where t.id == Convert.ToInt32(id)
                           select t).FirstOrDefault();
                if (row != null)
                {
                    if (row.id > 0)
                    {
                        row.is_deleted = true;
                    }
                    else
                    {
                        packageListData.Remove(row);
                    }
                }
            }
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                #region Package Header
                //Quotation Header
                using (SqlCommand cmd = new SqlCommand("sp_maintanance_package_delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                #endregion Package Header

            }
            HttpContext.Current.Session["SESSION_SERVICE_PACKAGE_SERVICEPARTLIST"] = packageListData;

            return returnData;
        }

        [WebMethod]
        public static int DeletePartListPackage(string id)
        {
            var returnData = 0;
            var partListPackage = (List<ServicePackagePartMapping>)HttpContext.Current.Session["SESSION_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"];
            if (partListPackage != null)
            {
                var row = (from t in partListPackage
                           where t.id == Convert.ToInt32(id)
                           select t).FirstOrDefault();
                if (row != null)
                {
                    if (row.id > 0)
                    {
                        row.is_delete = true;
                    }
                    else
                    {
                        partListPackage.Remove(row);
                    }
                }
                foreach (var rowReturn in (from t in partListPackage where !t.is_delete select t).ToList())
                {
                    returnData += rowReturn.selling_price;
                }
            }
            HttpContext.Current.Session["SESSION_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"] = partListPackage;
            return returnData;
        }
        [WebMethod]
        public static int SubmitPartListPackage()
        {
            var returnData = 0;
            var selectedData = (List<ServicePackagePartMapping>)HttpContext.Current.Session["SESSION_SELECTED_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"];
            if (selectedData != null)
            {
                HttpContext.Current.Session["SESSION_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"] = selectedData;
                HttpContext.Current.Session.Remove("SESSION_SELECTED_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST");
            }
            foreach (var row in (from t in selectedData where !t.is_delete select t).ToList())
            {
                returnData += row.selling_price;
            }
            return returnData;
        }
        protected void gridViewPackage_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //  What?
            //if (e.Parameters.Length < 2)
            if (e.Parameters.Contains("model_id"))
            {
                var splitStr = e.Parameters.ToString().Split('|');
                var model_id = splitStr[1];
                if (model_id.Length > 0)
                {

                    var data = new List<ServicePackage>();
                    using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
                    {
                        //Create array of Parameters
                        List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@model_id", SqlDbType.Int) { Value =Convert.ToInt32(model_id) },
                            new SqlParameter("@package_id", SqlDbType.Int) { Value = 0 },
                        };
                        conn.Open();
                        var dsPartList = SqlHelper.ExecuteDataset(conn, "sp_maintenance_package_list", arrParm.ToArray());
                        conn.Close();
                        if (dsPartList != null)
                        {
                            var dataPartList = (from t in dsPartList.Tables[0].AsEnumerable() select t).ToList();

                            foreach (var row in dataPartList)
                            {
                                data.Add(new ServicePackage()
                                {
                                    count_year = Convert.IsDBNull(row["count_year"]) ? 0 : Convert.ToInt32(row["count_year"]),
                                    id = Convert.IsDBNull(row["package_id"]) ? 0 : Convert.ToInt32(row["package_id"]),
                                    item_of_part = Convert.IsDBNull(row["item_of_part"]) ? string.Empty : Convert.ToString(row["item_of_part"]),
                                    running_hours = Convert.IsDBNull(row["running_hours"]) ? string.Empty : Convert.ToString(row["running_hours"]),
                                    service_charge = Convert.IsDBNull(row["service_charge"]) ? 0 : Convert.ToInt32(row["service_charge"]),
                                    overhaul_service = Convert.IsDBNull(row["overhaul_service"]) ? string.Empty : Convert.ToString(row["overhaul_service"]),
                                    overhaul_servicedes = Convert.IsDBNull(row["overhaul_servicedes"]) ? string.Empty : Convert.ToString(row["overhaul_servicedes"]),
                                    overhaul_rate = Convert.IsDBNull(row["overhaul_rate"]) ? string.Empty : Convert.ToString(row["overhaul_rate"]),

                                    model_id = Convert.IsDBNull(row["model_id"]) ? 0 : Convert.ToInt32(row["model_id"]),
                                    is_enable = Convert.IsDBNull(row["is_enable"]) ? false : Convert.ToBoolean(row["is_enable"]),
                                    total_price = Convert.IsDBNull(row["total_price"]) ? 0 : Convert.ToInt32(row["total_price"]),
                                    count_part_list = Convert.IsDBNull(row["count_part_list"]) ? 0 : Convert.ToInt32(row["count_part_list"]),
                                });
                            }


                        }

                    }
                    HttpContext.Current.Session["SESSION_SERVICE_PACKAGE_SERVICEPARTLIST"] = data;

                    gridViewPackage.DataSource = (from t in data
                                                  where t.is_deleted == false
                                                  select t).ToList();
                    gridViewPackage.DataBind();
                }
            }
            else
            {
                var splitStr = e.Parameters.ToString().Split('|');
                string searchText = string.Empty;

                if (splitStr.Length > 1)
                {
                    searchText = splitStr[1];
                }
                if (string.IsNullOrEmpty(searchText))
                {
                    gridViewPackage.DataSource = (from t in servicePackageList
                                                  where t.is_deleted == false
                                                  select t).ToList();
                    gridViewPackage.DataBind();
                }
                else
                {
                    gridViewPackage.DataSource = (from t in servicePackageList
                                                  where (t.running_hours.ToUpper().Contains(searchText.ToUpper()) || t.item_of_part.ToUpper().Contains(searchText.ToUpper()))
                                                  where t.is_deleted == false
                                                  select t).ToList();
                    gridViewPackage.DataBind();
                }
            }

        }
        private void BindGridViewPackage()
        {
            gridViewPackage.DataSource = (from t in servicePackageList
                                          where t.is_deleted == false
                                          select t).ToList();
            gridViewPackage.DataBind();
        }

        protected void gridViewPartListPackage_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridViewPartListPackage.DataSource = (from t in servicePartPackageMappingList
                                                  where t.is_delete == false
                                                  orderby t.sort_no ascending
                                                  select t).ToList();
            gridViewPartListPackage.DataBind();
        }




        private void BindGridViewPartListPackage()
        {
            gridViewPartListPackage.DataSource = (from t in servicePartPackageMappingList
                                                  where t.is_delete == false
                                                  orderby t.sort_no ascending
                                                  select t).ToList();
            gridViewPartListPackage.DataBind();
        }

        protected void gridViewPartListPackageSelect_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var splitStr = e.Parameters.ToString().Split('|');
            string searchText = string.Empty;

            if (splitStr.Length > 1)
            {
                searchText = splitStr[1];
            }
            if (string.IsNullOrEmpty(searchText))
            {
                gridViewPartListPackageSelect.DataSource = servicePartListPackageSelectList;
                gridViewPartListPackageSelect.DataBind();
            }
            else
            {
                gridViewPartListPackageSelect.DataSource = (from t in servicePartListPackageSelectList
                                                            where (t.part_no.ToUpper().Contains(searchText.ToUpper()) || t.part_name_tha.ToUpper().Contains(searchText.ToUpper()))
                                                            select t).ToList();
                gridViewPartListPackageSelect.DataBind();
            }


        }
        private void BindGridViewPartListPackageSelect()
        {
            gridViewPartListPackageSelect.DataSource = servicePartListPackageSelectList;
            gridViewPartListPackageSelect.DataBind();
        }

        protected void gridViewPartListPackageSelect_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxCheckBox checkBox = gridViewPartListPackageSelect.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chkBox") as ASPxCheckBox;
                if (checkBox != null)
                {
                    if (e.DataColumn.FieldName == "is_selected")
                    {
                        if (selectedServicePartPackageMappingList != null)
                        {
                            var row = (from t in selectedServicePartPackageMappingList where t.part_id == Convert.ToInt32(e.KeyValue) select t).FirstOrDefault();
                            if (row != null)
                            {
                                checkBox.Checked = true;
                            }
                            else
                            {
                                checkBox.Checked = false;
                            }
                        }
                        //checkBox.Checked = (row["is_selected"]) == DBNull.Value ? false : true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        protected void btnSavePackage_Click(object sender, EventArgs e)
        {
            //SavePackage();
        }
        [WebMethod]
        public static string SavePackage(PackageData[] Data)
        {
            var selectedServicePackage = (List<ServicePackage>)HttpContext.Current.Session["SESSION_SERVICE_PACKAGE_SERVICEPARTLIST"];
            var dateReturn = "success";
            int newID = 0;
            int modelId = 0;
            var totalPrice = 0;
            var servicePartPackageMappingList = (List<ServicePackagePartMapping>)HttpContext.Current.Session["SESSION_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"];
            var servicePartPackageDescriptionList = (List<ServicePackageDescription>)HttpContext.Current.Session["SESSION_SELECTED_SERVICE_DESCRIPTION_PACKAGE_"];

            foreach (var row in (from t in servicePartPackageMappingList where !t.is_delete select t).ToList())
            {
                totalPrice += row.selling_price;
            }
            var dataPackage = (from t in Data select t).FirstOrDefault();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                modelId = Convert.ToInt32(dataPackage.ModelId);
                var dataId = Convert.ToInt32(dataPackage.PackageId) == 0 ? 0 : Convert.ToInt32(dataPackage.PackageId);
                if (dataId == 0)
                {
                    #region Package Header
                    //Quotation Header
                    using (SqlCommand cmd = new SqlCommand("sp_maintanance_package_add", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@service_maintenance_id", SqlDbType.Int).Value = modelId;
                        cmd.Parameters.Add("@count_year", SqlDbType.Int).Value = dataPackage.PackageCountYear;
                        cmd.Parameters.Add("@item_of_part", SqlDbType.VarChar, 100).Value = dataPackage.PackageItemOfPart;
                        cmd.Parameters.Add("@running_hours", SqlDbType.VarChar, 100).Value = dataPackage.PackageRunningHours;
                        cmd.Parameters.Add("@total_price", SqlDbType.Decimal).Value = totalPrice;//txtPackageTotalPrice.Value;
                        cmd.Parameters.Add("@service_charge", SqlDbType.Decimal).Value = dataPackage.PackageServiceChange;
                        cmd.Parameters.Add("@overhaul_service", SqlDbType.VarChar, 50).Value = dataPackage.Packageoverhaul_service;
                        cmd.Parameters.Add("@overhaul_servicedes", SqlDbType.VarChar, 100).Value = dataPackage.Packageoverhaul_servicedes;
                        cmd.Parameters.Add("@overhaul_rate", SqlDbType.VarChar, 50).Value = dataPackage.Packageoverhaul_rate;
                        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                        conn.Open();
                        newID = Convert.ToInt32(cmd.ExecuteScalar());
                        conn.Close();
                    }
                    #endregion Package Header

                    //Create array of Parameters
                    #region Package PartList
                    // Quotation Detail
                    var oldDetailId = 0;
                    foreach (var row in servicePartPackageMappingList)
                    {
                        oldDetailId = row.id;

                        using (SqlCommand cmd = new SqlCommand("sp_maintanance_package_part_mapping_add", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@maintanance_part_id", SqlDbType.Int).Value = row.part_id;
                            cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                            cmd.Parameters.Add("@maintanance_package_id", SqlDbType.Int).Value = newID;
                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }

                    #endregion Package PartList

                    #region Package Description
                    foreach (var row in servicePartPackageDescriptionList)
                    {
                        oldDetailId = row.id;

                        using (SqlCommand cmd = new SqlCommand("sp_maintanance_package_description_add", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@description", SqlDbType.VarChar, 250).Value = row.description;
                            cmd.Parameters.Add("@maintanance_package_id", SqlDbType.Int).Value = newID;
                            cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Package Header
                    //Quotation Header
                    using (SqlCommand cmd = new SqlCommand("sp_maintanance_package_edit", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = dataId;
                        cmd.Parameters.Add("@service_maintenance_id", SqlDbType.Int).Value = modelId;
                        cmd.Parameters.Add("@count_year", SqlDbType.Int).Value = dataPackage.PackageCountYear;
                        cmd.Parameters.Add("@item_of_part", SqlDbType.VarChar, 100).Value = dataPackage.PackageItemOfPart;
                        cmd.Parameters.Add("@running_hours", SqlDbType.VarChar, 100).Value = dataPackage.PackageRunningHours;
                        cmd.Parameters.Add("@total_price", SqlDbType.Decimal).Value = totalPrice;//txtPackageTotalPrice.Value;
                        cmd.Parameters.Add("@service_charge", SqlDbType.Decimal).Value = dataPackage.PackageServiceChange;
                        cmd.Parameters.Add("@overhaul_service", SqlDbType.VarChar, 50).Value = dataPackage.Packageoverhaul_service;
                        cmd.Parameters.Add("@overhaul_servicedes", SqlDbType.VarChar, 100).Value = dataPackage.Packageoverhaul_servicedes;
                        cmd.Parameters.Add("@overhaul_rate", SqlDbType.VarChar, 50).Value = dataPackage.Packageoverhaul_rate;
                        cmd.Parameters.Add("@is_enable", SqlDbType.Bit).Value = 1;
                        cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = 0;
                        cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    #endregion Package Header

                    //Create array of Parameters
                    #region Package PartList
                    // Quotation Detail
                    var oldDetailId = 0;
                    foreach (var row in servicePartPackageMappingList)
                    {
                        oldDetailId = row.id;
                        if (row.id < 0)
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_maintanance_package_part_mapping_add", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                cmd.Parameters.Add("@maintanance_part_id", SqlDbType.Int).Value = row.part_id;
                                cmd.Parameters.Add("@maintanance_package_id", SqlDbType.Int).Value = dataId;
                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                conn.Open();
                                cmd.ExecuteNonQuery();
                                conn.Close();
                            }
                        }
                        else
                        {
                            if (!row.is_delete)
                            {
                                //throw new Exception("x");
                                using (SqlCommand cmd = new SqlCommand("sp_maintanance_package_part_mapping_edit", conn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                    cmd.Parameters.Add("@maintanance_part_id", SqlDbType.Int).Value = row.part_id;
                                    cmd.Parameters.Add("@sort_no", SqlDbType.Int).Value = row.sort_no;
                                    cmd.Parameters.Add("@maintanance_package_id", SqlDbType.Int).Value = dataId;
                                    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);
                                    cmd.Parameters.Add("@is_delete", SqlDbType.Bit).Value = row.is_delete;
                                    cmd.Parameters.Add("@is_enable", SqlDbType.Bit).Value = 1;

                                    conn.Open();
                                    cmd.ExecuteNonQuery();
                                    conn.Close();
                                }
                            }
                            else
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_maintanance_package_part_mapping_delete", conn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                                    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);


                                    conn.Open();
                                    cmd.ExecuteNonQuery();
                                    conn.Close();
                                }
                            }
                        }
                    }

                    #endregion Package PartList

                    #region Package Description

                    foreach (var row in servicePartPackageDescriptionList)
                    {
                        oldDetailId = row.id;
                        if (row.id < 0)
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_maintanance_package_description_add", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@description", SqlDbType.VarChar, 250).Value = row.description;
                                cmd.Parameters.Add("@maintanance_package_id", SqlDbType.Int).Value = dataId;
                                cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                conn.Open();
                                cmd.ExecuteNonQuery();
                                conn.Close();
                            }
                        }
                        else
                        {
                                //throw new Exception("x");
                                using (SqlCommand cmd = new SqlCommand("sp_maintanance_package_description_edit", conn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(row.id);
                                    cmd.Parameters.Add("@description", SqlDbType.VarChar,250).Value = row.description;
                                    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                                    conn.Open();
                                    cmd.ExecuteNonQuery();
                                    conn.Close();
                                }
                        }
                    }

                    #endregion
                }

            }
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                            new SqlParameter("@model_id", SqlDbType.Int) { Value = dataPackage.ModelId },
                            new SqlParameter("@package_id", SqlDbType.Int) { Value = 0 },
                        };
                conn.Open();
                var dsPartList = SqlHelper.ExecuteDataset(conn, "sp_maintenance_package_list", arrParm.ToArray());
                conn.Close();
                if (dsPartList != null)
                {
                    var dataPartList = (from t in dsPartList.Tables[0].AsEnumerable() select t).ToList();
                    selectedServicePackage.Clear();
                    foreach (var row in dataPartList)
                    {
                        selectedServicePackage.Add(new ServicePackage()
                        {
                            count_year = Convert.IsDBNull(row["count_year"]) ? 0 : Convert.ToInt32(row["count_year"]),
                            id = Convert.IsDBNull(row["package_id"]) ? 0 : Convert.ToInt32(row["package_id"]),
                            item_of_part = Convert.IsDBNull(row["item_of_part"]) ? string.Empty : Convert.ToString(row["item_of_part"]),
                            running_hours = Convert.IsDBNull(row["running_hours"]) ? string.Empty : Convert.ToString(row["running_hours"]),
                            service_charge = Convert.IsDBNull(row["service_charge"]) ? 0 : Convert.ToInt32(row["service_charge"]),
                            overhaul_service = Convert.IsDBNull(row["overhaul_service"]) ? string.Empty : Convert.ToString(row["overhaul_service"]),
                            overhaul_servicedes = Convert.IsDBNull(row["overhaul_servicedes"]) ? string.Empty : Convert.ToString(row["overhaul_servicedes"]),
                            overhaul_rate = Convert.IsDBNull(row["overhaul_rate"]) ? string.Empty : Convert.ToString(row["overhaul_rate"]),

                            model_id = Convert.IsDBNull(row["model_id"]) ? 0 : Convert.ToInt32(row["model_id"]),
                            is_enable = Convert.IsDBNull(row["is_enable"]) ? false : Convert.ToBoolean(row["is_enable"]),
                            total_price = Convert.IsDBNull(row["total_price"]) ? 0 : Convert.ToInt32(row["total_price"]),
                            count_part_list = Convert.IsDBNull(row["count_part_list"]) ? 0 : Convert.ToInt32(row["count_part_list"]),
                        });
                    }

                }

            }
            HttpContext.Current.Session["SESSION_SERVICE_PACKAGE_SERVICEPARTLIST"] = selectedServicePackage;
            return dateReturn;
        }

        [WebMethod]
        public static List<ServicePartMaintenance> MoveUpPartListModel(string id)
        {
            List<ServicePartMaintenance> data = (List<ServicePartMaintenance>)HttpContext.Current.Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"];
            if (data != null)
            {
                var row = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (row != null)
                {
                    if (row.item != 1)
                    {
                        row.item = row.item - 1;
                    }
                    else
                    {
                        row.item = 1;
                    }
                    var sortNoLower = (from t in data where t.item == row.item && t.id != Convert.ToInt32(id) select t).FirstOrDefault();
                    if (sortNoLower != null)
                    {
                        sortNoLower.item = sortNoLower.item + 1;
                    }
                }
            }
            HttpContext.Current.Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"] = data;
            return data;
        }
        [WebMethod]
        public static List<ServicePartMaintenance> MoveDownPartListModel(string id)
        {
            List<ServicePartMaintenance> data = (List<ServicePartMaintenance>)HttpContext.Current.Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"];
            if (data != null)
            {
                var row = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (row != null)
                {
                    if (row.item != data.Count)
                    {
                        row.item = row.item + 1;
                    }
                    else
                    {
                        row.item = data.Count;
                    }
                    var sortNoLower = (from t in data where t.item == row.item && t.id != Convert.ToInt32(id) select t).FirstOrDefault();
                    if (sortNoLower != null)
                    {
                        sortNoLower.item = sortNoLower.item - 1;
                    }
                }
            }
            HttpContext.Current.Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"] = data;
            return data;
        }

        [WebMethod]
        public static List<ServicePackagePartMapping> MoveUpPartListPackage(string id)
        {
            List<ServicePackagePartMapping> data = (List<ServicePackagePartMapping>)HttpContext.Current.Session["SESSION_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"];
            if (data != null)
            {
                var row = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (row != null)
                {
                    if (row.sort_no != 1)
                    {
                        row.sort_no = row.sort_no - 1;
                    }
                    else
                    {
                        row.sort_no = 1;
                    }
                    var sortNoLower = (from t in data where t.sort_no == row.sort_no && t.id != Convert.ToInt32(id) select t).FirstOrDefault();
                    if (sortNoLower != null)
                    {
                        sortNoLower.sort_no = sortNoLower.sort_no + 1;
                    }
                }
            }
            HttpContext.Current.Session["SESSION_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"] = data;
            return data;
        }
        [WebMethod]
        public static List<ServicePackagePartMapping> MoveDownPartListPackage(string id)
        {
            List<ServicePackagePartMapping> data = (List<ServicePackagePartMapping>)HttpContext.Current.Session["SESSION_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"];
            if (data != null)
            {
                var row = (from t in data where t.id == Convert.ToInt32(id) select t).FirstOrDefault();
                if (row != null)
                {
                    if (row.sort_no != data.Count)
                    {
                        row.sort_no = row.sort_no + 1;
                    }
                    else
                    {
                        row.sort_no = data.Count;
                    }
                    var sortNoLower = (from t in data where t.sort_no == row.sort_no && t.id != Convert.ToInt32(id) select t).FirstOrDefault();
                    if (sortNoLower != null)
                    {
                        sortNoLower.sort_no = sortNoLower.sort_no - 1;
                    }
                }
            }
            HttpContext.Current.Session["SESSION_SERVICE_PART_PACKAGE_MAPPING_SERVICEPARTLIST"] = data;
            return data;
        }

        protected void gridModelRemark_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridModelRemark.DataSource = (from t in modelRemarkList where !t.is_delete select t).ToList();
            gridModelRemark.DataBind();
        }
        [WebMethod]
        public static string AddModelRemark(string remark)
        {
            var returnData = string.Empty;
            List<ModelRemark> data = (List<ModelRemark>)HttpContext.Current.Session["SESSION_MODEL_REMARK_SERVICEPARTLIST"];
            if (data != null)
            {
                data.Add(new ModelRemark()
                {
                    id = (data.Count + 1) * -1,
                    is_delete = false,
                    remark = remark
                });
            }
            else
            {
                data = new List<ModelRemark>();
                data.Add(new ModelRemark()
                {
                    id = -1,
                    is_delete = false,
                    remark = remark
                });
            }
            HttpContext.Current.Session["SESSION_MODEL_REMARK_SERVICEPARTLIST"] = data;
            return returnData;
        }
        [WebMethod]
        public static ModelRemark EditModelRemark(int id)
        {
            var returnData = new ModelRemark();
            List<ModelRemark> data = (List<ModelRemark>)HttpContext.Current.Session["SESSION_MODEL_REMARK_SERVICEPARTLIST"];
            if (data != null)
            {
                var row = (from t in data where t.id == id select t).FirstOrDefault();
                if (row != null)
                {
                    returnData = row;
                }
            }
            return returnData;
        }
        [WebMethod]
        public static string SubmitModelRemark(int id, string remark)
        {
            var returnData = string.Empty;
            List<ModelRemark> data = (List<ModelRemark>)HttpContext.Current.Session["SESSION_MODEL_REMARK_SERVICEPARTLIST"];
            if (data != null)
            {
                var row = (from t in data where t.id == id select t).FirstOrDefault();
                if (row != null)
                {
                    row.remark = remark;
                }
            }
            HttpContext.Current.Session["SESSION_MODEL_REMARK_SERVICEPARTLIST"] = data;
            return returnData;
        }
        [WebMethod]
        public static string DeleteModelRemark(int id)
        {
            var returnData = string.Empty;
            List<ModelRemark> data = (List<ModelRemark>)HttpContext.Current.Session["SESSION_MODEL_REMARK_SERVICEPARTLIST"];
            if (data != null)
            {
                var row = (from t in data where t.id == id select t).FirstOrDefault();
                if (row != null)
                {
                    if (row.id < 0)
                    {
                        data.Remove(row);
                    }
                    else
                    {
                        row.is_delete = true;
                    }
                }
            }
            HttpContext.Current.Session["SESSION_MODEL_REMARK_SERVICEPARTLIST"] = data;
            return returnData;
        }

        protected void gridViewModel_PageIndexChanged(object sender, EventArgs e)
        {
            int pageIndex = (sender as ASPxGridView).PageIndex;
            Session["PAGE"] = pageIndex;
        }

        protected void gridViewModel_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            string column = e.Column.FieldName;
            int order = Convert.ToInt32(e.Column.SortOrder);

            Session["COLUMN"] = column;
            Session["ORDER"] = order;
        }

        protected void gridViewDescriptionListPackage_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridViewDescriptionListPackage.DataSource = (from t in selectedServicePackageDescriptionList select t).ToList();
            gridViewDescriptionListPackage.DataBind();
        }

        private void BindGridViewDescriptionListPackage()
        {
            gridViewDescriptionListPackage.DataSource = (from t in selectedServicePackageDescriptionList
                                                         select t).ToList();
            gridViewDescriptionListPackage.DataBind();
        }
        [WebMethod]
        public static string AddDescription(string description)
        {
            var returnData = string.Empty;
            List<ServicePackageDescription> data = (List<ServicePackageDescription>)HttpContext.Current.Session["SESSION_SELECTED_SERVICE_DESCRIPTION_PACKAGE_"];
            if (data != null)
            {
                data.Add(new ServicePackageDescription()
                {
                    id = (data.Count + 1) * -1,
                    description = description
                });
            }
            else
            {
                data = new List<ServicePackageDescription>();
                data.Add(new ServicePackageDescription()
                {
                    id = -1,
                    description = description
                });
            }
            HttpContext.Current.Session["SESSION_SELECTED_SERVICE_DESCRIPTION_PACKAGE_"] = data;
            return returnData;
        }
        [WebMethod]
        public static ServicePackageDescription EditDescription(int id)
        {
            var returnData = new ServicePackageDescription();
            List<ServicePackageDescription> data = (List<ServicePackageDescription>)HttpContext.Current.Session["SESSION_SELECTED_SERVICE_DESCRIPTION_PACKAGE_"];
            if (data != null)
            {
                var row = (from t in data where t.id == id select t).FirstOrDefault();
                if (row != null)
                {
                    returnData = row;
                }
            }
            return returnData;
        }
        [WebMethod]
        public static string SubmitDescription(int id, string description)
        {
            var returnData = string.Empty;
            List<ServicePackageDescription> data = (List<ServicePackageDescription>)HttpContext.Current.Session["SESSION_SELECTED_SERVICE_DESCRIPTION_PACKAGE_"];
            if (data != null)
            {
                var row = (from t in data where t.id == id select t).FirstOrDefault();
                if (row != null)
                {
                    row.description = description;
                }
            }
            HttpContext.Current.Session["SESSION_SELECTED_SERVICE_DESCRIPTION_PACKAGE_"] = data;
            return returnData;
        }
        [WebMethod]
        public static string DeleteDescription(int id)
        {
            var returnData = string.Empty;
            List<ServicePackageDescription> data = (List<ServicePackageDescription>)HttpContext.Current.Session["SESSION_SELECTED_SERVICE_DESCRIPTION_PACKAGE_"];
            if (data != null)
            {
                var row = (from t in data where t.id == id select t).FirstOrDefault();
                if (row != null)
                {
                    data.Remove(row);

                    //using (SqlCommand cmd = new SqlCommand("sp_maintanance_package_description_edit", conn))
                    //{
                    //    cmd.CommandType = CommandType.StoredProcedure;

                    //    cmd.Parameters.Add("@id", SqlDbType.Int).Value = row.id;
                    //    cmd.Parameters.Add("@description", SqlDbType.Int).Value = row.description;
                    //    cmd.Parameters.Add("@updated_by", SqlDbType.Int).Value = Convert.ToInt32(ConstantClass.SESSION_USER_ID);

                    //    conn.Open();
                    //    cmd.ExecuteNonQuery();
                    //    conn.Close();
                    //}
                }
            }
            HttpContext.Current.Session["SESSION_SELECTED_SERVICE_DESCRIPTION_PACKAGE_"] = data;
            return returnData;
        }

        [WebMethod]
        public static ModelServiceHeader CopyModelData(string id)
        {
            //  Set active row

            var modelHeaderList = new ModelServiceHeader();
            var partList = new List<ServicePartMaintenance>();
            using (SqlConnection conn = new SqlConnection(SPlanetUtil.GetConnectionString()))
            {
                //Create array of Parameters
                List<SqlParameter> arrParm = new List<SqlParameter>
                        {
                    new SqlParameter("@search_name", SqlDbType.VarChar,200) { Value = "" },
                            new SqlParameter("@id", SqlDbType.Int) { Value = id },
                        };
                conn.Open();
                var dsModel = SqlHelper.ExecuteDataset(conn, "sp_maintenance_model_list", arrParm.ToArray());
                conn.Close();
                if (dsModel != null)
                {
                    var row = (from t in dsModel.Tables[0].AsEnumerable() select t).FirstOrDefault();
                    if (row != null)
                    {
                        modelHeaderList.count_package = Convert.IsDBNull(row["count_package"]) ? 0 : Convert.ToInt32(row["count_package"]);
                        modelHeaderList.count_package = Convert.IsDBNull(row["count_partlist"]) ? 0 : Convert.ToInt32(row["count_partlist"]);

                        modelHeaderList.description_tha = Convert.IsDBNull(row["description_tha"]) ? string.Empty : Convert.ToString(row["description_tha"]);
                        modelHeaderList.id = -1;//Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]);
                        modelHeaderList.model_name = Convert.IsDBNull(row["model_name"]) ? string.Empty : Convert.ToString(row["model_name"]);
                    }
                }

                //Create array of Parameters
                List<SqlParameter> arrParm2 = new List<SqlParameter>
                        {
                            new SqlParameter("@id", SqlDbType.Int) { Value = 0 },
                            new SqlParameter("@model_id", SqlDbType.Int) { Value = id },
                        };
                conn.Open();
                var dsPartList = SqlHelper.ExecuteDataset(conn, "sp_maintanance_part_list", arrParm2.ToArray());
                conn.Close();
                if (dsPartList != null)
                {
                    var dataPartList = (from t in dsPartList.Tables[0].AsEnumerable() select t).ToList();

                    foreach (var row in dataPartList)
                    {
                        partList.Add(new ServicePartMaintenance()
                        {
                            is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                            id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                            item = Convert.IsDBNull(row["item"]) ? 1 : Convert.ToInt32(row["item"]),
                            no = Convert.IsDBNull(row["no"]) ? string.Empty : Convert.ToString(row["no"]),
                            type = Convert.IsDBNull(row["type"]) ? string.Empty : Convert.ToString(row["type"]),
                            part_name = Convert.IsDBNull(row["part_name_tha"]) ? string.Empty : Convert.ToString(row["part_name_tha"]),
                            qty = Convert.IsDBNull(row["qty"]) ? 0 : Convert.ToInt32(row["qty"]),
                            part_no = Convert.IsDBNull(row["part_no"]) ? string.Empty : Convert.ToString(row["part_no"]),
                            part_id = Convert.IsDBNull(row["part_id"]) ? 0 : Convert.ToInt32(row["part_id"]),
                            model_id = -1,//Convert.IsDBNull(row["maintenance_model_id"]) ? 0 : Convert.ToInt32(row["maintenance_model_id"]),
                            selling_price = Convert.IsDBNull(row["selling_price"]) ? 0 : Convert.ToInt32(row["selling_price"]),
                            total_price = Convert.IsDBNull(row["total_price"]) ? 0 : Convert.ToInt32(row["total_price"]),
                            sort_no = Convert.IsDBNull(row["sort_no"]) ? 0 : Convert.ToInt32(row["sort_no"])
                        });
                    }

                }
                var modelRemarkList = new List<ModelRemark>();
                List<SqlParameter> arrParm3 = new List<SqlParameter>
                        {
                            new SqlParameter("@model_id", SqlDbType.Int) { Value = id },
                        };
                conn.Open();
                var dsModelRemark = SqlHelper.ExecuteDataset(conn, "sp_maintenance_model_remark_list", arrParm3.ToArray());
                conn.Close();
                if (dsModel != null)
                {
                    var dataRemark = (from t in dsModelRemark.Tables[0].AsEnumerable() select t).ToList();
                    if (dataRemark != null)
                    {
                        foreach (var row in dataRemark)
                        {
                            modelRemarkList.Add(new ModelRemark()
                            {
                                id = Convert.IsDBNull(row["id"]) ? 0 : Convert.ToInt32(row["id"]),
                                is_delete = Convert.IsDBNull(row["is_delete"]) ? false : Convert.ToBoolean(row["is_delete"]),
                                model_id =-1,// Convert.IsDBNull(row["model_id"]) ? 0 : Convert.ToInt32(row["model_id"]),
                                remark = Convert.IsDBNull(row["remark"]) ? string.Empty : Convert.ToString(row["remark"])
                            });
                        }
                    }
                }
                HttpContext.Current.Session["SESSION_MODEL_REMARK_SERVICEPARTLIST"] = modelRemarkList;
                HttpContext.Current.Session["SESSION_SERVICE_PART_MAINTENANCE_SERVICEPARTLIST"] = partList;
            }


            return modelHeaderList;
        }
    }
}