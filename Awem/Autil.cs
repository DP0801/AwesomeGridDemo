using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Helpers;
using System.Web.Mvc;
using Omu.Awem.Utils;
using Omu.AwesomeMvc;

namespace Omu.Awem
{
    internal static class Autil
    {
        internal static string Serialize(object input)
        {
            return Json.Encode(input);
        }

        internal static CultureInfo CurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture;
        }

        internal static bool IsMobileOrTablet<T>(AwesomeHtmlHelper<T> ahtml)
        {
            return MobileUtils.IsMobileOrTablet();
        }

        internal static IDictionary<string, IDictionary<string, string>> GetVldAttrs<TModel>(HtmlHelper html)
        {
            var res = new Dictionary<string, IDictionary<string, string>>();
            
            foreach (var prop in typeof(TModel).GetProperties())
            {
                var clientAttributes = new Dictionary<string, object>();
                Func<string, ModelMetadata, IEnumerable<ModelClientValidationRule>> ClientValidationRuleFactory 
                    = (name, metadata) => 
                        ModelValidatorProviders
                            .Providers
                            .GetValidators(metadata ?? ModelMetadata.FromStringExpression(name, html.ViewData), html.ViewContext)
                            .SelectMany(v => v.GetClientValidationRules());

                var clientRules = ClientValidationRuleFactory(prop.Name, null);
                UnobtrusiveValidationAttributesGenerator.GetValidationAttributes(clientRules, clientAttributes);

                if (clientAttributes.Count > 0)
                {
                    res.Add(prop.Name, clientAttributes.ToDictionary(kv => kv.Key, kv => kv.Value != null ? kv.Value.ToString() : string.Empty));
                }
            }

            return res;
        }
    }
}