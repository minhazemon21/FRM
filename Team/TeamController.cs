using Common.Data.CommonDataModel;
using Common.Service;
using Common.Service.StoredProcedure;
using Common.Web.Controllers;
using Common.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Apartment.Controllers
{
    public class TeamController : BaseController
    {
        private readonly ISPService spService;
        private readonly IOfficeExecutiveService officeExecutiveService;
        private readonly ISourceService sourceService;
        public TeamController(ISPService spService, IOfficeExecutiveService officeExecutiveService, ISourceService sourceService)
        {
            this.spService = spService;
            this.sourceService = sourceService;
            this.officeExecutiveService = officeExecutiveService;
        }
        // GET: Team
        public ActionResult Index()
        {
            ViewBag.TeamLeadAssign = spService.GetDataWithoutParameter("USP_Get_Executive_For_TeamLead_Assign").Tables[0];
            ViewBag.TeamList = spService.GetDataWithoutParameter("USP_Get_Team_Info_for_List").Tables[0];
            return View();
        }
        public ActionResult TeamReport()
        {
            var moduleid = spService.GetSecurityModuleByControllerAction("Team", "TeamReport");
            ViewBag.Reports = spService.GetDataWithParameter(new
            {
                USER_ID = SessionHelper.LoggedInUserId,
                ReportModuleId = moduleid
            }, "USP_GET_USER_ACCESSED_REPORT").Tables[0].AsEnumerable().Select(x => new ReportInformation()
            {
                Id = x.Field<int>(0),
                ReportName = x.Field<string>(1),
                SerialNo = x.Field<int>(2)
            }).ToList();
            ViewBag.TeamList = spService.GetDataWithoutParameter("USP_Get_Team_Info_for_List").Tables[0];


            return View();
        }
        public JsonResult SaveTeamInfo(int teamId, string teamName, string teamStName, int teamLeadId, string remarks)
        {
            try
            {
                //var check = spService.GetDataWithParameter(new { teamId = teamId, teamLeaderId = teamLeadId }, "USP_CHECK_TeamLeadAndOther").Tables[0].AsEnumerable().Select(R => new
                //{

                //    Status = R.Field<string>("Status")
                //}).FirstOrDefault();
                //return Json(new { Message = check.msg, Status = true }, JsonRequestBehavior.AllowGet);

                var data = spService.GetDataWithParameter(new { teamId = teamId, teamName = teamName, teamStName, teamLeaderId = teamLeadId, remarks = remarks, userId = SessionHelper.LoggedInUserId }, "USP_Insert_Team").Tables[0].AsEnumerable().Select(R => new
                {
                    msg = R.Field<string>("Message"),
                    response = R.Field<int>("Response")
                }).FirstOrDefault();
                return Json(new { Message = data.msg, Status = true, Response = data.response }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.InnerException.Message, Status = false }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult DeleteTeam(int CRMTeamId, int TeamLeaderId)
        {
            try
            {
                var data = spService.GetDataWithParameter(new { CRMTeamId = CRMTeamId, TeamLeaderId = TeamLeaderId, userId = SessionHelper.LoggedInUserId }, "USP_Delete_Team_By_Id").Tables[0].AsEnumerable().Select(R => new
                {
                    msg = R.Field<string>("Message"),
                }).FirstOrDefault();
                return Json(new { Message = data.msg, Status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.InnerException.Message, Status = false }, JsonRequestBehavior.AllowGet);
            }

        }
        public void ShowTeamReport(string type = "pdf")
        {
            var data = spService.GetDataWithoutParameter("USP_RPT_TEAM_LIST").Tables[0];
            ReportHelper.ShowReport(data, type, "rpt_Team_Info.rpt", "TeamList");
        }

        public void ShowReportforTeamDetails(int TeamId = 0,  string type = "pdf")
        {
            var data =
            spService.GetDataWithParameter(
                new
                {
                    TeamId = TeamId,
                },
                "USP_RPT_Team_Details").Tables[0];
            ReportHelper.ShowReport(data, type, "rpt_Team_Details.rpt", "rpt_Team_Details");
        }
    }
}