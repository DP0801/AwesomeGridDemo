using System.Web;
using AwesomeMvcDemo.Models;
using AwesomeMvcDemo.ViewModels.Input.Settings;
using Omu.Awem.Utils;

namespace AwesomeMvcDemo.Utils
{
    public class SiteSettings
    {
        public static SettingsVal Read(HttpRequest request)
        {
            var settings = new SettingsVal();

            if (request.Cookies[DemoSettings.CookieName] != null)
            {
                var val = request.Cookies[DemoSettings.CookieName].Value;
                settings.Theme = val;
            }

            if (string.IsNullOrWhiteSpace(settings.Theme))
            {
                if (MobileUtils.IsMobileOrTablet())
                {
                    settings.Theme = DemoSettings.MobileTheme;
                }
                else
                {
                    settings.Theme = DemoSettings.DefaultTheme;
                }
            }

            return settings;
        }
    }
}