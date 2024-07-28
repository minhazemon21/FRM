//using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Common.Data.CommonDataModel;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ERP.Web.ViewModels;

namespace ERP.Web.Helpers
{
    public class SessionHelper
    {
        public static bool IsAuthenticated
        {
            get { return HttpContext.Current.Request.IsAuthenticated; }
        }
        public static void LogSessionInfo(List<AspNetSecurityModule> parentModules, List<AspNetSecurityModule> parentSecondModules, List<AspNetSecurityModule> userSecurityModules, List<ReportInformation> userReportModules, List<UcasSoftware_Projects> _Projects)
        {
            if (HttpContext.Current.Session != null)
            {
                // HttpContext.Current.Session[SessionKeys.LOGGEDIN_EMPLOYEE] = user;
                HttpContext.Current.Session[SessionKeys.PARENT_MODULES] = parentModules;
                HttpContext.Current.Session[SessionKeys.SecondPARENT_MODULES] = parentSecondModules;
                HttpContext.Current.Session[SessionKeys.USER_SECURITY_MODULES] = userSecurityModules;
                HttpContext.Current.Session[SessionKeys.USER_REPORT_MODULES] = userReportModules;
                HttpContext.Current.Session[SessionKeys.USER_Projects] = _Projects;
            }
        }
      
