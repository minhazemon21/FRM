//using System.Web.UI.WebControls;
using System;
using System.Data;
using System.Web.Mvc;
using Common.Data.CommonDataModel;
using Common.Service;
using Common.Service.StoredProcedure;
using ERP.Web.Helpers;
using System.Linq;

namespace ERP.Web.Controllers
{
    public class LookupDesignationController : BaseController
    {
        #region Variables
        private readonly ILookupDesignationService lookupDesignationService;
        private readonly ISPService spService;
        public LookupDesignationController(ILookupDesignationService lookupDesignationService, ISPService spService)
        {
            this.lookupDesignationService = lookupDesignationService;
            this.spService = spService;
        }
        #endregion
        #region Methods
        public JsonResult GetDesignation()
        {
            var DesignationInfo = spService.GetDataWithoutParameter("USP_GET_ALL_DESIGNATION_LIST").Tables[0].AsEnumerable()
                .Select(row => new {

                    Id = row.Field<int>("Id"),
                    RowSl = row.Field<string>("RowSl"),
                    DesignationShortName = row.Field<string>("DesignationShortName"),
                    DesignationName = row.Field<string>("DesignationName"),
                    JobTypeId = row.Field<int?>("JobTypeId"),
                    JobTypeName =row.Field<string>("job_name"),
                    DisplayOrder = row.Field<int>("DisplayOrder")
                });
            return Json(DesignationInfo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DesignationDelete(string Id)
        {
            var Des = lookupDesignationService.GetById(Convert.ToInt32(Id));


            Des.IsActive = false;
            Des.UpdateDate = DateTime.Now;
            Des.UpdateUserId = SessionHelper.LoggedInUserId;
            lookupDesignationService.Update(Des);
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveDesignation(string Designation, string ShortName, string DesignationId, int JobTypeId,int desg_reportorder)
        {
            bool result = false;
            string message = "";
            try
            {

                if (DesignationId == "0") //Save
                {
                    var Desig = new LookupDesignation()
                    {
                        DesignationName = Designation,
                        DesignationShortName = ShortName,
                        JobTypeId = JobTypeId,
                        desg_reportorder = desg_reportorder,
                        IsActive = true,
                        CreateDate = DateTime.Now,
                        CreatedUserId = SessionHelper.LoggedInUserId
                    };
                     lookupDesignationService.Create(Desig);
                     result = true;
                     message = "Save Successful.";
                }
                else   // Edit
                {
                    var Des = lookupDesignationService.GetById(Convert.ToInt32(DesignationId));

                    Des.DesignationName = Designation;
                    Des.DesignationShortName = ShortName;
                    Des.JobTypeId = JobTypeId;
                    Des.desg_reportorder = desg_reportorder;
                    Des.UpdateDate = DateTime.Now;
                    Des.UpdateUserId = SessionHelper.LoggedInUserId;
                    lookupDesignationService.Update(Des);
                    result = true;
                    message = "Update Successful.";
                }

                return Json(new { result, message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //result = 0;
                return Json(new { result, message = ex.InnerException.Message },JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Events
        public ActionResult Index()
        {
            ViewBag.JobTypeList = spService.GetDataWithoutParameter("USP_HR_JOB_TYPE_LIST").Tables[0].AsEnumerable().Select(row => new { JobTypeId = row.Field<int>("job_id"), JobTypeName = row.Field<string>("job_name") }).ToList();
            return View();
        }
        #endregion

    }
}
