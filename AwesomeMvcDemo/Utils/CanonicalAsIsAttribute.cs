using System.Web.Mvc;

namespace AwesomeMvcDemo.Utils
{
    /// <summary>
    /// permanent redirect from /griddemo/groUPing to /GridDemo/Grouping
    /// intended for this solution, may not work if using custom routing, action name attribute etc.
    /// </summary>
    public class CanonicalAsIsAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            if (request.Url == null || request.HttpMethod == "POST") return;

            var controller = (Controller)filterContext.Controller;

            var pcontr = controller.GetType().Name.Replace("Controller", string.Empty);
            var pact = ((ReflectedActionDescriptor)filterContext.ActionDescriptor).MethodInfo.Name;

            if (filterContext.RouteData.Values["controller"].ToString() != pcontr || filterContext.ActionDescriptor.ActionName != pact)
            {
                var urla = controller.Url.Action(pact, pcontr);
                if (request.QueryString.Count > 0)
                {
                    urla = urla + "?" + request.QueryString;
                }

                filterContext.Result = new RedirectResult(urla, true);
            }
        }
    }
}