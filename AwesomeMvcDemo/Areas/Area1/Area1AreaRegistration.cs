using System.Web.Mvc;

namespace AwesomeMvcDemo.Areas.Area1
{
    public class Area1AreaRegistration : AreaRegistration
    {
        public override string AreaName => "Area1";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Area1_default",
                "Area1/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}