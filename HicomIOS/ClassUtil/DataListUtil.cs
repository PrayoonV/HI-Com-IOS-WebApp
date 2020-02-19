﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HicomIOS.ClassUtil
{
    public class DataListUtil
    {
        public static int emptyEntryID = -1;

        /****************** ENUM LIST *********************/

        public static class DropdownStoreProcedureName
        {
            
            public static String Product_No { get { return "sp_dropdown_product_no"; } }
            public static String Quotation_Document { get { return "sp_dropdown_quotation_document_in"; } }
            public static String Quotation_Document_PR { get { return "sp_dropdown_quotation_document_in_pr"; } }
            public static String Part_No { get { return "sp_dropdown_product_spare_part"; } }
            public static String Type_Contract { get { return "sp_dropdown_type_contract"; } }
            public static String Customer_IndustryType { get { return "sp_dropdown_customer_industry_type"; } }
            public static String Product_Model { get { return "sp_dropdown_model_product"; } }
            public static String Customer { get { return "sp_dropdown_customer"; } }
            public static String Customer_MFG { get { return "sp_dropdown_customer_mfg"; } }
            public static String Customer_Group { get { return "sp_dropdown_customer_group"; } }
            public static String Customer_Branch { get { return "sp_dropdown_customer_branch"; } }
            public static String Customer_Business { get { return "sp_dropdown_customer_business"; } }            
            public static String Employee_Department { get { return "sp_dropdown_employee_department"; } }
            public static String Employee_Position { get { return "sp_dropdown_employee_position"; } }
            public static String Thailand_Province { get { return "sp_dropdrown_tb_thailand_2_province"; } }
            public static String Thailand_Amphur { get { return "sp_dropdrown_tb_thailand_3_amphur"; } }
            public static String Thailand_District { get { return "sp_dropdrown_tb_thailand_4_district"; } }
            public static String Thailand_Geo { get { return "sp_dropdrown_tb_thailand_1_geo"; } }
            public static String Delivery_Note_Issue { get { return "sp_dropdown_delivery_note_issue"; } }
            public static String Supplier_Group { get { return "sp_dropdown_supplier_group"; } }
            public static String Supplier_Brand { get { return "sp_dropdown_supplier_brand"; } }
            public static String Supplier { get { return "sp_dropdown_supplier"; } }
            public static String Product { get { return "sp_dropdown_product"; } }
            public static String Product_Brand { get { return "sp_dropdown_product_brand"; } }
            public static String Product_Unit { get { return "sp_dropdown_product_unit"; } }
            public static String Product_Category { get { return "sp_dropdown_product_category"; } }
            public static String SparePart_Category { get { return "sp_dropdown_spare_part_category"; } }
            public static String Security_User_Group { get { return "sp_dropdown_security_user_group"; } }
            public static String Security_User { get { return "sp_dropdown_security_user"; } }
            public static String Security_User_Mapping { get { return "sp_dropdown_security_user_for_mapping"; } }
            public static String Quotation { get { return "sp_dropdown_quotation_no"; } }
            public static String Sale_Order_Quotation { get { return "sp_dropdown_sale_order_quotation"; } }
            public static String Issue_Sale_Order { get { return "sp_dropdown_issue_saleorder"; } }
            public static String Product_MFG_Quotation { get { return "sp_dropdown_product_mfg"; } }
            public static String Delivery_Order_Issue_Stock { get { return "sp_dropdown_issue_stock"; } }
            public static String Return_Delivery_Note { get { return "sp_dropdown_return_delivery_note"; } }
            public static string Return_Issue { get { return "sp_dropdown_return_issue"; } }
            public static string Employee_List { get { return "sp_dropdown_employee"; } }
            public static string Shelf_List { get { return "sp_dropdown_shelf"; } }
            public static string Return_Borrow { get { return "sp_dropdown_return_borrow"; } }
            public static string Issue_Header_List_On { get { return "sp_dropdown_issue_header_list_on"; } }
            public static string Return_PR { get { return "sp_dropdown_return_pr"; } }
        }

        public enum EnumGender
        {
            Male = 'M',
            Female = 'F'
        }
        public enum EnumStatus
        {
            Disabled = 0,
            Enabled = 1
        }
    }
}