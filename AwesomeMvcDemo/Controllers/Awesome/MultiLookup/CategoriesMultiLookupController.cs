using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using AwesomeMvcDemo.Models;

using Omu.AwesomeMvc;

namespace AwesomeMvcDemo.Controllers.Awesome.MultiLookup
{
    public class CategoriesMultiLookupController : Controller
    {
        public ActionResult GetItems(IEnumerable<int> v)
        {
            return Json(Db.Categories.Where(o => v != null && v.Contains(o.Id))
                          .Select(f => new KeyContent(f.Id, f.Name)));
        }

        public ActionResult Search(string search, int[] selected)
        {
            search = (search ?? "").ToLower();
            selected = selected ?? new int[] { };

            return Json(new AjaxListResult
            {
                Items = Db.Categories.Where(o => o.Name.ToLower().Contains(search) && !selected.Contains(o.Id))
                              .Select(o => new KeyContent(o.Id, o.Name))
            });
        }

        public ActionResult Selected(int[] selected)
        {
            selected = selected ?? new int[] { };
            return Json(new AjaxListResult
            {
                Items = Db.Categories.Where(o => selected.Contains(o.Id))
                                  .Select(o => new KeyContent(o.Id, o.Name))
            });

        }
    }
}