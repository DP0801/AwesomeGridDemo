using System.Linq;
using System.Web.Mvc;

using AwesomeMvcDemo.Models;

using Omu.AwesomeMvc;

namespace AwesomeMvcDemo.Controllers.Awesome.AjaxList
{
    public class MealsAjaxListController : Controller
    {
        public ActionResult Search(string parent, int? pivot)
        {
            const int PageSize = 5;
            var query = (parent ?? "").ToLower();

            var list = Db.Meals.Where(o => o.Name.ToLower().Contains(query));

            var items = (pivot.HasValue ? list.Where(o => o.Id >= pivot.Value) : list)
                .Take(PageSize + 1)
                .ToList();

            var isMore = items.Count > PageSize;

            var result = new AjaxListResult
            {
                Items = items.Take(PageSize).Select(o => new KeyContent(o.Id, o.Name)),
                More = isMore
            };

            if (isMore)
            {
                result.Pivot = items.Last().Id;
            }

            return Json(result);
        }
    }
}