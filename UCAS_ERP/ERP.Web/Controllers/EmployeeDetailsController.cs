#region namespace

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Service;
using Common.Service.StoredProcedure;
using ERP.Web.Controllers;
using ERP.Web.Helpers;
using FMS.Service;
using Common.Data.CommonDataModel;
using ERP.Web.Models;
#endregion

namespace FMS.Web.Controllers
{
    public class EmployeeDetailsController : BaseController
    {
        #region variables

        private readonly ISPService spService;
        private readonly IEmployeeDetailsService employeeDetailsService;
        private readonly ILookupDistrictService lookupDistrictService;
        private readonly ILookupThanaService lookupThanaService;
        private readonly ILookupCountryService lookupCountryService;
        private readonly ILookupDesignationService lookupDesignationService;
        private readonly ILookupRelationService lookupRelationService;
        private readonly IBrokerDepartmentService brokerDepartmentService;
        private readonly IOfficeBranchService officeBranchService;
        public EmployeeDetailsController(ISPService spService
            , IEmployeeDetailsService employeeDetailsService
            , ILookupDistrictService lookupDistrictService
            , ILookupThanaService lookupThanaService
            , ILookupCountryService lookupCountryService
            , ILookupDesignationService lookupDesignationService
            , ILookupRelationService lookupRelationService
            , IBrokerDepartmentService brokerDepartmentService
            , IOfficeBranchService officeBranchService
            )
        {
            this.spService = spService;
            this.employeeDetailsService = employeeDetailsService;
            this.lookupDistrictService = lookupDistrictService;
            this.lookupThanaService = lookupThanaService;
            this.lookupCountryService = lookupCountryService;
            this.lookupDesignationService = lookupDesignationService;
            this.lookupRelationService = lookupRelationService;
            this.brokerDepartmentService = brokerDepartmentService;
            this.officeBranchService = officeBranchService;
        }
        #endregion

        #region Methods
        public byte[] GetImageFromDataBase(int Id)
        {
            var InvImg = employeeDetailsService.GetByEmpId(Id);
            if (InvImg != null)
            {
                var img = InvImg.EMP_PICTURE_IMAGE;
                byte[] cover = img;
                return cover;
            }
            else
            {

                byte[] cover = null;
                return cover;
            }
        }
        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                MemoryStream ms = new MemoryStream(cover);
                return File(ms.ToArray(), "image/png");
            }
            else
            {
                string strImgPathAbsolute = HttpContext.Server.MapPath("~/Images/blank-headshot.jpg");
                Image img = Image.FromFile(strImgPathAbsolute);
                byte[] blnk;
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    blnk = ms.ToArray();
                }

