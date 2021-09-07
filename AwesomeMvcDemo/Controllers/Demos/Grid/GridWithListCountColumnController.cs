using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using AwesomeMvcDemo.Models;
using AwesomeMvcDemo.Utils;
using Omu.AwesomeMvc;
using Omu.AwesomeMvc.DynamicQuery;

namespace AwesomeMvcDemo.Controllers.Demos.Grid
{
    public class GridWithListCountColumnController : Controller
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
                    o.MealsCount,
                    MealsStr = string.Join(", ", o.Meals.Select(m => m.Name))
                },
                OrderByFunc = (lunches, rules) =>
                {
                    lunches = Dlinq.OrderBy(lunches, rules, new Dictionary<string, LambdaExpression>
                    {
                        { "MealsCount", ExprHelper.GetCountLambda<Lunch>(o => o.Meals) }
                    });

                    return lunches;
                }
            }.Build());

            // setting OrderByFunc in this demo is actually unnecessary because we're not using Entity Framework
            // and we have Lunch.MealCount property that returns the count,
            // but EF won't be able to translate OrderBy MealCount into a sql query
        }
    }
}