        public static void UserSessionInfoAfterLogin(AspNetUser user)
        {
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[SessionKeys.LOGGEDIN_EMPLOYEE] = user;
            }
        }
        //SecondPARENT_MODULES
        public static List<AspNetSecurityModule> AllPrentModules
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeys.PARENT_MODULES] != null)
                    return HttpContext.Current.Session[SessionKeys.PARENT_MODULES] as List<AspNetSecurityModule>;
                else
                    return null;
            }
        }
        public static List<AspNetSecurityModule> SecondPrentModules
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeys.SecondPARENT_MODULES] != null)
                    return HttpContext.Current.Session[SessionKeys.SecondPARENT_MODULES] as List<AspNetSecurityModule>;
                else
                    return null;
            }
        }
        public static List<AspNetSecurityModule> UserSecurityModules
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeys.USER_SECURITY_MODULES] != null)
                    return HttpContext.Current.Session[SessionKeys.USER_SECURITY_MODULES] as List<AspNetSecurityModule>;
                else
                    return null;
            }
        }
        public static List<ReportInformation> UserReportModules
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeys.USER_REPORT_MODULES] != null)
                    return HttpContext.Current.Session[SessionKeys.USER_REPORT_MODULES] as List<ReportInformation>;
                else
                    return null;
            }
        }
        public static List<UcasSoftware_Projects> Projects
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeys.USER_Projects] != null)
                    return HttpContext.Current.Session[SessionKeys.USER_Projects] as List<UcasSoftware_Projects>;
                else
                    return null;
            }
        }


        public static void UserprojectPermission(List<UcasSoftware_Projects> _Projects)
        {
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[SessionKeys.ddl_USER_Projects] = _Projects;
            }
        }


        public static List<UcasSoftware_Projects> ddlProjects
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeys.ddl_USER_Projects] != null)
                    return HttpContext.Current.Session[SessionKeys.ddl_USER_Projects] as List<UcasSoftware_Projects>;
                else
                    return null;
            }
        }
        public static string UserName
        {
            get { return HttpContext.Current.Session[SessionKeys.UserName].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.UserName] = value; }
        }
        public static string Password
        {
            get { return HttpContext.Current.Session[SessionKeys.Password].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.Password] = value; }
        }
        public static int LoggedInUserId
        {
            get { return HttpContext.Current.Session[SessionKeys.USER_ID] == null ? default(Int32) : Convert.ToInt32(HttpContext.Current.Session[SessionKeys.USER_ID]); }
            set { HttpContext.Current.Session[SessionKeys.USER_ID] = value; }
        }
        public static int LoggedInUserId_Hrm
        {
            get { return HttpContext.Current.Session[SessionKeys.USER_ID_HRM] == null ? default(Int32) : Convert.ToInt32(HttpContext.Current.Session[SessionKeys.USER_ID_HRM]); }
            set { HttpContext.Current.Session[SessionKeys.USER_ID_HRM] = value; }
        }
        public static string EmployeeCode
        {
            get { return HttpContext.Current.Session[SessionKeys.EmployeeCode].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.EmployeeCode] = value; }
        }
        public static string LoggedInUserFullName
        {
            get { return HttpContext.Current.Session[SessionKeys.USER_FULL_NAME] == null ? string.Empty : HttpContext.Current.Session[SessionKeys.USER_FULL_NAME].ToString(); }
            //get { return "Testing"; }
            set { HttpContext.Current.Session[SessionKeys.USER_FULL_NAME] = value; }
        }

        public static ReportDocument Report
        {
            get { return (ReportDocument)HttpContext.Current.Session[SessionKeys.Report]; }
            //get { return "Testing"; }
            set { HttpContext.Current.Session[SessionKeys.Report] = value; }
        }
        public static string ReportExportName
        {
            get { return (string)HttpContext.Current.Session[SessionKeys.ReportExportName]; }
            //get { return "Testing"; }
            set { HttpContext.Current.Session[SessionKeys.ReportExportName] = value; }
        }

        public static List<ReportHeader> ReportHeader
        {
            get { return (List<ReportHeader>)HttpContext.Current.Session[SessionKeys.ReportHeader]; }
            //get { return "Testing"; }
            set { HttpContext.Current.Session[SessionKeys.ReportHeader] = value; }
        }

        public static ExportFormatType ReportFormat
        {
            get { return (ExportFormatType)HttpContext.Current.Session[SessionKeys.ReportFormat]; }
            //get { return "Testing"; }
            set { HttpContext.Current.Session[SessionKeys.ReportFormat] = value; }
        }
        public static long LoggedInPersonId
        {
            get { return HttpContext.Current.Session[SessionKeys.PERSON_ID] == null ? 0 : Convert.ToInt64(HttpContext.Current.Session[SessionKeys.PERSON_ID]); }
            //get { return 1; }
            set { HttpContext.Current.Session[SessionKeys.PERSON_ID] = value; }
        }
        public static string LoggedInPersonType
        {
            get { return HttpContext.Current.Session[SessionKeys.PERSON_TYPE] == null ? "" : HttpContext.Current.Session[SessionKeys.PERSON_TYPE].ToString(); }
            //get { return "Staff"; }
            set { HttpContext.Current.Session[SessionKeys.PERSON_TYPE] = value; }
        }
        public static int LoggedInOfficeId
        {
            get { return HttpContext.Current.Session[SessionKeys.Office_ID].ToString() == null ? 0 : int.Parse(HttpContext.Current.Session[SessionKeys.Office_ID].ToString()); }
            set { HttpContext.Current.Session[SessionKeys.Office_ID] = value; }
        }
        public static string LoggedInOfficeName
        {
            get { return HttpContext.Current.Session[SessionKeys.Office_Name] == null ? "" : HttpContext.Current.Session[SessionKeys.Office_Name].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.Office_Name] = value; }
        }

        public static string OrganizationSmtpServerName
        {
            get { return HttpContext.Current.Session[SessionKeys.OrganizationSmtpServerName] == null ? "" : HttpContext.Current.Session[SessionKeys.OrganizationSmtpServerName].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.OrganizationSmtpServerName] = value; }
        }

        public static int LoggedIn_RoleId
        {
            get { return HttpContext.Current.Session[SessionKeys.Role_Id] == null ? 0 : int.Parse(HttpContext.Current.Session[SessionKeys.Role_Id].ToString()); }
            set { HttpContext.Current.Session[SessionKeys.Role_Id] = value; }
        }
        public static int IsAdmin
        {
            get { return HttpContext.Current.Session[SessionKeys.IsAdmin] == null ? 0 : int.Parse(HttpContext.Current.Session[SessionKeys.IsAdmin].ToString()); }
            set { HttpContext.Current.Session[SessionKeys.IsAdmin] = value; }
        }
        public static int UserNameType
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeys.UserNameType] != null)
                    return int.Parse(HttpContext.Current.Session[SessionKeys.UserNameType].ToString());
                return 1;
            }
            set { HttpContext.Current.Session[SessionKeys.UserNameType] = value; }
        }
        public static string OrganizationName
        {
            get { return HttpContext.Current.Session[SessionKeys.ORGANIZATION_NAME] == null ? "" : HttpContext.Current.Session[SessionKeys.ORGANIZATION_NAME].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.ORGANIZATION_NAME] = value; }
        }
        public static string OrganizationShortName
        {
            get { return HttpContext.Current.Session[SessionKeys.ORGANIZATION_SHORT_NAME] == null ? "" : HttpContext.Current.Session[SessionKeys.ORGANIZATION_SHORT_NAME].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.ORGANIZATION_SHORT_NAME] = value; }
        }
        public static string RoleName
        {
            get { return HttpContext.Current.Session[SessionKeys.Role_Name] == null ? "" : HttpContext.Current.Session[SessionKeys.Role_Name].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.Role_Name] = value; }
        }
        public static string OrganizationDealerCode
        {
            get { return HttpContext.Current.Session[SessionKeys.ORGANIZATION_DEALER_CODE] == null ? "" : HttpContext.Current.Session[SessionKeys.ORGANIZATION_DEALER_CODE].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.ORGANIZATION_DEALER_CODE] = value; }
        }
        public static string OrganizationLogoPath
        {
            get { return HttpContext.Current.Session[SessionKeys.ORGANIZATION_LOGO_PATH] == null ? "" : HttpContext.Current.Session[SessionKeys.ORGANIZATION_LOGO_PATH].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.ORGANIZATION_LOGO_PATH] = value; }
        }

        public static string OrganizationLogo
        {
            get { return HttpContext.Current.Session[SessionKeys.ORGANIZATION_LOGO] == null ? "" : HttpContext.Current.Session[SessionKeys.ORGANIZATION_LOGO].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.ORGANIZATION_LOGO] = value; }
        }
        public static string OrganizationBuySaleOrderLogo
        {
            get { return HttpContext.Current.Session[SessionKeys.ORGANIZATION_BUY_SALE_ORDER_LOGO] == null ? "" : HttpContext.Current.Session[SessionKeys.ORGANIZATION_BUY_SALE_ORDER_LOGO].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.ORGANIZATION_BUY_SALE_ORDER_LOGO] = value; }
        }

        public static string OrgEmail
        {
            get { return HttpContext.Current.Session[SessionKeys.ORG_EMAIL] == null ? "" : HttpContext.Current.Session[SessionKeys.ORG_EMAIL].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.ORG_EMAIL] = value; }
        }
        public static string OrgEmailPassword
        {
            get { return HttpContext.Current.Session[SessionKeys.ORG_EMAIL_PASSWORD] == null ? "" : HttpContext.Current.Session[SessionKeys.ORG_EMAIL_PASSWORD].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.ORG_EMAIL_PASSWORD] = value; }
        }
        public static string SMSMobileNo
        {
            get { return HttpContext.Current.Session[SessionKeys.SMS_MOBILE_NO] == null ? "" : HttpContext.Current.Session[SessionKeys.SMS_MOBILE_NO].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.SMS_MOBILE_NO] = value; }
        }
        public static string SMSUserName
        {
            get { return HttpContext.Current.Session[SessionKeys.SMS_USER_NAME] == null ? "" : HttpContext.Current.Session[SessionKeys.SMS_USER_NAME].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.SMS_USER_NAME] = value; }
        }

        public static string OrganizationAddress
        {
            get { return HttpContext.Current.Session[SessionKeys.ORGANIZATION_Address] == null ? "" : HttpContext.Current.Session[SessionKeys.ORGANIZATION_Address].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.ORGANIZATION_Address] = value; }
        }
        public static string TransactionDay
        {
            get { return HttpContext.Current.Session[SessionKeys.TRANSACTION_DAY] == null ? "" : HttpContext.Current.Session[SessionKeys.TRANSACTION_DAY].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.TRANSACTION_DAY] = value; }
        }
        public static int EmailServerPort
        {
            get { return HttpContext.Current.Session[SessionKeys.EmailServerPort] == null ? 0 : int.Parse(HttpContext.Current.Session[SessionKeys.EmailServerPort].ToString()); }
            set { HttpContext.Current.Session[SessionKeys.EmailServerPort] = value; }
        }
        public static int EnableSSL
        {
            get { return HttpContext.Current.Session[SessionKeys.EnableSSL] == null ? 0 : int.Parse(HttpContext.Current.Session[SessionKeys.EnableSSL].ToString()); }
            set { HttpContext.Current.Session[SessionKeys.EnableSSL] = value; }
        }
        public static List<string> Areas
        {
            get { return HttpContext.Current.Session[SessionKeys.AREAS] == null ? new List<string>() : (List<string>)HttpContext.Current.Session[SessionKeys.AREAS]; }
            set { HttpContext.Current.Session[SessionKeys.AREAS] = value; }
        }
        public static int CurrentReportId
        {
            get { return HttpContext.Current.Session[SessionKeys.CurrentReportId] == null ? 0 : int.Parse(HttpContext.Current.Session[SessionKeys.CurrentReportId].ToString()); }
            set { HttpContext.Current.Session[SessionKeys.CurrentReportId] = value; }
        }
        public static string ErrorMessage
        {
            get { return HttpContext.Current.Session[SessionKeys.ErrorMessage] == null ? "" : HttpContext.Current.Session[SessionKeys.ErrorMessage].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.ErrorMessage] = value; }
        }
        public static string ErrorStatusCode
        {
            get { return HttpContext.Current.Session[SessionKeys.ErrorStatusCode] == null ? "" : HttpContext.Current.Session[SessionKeys.ErrorStatusCode].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.ErrorStatusCode] = value; }
        }
    }
    public class SessionKeys
    {
        public const string LOGGEDIN_EMPLOYEE = "LOGGEDIN_EMPLOYEE";
        public const string ORG_EMAIL = "ORG_EMAIL";
        public const string ORG_EMAIL_PASSWORD = "ORG_EMAIL_PASSWORD";
        public const string USER_REPORT_MODULES = "USER_REPORT_MODULES";
        public const string USER_Projects = "USER_Projects";
        public const string ddl_USER_Projects = "ddl_USER_Projects";
       // public const string TRANSACTION_DATE = "TRANSACTION_DATE";
        public const string TRANSACTION_DAY = "TRANSACTION_DAY";
        public const string IS_DAY_INITIATED = "IS_DAY_INITIATED";
        public const string USER_FULL_NAME = "USER_FULL_NAME";
        public const string UserName = "UserName";//Email 
        public const string Password = "Password";
        public const string PROCESS_TYPE = "PROCESS_TYPE";
        public const string PARENT_MODULES = "PARENT_MODULES";
        public const string SecondPARENT_MODULES = "SecondPARENT_MODULES";
        public const string USER_SECURITY_MODULES = "ROLE_MODULES";
        public const string USER_ID_HRM = "USER_ID_HRM";
        public const string EmployeeCode = "EmployeeCode";
        public const string LOGGED_IN_OFFICE_DETAIL = "LOGGED_IN_OFFICE_DETAIL";
        public const string USER_ID = "USER_ID";
        public const string PERSON_ID = "PERSON_ID";
        public const string PERSON_TYPE = "PERSON_TYPE";
        public const string ORGANIZATION_NAME = "ORGANIZATION_NAME";
        public const string ORGANIZATION_SHORT_NAME = "ORGANIZATION_SHORT_NAME";
        public const string SMS_PASSWORD = "SMS_PASSWORD";
        public const string SMS_MOBILE_NO = "SMS_MOBILE_NO";
        public const string SMS_USER_NAME = "SMS_USER_NAME";
        public const string ORGANIZATION_Address = "ORGANIZATION_Address";
        public const string Office_ID = "Office_ID";
        public const string Office_Name = "Office_Name";
        public const string Role_Id = "Role_Id";
        public const string IsAdmin = "IsAdmin";
        public const string Role_Name = "Role_Name";
        public const string ORGANIZATION_DEALER_CODE = "ORGANIZATION_DEALER_CODE";
        public const string ORGANIZATION_LOGO_PATH = "ORGANIZATION_LOGO_PATH";
        public const string ORGANIZATION_LOGO = "ORGANIZATION_LOGO";
        public const string Report = "Report";
        public const string ReportFormat = "ReportFormat";
        public const string ReportExportName = "ReportExportName";
        public const string ReportHeader = "ReportHeader";
        public const string ORGANIZATION_BUY_SALE_ORDER_LOGO = "ORGANIZATION_BUY_SALE_ORDER_LOGO";
        public const string AREAS = "AREAS";
        public const string OrganizationSmtpServerName = "OrganizationSmtpServerName";
        public const string EmailServerPort = "EmailServerPort";
        public const string EnableSSL = "EnableSSL";
        public const string CurrentReportId = "CurrentReportId";
        public const string ErrorMessage = "ErrorMessage";
        public const string ErrorStatusCode = "ErrorStatusCode";
        public const string UserNameType = "UserNameType";
    }
}