                return File(blnk, "image/*");
            }
        }
        public byte[] GetSignFromDataBase(int Id)
        {
            var InvImg = employeeDetailsService.GetByEmpId(Id);
            if (InvImg != null)
            {
                var img = InvImg.EMP_SIGN_IMAGE;
                byte[] cover = img;
                return cover;
            }
            else
            {

                byte[] cover = null;
                return cover;
            }
        }
        public ActionResult RetrieveSign(int id)
        {
            byte[] cover = GetSignFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/*");
            }
            else
            {
                string strImgPathAbsolute = HttpContext.Server.MapPath("~/Images/signature.png");
                Image img = Image.FromFile(strImgPathAbsolute);
                byte[] blnk;
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    blnk = ms.ToArray();
                }

                return File(blnk, "image/*"); ;
            }
        }
        
        public JsonResult Get_EmployeeInfo_For_Edit(int EmployeeId)
        {
            try
            {
                var param = new { EmployeeId = EmployeeId };
                var empList = spService.GetDataWithParameter(param, "Get_EmployeeInfo_For_Edit");

                var EmployeeInfo = empList.Tables[0].AsEnumerable()
                .Select(row => new
                {
                    emp_id = row.Field<int>("emp_id"),
                    emp_office_code = row.Field<string>("emp_office_code"),
                    emp_name = row.Field<string>("emp_name"),
                    OfficeEmail = row.Field<string>("OfficeEmail"),
                    emp_datetimeof_birth = row.Field<string>("emp_datetimeof_birth"),
                    emp_joining_datetime = row.Field<string>("emp_joining_datetime"),
                    emp_confirmation_datetime = row.Field<string>("emp_confirmation_datetime"),
                    emp_status_id = row.Field<decimal?>("emp_status_id"),
                    emp_job_id = row.Field<int?>("emp_job_id"),
                    emp_desg_id = row.Field<int?>("emp_desg_id"),
                    emp_branch_id = row.Field<int?>("emp_branch_id"),
                    emp_dept_id = row.Field<int?>("emp_dept_id"),
                    emp_desk_id = row.Field<int?>("emp_desk_id"),
                    emp_increment_flag = row.Field<decimal?>("emp_increment_flag"),
                    emp_noof_increment = row.Field<decimal?>("emp_noof_increment"),

                    //Detail 
                    DetailId = row.Field<int?>("DetailId"),
                    blood_group_id = row.Field<int?>("blood_group_id"),
                    religion_id = row.Field<int?>("religion_id"),
                    marital_status_id = row.Field<int?>("marital_status_id"),
                    emp_country_id = row.Field<int?>("emp_country_id"),
                    emp_gender = row.Field<string>("emp_gender"),
                    emp_father_name = row.Field<string>("emp_father_name"),
                    emp_mother_name = row.Field<string>("emp_mother_name"),
                    emp_spouse_name = row.Field<string>("emp_spouse_name"),
                    emp_spouse_dateofbirth = row.Field<string>("emp_spouse_dateofbirth"),
                    emp_present_address = row.Field<string>("emp_present_address"),
                    emp_present_district_id = row.Field<int?>("emp_present_district_id"),
                    emp_permanent_address = row.Field<string>("emp_permanent_address"),
                    emp_permanent_district_id = row.Field<int?>("emp_permanent_district_id"),

                    emp_phone_office = row.Field<string>("emp_phone_office"),
                    emp_phone_residance = row.Field<string>("emp_phone_residance"),
                    emp_mobile = row.Field<string>("emp_mobile"),
                    emp_office_mail_address = row.Field<string>("emp_office_mail_address"),
                    emp_personal_mail_address = row.Field<string>("emp_personal_mail_address"),
                    emp_nation_id_no = row.Field<string>("emp_nation_id_no"),
                    emp_passport_no = row.Field<string>("emp_passport_no"),
                    remarks = row.Field<string>("remarks"),
                    emp_present_thana_id = row.Field<int?>("emp_present_thana_id"),
                    emp_permanent_thana_id = row.Field<int?>("emp_permanent_thana_id"),
                    emp_spouse_contact_no = row.Field<string>("emp_spouse_contact_no"),
                    emergency_contact_person_name = row.Field<string>("emergency_contact_person_name"),
                    emergency_contact_no = row.Field<string>("emergency_contact_no"),
                    relation_emergency_person = row.Field<int?>("relation_emergency_person"),
                    IsSalaryDisburse = row.Field<int?>("IsSalaryDisburse")

                }).ToList();

                return Json(EmployeeInfo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult EmployeeInfoListForAutoComplete()
        {
            try
            {

                var empList = spService.GetDataWithoutParameter("USP_EmployeeInfoListForAutoComplete");

                var List_EmployeeInfo = empList.Tables[0].AsEnumerable()
                .Select(row => new
                {

                    emp_id = row.Field<int>("emp_id"),
                    emp_office_code = row.Field<string>("emp_office_code"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                }).ToList();

                return Json(List_EmployeeInfo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveEmployeeDetails(int EmployeeId, string FatherName, string MotherName, string Gender, int BloodGroupList, int ReligionList, int MaritalStatusList
           , string SpouseName, string SpouseDateOfBirth, string SpouseContactNo, int CountryList, string NationalId, string PassportNo, string LandPhoneNo, string SellPhoneNo
           , string LandPhoneNoOffice, string EmailOffice, string EmailPersonal, string EmergencyContactPerson, string EmergencyContactNo, int? RelationList, string Remarks, string TaxIdentificationNumber
           , string PresentAddress, int? ddlDistrictlist, int? ddlThanalist, string PermanentAddress, int? ddlPerDistrictlist, int? ddlPerThanalist, EmployeeModel model)
        {
            try
            {
                //if (RelationList == "") { RelationList = null; };

                var param = new
                {
                    PEMP_ID = EmployeeId,
                    PBLOOD_GROUP_ID = BloodGroupList,
                    PRELIGION_ID = ReligionList,
                    PMARITAL_STATUS_ID = MaritalStatusList,
                    PEMP_COUNTRY_ID = CountryList,
                    PEMP_GENDER = Gender,
                    PEMP_FATHER_NAME = FatherName,
                    PEMP_FATHER_STATUS = 1,
                    PEMP_MOTHER_NAME = MotherName,
                    PEMP_MOTHER_STATUS = 1,
                    PEMP_SPOUSE_NAME = SpouseName,
                    PEMP_SPOUSE_dateOFBIRTH = SpouseDateOfBirth == "" ? null : ReportHelper.FormatDateToString(SpouseDateOfBirth),
                    PEMP_PRESENT_ADDRESS = PresentAddress,
                    PEMP_PRESENT_DISTRICT_ID = ddlDistrictlist,
                    PEMP_PERMANENT_ADDRESS = PermanentAddress,
                    PEMP_PERMANENT_DISTRICT_ID = ddlPerDistrictlist,
                    PEMP_PHONE_OFFICE = LandPhoneNoOffice,
                    PEMP_PHONE_RESIDANCE = LandPhoneNo,
                    PEMP_MOBILE = SellPhoneNo,
                    PEMP_OFFICE_MAIL_ADDRESS = EmailOffice,
                    PEMP_PERSONAL_MAIL_ADDRESS = EmailPersonal,
                    PEMP_NATION_ID_NO = NationalId,
                    PEMP_PASSPORT_NO = PassportNo,
                    PEMP_PRESENT_THANA_ID = ddlThanalist,
                    PEMP_PERMANENT_THANA_ID = ddlPerThanalist,
                    PEMP_SPOUSE_CONTACT_NO = SpouseContactNo,
                    PEMERGENCY_CONTACT_PERSON_NAME = EmergencyContactPerson,
                    PEMERGENCY_CONTACT_NO = EmergencyContactNo,
                    PEMERGENCY_PERSON_RELATION = RelationList,
                    PREMARKS = Remarks,
                    PUSER_ID = SessionHelper.LoggedInUserId,
                    TaxIdentificationNumber = TaxIdentificationNumber
                };
                spService.GetDataWithParameter(param, "FSP_ADD_UP_EMP_PERS_DET_PRO");


                //Image Upload

                var Emp = employeeDetailsService.GetByEmpId(EmployeeId);
                if (Emp != null)
                {
                    if (model.PhotographMsg != null)
                    {
                        byte[] data = new byte[model.PhotographMsg.ContentLength];
                        model.PhotographMsg.InputStream.Read(data, 0, model.PhotographMsg.ContentLength);
                        Emp.EMP_PICTURE_IMAGE = data;
                    }
                    //else
                    //{
                    //    Emp.EMP_PICTURE_IMAGE = null;
                    //}
                    if (model.SignatureMsg != null)
                    {
                        byte[] data = new byte[model.SignatureMsg.ContentLength];
                        model.SignatureMsg.InputStream.Read(data, 0, model.SignatureMsg.ContentLength);
                        Emp.EMP_SIGN_IMAGE = data;
                    }
                    //else
                    //{
                    //    Emp.EMP_SIGN_IMAGE = null;
                    //}
                    employeeDetailsService.Update(Emp);
                }

                return Json(new { Result = "Success", EmployeeId = EmployeeId, Message = "Save successfull. " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", EmployeeId = "0", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveEmployeeDetailsEx(int EmployeeId, string FatherName, string MotherName, string Gender, int BloodGroupList, int ReligionList, int MaritalStatusList
            , string SpouseName, string SpouseDateOfBirth, string SpouseContactNo, int CountryList, string NationalId, string PassportNo, string LandPhoneNo, string SellPhoneNo
            , string LandPhoneNoOffice, string EmailOffice, string EmailPersonal, string EmergencyContactPerson, string EmergencyContactNo, int? RelationList, string Remarks
            , string PresentAddress, int? ddlDistrictlist, int? ddlThanalist, string PermanentAddress, int? ddlPerDistrictlist, int? ddlPerThanalist)
        {
            try
            {
                //if (RelationList == "") { RelationList = null; };
                if (SpouseDateOfBirth == "01/01/1900")
                {
                    SpouseDateOfBirth = "";
                }
                var param = new
                {
                    PEMP_ID = EmployeeId,
                    PBLOOD_GROUP_ID = BloodGroupList,
                    PRELIGION_ID = ReligionList,
                    PMARITAL_STATUS_ID = MaritalStatusList,
                    PEMP_COUNTRY_ID = CountryList,
                    PEMP_GENDER = Gender,
                    PEMP_FATHER_NAME = FatherName,
                    PEMP_FATHER_STATUS = 1,
                    PEMP_MOTHER_NAME = MotherName,
                    PEMP_MOTHER_STATUS = 1,
                    PEMP_SPOUSE_NAME = SpouseName,
                    PEMP_SPOUSE_dateOFBIRTH = ReportHelper.FormatDateToString(SpouseDateOfBirth),
                    PEMP_PRESENT_ADDRESS = PresentAddress,
                    PEMP_PRESENT_DISTRICT_ID = ddlDistrictlist,
                    PEMP_PERMANENT_ADDRESS = PermanentAddress,
                    PEMP_PERMANENT_DISTRICT_ID = ddlPerDistrictlist,
                    PEMP_PHONE_OFFICE = LandPhoneNoOffice,
                    PEMP_PHONE_RESIDANCE = LandPhoneNo,
                    PEMP_MOBILE = SellPhoneNo,
                    PEMP_OFFICE_MAIL_ADDRESS = EmailOffice,
                    PEMP_PERSONAL_MAIL_ADDRESS = EmailPersonal,
                    PEMP_NATION_ID_NO = NationalId,
                    PEMP_PASSPORT_NO = PassportNo,
                    PEMP_PRESENT_THANA_ID = ddlThanalist,
                    PEMP_PERMANENT_THANA_ID = ddlPerThanalist,
                    PEMP_SPOUSE_CONTACT_NO = SpouseContactNo,
                    PEMERGENCY_CONTACT_PERSON_NAME = EmergencyContactPerson,
                    PEMERGENCY_CONTACT_NO = EmergencyContactNo,
                    PEMERGENCY_PERSON_RELATION = RelationList,
                    PREMARKS = Remarks,
                    PUSER_ID = SessionHelper.LoggedInUserId
                };
                spService.GetDataWithParameter(param, "FSP_ADD_UP_EMP_PERS_DET_PRO");
                return Json(new { Result = "Success", EmployeeId = EmployeeId, Message = "Save successfull. " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", EmployeeId = "0", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult EmployeeImageSave(int EmployeeId, EmployeeModel model)
        {
            var Empl = employeeDetailsService.GetByEmpId(EmployeeId);
            if (model.PhotographMsg != null)
            {

                byte[] data = new byte[model.PhotographMsg.ContentLength];
                model.PhotographMsg.InputStream.Read(data, 0, model.PhotographMsg.ContentLength);
                Empl.EMP_PICTURE_IMAGE = data;

            }

            if (model.SignatureMsg != null)
            {
                byte[] data = new byte[model.SignatureMsg.ContentLength];
                model.SignatureMsg.InputStream.Read(data, 0, model.SignatureMsg.ContentLength);
                Empl.EMP_SIGN_IMAGE = data;
            }
            employeeDetailsService.Update(Empl);

            return Json(new { Result = "Success", Message = "Successfull." }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Actions
        public ActionResult EmpDetails()
        {
            ViewBag.BloodGroupList = spService.GetDataWithParameter(new { PBLOOD_GROUP_ID = -1 }, "FSP_GET_HR_BLOOD_GROUP").Tables[0].AsEnumerable().Select(row => new { BLOOD_GROUP_ID = row.Field<int>("BLOOD_GROUP_ID"), BLOOD_GROUP_NAME = row.Field<string>("BLOOD_GROUP_NAME") }).ToList();
            ViewBag.ReligionList = spService.GetDataWithParameter(new { PRELIGION_ID = -1 }, "FSP_GET_HR_RELIGION").Tables[0].AsEnumerable().Select(row => new { RELIGION_ID = row.Field<int>("RELIGION_ID"), RELIGION_NAME = row.Field<string>("RELIGION_NAME") }).ToList();
            ViewBag.MaritalStatusList = spService.GetDataWithParameter(new { PMARITAL_STATUS_ID = -1 }, "FSP_GET_HR_MARITAL_STATUS").Tables[0].AsEnumerable().Select(row => new { MARITAL_STATUS_ID = row.Field<int>("MARITAL_STATUS_ID"), MARITAL_STATUS_NAME = row.Field<string>("MARITAL_STATUS_NAME") }).ToList();
            ViewBag.CountryList = lookupCountryService.GetAll().ToList();//spService.GetDataWithParameter(new { PCOUNTRY_ID = -1 }, "FSP_GET_HR_COUNTRY").Tables[0].AsEnumerable().Select(row => new { COUNTRY_ID = row.Field<int>("COUNTRY_ID"), COUNTRY_NAME = row.Field<string>("COUNTRY_NAME") }).ToList();
            ViewBag.RelationList = lookupRelationService.GetAll().Where(x => x.IsActive == true).ToList();//spService.GetDataWithParameter(new { PNOMINEE_RELATION_ID = -1 }, "FSP_GET_HR_NOMINEE_RELATION").Tables[0].AsEnumerable().Select(row => new { NOMINEE_RELATION_ID = row.Field<int>("NOMINEE_RELATION_ID"), NOMINEE_RELATION_NAME = row.Field<string>("NOMINEE_RELATION_NAME") }).ToList();

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["Districtlist"] = items;
            ViewData["Thanalist"] = items;
            ViewData["PerDistrictlist"] = items;
            ViewData["PerThanalist"] = items;
            //ViewData["DesignationList"] = items;
            return View();
        }
        public ActionResult EmpRelease()
        {
            return View();
        }
        #endregion
        #region Employee Details Section
        public JsonResult GetDesignationList(int JobTypeId)
        {
            var DesgList = lookupDesignationService.GetAll().Where(x => x.JobTypeId == JobTypeId && x.IsActive == true);
            return Json(DesgList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DepartmentListForDropdown(int CompanyId)
        {

            var Department = spService.GetDataWithParameter(new { CompanyId = CompanyId }, "USP_GET_DepartmentListForDropdown").Tables[0]
               .AsEnumerable().Select(row => new { Id = row.Field<int>("Id"), DepartmentName = row.Field<string>("DepartmentName"), DepartmentShortName = row.Field<string>("DepartmentShortName") }).ToList();
            return Json(Department, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_EmployeeInfo_Details_By_Code(string EmployeeCode)
        {
            try
            {
                var param = new { EmployeeCode = EmployeeCode };
                var empList = spService.GetDataWithParameter(param, "Get_EmployeeInfo_Details_By_Code").Tables[0];
                if (empList.Rows.Count == 0)
                {
                    return Json(new { Result = "NoFound", Message = "No employee found", EmployeeInfo = 0 }, JsonRequestBehavior.AllowGet);
                }
                var EmployeeInfo = empList.AsEnumerable()
                .Select(row => new
                {
                    emp_id = row.Field<int>("emp_id"),
                    emp_office_code = row.Field<string>("emp_office_code"),
                    emp_name = row.Field<string>("emp_name"),
                    OfficeEmail = row.Field<string>("OfficeEmail"),
                    emp_datetimeof_birth = row.Field<string>("emp_datetimeof_birth"),
                    emp_joining_datetime = row.Field<string>("emp_joining_datetime"),
                    emp_confirmation_datetime = row.Field<string>("emp_confirmation_datetime"),
                    emp_status_id = row.Field<decimal?>("emp_status_id"),
                    emp_job_id = row.Field<int?>("emp_job_id"),
                    emp_desg_id = row.Field<int?>("emp_desg_id"),
                    emp_branch_id = row.Field<int?>("emp_branch_id"),
                    emp_dept_id = row.Field<int?>("emp_dept_id"),
                    emp_desk_id = row.Field<int?>("emp_desk_id"),
                    emp_increment_flag = row.Field<decimal?>("emp_increment_flag"),
                    emp_noof_increment = row.Field<decimal?>("emp_noof_increment"),

                    //Detail 
                    DetailId = row.Field<int?>("DetailId"),
                    blood_group_id = row.Field<int?>("blood_group_id"),
                    religion_id = row.Field<int?>("religion_id"),
                    marital_status_id = row.Field<int?>("marital_status_id"),
                    emp_country_id = row.Field<int?>("emp_country_id"),
                    emp_gender = row.Field<string>("emp_gender"),
                    emp_father_name = row.Field<string>("emp_father_name"),
                    emp_mother_name = row.Field<string>("emp_mother_name"),
                    emp_spouse_name = row.Field<string>("emp_spouse_name"),
                    emp_spouse_dateofbirth = row.Field<string>("emp_spouse_dateofbirth"),
                    emp_present_address = row.Field<string>("emp_present_address"),
                    emp_present_district_id = row.Field<int?>("emp_present_district_id"),
                    emp_permanent_address = row.Field<string>("emp_permanent_address"),
                    emp_permanent_district_id = row.Field<int?>("emp_permanent_district_id"),


                    desg_name = row.Field<string>("DesignationName"),
                    Dept_name = row.Field<string>("DepartmentName"),
                    job_name = row.Field<string>("job_name"),
                    branch_name = row.Field<string>("BranchName"),
                    desk_name = row.Field<string>("desk_name"),

                    emp_phone_office = row.Field<string>("emp_phone_office"),
                    emp_phone_residance = row.Field<string>("emp_phone_residance"),
                    emp_mobile = row.Field<string>("emp_mobile"),
                    emp_office_mail_address = row.Field<string>("emp_office_mail_address"),
                    emp_personal_mail_address = row.Field<string>("emp_personal_mail_address"),
                    emp_nation_id_no = row.Field<string>("emp_nation_id_no"),
                    emp_passport_no = row.Field<string>("emp_passport_no"),
                    remarks = row.Field<string>("remarks"),
                    TaxIdentificationNumber = row.Field<string>("TaxIdentificationNumber"),
                    emp_present_thana_id = row.Field<int?>("emp_present_thana_id"),
                    emp_permanent_thana_id = row.Field<int?>("emp_permanent_thana_id"),
                    emp_spouse_contact_no = row.Field<string>("emp_spouse_contact_no"),
                    emergency_contact_person_name = row.Field<string>("emergency_contact_person_name"),
                    emergency_contact_no = row.Field<string>("emergency_contact_no"),
                    relation_emergency_person = row.Field<int?>("relation_emergency_person"),
                    IsSalaryDisburse = row.Field<int?>("IsSalaryDisburse")

                }).ToList();

                return Json(new { Result = "Success", Message = "", EmployeeInfo = EmployeeInfo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDistrictList()
        {
            var DistrictList = lookupDistrictService.GetAll().ToList();//spService.GetDataWithParameter(new { PDIVISION_ID = -1, PDISTRICT_ID = -1 }, "FSP_GET_HR_DISTRICT").Tables[0].AsEnumerable()
            // .Select(row => new { DISTRICT_ID = row.Field<int>("DISTRICT_ID"), DISTRICT_NAME = row.Field<string>("DISTRICT_NAME").ToList() });
            return Json(DistrictList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetddlThanaList(string DistrictId)
        {
            var ThanaList = lookupThanaService.GetAll().Where(x => x.IsActive == true && x.DistrictId == Convert.ToInt32(DistrictId));
            return Json(ThanaList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Insert_Employee_Release_Info(int EmployeeId, string ResigDate, string ResigAcceptDate, string ReleaseDate, string ReleaseOrderNo, string FileCloseDate, int EligibleForSalary, int RelEmpSalPayDay = 0, string ReleasedRemarks = "")
        {
            try
            {

                if (EmployeeId == 0)
                {
                    return Json(new { Status = false, Result = "Error", Message = "Please insert employee." }, JsonRequestBehavior.AllowGet);
                }
                else if (ResigDate == "")
                {
                    return Json(new { Status = false, Result = "Error", Message = "Please insert resignation date." }, JsonRequestBehavior.AllowGet);
                }
                else if (ResigAcceptDate == "")
                {
                    return Json(new { Status = false, Result = "Error", Message = "Please insert resignation accept date." }, JsonRequestBehavior.AllowGet);
                }
                else if (ReleaseDate == "")
                {
                    return Json(new { Status = false, Result = "Error", Message = "Please insert release date." }, JsonRequestBehavior.AllowGet);
                }
                else if (ReleaseOrderNo == "")
                {
                    return Json(new { Status = false, Result = "Error", Message = "Please insert release order no." }, JsonRequestBehavior.AllowGet);
                }
                else if (EligibleForSalary == -1)
                {
                    return Json(new { Status = false, Result = "Error", Message = "Please select release employee eligible for salary or not." }, JsonRequestBehavior.AllowGet);
                }
                else if (EligibleForSalary == 1 && RelEmpSalPayDay <= 0)
                {
                    return Json(new { Status = false, Result = "Error", Message = "Please insert salary payable day." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var param = new
                    {
                        EmployeeId = EmployeeId,
                        ResigDate = ResigDate == "" ? null : ReportHelper.FormatDateToString(ResigDate),
                        ResigAcceptDate = ResigAcceptDate == "" ? null : ReportHelper.FormatDateToString(ResigAcceptDate),
                        ReleaseDate = ReleaseDate == "" ? null : ReportHelper.FormatDateToString(ReleaseDate),
                        ReleaseOrderNo = ReleaseOrderNo,
                        FileCloseDate = FileCloseDate == "" ? null : ReportHelper.FormatDateToString(FileCloseDate),
                        EligibleForSalary = EligibleForSalary,
                        RelEmpSalPayDay = RelEmpSalPayDay,
                        ReleasedRemarks = ReleasedRemarks
                    };

                    spService.GetDataWithParameter(param, "USP_Save_EmployeeReleaseInfo");
                    spService.GetDataWithParameter(new { EmployeeId = EmployeeId }, "USP_UPDATE_MblInfoForReleaseEmployee");
                    return Json(new { Status = true, Result = "Success", Message = "Save successfull." }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Result = "Error", Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }

}