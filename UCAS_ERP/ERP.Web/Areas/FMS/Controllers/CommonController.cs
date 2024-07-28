using System;
using System.Linq;
using System.Web.Mvc;
using ERP.Web.Controllers;
using FMS.Web.CommonDropDownList;
using FMS.Web.FMSViewModel;
using FMS.Service.ServiceModel;

namespace FMS.Web.Controllers
{
    public class CommonController : BaseController
    {
        CommonDropDownList.CommonDropDownList commonDropDownList;
        RResult oResult;
        public CommonController()
        {
            commonDropDownList = new CommonDropDownList.CommonDropDownList();
            oResult = new RResult();
        }
        public JsonResult getEMPBranchList()
        {
            try
            {
                oResult.result = 1;
                var data = commonDropDownList.getEMPBranchList();
                oResult.data = data;

            }
            catch (Exception e)
            {
                oResult.result = 0;
                oResult.message = e.Message;
            }
            return Json(oResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getEMPDepartment()
        {
            try
            {
                oResult.result = 1;
                var data = commonDropDownList.getEMPDepartment();
                oResult.data = data;

            }
            catch (Exception e)
            {
                oResult.result = 0;
                oResult.message = e.Message;
            }
            return Json(oResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getEmpJobType()
        {
            try
            {
                oResult.result = 1;
                var data = commonDropDownList.getEmpJobType();
                oResult.data = data;

            }
            catch (Exception e)
            {
                oResult.result = 0;
                oResult.message = e.Message;
            }
            return Json(oResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getDesignation()
        {
            try
            {
                oResult.result = 1;
                var data = commonDropDownList.getDesignation();
                oResult.data = data;

            }
            catch (Exception e)
            {
                oResult.result = 0;
                oResult.message = e.Message;
            }
            return Json(oResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getBloodGroup()
        {
            try
            {
                oResult.result = 1;
                var data = commonDropDownList.getBloodGroup();
                oResult.data = data;

            }
            catch (Exception e)
            {
                oResult.result = 0;
                oResult.message = e.Message;
            }
            return Json(oResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getEmpActivStatus()
        {
            try
            {
                oResult.result = 1;
                var data = commonDropDownList.getEmpActivStatus();
                oResult.data = data;

            }
            catch (Exception e)
            {
                oResult.result = 0;
                oResult.message = e.Message;
            }
            return Json(oResult, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult EmployeeCodeAutoCompleteByNameAndCode(string Prefix)
        {
            oResult.result = 0;
            try
            {
                var list = commonDropDownList.getEmpShortInfoByCodeAndName(Prefix);
                oResult.data = list.Select(b => new { label = b.emp_name, value = b.emp_id, emp_office_code = b.emp_office_code });

                oResult.result = 1;
            }
            catch (Exception e)
            {
                oResult.result = 0;
                oResult.message = e.Message;
            }
            return Json(oResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EmployeeShortInfoByEmpCodeOrId(string EmployeeCode = "", int? Emp_Id = 0)
        {
            oResult.result = 0;
            try
            {
                var list = commonDropDownList.getEmpShortInfoByCodeAndName(EmployeeCode, Emp_Id);
                oResult.data = list;

                oResult.result = 1;
            }
            catch (Exception e)
            {
                oResult.result = 0;
                oResult.message = e.Message;
            }
            return Json(oResult, JsonRequestBehavior.AllowGet);
        }
    }
}