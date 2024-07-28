﻿using System;
using System.Configuration;
using System.Web.Mvc;
using Common.Service.StoredProcedure;
using Common.Data.CommonDataModel;

namespace ERP.Web.Controllers
{
    public class DatabaseBackupController : BaseController
    {
        private readonly ISPService spService;

        public DatabaseBackupController(ISPService spService)
        {
            this.spService = spService;
        }
        public ActionResult Index()
        {
            ViewBag.FileLocation = ConfigurationManager.AppSettings["DatabaseBackupPath"];
            return View();
        }

        public JsonResult GenerateDatabaseBackup(string fileName)
        {
            try
            {
                var location = ConfigurationManager.AppSettings["DatabaseBackupPath"];
                var context = new CommonDbContext();
                var database = context.Database.Connection.Database;
                //spService.GetDataBySqlCommand(@"BACKUP DATABASE " + database + " TO DISK = '" + path + "'");
                spService.GetDataWithParameter(new { DATABASE_NAME = database, BACK_UP_PATH = location, FILE_NAME = fileName },
                    "USP_GENERATE_DATABASE_BACKUP");
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Status = true, Message = "Backup Successfull." }, JsonRequestBehavior.AllowGet);
        }
    }
}