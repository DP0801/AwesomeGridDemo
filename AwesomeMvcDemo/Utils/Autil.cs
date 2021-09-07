using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AwesomeMvcDemo.Utils
{
    public static class Autil
    {
        public static string ServerMapPath(this HtmlHelper html)
        {
            return html.ViewContext.HttpContext.Server.MapPath(@"~\");
        }

        public static string CssDir()
        {
            return "~/Content/";
        }

        public static string JsDir()
        {
            return "~/Scripts/";
        }

        public static string JsonEncode(object o)
        {
            return new JavaScriptSerializer().Serialize(o);
        }

        public static bool IsDev()
        {
            var compilationSection = (CompilationSection)System.Configuration.ConfigurationManager.GetSection(@"system.web/compilation");
            return compilationSection.Debug;
        }
    }
}