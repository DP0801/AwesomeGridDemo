using System;
using System.Linq;
using System.Web.Mvc;
using AwesomeMvcDemo.Models;
using AwesomeMvcDemo.ViewModels.Input;
using Omu.Awem.Utils;
using Omu.AwesomeMvc;

namespace AwesomeMvcDemo.Controllers.Demos.Grid.MasterDetailCrud
{
    public class AddressesGridCrudController : Controller
    {
        private object mapToGridModel(RestaurantAddress o)
        {
            return new
            {
                o.Id, 
                o.Line1, 
                o.Line2, 
                ChefName = o.Chef.FirstName + " " + o.Chef.LastName,
                
                ChefId = o.Chef.Id // for inline editing, gives value to the Chef inline dropdown
            };
        }

        public ActionResult GridGetItems(GridParams g, int restaurantId)
        {
            var items = Db.RestaurantAddresses.Where(o => o.RestaurantId == restaurantId).AsQueryable();
            var model = new GridModelBuilder<RestaurantAddress>(items, g)
                {
                    KeyProp = o => o.Id,
                    Map = mapToGridModel,
                    GetItem = () => Db.Get<RestaurantAddress>(Convert.ToInt32(g.Key))
                }.Build();
            return Json(model);
        }

        public ActionResult Create(int restaurantId)
        {
            return PartialView(new RestaurantAddressInput { RestaurantId = restaurantId });
        }

        [HttpPost]
        public ActionResult Create(RestaurantAddressInput input)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(input);
            }

            var address = Db.Insert(new RestaurantAddress
            {
                Line1 = input.Line1, 
                Line2 = input.Line2, 
                RestaurantId = input.RestaurantId, 
                Chef = Db.Get<Chef>(input.ChefId)
            });

            return Json(mapToGridModel(address));
        }

        public ActionResult Edit(int id)
        {
            var address = Db.Get<RestaurantAddress>(id);

            return PartialView(
                "Create",
                new RestaurantAddressInput
                    {
                        Line1 = address.Line1,
                        Line2 = address.Line2,
                        ChefId = address.Chef.Id,
                        RestaurantId = address.RestaurantId
                    });
        }

        [HttpPost]
        public ActionResult Edit(RestaurantAddressInput input)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("Create", input);
            }

            var address = Db.Get<RestaurantAddress>(input.Id);
            address.Line1 = input.Line1;
            address.Line2 = input.Line2;
            address.Chef = Db.Get<Chef>(input.ChefId);

            return Json(new { input.Id });
        }

        public ActionResult Delete(int id)
        {
            var address = Db.Get<RestaurantAddress>(id);

            return PartialView(new DeleteConfirmInput
                {
                    Id = id,
                    Type = "restaurant address",
                    Name = address.Line1 + " "+ address.Line2
                });
        }

        [HttpPost]
        public ActionResult Delete(DeleteConfirmInput input)
        {
            Db.Delete<RestaurantAddress>(input.Id);
            return Json(new { input.Id });
        }

        #region for inline editing
        [HttpPost]
        public ActionResult CreateInline(RestaurantAddressInput input)
        {
            if (!ModelState.IsValid)
            {
                return Json(ModelState.GetErrorsInline());
            }

            var ent = Db.Insert(new RestaurantAddress
            {
                RestaurantId = input.RestaurantId,
                Line1 = input.Line1,
                Line2 = input.Line2,
                Chef = Db.Get<Chef>(input.ChefId)
            });

            return Json(new { Item = mapToGridModel(ent) });

        }

        [HttpPost]
        public ActionResult EditInline(RestaurantAddressInput input)
        {
            if (!ModelState.IsValid)
            {
                return Json(ModelState.GetErrorsInline());
            }

            var ent = Db.Get<RestaurantAddress>(input.Id);
            ent.Line1 = input.Line1;
            ent.Line2 = input.Line2;
            ent.Chef = Db.Get<Chef>(input.ChefId);

            Db.Update(ent);

            return Json(new { });
        } 
        #endregion
    }
}