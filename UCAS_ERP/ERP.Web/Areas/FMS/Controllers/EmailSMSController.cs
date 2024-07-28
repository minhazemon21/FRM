using ERP.Web.Helpers;
using ERP.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERP.Web.Controllers;


namespace FMS.Web.Controllers
{
    public class EmailSMSController : BaseController
    {
        public ActionResult EmailSending()
        {
            return View();
        }
        #region
        [ValidateInput(false)]
        public JsonResult SendingEmail(string email, string ccemail, string message, List<HttpPostedFileBase> emailAttachment = null, string subject = "")
        {
            try
            {
                if(!string.IsNullOrWhiteSpace(email) && ReportHelper.IsValidEmail(email))
                {
                        var allEmails = email;
                        var allfiles = emailAttachment;//Request.Files;
                        var attachments = new Dictionary<string, string>();
                        if (allfiles != null)
                        {
                            var directroryPath = Server.MapPath("~/EmailAttachments/");
                            if (!Directory.Exists(directroryPath))
                            {
                                Directory.CreateDirectory(directroryPath);
                            }
                            var dir = new DirectoryInfo(directroryPath);
                            foreach (var f in dir.GetFiles())
                            {
                                f.Delete();
                            }
                            foreach (var file in allfiles)
                            {
                                if (file != null)
                                {
                                    file.SaveAs(Server.MapPath("~/EmailAttachments/" + file.FileName));
                                    attachments.Add(file.FileName, Server.MapPath("~/EmailAttachments/" + file.FileName));
                                }
                            }
                        }
                    ReportHelper.SendEmail(email, subject, message, null,null,ccemail, attachments);
                }
                
                var json = Json(new { Status = true, Message = "Email send successful." }, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.GetErrorMessage() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}