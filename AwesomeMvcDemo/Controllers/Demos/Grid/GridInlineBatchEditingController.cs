using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AwesomeMvcDemo.Models;
using AwesomeMvcDemo.ViewModels.Input;
using Newtonsoft.Json;
using Omu.Awem.Utils;
using Omu.AwesomeMvc;
using WebHttpResponse = AwesomeMvcDemo.HttpResponse;

namespace AwesomeMvcDemo.Controllers.Demos.Grid
{
    public class GridInlineBatchEditingController : Controller
    {
        public ActionResult Index()
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
                MealsIds = o.Meals.Select(m => m.Id.ToString()).ToArray(), // value for meals multiselect
                ChefId = o.Chef.Id, // value for chef dropdown
                BonusMealId = o.BonusMeal.Id // value for bonus meal dropdown
            };
        }

        public ActionResult GridGetData(GridParams g, string search)
        {
            search = (search ?? "").ToLower();
            var items = Db.Dinners.Where(o => o.Name.ToLower().Contains(search)).AsQueryable();

            var response = new WebHttpResponse();
            var baseModel = new BaseGridModel();
            baseModel.search = search;
            baseModel.pagenumber = g.Page;
            baseModel.pagesize = g.PageSize;

            string data = JsonConvert.SerializeObject(baseModel);
            string url = string.Format("{0}T1Service/GetServiceControllerData_Count", "http://localhost:11977/api/");

            response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", data);
            var totalCount = Convert.ToInt32(response.RawResponse);


            url = string.Format("{0}T1Service/ServiceDataGetAll_New", "http://localhost:11977/api/");

            response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", data);
            var data1 = JsonConvert.DeserializeObject<List<T1ServiceModel>>(response.RawResponse).ToList().AsQueryable();

            //var model = new GridModelBuilder<Dinner>(items, g)
            //{
            //    KeyProp = o => o.Id, // needed for api select, update, tree, nesting, EF
            //    GetItem = () => Db.Get<Dinner>(Convert.ToInt32(g.Key)), // called by the grid.api.update
            //    Map = MapToGridModel,
            //}.Build();

            var model = new GridModelBuilder<T1ServiceModel>(data1, g)
            {
                KeyProp = o => o.Id, // needed for api select, update, tree, nesting, EF
                PageCount = (totalCount/ g.PageSize)
            }.Build();

            return Json(model);
        }

        [HttpPost]
        public ActionResult BatchSave(DinnerInput[] inputs)
        {
            var res = new List<object>();

            foreach (var input in inputs)
            {
                var vstate = ModelUtil.Validate(input);

                if (vstate.IsValid())
                {
                    try
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

                        res.Add(new { Item = MapToGridModel(ent) });
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
    }
}