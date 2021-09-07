using System;
using System.Linq;
using System.Web.Mvc;

using AwesomeMvcDemo.Models;
using AwesomeMvcDemo.ViewModels.Input;
using Omu.Awem.Utils;
using Omu.AwesomeMvc;

namespace AwesomeMvcDemo.Controllers.Demos.Grid
{
    public class GridInlineEditDemoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConditionalDemo()
        {
            return View();
        }

        public ActionResult MultiEditorsDemo()
        {
            return View();
        }

        public ActionResult ClientSave()
        {
            return View();
        }


        private object MapToGridModel(Dinner o)
        {
            return new
            {
                o.Id,
                o.Name,
                Date = o.Date.ToShortDateString(),
                ChefName = o.Chef.FirstName + " " + o.Chef.LastName,
                Meals = string.Join(", ", o.Meals.Select(m => m.Name)),
                BonusMeal = o.BonusMeal.Name,
                o.Organic,
                DispOrganic = o.Organic ? "Yes" : "No",

                // below properties used for inline editing only
                MealsIds = o.Meals.Select(m => m.Id).ToArray(),
                ChefId = o.Chef.Id,
                BonusMealId = o.BonusMeal.Id,

                // for conditional demo
                Editable = o.Meals.Count() > 1,
                DateReadOnly = o.Date.Year < 2015
            };
        }

        public ActionResult GridGetItems(GridParams g, string search)
        {
            search = (search ?? "").ToLower();
            var items = Db.Dinners.Where(o => o.Name.ToLower().Contains(search)).AsQueryable();

            var model = new GridModelBuilder<Dinner>(items, g)
            {
                KeyProp = o => o.Id, // needed for api select, update, tree, nesting, EF
                GetItem = () => Db.Get<Dinner>(Convert.ToInt32(g.Key)), // called by the grid.api.update
                Map = MapToGridModel,
            }.Build();

            return Json(model);
        }

        [HttpPost]
        public ActionResult Save(DinnerInput input)
        {
            if (ModelState.IsValid)
            {
                var edit = input.Id.HasValue;
                var ent = edit ? Db.Get<Dinner>(input.Id) : new Dinner();

                ent.Name = input.Name;
                ent.Date = input.Date.Value;
                ent.Chef = Db.Get<Chef>(input.Chef);
                ent.Meals = input.Meals.Select(mid => Db.Get<Meal>(mid));
                ent.BonusMeal = Db.Get<Meal>(input.BonusMealId);
                ent.Organic = input.Organic ?? false;

                if (edit)
                {
                    Db.Update(ent);
                }
                else
                {
                    Db.Insert(ent);
                }

                return Json(new { Item = MapToGridModel(ent) });
            }

            return Json(ModelState.GetErrorsInline());
        }

        public ActionResult Delete(int id)
        {
            var dinner = Db.Get<Dinner>(id);

            return PartialView(new DeleteConfirmInput
            {
                Id = id,
                Type = "dinner",
                Name = dinner.Name
            });
        }

        [HttpPost]
        public ActionResult Delete(DeleteConfirmInput input)
        {
            Db.Delete<Dinner>(input.Id);

            // the PopupForm's success function utils.itemDeleted expects an obj with "Id" property
            return Json(new { input.Id });
        }

        public ActionResult Popup()
        {
            return PartialView();
        }
    }
}