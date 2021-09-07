using System.Linq;
using System.Web.Mvc;
using AwesomeMvcDemo.Models;
using Omu.AwesomeMvc;

namespace AwesomeMvcDemo.Controllers.Demos.Grid
{
    public class GridFrozenColumnsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetItems(GridParams g)
        {   
            var list = Db.Lunches.AsQueryable();

            return Json(new GridModelBuilder<Lunch>(list, g)
            {
                KeyProp = o => o.Id,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.Id,
                    o.Person,
                    o.Food,
                    o.FoodPic,
                    o.Location,
                    o.Price,
                    Date = o.Date.ToShortDateString(),
                    CountryName = o.Country.Name,
                    ChefName = o.Chef.FirstName + " " + o.Chef.LastName,
                    Meals = string.Join(", ", o.Meals.Select(m => m.Name)),
                    Organic = o.Organic ? "Yes" : "No",
                }
            }.Build());
        }
    }
}