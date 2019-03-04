using System.Web;
using System.Web.Mvc;

namespace Azure.Identity.KeyVaultSourceAppSetting
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
