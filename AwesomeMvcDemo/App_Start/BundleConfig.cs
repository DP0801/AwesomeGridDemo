using System.Web.Optimization;

namespace AwesomeMvcDemo.App_Start
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundle/Scripts/js").Include(
                "~/Scripts/AwesomeMvc.js",
                "~/Scripts/awem.js",
                "~/Scripts/utils.js",
                "~/Scripts/sidemenu.js",
                "~/Scripts/signalrSync.js",
                "~/Scripts/aweui.js",
                "~/Scripts/awedict.js",
                "~/Scripts/Site.js",
                "~/Scripts/cstorg.js", // cache storage
                "~/Scripts/ovld.js" // client validation used for grid inline/batch editing
            ));
        }
    }
}