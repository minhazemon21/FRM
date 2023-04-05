using Common.Service.StoredProcedure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Data.CommonDataModel;
using Common.Web.Helpers;
using System.Data;
using Common.Service;
using System.EnterpriseServices;
using Common.Web.ViewModels;
using Common.Web.Controllers;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Windows.Media;
using System.Reflection;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Web.Services.Description;

namespace MMS.Web.Controllers
{
    public class BoardMemberController : Controller
    {
        private readonly ISPService spService;
        private readonly IOfficeExecutiveService officeExecutiveService;

        public BoardMemberController(IOfficeExecutiveService officeExecutiveService, ISPService spService)
        {
            this.officeExecutiveService = officeExecutiveService;
            this.spService = spService;
        }
        // GET: BoardMember
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetDesignationList()
        {
            try
            {
                var offcData = officeExecutiveService.GetAll();
                var data = spService.GetDataWithoutParameter("USP_GET_DESIGNATION").Tables[0].ToList<Designation>();
                var json = Json(new { Status = true, desigList = data, exeList = offcData }, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.GetErrorMessage() }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetOtherOrganizationList()
        {
            try
            {
                var data = spService.GetDataWithoutParameter("USP_GET_OTHER_ORGANIZATION").Tables[0].AsEnumerable().Select(x => new
                {
                    Id = x.Field<int>("Id"),
                    OrganizationName = x.Field<string>("OrganizationName")
                }).ToList();

                return Json(new { Status = true, othrOrgList = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.GetErrorMessage() }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetParticipantTypeList()
        {
            try
            {
                var data = spService.GetDataWithoutParameter("USP_GET_PARTICIPANT_TYPE").Tables[0].AsEnumerable().Select(x => new
                {
                    Id = x.Field<int>("Id"),
                    TypeName = x.Field<string>("TypeName")
                }).ToList();

                return Json(new { Status = true, typeList = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.GetErrorMessage() }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetBoardMemberList()
        {
            try
            {
                var dt = spService.GetDataWithoutParameter("USP_GET_BOARD_MEMBER_LIST");
                var data = dt.Tables[0].AsEnumerable().Select(x => new
                {
                    Id = x.Field<int>("Id"),
                    Name = x.Field<string>("Name"),
                    DesignationId = x.Field<int>("DesignationId"),
                    DesignationName = x.Field<string>("DesignationName"),
                    RepresentedOrganizationId = x.Field<int>("RepresentedOrganizationId"),
                    RepresentedOrganizationName = x.Field<string>("RepresentedOrganizationName"),
                    ExecutiveId = x.Field<int>("ExecutiveId"),
                    ExecutiveName = x.Field<string>("ExecutiveName"),
                    ParticipantTypeId = x.Field<int>("ParticipantTypeId"),
                    TypeName = x.Field<string>("TypeName"),
                    MobileNo = x.Field<string>("MobileNo"),
                    EmailAddress = x.Field<string>("EmailAddress"),
                    Remarks = x.Field<string>("Remarks"),
                    Photograph = x.Field<byte[]>("Photograph") != null ? "data:image;base64," + Convert.ToBase64String(x.Field<byte[]>("Photograph")) : "",
                    Signature = x.Field<byte[]>("Signature") != null ? "data:image;base64," + Convert.ToBase64String(x.Field<byte[]>("Signature")) : ""
                }).ToList();

                return Json(new { Status = true, memberList = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        //try
        //{
        //    var dt = spService.GetDataWithoutParameter("USP_GET_BOARD_MEMBER_LIST");
        //    var data = dt.Tables[0].AsEnumerable().Select(x => new
        //    {
        //        Id = x.Field<int>("Id"),
        //        Name = x.Field<string>("Name"),
        //        DesignationId = x.Field<int>("DesignationId"),
        //        DesignationName = x.Field<string>("DesignationName"),
        //        RepresentedOrganizationId = x.Field<int>("RepresentedOrganizationId"),
        //        RepresentedOrganizationName = x.Field<string>("RepresentedOrganizationName"),
        //        ExecutiveId = x.Field<int>("ExecutiveId"),
        //        ExecutiveName = x.Field<string>("ExecutiveName"),
        //        ParticipantTypeId = x.Field<int>("ParticipantTypeId"),
        //        TypeName = x.Field<string>("TypeName"),
        //        MobileNo = x.Field<string>("MobileNo"),
        //        EmailAddress = x.Field<string>("EmailAddress"),
        //        Remarks = x.Field<string>("Remarks")
        //    }).ToList();

        //     return Json(new { Status = true, memberList = data }, JsonRequestBehavior.AllowGet);
        //}
        //catch (Exception ex)
        //{
        //    return Json(new { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult SaveBoardMember(int Id, int executiveId, string memberName, int designationId, int representedOrgId, int participantTypeId, string remarks, string mobile, string email, HttpPostedFileBase photoGraph = null, HttpPostedFileBase signature = null)
        {
            try
            {
                byte[] photo, sign;
                photo = sign = null;
                if (photoGraph != null)
                    photo = ReportHelper.ConvertStreamToByte(photoGraph.InputStream);
                if (signature != null)
                    sign = ReportHelper.ConvertStreamToByte(signature.InputStream);

                var data = new DataTable();
                var conn = ConfigurationManager.ConnectionStrings["MMSDBContext"].ToString();
                using (var sqlconnection = new SqlConnection(conn))
                {
                    sqlconnection.Open();
                    using (var ds = new SqlDataAdapter("USP_SAVE_BOARD_MEMBER", sqlconnection))
                    {
                        var comm = ds.SelectCommand;
                        comm.CommandType = CommandType.StoredProcedure;

                        comm.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                        comm.Parameters.Add("@Name", SqlDbType.NVarChar).Value = memberName;
                        comm.Parameters.Add("@DesignationId", SqlDbType.Int).Value = designationId;
                        comm.Parameters.Add("@RepresentedOrganizationId", SqlDbType.Int).Value = representedOrgId;
                        comm.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = remarks;
                        comm.Parameters.Add("@ParticipantTypeId", SqlDbType.Int).Value = participantTypeId;
                        comm.Parameters.Add("@ExecutiveId", SqlDbType.Int).Value = executiveId;
                        comm.Parameters.Add("@Mobile", SqlDbType.VarChar).Value = mobile;
                        comm.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
                        comm.Parameters.Add("@pGraph", SqlDbType.VarBinary).Value = photo;
                        comm.Parameters.Add("@signa", SqlDbType.VarBinary).Value = sign;
                        comm.Parameters.Add("@EntryUser", SqlDbType.Int).Value = SessionHelper.LoggedInUserId;

                        ds.Fill(data);
                        //var rowsAffected = comm.ExecuteNonQuery();

                        sqlconnection.Close();
                    }
                }



                if (data.Rows[0]["Status"].ToString() == "0")
                {
                    return Json(new { Status = false, Message = "ইতিপূর্বে এই কর্মকর্তা সভায় অংশগ্রহনকারী হিসেবে ব্যবহৃত হয়েছে ।" }, JsonRequestBehavior.AllowGet);
                }
                if (Id == 0)
                {
                    return Json(new { Status = true, Message = "সভায় অংশগ্রহনকারীর তথ্য সফলভাবে সংরক্ষণ করা হয়েছে ।" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = true, Message = "সভায় অংশগ্রহনকারীর তথ্য সফলভাবে পরিবর্তন করা হয়েছে ।" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult RemoveBoardMemberById(int id = 0)
        {
            try
            {
                var param = new
                {
                    Id = id,
                    EntryUser = SessionHelper.LoggedInUserId
                };
                var procName = "USP_REMOVE_BOARD_MEMBER_BY_ID";
                spService.ExecuteStoredProcedure(param, procName);
                return Json(new { Status = true, Message = "আপনি অংশগ্রহনকারীর তথ্য সফলভাবে মুছে ফেলেছেন ।" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.GetErrorMessage() }, JsonRequestBehavior.AllowGet);
            }
        }
        public FileResult GetExecutivePhoto(int id = 0)
        {
            var o = officeExecutiveService.GetById(id);
            if (o != null && o.Photograph != null)
            {
                return File(o.Photograph, "image/jpg");
            }
            else
            {
                return null;
            }

        }
        public FileResult GetExecutiveSign(int id = 0)
        {
            var o = officeExecutiveService.GetById(id);
            if (o != null && o.Signature != null)
            {
                return File(o.Signature, "image/png");
            }
            else
            {
                return null;
            }
        }
        public JsonResult GetOfficeExecutiveForBoardMember(int Id)
        {
            try
            {
                var param = new
                {
                    Id = Id

                };
                string storedProcedureName = "USP_GET_OFFICE_EXECUTIVE_FOR_BOARD_MEMBER_INFO";

                var data = spService.GetDataWithParameter(param, storedProcedureName).Tables[0];
                var GMWAFD = data.AsEnumerable().Select(r => new
                {
                    Id = r.Field<int>("Id"),
                    ExecutiveName = r.Field<string>("ExecutiveName"),
                    DesignationId = r.Field<int>("DesignationId"),
                    Mobile = r.Field<string>("Mobile"),
                    Email = r.Field<string>("Email"),
                    RepresentedOrganizationId = r.Field<int>("RepresentedOrganizationId"),
                    ParticipantTypeId = r.Field<int>("ParticipantTypeId"),
                    Remarks = r.Field<string>("Remarks"),
                    //Photograph = r.Field<byte[]>("Photograph") != null ? "data:image;base64," + Convert.ToBase64String(r.Field<byte[]>("Photograph")) : "",
                    //Signature = r.Field<byte[]>("Signature") != null ? "data:image;base64," + Convert.ToBase64String(r.Field<byte[]>("Signature")) : ""

                }).FirstOrDefault();
                return Json(new { Status = true, Result = GMWAFD }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.GetErrorMessage() }, JsonRequestBehavior.AllowGet);
            }
        }
        private class Param
        {
            public int id { get; set; }
            public string Name { get; set; }
            public int DesignationId { get; set; }
            public int RepresentedOrganizationId { get; set; }
            public string Remarks { get; set; }
            public int ParticipantTypeId { get; set; }
            public int ExecutiveId { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public byte[] photoGraph { get; set; }
            public byte[] signature { get; set; }
            public int EntryUser { get; set; }
        }
    }
}