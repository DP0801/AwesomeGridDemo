using System.Web.Mvc;

using AwesomeMvcDemo.ViewModels.Input;

namespace AwesomeMvcDemo.Controllers.Demos.Generic
{
    public class UnobtrusiveController : Controller
    {
        public ActionResult Index()
        {
            return View(new UnobtrusiveInput());
        }

        [HttpPost]
        public ActionResult Index(UnobtrusiveInput input)
        {
            return View(input);
        }
    }
}