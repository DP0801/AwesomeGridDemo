using System;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using AwesomeMvcDemo.App_Start;
using AwesomeMvcDemo.Controllers;
using AwesomeMvcDemo.Utils;
using Omu.Awem.Helpers;
using Omu.AwesomeMvc;

namespace AwesomeMvcDemo
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //FilterProviders.Providers.Add(new AntiForgeryTokenFilter());
            

            //GlobalFilters.Filters.Add(new JsonAllowGetAttribute());

            //new Worker().Start();

            //Settings.CheckboxModFunc = box => box.Ochk();
            log4net.Config.XmlConfigurator.Configure();
        }


        protected void Application_Error(object sender, EventArgs e)
        {
            //return;
            var exception = Server.GetLastError();
            // Log the exception.
            Response.Clear();

            HttpContext.Current.Response.TrySkipIisCustomErrors = true;

            var httpException = exception as HttpException;

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");

            if (httpException == null)
            {
                routeData.Values.Add("action", "Index");
            }
            else //It's an Http Exception, Let's handle it.
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // Page not found.
                        routeData.Values.Add("action", "HttpError404");
                        break;
                    case 505:
                        // Server error.
                        routeData.Values.Add("action", "HttpError505");
                        break;

                    // Here you can handle Views to other error codes.
                    // I choose a General error template  
                    default:
                        routeData.Values.Add("action", "Index");
                        break;
                }
            }

            // Pass exception details to the target error View.
            routeData.Values.Add("error", exception);

            // Clear the error on server.
            Server.ClearError();

            try
            {
                // Call target Controller and pass the routeData.
                IController errorController = new ErrorController();
                errorController.Execute(new RequestContext(
                    new HttpContextWrapper(Context), routeData));
            }
            catch (Exception)
            {
                var rd = new RouteData();
                rd.Values.Add("controller", "Error");
                rd.Values.Add("action", "Master");
                rd.Values.Add("error", exception);

                IController errorController = new ErrorController();
                errorController.Execute(new RequestContext(
                    new HttpContextWrapper(Context), rd));
            }
        }
    }
}