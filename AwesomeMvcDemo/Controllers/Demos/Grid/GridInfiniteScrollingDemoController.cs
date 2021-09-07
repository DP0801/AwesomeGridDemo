using System;
using System.Linq;
using System.Web.Mvc;
using AwesomeMvcDemo.Models;
using Omu.AwesomeMvc;
using System.Linq.Dynamic.Core;

namespace AwesomeMvcDemo.Controllers.Demos.Grid
{
    public class GridInfiniteScrollingDemoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetItems(GridParams g)
        {
            var list = Db.Lunches.AsQueryable();

            var nr = 1;

            return Json(new GridModelBuilder<Lunch>(list, g)
            {
                KeyProp = o => o.Id,
                MakeHeader = MakeHeader,
                Map = o => new
                {
                    Nr = nr++ + (g.Spage - 1) * g.PageSize,
                    o.Id,
                    o.Person,
                    o.Food,
                    o.FoodPic,
                    o.Location,
                    o.Price,
                    Date = o.Date.ToShortDateString(),
                    CountryName = o.Country.Name,
                    ChefName = o.Chef.FirstName + " " + o.Chef.LastName
                }
            }.Build());

            // only needed if you have group aggregate values
            GroupHeader MakeHeader(GroupInfo<Lunch> info)
            {
                // get first item in the group
                var first = info.Items.First();

                // get the grouped column value(s) for the first item
                var val = string.Join(" ", AweUtil.GetColumnValue(info.Column, first).Select(ToStr));

                // info.Items has only the items on the current page, so the aggregate value (Count, Sum, etc) may be incorrect
                // here we're manually selecting the items for the current group without considering page
                var qitems = Db.Lunches.AsQueryable();

                foreach (var groupName in g.Groups)
                {
                    var names = groupName.Split(','); // call Split for groups like Chef column "Chef.FirstName,Chef.LastName"
                    var gvals = AweUtil.GetColumnValue(groupName, first).ToArray();

                    for (var i = 0; i < names.Length; i++)
                    {
                        qitems = qitems.Where(names[i] + "==@0", gvals[i]); // using Dynamic Linq
                    }

                    if (groupName == info.Column) break; // reached current group level
                }

                return new GroupHeader { Content = $" {info.Header} : {val} ( Count = {qitems.Count()} )" };
            }
        }

        private static string ToStr(object o)
        {
            return o is DateTime ? ((DateTime)o).ToShortDateString() : o.ToString();
        }
    }
}