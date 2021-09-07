using System.Linq;
using System.Web.Mvc;
using AwesomeMvcDemo.Models;
using Omu.AwesomeMvc;

namespace AwesomeMvcDemo.Controllers.Demos.Grid
{
    public class GanttChartController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GridData(GridParams g)
        {
            var data = Db.Meetings.AsQueryable();

            var gmb = new GridModelBuilder<Meeting>(g);

            data = gmb.OrderBy(data);

            var count = data.Count();
            var page = gmb.GetPage(data).Select(o => new
            {
                o.Id,
                o.Title,
                o.Start,
                o.End,
                StartStr = o.Start.ToShortDateString() + " " + o.Start.ToShortTimeString(),
                EndStr = o.End.ToShortDateString() + " " + o.End.ToShortTimeString()
            });

            return Json(new { page, count });
        }
    }
}