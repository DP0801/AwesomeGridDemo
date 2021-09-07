using System.Web.Mvc;

using AwesomeMvcDemo.ViewModels.Input;

namespace AwesomeMvcDemo.Controllers.Demos.Helpers
{
    public class MultiLookupDemoController : Controller
    {
        public ActionResult Index()
        {
            return View(new MultiLookupDemoCfgInput
            {
                HighlightChange = true,
                DragAndDrop = true
            });
        }

        [HttpPost]
        public ActionResult IndexContent(MultiLookupDemoCfgInput input)
        {
            return PartialView(input);
        }

        public ActionResult CustomSearch()
        {
            return View();
        }

        public ActionResult Misc()
        {
            return View();
        }

        public ActionResult MealMultiLookup()
        {
            return PartialView();
        }
    }
}