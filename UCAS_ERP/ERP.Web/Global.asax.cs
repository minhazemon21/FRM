using Common.Service.StoredProcedure;
using System;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ERP.Web.Helpers;
using Microsoft.Owin.Security;

namespace ERP.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801



    public class MvcApplication : HttpApplication
    {
        private const string JobCacheAction = "http://localhost:8088";
        private IAuthenticationManager _authnManager;

        public IAuthenticationManager AuthenticationManager
        {
            get { return _authnManager ?? (_authnManager = HttpContext.Current.GetOwinContext().Authentication); }
            set { _authnManager = value; }
        }
        protected void Application_Start()
        {
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            Bootstrapper.Run();
        }

        protected void Application_Error()
        {
            HttpContext httpContext = HttpContext.Current;
            if (httpContext != null)
            {
                RequestContext requestContext = ((MvcHandler)httpContext.CurrentHandler).RequestContext;
                if (!requestContext.HttpContext.Request.IsAjaxRequest())
                {
                    var exception = Server.GetLastError();

                    var httpException = exception as HttpException;

                    var message = "";
                    var code = 0;
                    if (httpException != null)
                    {
                        code = httpException.GetHttpCode();
                        switch (httpException.GetHttpCode())
                        {
                            case 404:
                                message = "Sorry, the resource you requested could not be found.";
                                break;
                            default:
                                message = exception.GetErrorMessage();
                                break;
                        }
                    }
                    if (string.IsNullOrEmpty(message))
                    {
                        message = exception.GetErrorMessage();
                    }
                    SessionHelper.ErrorMessage = message;
                    SessionHelper.ErrorStatusCode = code.ToString();
                    httpContext.Response.Redirect("/Home/Error");
                }
            }
        }

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    var value = HttpContext.Current.Cache["remoteIp"];

        //    if (HttpContext.Current.Cache["remoteIp"] == null)
        //    {
        //        UpdateCountFor("remoteIp");
        //    }
        //}

        //public class IncrementingCacheCounter
        //{
        //    public int Count;
        //    public DateTime ExpireDate;
        //}

        //public void UpdateCountFor(string remoteIp)
        //{
        //    IncrementingCacheCounter counter = null;
        //    if (HttpContext.Current.Cache[remoteIp] == null)
        //    {
        //        var expireDate = DateTime.Now.AddMinutes(1);
        //        counter = new IncrementingCacheCounter { Count = 5, ExpireDate = expireDate };
        //    }
        //    else
        //    {
        //        counter = (IncrementingCacheCounter)HttpContext.Current.Cache[remoteIp];
        //        counter.Count++;
        //    }

        //    //HttpContext.Current.Cache.Insert(remoteIp, counter, null, counter.ExpireDate,
        //    //    Cache.NoSlidingExpiration, CacheItemPriority.Default, JobCacheRemoved);

        //    HttpContext.Current.Cache.Add(remoteIp, counter, null,
        //                    DateTime.MaxValue, TimeSpan.FromMinutes(1),
        //                    CacheItemPriority.Normal,
        //                    new CacheItemRemovedCallback(JobCacheRemoved));

        //    //HttpContext.Current.Cache.Insert(remoteIp, counter, null, counter.ExpireDate,
        //    //    Cache.NoSlidingExpiration, CacheItemPriority.Default,
        //    //    new CacheItemRemovedCallback(JobCacheRemoved));

        //}

        //private static void JobCacheRemoved(string key, object value, CacheItemRemovedReason reason)
        //{
        //    var client = new WebClient();

        //    ScheduledJob();
        //}

        //private static void ScheduledJob()
        //{
        //    SPService spService = new SPService();
        //    spService.GetDataWithoutParameter("proc_import_data");
        //}
    }
}