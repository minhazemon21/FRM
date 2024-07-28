using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Common.Data.CommonDataModel;
using Common.Service;
using Common.Service.StoredProcedure;
using ERP.Web.Controllers;
using ERP.Web.Helpers;

namespace ERP.Web.Controllers
{
    public class AccessController : BaseController
    {
        //services declaration
        private readonly ISPService spService;
        private readonly IReportInformationService reportInformationService;
        private readonly IReportAccessService reportAccessService;
        private readonly IAspNetUserService aspNetUserService;
        private readonly ISecurityService securityService;
        public AccessController(ISPService spService, IReportInformationService reportInformationService, IReportAccessService reportAccessService,
            IAspNetUserService aspNetUserService, ISecurityService securityService)
        {
            this.spService = spService;
            this.reportInformationService = reportInformationService;
            this.reportAccessService = reportAccessService;
            this.aspNetUserService = aspNetUserService;
            this.securityService = securityService;
        }
        public ActionResult ReportAccess()
        {
            var users =
                spService.GetDataWithoutParameter("USP_GET_ALL_USER").Tables[0].AsEnumerable()
                    .Select(x => new AspNetUser() { UserId = x.Field<int>(0), UserName = x.Field<string>(1) })
                    .ToList();
            var reportsModule =
                spService.GetDataWithoutParameter("USP_GET_ALL_Report_Module").Tables[0].AsEnumerable()
                    .Select(x => new AspNetSecurityModule() { AspNetSecurityModuleId = x.Field<int>(0), LinkText = x.Field<string>(1) })
                    .ToList();
            ViewBag.Users = users;
            ViewBag.ReportModules = reportsModule;
            if(SessionHelper.RoleName.ToLower() == "supper admin")
            {
                ViewBag.Reports = reportInformationService.GetAll().ToList();
            }
            else
            {
                //R.Id,R.ReportName,R.AspNetSecurityModuleId,R.SerialNo,R.ControllerName,R.ActionName,R.ProjectShortName,R.IsActive
                ViewBag.Reports = spService.GetDataWithParameter(new { LogInUserId = SessionHelper.LoggedInUserId}, "USP_GET_All_ReportInformationByLogInUserId")
                    .Tables[0].AsEnumerable().Select(R => new ReportInformation
                    {
                        Id = R.Field<int>("Id"),
                        ReportName = R.Field<string>("ReportName"),
                        AspNetSecurityModuleId = R.Field<int>("AspNetSecurityModuleId"),
                        SerialNo = R.Field<int>("SerialNo"),
                        ControllerName = R.Field<string>("ControllerName"),
                        ActionName= R.Field<string>("ActionName"),
                        ProjectShortName = R.Field<string>("ProjectShortName"),
                        IsActive= R.Field<bool>("IsActive")
                    }).ToList();
            }
            ViewBag.Projects = securityService.GetAllProject(SessionHelper.LoggedIn_RoleId, "0");
            return View();
        }

