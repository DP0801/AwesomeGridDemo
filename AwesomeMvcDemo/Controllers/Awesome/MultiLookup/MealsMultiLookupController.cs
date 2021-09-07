using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using AwesomeMvcDemo.Models;

using Omu.AwesomeMvc;

namespace AwesomeMvcDemo.Controllers.Awesome.MultiLookup
{
    /*begin*/
    public class MealsMultiLookupController : Controller
    {
        public ActionResult GetItems(int[] v)
        {
            var items = new List<Meal>();
            if (v != null)
            {
                items.AddRange(v.Select(Db.Get<Meal>));
            }

            return Json(items.Select(meal => new KeyContent(meal.Id, meal.Name)));
        }

        public ActionResult Search(string search, int[] selected, int page)
        {
            const int pageSize = 10;
            selected = selected ?? new int[] { };
            search = (search ?? "").ToLower().Trim();

            var items = Db.Meals.Where(o => o.Name.ToLower().Contains(search) && !selected.Contains(o.Id));

            return Json(new AjaxListResult
                            {
                                Items = items.Skip((page - 1) * pageSize).Take(pageSize).Select(o => new KeyContent(o.Id, o.Name)),
                                More = items.Count() > page * pageSize
                            });
        }

        public ActionResult Selected(int[] selected)
        {
            var items = new List<Meal>();
            if (selected != null)
            {
                items.AddRange(selected.Select(Db.Get<Meal>));
            }
            
            return Json(new AjaxListResult
                            {
                                Items = items.Select(o => new KeyContent(o.Id, o.Name))
                            });
        }
    }
    /*end*/
}