using System.Web.Mvc;

using AwesomeMvcDemo.ViewModels.Input;

namespace AwesomeMvcDemo.Controllers.Demos.Helpers
{
    public class PopupFormDemoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /*begin*/
        public ActionResult PopupWithParameters(int? id, string parent, int p1, string a, string b)
        {
            ViewData["parent"] = parent;
            ViewData["p1"] = p1;
            ViewData["a"] = a;
            ViewData["b"] = b;
            ViewData["id"] = id;
            return PartialView();
        }

        [HttpPost]
        public ActionResult PopupWithParameters()
        {
            return Json(new { });
        }
        /*end*/

        public ActionResult PopupConfirm()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult PopupConfirm(PopupConfirmInput input)
        {
            return Json(new { input.Name });
        }
    }
}