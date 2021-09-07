using System.Web.Mvc;
using Omu.AwesomeMvc;

namespace AwesomeMvcDemo.Areas.Area1.Controllers
{
    public class A1HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string name)
        {
            ViewData["result"] = name;
            return View();
        }
        
        public ActionResult GetPart()
        {
            return Content(this.RenderPartialView("AreaPart", new { Number = 1 }));
        }
    }
}