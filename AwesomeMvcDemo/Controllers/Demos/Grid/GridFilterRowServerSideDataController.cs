using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AwesomeMvcDemo.Models;
using AwesomeMvcDemo.Utils;
using AwesomeMvcDemo.ViewModels.Display;
using Newtonsoft.Json;
using Omu.AwesomeMvc;
using WebHttpResponse = AwesomeMvcDemo.HttpResponse;
using AwesomeMvcDemo.ViewModels.Input;
using Omu.Awem.Utils;

namespace AwesomeMvcDemo.Controllers.Demos.Grid
{
    public class GridFilterRowServerSideDataController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LargeDataSource()
        {
            return View();
        }

        private static object MapToGridModel(Lunch o)
        {
            return
                new
                {
                    o.Id,
                    o.Person,
                    DateStr = o.Date.ToShortDateString(),
                    CountryName = o.Country.Name,
                    ChefName = o.Chef.FirstName + " " + o.Chef.LastName,
                    MealsStr = string.Join(", ", o.Meals.Select(m => m.Name)),
                    o.Food,
                    o.FoodPic,
                    Organic = o.Organic ? "Yes" : "No"
                };
        }

        public ActionResult LunchFilterGrid(GridParams g, string[] forder, int? date, string person, string food, int? country, int? chef, int[] meals, bool? organic)
        {
            forder = forder ?? new string[] { };
            var query = Db.Lunches.AsQueryable();
            var filterRules = new Dictionary<string, Action>();
            var frow = new LunchFrow();

            filterRules.Add("Person", () =>
            {
                if (person != null)
                {
                    query = query.Where(o => o.Person.IndexOf(person, StringComparison.OrdinalIgnoreCase) >= 0);
                }
            });

            filterRules.Add("Country", () =>
            {
                frow.Country = query.Select(o => o.Country).Distinct().Select(o => new KeyContent(o.Id, o.Name)).ToArray();

                if (country.HasValue)
                {
                    query = query.Where(o => o.Country.Id == country);
                }
            });

            filterRules.Add("Chef", () =>
            {
                frow.Chef = query.Select(o => o.Chef).Distinct().Select(o => new KeyContent(o.Id, o.FullName)).ToArray();

                if (chef.HasValue)
                {
                    query = query.Where(o => o.Chef.Id == chef);
                }
            });

            filterRules.Add("Food", () =>
            {
                var lst = new List<MealDisplay> { new MealDisplay("", "any", "pasta.png") };

                // distinct by Food
                lst.AddRange(query.GroupBy(o => o.Food).Select(gr => gr.First())
                    .Select(o => new MealDisplay(o.Food, o.Food, o.FoodPic)));

                frow.Food = lst.ToArray();

                if (!string.IsNullOrWhiteSpace(food))
                {
                    query = query.Where(o => o.Food == food);
                }
            });

            filterRules.Add("Date", () =>
            {
                var list = new List<KeyContent>
                {
                    new KeyContent("", "all years")
                };

                list.AddRange(AweUtil.ToKeyContent(query.Select(o => o.Date.Year).Distinct().OrderBy(o => o)));

                frow.Date = list.ToArray();

                if (date.HasValue)
                {
                    query = query.Where(o => o.Date.Year == date);
                }
            });

            filterRules.Add("Meals", () =>
            {
                if (meals != null)
                {
                    query = query.Where(o => meals.All(mid => o.Meals.Select(cm => cm.Id).Contains(mid)));
                }

                // get data after querying this time, to filter the meals dropmenu as well
                frow.Meals = query.SelectMany(o => o.Meals).Distinct().OrderBy(o => o.Id).Select(o => new KeyContent(o.Id, o.Name)).ToArray();
            });

            filterRules.Add("Organic", () =>
            {
                var list = new List<KeyContent> { new KeyContent("", "all") };
                list.AddRange(query.Select(o => o.Organic).Distinct().Select(o => new KeyContent(o, o ? "Yes" : "No")));

                frow.Organic = list.ToArray();

                if (organic != null)
                {
                    query = query.Where(o => o.Organic == organic);
                }
            });

            // apply rules present in forder (order picked by the user)
            foreach (var prop in forder)
            {
                if (filterRules.ContainsKey(prop))
                {
                    filterRules[prop]();
                }
            }

            // apply the rest, to populate the rest of the filter row editors
            foreach (var pair in filterRules.Where(o => !forder.Contains(o.Key)))
            {
                pair.Value();
            }

            return Json(new GridModelBuilder<Lunch>(query, g)
            {
                KeyProp = o => o.Id,
                Map = MapToGridModel,
                Tag = new { frow = frow }
            }.Build());
        }

