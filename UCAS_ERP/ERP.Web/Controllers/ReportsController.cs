using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Data.CommonDataModel;
using ERP.Web.Helpers;
using Common.Service.StoredProcedure;
using ERP.Web.ViewModels;

namespace ERP.Web.Controllers
{
    public class ReportsController : BaseController
    {

        readonly ISPService sPService;
        public ReportsController(ISPService sPService)
        {
            this.sPService = sPService;
        }
        public void Get_GeneralReport(int reportNo, string Qtype, string exportType)
        {
            if (Qtype == "Occupation" || Qtype == "Relation" || Qtype == "Designation" || Qtype == "Department")
            {
                var data =
                sPService.GetDataWithParameter(
                    new { Qtype = Qtype },
                    "Rpt_Get_Occupation_Relation_Designation").Tables[0];
                var reportParam = new Dictionary<string, object> { { "param_orgName", "Ucas" } };
                if (Qtype == "Occupation")
                {
                    ReportHelper.ShowReport(data, exportType, "rpt_Occupation.rpt", "rpt_Occupation", reportParam);
                }
                else if (Qtype == "Relation")
                {
                    ReportHelper.ShowReport(data, exportType, "rpt_Relation.rpt", "rpt_Relation", reportParam);
                }
                else if (Qtype == "Designation")
                {
                    ReportHelper.ShowReport(data, exportType, "rpt_Designation.rpt", "rpt_Designation", reportParam);
                }
                else if (Qtype == "Department")
                {
                    ReportHelper.ShowReport(data, exportType, "rpt_Department.rpt", "rpt_Department", reportParam);
                }
            }

            else
            {
                var data2 =
              sPService.GetDataWithParameter(
                  new { Qtype = Qtype },
                  "Rpt_Get_Country_Divition_District_Thana").Tables[0];
                var reportParam = new Dictionary<string, object> { { "param_orgName", "Ucas" } };

                if (Qtype == "Country")
                {
                    ReportHelper.ShowReport(data2, exportType, "rpt_Country.rpt", "rpt_Country", reportParam);
                }
                else if (Qtype == "Division")
                {
                    ReportHelper.ShowReport(data2, exportType, "rpt_Division.rpt", "rpt_Division", reportParam);
                }
                else if (Qtype == "District")
                {
                    ReportHelper.ShowReport(data2, exportType, "rpt_District.rpt", "rpt_District", reportParam);
                }
                else if (Qtype == "Thana")
                {
                    ReportHelper.ShowReport(data2, exportType, "rpt_Thana.rpt", "rpt_Thana", reportParam);
                }

            }
        }
        public void GetBankWiseAllBranch(int reportNo, string exportType)
        {
            var data =
                sPService.GetDataWithoutParameter(
                    "RPT_GetBankWiseAllBranch").Tables[0];
            var reportParam = new Dictionary<string, object> { { "param_orgName", "Ucas" } };
            ReportHelper.ShowReport(data, exportType, "rpt_Bank_BranchList.rpt", "rpt_BrokerInfo", reportParam);
        }
        public void GetRptBankInformation(int reportNo, string exportType)
        {
            var data =
                sPService.GetDataBySqlCommand(
                    "SELECT B.Id,UPPER(B.BankShortName) BankShortName,UPPER(B.BankName) AS BankName FROM LookupBank AS B WHERE B. IsActive = 1 ORDER BY B.BankName").Tables[0];
            var reportParam = new Dictionary<string, object> {  };
            ReportHelper.ShowReport(data, exportType, "rpt_BankList.rpt", "rpt_BankList", reportParam);
        }
        public void Index()
        {
            var report = SessionHelper.Report;
            var exportOption = SessionHelper.ReportFormat;
            var exportFileName = SessionHelper.ReportExportName;
            System.Web.HttpContext.Current.Response.BufferOutput = true;
            report.ExportToHttpResponse(exportOption, System.Web.HttpContext.Current.Response, false, exportFileName);
        }
        public ActionResult GnlRpt()
        {
            var moduleid = sPService.GetSecurityModuleByControllerAction("Reports", "GnlRpt");
            ViewBag.Reports = sPService.GetDataWithParameter(new
            {
                USER_ID = SessionHelper.LoggedInUserId,
                ReportModuleId = moduleid
            }, "USP_GET_USER_ACCESSED_REPORT").Tables[0].AsEnumerable().Select(x => new ReportInformation()
            {
                Id = x.Field<int>(0),
                ReportName = x.Field<string>(1),
                SerialNo = x.Field<int>(2)
            }).ToList();
            return View();
        }
        #region EMP Report
        public ActionResult ReleasedEmployeelist()
        {
            var model = new EmployeeReportViewModel();
            var itemList = new List<SelectListItem>();
            itemList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            ViewBag.JobTypeList = sPService.GetDataWithParameter(new { PJOB_ID = -1 }, "FSP_GET_HR_JOB_TYPE").Tables[0].AsEnumerable().Select(row => new { JOB_ID = row.Field<int>("JOB_ID"), JOB_NAME = row.Field<string>("JOB_NAME") }).ToList();
            ViewBag.BloodGroupList = sPService.GetDataWithParameter(new { PBLOOD_GROUP_ID = -1 }, "FSP_GET_HR_BLOOD_GROUP").Tables[0].AsEnumerable().Select(row => new { BLOOD_GROUP_ID = row.Field<int>("BLOOD_GROUP_ID"), BLOOD_GROUP_NAME = row.Field<string>("BLOOD_GROUP_NAME") }).ToList();
            model.ReportNameList = itemList;
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["DesignationList"] = items;
            return View(model);
        }
        public ActionResult EmployeelistReport()
        {
            var model = new EmployeeReportViewModel();
            var itemList = new List<SelectListItem>();
            itemList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            ViewBag.JobTypeList = sPService.GetDataWithParameter(new { PJOB_ID = -1 }, "FSP_GET_HR_JOB_TYPE").Tables[0].AsEnumerable().Select(row => new { JOB_ID = row.Field<int>("JOB_ID"), JOB_NAME = row.Field<string>("JOB_NAME") }).ToList();
            ViewBag.BloodGroupList = sPService.GetDataWithParameter(new { PBLOOD_GROUP_ID = -1 }, "FSP_GET_HR_BLOOD_GROUP").Tables[0].AsEnumerable().Select(row => new { BLOOD_GROUP_ID = row.Field<int>("BLOOD_GROUP_ID"), BLOOD_GROUP_NAME = row.Field<string>("BLOOD_GROUP_NAME") }).ToList();
            model.ReportNameList = itemList;

            var companyList = sPService.GetDataWithoutParameter("USP_GET_SisterConcern_List_For_DropdownList").Tables[0];
            ViewBag.CompanyList = companyList;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["DesignationList"] = items;
            return View(model);
        }
        public ActionResult EmployeeDetailsReport()
        {
            var model = new EmployeeReportViewModel();
            var itemList = new List<SelectListItem>();
            itemList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            return View(model);
        }
        public void ShowEmployeelistReport(string rptslnx, string StartDate, string EndDate, int BranchId, int DepartmentId, int PDESK_ID, int JobTypeId, int PDESG_ID_FROM, int PDESG_ID_TO, int PBLOOD_GROUP_ID, int PEMP_ACTIVE_STATUS, string ReportType)
        {
            try
            {
                var param = new
                {
                    PEFFECTIVE_date = ReportHelper.FormatDateToString(StartDate),
                    PDECLARATION_date = ReportHelper.FormatDateToString(EndDate),
                    PBRANCH_ID = BranchId,
                    PDEPT_ID = DepartmentId,
                    PDESK_ID = PDESK_ID,
                    JobTypeId = JobTypeId,
                    PDESG_ID_FROM = PDESG_ID_FROM,
                    PDESG_ID_TO = PDESG_ID_TO,
                    PBLOOD_GROUP_ID = PBLOOD_GROUP_ID,
                    PEMP_ACTIVE_STATUS = PEMP_ACTIVE_STATUS

                };
                var MainReport = sPService.GetDataWithParameter(param, "RSP_GET_EMP_LIST_BG_WISE");

                var reportParam = new Dictionary<string, object>();

                ReportHelper.ShowReport(MainReport.Tables[0], ReportType, "rpt_Employee_List_Blood_Group_Wise.rpt", "rpt_Employee_List_Blood_Group_Wise", null, null, BranchId);
            }
            catch (Exception ex)
            {
                //return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public void ShowEmployeeDetailsReport(string rptslnx, int BranchId, int DepartmentId, string ExperienceDate, string ExportType)
        {
            try
            {
                var param = new
                {
                    EMP_Code = "",
                    Emp_Name = "",
                    Designation_Id = 0,
                    Branch_Id = BranchId,
                    Department_Id = DepartmentId,
                    BloodGroup_Id = 0,
                    Desk_Id = 0,
                    JobType_Id = 0,
                    District_Id = 0,
                    Religion_Id = 0,
                    OnExperienceDate = ReportHelper.FormatDateToString(ExperienceDate)

                };
                var MainReport = sPService.GetDataWithParameter(param, "FSP_GET_EMP_PROFILE_INDIVIDUAL");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.ShowReport(MainReport.Tables[0], ExportType, "rpt_Employee_Information_Dtails.rpt", "rpt_Employee_Information_Dtails.cs");
            }
            catch (Exception ex)
            {
                //return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public void ShowReleasedEmployeeReport(string rptslnx, int DepartmentId, string ExportType)
        {
            try
            {
                var param = new
                {
                    DepartmentId = DepartmentId,
                };
                var MainReport = sPService.GetDataWithParameter(param, "USP_GET_RELEASE_EMPLOYEE_DETAISL");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.ShowReport(MainReport.Tables[0], ExportType, "rpt_Released_Employee_List.rpt", "rpt_Released_Employee_List");
            }
            catch (Exception ex)
            {
                //return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}