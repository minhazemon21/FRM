using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Common.Data.CommonDataModel;
using Common.Service;
using Common.Service.StoredProcedure;
using ERP.Web.Controllers;
using ERP.Web.Helpers;
using ERP.Web.ViewModels;


namespace ERP.Web.Controllers
{
    public class DepartmentController : BaseController
    {

        #region Variables

        private readonly IBrokerDepartmentService brokerDepartmentService;
        private readonly ISPService spService;

        public DepartmentController(IBrokerDepartmentService brokerDepartmentService, ISPService spService)
        {
            this.brokerDepartmentService = brokerDepartmentService;
            this.spService = spService;
        }

        #endregion




        #region Methods


        public ActionResult Department()
        {
            return View();
        }

        public JsonResult GetBrokerDepartment()
        {
            var DeptInfo = spService.GetDataWithoutParameter("USP_GET_ALL_DEPARTMENT_LIST").Tables[0].AsEnumerable()
                .Select(row => new {

                    Id = row.Field<int>("Id"),
                    RowSl = row.Field<string>("RowSl"),
                    DepartmentShortName = row.Field<string>("DepartmentShortName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DisplayOrder = row.Field<int>("DisplayOrder"),
                    OrganizationName = row.Field<string>("OrganizationName"),
                    OrganizationId = row.Field<int>("OrganizationId")
                });
            //var DeptInfo = brokerDepartmentService.GetAll();
            return Json(DeptInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DepartmentNameDelete(string Id)
        {
            var Des = brokerDepartmentService.GetById(Convert.ToInt32(Id));


            Des.IsActive = false;
            Des.UpdateDate = DateTime.Now;
            Des.UpdateUserId = SessionHelper.LoggedInUserId;
            brokerDepartmentService.Update(Des);
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveBrokerDepartment(string DepartmentName, string ShortName, string hdnBrokerDeptId,int DisplayOrder,int CompanyId)
        {
            bool result = false;
            string message = "";
            try
            {

                if (hdnBrokerDeptId == "0") //Save
                {
                    var Desig = new BrokerDepartment()
                    {
                        DepartmentName = DepartmentName,
                        DepartmentShortName = ShortName,
                        Depart_ReportOrder = DisplayOrder,
                        IsActive = true,
                        CompanyId = CompanyId,
                        CreateDate = DateTime.Now,
                        CreatedUserId = SessionHelper.LoggedInUserId
                    };
                    brokerDepartmentService.Create(Desig);
                    result = true;
                    message = "Save Successful.";
                }
                else   // Edit
                {
                    var Des = brokerDepartmentService.GetById(Convert.ToInt32(hdnBrokerDeptId));

                    Des.DepartmentName = DepartmentName;
                    Des.DepartmentShortName = ShortName;
                    Des.Depart_ReportOrder= DisplayOrder;
                    Des.CompanyId = CompanyId;
                    Des.UpdateDate = DateTime.Now;
                    Des.UpdateUserId = SessionHelper.LoggedInUserId;
                    brokerDepartmentService.Update(Des);
                    result = true;
                    message = "Update Successful.";
                }

                return Json(new { result, message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result, message = ex.InnerException.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion
        public ActionResult Index()
        {
            var companyList = spService.GetDataWithoutParameter("USP_GET_SisterConcern_List_For_DropdownList").Tables[0];
            ViewBag.CompanyList = companyList;
            return View();
        }

        //
        // GET: /Department/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Department/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Department/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Department/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Department/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Department/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Department/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
