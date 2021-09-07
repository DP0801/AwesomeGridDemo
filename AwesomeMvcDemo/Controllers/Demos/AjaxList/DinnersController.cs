using System.Web.Mvc;

namespace AwesomeMvcDemo.Controllers.Demos.AjaxList
{
    public class DinnersController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Crud","AjaxListDemo");
        }
    }
}