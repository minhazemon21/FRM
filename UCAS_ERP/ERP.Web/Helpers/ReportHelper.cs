using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ERP.Web.ViewModels;
using System.Threading;
using Common.Service.StoredProcedure;
using Common.Data.CommonDataModel;
using MySql.Data.MySqlClient;
using System.Net.Http;
using Utility;
using Newtonsoft.Json;

namespace ERP.Web.Helpers
{
    public static class ReportHelper
    {
        public static void PrintReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters)
        {
            try
            {

                var crDocument = new ReportDocument();

                var crExportOptions = new ExportOptions();
                var crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                var strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);

                foreach (var kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value); 
                }
                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDocument.Export();
                crDocument.Dispose();
                crDocument.Close();
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();
                // Response.Close();
                File.Delete(strFName);

            }
            catch (Exception ex)
            {

            }
        }

        public static void PrintWithSubReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters, Dictionary<string, DataTable> subReportDatasources, ReportClass reportClass)
        {
            try
            {


                // ReportDocument crDocument = new ReportDocument();

                var crExportOptions = new ExportOptions();
                var crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                var strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/"+ SessionHelper.OrganizationShortName +"/" + reportName);
                reportClass.Load(strReportPathAbsolute);
                reportClass.SetDataSource(dataSource);

                foreach (var kvp in parameters)
                {
                    reportClass.SetParameterValue(kvp.Key, kvp.Value);

                }
                if (subReportDatasources != null)
                {
                    foreach (var kvp in subReportDatasources)
                    {
                        reportClass.OpenSubreport(kvp.Key).SetDataSource(kvp.Value);
                    }
                }

                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = reportClass.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                reportClass.Export();
                reportClass.Dispose();
                reportClass.Close();
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();
                // Response.Close();
                File.Delete(strFName);

            }
            catch (Exception ex)
            {


            }

        }


        public static void ShowReport(DataTable data, string exportType
          , string reportName, string exportFileName
          , Dictionary<string, object> reportParameterCollection = null
          , Dictionary<int, DataTable> reportSubreportCollection = null
          , int companyId = 0
          )
        {
            if (HttpContext.Current.Response.IsRequestBeingRedirected)
                return;

            if (data.Rows.Count == 0)
            {
                HttpContext.Current.Response.Write("<script type='text/javascript'>alert('No data found.'); window.close();</script>");
                HttpContext.Current.Response.End();
                return;
            }
            if (HttpContext.Current.Request.UrlReferrer == null)
            {
                HttpContext.Current.Response.Write("<script type='text/javascript'>alert('Unauthorized.'); window.close();</script>");
                HttpContext.Current.Response.End();
                return;
            }
            var arrReferer = HttpContext.Current.Request.UrlReferrer.AbsolutePath.Split('/');
            var path = "~/";
            var areas = SessionHelper.Areas;
            if (areas.Contains(arrReferer[1]))
            {
                path = "~/Areas/" + arrReferer[1] + "/";
            }

            var reportPath = HttpContext.Current.Server.MapPath(path + @"Reports/" + SessionHelper.OrganizationShortName + @"/" + reportName);

            var report = new ReportDocument();
            report.Load(reportPath);
            report.SetDataSource(data);
            if (reportSubreportCollection != null)
            {
                foreach (var subReportPair in reportSubreportCollection)
                {
                    report.Subreports[subReportPair.Key].SetDataSource(subReportPair.Value);
                }
            }
            if (reportParameterCollection != null)
            {
                foreach (var parameterPair in reportParameterCollection)
                {
                    report.SetParameterValue(parameterPair.Key, parameterPair.Value);
                }
            }
            var exportOption = ExportFormatType.PortableDocFormat;
            switch (exportType.ToLower())
            {
                case "excel":
                    exportOption = ExportFormatType.Excel;
                    break;
                case "exceldata":
                    exportOption = ExportFormatType.ExcelRecord;
                    break;
                case "excelbook":
                    exportOption = ExportFormatType.ExcelWorkbook;
                    break;
                case "word":
                    exportOption = ExportFormatType.WordForWindows;
                    break;
                case "xml":
                    exportOption = ExportFormatType.Xml;
                    break;
            }

            report = SetReportHeader(report, reportName, companyId);

            var mode = ConfigurationManager.AppSettings["Mode"];
            HttpContext.Current.Response.BufferOutput = true;
            if (mode == "0")
            {
                SessionHelper.Report = report;
                SessionHelper.ReportFormat = exportOption;
                SessionHelper.ReportExportName = exportFileName;
                HttpContext.Current.Response.Redirect("~/Reports/");
            }
            else if (mode == "1")
            {
                report.ExportToHttpResponse(exportOption, HttpContext.Current.Response, false, exportFileName);
            }
            report.Dispose();
            report.Close();
        }
        public static ReportDocument SetReportHeader(ReportDocument report, string reportName,int companyId = 0)
        {
            var spService = new SPService();
            if(companyId == 0)
            {
                report.SummaryInfo.ReportAuthor = HttpContext.Current.Server.MapPath(SessionHelper.OrganizationLogo);
                report.SummaryInfo.ReportTitle = SessionHelper.OrganizationName;
                report.SummaryInfo.ReportComments = SessionHelper.OrganizationAddress;
            }
            else
            {
                var comInfo = spService.GetDataWithParameter(new { CompanyId = companyId }, "USP_RPT_GET_SisterConcernInfo").Tables[0].AsEnumerable()
                .Select(R => new
                {
                    CompanyLogo = R.Field<string>("OrganizationLogoName"),
                    CompanyName = R.Field<string>("OrganizationName"),
                    CompanyAddress = R.Field<string>("OrganizationAddress")
                }).FirstOrDefault();
                report.SummaryInfo.ReportAuthor = HttpContext.Current.Server.MapPath(comInfo.CompanyLogo);
                report.SummaryInfo.ReportTitle = comInfo.CompanyName;
                report.SummaryInfo.ReportComments = comInfo.CompanyAddress;
            }

            if (IsFixedHeaderReport(reportName))
                return report;
            var header = new ReportHeader();
            //if (IsLandscapeReport(reportName))
            if (report.PrintOptions.PaperOrientation == PaperOrientation.Landscape)
                header =
                    SessionHelper.ReportHeader.Where(x => x.ReportType.ToLower() == "landscape")
                        .ToList()
                        .FirstOrDefault();
            else
                header =
                    SessionHelper.ReportHeader.Where(x => x.ReportType.ToLower() == "portrait")
                        .ToList()
                        .FirstOrDefault();


            var companyName = (FieldObject)report.ReportDefinition.ReportObjects["CompanyName"];
            var companyNameFont = new Font("Arial", header.CompanyNameFontSize, header.CompanyNameBold ? FontStyle.Bold : FontStyle.Regular);
            companyName.ApplyFont(companyNameFont);

            companyName.ObjectFormat.EnableCanGrow = true;
            companyName.Color = Color.Black;
            companyName.Left = header.CompanyNameLeft;
            companyName.Top = header.CompanyNameTop;
            companyName.Height = header.CompanyNameHeight;
            companyName.Width = header.CompanyNameWidth;
            if (header.CompanyNameAlign.ToLower() == "center")
                companyName.ObjectFormat.HorizontalAlignment = Alignment.HorizontalCenterAlign;
            if (header.CompanyNameAlign.ToLower() == "left")
                companyName.ObjectFormat.HorizontalAlignment = Alignment.LeftAlign;
            if (header.CompanyNameAlign.ToLower() == "right")
                companyName.ObjectFormat.HorizontalAlignment = Alignment.RightAlign;
            if (header.CompanyNameAlign.ToLower() == "justified")
                companyName.ObjectFormat.HorizontalAlignment = Alignment.Justified;

            var companyAddress = (FieldObject)report.ReportDefinition.ReportObjects["CompanyAddress"];
            var companyAddressFont = new Font("Arial", header.CompanyAddressFontSize, header.CompanyAddressBold ? FontStyle.Bold : FontStyle.Regular);
            companyAddress.ApplyFont(companyAddressFont);
            companyAddress.ObjectFormat.EnableCanGrow = true;
            companyAddress.Color = Color.Black;
            companyAddress.Left = header.CompanyAddressLeft;
            companyAddress.Top = header.CompanyAddressTop;
            companyAddress.Height = header.CompanyAddressHeight;
            companyAddress.Width = header.CompanyAddressWidth;
            if (header.CompanyAddressAlign.ToLower() == "center")
                companyAddress.ObjectFormat.HorizontalAlignment = Alignment.HorizontalCenterAlign;
            if (header.CompanyAddressAlign.ToLower() == "left")
                companyAddress.ObjectFormat.HorizontalAlignment = Alignment.LeftAlign;
            if (header.CompanyAddressAlign.ToLower() == "right")
                companyAddress.ObjectFormat.HorizontalAlignment = Alignment.RightAlign;
            if (header.CompanyAddressAlign.ToLower() == "justified")
                companyAddress.ObjectFormat.HorizontalAlignment = Alignment.Justified;

            var reportsName = (TextObject)report.ReportDefinition.ReportObjects["ReportName"];
            var reportNameFont = new Font("Arial", header.ReportNameFontSize, header.ReportNameBold ? FontStyle.Bold : FontStyle.Regular);
            reportsName.ApplyFont(reportNameFont);
            reportsName.ObjectFormat.EnableCanGrow = true;
            reportsName.Color = Color.Black;
            reportsName.Left = header.ReportNameLeft;
            reportsName.Top = header.ReportNameTop;
            reportsName.Height = header.ReportNameHeight;
            reportsName.Width = header.ReportNameWidth;
            if (header.ReportNameAlign.ToLower() == "center")
                reportsName.ObjectFormat.HorizontalAlignment = Alignment.HorizontalCenterAlign;
            if (header.ReportNameAlign.ToLower() == "left")
                reportsName.ObjectFormat.HorizontalAlignment = Alignment.LeftAlign;
            if (header.ReportNameAlign.ToLower() == "right")
                reportsName.ObjectFormat.HorizontalAlignment = Alignment.RightAlign;
            if (header.ReportNameAlign.ToLower() == "justified")
                reportsName.ObjectFormat.HorizontalAlignment = Alignment.Justified;

            var firstLine = (TextObject)report.ReportDefinition.ReportObjects["FirstLine"];

            firstLine.Left = header.FirstLineLeft;
            firstLine.Top = header.FirstLineTop;
            firstLine.Width = header.FirstLineWidth;
            firstLine.ObjectFormat.EnableSuppress = header.FirstLineSuppress;

            var secondLine = (TextObject)report.ReportDefinition.ReportObjects["SecondLine"];
            secondLine.Left = header.SecondLineLeft;
            secondLine.Top = header.SecondLineTop;
            secondLine.Width = header.SecondLineWidth;
            secondLine.ObjectFormat.EnableSuppress = header.SecondLineSuppress;

            var companyLogo = (PictureObject)report.ReportDefinition.ReportObjects["CompanyLogo"];

            companyLogo.Left = header.CompanyLogoLeft;
            companyLogo.Top = header.CompanyLogoTop;
            companyLogo.Height = header.CompanyLogoHeight;
            companyLogo.Width = header.CompanyLogoWidth;



            return report;
        }
        public static void SaveReport(
            string directory, object data, string exportType
            , string reportName, string exportFileName
            , Dictionary<string, object> reportParameterCollection = null
            , Dictionary<int, DataTable> reportSubreportCollection = null
            )
        {
            var arrReferer = HttpContext.Current.Request.UrlReferrer.AbsolutePath.Split('/');
            var path = "~/";
            var areas = SessionHelper.Areas;
            if (areas.Contains(arrReferer[1]))
            {
                path = "~/Areas/" + arrReferer[1] + "/";
            }
            var reportPath = HttpContext.Current.Server.MapPath(path + @"Reports/" + SessionHelper.OrganizationShortName + @"/" + reportName);

            var report = new ReportDocument();
            report.Load(reportPath);
            report.SetDataSource(data);

            if (reportSubreportCollection != null)
            {
                foreach (var subReportPair in reportSubreportCollection)
                {
                    report.Subreports[subReportPair.Key].SetDataSource(subReportPair.Value);
                }
            }
            if (reportParameterCollection != null)
            {
                foreach (var parameterPair in reportParameterCollection)
                {
                    report.SetParameterValue(parameterPair.Key, parameterPair.Value);
                }
            }
            var exportOption = ExportFormatType.PortableDocFormat;
            var ext = "pdf";
            switch (exportType.ToLower())
            {
                case "excel":
                    exportOption = ExportFormatType.Excel;
                    ext = "xls";
                    break;
                case "exceldata":
                    exportOption = ExportFormatType.ExcelRecord;
                    ext = "xls";
                    break;
                case "excelbook":
                    exportOption = ExportFormatType.ExcelWorkbook;
                    ext = "xlsx";
                    break;
                case "word":
                    exportOption = ExportFormatType.WordForWindows;
                    ext = "doc";
                    break;
                case "xml":
                    exportOption = ExportFormatType.Xml;
                    ext = "xml";
                    break;
            }

            report = SetReportHeader(report, reportName);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            var filePath = directory + "/" + exportFileName + "." + ext;
            if (File.Exists(filePath))
                File.Delete(filePath);
            using (var stream = report.ExportToStream(exportOption))
            {
                var fileStream = File.Create(filePath, (int)stream.Length);
                var bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, bytesInStream.Length);
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
                fileStream.Close();
            }
            report.Dispose();
            report.Close();
        }

        public static void SaveDatatableInTextFormat(string directory, DataTable data, string exportFileName, string delimeter, string format = "txt")
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            var filePath = directory + "/" + exportFileName + "." + format;
            if (File.Exists(filePath))
                File.Delete(filePath);
            using (var file = File.CreateText(filePath))
            {
                for (var row = 0; row < data.Rows.Count; row++)
                {
                    var stringToWrite = string.Join(delimeter, data.Rows[row].ItemArray);
                    file.WriteLine(stringToWrite);
                }
            }
        }

        public static void SaveStringToXMLFile(string directory, string data, string exportFileName, string format = "xml")
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            var filePath = directory + "/" + exportFileName + "." + format;
            if (File.Exists(filePath))
                File.Delete(filePath);
            using (var file = File.CreateText(filePath))
            {
                file.WriteLine(data);
            }
        }

        public static DateTime FormatDate(string date)
        {
            return !string.IsNullOrEmpty(date) ? DateTime.ParseExact(date, "dd/MM/yyyy", null) : default(DateTime);
        }

        public static string FormatDateToString(string date)
        {
            return !string.IsNullOrEmpty(date) ? DateTime.ParseExact(date, "dd/MM/yyyy", null).ToString("dd MMM yyyy") : "";
        }

        public static DateTime? FormatNullableDate(string date)
        {
            return !string.IsNullOrEmpty(date) ? DateTime.ParseExact(date, "dd/MM/yyyy", null) : (DateTime?)null;
        }
        
        public static DataTable ConvertExcelltoDataTable(string path, string fileName)
        {
            var connString = "";
            var filePath = path + fileName;
            var extension = fileName.ToLower().Split('.')[1];
            if (extension.Trim() == "xls")
            {
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (extension.Trim() == "xlsx")
            {
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }
            var oledbConn = new OleDbConnection(connString);
            var data = new DataTable();
            try
            {

                oledbConn.Open();
                using (var cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn))
                {
                    var oleda = new OleDbDataAdapter { SelectCommand = cmd };
                    var ds = new DataSet();
                    oleda.Fill(ds);
                    data = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                oledbConn.Close();
            }
            return data;
        }
        public static bool CheckSoftwareExpiration()
        {
            var isExpired = 0;
            try
            {
                var sqlcon = ConfigurationManager.ConnectionStrings["ERPDbContext"].ConnectionString;
                var sqlConnection = new SqlConnection(sqlcon);
                sqlConnection.Open();
                var sql = "OPEN SYMMETRIC KEY ERP_SYMMETRIC_KEY DECRYPTION BY CERTIFICATE ERP_CERTIFICATE";
                sql +=
                    " IF CONVERT(DATE,GETDATE(),106)>CONVERT(DATE,CAST(DecryptByKey((SELECT TOP 1 SoftwareExpirationKey FROM Organization)) AS VARCHAR(MAX)),106)";
                sql += " SELECT 1";
                sql += " ELSE";
                sql += " SELECT 0";
                var sqlCommand = new SqlCommand(sql, sqlConnection);
                var dataReader = sqlCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    isExpired = int.Parse(dataReader[0].ToString());
                }
                sqlConnection.Close();
            }
            catch (Exception exception)
            {
                isExpired = 1;
            }
            return isExpired == 0;
        }
        public static Result SendEmail(string emailAddress, string subject, string body,Dictionary<string, Stream> attachmentFilesStream = null, string bccEmailAddress = null,string ccEmailAddress = null, Dictionary<string, string> attachmentFilesLocation = null)
        {
            var sender =
                new
                {
                    EmailAddress = SessionHelper.OrgEmail,
                    Password = SessionHelper.OrgEmailPassword,
                    DisplayName = SessionHelper.OrganizationName,
                    Host = SessionHelper.OrganizationSmtpServerName,
                    Port = SessionHelper.EmailServerPort,
                    EnableSSL = SessionHelper.EnableSSL
                };
            var bodyHtml = GetEmailBody(body);
            var email =
                new
                {
                    EmailAddress = emailAddress,
                    CC = ccEmailAddress,
                    BCC = bccEmailAddress,
                    Subject = subject,
                    Body = bodyHtml
                };
            using (var mail = new MailMessage
            {
                From = new MailAddress(sender.EmailAddress, sender.DisplayName),
                Subject = email.Subject,
                Body = email.Body,
                //BodyEncoding = Encoding.GetEncoding(1252),
                IsBodyHtml = true
            })
            {
                //mail.AlternateViews.Add(htmlView);
                mail.To.Add(email.EmailAddress);
                if (!string.IsNullOrEmpty(email.CC))
                {
                    mail.CC.Add(email.CC);
                }
                if (!string.IsNullOrEmpty(email.BCC))
                {
                    mail.Bcc.Add(email.BCC);
                }
                if ((attachmentFilesLocation != null && attachmentFilesLocation.Count > 0) ||
                    (attachmentFilesStream != null && attachmentFilesStream.Count > 0))
                {
                    if (attachmentFilesLocation != null && attachmentFilesLocation.Count > 0)
                    {
                        foreach (var aFile in attachmentFilesLocation)
                        {
                            var obj = new System.Net.Mime.ContentType { Name = aFile.Key };
                            var attachment = new Attachment(aFile.Value, obj);
                            mail.Attachments.Add(attachment);
                        }
                    }
                    else if (attachmentFilesStream != null && attachmentFilesStream.Count > 0)
                    {
                        foreach (var aFile in attachmentFilesStream)
                        {
                            var obj = new System.Net.Mime.ContentType { Name = aFile.Key };
                            var attachment = new Attachment(aFile.Value, obj);
                            mail.Attachments.Add(attachment);
                        }
                    }
                }

                using (var smtp = new SmtpClient
                {
                    Host = sender.Host,
                    EnableSsl = sender.EnableSSL == 1,
                    Credentials = new NetworkCredential(sender.EmailAddress, sender.Password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = int.MaxValue,
                    Port = sender.Port
                })
                {
                    //bdlamps
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                    //                  | SecurityProtocolType.Tls11
                    //                  | SecurityProtocolType.Tls12;
                    //smtp.UseDefaultCredentials = false;
                    smtp.Send(mail);
                }
            }
            return new Result()
            {
                Message = body,
                Status = true
            };
        }
        public static void EmailReport(
            string email, string subject, string body, object data
            , string exportType, string reportName, string exportFileName
            , Dictionary<string, object> reportParameterCollection = null
            , Dictionary<int, DataTable> reportSubreportCollection = null
            )
        {
            var arrReferer = HttpContext.Current.Request.UrlReferrer.AbsolutePath.Split('/');
            var path = "~/";
            var areas = SessionHelper.Areas;
            if (areas.Contains(arrReferer[1]))
            {
                path = "~/Areas/" + arrReferer[1] + "/";
            }
            var reportPath = HttpContext.Current.Server.MapPath(path + @"Reports/" + SessionHelper.OrganizationShortName + @"/" + reportName);

            var report = new ReportDocument();
            report.Load(reportPath);
            report.SetDataSource(data);

            if (reportSubreportCollection != null)
            {
                foreach (var subReportPair in reportSubreportCollection)
                {
                    report.Subreports[subReportPair.Key].SetDataSource(subReportPair.Value);
                }
            }
            if (reportParameterCollection != null)
            {
                foreach (var parameterPair in reportParameterCollection)
                {
                    report.SetParameterValue(parameterPair.Key, parameterPair.Value);
                }
            }
            var exportOption = ExportFormatType.PortableDocFormat;
            var ext = "pdf";
            switch (exportType.ToLower())
            {
                case "excel":
                    exportOption = ExportFormatType.Excel;
                    ext = "xls";
                    break;
                case "exceldata":
                    exportOption = ExportFormatType.ExcelRecord;
                    ext = "xls";
                    break;
                case "excelbook":
                    exportOption = ExportFormatType.ExcelWorkbook;
                    ext = "xlsx";
                    break;
                case "word":
                    exportOption = ExportFormatType.WordForWindows;
                    ext = "doc";
                    break;
                case "xml":
                    exportOption = ExportFormatType.Xml;
                    ext = "xml";
                    break;
            }

            report = SetReportHeader(report, reportName);

            using (var stream = report.ExportToStream(exportOption))
            {
                var dt = new Dictionary<string, Stream>();
                dt.Add(exportFileName + "." + ext, stream);
                var aDirectory = new DirectoryInfo(ConfigurationManager.AppSettings["EmailAttachment"]);
                foreach (var aFile in aDirectory.GetFiles())
                {
                    var str = new StreamReader(aFile.FullName);
                    dt.Add(aFile.Name, str.BaseStream);
                }
                SendEmail(email, subject, body, dt);
            }
            report.Dispose();
            report.Close();
        }
        public static bool IsFixedHeaderReport(string reportName)
        {
            var fixedHeaderReports = new[]
            {
               "rpt_BankAdvice_IncentiveEidEarnLeave.rpt","rpt_BankAdvice_Salary.rpt","rpt_Employee_Salary_Sheet.rpt","rpt_Proposed_Employee_Salary_IncrementList.rpt"
            };
            return fixedHeaderReports.Contains(reportName);
        }

        public static string GetEmailBody(int type = 2)
        {
            var html = "";
            if (type == 2)
            {
                html = File.ReadAllText(HttpContext.Current.Server.MapPath("~/EmailHTMLPage/emailBodyPortfolio.txt"));
            }
            else if (type == 1)
            {
                html = File.ReadAllText(HttpContext.Current.Server.MapPath("~/EmailHTMLPage/emailBodyTrade.txt"));
            }
            else if (type == 3)
            {
                html = File.ReadAllText(HttpContext.Current.Server.MapPath("~/EmailHTMLPage/emailBodyBOOpening.txt"));
            }
            return html.Replace('"', '\'');
        }


        public static string GetMd5Hash(string input)
        {
            var hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
            var stringBuilder = new StringBuilder();
            foreach (var bt in hash)
                stringBuilder.Append(bt.ToString("x2"));
            return stringBuilder.ToString();
        }
        public static string GetLocalIPAddress()
        {
            var ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"];
            if(string.IsNullOrEmpty(ipAddress))
                ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipAddress))
                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return ipAddress;
        }

        public static string AmountToWords(decimal amount)
        {
            string words = "";
            if (amount.ToString().Contains("."))
            {
                var arr = amount.ToString().Split('.');
                var firstNumber = int.Parse(arr[0]);
                var length = arr[1].Length;
                var secondNumber = int.Parse(arr[1]);
                if (firstNumber > 0)
                {
                    words += NumberToWords(firstNumber);
                }
                if (secondNumber > 0)
                {
                    if (length == 1)
                    {
                        secondNumber = int.Parse(secondNumber + "0");
                    }
                    if (!string.IsNullOrEmpty(words))
                        words += " and ";
                    words += "paisa " + NumberToWords(secondNumber);
                }
            }
            else
            {
                words = NumberToWords(int.Parse(amount.ToString()));
            }

            return words + " only";
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 10000000) > 0)
            {
                words += NumberToWords(number / 10000000) + " crore ";
                number %= 10000000;
            }

            if ((number / 100000) > 0)
            {
                words += NumberToWords(number / 100000) + " lac ";
                number %= 100000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }

            return words;
        }
        public static DataTable ListToDataTable<T>(IList<T> data) where T : class
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static List<ReportInformation> UserwiseReportsAccess(string ControllerName, string ActionName)
        {

            var sPService = new SPService();
            var param = new { USER_ID = SessionHelper.LoggedInUserId, ControllerName = ControllerName, ActionName = ActionName };

            var Reports = sPService.GetDataWithParameter(param, "GET_USER_ACCESSED_REPORT")
            .Tables[0].AsEnumerable().Select(x => new ReportInformation()
            {
                Id = x.Field<int>("Id"),
                ReportName = x.Field<string>("ReportName"),
                SerialNo = x.Field<int>("SerialNo")
            }).ToList();

            return Reports;
        }
        public static void GetDataFromDSE()
        {
            API.PostData(ConfigurationManager.AppSettings["API"] + "/api/DSE/News", "{}");
        }
        public static string GetEmailBody(string body)
        {
            var html = "<table>";
            html += "<tbody><tr>";
            html += "<td>";
            html += "<div>" + body.Replace(Environment.NewLine, "<br/>") + "</div>";
            html += "<br/>";
            html += "<div> <span style='font-weight:bold'>With Best Regards,</span></div>";
            html += "<div><span style='font-weight:bold'>"+SessionHelper.OrganizationName+"</span></div>";
            html += "</td>";
            html += "</tr>";
            html += "</tbody></table>";

            html += "<h4>Contact us</h4>";
            html += File.ReadAllText(HttpContext.Current.Server.MapPath("~/EmailHTMLPage/emailFooterAddress.txt"));
            return html.Replace('"', '\'');
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static string GetErrorMessageFromException(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex.Message;
        }
        public static string GetErrorMessage(this Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex.Message;
        }
    }
}