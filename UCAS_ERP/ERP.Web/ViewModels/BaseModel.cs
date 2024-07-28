﻿using System;
using System.Web;
using ERP.Web.Helpers;

namespace ERP.Web.ViewModels
{
    public class BaseModel
    {
        //public string CreateUser
        //{
        //    get
        //    {
        //        if (HttpContext.Current.User.Identity.IsAuthenticated)
        //            return HttpContext.Current.User.Identity.Name;
        //        else
        //            return "SYSTEM";
        //    }
        //}
        public long CreateBy
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                    return Convert.ToInt64(SessionHelper.LoggedInUserId);
                else
                    return 0;
            }
        }
        public DateTime CreateDate { get { return DateTime.Now; } }

        //public Int64 UpdateUser
        //{
        //    get
        //    {
        //        if (HttpContext.Current.User.Identity.IsAuthenticated)
        //            return Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
        //        else
        //            return 0;
        //    }
        //}
        //public DateTime UpdateDate { get { return DateTime.Now; } }
        public Nullable<long> UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public Nullable<bool> IsRemoved { get; set; }
        
        

       // public Nullable<bool> IsActive { get { return IsActive.HasValue ? IsActive : true; } set; }
        //public Nullable<System.DateTime> InActiveDate { get; set { InActiveDate = IsActive.HasValue ? (IsActive.Value == false ? DateTime.Now : default(DateTime?)) : default(DateTime?); } }
    }
}