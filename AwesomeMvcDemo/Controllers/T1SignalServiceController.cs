﻿using System;
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
using System.Net;
using log4net;

namespace AwesomeMvcDemo.Controllers
{
    public class T1SignalServiceController : Controller
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: T1SignalService
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult T1SignalServiceGrid(GridParams g, string[] forder, string HostName, string ProgramName, string Key, string Value)
        {
            log.Debug("test");
            Session.Remove("filterCriteria");
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

            var response = new WebHttpResponse();
            var baseModel = new BaseGridModel();
            baseModel.search = filterCriteria;
            baseModel.pagenumber = g.Page;
            baseModel.pagesize = g.PageSize;

            string requestData = JsonConvert.SerializeObject(baseModel);
            string url = string.Format("{0}T1Service/GetServiceControllerData_Count", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);
            try
            {

                response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", requestData);

                if (response.RawResponse == null)
                {
                    return Json(new GridModelBuilder<T1ServiceModel>(null, g)
                    {
                        KeyProp = o => o.Id
                    }.Build());
                }

                var totalCount = Convert.ToInt32(response.RawResponse);


                url = string.Format("{0}T1Service/ServiceDataGetAll_New", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);

                response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", requestData);

                if (response.RawResponse != null)
                {
                    var responseData = JsonConvert.DeserializeObject<List<T1ServiceModel>>(response.RawResponse).ToList().AsQueryable();

                    Session.Add("filterCriteria", filterCriteria);

                    int PageCount = (totalCount / g.PageSize);

                    if (totalCount > g.PageSize)
                        PageCount = PageCount + 1;

                    return Json(new GridModelBuilder<T1ServiceModel>(responseData, g)
                    {
                        KeyProp = o => o.Id,
                        PageCount = PageCount
                        //,Tag = new { frow = frow }
                    }.Build());
                }
                else
                {
                    log.Error("This could be an error");
                }

            }
            catch (Exception ex)
            {
                log.Error($"Error to call API {url}:{ex.ToString()}");
            }
            return Json(new GridModelBuilder<T1ServiceModel>(null, g)
            {
                KeyProp = o => o.Id
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
                            baseModel.Id = (input.Id == null) ? 0 : input.Id;
                            baseModel.ProgramID = input.ProgramName;
                            baseModel.ProgramName = input.ProgramName;
                            baseModel.HostName = input.HostName;
                            baseModel.Key = input.Key;
                            baseModel.Value = input.Value;
                            baseModel.IsActive = (input.IsActive == null) ? false : input.IsActive;
                            baseModel.Notes = input.Notes;

                            string data = JsonConvert.SerializeObject(baseModel);
                            string url = string.Empty;

                            if (input.Id == 0 || input.Id == null)
                            {
                                url = string.Format("{0}T1Service/InsertServiceData", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);
                            }
                            else
                            {
                                url = string.Format("{0}T1Service/UpdateServiceData", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);
                            }

                            var response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", data);
                            var edit = input.Id;
                            // res.Add(input);
                            res.Add(new { Item = MapToGridModel(baseModel) });
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
                            baseModel.Id = (input.Id == null) ? 0 : input.Id;
                            baseModel.ProgramID = input.ProgramName;
                            baseModel.ProgramName = input.ProgramName;
                            baseModel.HostName = input.HostName;
                            baseModel.Key = input.Key;
                            baseModel.Value = input.Value;
                            baseModel.IsActive = (input.IsActive == null) ? false : input.IsActive;
                            baseModel.Notes = input.Notes;
                            lstT1ServiceModel.Add(baseModel);

                            res.Add(new { Item = MapToGridModel(baseModel) });
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
                try
                {
                    var response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", data);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    log.Error($"Error to call API {url}:{ex.ToString()}");
                }
            }

            return Json(res);
        }

        [HttpPost]
        public void BatchSave1()
        {
            if ((string)Session["filterCriteria"] != null)
            {
                string filterCriteria = (string)Session["filterCriteria"];
            }

            //return RedirectToAction("T1SignalServiceGrid", "T1SignalService");
        }

        private object MapToGridModel(T1ServiceModel o)
        {
            return new
            {
                o.Id,
                o.HostName,
                o.ProgramName,
                o.Key,
                o.Value,
                o.IsActive,
                o.Notes
            };
        }
    }
}