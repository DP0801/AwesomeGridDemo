using System.Web.Mvc;

namespace AwesomeMvcDemo.Controllers.Demos.Helpers
{
    public class DropmenuController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Popup1()
        {
            return PartialView();
        }
    }
}