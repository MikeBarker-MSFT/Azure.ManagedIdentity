using System.Web;
using System.Web.Mvc;

namespace Azure.Identity.KeyVaultMSI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
