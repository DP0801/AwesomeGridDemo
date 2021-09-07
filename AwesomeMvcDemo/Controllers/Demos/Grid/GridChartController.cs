using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;

namespace AwesomeMvcDemo.Controllers.Demos.Grid
{
    public class GridChartController : Controller
    {
        public ActionResult Pie()
        {
            return View();
        }

        private static readonly IList<ChartItem> items;

        static GridChartController()
        {
            var names = new[] { "Cherry trees", "Apple trees", "Apricot and other trees", "Swiss chard", "Parris island cos lettuce", "Tomatoes", "Grapes", "Flowers", "Raspberry", "Other berries" };
            var id = 1;

            var rnd = new Random();
            var val = 100;

            items = new List<ChartItem>();
            var len = names.Length;
            var part = val / len;
            foreach (var name in names)
            {
                var v = rnd.Next((int) (part * 0.7), part);
                items.Add(new ChartItem { Name = name, Value = v, Id = id++ });
                val -= v;
            }

            items.Add(new ChartItem { Name = "Weeds", Value = val, Id = id });
        }

        public ActionResult GetData(GridParams g, string search)
        {
            var gmb = new GridModelBuilder<ChartItem>(g);
            var query = items.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(o => o.Name.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0);
            }

            var data = gmb.OrderBy(query);
                
            return Json(data);
        }


        public class ChartItem
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public float Value { get; set; }
        }
    }
}