using Common.Service;
using Common.Service.StoredProcedure;
using ERP.Web.Helpers;
using ERP.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Hrms.Controllers.MobileAllowanceController;

namespace Hrms.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ISPService spService;
        public ProjectController(ISPService spService)
        {
            this.spService = spService;

        }
        // GET: Project
        public ActionResult Index()
        {
            var companyList = spService.GetDataWithoutParameter("USP_GET_SisterConcern_List_For_DropdownList").Tables[0];
            ViewBag.CompanyList = companyList;
            return View();
        }
        public ActionResult EmployeeWiseProject()
        {
            return View();
        }
        public ActionResult EmployeeProjectList()
        {
            var projectList = spService.GetDataWithoutParameter("USP_GET_ProjectListforDD").Tables[0];
            ViewBag.projectList = projectList;
            return View();
        }
        public ActionResult ProjectAssign()
        {
            var projectList = spService.GetDataWithoutParameter("USP_GET_ProjectListforDD").Tables[0];
            ViewBag.projectList = projectList;
            return View();
        }
        public ActionResult ProjectEdit(int Id, int empId)
        {
            ViewBag.empId = empId;
            ViewBag.Id = Id;
            var editdata = spService.GetDataWithParameter(new { Id = Id }, "USP_EmployeeProjectAssignEdit").Tables[0];
            ViewBag.editdata = editdata;

            return View();
        }
        public JsonResult SaveProjectSetup(string projectName, string projectShortName, string projectAddress, string projectDescription, string projectODate, int CompanyId, int Id = 0)
        {
            try
            {
                var data = spService.GetDataWithParameter(new { ProjectName = projectName, ProjectSName = projectShortName, ProjectAddress = projectAddress, ProjectDescr = projectDescription, ProjectOpDate = ReportHelper.FormatDateToString(projectODate), CompanyId = CompanyId, UserId = SessionHelper.LoggedInUserId, Id = Id }, "USP_INSERT_ProjectSetup").Tables[0].AsEnumerable().Select(R => new
                {
                    msg = R.Field<string>("Message")
                }).FirstOrDefault();

                return Json(new { Message = data.msg, Status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.InnerException.Message, Status = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveProjectAssign( string proIds = "", string datefroms = "",string dateTos = "", string remarkss = "", int empId = 0)
        {
            try
            {
                var param = new { proIds = proIds, datefroms = datefroms, dateTos = dateTos, remarkss = remarkss, empId = empId, UserId = SessionHelper.LoggedInUserId };
                spService.GetDataWithParameter(param, "USP_Insert_ProjectAssign");
                return Json(new { Status = true, Result = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.InnerException.Message == null ? ex.Message : ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProjectDetails()
        {
            try

            {
                var projectList = spService.GetDataWithoutParameter("USP_GET_ProjectList").Tables[0]
                    .AsEnumerable().Select(x => new
                    {
                        Id = x.Field<int>("Id"),
                        RowSl = x.Field<string>("RowSl"),
                        ProjectName = x.Field<string>("ProjectName"),
                        ProjectShortName = x.Field<string>("ProjectShortName"),
                        ProjectAddress = x.Field<string>("ProjectAddress"),
                        ProjectDescription = x.Field<string>("ProjectDescription"),
                        CompanyId = x.Field<int>("CompanyId"),
                        ProjectOpeningDate = x.Field<string>("ProjectOpeningDate"),
                        ProjectCloseingDate = x.Field<string>("ProjectCloseingDate")


                    }).ToList();
                //return Json(projectList, JsonRequestBehavior.AllowGet);
                return Json(new { Status = true, Result = projectList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProjectDetailsempIdwise(string empCode)
        {
            try

            {
                var projectList = spService.GetDataWithParameter(new { empCode = empCode }, "USP_GET_ProjectListbyempId").Tables[0]
                    .AsEnumerable().Select(x => new
                    {
                        Id = x.Field<int>("Id"),
                        RowSl = x.Field<string>("RowSl"),
                        ProjectName = x.Field<string>("ProjectName"),
                        ProjectShortName = x.Field<string>("ProjectShortName"),
                        ProjectAddress = x.Field<string>("ProjectAddress"),
                        ProjectDescription = x.Field<string>("ProjectDescription"),
                        CompanyId = x.Field<int>("CompanyId"),
                        ProjectOpeningDate = x.Field<string>("ProjectOpeningDate"),
                        ProjectCloseingDate = x.Field<string>("ProjectCloseingDate")


                    }).ToList();
                //return Json(projectList, JsonRequestBehavior.AllowGet);
                return Json(new { Status = true, Result = projectList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProjectDetailsempIdwiseDD(string empCode)
        {
            try

            {
                var projectList = spService.GetDataWithParameter(new { empCode = empCode }, "USP_GET_ProjectListbyempIdDD").Tables[0]
                    .AsEnumerable().Select(x => new
                    {
                        Id = x.Field<int>("Id"),
                        ProjectName = x.Field<string>("ProjectName"),
                        ProjectShortName = x.Field<string>("ProjectShortName")


                    }).ToList();
                //return Json(projectList, JsonRequestBehavior.AllowGet);
                return Json(new { Status = true, Result = projectList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetProjectDetailsempIdwiseDDforUpdate(string empCode, int epId)
        {
            try

            {
                var projectList = spService.GetDataWithParameter(new { empCode = empCode, epId = epId }, "USP_GET_ProjectListbyempIdDDforup").Tables[0]
                    .AsEnumerable().Select(x => new
                    {
                        Id = x.Field<int>("Id"),
                        ProjectName = x.Field<string>("ProjectName"),
                        ProjectShortName = x.Field<string>("ProjectShortName")


                    }).ToList();
                //return Json(projectList, JsonRequestBehavior.AllowGet);
                return Json(new { Status = true, Result = projectList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DeleteProjectbyId(int Id)
        {
            try
            {

                var data = spService.GetDataWithParameter(new { Id = Id, UserId = SessionHelper.LoggedInUserId }, "USP_DELETE_ProjectListbyId").Tables[0].AsEnumerable().Select(R => new
                {
                    msg = R.Field<string>("Message")
                }).FirstOrDefault();

                return Json(new { Message = data.msg, Status = true }, JsonRequestBehavior.AllowGet);
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

        public JsonResult UpdateProjectCloseDate(int Id, string closeDate)
        {
            try
            {

                var data = spService.GetDataWithParameter(new { Id = Id, CloseDate = ReportHelper.FormatDateToString(closeDate), UserId = SessionHelper.LoggedInUserId }, "USP_Update_ProjectCloseDate").Tables[0].AsEnumerable().Select(R => new
                {
                    msg = R.Field<string>("Message")
                }).FirstOrDefault();

                return Json(new { Message = data.msg, Status = true }, JsonRequestBehavior.AllowGet);
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

        public JsonResult InsertEmployeewiseProject(List<EmployeewiseProject> allmobileid)
        {


            try
            {
                if (allmobileid.Count > 0)
                {
                    foreach (var data in allmobileid)
                    {
                        var param = new
                        {
                            ProjectId = data.projectId,
                            EmployeeId = data.employeeId,
                            AssignDate = ReportHelper.FormatDateToString(data.assignDate),
                            Remarks = data.remarks,
                            userid = SessionHelper.LoggedInUserId
                        };
                        var Data = spService.GetDataWithParameter(param, "USP_Insert_Employee_wise_Project");
                    }
                }



                return Json(new { Status = true, Result = "", Message = "Save successfull." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Result = "", Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public void ShowProjectDetailsReport()
        {
            var data = spService.GetDataWithoutParameter("USP_GET_ProjectList").Tables[0];
            ReportHelper.ShowReport(data, "pdf", "rpt_ProjectInformation.rpt", "rpt_ProjectInformation");

        }

        public JsonResult GetProjectListforSearch( int empId = 0, int ProjectId = 0)
        {
            try
            {
                var Pro = spService.GetDataWithParameter(new { EmployeId = empId, ProjectId = ProjectId }, "USP_GET_EmployeeProjectListForSearch").Tables[0];

                var Result = Pro.AsEnumerable().Select(row => new
                {
                    RowSl = row.Field<string>("RowSl"),
                    Id = row.Field<int>("Id"),
                    EmployeeId = row.Field<int>("EmployeeId"),
                    emp_name = row.Field<string>("emp_name"),
                    ProjectName = row.Field<string>("ProjectName"),
                    FormDate = row.Field<string>("FormDate"),
                    ToDate = row.Field<string>("ToDate")
                }).ToList();
                return Json(new { Status = true, Result = Result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.InnerException.Message == null ? ex.Message : ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DeleteEmployeeProjectbyId(int Id)
        {
            try
            {

                var data = spService.GetDataWithParameter(new { Id = Id, UserId = SessionHelper.LoggedInUserId }, "USP_DELETE_EmployeeProjectListbyId").Tables[0].AsEnumerable().Select(R => new
                {
                    msg = R.Field<string>("Message")
                }).FirstOrDefault();

                return Json(new { Message = data.msg, Status = true }, JsonRequestBehavior.AllowGet);
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

        public JsonResult UpdateProjectAssign(int Id, string dateFrom, string dateto, int projectId, string remarks, int employeeId)
        {
            try
            {
                var param = new { Id = Id, dateFrom = ReportHelper.FormatDateToString(dateFrom), dateto = ReportHelper.FormatDateToString(dateto), projectId = projectId, remarks = remarks, employeeId= employeeId, UserId = SessionHelper.LoggedInUserId };
                spService.GetDataWithParameter(param, "USP_Update_ProjectAssign");
                return Json(new { Status = true, Result = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.InnerException.Message == null ? ex.Message : ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public class EmployeewiseProject
        {
            public int projectId { get; set; }
            public int employeeId { get; set; }
            public string assignDate { get; set; }
            public string remarks { get; set; }
        }
    }
}