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
using System.Configuration;

namespace AwesomeMvcDemo.Controllers
{
    public class T1SignalServiceController : Controller
    {
        // GET: T1SignalService
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult T1SignalServiceGrid(GridParams g, string[] forder, string HostName, string ProgramName, string Key, string Value)
        {
            string filterCriteria = string.Empty;

            if (!string.IsNullOrEmpty(HostName))
            {
                filterCriteria = " HostName like '%" + HostName + "%' ";
            }

            if (!string.IsNullOrEmpty(ProgramName))
            {
                if (string.IsNullOrEmpty(filterCriteria))
                    filterCriteria = " ProgramName like '%" + ProgramName + "%' ";
                else
                    filterCriteria = filterCriteria + " AND ProgramName like '%" + ProgramName + "%' ";
            }

            if (!string.IsNullOrEmpty(Key))
            {
                if (string.IsNullOrEmpty(filterCriteria))
                    filterCriteria = " [Key] like '%" + Key + "%' ";
                else
                    filterCriteria = filterCriteria + " AND [Key] like '%" + Key + "%' ";
            }

            if (!string.IsNullOrEmpty(Value))
            {
                if (string.IsNullOrEmpty(filterCriteria))
                    filterCriteria = " [Value] like '%" + Value + "%' ";
                else
                    filterCriteria = filterCriteria + " AND [Value] like '%" + Value + "%' ";
            }

            forder = forder ?? new string[] { };
            var query = Db.Dinners.AsQueryable();

            var response = new WebHttpResponse();
            var baseModel = new BaseGridModel();
            baseModel.search = filterCriteria;
            baseModel.pagenumber = g.Page;
            baseModel.pagesize = g.PageSize;

            string requestData = JsonConvert.SerializeObject(baseModel);
            string url = string.Format("{0}T1Service/GetServiceControllerData_Count", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);

            response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", requestData);
            var totalCount = Convert.ToInt32(response.RawResponse);


            url = string.Format("{0}T1Service/ServiceDataGetAll_New", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);

            response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", requestData);
            var responseData = JsonConvert.DeserializeObject<List<T1ServiceModel>>(response.RawResponse).ToList().AsQueryable();

            return Json(new GridModelBuilder<T1ServiceModel>(responseData, g)
            {
                KeyProp = o => o.Id,
                PageCount = (totalCount / g.PageSize)
                //,Tag = new { frow = frow }
            }.Build());
        }

        [HttpPost]
        public ActionResult BatchSave(T1ServiceModel[] inputs)
        {
            var res = new List<object>();

            if (inputs.Count() == 1)
            {
                foreach (var input in inputs)
                {
                    var vstate = ModelUtil.Validate(input);

                    if (vstate.IsValid())
                    {
                        try
                        {
                            var baseModel = new T1ServiceModel();
                            baseModel.Id = input.Id;
                            baseModel.ProgramID = input.ProgramName;
                            baseModel.ProgramName = input.ProgramName;
                            baseModel.HostName = input.HostName;
                            baseModel.Key = input.Key;
                            baseModel.Value = input.Value;
                            baseModel.IsActive = input.IsActive;
                            baseModel.Notes = input.Notes;

                            string data = JsonConvert.SerializeObject(baseModel);
                            string url = string.Empty;

                            if (input.Id == 0)
                            {
                                url = string.Format("{0}T1Service/InsertServiceData", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);
                            }
                            else
                            {
                                url = string.Format("{0}T1Service/UpdateServiceData", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);
                            }

                            var response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", data);
                            var edit = input.Id;
                            res.Add(input);
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
            }
            else
            {
                var lstT1ServiceModel = new List<T1ServiceModel>();
                foreach (var input in inputs)
                {
                    var vstate = ModelUtil.Validate(input);

                    if (vstate.IsValid())
                    {
                        try
                        {
                            var baseModel = new T1ServiceModel();
                            baseModel.Id = input.Id;
                            baseModel.ProgramID = input.ProgramName;
                            baseModel.ProgramName = input.ProgramName;
                            baseModel.HostName = input.HostName;
                            baseModel.Key = input.Key;
                            baseModel.Value = input.Value;
                            baseModel.IsActive = input.IsActive;
                            baseModel.Notes = input.Notes;
                            lstT1ServiceModel.Add(baseModel);
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

                string data = JsonConvert.SerializeObject(lstT1ServiceModel);
                string url = string.Format("{0}T1Service/InsertServiceData_New", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);
                var response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", data);
            }

            return Json(res);
        }

        [HttpPost]
        public ActionResult BatchSaveAll(T1ServiceModel[] inputs)
        {
            var res = new List<object>();
            var lstT1ServiceModel = new List<T1ServiceModel>();
            foreach (var input in inputs)
            {
                var vstate = ModelUtil.Validate(input);

                if (vstate.IsValid())
                {
                    try
                    {
                        var baseModel = new T1ServiceModel();
                        baseModel.Id = input.Id;
                        baseModel.ProgramID = input.ProgramName;
                        baseModel.ProgramName = input.ProgramName;
                        baseModel.HostName = input.HostName;
                        baseModel.Key = input.Key;
                        baseModel.Value = input.Value;
                        baseModel.IsActive = input.IsActive;
                        baseModel.Notes = input.Notes;
                        lstT1ServiceModel.Add(baseModel);
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

            string data = JsonConvert.SerializeObject(lstT1ServiceModel);
            string url = string.Format("{0}T1Service/InsertServiceData_New", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);
            var response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", data);

            return Json(res);
        }

        [HttpPost]
        public ActionResult BatchSave1(T1ServiceModel[] inputs)
        {
            string value = Convert.ToString(Request["txtKeyValue"].ToString());
            var res = new List<object>();

            foreach (var input in inputs)
            {
                var vstate = ModelUtil.Validate(input);

                if (vstate.IsValid())
                {
                    try
                    {
                        var baseModel = new T1ServiceModel();
                        baseModel.Id = input.Id;
                        baseModel.ProgramID = input.ProgramName;
                        baseModel.ProgramName = input.ProgramName;
                        baseModel.HostName = input.HostName;
                        baseModel.Key = input.Key;
                        baseModel.Value = input.Value;
                        baseModel.IsActive = input.IsActive;
                        baseModel.Notes = input.Notes;

                        string data = JsonConvert.SerializeObject(baseModel);
                        string url = string.Empty;

                        if (input.Id == 0)
                        {
                            url = string.Format("{0}T1Service/InsertServiceData", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);
                        }
                        else
                        {
                            url = string.Format("{0}T1Service/UpdateServiceData", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);
                        }

                        var response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", data);
                        var edit = input.Id;
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
                        res.Add(input);
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
    }
}