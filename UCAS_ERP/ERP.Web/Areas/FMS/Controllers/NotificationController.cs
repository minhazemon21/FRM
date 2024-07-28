using Common.Service.StoredProcedure;
using ERP.Web.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using ERP.Web.Helpers;

namespace FMS.Web.Controllers
{
    public class NotificationController : BaseController
    {
        #region variable
        private readonly ISPService spService;
        public NotificationController(ISPService spService)
        {
            this.spService = spService;
        }
        #endregion
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Notify()
        {
            ViewBag.Department = spService.GetDataWithoutParameter("USP_GET_DEPARTMENT_LIST_FOR_DROPDOWN_LIST").Tables[0];
            return View();
        }
        #region All Data return method
        public JsonResult GetDepartmentwiseExecutive(int? deptId = 0)
        {
            try
            {
                var empList = spService.GetDataWithParameter(new { deptId = deptId}, "USP_GET_EmployeeList").Tables[0]
                    .AsEnumerable().Select(x => new
                    {
                        Id = x.Field<int>("emp_id"),
                        ExecutiveCode = x.Field<string>("emp_office_code"),
                        ExecutiveName = x.Field<string>("EmployeeName"),
                        DesignationName = x.Field<string>("DesignationName"),
                        DepartmentName = x.Field<string>("DepartmentName")
                    }).ToList();
                return Json(new { Status = true, data = empList, Message = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveNotification(string executiveIds, string message)
        {
            try
            {
                spService.GetDataWithParameter(new { executiveIds = executiveIds, message = message, userId = SessionHelper.LoggedInUserId }, "USP_Save_Notification");
                return Json(new { Status = true, Message = "Save successfull." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}