        class LunchFrow
        {
            public KeyContent[] Date { get; set; }
            public KeyContent[] Chef { get; set; }
            public KeyContent[] Country { get; set; }
            public KeyContent[] Meals { get; set; }
            public KeyContent[] Organic { get; set; }

            // MealDisplay inherits KeyContent, has url property for FoodPic
            public MealDisplay[] Food { get; set; }
        }

        #region Outside filter row

        public ActionResult DinnersFilterGrid(GridParams g, string[] forder, string ProgramID, string ProgramName, string Key, string Value)
        {
            string filterCriteria = string.Empty;

            if (!string.IsNullOrEmpty(ProgramID))
            {
                filterCriteria = " ProgramID like '%" + ProgramID + "%' ";
            }

            if (!string.IsNullOrEmpty(ProgramName))
            {
                if(string.IsNullOrEmpty(filterCriteria))
                    filterCriteria = " ProgramName like '%" + ProgramName + "%' ";
                else
                    filterCriteria = filterCriteria + " AND ProgramName like '%" + ProgramName + "%' ";
            }

            if (!string.IsNullOrEmpty(Key))
            {
                if (string.IsNullOrEmpty(filterCriteria))
                    filterCriteria = " Key like '%" + Key + "%' ";
                else
                    filterCriteria = filterCriteria + " AND Key like '%" + Key + "%' ";
            }

            if (!string.IsNullOrEmpty(Value))
            {
                if (string.IsNullOrEmpty(filterCriteria))
                    filterCriteria = " Value like '%" + Value + "%' ";
                else
                    filterCriteria = filterCriteria + " AND Value like '%" + Value + "%' ";
            }

            forder = forder ?? new string[] { };
            var query = Db.Dinners.AsQueryable();

            var response = new WebHttpResponse();
            var baseModel = new BaseGridModel();
            baseModel.search = filterCriteria;
            baseModel.pagenumber = g.Page;
            baseModel.pagesize = g.PageSize;

            string data = JsonConvert.SerializeObject(baseModel);
            string url = string.Format("{0}T1Service/GetServiceControllerData_Count", "http://localhost:11977/api/");

            response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", data);
            var totalCount = Convert.ToInt32(response.RawResponse);


            url = string.Format("{0}T1Service/ServiceDataGetAll_New", "http://localhost:11977/api/");

            response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", data);
            var data1 = JsonConvert.DeserializeObject<List<T1ServiceModel>>(response.RawResponse).ToList().AsQueryable();

            //var filterRules = new Dictionary<string, Action>();
            //var frow = new DinnerFrow();

            //filterRules.Add("ProgramID", () =>
            //{
            //    if (ProgramID != null)
            //    {
            //        data1 = data1.Where(o => o.ProgramID.IndexOf(ProgramID, StringComparison.OrdinalIgnoreCase) >= 0);
            //    }
            //});

            //filterRules.Add("ProgramName", () =>
            //{
            //    if (ProgramName != null)
            //    {
            //        data1 = data1.Where(o => o.ProgramName.IndexOf(ProgramName, StringComparison.OrdinalIgnoreCase) >= 0);
            //    }
            //});

            //filterRules.Add("Key", () =>
            //{
            //    if (Key != null)
            //    {
            //        data1 = data1.Where(o => o.Key.IndexOf(Key, StringComparison.OrdinalIgnoreCase) >= 0);
            //    }
            //});

            //filterRules.Add("Value", () =>
            //{
            //    if (Value != null)
            //    {
            //        data1 = data1.Where(o => o.Value.IndexOf(Value, StringComparison.OrdinalIgnoreCase) >= 0);
            //    }
            //});

            //// apply rules present in forder (touched by the user)
            //foreach (var prop in forder)
            //{
            //    if (filterRules.ContainsKey(prop))
            //    {
            //        filterRules[prop]();
            //    }
            //}

            //// apply the rest
            //foreach (var pair in filterRules.Where(o => !forder.Contains(o.Key)))
            //{
            //    pair.Value();
            //}

            return Json(new GridModelBuilder<T1ServiceModel>(data1, g)
            {
                KeyProp = o => o.Id,
                PageCount = (totalCount / g.PageSize)
                //,Tag = new { frow = frow }
            }.Build());
        }

