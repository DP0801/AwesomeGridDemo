using System;
using System.Web.Mvc;
using AwesomeMvcDemo.Models;
using AwesomeMvcDemo.Utils;

namespace AwesomeMvcDemo.Controllers
{
    public class ErrorController : Controller
    {
        [IgnoreAntiforgeryToken]
        public ActionResult Index(Exception error)
        {
            SetMessage(error);

            if (error.Message != null && error.Message.Contains("The parameters dictionary contains") ||
                error is AweArgumentNullException ||
                error is EntityMissingException)
            {
                Response.StatusCode = 400;

                if (Request.IsAjaxRequest())
                {
                    return PartialView("Resource400");
                }

                return View("Resource400");
            }

            if (error is AwesomeDemoException)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Expected");
                }

                return View("Expected");
            }

            if (Response.StatusCode == 200)
            {
                Response.StatusCode = 500;
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("ErrorPartial");
            }

            return View("Error");
        }

        [IgnoreAntiforgeryToken]
        public ActionResult Master(Exception error)
        {
            SetMessage(error);

            return View();
        }

        public ActionResult HttpError404(Exception error)
        {
            Response.StatusCode = 404;

            if (Request.IsAjaxRequest())
            {
                return Content("404 not found");
            }

            return View();
        }


        public ActionResult HttpError400(Exception error)
        {
            Response.StatusCode = 400;

            return Content("400 bad request");
        }

        public ActionResult HttpError505(Exception error)
        {
            return View();
        }

        private void SetMessage(Exception error)
        {
            var isDev = Autil.IsDev();

            if (error is AwesomeDemoException)
            {
                ViewData["message"] = Message(error);
            }
            else if (isDev)
            {
                ViewData["debugInfo"] = "This message is showing because there is <code>compilation debug=\"true\"</code> in web.config";
                ViewData["message"] = error.ToString();
            }
            else
            {
                ViewData["debugInfo"] = "Set <code>compilation debug=\"true\"</code> in web.config to get more details";
                ViewData["message"] = Message(error);
            }
        }

        private string Message(Exception ex)
        {
            if (ex == null) return "";
            return ex.Message + "\n" + Message(ex.InnerException);
        }
    }
}