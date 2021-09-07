using System;
using System.Linq;
using System.Web.Mvc;
using AwesomeMvcDemo.Models;
using AwesomeMvcDemo.ViewModels.Input;
using Omu.Awem.Utils;
using Omu.AwesomeMvc;

namespace AwesomeMvcDemo.Controllers.Demos.Grid
{
    public class GridInlineEditParentController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private object MapToGridModel(ParentMeal o)
        {
            return new
            {
                o.Id,

                // for display
                CategoryName = o.Category.Name,
                MealName = o.Meal.Name,

                // for inline editing helpers to get value
                CategoryId = o.Category.Id,
                MealId = o.Meal.Id
            };
        }

        public ActionResult GridGetItems(GridParams g)
        {
            var items = Db.ParentMeals.AsQueryable();

            var model = new GridModelBuilder<ParentMeal>(items, g)
            {
                KeyProp = o => o.Id,
                GetItem = () => Db.Get<ParentMeal>(Convert.ToInt32(g.Key)),
                Map = MapToGridModel,
            }.Build();

            return Json(model);
        }

        [HttpPost]
        public ActionResult Create(ParentMealInput input)
        {
            if (ModelState.IsValid)
            {
                var parentMeal = new ParentMeal
                {
                    Category = Db.Get<Category>(input.CategoryId),
                    Meal = Db.Get<Meal>(input.MealId)
                };

                Db.Insert(parentMeal);

                return Json(new { Item = MapToGridModel(parentMeal) });
            }

            return Json(ModelState.GetErrorsInline());
        }

        [HttpPost]
        public ActionResult Edit(ParentMealInput input)
        {
            if (ModelState.IsValid)
            {
                var parentMeal = Db.Get<ParentMeal>(input.Id);
                parentMeal.Category = Db.Get<Category>(input.CategoryId);
                parentMeal.Meal = Db.Get<Meal>(input.MealId);
                Db.Update(parentMeal);

                return Json(new { });
            }

            return Json(ModelState.GetErrorsInline());
        }

        public ActionResult Delete(int id)
        {
            var parentMeal = Db.Get<ParentMeal>(id);

            return PartialView(new DeleteConfirmInput
            {
                Id = id,
                Name = parentMeal.Meal.Name
            });
        }

        [HttpPost]
        public ActionResult Delete(DeleteConfirmInput input)
        {
            Db.Delete<ParentMeal>(input.Id);
            return Json(new { input.Id });
        }
    }
}