using System.Web.Mvc;

namespace AwesomeMvcDemo.Controllers.Demos.Generic
{
    public class DisabledController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}