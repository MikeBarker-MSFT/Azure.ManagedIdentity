using System.Configuration;
using System.Web.Mvc;

namespace Azure.Identity.KeyVaultSourceAppSetting.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Secret()
        {
            ViewBag.Message = ConfigurationManager.AppSettings["KeyVaultSecret"];

            return View();
        }
    }
}