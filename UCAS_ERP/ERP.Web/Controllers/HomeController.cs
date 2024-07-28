using System.Web.Mvc;
using Common.Service.StoredProcedure;
using System;
using System.Data;
using System.Linq;
//using Common.Service.StoredProcedure;
using ERP.Web.Helpers;
using System.Collections.Generic;
using Common.Data.CommonDataModel;


namespace ERP.Web.Controllers
{
    public class HomeController : BaseController
    {
        #region Variables
        private readonly IUltimateReportService ultimateReportService;
        private readonly ISPService spService;
        public HomeController(IUltimateReportService ultimateReportService, ISPService spService)
        {
            this.ultimateReportService = ultimateReportService;
            this.spService = spService;
        }
        #endregion

        #region Methods
        public JsonResult GetEmployeeNotification()
        {
            try
            {
                var param = new { EmployeeId = SessionHelper.LoggedInUserId };
                var NotiListData = spService.GetDataWithParameter(param, "GetEmployeeNotification");

                var NotiList = NotiListData.Tables[0].AsEnumerable()
                .Select(row => new
                {
                    employee_id = row.Field<int>("employee_id"),
                    LoactionURL = row.Field<string>("LoactionURL"),
                    NotiNo = row.Field<int>("NotiNo"),
                    NotiType = row.Field<string>("NotiType"),
                    NotiTex = row.Field<string>("NotiTex")
                }).ToList();

                return Json(NotiList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        #endregion
        #region New Notification
        public JsonResult GetNotification(bool isAllExecutive = false)
        {
            try
            {
                var param = new { ExecutiveId = SessionHelper.LoggedInUserId };
                if (isAllExecutive == true)
                    param = new { ExecutiveId = 0 };

                var NotiListData = spService.GetDataWithParameter(param, "USP_Get_Notification");

                var NotiList = NotiListData.Tables[0].AsEnumerable()
                .Select(row => new
                {
                    ExecutiveId = row.Field<int>("ExecutiveId"),
                    NotiNo = row.Field<int>("NotiNo")
                }).ToList();

                //return Json(new { Status = true, data = NotiList, Message = "" }, JsonRequestBehavior.AllowGet);

                var json = Json(new { Status = true, data = NotiList, Message = "" }, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;

                return json;
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.Message });
            }

        }
        public JsonResult GetNotificationDetails(int ExecutiveId = 0, int IsShow = -1, string DateFrom = "", string DateTo = "")
        {
            try
            {
                if (ExecutiveId == 0)
                {
                    ExecutiveId = SessionHelper.LoggedInUserId;
                }
                var param = new { ExecutiveId = ExecutiveId, IsShow = IsShow, DateFrom = DateFrom, DateTo = DateTo };
                var NotiListData = spService.GetDataWithParameter(param, "USP_Get_NotificationDetails");

                var NotiList = NotiListData.Tables[0].AsEnumerable()
                .Select(row => new
                {
                    RowSl = row.Field<string>("RowSl"),
                    Id = row.Field<int>("Id"),
                    LoactionURL = row.Field<string>("LoactionURL"),
                    ExecutiveId = row.Field<int>("ExecutiveId"),
                    NotiTypeId = row.Field<string>("NotificationType"),
                    MessageText = row.Field<string>("MessageText"),
                    NoticeDate = row.Field<string>("NoticeDate"),
                    AssignBy = row.Field<string>("AssignBy"),
                    AssignById = row.Field<int>("AssignById"),
                    Photograph = row.Field<byte[]>("Photograph") != null ? "data:image;base64," + Convert.ToBase64String(row.Field<byte[]>("Photograph")) : ""
                }).ToList();

                //return Json(new { Status = true, data = NotiList, Message = "" }, JsonRequestBehavior.AllowGet);
                var json = Json(new { Status = true, data = NotiList, Message = "" }, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;

                return json;
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.Message });
            }

        }
        public JsonResult GetRemoveNotification(int Id)
        {
            try
            {
                var param = new { Id = Id, UserId = SessionHelper.LoggedInUserId };
                var Data = spService.GetDataWithParameter(param, "USP_Remove_Notification");
                var NotiList = Data.Tables[0].AsEnumerable()
                .Select(row => new
                {
                    ExecutiveId = row.Field<int>("ExecutiveId"),
                    NotiNo = row.Field<int>("NotiNo")
                }).ToList();
                var json = Json(new { Status = true, data = NotiList, Message = "" }, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "error" });
            }

        }
        public JsonResult RemoveAllNotificationByExecutiveId(int Id)
        {
            try
            {
                var param = new { Id = Id };
                var Data = spService.GetDataWithParameter(param, "USP_Remove_All_Notification_By_ExecutiveId");
                var NotiList = Data.Tables[0].AsEnumerable()
                .Select(row => new
                {
                    ExecutiveId = row.Field<int>("ExecutiveId"),
                    NotiNo = row.Field<int>("NotiNo")
                }).ToList();
                var json = Json(new { Status = true, data = NotiList, Message = "" }, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "error" });
            }

        }
        #endregion

        #region Events

        public ActionResult AccIndex()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult HrmIndex()
        {
            return View();
        }
        public ActionResult UnauthorizedAccess()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult DashIndex()
        {
            return View();
        }
        public ActionResult Projects()
        {

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        #endregion
    }
}
