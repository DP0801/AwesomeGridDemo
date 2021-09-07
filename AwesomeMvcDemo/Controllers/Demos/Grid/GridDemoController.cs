using System;
using System.Linq;
using System.Web.Mvc;
using AwesomeMvcDemo.Models;
using AwesomeMvcDemo.ViewModels.Input;
using Omu.AwesomeMvc;

namespace AwesomeMvcDemo.Controllers.Demos.Grid
{
    public class GridDemoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Filtering()
        {
            return View();
        }

        public ActionResult Selection()
        {
            return View();
        }

        public ActionResult RTLSupport()
        {
            return View();
        }

        public ActionResult CustomFormat()
        {
            return View();
        }

        public ActionResult CustomQuerying()
        {
            return View();
        }

        public ActionResult ClientSideApi()
        {
            return View();
        }

        public ActionResult Sorting()
        {
            var o = new GridDemoSortingCfgInput
            {
                Sortable = true,
                PageSize = 15,
                PersonSortable = true,
                PersonSort = Sort.Asc,
                PersonRank = 1,
                FoodSortable = true,
                FoodSort = Sort.None,
                FoodRank = 2,
            };
            return View(o);
        }

        public ActionResult SortingContent(GridDemoSortingCfgInput input)
        {
            return PartialView(input);
        }

        public ActionResult Grouping()
        {
            var o = new GridDemoGroupingCfgInput
            {
                Groupable = true,
                ShowGroupedColumn = true,
                ShowGroupBar = true,
                PersonGrouped = true,
                PersonRem = true,
                PersonGroupable = true,
                PersonRank = 1,
                FoodGrouped = false,
                FoodRem = true,
                FoodGroupable = true,
                PageSize = 15
            };

            return View(o);
        }

        public ActionResult GroupingContent(GridDemoGroupingCfgInput input)
        {
            return PartialView(input);
        }

        /*beginformat*/
        public ActionResult CustomFormatGrid(GridParams g)
        {
            return Json(new GridModelBuilder<Lunch>(Db.Lunches.AsQueryable(), g)
            {
                Map = o =>
                {
                    var rowcls = o.Price > 90 ? "pinkb" : o.Price < 30 ? "greenb" : string.Empty;

                    if (o.Date.Year > 2013) rowcls += " date1";

                    // this will be our rowModel
                    return new
                    {
                        o.Id,
                        o.Person,
                        o.Price,
                        o.Food,
                        Date = o.Date.ToString("dd MMMM yyyy"),
                        o.Location,
                        o.Organic,
                        RowClass = rowcls
                    };
                },
                MakeHeader = gr =>
                {
                    var value = AweUtil.GetColumnValue(gr.Column, gr.Items.First()).Single();
                    var strVal = gr.Column == "Date" ? ((DateTime)value).ToString("dd MMMM yyyy") :
                                 gr.Column == "Price" ? value + " GBP" : value.ToString();

                    return new GroupHeader { Content = gr.Header + " - " + strVal };
                }
            }.Build());
        }

        public ActionResult Details(int id)
        {
            var lunch = Db.Get<Lunch>(id);

            return PartialView(lunch);
        }

        public ActionResult OpenDetails(int id)
        {
            var lunch = Db.Get<Lunch>(id);

            return View(lunch);
        }
        /*endformat*/

        public ActionResult ApiGrid(GridParams g, string person, string food, int? country, int? price)
        {
            food = (food ?? "").ToLower();
            person = (person ?? "").ToLower();

            var list = Db.Lunches
                .Where(o => o.Food.ToLower().Contains(food) && o.Person.ToLower().Contains(person))
                .AsQueryable();

            if (price.HasValue)
            {
                list = list.Where(o => o.Price == price);
            }

            if (country.HasValue) list = list.Where(o => o.Country.Id == country);

            return Json(new GridModelBuilder<Lunch>(list, g)
            {
                KeyProp = o => o.Id, // needed for Entity Framework | nesting | tree | api
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
                    ChefName = o.Chef.FirstName + " " + o.Chef.LastName
                }
            }.Build());
        }
    }
}