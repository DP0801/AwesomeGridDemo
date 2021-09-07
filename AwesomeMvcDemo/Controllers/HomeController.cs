using System.Web.Mvc;

namespace AwesomeMvcDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult Ptest()
        {
            return View();
        }

        public ActionResult Social()
        {
            return PartialView();
        }
    }
}