using System.Web.Mvc;
using AwesomeMvcDemo.ViewModels.Input;

namespace AwesomeMvcDemo.Controllers.Demos.Helpers
{
    public class TextBoxDemoController : Controller
    {
        public ActionResult Index()
        {
            return View(new TextBoxDemoInput());
        }
    }
}