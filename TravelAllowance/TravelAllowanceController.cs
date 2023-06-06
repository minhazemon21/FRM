using Common.Service.StoredProcedure;
using ERP.Web.Helpers;
using Hrms.CommonDropDownList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utility;

namespace Hrms.Controllers
{
    public class TravelAllowanceController : Controller
    {
        private readonly ISPService spService;
        public TravelAllowanceController(ISPService spService)
        {
            this.spService = spService;
          
        }
        // GET: TravelAllowance
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult TAEdit(int Invoice = 0)
        {
            ViewBag.Invoice = Invoice;
            var List = spService.GetDataWithParameter(new { Invoice = Invoice }, "USP_TA_Info_For_Edit").Tables[0];
            ViewBag.TAList = List;
            return View();
        }
        public ActionResult TAList()
        {
            var UserId = SessionHelper.LoggedInUserId;
            ViewBag.UserId = UserId;
            return View();
        }
        public JsonResult SaveTravelAllowance(int invoiceNo, string dates, string locationFroms, string locationTos, string trnsTypes, string trnsNames, string descriptions, string amounts)
        {
            try
            {
                //var invoiceCnt = spService.GetDataBySqlCommand("SELECT COUNT(m.InvoiceNo) InvoiceNo FROM StoreInMaster m WHERE m.IsActive = 1 AND m.InvoiceNo = '" + invoiceNo + "'").Tables[0].Rows[0][0].ToString();
                //if ((masterId == 0 && invoiceCnt != "0") || (masterId != 0 && Convert.ToInt32(invoiceCnt) > 1))
                //{
                //    return Json(new { Status = false, Message = "Duplicate invoice no." }, JsonRequestBehavior.AllowGet);
                //}

                var param = new { invoiceNo = invoiceNo, dates = dates, locationFroms = locationFroms, locationTos = locationTos, trnsTypes = trnsTypes, trnsNames = trnsNames, descriptions = descriptions, amounts = amounts, UserId = SessionHelper.LoggedInUserId };
                spService.GetDataWithParameter(param, "USP_Insert_TravelAllowance");
                return Json(new { Status = true, Result = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.InnerException.Message == null ? ex.Message : ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTAInInformation()
        {
            try
            {
                var Pro = spService.GetDataWithParameter(new { UserId = SessionHelper.LoggedInUserId, roleId = SessionHelper.LoggedIn_RoleId }, "USP_Get_TA_Information").Tables[0];

                var Result = Pro.AsEnumerable().Select(row => new
                {
                    InvoiceNo = row.Field<int>("InvoiceNo"),
                    Amount = row.Field<decimal>("Amount"),
                    emp_name = row.Field<string>("emp_name")
                }).ToList();
                return Json(new { Status = true, Result = Result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.InnerException.Message == null ? ex.Message : ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DeleteTABill(int Id = 0)
        {
            try
            {
                spService.GetDataBySqlCommand("UPDATE TravelAllowanceBill SET IsActive = 0,UpdateDate = GETDATE(),UpdateUserId = " + SessionHelper.LoggedInUserId + " WHERE Id = " + Id + "");
                return Json(new { Status = true, Result = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.InnerException.Message == null ? ex.Message : ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateTravelAllowance(int invoiceNo, string dates, string locationFroms, string locationTos, string trnsTypes, string trnsNames, string descriptions, string amounts)
        {
            try
            {
                //var invoiceCnt = spService.GetDataBySqlCommand("SELECT COUNT(m.InvoiceNo) InvoiceNo FROM StoreInMaster m WHERE m.IsActive = 1 AND m.InvoiceNo = '" + invoiceNo + "'").Tables[0].Rows[0][0].ToString();
                //if ((masterId == 0 && invoiceCnt != "0") || (masterId != 0 && Convert.ToInt32(invoiceCnt) > 1))
                //{
                //    return Json(new { Status = false, Message = "Duplicate invoice no." }, JsonRequestBehavior.AllowGet);
                //}

                var param = new { invoiceNo = invoiceNo, dates = dates, locationFroms = locationFroms, locationTos = locationTos, trnsTypes = trnsTypes, trnsNames = trnsNames, descriptions = descriptions, amounts = amounts, UserId = SessionHelper.LoggedInUserId };
                spService.GetDataWithParameter(param, "USP_Update_TravelAllowance");
                return Json(new { Status = true, Result = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.InnerException.Message == null ? ex.Message : ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ApprovedInvoice(int InvoiceNo)
        {
            try
            {

                var data = spService.GetDataWithParameter(new { InvoiceNo = InvoiceNo, UserId = SessionHelper.LoggedInUserId }, "USP_Approved_Invoce_For_TA").Tables[0].AsEnumerable().Select(R => new
                {
                    meg = R.Field<string>("Message")
                }).FirstOrDefault();

                return Json(new { Message = data.meg, Status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                    Status = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult RejectInvoice(int InvoiceNo)
        {
            try
            {

                var data = spService.GetDataWithParameter(new { InvoiceNo = InvoiceNo, UserId = SessionHelper.LoggedInUserId }, "USP_Reject_Invoce_For_TA").Tables[0].AsEnumerable().Select(R => new
                {
                    meg = R.Field<string>("Message")
                }).FirstOrDefault();

                return Json(new { Message = data.meg, Status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                    Status = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ShowTAReportforInvoice(int InvoiceNo, string ReportType = "pdf")
        {
            try
            {
                var param = new
                {
                    InvoiceNo = InvoiceNo
                };
                var MainReport = spService.GetDataWithParameter(param, "USP_RPT_TravelAllowanceByInvoce");

                var reportParam = new Dictionary<string, object>();

                ERP.Web.Helpers.ReportHelper.ShowReport(MainReport.Tables[0], ReportType, "rpt_TABYConvence.rpt", "rpt_TABYConvence");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}