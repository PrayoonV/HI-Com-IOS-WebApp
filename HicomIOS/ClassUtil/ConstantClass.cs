﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HicomIOS.ClassUtil
{
    public class ConstantClass
    {
        public static string CONSTANT_NO_PERMISSION_PAGE
        {
            get
            {
                return "/Default.aspx";
            }
        }

        public static string SESSION_USER_ID
        {
            get
            {
                return HttpContext.Current.Session["USER_ID"] == null ? string.Empty : HttpContext.Current.Session["USER_ID"].ToString();
            }
            set { HttpContext.Current.Session["USER_ID"] = value; }
        }
        public static string SESSION_USER_GROUP_ID
        {
            get { return HttpContext.Current.Session["USER_GROUP_ID"] == null ? string.Empty : HttpContext.Current.Session["USER_GROUP_ID"].ToString(); }
            set { HttpContext.Current.Session["USER_GROUP_ID"] = value; }
        }
        public static string SESSION_USER_CODE
        {
            get { return HttpContext.Current.Session["USERNAME"] == null ? string.Empty : HttpContext.Current.Session["USERNAME"].ToString(); } //check session empty
            set { HttpContext.Current.Session["USERNAME"] = value; }
        }
        public static string SESSION_FIRST_NAME
        {
            get { return HttpContext.Current.Session["FIRST_NAME"] == null ? string.Empty : HttpContext.Current.Session["FIRST_NAME"].ToString(); }
            set { HttpContext.Current.Session["FIRST_NAME"] = value; }
        }
        public static string SESSION_LAST_NAME
        {
            get { return HttpContext.Current.Session["LAST_NAME"] == null ? string.Empty : HttpContext.Current.Session["LAST_NAME"].ToString(); }
            set { HttpContext.Current.Session["LAST_NAME"] = value; }
        }
        public static string SESSION_PICTURE
        {
            get { return HttpContext.Current.Session["USER_PICTURE"] == null ? string.Empty : HttpContext.Current.Session["USER_PICTURE"].ToString(); }
            set { HttpContext.Current.Session["USER_PICTURE"] = value; }
        }
        public static int VAT
        {
            get { return HttpContext.Current.Session["SESSION_VAT"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["SESSION_VAT"]); }
            set { HttpContext.Current.Session["SESSION_VAT"] = value; }
        }
        public static string SESSION_DEPARTMENT_NAME
        {
            get { return HttpContext.Current.Session["DEPARTMENT_NAME"] == null ? string.Empty : HttpContext.Current.Session["DEPARTMENT_NAME"].ToString(); }
            set { HttpContext.Current.Session["DEPARTMENT_NAME"] = value; }
        }
        public static string SESSION_DEPARTMENT_SERVICE_TYPE
        {
            get { return HttpContext.Current.Session["DEPARTMENT_SERVICE_TYPE"] == null ? string.Empty : HttpContext.Current.Session["DEPARTMENT_SERVICE_TYPE"].ToString(); }
            set { HttpContext.Current.Session["DEPARTMENT_SERVICE_TYPE"] = value; }
        }

    }
}
