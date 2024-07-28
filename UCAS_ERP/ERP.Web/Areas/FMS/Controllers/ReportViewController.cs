using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Common.Service.StoredProcedure;
using ERP.Web.Controllers;
using ERP.Web.Helpers;
using FMS.Web.CommonDropDownList;
using Common.Data.CommonDataModel;
using FMS.Web.FMSViewModel;

namespace FMS.Web.Controllers
{
    public class ReportViewController : BaseController
    {
        #region member
        private readonly ISPService spService;
        CommonDropDownList.CommonDropDownList commonDropDownList;
        public ReportViewController(ISPService spService)
        {
            this.spService = spService;
            this.commonDropDownList = new CommonDropDownList.CommonDropDownList();
        }
        #endregion
        public ActionResult Index()
        {
            return View();
        }
    }
}