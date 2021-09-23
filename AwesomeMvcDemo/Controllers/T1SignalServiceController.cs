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
            var model = new T1ServiceModel();
            ////This API call will retrieve data to bind dropdown
            //string url = string.Format("{0}T1Service/GetCommonDropdown", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);

            //var response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", null);
            //if (response.RawResponse != null)
            //{
            //    var dropdownData = JsonConvert.DeserializeObject<T1ServiceModel>(response.RawResponse);

            //    var viewbagHostName = new List<SelectListItem>();

            //    dropdownData.lstHostName.ForEach(h =>
            //    {
            //        var host = new SelectListItem();
            //        host.Value = h.Value;
            //        viewbagHostName.Add(host);
            //    });

            //    model.lstHostName = dropdownData.lstHostName;
            //    model.lstProgramName = dropdownData.lstProgramName;
            //    model.lstKeys = dropdownData.lstKeys;
            //    ViewBag.HostNameList = viewbagHostName;
            //}

            return View(model);
        }

        public ActionResult GetHostName()
        {
            string url = string.Format("{0}T1Service/GetCommonDropdown", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);

            var response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", null);
            var dropdownData = new T1ServiceModel();
            if (response.RawResponse != null)
            {
                dropdownData = JsonConvert.DeserializeObject<T1ServiceModel>(response.RawResponse);

                var viewbagHostName = new List<SelectListItem>();

                dropdownData.lstHostName.ForEach(h =>
                {
                    var host = new SelectListItem();
                    host.Value = h.Value;
                    viewbagHostName.Add(host);
                });
                 
                ViewBag.HostNameList = viewbagHostName;
            }

            return Json(dropdownData.lstHostName);
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


            Session.Add("filterCriteria", filterCriteria);

            var response = new WebHttpResponse();
            var baseModel = new BaseGridModel();
            baseModel.search = filterCriteria;
            baseModel.pagenumber = g.Page;
            baseModel.pagesize = g.PageSize;

            string requestData = JsonConvert.SerializeObject(baseModel);
            string url = string.Format("{0}T1Service/GetServiceControllerData_Count", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);
            try
            {
                //This API call is getting total count
                response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", requestData);

                if (response.RawResponse == null)
                {
                    log.Error($"Unable to get data for {url}:{response.StatusCode}:{response.ErrorMessage}");
                    return Json(new GridModelBuilder<T1ServiceModel>(null, g)
                    {
                        KeyProp = o => o.Id
                    }.Build());
                }

                var totalCount = Convert.ToInt32(response.RawResponse);

                //This API call will retrieve all records based on search
                url = string.Format("{0}T1Service/ServiceDataGetAll_New", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);

                response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", requestData);

                if (response.RawResponse != null)
                {
                    var responseData = JsonConvert.DeserializeObject<List<T1ServiceModel>>(response.RawResponse).ToList().AsQueryable();
                    int PageCount = (totalCount / g.PageSize);

                    if (totalCount > g.PageSize)
                        PageCount = PageCount + 1;

                    return Json(new GridModelBuilder<T1ServiceModel>(responseData, g)
                    {
                        KeyProp = o => o.Id,
                        PageCount = PageCount                        
                    }.Build());
                }
                else
                {
                    log.Error($"Unable to get data for {url}:{response.StatusCode}:{response.ErrorMessage}");
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
                        log.Error($"Unable to post data for {url}:{response.StatusCode}:{response.ErrorMessage}");
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
        public JsonResult BulkUpdate(string txtKeyValue)
        {
            string returnMessage = string.Empty;
            if (string.IsNullOrEmpty(Convert.ToString(Session["filterCriteria"])))
            {
                returnMessage = $"Please apply filter first.";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(txtKeyValue))
            {
                returnMessage = $"Please apply Value.";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(Session["filterCriteria"])))
            {
                string filterCriteria = (string)Session["filterCriteria"];

                var bulkUpdateModel = new BulkUpdateModel();
                bulkUpdateModel.Search = filterCriteria;
                bulkUpdateModel.Value = txtKeyValue;
                string url = string.Format("{0}T1Service/SearchAndBulkUpdateServiceControllerData", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);

                try
                {
                    string data = JsonConvert.SerializeObject(bulkUpdateModel);


                    var response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", data);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        returnMessage = "success";
                    }
                    else
                    {
                        returnMessage = $"Error occured while bulk update:{response.StatusCode}";
                        log.Error($"Error occured while bulk update for url:{response.StatusCode}:{response.RawResponse}");
                    }
                }
                catch (Exception ex)
                {
                    returnMessage = ex.Message.ToString();
                    log.Error($"Error to call API {url}:{ex.ToString()}");
                }
            }

            return Json(returnMessage, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DropdownSearch(GridParams g, string HostName, string ProgramName = null, string Key = null)
        {
            string returnMessage = string.Empty;

            string filterCriteria = string.Empty;

            if (!string.IsNullOrEmpty(HostName))
            {
                filterCriteria = " HostName = '" + HostName + "' ";
            }

            if (!string.IsNullOrEmpty(ProgramName))
            {
                if (string.IsNullOrEmpty(filterCriteria))
                    filterCriteria = " ProgramName = '" + ProgramName + "' ";
                else
                    filterCriteria = filterCriteria + " AND ProgramName = '" + ProgramName + "' ";
            }

            if (!string.IsNullOrEmpty(Key))
            {
                if (string.IsNullOrEmpty(filterCriteria))
                    filterCriteria = " [Key] = '" + Key + "' ";
                else
                    filterCriteria = filterCriteria + " AND [Key] = '" + Key + "' ";
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
                //This API call is getting total count
                response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", requestData);

                if (response.RawResponse == null)
                {
                    log.Error($"Unable to get data for {url}:{response.StatusCode}:{response.ErrorMessage}");
                    return Json(new GridModelBuilder<T1ServiceModel>(null, g)
                    {
                        KeyProp = o => o.Id
                    }.Build());
                }

                var totalCount = Convert.ToInt32(response.RawResponse);

                //This API call will retrieve all records based on search
                url = string.Format("{0}T1Service/ServiceDataGetAll_New", ConfigurationManager.AppSettings["dronacontrolsiteapiurl"]);

                response = HttpHelper.SendHTTPRequest(url, "POST", @"application/json; charset=utf-8", requestData);

                if (response.RawResponse != null)
                {
                    var responseData = JsonConvert.DeserializeObject<List<T1ServiceModel>>(response.RawResponse).ToList().AsQueryable();
                    int PageCount = (totalCount / g.PageSize);

                    if (totalCount > g.PageSize)
                        PageCount = PageCount + 1;

                    return Json(new GridModelBuilder<T1ServiceModel>(responseData, g)
                    {
                        KeyProp = o => o.Id,
                        PageCount = PageCount
                    }.Build());
                }
                else
                {
                    log.Error($"Unable to get data for {url}:{response.StatusCode}:{response.ErrorMessage}");
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