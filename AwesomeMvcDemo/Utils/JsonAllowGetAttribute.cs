using System.Web.Mvc;

namespace AwesomeMvcDemo.Utils
{
    public class JsonAllowGetAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var jsonResult = filterContext.Result as JsonResult;

            if (jsonResult != null)
            {
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }

            base.OnResultExecuting(filterContext);
        }
    }
}