        public void Get_AccessReport(int rptslnx, string InvestorCode, string UserCode, string cmbReportType)
        {

            if (rptslnx == 1)
            {
                var data =
               spService.GetDataWithParameter(
                   new
                   {
                       Qtype = rptslnx,
                       InvestorCode = "",
                       UserCode = ""
                   },
                   "Rpt_SpecialPermissionInvestorWise").Tables[0];
                ReportHelper.ShowReport(data, cmbReportType, "Rpt_SpecialPermissionInvestor.rpt", "Rpt_SpecialPermissionInvestor");
            }
            else if (rptslnx == 2)
            {
                var data =
               spService.GetDataWithParameter(
                   new
                   {
                       Qtype = rptslnx,
                       InvestorCode = InvestorCode,
                       UserCode = ""
                   },
                   "Rpt_SpecialPermissionInvestorWise").Tables[0];
                ReportHelper.ShowReport(data, cmbReportType, "Rpt_SpecialPermissionInvestorWise.rpt", "Rpt_SpecialPermissionInvestorWise");
            }
            else
            {
                var data =
               spService.GetDataWithParameter(
                   new
                   {
                       Qtype = rptslnx,
                       InvestorCode = "",
                       UserCode = UserCode
                   },
                   "Rpt_SpecialPermissionInvestorWise").Tables[0];
                ReportHelper.ShowReport(data, cmbReportType, "Rpt_SpecialPermissionUserWise.rpt", "Rpt_SpecialPermissionUserWise");
            }

        }
        public JsonResult GetAccessReportList(int userid, string projectShortName)
        {
            var roleid = aspNetUserService.GetByUserId(userid).RoleId;
            var report = reportAccessService.GetAll().Where(x => x.UserId == userid && x.IsActive).ToList();
            var module = spService.GetDataWithParameter(new { USER_ROLE_ID = roleid, Project_Short_Name = projectShortName }, "USP_GET_ALL_Report_Module").Tables[0].AsEnumerable()
                    .Select(x => new AspNetSecurityModule() { AspNetSecurityModuleId = x.Field<int>(0), LinkText = x.Field<string>(1) })
                    .ToList();
            return Json(new { Report = report, Module = module }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetReportAccess(int userid, List<ReportAccess> reportList)
        {
            var reports = reportAccessService.GetAll().Where(x => x.UserId == userid).ToList();

            foreach (var aReport in reports.Where(aReport => aReport.IsActive && reportList.FirstOrDefault(z=> z.ReportId==aReport.ReportId)==null))
            {
                aReport.IsActive = false;
                aReport.IsAllInvestorAccess = 0;
                aReport.UpdateDate = DateTime.Now;
                aReport.UpdateUserId = SessionHelper.LoggedInUserId;
                reportAccessService.Update(aReport);
            }
            foreach (var report in reportList)
            {
                var access = reports.FirstOrDefault(x => x.ReportId == report.ReportId);
                if (access == null)
                {
                    reportAccessService.Create(new ReportAccess()
                    {
                        UserId = userid,
                        ReportId = report.ReportId,
                        IsAllInvestorAccess = report.IsAllInvestorAccess,
                        CreateDate = DateTime.Now,
                        CreatedUserId = SessionHelper.LoggedInUserId,
                        IsActive = true
                    });
                }
                else
                {
                    if (!access.IsActive || access.IsAllInvestorAccess != report.IsAllInvestorAccess)
                    {
                        access.IsActive = true;
                        access.IsAllInvestorAccess = report.IsAllInvestorAccess;
                        access.UpdateDate = DateTime.Now;
                        access.UpdateUserId = SessionHelper.LoggedInUserId;
                        reportAccessService.Update(access);
                    }
                }
            }
            System.Web.HttpContext.Current.Session[SessionKeys.USER_REPORT_MODULES] =
                securityService.GetReportModules(userid, "0");
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LockUnlockUser()
        {
            var users =
                spService.GetDataWithoutParameter("USP_GET_ALL_USER").Tables[0].AsEnumerable()
                    .Select(
                        x =>
                            new AspNetUser()
                            {
                                UserId = x.Field<int>(0),
                                UserName = x.Field<string>(1),
                                LockoutEnabled = x.Field<bool>(2)
                            })
                    .ToList();
            ViewBag.Users = users;
            return View();
        }

        public JsonResult ManualLockUnlockUser(int userId, int isLock)
        {
            try
            {
                var user = aspNetUserService.GetByUserId(userId);
                if (isLock == 0)
                {
                    user.LockoutEnabled = false;
                    user.LockoutDate = DateTime.Now;
                    user.AccessFailedCount = 0;
                }
                else
                {
                    user.LockoutEnabled = true;
                    user.LockoutDate = DateTime.Now;
                }
                aspNetUserService.Update(user);
                return Json(new { Status = true, Message = (isLock == 1 ? "Lock" : "Unlock") + " user successfull." },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return
                    Json(
                        new
                        {
                            Status = false,
                            Message = (ex.InnerException == null ? ex.Message : ex.InnerException.Message)
                        },
                        JsonRequestBehavior.AllowGet);
            }
        }

    }
}