        public ActionResult DinnersFilterGrid1(GridParams g, string[] forder, int? date, string name, int? chef, int[] meal, bool? organic)
        {
            forder = forder ?? new string[] { };
            var query = Db.Dinners.AsQueryable();
            var filterRules = new Dictionary<string, Action>();
            var frow = new DinnerFrow();

            filterRules.Add("Name", () =>
            {
                if (name != null)
                {
                    query = query.Where(o => o.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                }
            });

            filterRules.Add("Date", () =>
            {
                var list = new List<KeyContent>
                {
                    new KeyContent("", "all years")
                };

                list.AddRange(AweUtil.ToKeyContent(query.Select(o => o.Date.Year).Distinct().OrderBy(o => o)));

                frow.Date = list.ToArray();

                if (date.HasValue)
                {
                    query = query.Where(o => o.Date.Year == date);
                }
            });

            filterRules.Add("Chef", () =>
            {
                frow.Chef = query.Select(o => o.Chef).Distinct().Select(o => new KeyContent(o.Id, o.FirstName + " " + o.LastName)).ToArray();

                if (chef.HasValue)
                {
                    query = query.Where(o => o.Chef.Id == chef);
                }
            });

            filterRules.Add("Meal", () =>
            {
                if (meal != null)
                {
                    query = query.Where(o => meal.All(mid => o.Meals.Select(cm => cm.Id).Contains(mid)));
                }

                // get data after querying this time, to filter the meals dropmenu as well
                frow.Meal = query.SelectMany(o => o.Meals).Distinct().OrderBy(o => o.Id).Select(o => new KeyContent(o.Id, o.Name)).ToArray();
            });

            filterRules.Add("Organic", () =>
            {
                var list = new List<KeyContent> { new KeyContent("", "all") };
                list.AddRange(query.Select(o => o.Organic).Distinct().Select(o => new KeyContent(o, o ? "Yes" : "No")));

                frow.Organic = list.ToArray();

                if (organic != null)
                {
                    query = query.Where(o => o.Organic == organic);
                }
            });

            // apply rules present in forder (touched by the user)
            foreach (var prop in forder)
            {
                if (filterRules.ContainsKey(prop))
                {
                    filterRules[prop]();
                }
            }

            // apply the rest
            foreach (var pair in filterRules.Where(o => !forder.Contains(o.Key)))
            {
                pair.Value();
            }

            return Json(new GridModelBuilder<Dinner>(query, g)
            {
                KeyProp = o => o.Id,
                Map = DinnerMapToGridModel,
                Tag = new { frow = frow }
            }.Build());
        }

        [HttpPost]
        public ActionResult BatchSave(T1ServiceModel[] inputs)
        {
            var res = new List<object>();

            foreach (var input in inputs)
            {
                var vstate = ModelUtil.Validate(input);

                if (vstate.IsValid())
                {
                    try
                    {
                        //var edit = input.Id.HasValue;
                        //var ent = edit ? Db.Get<Dinner>(input.Id) : new Dinner();

                        //ent.Name = input.Name;
                        //ent.Date = input.Date.Value;
                        //ent.Chef = Db.Get<Chef>(input.Chef);
                        //ent.Meals = input.Meals.Select(mid => Db.Get<Meal>(mid));
                        //ent.BonusMeal = Db.Get<Meal>(input.BonusMealId);
                        //ent.Organic = input.Organic ?? false;

                        //if (edit)
                        //{
                        //    Db.Update(ent);
                        //}
                        //else
                        //{
                        //    Db.Insert(ent);
                        //}

                        // res.Add(new { Item = MapToGridModel(ent) });
                    }
                    catch (Exception ex)
                    {
                        vstate.Add("Name", ex.Message);
                    }
                }

                if (!vstate.IsValid())
                {
                    res.Add(vstate.ToInlineErrors());
                }
            }

            return Json(res);
        }

        class DinnerFrow
        {
            public KeyContent[] Date { get; set; }
            public KeyContent[] Chef { get; set; }
            public KeyContent[] Meal { get; set; }
            public KeyContent[] Organic { get; set; }
        }

        private static object DinnerMapToGridModel(Dinner o)
        {
            return
                new
                {
                    o.Id,
                    o.Name,
                    Date = o.Date.ToShortDateString(),
                    ChefName = o.Chef.FirstName + " " + o.Chef.LastName,
                    Meals = string.Join(", ", o.Meals.Select(m => m.Name)),
                    Organic = o.Organic ? "Yes" : "No"
                };
        }
        #endregion

        #region Large data source
        /*beginlrgd*/
        public ActionResult LunchFilterGridLarge(GridParams g, LunchFilterPrm fp)
        {
            fp.Forder = fp.Forder ?? new string[] { };
            var query = Db.Lunches.AsQueryable();

            var frow = new LunchFrow();

            query = ApplyFilter(query, fp, (prop, q) =>
            {
                if (prop == "Meals" && fp.Meals != null)
                {
                    // getting the items only for the currently selected value, rest can be obtain by searching ( SearchFunc )
                    frow.Meals = Db.Meals.Where(o => fp.Meals.Contains(o.Id))
                        .Select(o => new KeyContent(o.Id, o.Name))
                        .ToArray();
                }

                if (prop == "Chef" && fp.Chef != null)
                {
                    frow.Chef = q.Select(o => o.Chef)
                        .Where(o => o.Id == fp.Chef)
                        .Take(1)
                        .Select(o => new KeyContent(o.Id, o.FullName))
                        .ToArray();

                    // getting the Chef from q and not from Db.Chefs,
                    // when a lower forder filter is changed frow.Chef could be empty and current filter will reset
                }
            });

            return Json(new GridModelBuilder<Lunch>(query, g)
            {
                KeyProp = o => o.Id,
                Map = MapToGridModel,
                Tag = new { frow = frow }
            }.Build());
        }

        private IQueryable<Lunch> ApplyFilter(IQueryable<Lunch> query, LunchFilterPrm fp, Action<string, IQueryable<Lunch>> setProp = null)
        {
            var filterRules = new Dictionary<string, Action>();
            fp.Forder = fp.Forder ?? new string[] { };

            filterRules.Add("Person", () =>
            {
                if (fp.Person != null)
                {
                    query = query.Where(o => o.Person.IndexOf(fp.Person, StringComparison.OrdinalIgnoreCase) >= 0);
                }
            });

            // not setting frow for Country, Food and Meals, they get their data using SearchFunc
            filterRules.Add("Country", () =>
            {
                if (fp.Country.HasValue)
                {
                    query = query.Where(o => o.Country.Id == fp.Country);
                }
            });

            filterRules.Add("Food", () =>
            {
                if (!string.IsNullOrWhiteSpace(fp.Food))
                {
                    query = query.Where(o => o.Food == fp.Food);
                }
            });

            filterRules.Add("Meals", () =>
            {
                if (fp.Meals != null)
                {
                    query = query.Where(o => fp.Meals.All(mid => o.Meals.Select(cm => cm.Id).Contains(mid)));
                }

                if (setProp != null)
                {
                    setProp("Meals", query);
                }
            });

            filterRules.Add("Chef", () =>
            {
                if (setProp != null)
                {
                    setProp("Chef", query);
                }

                if (fp.Chef.HasValue)
                {
                    query = query.Where(o => o.Chef.Id == fp.Chef);
                }
            });

            // apply rules present in forder (order picked by the user)
            foreach (var prop in fp.Forder)
            {
                if (filterRules.ContainsKey(prop))
                {
                    filterRules[prop]();
                }
            }

            // apply the rest, to populate the rest of the filter row editors
            foreach (var pair in filterRules.Where(o => !fp.Forder.Contains(o.Key)))
            {
                pair.Value();
            }

            return query;
        }

        public ActionResult SearchFood(string term = "")
        {
            var items = Db.Lunches
                .Where(o => o.Food.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                .DistinctBy(o => o.Food) // DistinctBy extension from DemoUtils.cs
                .Take(10)
                .Select(o => new MealDisplay(o.Food, o.Food, o.FoodPic));

            return Json(items);
        }

        public ActionResult GetFoodsInit(string v)
        {
            var items = Db.Lunches.Take(3).ToList();
            var selected = Db.Lunches.FirstOrDefault(o => o.Food == v);

            if (selected != null && !items.Contains(selected))
            {
                items.Add(selected);
            }

            items = items.DistinctBy(o => o.Food).ToList();

            var lst = new List<MealDisplay> { new MealDisplay("", "any", "pasta.png") };
            lst.AddRange(items.Select(o => new MealDisplay(o.Food, o.Food, o.FoodPic)));

            return Json(lst);
        }

        public ActionResult GetCountryInit(int? v)
        {
            var items = Db.Countries.Take(3).ToList();
            var selected = Db.Countries.SingleOrDefault(o => o.Id == v);

            if (selected != null && !items.Contains(selected))
            {
                items.Add(selected);
            }

            return Json(items.Select(o => new KeyContent(o.Id, o.Name)));
        }

        public ActionResult SearchCountry(string term = "")
        {
            var items = Db.Countries
                .Where(o => o.Name.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0) // with EF/real Db, we would use o.Name.Contains(term)
                .Take(10)
                .Select(o => new KeyContent(o.Id, o.Name));

            return Json(items);
        }

        public ActionResult SearchMeals(string term, LunchFilterPrm fp)
        {
            var query = Db.Lunches.AsQueryable();

            var frow = new LunchFrow();

            ApplyFilter(query, fp, (prop, q) =>
            {
                if (prop == "Meals")
                {
                    frow.Meals = q.SelectMany(o => o.Meals)
                        .Where(m => m.Name.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                        .Distinct()
                        .Take(10)
                        .OrderBy(o => o.Id)
                        .Select(o => new KeyContent(o.Id, o.Name))
                        .ToArray();
                }
            });

            return Json(frow.Meals);
        }

        public ActionResult SearchChef(string term, LunchFilterPrm fp)
        {
            var query = Db.Lunches.AsQueryable();

            var frow = new LunchFrow();

            ApplyFilter(query, fp, (prop, q) =>
            {
                if (prop == "Chef")
                {
                    frow.Chef = q.Select(o => o.Chef)
                        .Where(m => m.FullName.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                        .Distinct()
                        .Take(10)
                        .OrderBy(o => o.Id)
                        .Select(o => new KeyContent(o.Id, o.FullName))
                        .ToArray();
                }
            });

            return Json(frow.Chef);
        }

        public class LunchFilterPrm
        {
            public string[] Forder { get; set; }
            public string Person { get; set; }
            public string Food { get; set; }
            public int? Country { get; set; }
            public int? Chef { get; set; }
            public int[] Meals { get; set; }
        }
        /*endlrgd*/
        #endregion
    }
}