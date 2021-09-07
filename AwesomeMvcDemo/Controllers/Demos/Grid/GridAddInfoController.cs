using System.Linq;
using System.Web.Mvc;
using AwesomeMvcDemo.Models;
using Omu.AwesomeMvc;

namespace AwesomeMvcDemo.Controllers.Demos.Grid
{
    public class GridAddInfoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetItems(GridParams g, string person)
        {
            person = (person ?? "").ToLower();

            var data = Db.Lunches.Where(o => o.Person.ToLower().Contains(person));

            var count = data.Count(); // Page = custom extension in our main demo

            var avgPrice = count > 0 ? data.Average(o => o.Price).ToString("N") : ""; 

            return Json(new GridModelBuilder<Lunch>(data.AsQueryable(), g)
            {
                Map = o => new
                {
                    o.Id,
                    o.Person,
                    o.Price,
                    o.Food
                },
                Tag = new { avgPrice, count }
            }.Build());
        }
    }
}